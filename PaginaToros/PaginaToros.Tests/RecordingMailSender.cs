using System.Net.Mail;
using PaginaToros.Server.Services;

namespace PaginaToros.Tests;

/// <summary>IMailSender de prueba: no abre conexiones, solo registra lo que se envió.</summary>
internal sealed class RecordingMailSender : IMailSender
{
    public const string TestPlantelesAddress = "planteles@test.local";

    public List<SentMail> Sent { get; } = new();

    public bool IsConfigured => true;

    public string PlantelesAddress => TestPlantelesAddress;

    public Task SendAsync(MailMessage message, CancellationToken cancellationToken = default)
    {
        Sent.Add(SentMail.Capture(message));
        return Task.CompletedTask;
    }
}

internal sealed record SentMail(string? From, IReadOnlyList<string> To, string Subject, string Body)
{
    public static SentMail Capture(MailMessage message)
    {
        var body = message.Body;
        if (message.AlternateViews.Count > 0)
        {
            var stream = message.AlternateViews[0].ContentStream;
            stream.Position = 0;
            body = new StreamReader(stream).ReadToEnd();
        }

        return new SentMail(
            message.From?.Address,
            message.To.Select(x => x.Address).ToList(),
            message.Subject,
            body);
    }
}
