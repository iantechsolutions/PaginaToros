using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using PaginaToros.Server.Services;
using PaginaToros.Shared.Models;

namespace PaginaToros.Tests;

public class MailSenderTests
{
    private const string TestUserName = "cuenta@test.local";
    private const string TestPassword = "secreto-de-prueba";

    [Fact]
    public async Task SendAsync_AuthenticatesWithConfiguredAccountAndSendsFromConfiguredAddress()
    {
        using var server = new FakeSmtpServer();
        var sender = CreateSender(OptionsFor(server));

        using var mail = NewMail();
        await sender.SendAsync(mail);

        var session = Assert.Single(server.Sessions);
        Assert.Equal(TestUserName, session.UserName);
        Assert.Equal(TestPassword, session.Password);
        Assert.Equal("remitente@test.local", session.MailFrom);
        Assert.Equal(new[] { "destino@test.local" }, session.Recipients);
        Assert.Contains("Subject: Prueba de envio", session.Data);
    }

    [Fact]
    public async Task SendAsync_WithoutFrom_SendsFromTheAccount()
    {
        using var server = new FakeSmtpServer();
        var options = OptionsFor(server);
        options.From = null;
        var sender = CreateSender(options);

        using var mail = NewMail();
        await sender.SendAsync(mail);

        Assert.Equal(TestUserName, Assert.Single(server.Sessions).MailFrom);
    }

    [Fact]
    public async Task SendAsync_WithoutPassword_FailsWithClearMessageAndDoesNotConnect()
    {
        using var server = new FakeSmtpServer();
        var options = OptionsFor(server);
        options.Password = null;
        var sender = CreateSender(options);

        Assert.False(sender.IsConfigured);
        using var mail = NewMail();
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => sender.SendAsync(mail));

        Assert.Contains("Mail:Password", ex.Message);
        Assert.Empty(server.Sessions);
    }

    [Fact]
    public async Task SendAsync_WhenServerRejectsCredentials_PointsToMailSettings()
    {
        // SmtpClient no informa el 535 del login: sigue sin autenticar y el servidor
        // recién lo rechaza en MAIL FROM con "SMTP AUTH is required".
        using var server = new FakeSmtpServer(rejectCredentials: true);
        var sender = CreateSender(OptionsFor(server));

        using var mail = NewMail();
        var ex = await Assert.ThrowsAsync<SmtpException>(() => sender.SendAsync(mail));

        Assert.Contains("Mail:Password", ex.Message);
        Assert.Contains("SMTP AUTH is required", ex.Message);
        Assert.Equal(TestPassword, Assert.Single(server.Sessions).Password);
    }

    [Fact]
    public void MailPassword_CanComeFromEnvironmentVariable()
    {
        const string prefix = "PAGINATOROS_TESTS_";
        Environment.SetEnvironmentVariable(prefix + "Mail__Password", "desde-variable-de-entorno");
        try
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["Mail:Host"] = "smtp.test.local",
                    ["Mail:UserName"] = TestUserName,
                    ["Mail:PlantelesAddress"] = RecordingMailSender.TestPlantelesAddress
                })
                .AddEnvironmentVariables(prefix)
                .Build();

            var options = configuration.GetSection(MailOptions.SectionName).Get<MailOptions>();

            Assert.Equal("desde-variable-de-entorno", options.Password);
            Assert.Empty(options.GetMissingSettings());
        }
        finally
        {
            Environment.SetEnvironmentVariable(prefix + "Mail__Password", null);
        }
    }

    [Theory]
    [InlineData("appsettings.json")]
    [InlineData("appsettingspublish.json")]
    public void VersionedAppSettings_ConfigureTheAccountButNotItsPassword(string fileName)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(FindServerFile(fileName))
            .Build();

        var options = configuration.GetSection(MailOptions.SectionName).Get<MailOptions>();

        Assert.NotNull(options);
        Assert.Equal(new[] { "Mail:Password" }, options.GetMissingSettings());
    }

    [Fact]
    public async Task AccessMail_GoesThroughSharedSenderWithConfiguredContactAddress()
    {
        var mailSender = new RecordingMailSender();
        var service = new AccessMailService(mailSender);
        // Con una casilla de Outlook el mail va sin imágenes embebidas, así que no
        // depende de los archivos de wwwroot.
        var user = new User { Email = "socio@outlook.test", Names = "Ana", LastNames = "Perez" };

        var result = await service.SendAccessMailAsync(user, "clave-temporal", AccessMailTemplate.Reset);

        Assert.True(result.Success, result.ErrorMessage);
        var sent = Assert.Single(mailSender.Sent);
        Assert.Null(sent.From);
        Assert.Equal(new[] { "socio@outlook.test" }, sent.To);
        Assert.Contains($"mailto:{RecordingMailSender.TestPlantelesAddress}", sent.Body);
        Assert.DoesNotContain("hereford.org.ar", sent.Body);
    }

    private static MailOptions OptionsFor(FakeSmtpServer server) => new()
    {
        Host = "127.0.0.1",
        Port = server.Port,
        EnableSsl = false,
        UserName = TestUserName,
        Password = TestPassword,
        From = "remitente@test.local",
        PlantelesAddress = RecordingMailSender.TestPlantelesAddress
    };

    private static SmtpMailSender CreateSender(MailOptions options) =>
        new(new FixedOptionsMonitor(options), NullLogger<SmtpMailSender>.Instance);

    private static MailMessage NewMail()
    {
        var mail = new MailMessage { Subject = "Prueba de envio", Body = "Hola" };
        mail.To.Add("destino@test.local");
        return mail;
    }

    private static string FindServerFile(string fileName)
    {
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir != null; dir = dir.Parent)
        {
            var candidate = Path.Combine(dir.FullName, "Server", fileName);
            if (File.Exists(candidate))
            {
                return candidate;
            }
        }

        throw new FileNotFoundException($"No se encontró Server/{fileName} subiendo desde {AppContext.BaseDirectory}.");
    }

    private sealed class FixedOptionsMonitor : IOptionsMonitor<MailOptions>
    {
        public FixedOptionsMonitor(MailOptions value) => CurrentValue = value;

        public MailOptions CurrentValue { get; }

        public MailOptions Get(string name) => CurrentValue;

        public IDisposable OnChange(Action<MailOptions, string> listener) => NullDisposable.Instance;

        private sealed class NullDisposable : IDisposable
        {
            public static readonly NullDisposable Instance = new();
            public void Dispose() { }
        }
    }
}
