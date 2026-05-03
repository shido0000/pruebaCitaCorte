using API.Data.Entidades.Multibarbero;
using Microsoft.EntityFrameworkCore;
using API.Data.ConfiguracionEntidades;

namespace API.Data.ConfiguracionEntidades.Multibarbero
{
    public class PerfilBarberoConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PerfilBarbero>().ToTable("PerfilesBarberos");
            EntidadBaseConfiguracionBD<PerfilBarbero>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<PerfilBarbero>().Property(e => e.Biografia).HasMaxLength(500);
            modelBuilder.Entity<PerfilBarbero>().Property(e => e.ExperienciaAnios).IsRequired();
            modelBuilder.Entity<PerfilBarbero>().Property(e => e.EspecialidadesJson).HasMaxLength(1000).IsRequired();
            modelBuilder.Entity<PerfilBarbero>().Property(e => e.CalificacionPromedio).HasPrecision(3, 2);
            modelBuilder.Entity<PerfilBarbero>().Property(e => e.TotalReseñas).IsRequired();

            modelBuilder.Entity<PerfilBarbero>()
                .HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PerfilBarbero>()
                .HasOne(e => e.PerfilBarberia)
                .WithMany(e => e.PerfilesBarberos)
                .HasForeignKey(e => e.PerfilBarberiaId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
