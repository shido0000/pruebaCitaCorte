using API.Data.Entidades.Multibarbero;
using Microsoft.EntityFrameworkCore;
using API.Data.ConfiguracionEntidades;

namespace API.Data.ConfiguracionEntidades.Multibarbero
{
    public class AfiliacionBarberoConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AfiliacionBarbero>().ToTable("AfiliacionesBarberos");
            EntidadBaseConfiguracionBD<AfiliacionBarbero>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<AfiliacionBarbero>().Property(e => e.FechaAfiliacion).IsRequired();
            modelBuilder.Entity<AfiliacionBarbero>().Property(e => e.Activo).IsRequired();
            modelBuilder.Entity<AfiliacionBarbero>().Property(e => e.EsPrincipal).IsRequired();

            modelBuilder.Entity<AfiliacionBarbero>()
                .HasOne(e => e.Barbero)
                .WithMany()
                .HasForeignKey(e => e.BarberoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AfiliacionBarbero>()
                .HasOne(e => e.Barberia)
                .WithMany()
                .HasForeignKey(e => e.BarberiaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índice para búsquedas de afiliaciones activas
            modelBuilder.Entity<AfiliacionBarbero>()
                .HasIndex(e => new { e.BarberoId, e.Activo });
            
            modelBuilder.Entity<AfiliacionBarbero>()
                .HasIndex(e => new { e.BarberiaId, e.Activo });
        }
    }
}
