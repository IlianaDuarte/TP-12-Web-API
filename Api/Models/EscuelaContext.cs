using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Api.Models;

public partial class EscuelaContext : DbContext
{
    public EscuelaContext()
    {
    }

    public EscuelaContext(DbContextOptions<EscuelaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Usuariorol> Usuariorols { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rol_pkey");

            entity.ToTable("rol");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Fechacreacion)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fechacreacion");
            entity.Property(e => e.Habilitado).HasColumnName("habilitado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuario_pkey");

            entity.ToTable("usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contrasenia)
                .HasMaxLength(50)
                .HasColumnName("contrasenia");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Fechacreacion)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fechacreacion");
            entity.Property(e => e.Habilitado).HasColumnName("habilitado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Nombreusuario)
                .HasMaxLength(50)
                .HasColumnName("nombreusuario");
        });

        modelBuilder.Entity<Usuariorol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuariorol_pkey");

            entity.ToTable("usuariorol");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Idrol).HasColumnName("idrol");
            entity.Property(e => e.Idusuario).HasColumnName("idusuario");

            entity.HasOne(d => d.IdrolNavigation).WithMany(p => p.Usuariorols)
                .HasForeignKey(d => d.Idrol)
                .HasConstraintName("fk_usuariorol_rol");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Usuariorols)
                .HasForeignKey(d => d.Idusuario)
                .HasConstraintName("fk_usuariorol_usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
