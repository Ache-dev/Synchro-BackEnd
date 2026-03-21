using ApiSynchro.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiSynchro.Data
{
    public class SynchroDbContext : DbContext
    {
        public SynchroDbContext(DbContextOptions<SynchroDbContext> options) : base(options)
        {
        }

        public DbSet<IntencionBusqueda> IntencionBusquedas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Sesion> Sesiones { get; set; }
        public DbSet<PreguntaEncuesta> PreguntasEncuesta { get; set; }
        public DbSet<RespuestaEncuesta> RespuestasEncuesta { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de IntencionBusqueda
            modelBuilder.Entity<IntencionBusqueda>(entity =>
            {
                entity.HasKey(e => e.IdIntencion);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.NombreEN).HasMaxLength(100);
            });

            // Configuración de Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Contrasena).IsRequired().HasMaxLength(250);
                entity.Property(e => e.FechaNacimiento).IsRequired();
                entity.Property(e => e.Estado).HasDefaultValue(true);

                // Relación con IntencionBusqueda
                entity.HasOne(e => e.IntencionBusquedaNavigation)
                    .WithMany(i => i.Usuarios)
                    .HasForeignKey(e => e.IntencionBusqueda)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configuración de Sesion
            modelBuilder.Entity<Sesion>(entity =>
            {
                entity.HasKey(e => e.IdSesion);

                entity.HasOne(e => e.Usuario)
                    .WithMany(u => u.Sesiones)
                    .HasForeignKey(e => e.IdUsuario)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de PreguntaEncuesta
            modelBuilder.Entity<PreguntaEncuesta>(entity =>
            {
                entity.HasKey(e => e.IdPregunta);
                entity.Property(e => e.TextoPregunta).IsRequired().HasMaxLength(500);
                entity.Property(e => e.TextoPreguntaEN).HasMaxLength(500);
                entity.Property(e => e.Icono).HasMaxLength(50);
            });

            // Configuración de RespuestaEncuesta
            modelBuilder.Entity<RespuestaEncuesta>(entity =>
            {
                entity.HasKey(e => e.IdRespuesta);

                entity.HasOne(e => e.Usuario)
                    .WithMany(u => u.Respuestas)
                    .HasForeignKey(e => e.IdUsuario)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Pregunta)
                    .WithMany(p => p.Respuestas)
                    .HasForeignKey(e => e.IdPregunta)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de Match
            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasKey(e => e.IdMatch);
                entity.Property(e => e.Estado).HasDefaultValue(true);

                entity.HasOne(e => e.Usuario1)
                    .WithMany(u => u.MatchesComoUsuario1)
                    .HasForeignKey(e => e.IdUsuario1)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Usuario2)
                    .WithMany(u => u.MatchesComoUsuario2)
                    .HasForeignKey(e => e.IdUsuario2)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de Mensaje
            modelBuilder.Entity<Mensaje>(entity =>
            {
                entity.HasKey(e => e.IdMensaje);
                entity.Property(e => e.EstadoLeido).HasDefaultValue(false);

                entity.HasOne(e => e.Match)
                    .WithMany(m => m.Mensajes)
                    .HasForeignKey(e => e.IdMatch)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Remitente)
                    .WithMany(u => u.MensajesEnviados)
                    .HasForeignKey(e => e.IdRemitente)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Destinatario)
                    .WithMany(u => u.MensajesRecibidos)
                    .HasForeignKey(e => e.IdDestinatario)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
