using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Context;

public partial class MasterContext : DbContext
{
    public MasterContext()
    {
    }

    public MasterContext(DbContextOptions<MasterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Medicament> Medicaments { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=master;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.IdDoctor).HasName("PK__Doctor__F838DB3ED2B90B43");

            entity.ToTable("Doctor");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
        });

        modelBuilder.Entity<Medicament>(entity =>
        {
            entity.HasKey(e => e.IdMedicament).HasName("PK__Medicame__0C2328E67D73D2B2");

            entity.ToTable("Medicament");

            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(100);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.IdPatient).HasName("PK__Patient__B7E7B5A4B2EDAD4F");

            entity.ToTable("Patient");

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.IdPrescription).HasName("PK__Prescrip__FE1EDDF570717EC5");

            entity.ToTable("Prescription");

            entity.HasOne(d => d.IdDoctorNavigation).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.IdDoctor)
                .HasConstraintName("FK__Prescript__IdDoc__39AD8A7F");

            entity.HasOne(d => d.IdPatientNavigation).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.IdPatient)
                .HasConstraintName("FK__Prescript__IdPat__38B96646");
        });

        modelBuilder.Entity<PrescriptionMedicament>(entity =>
        {
            entity.HasKey(e => new { e.IdMedicament, e.IdPrescription }).HasName("PK__Prescrip__43C2C539E87A36AA");

            entity.ToTable("Prescription_Medicament");

            entity.Property(e => e.Details).HasMaxLength(100);

            entity.HasOne(d => d.IdMedicamentNavigation).WithMany(p => p.PrescriptionMedicaments)
                .HasForeignKey(d => d.IdMedicament)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prescript__IdMed__3C89F72A");

            entity.HasOne(d => d.IdPrescriptionNavigation).WithMany(p => p.PrescriptionMedicaments)
                .HasForeignKey(d => d.IdPrescription)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prescript__IdPre__3D7E1B63");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
