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

    public virtual DbSet<Establecimiento> Establecimientos { get; set; }

    public virtual DbSet<Inspectore> Inspectores { get; set; }

    public virtual DbSet<Plantele> Planteles { get; set; }

    public virtual DbSet<ResultadoInspeccion> ResultadoInspeccions { get; set; }

    public virtual DbSet<Socio> Socios { get; set; }

    public virtual DbSet<SolicitudInspeccion> SolicitudInspeccions { get; set; }

    public virtual DbSet<Toro> Toros { get; set; }

    public virtual DbSet<Transaccione> Transacciones { get; set; }

    public virtual DbSet<TransaccionesHistorica> TransaccionesHistoricas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=BlazorCrud;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Establecimiento>(entity =>
        {
            entity.ToTable("Establecimiento");

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
            entity.Property(e => e.NombreSocio)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CodPostal)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Provincia)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Inspectore>(entity =>
        {
            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Localidad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Provincia)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Plantele>(entity =>
        {
            entity.Property(e => e.FechaExistencia).HasColumnType("datetime");
            entity.Property(e => e.NroPlantel)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.NroUltInspeccion)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.PrenadasVip).HasColumnName("PrenadasVIP");
            entity.Property(e => e.Socio)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UltimaInspeccion).HasColumnType("datetime");
            entity.Property(e => e.UltimaReinspeccion).HasColumnType("datetime");
            entity.Property(e => e.VacasVip).HasColumnName("VacasVIP");
            entity.Property(e => e.VaquillNoServicioVip).HasColumnName("VaquillNoServicioVIP");
        });

        modelBuilder.Entity<ResultadoInspeccion>(entity =>
        {
            entity.ToTable("ResultadoInspeccion");

            entity.Property(e => e.AñoExistenciaPlantel).HasColumnType("datetime");
            entity.Property(e => e.Establecimiento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaDeInspeccion).HasColumnType("datetime");
            entity.Property(e => e.FechaDeSolicitud).HasColumnType("datetime");
            entity.Property(e => e.Inspector)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Localidad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreSocio)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Plantel)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Socio>(entity =>
        {
            entity.Property(e => e.CodPostal)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Domicilio)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaExistencia).HasColumnType("datetime");
            entity.Property(e => e.Localidad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Mail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UltimoPlantel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Provincia)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono2)
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
            entity.Property(e => e.FechaAutorizacion).HasColumnType("datetime");
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
