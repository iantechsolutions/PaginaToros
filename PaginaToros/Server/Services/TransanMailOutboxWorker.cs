using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Shared.Models;

namespace PaginaToros.Server.Services
{
    public sealed class TransanMailOutboxWorker : BackgroundService
    {
        private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(10);
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMailSender _mailSender;
        private readonly ILogger<TransanMailOutboxWorker> _logger;
        private bool _notConfiguredLogged;

        public TransanMailOutboxWorker(IServiceScopeFactory scopeFactory, IMailSender mailSender, ILogger<TransanMailOutboxWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _mailSender = mailSender;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessPendingAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error procesando la cola de mails de transferencias.");
                }

                try
                {
                    await Task.Delay(PollInterval, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        private async Task ProcessPendingAsync(CancellationToken cancellationToken)
        {
            // Sin configuración de mail no se toca la cola: si se tomaran las filas,
            // cada una pasaría a "Error" y acumularía reintentos sin poder salir.
            // Quedan "Pendiente" y se envían apenas se configure el mail.
            if (!_mailSender.IsConfigured)
            {
                if (!_notConfiguredLogged)
                {
                    _logger.LogWarning("TransanMailOutboxWorker en pausa: el envío de mails no está configurado. Los mails de transferencias quedan pendientes.");
                    _notConfiguredLogged = true;
                }
                return;
            }
            _notConfiguredLogged = false;

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<hereford_prContext>();

            var now = DateTime.UtcNow;
            var pending = await db.TransanMailOutboxes
                .Where(x => (x.Estado == "Pendiente" || x.Estado == "Error") &&
                            (!x.ProximoIntento.HasValue || x.ProximoIntento <= now))
                .OrderBy(x => x.Id)
                .Take(10)
                .ToListAsync(cancellationToken);

            if (pending.Count > 0)
            {
                _logger.LogInformation("TransanMailOutboxWorker encontró {Count} mails pendientes para procesar.", pending.Count);
            }

            foreach (var item in pending)
            {
                await ProcessOneAsync(db, item, cancellationToken);
            }
        }

        private async Task ProcessOneAsync(hereford_prContext db, TransanMailOutbox item, CancellationToken cancellationToken)
        {
            item.Estado = "Procesando";
            item.UltimoIntento = DateTime.UtcNow;
            await db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("TransanMailOutboxWorker procesando outbox. OutboxId={OutboxId} TransanId={TransanId} Intentos={Intentos}",
                item.Id, item.TransanId, item.Intentos);

            try
            {
                using var mail = new MailMessage
                {
                    Subject = item.Asunto,
                    Body = item.CuerpoHtml,
                    IsBodyHtml = true
                };

                foreach (var recipient in SplitRecipients(item.Destinatarios))
                {
                    mail.To.Add(recipient);
                }

                if (mail.To.Count == 0)
                {
                    throw new InvalidOperationException("La cola de mail no tiene destinatarios válidos.");
                }

                await _mailSender.SendAsync(mail);

                item.Estado = "Enviado";
                item.FechaEnvio = DateTime.UtcNow;
                item.UltimoError = null;
                item.ProximoIntento = null;
                await db.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("TransanMailOutboxWorker mail enviado. OutboxId={OutboxId} TransanId={TransanId}", item.Id, item.TransanId);
            }
            catch (Exception ex)
            {
                item.Estado = "Error";
                item.Intentos += 1;
                item.UltimoError = ex.Message;
                item.ProximoIntento = DateTime.UtcNow.AddMinutes(Math.Min(30, Math.Max(1, item.Intentos * 2)));
                await db.SaveChangesAsync(cancellationToken);

                _logger.LogWarning(ex, "No se pudo enviar el mail de la transferencia {OutboxId}. Se reintentará luego.", item.Id);
            }
        }

        private static IEnumerable<string> SplitRecipients(string? recipients)
        {
            if (string.IsNullOrWhiteSpace(recipients))
            {
                yield break;
            }

            var separators = new[] { ';', ',', '\n', '\r' };
            foreach (var raw in recipients.Split(separators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (IsValidEmail(raw))
                {
                    yield return raw;
                }
            }
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                _ = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
