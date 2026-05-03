using API.Data.Entidades.Multibarbero;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Multibarbero
{
    public class ServicioConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Servicio>().ToTable("Servicios");
            EntidadBaseConfiguracionBD<Servicio>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Servicio>().Property(e => e.CategoriaServicioId).IsRequired();
            modelBuilder.Entity<Servicio>().Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Servicio>().Property(e => e.Descripcion).HasMaxLength(500).IsRequired();
            modelBuilder.Entity<Servicio>().Property(e => e.Precio).IsRequired();
            modelBuilder.Entity<Servicio>().Property(e => e.DuracionMinutos).IsRequired();
            modelBuilder.Entity<Servicio>().Property(e => e.PerfilBarberoId).IsRequired();

            modelBuilder.Entity<Servicio>()
                .HasOne(e => e.CategoriaServicio)
                .WithMany(c => c.Servicios)
                .HasForeignKey(e => e.CategoriaServicioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Servicio>()
                .HasOne(e => e.PerfilBarbero)
                .WithMany()
                .HasForeignKey(e => e.PerfilBarberoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índice para búsquedas de servicios por barbero
            modelBuilder.Entity<Servicio>()
                .HasIndex(e => e.PerfilBarberoId);

            // Índice para búsquedas de servicios por categoría
            modelBuilder.Entity<Servicio>()
                .HasIndex(e => e.CategoriaServicioId);
        }
    }
}
