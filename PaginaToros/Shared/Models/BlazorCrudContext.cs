using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PaginaToros.Shared.Models;

public partial class BlazorCrudContext : DbContext
{
    public BlazorCrudContext()
    {
    }

    public BlazorCrudContext(DbContextOptions<BlazorCrudContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CentrosIum> CentrosIa { get; set; }

    public virtual DbSet<Certifseman> Certifsemen { get; set; }

    public virtual DbSet<EdadMesPromedioRe> EdadMesPromedioRes { get; set; }

    public virtual DbSet<Establecimiento> Establecimientos { get; set; }

    public virtual DbSet<EstadoHistoricoRe> EstadoHistoricoRes { get; set; }

    public virtual DbSet<HembrasYmachosRe> HembrasYmachosRes { get; set; }

    public virtual DbSet<InspRe> InspRes { get; set; }

    public virtual DbSet<Inspectore> Inspectores { get; set; }

    public virtual DbSet<MovimientosRe> MovimientosRes { get; set; }

    public virtual DbSet<Plantele> Planteles { get; set; }

    public virtual DbSet<RechazoRe> RechazoRes { get; set; }

    public virtual DbSet<Socio> Socios { get; set; }

    public virtual DbSet<SolicitudInspeccion> SolicitudInspeccions { get; set; }

    public virtual DbSet<Toro> Toros { get; set; }

    public virtual DbSet<Torosuni> Torosunis { get; set; }

    public virtual DbSet<Transaccione> Transacciones { get; set; }

    public virtual DbSet<TransaccionesHistorica> TransaccionesHistoricas { get; set; }

    public virtual DbSet<Transan> Transans { get; set; }

    public virtual DbSet<Transem> Transems { get; set; }

    public virtual DbSet<Transsb> Transsbs { get; set; }

    public virtual DbSet<Zona> Zonas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=JULI2KAPO\\LOCALHOST;DataBase=BlazorCrud;Trusted_Connection=True;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CentrosIum>(entity =>
        {
            entity.ToTable("CentrosIA");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CodUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COD_USU");
            entity.Property(e => e.FchUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FCH_USU");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.NroCSayg)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NRO_C_SAYG");
            entity.Property(e => e.Nrocen)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NROCEN");
        });

        modelBuilder.Entity<Certifseman>(entity =>
        {
            entity.ToTable("CERTIFSEMEN");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apodo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("APODO");
            entity.Property(e => e.CategSc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CATEG_SC");
            entity.Property(e => e.CodUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COD_USU");
            entity.Property(e => e.FchConst)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FCH_CONST");
            entity.Property(e => e.FchUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FCH_USU");
            entity.Property(e => e.Fecvta)
                .HasColumnType("datetime")
                .HasColumnName("FECVTA");
            entity.Property(e => e.Fecvtatemp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FECVTATEMP");
            entity.Property(e => e.Hba)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("HBA");
            entity.Property(e => e.NomDad)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("NOM_DAD");
            entity.Property(e => e.NombreCentro)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreSocio)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NrDosi)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NR_DOSI");
            entity.Property(e => e.NrDosiOr)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NR_DOSI_OR");
            entity.Property(e => e.NrInsc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NR_INSC");
            entity.Property(e => e.NrInsd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NR_INSD");
            entity.Property(e => e.NrTsan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NR_TSAN");
            entity.Property(e => e.NroCert)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NRO_CERT");
            entity.Property(e => e.NroConst)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NRO_CONST");
            entity.Property(e => e.Nrocen)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NROCEN");
            entity.Property(e => e.Nrocri).HasColumnName("NROCRI");
            entity.Property(e => e.Nven)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NVEN");
            entity.Property(e => e.Scod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SCOD");
            entity.Property(e => e.Tatpart)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TATPART");
            entity.Property(e => e.TipEnv)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TIP_ENV");
            entity.Property(e => e.TipoCert)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TIPO_CERT");
            entity.Property(e => e.Variedad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VARIEDAD");
        });

        modelBuilder.Entity<EdadMesPromedioRe>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Iedad)
                .HasDefaultValueSql("((0))")
                .HasColumnName("IEDAD");
            entity.Property(e => e.Ipeso)
                .HasDefaultValueSql("((0))")
                .HasColumnName("IPESO");
            entity.Property(e => e.Medad)
                .HasDefaultValueSql("((0))")
                .HasColumnName("MEDAD");
            entity.Property(e => e.Mpeso)
                .HasDefaultValueSql("((0))")
                .HasColumnName("MPESO");
            entity.Property(e => e.Nrores).HasColumnName("NRORES");
            entity.Property(e => e.P2d)
                .HasDefaultValueSql("((0))")
                .HasColumnName("P2D");
            entity.Property(e => e.P4d)
                .HasDefaultValueSql("((0))")
                .HasColumnName("P4D");
            entity.Property(e => e.Pdl)
                .HasDefaultValueSql("((0))")
                .HasColumnName("PDL");
            entity.Property(e => e.Pedad)
                .HasDefaultValueSql("((0))")
                .HasColumnName("PEDAD");
            entity.Property(e => e.Ppeso)
                .HasDefaultValueSql("((0))")
                .HasColumnName("PPESO");
            entity.Property(e => e.Sexo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("SEXO");
        });

        modelBuilder.Entity<Establecimiento>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Establecimiento");

            entity.Property(e => e.CodPostal)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CodProvincia)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Domicilio)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Encargado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaExistencia).HasColumnType("datetime");
            entity.Property(e => e.Informacion).IsUnicode(false);
            entity.Property(e => e.Localidad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreSocio).IsUnicode(false);
            entity.Property(e => e.Provincia)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EstadoHistoricoRe>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ea1)
                .HasDefaultValueSql("((0))")
                .HasColumnName("EA1");
            entity.Property(e => e.Ea2)
                .HasDefaultValueSql("((0))")
                .HasColumnName("EA2");
            entity.Property(e => e.Ea3)
                .HasDefaultValueSql("((0))")
                .HasColumnName("EA3");
            entity.Property(e => e.Ea4)
                .HasDefaultValueSql("((0))")
                .HasColumnName("EA4");
            entity.Property(e => e.Ea5)
                .HasDefaultValueSql("((0))")
                .HasColumnName("EA5");
            entity.Property(e => e.Ea6)
                .HasDefaultValueSql("((0))")
                .HasColumnName("EA6");
            entity.Property(e => e.Nrores).HasColumnName("NRORES");
        });

        modelBuilder.Entity<HembrasYmachosRe>(entity =>
        {
            entity.ToTable("HembrasYMachosRes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Hdb)
                .HasDefaultValueSql("((0))")
                .HasColumnName("HDB");
            entity.Property(e => e.Hdp)
                .HasDefaultValueSql("((0))")
                .HasColumnName("HDP");
            entity.Property(e => e.HdpAs)
                .HasDefaultValueSql("((0))")
                .HasColumnName("HDP_AS");
            entity.Property(e => e.HdpM)
                .HasDefaultValueSql("((0))")
                .HasColumnName("HDP_M");
            entity.Property(e => e.Hdt)
                .HasDefaultValueSql("((0))")
                .HasColumnName("HDT");
            entity.Property(e => e.Hgqb)
                .HasDefaultValueSql("((0))")
                .HasColumnName("HGQB");
            entity.Property(e => e.Hgqp)
                .HasDefaultValueSql("((0))")
                .HasColumnName("HGQP");
            entity.Property(e => e.Hgvb)
                .HasDefaultValueSql("((0))")
                .HasColumnName("HGVB");
            entity.Property(e => e.Hgvp)
                .HasDefaultValueSql("((0))")
                .HasColumnName("HGVP");
            entity.Property(e => e.Hpb)
                .HasDefaultValueSql("((0))")
                .HasColumnName("HPB");
            entity.Property(e => e.Hpp)
                .HasDefaultValueSql("((0))")
                .HasColumnName("HPP");
            entity.Property(e => e.HppAs)
                .HasDefaultValueSql("((0))")
                .HasColumnName("HPP_AS");
            entity.Property(e => e.HppM)
                .HasDefaultValueSql("((0))")
                .HasColumnName("HPP_M");
            entity.Property(e => e.Hpt)
                .HasDefaultValueSql("((0))")
                .HasColumnName("HPT");
            entity.Property(e => e.Mcp)
                .HasDefaultValueSql("((0))")
                .HasColumnName("MCP");
            entity.Property(e => e.McpAs)
                .HasDefaultValueSql("((0))")
                .HasColumnName("MCP_AS");
            entity.Property(e => e.McpM)
                .HasDefaultValueSql("((0))")
                .HasColumnName("MCP_M");
            entity.Property(e => e.Mct)
                .HasDefaultValueSql("((0))")
                .HasColumnName("MCT");
            entity.Property(e => e.Msp)
                .HasDefaultValueSql("((0))")
                .HasColumnName("MSP");
            entity.Property(e => e.MspAs)
                .HasDefaultValueSql("((0))")
                .HasColumnName("MSP_AS");
            entity.Property(e => e.MspM)
                .HasDefaultValueSql("((0))")
                .HasColumnName("MSP_M");
            entity.Property(e => e.Mspsb)
                .HasDefaultValueSql("((0))")
                .HasColumnName("MSPSB");
            entity.Property(e => e.Mst)
                .HasDefaultValueSql("((0))")
                .HasColumnName("MST");
            entity.Property(e => e.Nrores).HasColumnName("NRORES");
        });

        modelBuilder.Entity<InspRe>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CodUsu).HasColumnName("COD_USU");
            entity.Property(e => e.Editar).HasColumnName("editar");
            entity.Property(e => e.Emax).HasColumnName("EMAX");
            entity.Property(e => e.Emin).HasColumnName("EMIN");
            entity.Property(e => e.Epromedio).HasColumnName("EPROMEDIO");
            entity.Property(e => e.Estcod).HasColumnName("ESTCOD");
            entity.Property(e => e.FchUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FCH_USU");
            entity.Property(e => e.FechaInspeccion).HasColumnType("datetime");
            entity.Property(e => e.Icod).HasColumnName("ICOD");
            entity.Property(e => e.NombreSocio)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nropla)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("NROPLA");
            entity.Property(e => e.Nrores).HasColumnName("NRORES");
            entity.Property(e => e.Observ)
                .HasMaxLength(267)
                .IsUnicode(false)
                .HasColumnName("OBSERV");
            entity.Property(e => e.Ppajust).HasColumnName("PPAJUST");
            entity.Property(e => e.Scod).HasColumnName("SCOD");
            entity.Property(e => e.Torsb).HasColumnName("TORSB");
            entity.Property(e => e.Tortot).HasColumnName("TORTOT");
        });

        modelBuilder.Entity<Inspectore>(entity =>
        {
            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Localidad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Mail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Provincia)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MovimientosRe>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ctomov)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CTOMOV");
            entity.Property(e => e.Nrores).HasColumnName("NRORES");
            entity.Property(e => e.Rdvac)
                .HasDefaultValueSql("((0))")
                .HasColumnName("RDVAC");
            entity.Property(e => e.Rdvaqcs)
                .HasDefaultValueSql("((0))")
                .HasColumnName("RDVAQCS");
            entity.Property(e => e.Rdvaqss)
                .HasDefaultValueSql("((0))")
                .HasColumnName("RDVAQSS");
            entity.Property(e => e.Rpvac)
                .HasDefaultValueSql("((0))")
                .HasColumnName("RPVAC");
            entity.Property(e => e.Rpvaqcs)
                .HasDefaultValueSql("((0))")
                .HasColumnName("RPVAQCS");
            entity.Property(e => e.Rpvaqss)
                .HasDefaultValueSql("((0))")
                .HasColumnName("RPVAQSS");
            entity.Property(e => e.Tipmov)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TIPMOV");
        });

        modelBuilder.Entity<Plantele>(entity =>
        {
            entity.Property(e => e.Comentarios)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaExistencia).HasColumnType("datetime");
            entity.Property(e => e.NombreSocio)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NroPlantel)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.NroUltInspeccion)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.PrenadasVip).HasColumnName("PrenadasVIP");
            entity.Property(e => e.UltimaInspeccion).HasColumnType("datetime");
            entity.Property(e => e.UltimaReinspeccion).HasColumnType("datetime");
            entity.Property(e => e.VacasVip).HasColumnName("VacasVIP");
            entity.Property(e => e.VaquillNoServicioVip).HasColumnName("VaquillNoServicioVIP");
        });

        modelBuilder.Entity<RechazoRe>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FchRealizada)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FCH_REALIZADA");
            entity.Property(e => e.Hembras)
                .HasDefaultValueSql("((0))")
                .HasColumnName("HEMBRAS");
            entity.Property(e => e.Machos)
                .HasDefaultValueSql("((0))")
                .HasColumnName("MACHOS");
            entity.Property(e => e.MotivoRechazo).HasColumnName("MOTIVO_RECHAZO");
            entity.Property(e => e.Nropla)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("NROPLA");
            entity.Property(e => e.Nrores).HasColumnName("NRORES");
        });

        modelBuilder.Entity<Socio>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CodPostal)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Domicilio).IsUnicode(false);
            entity.Property(e => e.FechaExistencia).HasColumnType("datetime");
            entity.Property(e => e.Localidad).IsUnicode(false);
            entity.Property(e => e.Mail).IsUnicode(false);
            entity.Property(e => e.NombreCompleto).IsUnicode(false);
            entity.Property(e => e.Postnom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("POSTNOM");
            entity.Property(e => e.Prenom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PRENOM");
            entity.Property(e => e.Provincia)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UltimoPlantel)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SolicitudInspeccion>(entity =>
        {
            entity.ToTable("SolicitudInspeccion");

            entity.Property(e => e.Ano).HasColumnType("datetime");
            entity.Property(e => e.Establecimiento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaAuTemp)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaAutorizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaSolTemp)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");
            entity.Property(e => e.Max)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Min)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreSocio)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ToroPr)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ToroPR");
            entity.Property(e => e.VcPr)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VcPR");
            entity.Property(e => e.VcVip)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VcVIP");
            entity.Property(e => e.VqVip)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Toro>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreEst)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Torosuni>(entity =>
        {
            entity.ToTable("TOROSUNI");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("ACTIVO");
            entity.Property(e => e.Actualizado).HasColumnName("ACTUALIZADO");
            entity.Property(e => e.Apodo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("APODO");
            entity.Property(e => e.Catego)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CATEGO");
            entity.Property(e => e.Cescrot)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cescrot");
            entity.Property(e => e.CircEscrotal)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CIRC_ESCROTAL");
            entity.Property(e => e.CodEstado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CodUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COD_USU");
            entity.Property(e => e.Comentario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COMENTARIO");
            entity.Property(e => e.Criador).HasColumnName("CRIADOR");
            entity.Property(e => e.EstDoc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EST_DOC");
            entity.Property(e => e.Estcod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ESTCOD");
            entity.Property(e => e.FchBaja)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FCH_BAJA");
            entity.Property(e => e.FchUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FCH_USU");
            entity.Property(e => e.Fecha)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FECHA");
            entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");
            entity.Property(e => e.Fechasba)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fechasba");
            entity.Property(e => e.Fecing)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FECING");
            entity.Property(e => e.Frame)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("frame");
            entity.Property(e => e.Gdpostdest)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("gdpostdest");
            entity.Property(e => e.Gdvida)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("gdvida");
            entity.Property(e => e.Hba)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("HBA");
            entity.Property(e => e.IdTipo).HasColumnName("Id_tipo");
            entity.Property(e => e.Indicedest)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("indicedest");
            entity.Property(e => e.Indicefinal)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("indicefinal");
            entity.Property(e => e.MotivoBaj)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MOTIVO_BAJ");
            entity.Property(e => e.Nacido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NACIDO");
            entity.Property(e => e.NomDad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOM_DAD");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.NombreSocio)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NrInsc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NR_INSC");
            entity.Property(e => e.NrInsd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NR_INSD");
            entity.Property(e => e.NrTsan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NR_TSAN");
            entity.Property(e => e.Otros1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("otros1");
            entity.Property(e => e.Otros2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("otros2");
            entity.Property(e => e.Pajudest)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pajudest");
            entity.Property(e => e.Pajufinal)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pajufinal");
            entity.Property(e => e.Plantel)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PLANTEL");
            entity.Property(e => e.Pnac)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pnac");
            entity.Property(e => e.Promgrupo1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("promgrupo1");
            entity.Property(e => e.Promgrupo2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("promgrupo2");
            entity.Property(e => e.ResInsp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RES_INSP");
            entity.Property(e => e.Sbcod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SBCOD");
            entity.Property(e => e.SbcodOld)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SBCOD_OLD");
            entity.Property(e => e.Tatpart)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TATPART");
            entity.Property(e => e.TipToro)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TIP_TORO");
            entity.Property(e => e.Variedad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VARIEDAD");
        });

        modelBuilder.Entity<Transaccione>(entity =>
        {
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.NombreComprador)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreVendedor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Toros).HasColumnType("text");
        });

        modelBuilder.Entity<TransaccionesHistorica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TRA");
        });

        modelBuilder.Entity<Transan>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TRANSAN");

            entity.Property(e => e.CantChem)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CANT_CHEM");
            entity.Property(e => e.CantCmach)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CANT_CMACH");
            entity.Property(e => e.CantHem)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CANT_HEM");
            entity.Property(e => e.CantMach)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CANT_MACH");
            entity.Property(e => e.CategSc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CATEG_SC");
            entity.Property(e => e.CategSv)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CATEG_SV");
            entity.Property(e => e.Cnom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CNOM");
            entity.Property(e => e.CodUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COD_USU");
            entity.Property(e => e.FchUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FCH_USU");
            entity.Property(e => e.Fecvta)
                .HasColumnType("datetime")
                .HasColumnName("FECVTA");
            entity.Property(e => e.Hemsta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("HEMSTA");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.Incorp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("INCORP");
            entity.Property(e => e.NroCert)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NRO_CERT");
            entity.Property(e => e.NvoPla)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NVO_PLA");
            entity.Property(e => e.Plant)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PLANT");
            entity.Property(e => e.Scom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SCOM");
            entity.Property(e => e.Sven)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SVEN");
            entity.Property(e => e.Tipani)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TIPANI");
            entity.Property(e => e.Tiphac)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TIPHAC");
            entity.Property(e => e.Tipohem)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TIPOHEM");
            entity.Property(e => e.Vnom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VNOM");
        });

        modelBuilder.Entity<Transem>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TRANSEM");

            entity.Property(e => e.CategSc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CATEG_SC");
            entity.Property(e => e.CodUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COD_USU");
            entity.Property(e => e.FchUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FCH_USU");
            entity.Property(e => e.Fecvta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FECVTA");
            entity.Property(e => e.Hba)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("HBA");
            entity.Property(e => e.NomDad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOM_DAD");
            entity.Property(e => e.NrDosi)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NR_DOSI");
            entity.Property(e => e.NrDosiOr)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NR_DOSI_OR");
            entity.Property(e => e.NrInsc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NR_INSC");
            entity.Property(e => e.NrInsd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NR_INSD");
            entity.Property(e => e.NrTsan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NR_TSAN");
            entity.Property(e => e.NroCert)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NRO_CERT");
            entity.Property(e => e.Nrocen)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NROCEN");
            entity.Property(e => e.Nrocri)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NROCRI");
            entity.Property(e => e.Nven)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NVEN");
            entity.Property(e => e.Scod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SCOD");
            entity.Property(e => e.Tatpart)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TATPART");
            entity.Property(e => e.TipEnv)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TIP_ENV");
            entity.Property(e => e.Variedad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VARIEDAD");
        });

        modelBuilder.Entity<Transsb>(entity =>
        {
            entity.ToTable("TRANSSB");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategSc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CATEG_SC");
            entity.Property(e => e.CategSv)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CATEG_SV");
            entity.Property(e => e.Cnom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CNOM");
            entity.Property(e => e.CodUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COD_USU");
            entity.Property(e => e.Ecod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ECOD");
            entity.Property(e => e.FchUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FCH_USU");
            entity.Property(e => e.Fectrans)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FECTRANS");
            entity.Property(e => e.NombreEstablecimiento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NroOrden)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NRO_ORDEN");
            entity.Property(e => e.NroTrans)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NRO_TRANS");
            entity.Property(e => e.Scom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SCOM");
            entity.Property(e => e.Sven)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SVEN");
            entity.Property(e => e.Vnom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VNOM");
        });

        modelBuilder.Entity<Zona>(entity =>
        {
            entity.Property(e => e.CodigoInspector)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Localidad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Meses)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreInspector)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Provincia)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
