using API.Data.Entidades.Multibarbero;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Multibarbero
{
    public class PerfilBarberiaConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PerfilBarberia>().ToTable("PerfilesBarberias");
            EntidadBaseConfiguracionBD<PerfilBarberia>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<PerfilBarberia>().Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<PerfilBarberia>().Property(e => e.Direccion).HasMaxLength(200).IsRequired();
            modelBuilder.Entity<PerfilBarberia>().Property(e => e.Telefono).HasMaxLength(20);
            modelBuilder.Entity<PerfilBarberia>().Property(e => e.CalificacionPromedio).HasPrecision(3, 2);
            modelBuilder.Entity<PerfilBarberia>().Property(e => e.TotalReseñas).IsRequired();
            modelBuilder.Entity<PerfilBarberia>().Property(e => e.Tipo).IsRequired();

            modelBuilder.Entity<PerfilBarberia>()
                .HasOne(e => e.PlanSuscripcion)
                .WithMany()
                .HasForeignKey(e => e.PlanSuscripcionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
