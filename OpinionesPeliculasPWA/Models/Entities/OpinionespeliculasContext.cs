using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace OpinionesPeliculasPWA.Models.Entities;

public partial class OpinionespeliculasContext : DbContext
{
    public OpinionespeliculasContext()
    {
    }

    public OpinionespeliculasContext(DbContextOptions<OpinionespeliculasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Opiniones> Opiniones { get; set; }

    public virtual DbSet<Peliculas> Peliculas { get; set; }

    public virtual DbSet<Tokens> Tokens { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Opiniones>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("opiniones");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdPelicula).HasMaxLength(45);
            entity.Property(e => e.IdUsuario).HasColumnType("int(11)");
            entity.Property(e => e.Opinion).HasColumnType("text");
        });

        modelBuilder.Entity<Peliculas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("peliculas");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Nombre).HasMaxLength(500);
        });

        modelBuilder.Entity<Tokens>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tokens");

            entity.HasIndex(e => e.IdUsuario, "fktokenus_idx");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdUsuario).HasColumnType("int(11)");
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(255);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fktokenus");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Contrasena).HasMaxLength(128);
            entity.Property(e => e.Correo).HasMaxLength(255);
            entity.Property(e => e.IdRol).HasColumnType("int(11)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
