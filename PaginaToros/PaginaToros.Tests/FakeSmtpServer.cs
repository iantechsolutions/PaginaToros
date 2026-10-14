using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PaginaToros.Tests;

/// <summary>
/// Servidor SMTP mínimo en loopback para probar el envío real sin salir de la máquina.
/// Anuncia AUTH PLAIN LOGIN como mail.hereford.org.ar y, como su Exim, rechaza el
/// MAIL FROM de una sesión que no se autenticó.
/// </summary>
internal sealed class FakeSmtpServer : IDisposable
{
    private readonly TcpListener _listener;
    private readonly bool _rejectCredentials;
    private readonly ConcurrentQueue<FakeSmtpSession> _sessions = new();

    public FakeSmtpServer(bool rejectCredentials = false)
    {
        _rejectCredentials = rejectCredentials;
        _listener = new TcpListener(IPAddress.Loopback, 0);
        _listener.Start();
        Port = ((IPEndPoint)_listener.LocalEndpoint).Port;
        _ = AcceptLoopAsync();
    }

    public int Port { get; }

    public IReadOnlyList<FakeSmtpSession> Sessions => _sessions.ToArray();

    public void Dispose() => _listener.Stop();

    private async Task AcceptLoopAsync()
    {
        while (true)
        {
            TcpClient client;
            try
            {
                client = await _listener.AcceptTcpClientAsync();
            }
            catch (Exception ex) when (ex is SocketException or ObjectDisposedException)
            {
                return;
            }

            _ = HandleAsync(client);
        }
    }

    private async Task HandleAsync(TcpClient client)
    {
        var session = new FakeSmtpSession();
        _sessions.Enqueue(session);

        using (client)
        {
            var stream = client.GetStream();
            var reader = new StreamReader(stream, Encoding.ASCII);
            var writer = new StreamWriter(stream, Encoding.ASCII) { NewLine = "\r\n", AutoFlush = true };

            try
            {
                await writer.WriteLineAsync("220 fake-smtp ESMTP");

                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    switch (parts.Length > 0 ? parts[0].ToUpperInvariant() : string.Empty)
                    {
                        case "EHLO":
                            await writer.WriteLineAsync("250-fake-smtp");
                            await writer.WriteLineAsync("250 AUTH PLAIN LOGIN");
                            break;
                        case "AUTH":
                            await AuthenticateAsync(parts, reader, writer, session);
                            break;
                        case "MAIL":
                            if (!session.Authenticated)
                            {
                                await writer.WriteLineAsync("550 SMTP AUTH is required for message submission on port 587");
                                break;
                            }
                            session.MailFrom = ExtractAddress(line);
                            await writer.WriteLineAsync("250 OK");
                            break;
                        case "RCPT":
                            session.Recipients.Add(ExtractAddress(line));
                            await writer.WriteLineAsync("250 Accepted");
                            break;
                        case "DATA":
                            await writer.WriteLineAsync("354 Enter message, ending with \".\" on a line by itself");
                            var data = new StringBuilder();
                            while ((line = await reader.ReadLineAsync()) != null && line != ".")
                            {
                                data.AppendLine(line.StartsWith("..") ? line[1..] : line);
                            }
                            session.Data = data.ToString();
                            await writer.WriteLineAsync("250 OK id=fake");
                            break;
                        case "QUIT":
                            await writer.WriteLineAsync("221 fake-smtp closing connection");
                            return;
                        default:
                            await writer.WriteLineAsync("250 OK");
                            break;
                    }
                }
            }
            catch (Exception ex) when (ex is IOException or ObjectDisposedException)
            {
                // El cliente cerró la conexión.
            }
        }
    }

    private async Task AuthenticateAsync(string[] parts, StreamReader reader, StreamWriter writer, FakeSmtpSession session)
    {
        if (parts.Length < 2 || !parts[1].Equals("LOGIN", StringComparison.OrdinalIgnoreCase))
        {
            await writer.WriteLineAsync("504 Unrecognized authentication type");
            return;
        }

        // System.Net.Mail manda el usuario junto con el comando ("AUTH login <base64>").
        if (parts.Length > 2)
        {
            session.UserName = Decode(parts[2]);
        }
        else
        {
            await writer.WriteLineAsync("334 VXNlcm5hbWU6");
            session.UserName = Decode(await reader.ReadLineAsync());
        }

        await writer.WriteLineAsync("334 UGFzc3dvcmQ6");
        session.Password = Decode(await reader.ReadLineAsync());

        if (_rejectCredentials)
        {
            await writer.WriteLineAsync("535 Incorrect authentication data");
            return;
        }

        session.Authenticated = true;
        await writer.WriteLineAsync("235 Authentication succeeded");
    }

    private static string? Decode(string? base64) =>
        base64 is null ? null : Encoding.UTF8.GetString(Convert.FromBase64String(base64.Trim()));

    private static string ExtractAddress(string line)
    {
        var start = line.IndexOf('<');
        var end = line.IndexOf('>', start + 1);
        return start >= 0 && end > start ? line[(start + 1)..end] : line;
    }
}

internal sealed class FakeSmtpSession
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public bool Authenticated { get; set; }
    public string? MailFrom { get; set; }
    public List<string> Recipients { get; } = new();
    public string? Data { get; set; }
}
