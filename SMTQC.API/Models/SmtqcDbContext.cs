using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SMTQC.API.Models;

public partial class SmtqcDbContext : DbContext
{
    public SmtqcDbContext()
    {
    }

    public SmtqcDbContext(DbContextOptions<SmtqcDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alerta> Alertas { get; set; }

    public virtual DbSet<Autorizacione> Autorizaciones { get; set; }

    public virtual DbSet<FocosCalor> FocosCalors { get; set; }

    public virtual DbSet<Productore> Productores { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-5R9U5GH\\SQLEXPRESS;Database=SmtqcDb;Trusted_Connection=True;TrustServerCertificate=True;", x => x.UseNetTopologySuite());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alerta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Alertas__3214EC072315515C");

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValue("No atendida");
            entity.Property(e => e.Tipo).HasMaxLength(50);
            entity.Property(e => e.Ubicacion).HasMaxLength(200);

            entity.HasOne(d => d.FocoCalor).WithMany(p => p.Alerta)
                .HasForeignKey(d => d.FocoCalorId)
                .HasConstraintName("FK_Alertas_FocosCalor");
        });

        modelBuilder.Entity<Autorizacione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Autoriza__3214EC07812CD2DE");

            entity.Property(e => e.Codigo).HasMaxLength(20);
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValue("Activo");
            entity.Property(e => e.Geometria).HasColumnType("geometry");
            entity.Property(e => e.NombrePredio).HasMaxLength(150);
            entity.Property(e => e.Municipio).HasMaxLength(100);

            entity.HasOne(d => d.Productor).WithMany(p => p.Autorizaciones)
                .HasForeignKey(d => d.ProductorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Autorizaciones_Productores");
        });

        modelBuilder.Entity<FocosCalor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FocosCal__3214EC07D1A7B4B5");

            entity.ToTable("FocosCalor");

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.Sensor).HasMaxLength(50);
            entity.Property(e => e.Ubicacion).HasColumnType("geometry");
        });

        modelBuilder.Entity<Productore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producto__3214EC07D180F911");

            entity.Property(e => e.Comunidad).HasMaxLength(100);
            entity.Property(e => e.Documento).HasMaxLength(20);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
            
            entity.HasOne(d => d.Usuario)
                .WithOne(p => p.Productor)
                .HasForeignKey<Productore>(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Productores_Usuarios");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC07BD8AA42D");

            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Rol).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
