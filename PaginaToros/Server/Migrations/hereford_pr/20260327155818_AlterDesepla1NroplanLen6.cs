using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaginaToros.Server.Migrations.hereford_pr
{
    public partial class AlterDesepla1NroplanLen6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.CreateTable(
                name: "actualizacion",
                columns: table => new
                {
                    socios = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    torosp = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    torospr = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    establec = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    certsemen = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    centrosia = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, defaultValueSql: "''", collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    solicinspecc = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    archrural = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    decserv = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    resultsol = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    inspect = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    transfsb = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    transfanim = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    transfafut = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    zonas = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    planteles = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "archrural",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TAT = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    HBA = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    TIPO = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NOMBRE = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NAC = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_archrural", x => x.id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", maxLength: 6, nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "AUTORIZA",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NROSOL = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CANTOR = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    CANVPR = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    BASVAC = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    BASVAQ = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    PROCRE = table.Column<double>(type: "double", nullable: true),
                    ANIOPR = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    VACSER = table.Column<double>(type: "double", nullable: true),
                    ANIOSE = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    FREALI = table.Column<DateTime>(type: "datetime", nullable: true),
                    FAUTOR = table.Column<DateTime>(type: "datetime", nullable: true),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true),
                    nrosolanual = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, defaultValueSql: "'1'", collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUTORIZA", x => x.id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "CENTROSIA",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NROCEN = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NOMBRE = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NRO_C_SAYG = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CENTROSIA", x => x.id);
                    table.UniqueConstraint("AK_CENTROSIA_NROCEN", x => x.NROCEN);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "DESE_USADAS",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NRODEC = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CERTSEMEN = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    DOSIS_USADA = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DESE_USADAS", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "DESEPLA2",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TATPART = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    HARDB = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NRODEC = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    DESDE = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    HASTA = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    APODO = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DESEPLA2", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "DESEPLA3",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TATPART = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    HARDB = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CANTV = table.Column<double>(type: "double", nullable: true),
                    NRODEC = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    TIPO = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    SERVICIO = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    DESDE = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    HASTA = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    APODO = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DESEPLA3", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "DESEPLA4 - No Usada",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CTST = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CANTV = table.Column<double>(type: "double", nullable: true),
                    NRODEC = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DESEPLA4 - No Usada", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "DESEPLA5 - No Usada",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NRODEC = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    E_NOM = table.Column<string>(type: "varchar(70)", maxLength: 70, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DESEPLA5 - No Usada", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "FUT_CONTROL",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NRO_TRANS = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    FECTRANS = table.Column<DateTime>(type: "datetime", nullable: true),
                    SVEN = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CATEG_SV = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    VNOM = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    SCOM = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CATEG_SC = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CNOM = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    PLANTEL = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    EDAD_CRIAS = table.Column<int>(type: "int(11)", nullable: true),
                    CANT_HEM = table.Column<int>(type: "int(11)", nullable: true),
                    CANT_MACH = table.Column<int>(type: "int(11)", nullable: true),
                    PLANT_DEST = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    INCORP = table.Column<sbyte>(type: "tinyint(4)", nullable: true),
                    HEMSTA = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FUT_CONTROL", x => x.id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "opcdecserv",
                columns: table => new
                {
                    natmin = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    natdosis = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    natmax = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    natcorrmin = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    natcorrdosis = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    natcorrmax = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    iamin = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    iadosis = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    iamax = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ia1min = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ia1dosis = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ia1max = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ia2min = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ia2dosis = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ia2max = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ia3min = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ia3dosis = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ia3max = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    hedadmin = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    hedadmax = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    medadmin = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    medadmax = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    cotamin = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    cotamax = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    unidadmes = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "PEG",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NRODEC = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NROSOCIO = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    SOCIO = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    HBA = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    DESDE = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    HASTA = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CVIENTRES = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    SERVICIO = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    APODO = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PEG", x => x.id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "PROVIN",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PCOD = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NOMBRE = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROVIN", x => x.id);
                    table.UniqueConstraint("AK_PROVIN_PCOD", x => x.PCOD);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "REMANSOL",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NRODEC = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NROPLAN = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NROSOL = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    REMANTOROPR = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    REMANVQPR = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    REMANVQVIP = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    nrosolanual = table.Column<int>(type: "int(10)", nullable: false, defaultValueSql: "'1'"),
                    ANIO = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    DISPTOROPR = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    DISPVQPR = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    DISPVQVIP = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    AUTTOROPR = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    AUTVQPR = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    AUTVQVIP = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    PEDTOROPR = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    PEDVQPR = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    PEDVQVIP = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REMANSOL", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "RESIN2",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EA1 = table.Column<int>(type: "int(10)", nullable: false),
                    EA2 = table.Column<double>(type: "double", nullable: true),
                    EA3 = table.Column<double>(type: "double", nullable: true),
                    EA4 = table.Column<double>(type: "double", nullable: true),
                    EA5 = table.Column<double>(type: "double", nullable: true),
                    EA6 = table.Column<double>(type: "double", nullable: true),
                    NRORES = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RESIN2", x => x.id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "RESIN3",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RDVAC = table.Column<double>(type: "double", nullable: true),
                    RDVAQCS = table.Column<double>(type: "double", nullable: true),
                    RDVAQSS = table.Column<double>(type: "double", nullable: true),
                    RPVAC = table.Column<double>(type: "double", nullable: true),
                    RPVAQCS = table.Column<double>(type: "double", nullable: true),
                    RPVAQSS = table.Column<double>(type: "double", nullable: true),
                    CTOMOV = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    TIPMOV = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NRORES = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RESIN3", x => x.id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "RESIN4",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PEDAD = table.Column<double>(type: "double", nullable: true),
                    PPESO = table.Column<double>(type: "double", nullable: true),
                    MEDAD = table.Column<double>(type: "double", nullable: true),
                    MPESO = table.Column<double>(type: "double", nullable: true),
                    IEDAD = table.Column<double>(type: "double", nullable: true),
                    IPESO = table.Column<double>(type: "double", nullable: true),
                    PDL = table.Column<double>(type: "double", nullable: true),
                    P2D = table.Column<double>(type: "double", nullable: true),
                    P4D = table.Column<double>(type: "double", nullable: true),
                    SEXO = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NRORES = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RESIN4", x => x.id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "RESIN8",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NRORES = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NROPLA = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    MOTIVO_RECHAZO = table.Column<int>(type: "int(11)", nullable: true),
                    HEMBRAS = table.Column<int>(type: "int(11)", nullable: true),
                    MACHOS = table.Column<int>(type: "int(11)", nullable: true),
                    FCH_REALIZADA = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RESIN8", x => x.id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "RESIN9",
                columns: table => new
                {
                    MOTIVO_RECHAZO = table.Column<int>(type: "int(11)", nullable: false),
                    DESCRIPCION = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MOTIVO_RECHAZO);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "retenidas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NRODEC = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NROPLAN = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    DESDE = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    HASTA = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FECDECL = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FCHRECEPCION = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    TIPSE = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    SEMEN = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CANTV = table.Column<double>(type: "double", nullable: true),
                    CANTB = table.Column<double>(type: "double", nullable: true),
                    REMBA = table.Column<double>(type: "double", nullable: true),
                    REMPR = table.Column<double>(type: "double", nullable: true),
                    SEMPROP = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    CANTOR = table.Column<double>(type: "double", nullable: true),
                    REMMPR = table.Column<double>(type: "double", nullable: true),
                    CTRLU = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    CTRLM = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    COEF_AUTO_SN = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    COEF_AUTO_IA = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    COEF_AUTO_IAR = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    IA_SINCRO = table.Column<float>(type: "float", nullable: true, defaultValueSql: "'0'"),
                    PASTILLAS_SINCRO = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    FECRET = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NR_FOLIO = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    reten = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false, defaultValueSql: "'0'", collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_retenidas", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "SOLICI4",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NROSOL = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CODPLA = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOLICI4", x => x.id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "SOLICI6",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NROSOL = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NRODEC = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    IND_HEM_MAC = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOLICI6", x => x.id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "TOROSPR",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    APODO = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NOMBRE = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    EST_DOC = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    RES_INSP = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    SBCOD = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    TIP_TORO = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    TATPART = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NOM_DAD = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NR_INSC = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NR_TSAN = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NR_INSD = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FECHA = table.Column<DateTime>(type: "datetime", nullable: true),
                    HBA = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    VARIEDAD = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CRIADOR = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CATEGO = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    PLANTEL = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ESTCOD = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FCH_BAJA = table.Column<DateTime>(type: "datetime", nullable: true),
                    ACTIVO = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    MOTIVO_BAJ = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NACIDO = table.Column<DateTime>(type: "datetime", nullable: true),
                    ACTUALIZADO = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    CIRC_ESCROTAL = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CodEstado = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true, defaultValueSql: "'0'", collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Id_tipo = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    FECING = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOROSPR", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "TorosRural",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hba = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    tatpart = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    fec_nac = table.Column<DateTime>(type: "datetime", nullable: true),
                    procesado = table.Column<sbyte>(type: "tinyint(4)", nullable: true),
                    tip_toro = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    fch_usu = table.Column<DateTime>(type: "datetime", nullable: true),
                    cod_usu = table.Column<int>(type: "int(11)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TorosRural", x => x.id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "TOROSUNIestados",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(5)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CodEstado = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOROSUNIestados", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_unicode_ci");

            migrationBuilder.CreateTable(
                name: "TRANSAN",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NRO_CERT = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    FECVTA = table.Column<DateTime>(type: "datetime", nullable: true),
                    SVEN = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CATEG_SV = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    VNOM = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    SCOM = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CATEG_SC = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CNOM = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    PLANT = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NVO_PLA = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CANT_HEM = table.Column<int>(type: "int(11)", nullable: true),
                    CANT_MACH = table.Column<int>(type: "int(11)", nullable: true),
                    TIPHAC = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    HEMSTA = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    TIPANI = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    INCORP = table.Column<sbyte>(type: "tinyint(4)", nullable: true),
                    TIPOHEM = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CANT_CHEM = table.Column<int>(type: "int(11)", nullable: true),
                    CANT_CMACH = table.Column<int>(type: "int(11)", nullable: true),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSAN", x => x.id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "TRANSEM",
                columns: table => new
                {
                    NRO_CERT = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NROCEN = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    FECVTA = table.Column<DateTime>(type: "datetime", nullable: true),
                    NVEN = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NROCRI = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CATEG_SC = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    SCOD = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    TATPART = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    HBA = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NOM_DAD = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NR_INSC = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NR_TSAN = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NR_INSD = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NR_DOSI = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NR_DOSI_OR = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    TIP_ENV = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    VARIEDAD = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true)
                },
                constraints: table =>
                {
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    pass = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    passmd5 = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    nrousu = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ultimavis = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ultimahora = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "AspNetRoleClaims_ibfk_1",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ProviderKey = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    UserId = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.LoginProvider, x.ProviderKey })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "AspNetUserLogins_ibfk_1",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "AspNetUserRole",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(450)", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    RoleId = table.Column<string>(type: "varchar(450)", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRole_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRole_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    RoleId = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.UserId, x.RoleId })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "AspNetUserRoles_ibfk_1",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "AspNetUserRoles_ibfk_2",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    LoginProvider = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    Name = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    Value = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.UserId, x.LoginProvider, x.Name })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });
                    table.ForeignKey(
                        name: "AspNetUserTokens_ibfk_1",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "INSPECT",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ICOD = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NOMBRE = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    DIRECC = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    LOCALI = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CODPOS = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CODPRO = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    TELEFO = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    mail = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INSPECT", x => x.id);
                    table.UniqueConstraint("AK_INSPECT_ICOD", x => x.ICOD);
                    table.ForeignKey(
                        name: "FK_INSPECT_PROVIN_CODPRO",
                        column: x => x.CODPRO,
                        principalTable: "PROVIN",
                        principalColumn: "PCOD");
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "SOCIOS",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SCOD = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false, defaultValueSql: "''", collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CATEGO = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CUENTA = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    PRENOM = table.Column<string>(type: "varchar(33)", maxLength: 33, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NOMBRE = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    POSNOM = table.Column<string>(type: "varchar(33)", maxLength: 33, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    DIRECC1 = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    LOCALI1 = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CODPOS1 = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CODPRO1 = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ORDALF = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    TELEFO1 = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    mail = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CRIADOR = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    DIRECC2 = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    LOCALI2 = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CODPOS2 = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CODPRO2 = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    TELEFO2 = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FECING = table.Column<DateTime>(type: "datetime", nullable: true),
                    VTOSUS = table.Column<DateTime>(type: "datetime", nullable: true),
                    ENVIO = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    PLACOD = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    mailreg = table.Column<string>(type: "varchar(101)", maxLength: 101, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    diaregautog = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.UniqueConstraint("AK_SOCIOS_SCOD", x => x.SCOD);
                    table.ForeignKey(
                        name: "FK_SOCIOS_PROVIN_CODPRO1",
                        column: x => x.CODPRO1,
                        principalTable: "PROVIN",
                        principalColumn: "PCOD");
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "ZONAS",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ZCOD = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    MESES = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    INSPEC = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CODPRO = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    LOCALI = table.Column<string>(type: "longtext", nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZONAS", x => x.id);
                    table.UniqueConstraint("AK_ZONAS_ZCOD", x => x.ZCOD);
                    table.ForeignKey(
                        name: "FK_ZONAS_INSPECT_INSPEC",
                        column: x => x.INSPEC,
                        principalTable: "INSPECT",
                        principalColumn: "ICOD");
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "CERTIFSEMEN",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TIPO_CERT = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NRO_CONST = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NRO_CERT = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NROCEN = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FECVTA = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FCH_CONST = table.Column<DateTime>(type: "datetime", nullable: true),
                    NVEN = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NROCRI = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CATEG_SC = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    SCOD = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    TATPART = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    HBA = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NOM_DAD = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NR_INSC = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NR_TSAN = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NR_INSD = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NR_DOSI = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    NR_DOSI_OR = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    TIP_ENV = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    VARIEDAD = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    APODO = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Apodacion = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CERTIFSEMEN", x => x.id);
                    table.ForeignKey(
                        name: "FK_CERTIFSEMEN_CENTROSIA_NROCEN",
                        column: x => x.NROCEN,
                        principalTable: "CENTROSIA",
                        principalColumn: "NROCEN");
                    table.ForeignKey(
                        name: "FK_CERTIFSEMEN_SOCIOS_NROCRI",
                        column: x => x.NROCRI,
                        principalTable: "SOCIOS",
                        principalColumn: "SCOD");
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "PLANTEL",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PLACOD = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ANIOEX = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    VAREDE = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    VQCSRD = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    VQSSRD = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    VAREPR = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    VQCSRP = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    VQSSRP = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    FEULTI = table.Column<DateTime>(type: "datetime", nullable: true),
                    NROINS = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NROCRI = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CATEGO = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ANIOPA = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    UREIN = table.Column<DateTime>(type: "datetime", nullable: true),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    comment = table.Column<string>(type: "longtext", nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ESTADO = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: false, defaultValueSql: "'S'", collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FECING = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLANTEL", x => x.id);
                    table.UniqueConstraint("AK_PLANTEL_PLACOD", x => x.PLACOD);
                    table.ForeignKey(
                        name: "FK_PLANTEL_SOCIOS_NROCRI",
                        column: x => x.NROCRI,
                        principalTable: "SOCIOS",
                        principalColumn: "SCOD");
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "TOROSUNI",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    APODO = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NOMBRE = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    EST_DOC = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    RES_INSP = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    SBCOD_OLD = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    SBCOD = table.Column<int>(type: "int(50)", nullable: true),
                    TIP_TORO = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    TATPART = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NOM_DAD = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NR_INSC = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NR_TSAN = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NR_INSD = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FECHA = table.Column<DateTime>(type: "datetime", nullable: true),
                    HBA = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    VARIEDAD = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CRIADOR = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CATEGO = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    PLANTEL = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ESTCOD = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FCH_BAJA = table.Column<DateTime>(type: "datetime", nullable: true),
                    ACTIVO = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    MOTIVO_BAJ = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NACIDO = table.Column<DateTime>(type: "datetime", nullable: true),
                    ACTUALIZADO = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    CIRC_ESCROTAL = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CodEstado = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true, defaultValueSql: "'0'", collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Id_tipo = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    FECING = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    fechasba = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    pnac = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    pajudest = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    pajufinal = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    gdpostdest = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    indicedest = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    cescrot = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    otros1 = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    promgrupo1 = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    promgrupo2 = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    gdvida = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    indicefinal = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    frame = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    otros2 = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    COMENTARIO = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOROSUNI", x => x.id);
                    table.ForeignKey(
                        name: "FK_TOROSUNI_SOCIOS_CRIADOR",
                        column: x => x.CRIADOR,
                        principalTable: "SOCIOS",
                        principalColumn: "SCOD");
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Names = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    LastNames = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    DNI = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Phone = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Rol = table.Column<string>(type: "text", nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Status = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdentityUserId = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    SocioId = table.Column<int>(type: "int(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_SOCIOS_SocioId",
                        column: x => x.SocioId,
                        principalTable: "SOCIOS",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "ESTABLE",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ECOD = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CODSOC = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CATEGO = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NOMBRE = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    DIRECC = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    LOCALI = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CODPOS = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CODPRO = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    PLANO = table.Column<string>(type: "longtext", nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CODZON = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ACTIVO = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    FECING = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    fechacreacion = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ENCARGADO = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    TEL = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESTABLE", x => x.id);
                    table.UniqueConstraint("AK_ESTABLE_ECOD", x => x.ECOD);
                    table.ForeignKey(
                        name: "FK_ESTABLE_PROVIN_CODPRO",
                        column: x => x.CODPRO,
                        principalTable: "PROVIN",
                        principalColumn: "PCOD");
                    table.ForeignKey(
                        name: "FK_ESTABLE_SOCIOS_CODSOC",
                        column: x => x.CODSOC,
                        principalTable: "SOCIOS",
                        principalColumn: "SCOD");
                    table.ForeignKey(
                        name: "FK_ESTABLE_ZONAS_CODZON",
                        column: x => x.CODZON,
                        principalTable: "ZONAS",
                        principalColumn: "ZCOD");
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "DESEPLA1",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NRODEC = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NROPLAN = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    DESDE = table.Column<DateTime>(type: "datetime", nullable: true),
                    HASTA = table.Column<DateTime>(type: "datetime", nullable: true),
                    FECDECL = table.Column<DateTime>(type: "datetime", nullable: true),
                    FCHRECEPCION = table.Column<DateTime>(type: "datetime", nullable: true),
                    TIPSE = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    SEMEN = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CANTV = table.Column<double>(type: "double", nullable: true),
                    CANTB = table.Column<double>(type: "double", nullable: true),
                    REMBA = table.Column<double>(type: "double", nullable: true),
                    REMPR = table.Column<double>(type: "double", nullable: true),
                    SEMPROP = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    CANTOR = table.Column<double>(type: "double", nullable: true),
                    REMMPR = table.Column<double>(type: "double", nullable: true),
                    CTRLU = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    CTRLM = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    COEF_AUTO_SN = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    COEF_AUTO_IA = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    COEF_AUTO_IAR = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    IA_SINCRO = table.Column<float>(type: "float", nullable: true, defaultValueSql: "'0'"),
                    PASTILLAS_SINCRO = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    FECRET = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NR_FOLIO = table.Column<int>(type: "int(9)", nullable: true),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    reten = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false, defaultValueSql: "'0'", collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    edicion = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false, defaultValueSql: "'0'", collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NROCRI = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DESEPLA1", x => x.id);
                    table.ForeignKey(
                        name: "FK_DESEPLA1_PLANTEL_NROPLAN",
                        column: x => x.NROPLAN,
                        principalTable: "PLANTEL",
                        principalColumn: "PLACOD");
                    table.ForeignKey(
                        name: "FK_DESEPLA1_SOCIOS_NROCRI",
                        column: x => x.NROCRI,
                        principalTable: "SOCIOS",
                        principalColumn: "SCOD",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "user_socios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int(11)", nullable: false),
                    socio_id = table.Column<int>(type: "int(11)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_socios_SOCIOS_socio_id",
                        column: x => x.socio_id,
                        principalTable: "SOCIOS",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_socios_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "RESIN1",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SCOD = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false, defaultValueSql: "''", collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NRORES = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NROPLA = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    FREALI = table.Column<DateTime>(type: "datetime", nullable: true),
                    OBSERV = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    PPAJUST = table.Column<int>(type: "int(11)", nullable: true),
                    EPROMEDIO = table.Column<int>(type: "int(11)", nullable: true),
                    EMAX = table.Column<int>(type: "int(11)", nullable: true),
                    EMIN = table.Column<int>(type: "int(11)", nullable: true),
                    TORTOT = table.Column<int>(type: "int(11)", nullable: true),
                    TORSB = table.Column<int>(type: "int(11)", nullable: true),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true),
                    editar = table.Column<int>(type: "int(1)", nullable: false),
                    ICOD = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false, defaultValueSql: "'0'", collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ESTCOD = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.id, x.SCOD })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.UniqueConstraint("AK_RESIN1_NRORES", x => x.NRORES);
                    table.ForeignKey(
                        name: "FK_RESIN1_ESTABLE_ESTCOD",
                        column: x => x.ESTCOD,
                        principalTable: "ESTABLE",
                        principalColumn: "ECOD");
                    table.ForeignKey(
                        name: "FK_RESIN1_SOCIOS_SCOD",
                        column: x => x.SCOD,
                        principalTable: "SOCIOS",
                        principalColumn: "SCOD",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "SOLICI1",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CODEST = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    NROSOL = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    FECSOL = table.Column<DateTime>(type: "datetime", nullable: true),
                    LUGAR = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CANTOR = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    CANTVQ = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    PRODUC = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    REINSP = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CANVAC = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    CANVAQ = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    EDAD_MIN_MAC = table.Column<int>(type: "int(11)", nullable: true),
                    EDAD_MAX_HEM = table.Column<int>(type: "int(11)", nullable: true),
                    EDAD_MIN_HEM = table.Column<int>(type: "int(11)", nullable: true),
                    EDAD_MAX_MAC = table.Column<int>(type: "int(11)", nullable: true),
                    TYNCTE = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    BANCO = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    IMPORT = table.Column<double>(type: "double", nullable: true),
                    FECINS = table.Column<DateTime>(type: "datetime", nullable: true),
                    CTRL1 = table.Column<sbyte>(type: "tinyint(4)", nullable: true),
                    CTRL2 = table.Column<sbyte>(type: "tinyint(4)", nullable: true),
                    FECRET = table.Column<DateTime>(type: "datetime", nullable: true),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true),
                    Anio = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOLICI1", x => x.id);
                    table.ForeignKey(
                        name: "FK_SOLICI1_ESTABLE_CODEST",
                        column: x => x.CODEST,
                        principalTable: "ESTABLE",
                        principalColumn: "ECOD");
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "TRANSSB",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NRO_TRANS = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    FECTRANS = table.Column<DateTime>(type: "datetime", nullable: true),
                    NRO_ORDEN = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    SVEN = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CATEG_SV = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    VNOM = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    SCOM = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CATEG_SC = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CNOM = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ECOD = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    FCH_USU = table.Column<DateTime>(type: "datetime", nullable: true),
                    COD_USU = table.Column<int>(type: "int(11)", nullable: true),
                    TOROVENDIDO = table.Column<int>(type: "int(11)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSSB", x => x.id);
                    table.ForeignKey(
                        name: "FK_TRANSSB_ESTABLE_ECOD",
                        column: x => x.ECOD,
                        principalTable: "ESTABLE",
                        principalColumn: "ECOD");
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "RESIN6",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HDP = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    HDP_M = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    HDP_AS = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    HDT = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    HDB = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    HPP = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    HPP_M = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    HPP_AS = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    HPT = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    HPB = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    HGVP = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    HGVB = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    HGQP = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    HGQB = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    MCP = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    MCP_M = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    MCP_AS = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    MCT = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    MSP = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    MSP_M = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    MSP_AS = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    MST = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    MSPSB = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    NRORES = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RESIN6", x => x.id);
                    table.ForeignKey(
                        name: "FK_RESIN6_RESIN1_NRORES",
                        column: x => x.NRORES,
                        principalTable: "RESIN1",
                        principalColumn: "NRORES");
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "SOLICI1AUX",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdSoli = table.Column<int>(type: "int(11)", nullable: false),
                    Anio = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CANTOR = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    CANTVQ = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    CANVAC = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'"),
                    CANVAQ = table.Column<double>(type: "double", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOLICI1AUX", x => x.id);
                    table.ForeignKey(
                        name: "FK_SOLICI1AUX_SOLICI1_IdSoli",
                        column: x => x.IdSoli,
                        principalTable: "SOLICI1",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateIndex(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "FK_AspNetUserClaims_AspNetUserUserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRole_RoleId",
                table: "AspNetUserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRole_UserId",
                table: "AspNetUserRole",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CERTIFSEMEN_NROCEN",
                table: "CERTIFSEMEN",
                column: "NROCEN");

            migrationBuilder.CreateIndex(
                name: "IX_CERTIFSEMEN_NROCRI",
                table: "CERTIFSEMEN",
                column: "NROCRI");

            migrationBuilder.CreateIndex(
                name: "IX_DESEPLA1_NROCRI",
                table: "DESEPLA1",
                column: "NROCRI");

            migrationBuilder.CreateIndex(
                name: "IX_DESEPLA1_NROPLAN",
                table: "DESEPLA1",
                column: "NROPLAN");

            migrationBuilder.CreateIndex(
                name: "IX_ESTABLE_CODPRO",
                table: "ESTABLE",
                column: "CODPRO");

            migrationBuilder.CreateIndex(
                name: "IX_ESTABLE_CODSOC",
                table: "ESTABLE",
                column: "CODSOC");

            migrationBuilder.CreateIndex(
                name: "IX_ESTABLE_CODZON",
                table: "ESTABLE",
                column: "CODZON");

            migrationBuilder.CreateIndex(
                name: "IX_INSPECT_CODPRO",
                table: "INSPECT",
                column: "CODPRO");

            migrationBuilder.CreateIndex(
                name: "IX_PLANTEL_NROCRI",
                table: "PLANTEL",
                column: "NROCRI");

            migrationBuilder.CreateIndex(
                name: "IX_RESIN1_ESTCOD",
                table: "RESIN1",
                column: "ESTCOD");

            migrationBuilder.CreateIndex(
                name: "IX_RESIN1_SCOD",
                table: "RESIN1",
                column: "SCOD");

            migrationBuilder.CreateIndex(
                name: "IX_RESIN6_NRORES",
                table: "RESIN6",
                column: "NRORES");

            migrationBuilder.CreateIndex(
                name: "IX_SOCIOS_CODPRO1",
                table: "SOCIOS",
                column: "CODPRO1");

            migrationBuilder.CreateIndex(
                name: "IX_SOLICI1_CODEST",
                table: "SOLICI1",
                column: "CODEST");

            migrationBuilder.CreateIndex(
                name: "IX_SOLICI1AUX_IdSoli",
                table: "SOLICI1AUX",
                column: "IdSoli");

            migrationBuilder.CreateIndex(
                name: "IX_TOROSUNI_CRIADOR",
                table: "TOROSUNI",
                column: "CRIADOR");

            migrationBuilder.CreateIndex(
                name: "IX_TRANSSB_ECOD",
                table: "TRANSSB",
                column: "ECOD");

            migrationBuilder.CreateIndex(
                name: "IX_User_SocioId",
                table: "User",
                column: "SocioId");

            migrationBuilder.CreateIndex(
                name: "IX_user_socios_socio_id",
                table: "user_socios",
                column: "socio_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_socios_user_id",
                table: "user_socios",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UX_user_socios_user_socio",
                table: "user_socios",
                columns: new[] { "user_id", "socio_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ZONAS_INSPEC",
                table: "ZONAS",
                column: "INSPEC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "actualizacion");

            migrationBuilder.DropTable(
                name: "archrural");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRole");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AUTORIZA");

            migrationBuilder.DropTable(
                name: "CERTIFSEMEN");

            migrationBuilder.DropTable(
                name: "DESE_USADAS");

            migrationBuilder.DropTable(
                name: "DESEPLA1");

            migrationBuilder.DropTable(
                name: "DESEPLA2");

            migrationBuilder.DropTable(
                name: "DESEPLA3");

            migrationBuilder.DropTable(
                name: "DESEPLA4 - No Usada");

            migrationBuilder.DropTable(
                name: "DESEPLA5 - No Usada");

            migrationBuilder.DropTable(
                name: "FUT_CONTROL");

            migrationBuilder.DropTable(
                name: "opcdecserv");

            migrationBuilder.DropTable(
                name: "PEG");

            migrationBuilder.DropTable(
                name: "REMANSOL");

            migrationBuilder.DropTable(
                name: "RESIN2");

            migrationBuilder.DropTable(
                name: "RESIN3");

            migrationBuilder.DropTable(
                name: "RESIN4");

            migrationBuilder.DropTable(
                name: "RESIN6");

            migrationBuilder.DropTable(
                name: "RESIN8");

            migrationBuilder.DropTable(
                name: "RESIN9");

            migrationBuilder.DropTable(
                name: "retenidas");

            migrationBuilder.DropTable(
                name: "SOLICI1AUX");

            migrationBuilder.DropTable(
                name: "SOLICI4");

            migrationBuilder.DropTable(
                name: "SOLICI6");

            migrationBuilder.DropTable(
                name: "TOROSPR");

            migrationBuilder.DropTable(
                name: "TorosRural");

            migrationBuilder.DropTable(
                name: "TOROSUNI");

            migrationBuilder.DropTable(
                name: "TOROSUNIestados");

            migrationBuilder.DropTable(
                name: "TRANSAN");

            migrationBuilder.DropTable(
                name: "TRANSEM");

            migrationBuilder.DropTable(
                name: "TRANSSB");

            migrationBuilder.DropTable(
                name: "user_socios");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CENTROSIA");

            migrationBuilder.DropTable(
                name: "PLANTEL");

            migrationBuilder.DropTable(
                name: "RESIN1");

            migrationBuilder.DropTable(
                name: "SOLICI1");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "ESTABLE");

            migrationBuilder.DropTable(
                name: "SOCIOS");

            migrationBuilder.DropTable(
                name: "ZONAS");

            migrationBuilder.DropTable(
                name: "INSPECT");

            migrationBuilder.DropTable(
                name: "PROVIN");
        }
    }
}
