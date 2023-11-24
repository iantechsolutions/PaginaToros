using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PaginaToros.Server.Models;
using PaginaToros.Shared.Models;

namespace PaginaToros.Server.Context
{
    public partial class hereford_prContext : DbContext
    {
        public hereford_prContext()
        {
        }

        public hereford_prContext(DbContextOptions<hereford_prContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actualizacion> Actualizacions { get; set; } = null!;
        public virtual DbSet<Archrural> Archrurals { get; set; } = null!;
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<Autoriza> Autorizas { get; set; } = null!;
        public virtual DbSet<Centrosium> Centrosia { get; set; } = null!;
        public virtual DbSet<Certifseman> Certifsemen { get; set; } = null!;
        public virtual DbSet<DeseUsada> DeseUsadas { get; set; } = null!;
        public virtual DbSet<Desepla1> Desepla1s { get; set; } = null!;
        public virtual DbSet<Desepla2> Desepla2s { get; set; } = null!;
        public virtual DbSet<Desepla3> Desepla3s { get; set; } = null!;
        public virtual DbSet<Desepla4NoUsadum> Desepla4NoUsada { get; set; } = null!;
        public virtual DbSet<Desepla5NoUsadum> Desepla5NoUsada { get; set; } = null!;
        public virtual DbSet<Estable> Estables { get; set; } = null!;
        public virtual DbSet<Futcontrol> Futcontrols { get; set; } = null!;
        public virtual DbSet<Inspect> Inspects { get; set; } = null!;
        public virtual DbSet<Opcdecserv> Opcdecservs { get; set; } = null!;
        public virtual DbSet<Peg> Pegs { get; set; } = null!;
        public virtual DbSet<Plantel> Planteles { get; set; } = null!;
        public virtual DbSet<Provin> Provins { get; set; } = null!;
        public virtual DbSet<Remansol> Remansols { get; set; } = null!;
        public virtual DbSet<Resin1> Resin1s { get; set; } = null!;
        public virtual DbSet<Resin2> Resin2s { get; set; } = null!;
        public virtual DbSet<Resin3> Resin3s { get; set; } = null!;
        public virtual DbSet<Resin4> Resin4s { get; set; } = null!;
        public virtual DbSet<Resin6> Resin6s { get; set; } = null!;
        public virtual DbSet<Resin8> Resin8s { get; set; } = null!;
        public virtual DbSet<Resin9> Resin9s { get; set; } = null!;
        public virtual DbSet<Retenida> Retenidas { get; set; } = null!;
        public virtual DbSet<Socio> Socios { get; set; } = null!;
        public virtual DbSet<Solici1> Solici1s { get; set; } = null!;
        public virtual DbSet<Solici4> Solici4s { get; set; } = null!;
        public virtual DbSet<Solici6> Solici6s { get; set; } = null!;
        public virtual DbSet<TorosRural> TorosRurals { get; set; } = null!;
        public virtual DbSet<Torospr> Torosprs { get; set; } = null!;
        public virtual DbSet<Torosuni> Torosunis { get; set; } = null!;
        public virtual DbSet<Torosuniestado> Torosuniestados { get; set; } = null!;
        public virtual DbSet<Transan> Transans { get; set; } = null!;
        public virtual DbSet<Transem> Transems { get; set; } = null!;
        public virtual DbSet<Transsb> Transsbs { get; set; } = null!;
        public virtual DbSet<User> User { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<Zona> Zonas { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=vxsct3514.avnam.net;port=3306;user=herefordapp_com_ar;password=RWEr4dod6g3G;persist security info=True;database=hereford_pr;convert zero datetime=True", ServerVersion.Parse("10.3.39-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("latin1_swedish_ci")
                .HasCharSet("latin1");

            modelBuilder.Entity<Actualizacion>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("actualizacion");

                entity.Property(e => e.Archrural)
                    .HasMaxLength(10)
                    .HasColumnName("archrural");

                entity.Property(e => e.Centrosia)
                    .HasMaxLength(10)
                    .HasColumnName("centrosia")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Certsemen)
                    .HasMaxLength(10)
                    .HasColumnName("certsemen");

                entity.Property(e => e.Decserv)
                    .HasMaxLength(10)
                    .HasColumnName("decserv");

                entity.Property(e => e.Establec)
                    .HasMaxLength(10)
                    .HasColumnName("establec");

                entity.Property(e => e.Inspect)
                    .HasMaxLength(10)
                    .HasColumnName("inspect");

                entity.Property(e => e.Planteles)
                    .HasMaxLength(10)
                    .HasColumnName("planteles");

                entity.Property(e => e.Resultsol)
                    .HasMaxLength(10)
                    .HasColumnName("resultsol");

                entity.Property(e => e.Socios)
                    .HasMaxLength(10)
                    .HasColumnName("socios");

                entity.Property(e => e.Solicinspecc)
                    .HasMaxLength(10)
                    .HasColumnName("solicinspecc");

                entity.Property(e => e.Torosp)
                    .HasMaxLength(10)
                    .HasColumnName("torosp");

                entity.Property(e => e.Torospr)
                    .HasMaxLength(10)
                    .HasColumnName("torospr");

                entity.Property(e => e.Transfafut)
                    .HasMaxLength(10)
                    .HasColumnName("transfafut");

                entity.Property(e => e.Transfanim)
                    .HasMaxLength(10)
                    .HasColumnName("transfanim");

                entity.Property(e => e.Transfsb)
                    .HasMaxLength(10)
                    .HasColumnName("transfsb");

                entity.Property(e => e.Zonas)
                    .HasMaxLength(10)
                    .HasColumnName("zonas");
            });

            modelBuilder.Entity<Archrural>(entity =>
            {
                entity.ToTable("archrural");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Hba)
                    .HasMaxLength(10)
                    .HasColumnName("HBA");

                entity.Property(e => e.Nac).HasColumnName("NAC");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Tat)
                    .HasMaxLength(10)
                    .HasColumnName("TAT");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(10)
                    .HasColumnName("TIPO");
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "FK_AspNetRoleClaims_AspNetRoles_RoleId");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.RoleId).HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("AspNetRoleClaims_ibfk_1");
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.AccessFailedCount).HasColumnType("int(11)");
                entity.Property(e => e.Email).HasMaxLength(256);
                entity.Property(e => e.LockoutEnd).HasMaxLength(6);
                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId").HasConstraintName("AspNetUserRoles_ibfk_1"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId").HasConstraintName("AspNetUserRoles_ibfk_2"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                            j.ToTable("AspNetUserRoles");
                            j.HasIndex(new[] { "RoleId" }, "RoleId");
                            j.IndexerProperty<string>("UserId").HasMaxLength(450);
                            j.IndexerProperty<string>("RoleId").HasMaxLength(450);
                        });
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(450);
                // Other configuration for the AspNetRole entity
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
                // Other configuration for the AspNetUserRole entity, if needed
            });


            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "FK_AspNetUserClaims_AspNetUserUserId");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.HasIndex(e => e.UserId, "UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.ProviderKey).HasMaxLength(450);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("AspNetUserLogins_ibfk_1");
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("AspNetUserTokens_ibfk_1");
            });

            modelBuilder.Entity<Autoriza>(entity =>
            {
                entity.ToTable("AUTORIZA");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Aniopr)
                    .HasMaxLength(4)
                    .HasColumnName("ANIOPR");

                entity.Property(e => e.Aniose)
                    .HasMaxLength(4)
                    .HasColumnName("ANIOSE");

                entity.Property(e => e.Basvac)
                    .HasColumnName("BASVAC")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Basvaq)
                    .HasColumnName("BASVAQ")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Cantor)
                    .HasColumnName("CANTOR")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Canvpr)
                    .HasColumnName("CANVPR")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU");

                entity.Property(e => e.Fautor)
                    .HasColumnType("datetime")
                    .HasColumnName("FAUTOR");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Freali)
                    .HasColumnType("datetime")
                    .HasColumnName("FREALI");

                entity.Property(e => e.Nrosol)
                    .HasMaxLength(6)
                    .HasColumnName("NROSOL");

                entity.Property(e => e.Nrosolanual)
                    .HasMaxLength(4)
                    .HasColumnName("nrosolanual")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Procre).HasColumnName("PROCRE");

                entity.Property(e => e.Vacser).HasColumnName("VACSER");
            });

            modelBuilder.Entity<Centrosium>(entity =>
            {
                entity.ToTable("CENTROSIA");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.NroCSayg)
                    .HasMaxLength(10)
                    .HasColumnName("NRO_C_SAYG");

                entity.Property(e => e.Nrocen)
                    .HasMaxLength(6)
                    .HasColumnName("NROCEN");
            });

            modelBuilder.Entity<Certifseman>(entity =>
            {
                entity.ToTable("CERTIFSEMEN");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Apodo)
                    .HasMaxLength(50)
                    .HasColumnName("APODO");

                entity.Property(e => e.CategSc)
                    .HasMaxLength(1)
                    .HasColumnName("CATEG_SC");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.FchConst)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_CONST");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Fecvta).HasColumnName("FECVTA");

                entity.Property(e => e.Hba)
                    .HasMaxLength(10)
                    .HasColumnName("HBA");

                entity.Property(e => e.NomDad)
                    .HasMaxLength(100)
                    .HasColumnName("NOM_DAD");

                entity.Property(e => e.NrDosi)
                    .HasColumnType("int(11)")
                    .HasColumnName("NR_DOSI")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.NrDosiOr)
                    .HasMaxLength(10)
                    .HasColumnName("NR_DOSI_OR");

                entity.Property(e => e.NrInsc)
                    .HasMaxLength(10)
                    .HasColumnName("NR_INSC");

                entity.Property(e => e.NrInsd)
                    .HasMaxLength(10)
                    .HasColumnName("NR_INSD");

                entity.Property(e => e.NrTsan)
                    .HasMaxLength(10)
                    .HasColumnName("NR_TSAN");

                entity.Property(e => e.NroCert)
                    .HasMaxLength(6)
                    .HasColumnName("NRO_CERT");

                entity.Property(e => e.NroConst)
                    .HasMaxLength(6)
                    .HasColumnName("NRO_CONST");

                entity.Property(e => e.Nrocen)
                    .HasMaxLength(50)
                    .HasColumnName("NROCEN");

                entity.Property(e => e.Nrocri)
                    .HasMaxLength(6)
                    .HasColumnName("NROCRI");

                entity.Property(e => e.Nven)
                    .HasMaxLength(25)
                    .HasColumnName("NVEN");

                entity.Property(e => e.Scod)
                    .HasMaxLength(4)
                    .HasColumnName("SCOD");

                entity.Property(e => e.Tatpart)
                    .HasMaxLength(8)
                    .HasColumnName("TATPART");

                entity.Property(e => e.TipEnv)
                    .HasMaxLength(8)
                    .HasColumnName("TIP_ENV");

                entity.Property(e => e.TipoCert)
                    .HasMaxLength(9)
                    .HasColumnName("TIPO_CERT");

                entity.Property(e => e.Variedad)
                    .HasMaxLength(2)
                    .HasColumnName("VARIEDAD");
            });

            modelBuilder.Entity<Certifseman>()
               .HasOne(t => t.Socio)
               .WithMany(s => s.Certificados)
               .HasForeignKey(t => t.Nrocri)
               .HasPrincipalKey(s => s.Scod);

            modelBuilder.Entity<Certifseman>()
               .HasOne(t => t.Centro)
               .WithMany(s => s.Certificados)
               .HasForeignKey(t => t.Nrocen)
               .HasPrincipalKey(s => s.Nrocen);


            modelBuilder.Entity<DeseUsada>(entity =>
            {
                entity.ToTable("DESE_USADAS");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Certsemen)
                    .HasMaxLength(10)
                    .HasColumnName("CERTSEMEN");

                entity.Property(e => e.DosisUsada)
                    .HasMaxLength(10)
                    .HasColumnName("DOSIS_USADA");

                entity.Property(e => e.Nrodec)
                    .HasMaxLength(6)
                    .HasColumnName("NRODEC");
            });

            modelBuilder.Entity<Desepla1>(entity =>
            {
                entity.ToTable("DESEPLA1");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Cantb).HasColumnName("CANTB");

                entity.Property(e => e.Cantor).HasColumnName("CANTOR");

                entity.Property(e => e.Cantv).HasColumnName("CANTV");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoefAutoIa)
                    .HasColumnName("COEF_AUTO_IA")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoefAutoIar)
                    .HasColumnName("COEF_AUTO_IAR")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoefAutoSn)
                    .HasColumnName("COEF_AUTO_SN")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Ctrlm)
                    .HasColumnName("CTRLM")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Ctrlu)
                    .HasColumnName("CTRLU")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Desde)
                    .HasColumnType("datetime")
                    .HasColumnName("DESDE");

                entity.Property(e => e.Edicion)
                    .HasMaxLength(1)
                    .HasColumnName("edicion")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Fchrecepcion)
                    .HasColumnType("datetime")
                    .HasColumnName("FCHRECEPCION");

                entity.Property(e => e.Fecdecl)
                    .HasColumnType("datetime")
                    .HasColumnName("FECDECL");

                entity.Property(e => e.Fecret)
                    .HasMaxLength(10)
                    .HasColumnName("FECRET");

                entity.Property(e => e.Hasta)
                    .HasColumnType("datetime")
                    .HasColumnName("HASTA");

                entity.Property(e => e.IaSincro)
                    .HasColumnName("IA_SINCRO")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.NrFolio)
                    .HasColumnType("int(9)")
                    .HasColumnName("NR_FOLIO");

                entity.Property(e => e.Nrocri)
                    .HasMaxLength(6)
                    .HasColumnName("NROCRI");

                entity.Property(e => e.Nrodec)
                    .HasMaxLength(6)
                    .HasColumnName("NRODEC");

                entity.Property(e => e.Nroplan)
                    .HasMaxLength(5)
                    .HasColumnName("NROPLAN");

                entity.Property(e => e.PastillasSincro)
                    .HasColumnType("int(11)")
                    .HasColumnName("PASTILLAS_SINCRO")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Remba).HasColumnName("REMBA");

                entity.Property(e => e.Remmpr).HasColumnName("REMMPR");

                entity.Property(e => e.Rempr).HasColumnName("REMPR");

                entity.Property(e => e.Reten)
                    .HasMaxLength(1)
                    .HasColumnName("reten")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Semen)
                    .HasMaxLength(1)
                    .HasColumnName("SEMEN");

                entity.Property(e => e.Semprop)
                    .HasColumnName("SEMPROP")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Tipse)
                    .HasMaxLength(5)
                    .HasColumnName("TIPSE");
            });

            modelBuilder.Entity<Desepla2>(entity =>
            {
                entity.ToTable("DESEPLA2");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Apodo)
                    .HasMaxLength(50)
                    .HasColumnName("APODO");

                entity.Property(e => e.Desde)
                    .HasMaxLength(10)
                    .HasColumnName("DESDE");

                entity.Property(e => e.Hardb)
                    .HasMaxLength(10)
                    .HasColumnName("HARDB");

                entity.Property(e => e.Hasta)
                    .HasMaxLength(10)
                    .HasColumnName("HASTA");

                entity.Property(e => e.Nrodec)
                    .HasMaxLength(6)
                    .HasColumnName("NRODEC");

                entity.Property(e => e.Tatpart)
                    .HasMaxLength(8)
                    .HasColumnName("TATPART");
            });

            modelBuilder.Entity<Desepla3>(entity =>
            {
                entity.ToTable("DESEPLA3");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Apodo)
                    .HasMaxLength(50)
                    .HasColumnName("APODO");

                entity.Property(e => e.Cantv).HasColumnName("CANTV");

                entity.Property(e => e.Desde)
                    .HasMaxLength(10)
                    .HasColumnName("DESDE");

                entity.Property(e => e.Hardb)
                    .HasMaxLength(10)
                    .HasColumnName("HARDB");

                entity.Property(e => e.Hasta)
                    .HasMaxLength(10)
                    .HasColumnName("HASTA");

                entity.Property(e => e.Nrodec)
                    .HasMaxLength(6)
                    .HasColumnName("NRODEC");

                entity.Property(e => e.Servicio)
                    .HasMaxLength(10)
                    .HasColumnName("SERVICIO");

                entity.Property(e => e.Tatpart)
                    .HasMaxLength(8)
                    .HasColumnName("TATPART");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(10)
                    .HasColumnName("TIPO");
            });

            modelBuilder.Entity<Desepla4NoUsadum>(entity =>
            {
                entity.ToTable("DESEPLA4 - No Usada");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Cantv).HasColumnName("CANTV");

                entity.Property(e => e.Ctst)
                    .HasMaxLength(6)
                    .HasColumnName("CTST");

                entity.Property(e => e.Nrodec)
                    .HasMaxLength(6)
                    .HasColumnName("NRODEC");
            });

            modelBuilder.Entity<Desepla5NoUsadum>(entity =>
            {
                entity.ToTable("DESEPLA5 - No Usada");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.ENom)
                    .HasMaxLength(70)
                    .HasColumnName("E_NOM");

                entity.Property(e => e.Nrodec)
                    .HasMaxLength(6)
                    .HasColumnName("NRODEC");
            });

            modelBuilder.Entity<Estable>(entity =>
            {
                entity.ToTable("ESTABLE");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Activo)
                    .HasMaxLength(1)
                    .HasColumnName("ACTIVO");

                entity.Property(e => e.Catego)
                    .HasMaxLength(1)
                    .HasColumnName("CATEGO");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Codpos)
                    .HasMaxLength(50)
                    .HasColumnName("CODPOS");

                entity.Property(e => e.Codpro)
                    .HasMaxLength(2)
                    .HasColumnName("CODPRO");

                entity.Property(e => e.Codsoc)
                    .HasMaxLength(6)
                    .HasColumnName("CODSOC");

                entity.Property(e => e.Codzon)
                    .HasMaxLength(2)
                    .HasColumnName("CODZON");

                entity.Property(e => e.Direcc)
                    .HasMaxLength(100)
                    .HasColumnName("DIRECC");

                entity.Property(e => e.Ecod)
                    .HasMaxLength(6)
                    .HasColumnName("ECOD");

                entity.Property(e => e.Encargado)
                    .HasMaxLength(50)
                    .HasColumnName("ENCARGADO");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Fechacreacion)
                    .HasMaxLength(4)
                    .HasColumnName("fechacreacion");

                entity.Property(e => e.Fecing)
                    .HasMaxLength(10)
                    .HasColumnName("FECING");

                entity.Property(e => e.Locali)
                    .HasMaxLength(100)
                    .HasColumnName("LOCALI");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Plano).HasColumnName("PLANO");

                entity.Property(e => e.Tel)
                    .HasMaxLength(30)
                    .HasColumnName("TEL");
            });

            modelBuilder.Entity<Estable>()
               .HasOne(p => p.Socio)
               .WithMany(s => s.Establecimientos)
               .HasForeignKey(t => t.Codsoc)
               .HasPrincipalKey(s => s.Scod);

            modelBuilder.Entity<Futcontrol>(entity =>
            {
                entity.ToTable("FUT_CONTROL");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.CantHem)
                    .HasColumnType("int(11)")
                    .HasColumnName("CANT_HEM");

                entity.Property(e => e.CantMach)
                    .HasColumnType("int(11)")
                    .HasColumnName("CANT_MACH");

                entity.Property(e => e.CategSc)
                    .HasMaxLength(1)
                    .HasColumnName("CATEG_SC");

                entity.Property(e => e.CategSv)
                    .HasMaxLength(1)
                    .HasColumnName("CATEG_SV");

                entity.Property(e => e.Cnom)
                    .HasMaxLength(50)
                    .HasColumnName("CNOM");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU");

                entity.Property(e => e.EdadCrias)
                    .HasColumnType("int(11)")
                    .HasColumnName("EDAD_CRIAS");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Fectrans)
                    .HasColumnType("datetime")
                    .HasColumnName("FECTRANS");

                entity.Property(e => e.Hemsta)
                    .HasMaxLength(3)
                    .HasColumnName("HEMSTA");

                entity.Property(e => e.Incorp)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("INCORP");

                entity.Property(e => e.NroTrans)
                    .HasMaxLength(6)
                    .HasColumnName("NRO_TRANS");

                entity.Property(e => e.PlantDest)
                    .HasMaxLength(5)
                    .HasColumnName("PLANT_DEST");

                entity.Property(e => e.Plantel)
                    .HasMaxLength(4)
                    .HasColumnName("PLANTEL");

                entity.Property(e => e.Scom)
                    .HasMaxLength(6)
                    .HasColumnName("SCOM");

                entity.Property(e => e.Sven)
                    .HasMaxLength(6)
                    .HasColumnName("SVEN");

                entity.Property(e => e.Vnom)
                    .HasMaxLength(35)
                    .HasColumnName("VNOM");
            });

            modelBuilder.Entity<Inspect>(entity =>
            {
                entity.ToTable("INSPECT");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Codpos)
                    .HasMaxLength(50)
                    .HasColumnName("CODPOS");

                entity.Property(e => e.Codpro)
                    .HasMaxLength(2)
                    .HasColumnName("CODPRO");

                entity.Property(e => e.Direcc)
                    .HasMaxLength(100)
                    .HasColumnName("DIRECC");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Icod)
                    .HasMaxLength(6)
                    .HasColumnName("ICOD");

                entity.Property(e => e.Locali)
                    .HasMaxLength(100)
                    .HasColumnName("LOCALI");

                entity.Property(e => e.Mail)
                    .HasMaxLength(50)
                    .HasColumnName("mail");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Telefo)
                    .HasMaxLength(50)
                    .HasColumnName("TELEFO");
            });

            modelBuilder.Entity<Opcdecserv>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("opcdecserv");

                entity.Property(e => e.Cotamax)
                    .HasMaxLength(3)
                    .HasColumnName("cotamax");

                entity.Property(e => e.Cotamin)
                    .HasMaxLength(3)
                    .HasColumnName("cotamin");

                entity.Property(e => e.Hedadmax)
                    .HasMaxLength(3)
                    .HasColumnName("hedadmax");

                entity.Property(e => e.Hedadmin)
                    .HasMaxLength(3)
                    .HasColumnName("hedadmin");

                entity.Property(e => e.Ia1dosis)
                    .HasMaxLength(3)
                    .HasColumnName("ia1dosis");

                entity.Property(e => e.Ia1max)
                    .HasMaxLength(3)
                    .HasColumnName("ia1max");

                entity.Property(e => e.Ia1min)
                    .HasMaxLength(3)
                    .HasColumnName("ia1min");

                entity.Property(e => e.Ia2dosis)
                    .HasMaxLength(3)
                    .HasColumnName("ia2dosis");

                entity.Property(e => e.Ia2max)
                    .HasMaxLength(3)
                    .HasColumnName("ia2max");

                entity.Property(e => e.Ia2min)
                    .HasMaxLength(3)
                    .HasColumnName("ia2min");

                entity.Property(e => e.Ia3dosis)
                    .HasMaxLength(3)
                    .HasColumnName("ia3dosis");

                entity.Property(e => e.Ia3max)
                    .HasMaxLength(3)
                    .HasColumnName("ia3max");

                entity.Property(e => e.Ia3min)
                    .HasMaxLength(3)
                    .HasColumnName("ia3min");

                entity.Property(e => e.Iadosis)
                    .HasMaxLength(3)
                    .HasColumnName("iadosis");

                entity.Property(e => e.Iamax)
                    .HasMaxLength(3)
                    .HasColumnName("iamax");

                entity.Property(e => e.Iamin)
                    .HasMaxLength(3)
                    .HasColumnName("iamin");

                entity.Property(e => e.Medadmax)
                    .HasMaxLength(3)
                    .HasColumnName("medadmax");

                entity.Property(e => e.Medadmin)
                    .HasMaxLength(3)
                    .HasColumnName("medadmin");

                entity.Property(e => e.Natcorrdosis)
                    .HasMaxLength(3)
                    .HasColumnName("natcorrdosis");

                entity.Property(e => e.Natcorrmax)
                    .HasMaxLength(3)
                    .HasColumnName("natcorrmax");

                entity.Property(e => e.Natcorrmin)
                    .HasMaxLength(3)
                    .HasColumnName("natcorrmin");

                entity.Property(e => e.Natdosis)
                    .HasMaxLength(3)
                    .HasColumnName("natdosis");

                entity.Property(e => e.Natmax)
                    .HasMaxLength(3)
                    .HasColumnName("natmax");

                entity.Property(e => e.Natmin)
                    .HasMaxLength(3)
                    .HasColumnName("natmin");

                entity.Property(e => e.Unidadmes)
                    .HasMaxLength(3)
                    .HasColumnName("unidadmes");
            });

            modelBuilder.Entity<Peg>(entity =>
            {
                entity.ToTable("PEG");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Apodo)
                    .HasMaxLength(255)
                    .HasColumnName("APODO");

                entity.Property(e => e.Cvientres)
                    .HasMaxLength(10)
                    .HasColumnName("CVIENTRES");

                entity.Property(e => e.Desde)
                    .HasMaxLength(10)
                    .HasColumnName("DESDE");

                entity.Property(e => e.Hasta)
                    .HasMaxLength(10)
                    .HasColumnName("HASTA");

                entity.Property(e => e.Hba)
                    .HasMaxLength(10)
                    .HasColumnName("HBA");

                entity.Property(e => e.Nrodec)
                    .HasMaxLength(10)
                    .HasColumnName("NRODEC");

                entity.Property(e => e.Nrosocio)
                    .HasMaxLength(10)
                    .HasColumnName("NROSOCIO");

                entity.Property(e => e.Servicio)
                    .HasMaxLength(10)
                    .HasColumnName("SERVICIO");

                entity.Property(e => e.Socio)
                    .HasMaxLength(200)
                    .HasColumnName("SOCIO");
            });

            modelBuilder.Entity<Plantel>(entity =>
            {
                entity.ToTable("PLANTEL");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Anioex)
                    .HasMaxLength(4)
                    .HasColumnName("ANIOEX");

                entity.Property(e => e.Aniopa)
                    .HasMaxLength(4)
                    .HasColumnName("ANIOPA");

                entity.Property(e => e.Catego)
                    .HasMaxLength(1)
                    .HasColumnName("CATEGO");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Comment).HasColumnName("comment");

                entity.Property(e => e.Estado)
                    .HasMaxLength(4)
                    .HasColumnName("ESTADO")
                    .HasDefaultValueSql("'S'");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Fecing)
                    .HasMaxLength(10)
                    .HasColumnName("FECING");

                entity.Property(e => e.Feulti)
                    .HasColumnType("datetime")
                    .HasColumnName("FEULTI");

                entity.Property(e => e.Nrocri)
                    .HasMaxLength(6)
                    .HasColumnName("NROCRI");

                entity.Property(e => e.Nroins)
                    .HasMaxLength(6)
                    .HasColumnName("NROINS");

                entity.Property(e => e.Placod)
                    .HasMaxLength(10)
                    .HasColumnName("PLACOD");

                entity.Property(e => e.Urein)
                    .HasColumnType("datetime")
                    .HasColumnName("UREIN");

                entity.Property(e => e.Varede)
                    .HasColumnName("VAREDE")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Varepr)
                    .HasColumnName("VAREPR")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Vqcsrd)
                    .HasColumnName("VQCSRD")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Vqcsrp)
                    .HasColumnName("VQCSRP")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Vqssrd)
                    .HasColumnName("VQSSRD")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Vqssrp)
                    .HasColumnName("VQSSRP")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<Plantel>()
               .HasOne(p => p.Socio)
               .WithMany(s => s.Planteles)
               .HasForeignKey(t => t.Nrocri)
               .HasPrincipalKey(s => s.Scod);

            modelBuilder.Entity<Provin>(entity =>
            {
                entity.ToTable("PROVIN");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(20)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Pcod)
                    .HasMaxLength(2)
                    .HasColumnName("PCOD");
            });

            modelBuilder.Entity<Remansol>(entity =>
            {
                entity.ToTable("REMANSOL");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Anio)
                    .HasMaxLength(10)
                    .HasColumnName("ANIO");

                entity.Property(e => e.Auttoropr)
                    .HasMaxLength(10)
                    .HasColumnName("AUTTOROPR");

                entity.Property(e => e.Autvqpr)
                    .HasMaxLength(10)
                    .HasColumnName("AUTVQPR");

                entity.Property(e => e.Autvqvip)
                    .HasMaxLength(10)
                    .HasColumnName("AUTVQVIP");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Disptoropr)
                    .HasMaxLength(10)
                    .HasColumnName("DISPTOROPR");

                entity.Property(e => e.Dispvqpr)
                    .HasMaxLength(10)
                    .HasColumnName("DISPVQPR");

                entity.Property(e => e.Dispvqvip)
                    .HasMaxLength(10)
                    .HasColumnName("DISPVQVIP");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Nrodec)
                    .HasMaxLength(6)
                    .HasColumnName("NRODEC");

                entity.Property(e => e.Nroplan)
                    .HasMaxLength(5)
                    .HasColumnName("NROPLAN");

                entity.Property(e => e.Nrosol)
                    .HasMaxLength(10)
                    .HasColumnName("NROSOL");

                entity.Property(e => e.Nrosolanual)
                    .HasColumnType("int(10)")
                    .HasColumnName("nrosolanual")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Pedtoropr)
                    .HasMaxLength(10)
                    .HasColumnName("PEDTOROPR");

                entity.Property(e => e.Pedvqpr)
                    .HasMaxLength(10)
                    .HasColumnName("PEDVQPR");

                entity.Property(e => e.Pedvqvip)
                    .HasMaxLength(10)
                    .HasColumnName("PEDVQVIP");

                entity.Property(e => e.Remantoropr)
                    .HasMaxLength(10)
                    .HasColumnName("REMANTOROPR");

                entity.Property(e => e.Remanvqpr)
                    .HasMaxLength(10)
                    .HasColumnName("REMANVQPR");

                entity.Property(e => e.Remanvqvip)
                    .HasMaxLength(10)
                    .HasColumnName("REMANVQVIP");
            });

            modelBuilder.Entity<Resin1>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Scod })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("RESIN1");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Scod)
                    .HasMaxLength(6)
                    .HasColumnName("SCOD")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU");

                entity.Property(e => e.Editar)
                    .HasColumnType("int(1)")
                    .HasColumnName("editar");

                entity.Property(e => e.Emax)
                    .HasColumnType("int(11)")
                    .HasColumnName("EMAX");

                entity.Property(e => e.Emin)
                    .HasColumnType("int(11)")
                    .HasColumnName("EMIN");

                entity.Property(e => e.Epromedio)
                    .HasColumnType("int(11)")
                    .HasColumnName("EPROMEDIO");

                entity.Property(e => e.Estcod)
                    .HasMaxLength(6)
                    .HasColumnName("ESTCOD");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Freali)
                    .HasColumnType("datetime")
                    .HasColumnName("FREALI");

                entity.Property(e => e.Icod)
                    .HasMaxLength(6)
                    .HasColumnName("ICOD")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Nropla)
                    .HasMaxLength(4)
                    .HasColumnName("NROPLA");

                entity.Property(e => e.Nrores)
                    .HasMaxLength(6)
                    .HasColumnName("NRORES");

                entity.Property(e => e.Observ)
                    .HasMaxLength(255)
                    .HasColumnName("OBSERV");

                entity.Property(e => e.Ppajust)
                    .HasColumnType("int(11)")
                    .HasColumnName("PPAJUST");

                entity.Property(e => e.Torsb)
                    .HasColumnType("int(11)")
                    .HasColumnName("TORSB");

                entity.Property(e => e.Tortot)
                    .HasColumnType("int(11)")
                    .HasColumnName("TORTOT");
            });

            modelBuilder.Entity<Resin1>()
             .HasOne(t => t.Socio)
             .WithMany(s => s.Resultados)
             .HasForeignKey(t => t.Scod)
             .HasPrincipalKey(s => s.Scod);

            modelBuilder.Entity<Resin2>(entity =>
            {
                entity.ToTable("RESIN2");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Ea1)
                    .HasColumnType("int(10)")
                    .HasColumnName("EA1");

                entity.Property(e => e.Ea2).HasColumnName("EA2");

                entity.Property(e => e.Ea3).HasColumnName("EA3");

                entity.Property(e => e.Ea4).HasColumnName("EA4");

                entity.Property(e => e.Ea5).HasColumnName("EA5");

                entity.Property(e => e.Ea6).HasColumnName("EA6");

                entity.Property(e => e.Nrores)
                    .HasMaxLength(6)
                    .HasColumnName("NRORES");
            });

            modelBuilder.Entity<Resin3>(entity =>
            {
                entity.ToTable("RESIN3");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Ctomov)
                    .HasMaxLength(2)
                    .HasColumnName("CTOMOV");

                entity.Property(e => e.Nrores)
                    .HasMaxLength(6)
                    .HasColumnName("NRORES");

                entity.Property(e => e.Rdvac).HasColumnName("RDVAC");

                entity.Property(e => e.Rdvaqcs).HasColumnName("RDVAQCS");

                entity.Property(e => e.Rdvaqss).HasColumnName("RDVAQSS");

                entity.Property(e => e.Rpvac).HasColumnName("RPVAC");

                entity.Property(e => e.Rpvaqcs).HasColumnName("RPVAQCS");

                entity.Property(e => e.Rpvaqss).HasColumnName("RPVAQSS");

                entity.Property(e => e.Tipmov)
                    .HasMaxLength(1)
                    .HasColumnName("TIPMOV");
            });

            modelBuilder.Entity<Resin4>(entity =>
            {
                entity.ToTable("RESIN4");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Iedad).HasColumnName("IEDAD");

                entity.Property(e => e.Ipeso).HasColumnName("IPESO");

                entity.Property(e => e.Medad).HasColumnName("MEDAD");

                entity.Property(e => e.Mpeso).HasColumnName("MPESO");

                entity.Property(e => e.Nrores)
                    .HasMaxLength(6)
                    .HasColumnName("NRORES");

                entity.Property(e => e.P2d).HasColumnName("P2D");

                entity.Property(e => e.P4d).HasColumnName("P4D");

                entity.Property(e => e.Pdl).HasColumnName("PDL");

                entity.Property(e => e.Pedad).HasColumnName("PEDAD");

                entity.Property(e => e.Ppeso).HasColumnName("PPESO");

                entity.Property(e => e.Sexo)
                    .HasMaxLength(1)
                    .HasColumnName("SEXO");
            });

            modelBuilder.Entity<Resin6>(entity =>
            {
                entity.ToTable("RESIN6");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Hdb)
                    .HasColumnName("HDB")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Hdp)
                    .HasColumnName("HDP")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.HdpAs)
                    .HasColumnName("HDP_AS")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.HdpM)
                    .HasColumnName("HDP_M")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Hdt)
                    .HasColumnName("HDT")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Hgqb)
                    .HasColumnName("HGQB")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Hgqp)
                    .HasColumnName("HGQP")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Hgvb)
                    .HasColumnName("HGVB")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Hgvp)
                    .HasColumnName("HGVP")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Hpb)
                    .HasColumnName("HPB")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Hpp)
                    .HasColumnName("HPP")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.HppAs)
                    .HasColumnType("int(11)")
                    .HasColumnName("HPP_AS")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.HppM)
                    .HasColumnType("int(11)")
                    .HasColumnName("HPP_M")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Hpt)
                    .HasColumnName("HPT")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Mcp)
                    .HasColumnName("MCP")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.McpAs)
                    .HasColumnName("MCP_AS")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.McpM)
                    .HasColumnName("MCP_M")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Mct)
                    .HasColumnName("MCT")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Msp)
                    .HasColumnName("MSP")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.MspAs)
                    .HasColumnName("MSP_AS")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.MspM)
                    .HasColumnName("MSP_M")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Mspsb)
                    .HasColumnName("MSPSB")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Mst)
                    .HasColumnName("MST")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Nrores)
                    .HasMaxLength(6)
                    .HasColumnName("NRORES");
            });

            modelBuilder.Entity<Resin8>(entity =>
            {
                entity.ToTable("RESIN8");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.FchRealizada)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_REALIZADA");

                entity.Property(e => e.Hembras)
                    .HasColumnType("int(11)")
                    .HasColumnName("HEMBRAS");

                entity.Property(e => e.Machos)
                    .HasColumnType("int(11)")
                    .HasColumnName("MACHOS");

                entity.Property(e => e.MotivoRechazo)
                    .HasColumnType("int(11)")
                    .HasColumnName("MOTIVO_RECHAZO");

                entity.Property(e => e.Nropla)
                    .HasMaxLength(4)
                    .HasColumnName("NROPLA");

                entity.Property(e => e.Nrores)
                    .HasMaxLength(6)
                    .HasColumnName("NRORES");
            });

            modelBuilder.Entity<Resin9>(entity =>
            {
                entity.HasKey(e => e.MotivoRechazo)
                    .HasName("PRIMARY");

                entity.ToTable("RESIN9");

                entity.Property(e => e.MotivoRechazo)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("MOTIVO_RECHAZO");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(20)
                    .HasColumnName("DESCRIPCION");
            });

            modelBuilder.Entity<Retenida>(entity =>
            {
                entity.ToTable("retenidas");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Cantb).HasColumnName("CANTB");

                entity.Property(e => e.Cantor).HasColumnName("CANTOR");

                entity.Property(e => e.Cantv).HasColumnName("CANTV");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoefAutoIa)
                    .HasColumnName("COEF_AUTO_IA")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoefAutoIar)
                    .HasColumnName("COEF_AUTO_IAR")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoefAutoSn)
                    .HasColumnName("COEF_AUTO_SN")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Ctrlm)
                    .HasColumnName("CTRLM")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Ctrlu)
                    .HasColumnName("CTRLU")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Desde)
                    .HasMaxLength(10)
                    .HasColumnName("DESDE");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Fchrecepcion)
                    .HasMaxLength(10)
                    .HasColumnName("FCHRECEPCION");

                entity.Property(e => e.Fecdecl)
                    .HasMaxLength(10)
                    .HasColumnName("FECDECL");

                entity.Property(e => e.Fecret)
                    .HasMaxLength(10)
                    .HasColumnName("FECRET");

                entity.Property(e => e.Hasta)
                    .HasMaxLength(10)
                    .HasColumnName("HASTA");

                entity.Property(e => e.IaSincro)
                    .HasColumnName("IA_SINCRO")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.NrFolio)
                    .HasMaxLength(9)
                    .HasColumnName("NR_FOLIO");

                entity.Property(e => e.Nrodec)
                    .HasMaxLength(6)
                    .HasColumnName("NRODEC");

                entity.Property(e => e.Nroplan)
                    .HasMaxLength(5)
                    .HasColumnName("NROPLAN");

                entity.Property(e => e.PastillasSincro)
                    .HasColumnType("int(11)")
                    .HasColumnName("PASTILLAS_SINCRO")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Remba).HasColumnName("REMBA");

                entity.Property(e => e.Remmpr).HasColumnName("REMMPR");

                entity.Property(e => e.Rempr).HasColumnName("REMPR");

                entity.Property(e => e.Reten)
                    .HasMaxLength(1)
                    .HasColumnName("reten")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Semen)
                    .HasMaxLength(1)
                    .HasColumnName("SEMEN");

                entity.Property(e => e.Semprop)
                    .HasColumnName("SEMPROP")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Tipse)
                    .HasMaxLength(2)
                    .HasColumnName("TIPSE");
            });

            modelBuilder.Entity<Socio>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Scod })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("SOCIOS");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Scod)
                    .HasMaxLength(6)
                    .HasColumnName("SCOD")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Catego)
                    .HasMaxLength(1)
                    .HasColumnName("CATEGO");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Codpos1)
                    .HasMaxLength(50)
                    .HasColumnName("CODPOS1");

                entity.Property(e => e.Codpos2)
                    .HasMaxLength(4)
                    .HasColumnName("CODPOS2");

                entity.Property(e => e.Codpro1)
                    .HasMaxLength(2)
                    .HasColumnName("CODPRO1");

                entity.Property(e => e.Codpro2)
                    .HasMaxLength(2)
                    .HasColumnName("CODPRO2");

                entity.Property(e => e.Criador)
                    .HasMaxLength(1)
                    .HasColumnName("CRIADOR");

                entity.Property(e => e.Cuenta)
                    .HasMaxLength(11)
                    .HasColumnName("CUENTA");

                entity.Property(e => e.Diaregautog)
                    .HasMaxLength(10)
                    .HasColumnName("diaregautog");

                entity.Property(e => e.Direcc1)
                    .HasMaxLength(100)
                    .HasColumnName("DIRECC1");

                entity.Property(e => e.Direcc2)
                    .HasMaxLength(40)
                    .HasColumnName("DIRECC2");

                entity.Property(e => e.Envio)
                    .HasMaxLength(1)
                    .HasColumnName("ENVIO");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Fecing)
                    .HasColumnType("datetime")
                    .HasColumnName("FECING");

                entity.Property(e => e.Locali1)
                    .HasMaxLength(100)
                    .HasColumnName("LOCALI1");

                entity.Property(e => e.Locali2)
                    .HasMaxLength(25)
                    .HasColumnName("LOCALI2");

                entity.Property(e => e.Mail)
                    .HasMaxLength(100)
                    .HasColumnName("mail");

                entity.Property(e => e.Mailreg)
                    .HasMaxLength(101)
                    .HasColumnName("mailreg");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Ordalf)
                    .HasMaxLength(10)
                    .HasColumnName("ORDALF");

                entity.Property(e => e.Placod)
                    .HasMaxLength(4)
                    .HasColumnName("PLACOD");

                entity.Property(e => e.Posnom)
                    .HasMaxLength(33)
                    .HasColumnName("POSNOM");

                entity.Property(e => e.Prenom)
                    .HasMaxLength(33)
                    .HasColumnName("PRENOM");

                entity.Property(e => e.Telefo1)
                    .HasMaxLength(50)
                    .HasColumnName("TELEFO1");

                entity.Property(e => e.Telefo2)
                    .HasMaxLength(50)
                    .HasColumnName("TELEFO2");

                entity.Property(e => e.Vtosus)
                    .HasColumnType("datetime")
                    .HasColumnName("VTOSUS");
            });

            modelBuilder.Entity<Solici1>(entity =>
            {
                entity.ToTable("SOLICI1");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Banco)
                    .HasMaxLength(18)
                    .HasColumnName("BANCO");

                entity.Property(e => e.Cantor)
                    .HasColumnName("CANTOR")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Cantvq)
                    .HasColumnName("CANTVQ")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Canvac)
                    .HasColumnName("CANVAC")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Canvaq)
                    .HasColumnName("CANVAQ")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU");

                entity.Property(e => e.Codest)
                    .HasMaxLength(6)
                    .HasColumnName("CODEST");

                entity.Property(e => e.Ctrl1)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("CTRL1");

                entity.Property(e => e.Ctrl2)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("CTRL2");

                entity.Property(e => e.EdadMaxHem)
                    .HasColumnType("int(11)")
                    .HasColumnName("EDAD_MAX_HEM");

                entity.Property(e => e.EdadMaxMac)
                    .HasColumnType("int(11)")
                    .HasColumnName("EDAD_MAX_MAC");

                entity.Property(e => e.EdadMinHem)
                    .HasColumnType("int(11)")
                    .HasColumnName("EDAD_MIN_HEM");

                entity.Property(e => e.EdadMinMac)
                    .HasColumnType("int(11)")
                    .HasColumnName("EDAD_MIN_MAC");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Fecins)
                    .HasColumnType("datetime")
                    .HasColumnName("FECINS");

                entity.Property(e => e.Fecret)
                    .HasColumnType("datetime")
                    .HasColumnName("FECRET");

                entity.Property(e => e.Fecsol)
                    .HasColumnType("datetime")
                    .HasColumnName("FECSOL");

                entity.Property(e => e.Import).HasColumnName("IMPORT");

                entity.Property(e => e.Lugar)
                    .HasMaxLength(25)
                    .HasColumnName("LUGAR");

                entity.Property(e => e.Nrosol)
                    .HasMaxLength(6)
                    .HasColumnName("NROSOL");

                entity.Property(e => e.Produc)
                    .HasMaxLength(1)
                    .HasColumnName("PRODUC");

                entity.Property(e => e.Reinsp)
                    .HasMaxLength(1)
                    .HasColumnName("REINSP");

                entity.Property(e => e.Tyncte)
                    .HasMaxLength(20)
                    .HasColumnName("TYNCTE");
            });

            modelBuilder.Entity<Solici1>()
               .HasOne(s => s.Establecimiento)
               .WithMany(e => e.Solicitudes)
               .HasForeignKey(t => t.Codest)
               .HasPrincipalKey(s => s.Ecod);

            //modelBuilder.Entity<Solici1>()
            // .HasOne(p => p.)
            // .WithMany(s => s.Establecimientos)
            // .HasForeignKey(t => t.Codsoc)
            // .HasPrincipalKey(s => s.Scod);
            modelBuilder.Entity<Solici4>(entity =>
            {
                entity.ToTable("SOLICI4");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Codpla)
                    .HasMaxLength(5)
                    .HasColumnName("CODPLA");

                entity.Property(e => e.Nrosol)
                    .HasMaxLength(6)
                    .HasColumnName("NROSOL");
            });

            modelBuilder.Entity<Solici6>(entity =>
            {
                entity.ToTable("SOLICI6");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.IndHemMac)
                    .HasMaxLength(1)
                    .HasColumnName("IND_HEM_MAC");

                entity.Property(e => e.Nrodec)
                    .HasMaxLength(6)
                    .HasColumnName("NRODEC");

                entity.Property(e => e.Nrosol)
                    .HasMaxLength(6)
                    .HasColumnName("NROSOL");
            });

            modelBuilder.Entity<TorosRural>(entity =>
            {
                entity.ToTable("TorosRural");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("cod_usu");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("fch_usu");

                entity.Property(e => e.FecNac)
                    .HasColumnType("datetime")
                    .HasColumnName("fec_nac");

                entity.Property(e => e.Hba)
                    .HasMaxLength(10)
                    .HasColumnName("hba");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .HasColumnName("nombre");

                entity.Property(e => e.Procesado)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("procesado");

                entity.Property(e => e.Tatpart)
                    .HasMaxLength(8)
                    .HasColumnName("tatpart");

                entity.Property(e => e.TipToro)
                    .HasMaxLength(1)
                    .HasColumnName("tip_toro");
            });

            modelBuilder.Entity<Torospr>(entity =>
            {
                entity.ToTable("TOROSPR");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Activo)
                    .HasColumnName("ACTIVO")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Actualizado)
                    .HasColumnName("ACTUALIZADO")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Apodo)
                    .HasMaxLength(50)
                    .HasColumnName("APODO");

                entity.Property(e => e.Catego)
                    .HasMaxLength(1)
                    .HasColumnName("CATEGO");

                entity.Property(e => e.CircEscrotal)
                    .HasMaxLength(10)
                    .HasColumnName("CIRC_ESCROTAL");

                entity.Property(e => e.CodEstado)
                    .HasMaxLength(15)
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Criador)
                    .HasMaxLength(6)
                    .HasColumnName("CRIADOR");

                entity.Property(e => e.EstDoc)
                    .HasMaxLength(2)
                    .HasColumnName("EST_DOC");

                entity.Property(e => e.Estcod)
                    .HasMaxLength(6)
                    .HasColumnName("ESTCOD");

                entity.Property(e => e.FchBaja)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_BAJA");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA");

                entity.Property(e => e.Fecing)
                    .HasMaxLength(10)
                    .HasColumnName("FECING");

                entity.Property(e => e.Hba)
                    .HasMaxLength(10)
                    .HasColumnName("HBA");

                entity.Property(e => e.IdTipo)
                    .HasColumnType("int(11)")
                    .HasColumnName("Id_tipo")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.MotivoBaj)
                    .HasMaxLength(50)
                    .HasColumnName("MOTIVO_BAJ");

                entity.Property(e => e.Nacido)
                    .HasColumnType("datetime")
                    .HasColumnName("NACIDO");

                entity.Property(e => e.NomDad)
                    .HasMaxLength(35)
                    .HasColumnName("NOM_DAD");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.NrInsc)
                    .HasMaxLength(10)
                    .HasColumnName("NR_INSC");

                entity.Property(e => e.NrInsd)
                    .HasMaxLength(10)
                    .HasColumnName("NR_INSD");

                entity.Property(e => e.NrTsan)
                    .HasMaxLength(10)
                    .HasColumnName("NR_TSAN");

                entity.Property(e => e.Plantel)
                    .HasMaxLength(5)
                    .HasColumnName("PLANTEL");

                entity.Property(e => e.ResInsp)
                    .HasMaxLength(2)
                    .HasColumnName("RES_INSP");

                entity.Property(e => e.Sbcod)
                    .HasMaxLength(6)
                    .HasColumnName("SBCOD");

                entity.Property(e => e.Tatpart)
                    .HasMaxLength(8)
                    .HasColumnName("TATPART");

                entity.Property(e => e.TipToro)
                    .HasMaxLength(1)
                    .HasColumnName("TIP_TORO");

                entity.Property(e => e.Variedad)
                    .HasMaxLength(2)
                    .HasColumnName("VARIEDAD");
            });

            modelBuilder.Entity<Torosuni>(entity =>
            {
                entity.ToTable("TOROSUNI");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Activo)
                    .HasColumnName("ACTIVO")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Actualizado)
                    .HasColumnName("ACTUALIZADO")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Apodo)
                    .HasMaxLength(50)
                    .HasColumnName("APODO");

                entity.Property(e => e.Catego)
                    .HasMaxLength(1)
                    .HasColumnName("CATEGO");

                entity.Property(e => e.Cescrot)
                    .HasMaxLength(4)
                    .HasColumnName("cescrot");

                entity.Property(e => e.CircEscrotal)
                    .HasMaxLength(10)
                    .HasColumnName("CIRC_ESCROTAL");

                entity.Property(e => e.CodEstado)
                    .HasMaxLength(15)
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(50)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Criador)
                    .HasMaxLength(6)
                    .HasColumnName("CRIADOR");

                entity.Property(e => e.EstDoc)
                    .HasMaxLength(2)
                    .HasColumnName("EST_DOC");

                entity.Property(e => e.Estcod)
                    .HasMaxLength(6)
                    .HasColumnName("ESTCOD");

                entity.Property(e => e.FchBaja)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_BAJA");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA");

                entity.Property(e => e.Fechasba)
                    .HasMaxLength(10)
                    .HasColumnName("fechasba");

                entity.Property(e => e.Fecing)
                    .HasMaxLength(10)
                    .HasColumnName("FECING");

                entity.Property(e => e.Frame)
                    .HasMaxLength(4)
                    .HasColumnName("frame");

                entity.Property(e => e.Gdpostdest)
                    .HasMaxLength(4)
                    .HasColumnName("gdpostdest");

                entity.Property(e => e.Gdvida)
                    .HasMaxLength(4)
                    .HasColumnName("gdvida");

                entity.Property(e => e.Hba)
                    .HasMaxLength(10)
                    .HasColumnName("HBA");

                entity.Property(e => e.IdTipo)
                    .HasColumnType("int(11)")
                    .HasColumnName("Id_tipo")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Indicedest)
                    .HasMaxLength(4)
                    .HasColumnName("indicedest");

                entity.Property(e => e.Indicefinal)
                    .HasMaxLength(4)
                    .HasColumnName("indicefinal");

                entity.Property(e => e.MotivoBaj)
                    .HasMaxLength(50)
                    .HasColumnName("MOTIVO_BAJ");

                entity.Property(e => e.Nacido)
                    .HasColumnType("datetime")
                    .HasColumnName("NACIDO");

                entity.Property(e => e.NomDad)
                    .HasMaxLength(35)
                    .HasColumnName("NOM_DAD");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.NrInsc)
                    .HasMaxLength(10)
                    .HasColumnName("NR_INSC");

                entity.Property(e => e.NrInsd)
                    .HasMaxLength(10)
                    .HasColumnName("NR_INSD");

                entity.Property(e => e.NrTsan)
                    .HasMaxLength(10)
                    .HasColumnName("NR_TSAN");

                entity.Property(e => e.Otros1)
                    .HasMaxLength(4)
                    .HasColumnName("otros1");

                entity.Property(e => e.Otros2)
                    .HasMaxLength(4)
                    .HasColumnName("otros2");

                entity.Property(e => e.Pajudest)
                    .HasMaxLength(4)
                    .HasColumnName("pajudest");

                entity.Property(e => e.Pajufinal)
                    .HasMaxLength(4)
                    .HasColumnName("pajufinal");

                entity.Property(e => e.Plantel)
                    .HasMaxLength(5)
                    .HasColumnName("PLANTEL");

                entity.Property(e => e.Pnac)
                    .HasMaxLength(4)
                    .HasColumnName("pnac");

                entity.Property(e => e.Promgrupo1)
                    .HasMaxLength(4)
                    .HasColumnName("promgrupo1");

                entity.Property(e => e.Promgrupo2)
                    .HasMaxLength(4)
                    .HasColumnName("promgrupo2");

                entity.Property(e => e.ResInsp)
                    .HasMaxLength(2)
                    .HasColumnName("RES_INSP");

                entity.Property(e => e.Sbcod)
                    .HasColumnType("int(50)")
                    .HasColumnName("SBCOD");

                entity.Property(e => e.SbcodOld)
                    .HasMaxLength(50)
                    .HasColumnName("SBCOD_OLD");

                entity.Property(e => e.Tatpart)
                    .HasMaxLength(8)
                    .HasColumnName("TATPART");

                entity.Property(e => e.TipToro)
                    .HasMaxLength(1)
                    .HasColumnName("TIP_TORO");

                entity.Property(e => e.Variedad)
                    .HasMaxLength(2)
                    .HasColumnName("VARIEDAD");
            });

            modelBuilder.Entity<Torosuni>()
               .HasOne(t => t.Socio)
               .WithMany(s => s.Torosunis)
               .HasForeignKey(t => t.Criador)
               .HasPrincipalKey(s => s.Scod);

            modelBuilder.Entity<Torosuniestado>(entity =>
            {
                entity.ToTable("TOROSUNIestados");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_unicode_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(5)")
                    .HasColumnName("id");

                entity.Property(e => e.CodEstado).HasMaxLength(10);
            });

            modelBuilder.Entity<Transan>(entity =>
            {
                entity.ToTable("TRANSAN");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.CantChem)
                    .HasColumnType("int(11)")
                    .HasColumnName("CANT_CHEM");

                entity.Property(e => e.CantCmach)
                    .HasColumnType("int(11)")
                    .HasColumnName("CANT_CMACH");

                entity.Property(e => e.CantHem)
                    .HasColumnType("int(11)")
                    .HasColumnName("CANT_HEM");

                entity.Property(e => e.CantMach)
                    .HasColumnType("int(11)")
                    .HasColumnName("CANT_MACH");

                entity.Property(e => e.CategSc)
                    .HasMaxLength(1)
                    .HasColumnName("CATEG_SC");

                entity.Property(e => e.CategSv)
                    .HasMaxLength(1)
                    .HasColumnName("CATEG_SV");

                entity.Property(e => e.Cnom)
                    .HasMaxLength(100)
                    .HasColumnName("CNOM");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Fecvta)
                    .HasColumnType("datetime")
                    .HasColumnName("FECVTA");

                entity.Property(e => e.Hemsta)
                    .HasMaxLength(3)
                    .HasColumnName("HEMSTA");

                entity.Property(e => e.Incorp)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("INCORP");

                entity.Property(e => e.NroCert)
                    .HasMaxLength(10)
                    .HasColumnName("NRO_CERT");

                entity.Property(e => e.NvoPla)
                    .HasMaxLength(4)
                    .HasColumnName("NVO_PLA");

                entity.Property(e => e.Plant)
                    .HasMaxLength(4)
                    .HasColumnName("PLANT");

                entity.Property(e => e.Scom)
                    .HasMaxLength(6)
                    .HasColumnName("SCOM");

                entity.Property(e => e.Sven)
                    .HasMaxLength(6)
                    .HasColumnName("SVEN");

                entity.Property(e => e.Tipani)
                    .HasMaxLength(1)
                    .HasColumnName("TIPANI");

                entity.Property(e => e.Tiphac)
                    .HasMaxLength(4)
                    .HasColumnName("TIPHAC");

                entity.Property(e => e.Tipohem)
                    .HasMaxLength(2)
                    .HasColumnName("TIPOHEM");

                entity.Property(e => e.Vnom)
                    .HasMaxLength(100)
                    .HasColumnName("VNOM");
            });

            modelBuilder.Entity<Transem>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TRANSEM");

                entity.Property(e => e.CategSc)
                    .HasMaxLength(1)
                    .HasColumnName("CATEG_SC");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Fecvta)
                    .HasColumnType("datetime")
                    .HasColumnName("FECVTA");

                entity.Property(e => e.Hba)
                    .HasMaxLength(10)
                    .HasColumnName("HBA");

                entity.Property(e => e.NomDad)
                    .HasMaxLength(35)
                    .HasColumnName("NOM_DAD");

                entity.Property(e => e.NrDosi)
                    .HasMaxLength(10)
                    .HasColumnName("NR_DOSI");

                entity.Property(e => e.NrDosiOr)
                    .HasMaxLength(10)
                    .HasColumnName("NR_DOSI_OR");

                entity.Property(e => e.NrInsc)
                    .HasMaxLength(10)
                    .HasColumnName("NR_INSC");

                entity.Property(e => e.NrInsd)
                    .HasMaxLength(10)
                    .HasColumnName("NR_INSD");

                entity.Property(e => e.NrTsan)
                    .HasMaxLength(10)
                    .HasColumnName("NR_TSAN");

                entity.Property(e => e.NroCert)
                    .HasMaxLength(6)
                    .HasColumnName("NRO_CERT");

                entity.Property(e => e.Nrocen)
                    .HasMaxLength(50)
                    .HasColumnName("NROCEN");

                entity.Property(e => e.Nrocri)
                    .HasMaxLength(6)
                    .HasColumnName("NROCRI");

                entity.Property(e => e.Nven)
                    .HasMaxLength(25)
                    .HasColumnName("NVEN");

                entity.Property(e => e.Scod)
                    .HasMaxLength(4)
                    .HasColumnName("SCOD");

                entity.Property(e => e.Tatpart)
                    .HasMaxLength(6)
                    .HasColumnName("TATPART");

                entity.Property(e => e.TipEnv)
                    .HasMaxLength(8)
                    .HasColumnName("TIP_ENV");

                entity.Property(e => e.Variedad)
                    .HasMaxLength(2)
                    .HasColumnName("VARIEDAD");
            });

            modelBuilder.Entity<Transsb>(entity =>
            {
                entity.ToTable("TRANSSB");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.CategSc)
                    .HasMaxLength(1)
                    .HasColumnName("CATEG_SC");

                entity.Property(e => e.CategSv)
                    .HasMaxLength(1)
                    .HasColumnName("CATEG_SV");

                entity.Property(e => e.Cnom)
                    .HasMaxLength(100)
                    .HasColumnName("CNOM");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU");

                entity.Property(e => e.Ecod)
                    .HasMaxLength(6)
                    .HasColumnName("ECOD");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Fectrans)
                    .HasColumnType("datetime")
                    .HasColumnName("FECTRANS");

                entity.Property(e => e.NroOrden)
                    .HasMaxLength(255)
                    .HasColumnName("NRO_ORDEN");

                entity.Property(e => e.NroTrans)
                    .HasMaxLength(6)
                    .HasColumnName("NRO_TRANS");

                entity.Property(e => e.Scom)
                    .HasMaxLength(6)
                    .HasColumnName("SCOM");

                entity.Property(e => e.Sven)
                    .HasMaxLength(6)
                    .HasColumnName("SVEN");

                entity.Property(e => e.Vnom)
                    .HasMaxLength(100)
                    .HasColumnName("VNOM");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Dni)
                    .HasMaxLength(30)
                    .HasColumnName("DNI");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.LastNames).HasMaxLength(100);

                entity.Property(e => e.Names).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(30);

                entity.Property(e => e.Rol).HasColumnType("text");

                entity.Property(e => e.Sid)
                    .HasColumnType("int(11)")
                    .HasColumnName("SId");

                entity.Property(e => e.Status).HasMaxLength(15);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Nrousu)
                    .HasMaxLength(10)
                    .HasColumnName("nrousu");

                entity.Property(e => e.Pass)
                    .HasMaxLength(50)
                    .HasColumnName("pass");

                entity.Property(e => e.Passmd5)
                    .HasMaxLength(50)
                    .HasColumnName("passmd5");

                entity.Property(e => e.Ultimahora)
                    .HasMaxLength(20)
                    .HasColumnName("ultimahora");

                entity.Property(e => e.Ultimavis)
                    .HasMaxLength(20)
                    .HasColumnName("ultimavis");

                entity.Property(e => e.User)
                    .HasMaxLength(50)
                    .HasColumnName("user");
            });

            modelBuilder.Entity<Zona>(entity =>
            {
                entity.ToTable("ZONAS");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.CodUsu)
                    .HasColumnType("int(11)")
                    .HasColumnName("COD_USU")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Codpro)
                    .HasMaxLength(2)
                    .HasColumnName("CODPRO");

                entity.Property(e => e.FchUsu)
                    .HasColumnType("datetime")
                    .HasColumnName("FCH_USU");

                entity.Property(e => e.Inspec)
                    .HasMaxLength(6)
                    .HasColumnName("INSPEC");

                entity.Property(e => e.Locali).HasColumnName("LOCALI");

                entity.Property(e => e.Meses)
                    .HasMaxLength(50)
                    .HasColumnName("MESES");

                entity.Property(e => e.Zcod)
                    .HasMaxLength(2)
                    .HasColumnName("ZCOD");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
