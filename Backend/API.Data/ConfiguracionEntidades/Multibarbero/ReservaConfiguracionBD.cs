using API.Data.Entidades.Multibarbero;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Multibarbero
{
    public class ReservaConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reserva>().ToTable("Reservas");
            EntidadBaseConfiguracionBD<Reserva>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Reserva>().Property(e => e.FechaHoraInicio).IsRequired();
            modelBuilder.Entity<Reserva>().Property(e => e.FechaHoraFin).IsRequired();
            modelBuilder.Entity<Reserva>().Property(e => e.Estado).IsRequired();
            modelBuilder.Entity<Reserva>().Property(e => e.NotasCliente).HasMaxLength(500);
            modelBuilder.Entity<Reserva>().Property(e => e.NotasProveedor).HasMaxLength(500);
            modelBuilder.Entity<Reserva>().Property(e => e.MotivoRechazo).HasMaxLength(500);
            modelBuilder.Entity<Reserva>().Property(e => e.FechaSolicitud).IsRequired();
            modelBuilder.Entity<Reserva>().Property(e => e.PrecioTotal).HasPrecision(10, 2);
            modelBuilder.Entity<Reserva>().Property(e => e.MetodoPago).HasMaxLength(50);
            modelBuilder.Entity<Reserva>().Property(e => e.EstadoPago).HasMaxLength(50);

            modelBuilder.Entity<Reserva>()
                .HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasOne(e => e.Servicio)
                .WithMany()
                .HasForeignKey(e => e.ServicioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índice para evitar solapamientos
            modelBuilder.Entity<Reserva>()
                .HasIndex(e => new { e.ProveedorId, e.FechaHoraInicio, e.FechaHoraFin });
        }
    }
}
