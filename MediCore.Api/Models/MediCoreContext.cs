using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Models;

public partial class MediCoreContext : DbContext
{
    public MediCoreContext()
    {
    }

    public MediCoreContext(DbContextOptions<MediCoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CitaMedica> CitaMedicas { get; set; }

    public virtual DbSet<Clinica> Clinicas { get; set; }

    public virtual DbSet<Cobro> Cobros { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CitaMedica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CitaMedi__3214EC0765513AB0");

            entity.ToTable("CitaMedica");

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValue("Programada");
            entity.Property(e => e.FechaHora).HasColumnType("datetime");
            entity.Property(e => e.MotivoConsulta).HasMaxLength(255);

            entity.HasOne(d => d.Clinica).WithMany(p => p.CitaMedicas)
                .HasForeignKey(d => d.ClinicaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CitaMedic__Clini__5629CD9C");

            entity.HasOne(d => d.Medico).WithMany(p => p.CitaMedicas)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CitaMedic__Medic__5812160E");

            entity.HasOne(d => d.Paciente).WithMany(p => p.CitaMedicas)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CitaMedic__Pacie__571DF1D5");
        });

        modelBuilder.Entity<Clinica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clinica__3214EC07F5317F80");

            entity.ToTable("Clinica");

            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(150);
            entity.Property(e => e.Rfc).HasMaxLength(20);
        });

        modelBuilder.Entity<Cobro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cobro__3214EC074C1F252D");

            entity.ToTable("Cobro");

            entity.Property(e => e.EstadoPago)
                .HasMaxLength(50)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaEmision)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.CitaMedica).WithMany(p => p.Cobros)
                .HasForeignKey(d => d.CitaMedicaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cobro__CitaMedic__5DCAEF64");

            entity.HasOne(d => d.Clinica).WithMany(p => p.Cobros)
                .HasForeignKey(d => d.ClinicaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cobro__ClinicaId__5CD6CB2B");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Paciente__3214EC07C5754FA2");

            entity.ToTable("Paciente");

            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.Telefono).HasMaxLength(20);

            entity.HasOne(d => d.Clinica).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.ClinicaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Paciente__Clinic__52593CB8");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC072E784A31");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D10534037411F6").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Rol).HasMaxLength(50);

            entity.HasOne(d => d.Clinica).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.ClinicaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__Clinica__4F7CD00D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
