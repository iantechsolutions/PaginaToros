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

internal sealed record SentMail(
    string? From,
    IReadOnlyList<string> To,
    string Subject,
    string Body,
    string? TextBody,
    IReadOnlyList<string> LinkedResourceIds)
{
    public static SentMail Capture(MailMessage message)
    {
        var body = message.Body;
        string? textBody = null;
        var linkedResourceIds = new List<string>();

        if (message.AlternateViews.Count > 0)
        {
            // Body = la vista HTML si existe (es la que ve el destinatario), si no la primera.
            var htmlView = message.AlternateViews.FirstOrDefault(v => v.ContentType.MediaType == "text/html")
                ?? message.AlternateViews[0];
            body = ReadView(htmlView);
            linkedResourceIds.AddRange(htmlView.LinkedResources.Select(r => r.ContentId));

            var textView = message.AlternateViews.FirstOrDefault(v => v.ContentType.MediaType == "text/plain");
            if (textView != null)
            {
                textBody = ReadView(textView);
            }
        }

        return new SentMail(
            message.From?.Address,
            message.To.Select(x => x.Address).ToList(),
            message.Subject,
            body,
            textBody,
            linkedResourceIds);
    }

    private static string ReadView(AlternateView view)
    {
        var stream = view.ContentStream;
        stream.Position = 0;
        return new StreamReader(stream).ReadToEnd();
    }
}
