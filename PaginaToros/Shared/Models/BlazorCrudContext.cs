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

    public virtual DbSet<Toro> Toros { get; set; }

    public virtual DbSet<Transaccione> Transacciones { get; set; }

    public virtual DbSet<TransaccionesHistorica> TransaccionesHistoricas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\MSSQLSERVER03;Database=BlazorCrud;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Establecimiento>(entity =>
        {
            entity.ToTable("Establecimiento");

            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Nombre)
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
