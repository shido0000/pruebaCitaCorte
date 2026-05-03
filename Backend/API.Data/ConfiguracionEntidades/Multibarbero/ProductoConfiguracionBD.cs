using API.Data.Entidades.Multibarbero;
using Microsoft.EntityFrameworkCore;
using API.Data.ConfiguracionEntidades;

namespace API.Data.ConfiguracionEntidades.Multibarbero
{
    public class ProductoConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>().ToTable("Productos");
            EntidadBaseConfiguracionBD<Producto>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Producto>().Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Producto>().Property(e => e.Descripcion).HasMaxLength(500).IsRequired();
            modelBuilder.Entity<Producto>().Property(e => e.Precio).IsRequired();
            modelBuilder.Entity<Producto>().Property(e => e.Stock).IsRequired();
            modelBuilder.Entity<Producto>().Property(e => e.Activo).IsRequired();
            modelBuilder.Entity<Producto>().Property(e => e.PerfilBarberoId).IsRequired();

            modelBuilder.Entity<Producto>()
                .HasOne(e => e.CategoriaProducto)
                .WithMany(c => c.Productos)
                .HasForeignKey(e => e.CategoriaProductoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Producto>()
                .HasOne(e => e.PerfilBarbero)
                .WithMany()
                .HasForeignKey(e => e.PerfilBarberoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índice para búsquedas de productos por barbero
            modelBuilder.Entity<Producto>()
                .HasIndex(e => e.PerfilBarberoId);
            
            // Índice para búsquedas de productos activos
            modelBuilder.Entity<Producto>()
                .HasIndex(e => new { e.PerfilBarberoId, e.Activo });
        }
    }
}
