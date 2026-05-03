using API.Data.Entidades.Multibarbero;
using Microsoft.EntityFrameworkCore;
using API.Data.ConfiguracionEntidades;
using API.Data.Enum.Multibarbero;

namespace API.Data.ConfiguracionEntidades.Multibarbero
{
    public class PlanSuscripcionConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlanSuscripcion>().ToTable("PlanesSuscripcion");
            EntidadBaseConfiguracionBD<PlanSuscripcion>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<PlanSuscripcion>().Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<PlanSuscripcion>().Property(e => e.Descripcion).HasMaxLength(500).IsRequired();
            modelBuilder.Entity<PlanSuscripcion>().Property(e => e.Precio).IsRequired();
            modelBuilder.Entity<PlanSuscripcion>().Property(e => e.DuracionDias).IsRequired();
            modelBuilder.Entity<PlanSuscripcion>().Property(e => e.Tipo).IsRequired();
            modelBuilder.Entity<PlanSuscripcion>().Property(e => e.Activo).IsRequired();

            #region Seed
            // Planes para Barberos
            var planFreeBarbero = new PlanSuscripcion 
            { 
                Id = new Guid("A0000000-0000-0000-0000-000000000001"), 
                Nombre = "Free", 
                Descripcion = "Plan gratuito para barberos - Sin reservas", 
                Precio = 0, 
                DuracionDias = 365, 
                Tipo = TipoPlan.Barbero, 
                Activo = true 
            };
            
            var planPopularBarbero = new PlanSuscripcion 
            { 
                Id = new Guid("A0000000-0000-0000-0000-000000000002"), 
                Nombre = "Popular", 
                Descripcion = "Plan popular para barberos - Con reservas y estadísticas básicas", 
                Precio = 500, 
                DuracionDias = 30, 
                Tipo = TipoPlan.Barbero, 
                Activo = true 
            };
            
            var planPremiumBarbero = new PlanSuscripcion 
            { 
                Id = new Guid("A0000000-0000-0000-0000-000000000003"), 
                Nombre = "Premium", 
                Descripcion = "Plan premium para barberos - Todas las funcionalidades", 
                Precio = 1000, 
                DuracionDias = 30, 
                Tipo = TipoPlan.Barbero, 
                Activo = true 
            };

            // Planes para Barberías
            var planBasicoBarberia = new PlanSuscripcion 
            { 
                Id = new Guid("A0000000-0000-0000-0000-000000000004"), 
                Nombre = "Básico", 
                Descripcion = "Plan básico para barberías - Hasta 3 barberos", 
                Precio = 1500, 
                DuracionDias = 30, 
                Tipo = TipoPlan.Barberia, 
                Activo = true 
            };
            
            var planEstandarBarberia = new PlanSuscripcion 
            { 
                Id = new Guid("A0000000-0000-0000-0000-000000000005"), 
                Nombre = "Estándar", 
                Descripcion = "Plan estándar para barberías - Hasta 10 barberos", 
                Precio = 3000, 
                DuracionDias = 30, 
                Tipo = TipoPlan.Barberia, 
                Activo = true 
            };
            
            var planEnterpriseBarberia = new PlanSuscripcion 
            { 
                Id = new Guid("A0000000-0000-0000-0000-000000000006"), 
                Nombre = "Enterprise", 
                Descripcion = "Plan enterprise para barberías - Barberos ilimitados", 
                Precio = 5000, 
                DuracionDias = 30, 
                Tipo = TipoPlan.Barberia, 
                Activo = true 
            };

            modelBuilder.Entity<PlanSuscripcion>().HasData(planFreeBarbero, planPopularBarbero, planPremiumBarbero, planBasicoBarberia, planEstandarBarberia, planEnterpriseBarberia);
            #endregion
        }
    }
}
