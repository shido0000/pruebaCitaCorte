using API.Data.Entidades.Multibarbero;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Multibarbero
{
    public class NotificacionConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notificacion>().ToTable("Notificaciones");
            EntidadBaseConfiguracionBD<Notificacion>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Notificacion>().Property(e => e.UsuarioDestinoId).IsRequired();
            modelBuilder.Entity<Notificacion>().Property(e => e.Tipo).IsRequired();
            modelBuilder.Entity<Notificacion>().Property(e => e.Titulo).HasMaxLength(200).IsRequired();
            modelBuilder.Entity<Notificacion>().Property(e => e.Mensaje).HasMaxLength(1000).IsRequired();
            modelBuilder.Entity<Notificacion>().Property(e => e.Leida).IsRequired();
            modelBuilder.Entity<Notificacion>().Property(e => e.AccionRequerida).IsRequired();

            modelBuilder.Entity<Notificacion>()
                .HasOne(e => e.UsuarioDestino)
                .WithMany()
                .HasForeignKey(e => e.UsuarioDestinoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índice para búsquedas de notificaciones no leídas por usuario
            modelBuilder.Entity<Notificacion>()
                .HasIndex(e => new { e.UsuarioDestinoId, e.Leida });

            // Índice para notificaciones con acción requerida
            modelBuilder.Entity<Notificacion>()
                .HasIndex(e => new { e.UsuarioDestinoId, e.AccionRequerida });
        }
    }
}
