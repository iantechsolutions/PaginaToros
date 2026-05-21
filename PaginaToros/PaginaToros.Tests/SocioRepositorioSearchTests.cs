using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PaginaToros.Server.Context;
using PaginaToros.Server.Repositorio.Implementacion;
using PaginaToros.Shared.Models;

namespace PaginaToros.Tests;

public class SocioRepositorioSearchTests
{
    [Theory]
    [InlineData("juan", 1)]
    [InlineData("perez", 1)]
    [InlineData("lopez", 1)]
    [InlineData("norte@correo.com", 1)]
    [InlineData("razon", 2)]
    public async Task SearchPagedAsync_FindsSocios_BySocioAndAssociatedUsers(string searchText, int expectedId)
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new SocioRepositorio(scope.Context);
        var result = await repo.SearchPagedAsync(0, 10, searchText);

        Assert.Contains(result.Items, socio => socio.Id == expectedId);
    }

    [Fact]
    public async Task SearchPagedAsync_AppliesPaging_AndReturnsTotalCount()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new SocioRepositorio(scope.Context);
        var result = await repo.SearchPagedAsync(0, 1, null);

        Assert.Equal(2, result.TotalCount);
        Assert.Single(result.Items);
    }

    [Fact]
    public async Task SearchPagedAsync_ReturnsEmptyWhenNoMatch()
    {
        using var scope = CreateContext();
        Seed(scope.Context);

        var repo = new SocioRepositorio(scope.Context);
        var result = await repo.SearchPagedAsync(0, 10, "no-existe");

        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }

    private static TestContextScope CreateContext()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<hereford_prContext>()
            .UseSqlite(connection)
            .Options;

        var context = new hereford_prContext(options);
        context.Database.ExecuteSqlRaw(@"
            CREATE TABLE PROVIN (
                id INTEGER PRIMARY KEY,
                COD_USU INTEGER NULL,
                FCH_USU TEXT NULL,
                NOMBRE TEXT NULL,
                PCOD TEXT NOT NULL
            );");

        context.Database.ExecuteSqlRaw(@"
            CREATE TABLE SOCIOS (
                id INTEGER PRIMARY KEY,
                SCOD TEXT NOT NULL,
                CATEGO TEXT NULL,
                CUENTA TEXT NULL,
                PRENOM TEXT NULL,
                NOMBRE TEXT NULL,
                POSNOM TEXT NULL,
                DIRECC1 TEXT NULL,
                LOCALI1 TEXT NULL,
                CODPOS1 TEXT NULL,
                CODPRO1 TEXT NULL,
                ORDALF TEXT NULL,
                TELEFO1 TEXT NULL,
                mail TEXT NULL,
                CRIADOR TEXT NULL,
                DIRECC2 TEXT NULL,
                LOCALI2 TEXT NULL,
                CODPOS2 TEXT NULL,
                CODPRO2 TEXT NULL,
                TELEFO2 TEXT NULL,
                FECING TEXT NULL,
                VTOSUS TEXT NULL,
                ENVIO TEXT NULL,
                FCH_USU TEXT NULL,
                COD_USU INTEGER NULL,
                PLACOD TEXT NULL,
                mailreg TEXT NULL,
                diaregautog TEXT NULL
            );");

        context.Database.ExecuteSqlRaw(@"
            CREATE TABLE User (
                Id INTEGER PRIMARY KEY,
                Names TEXT NOT NULL,
                LastNames TEXT NOT NULL,
                Dni TEXT NOT NULL,
                Phone TEXT NOT NULL,
                Email TEXT NOT NULL,
                Rol TEXT NOT NULL,
                Status TEXT NOT NULL,
                Created TEXT NOT NULL,
                IdentityUserId TEXT NULL,
                SocioId INTEGER NULL,
                FOREIGN KEY (SocioId) REFERENCES SOCIOS(id)
            );");

        return new TestContextScope(connection, context);
    }

    private static void Seed(hereford_prContext context)
    {
        var socioUno = new Socio
        {
            Id = 1,
            Scod = "0001",
            Codpos2 = "1001",
            Prenom = "Juan",
            Nombre = "Perez",
            Posnom = "Lopez",
            Mail = "norte@correo.com",
            Criador = "S"
        };

        var socioDos = new Socio
        {
            Id = 2,
            Scod = "0002",
            Codpos2 = "1002",
            Prenom = "Razon",
            Nombre = "Social",
            Posnom = "Dos",
            Criador = "S"
        };

        context.Socios.AddRange(socioUno, socioDos);
        context.User.AddRange(
            new User
            {
                Id = 1,
                Names = "Juan",
                LastNames = "Lopez",
                Dni = "1",
                Phone = "1",
                Email = "juan.lopez@example.com",
                Rol = "SOCIO",
                Status = "ACTIVO",
                Created = DateTime.UtcNow,
                SocioId = socioUno.Id
            },
            new User
            {
                Id = 2,
                Names = "Maria",
                LastNames = "Perez",
                Dni = "2",
                Phone = "2",
                Email = "maria.perez@example.com",
                Rol = "SOCIO",
                Status = "ACTIVO",
                Created = DateTime.UtcNow,
                SocioId = socioDos.Id
            });

        context.SaveChanges();
        context.ChangeTracker.Clear();
    }

    private sealed class TestContextScope : IDisposable
    {
        private readonly SqliteConnection _connection;

        public TestContextScope(SqliteConnection connection, hereford_prContext context)
        {
            _connection = connection;
            Context = context;
        }

        public hereford_prContext Context { get; }

        public void Dispose()
        {
            Context.Dispose();
            _connection.Dispose();
        }
    }
}
