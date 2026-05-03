using API.Data.Entidades.Multibarbero;
using Microsoft.EntityFrameworkCore;
using API.Data.ConfiguracionEntidades;

namespace API.Data.ConfiguracionEntidades.Multibarbero
{
    public class SolicitudAfiliacionConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SolicitudAfiliacion>().ToTable("SolicitudesAfiliacion");
            EntidadBaseConfiguracionBD<SolicitudAfiliacion>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<SolicitudAfiliacion>().Property(e => e.Estado).IsRequired();
            modelBuilder.Entity<SolicitudAfiliacion>().Property(e => e.FechaSolicitud).IsRequired();
            modelBuilder.Entity<SolicitudAfiliacion>().Property(e => e.MotivoRechazo).HasMaxLength(500);
            modelBuilder.Entity<SolicitudAfiliacion>().Property(e => e.Notas).HasMaxLength(500);

            modelBuilder.Entity<SolicitudAfiliacion>()
                .HasOne(e => e.Barbero)
                .WithMany()
                .HasForeignKey(e => e.BarberoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SolicitudAfiliacion>()
                .HasOne(e => e.Barberia)
                .WithMany()
                .HasForeignKey(e => e.BarberiaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índice para búsquedas por estado
            modelBuilder.Entity<SolicitudAfiliacion>()
                .HasIndex(e => new { e.BarberoId, e.Estado });
            
            modelBuilder.Entity<SolicitudAfiliacion>()
                .HasIndex(e => new { e.BarberiaId, e.Estado });
        }
    }
}
