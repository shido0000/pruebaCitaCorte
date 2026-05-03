using API.Data.Entidades.Multibarbero;
using API.Data.Enum.Multibarbero;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Seeders;

/// <summary>
/// Seeder inicial para cargar datos básicos del sistema Multibarbero
/// </summary>
public static class MultibarberoSeeder
{
    public static async Task SeedAsync(DbContext context)
    {
        // Evitar duplicados si ya existen datos
        if (context.Set<PlanSuscripcion>().Any())
        {
            return;
        }

        // ==================== PLANES PARA BARBEROS ====================
        
        var planFreeBarbero = new PlanSuscripcion
        {
            Id = Guid.NewGuid(),
            Nombre = "Free",
            Descripcion = "Plan gratuito para barberos que comienzan",
            Precio = 0,
            DuracionDias = 30,
            Tipo = TipoPlan.Barbero,
            Activo = true,
            FechaCreacion = DateTime.UtcNow,
            Caracteristicas = new List<CaracteristicaPlan>
            {
                new() { Id = Guid.NewGuid(), Nombre = "Perfil Personal", Descripcion = "Modificar datos personales" },
                new() { Id = Guid.NewGuid(), Nombre = "Mostrar Servicios", Descripcion = "Aparecer en búsquedas" }
            },
            Limites = new LimitesPlan
            {
                Id = Guid.NewGuid(),
                MaxBarberosAfiliados = 0,
                MaxReservasMensuales = 0,
                MaxProductosVenta = 0,
                PermiteEstadisticas = false,
                PermiteReservas = false,
                PermiteProductos = false
            }
        };

        var planPopularBarbero = new PlanSuscripcion
        {
            Id = Guid.NewGuid(),
            Nombre = "Popular",
            Descripcion = "Plan ideal para barberos independientes",
            Precio = 29.99m,
            DuracionDias = 30,
            Tipo = TipoPlan.Barbero,
            Activo = true,
            FechaCreacion = DateTime.UtcNow,
            Caracteristicas = new List<CaracteristicaPlan>
            {
                new() { Id = Guid.NewGuid(), Nombre = "Todo lo del plan Free", Descripcion = "" },
                new() { Id = Guid.NewGuid(), Nombre = "Recibir Reservas", Descripcion = "Los clientes pueden reservar turnos" },
                new() { Id = Guid.NewGuid(), Nombre = "Estadísticas Básicas", Descripcion = "Total servicios, clientes, ingresos mensuales" }
            },
            Limites = new LimitesPlan
            {
                Id = Guid.NewGuid(),
                MaxBarberosAfiliados = 0,
                MaxReservasMensuales = 100,
                MaxProductosVenta = 0,
                PermiteEstadisticas = true,
                PermiteReservas = true,
                PermiteProductos = false
            }
        };

        var planPremiumBarbero = new PlanSuscripcion
        {
            Id = Guid.NewGuid(),
            Nombre = "Premium",
            Descripcion = "Plan completo con todas las funcionalidades",
            Precio = 49.99m,
            DuracionDias = 30,
            Tipo = TipoPlan.Barbero,
            Activo = true,
            FechaCreacion = DateTime.UtcNow,
            Caracteristicas = new List<CaracteristicaPlan>
            {
                new() { Id = Guid.NewGuid(), Nombre = "Todo lo del plan Popular", Descripcion = "" },
                new() { Id = Guid.NewGuid(), Nombre = "Venta de Productos", Descripcion = "Postear productos en venta" },
                new() { Id = Guid.NewGuid(), Nombre = "Estadísticas Completas", Descripcion = "Análisis avanzado y tendencias" }
            },
            Limites = new LimitesPlan
            {
                Id = Guid.NewGuid(),
                MaxBarberosAfiliados = 0,
                MaxReservasMensuales = -1, // Ilimitado
                MaxProductosVenta = -1, // Ilimitado
                PermiteEstadisticas = true,
                PermiteReservas = true,
                PermiteProductos = true
            }
        };

        // ==================== PLANES PARA BARBERÍAS ====================
        
        var planBasicoBarberia = new PlanSuscripcion
        {
            Id = Guid.NewGuid(),
            Nombre = "Básico",
            Descripcion = "Para pequeñas barberías (hasta 3 barberos)",
            Precio = 59.99m,
            DuracionDias = 30,
            Tipo = TipoPlan.Barberia,
            Activo = true,
            FechaCreacion = DateTime.UtcNow,
            Caracteristicas = new List<CaracteristicaPlan>
            {
                new() { Id = Guid.NewGuid(), Nombre = "Gestión de Reservas", Descripcion = "Gestionar reservas propias" },
                new() { Id = Guid.NewGuid(), Nombre = "Estadísticas Básicas", Descripcion = "Métricas fundamentales" }
            },
            Limites = new LimitesPlan
            {
                Id = Guid.NewGuid(),
                MaxBarberosAfiliados = 3,
                MaxReservasMensuales = 200,
                MaxProductosVenta = 10,
                PermiteEstadisticas = true,
                PermiteReservas = true,
                PermiteProductos = true
            }
        };

        var planEstandarBarberia = new PlanSuscripcion
        {
            Id = Guid.NewGuid(),
            Nombre = "Estándar",
            Descripcion = "Para barberías en crecimiento (hasta 10 barberos)",
            Precio = 99.99m,
            DuracionDias = 30,
            Tipo = TipoPlan.Barberia,
            Activo = true,
            FechaCreacion = DateTime.UtcNow,
            Caracteristicas = new List<CaracteristicaPlan>
            {
                new() { Id = Guid.NewGuid(), Nombre = "Todo lo del plan Básico", Descripcion = "" },
                new() { Id = Guid.NewGuid(), Nombre = "Gestión de Afiliados", Descripcion = "Gestionar reservas de barberos afiliados" },
                new() { Id = Guid.NewGuid(), Nombre = "Estadísticas Intermedias", Descripcion = "Análisis más detallado" }
            },
            Limites = new LimitesPlan
            {
                Id = Guid.NewGuid(),
                MaxBarberosAfiliados = 10,
                MaxReservasMensuales = 500,
                MaxProductosVenta = 50,
                PermiteEstadisticas = true,
                PermiteReservas = true,
                PermiteProductos = true
            }
        };

        var planEnterpriseBarberia = new PlanSuscripcion
        {
            Id = Guid.NewGuid(),
            Nombre = "Enterprise",
            Descripcion = "Para grandes establecimientos (barberos ilimitados)",
            Precio = 199.99m,
            DuracionDias = 30,
            Tipo = TipoPlan.Barberia,
            Activo = true,
            FechaCreacion = DateTime.UtcNow,
            Caracteristicas = new List<CaracteristicaPlan>
            {
                new() { Id = Guid.NewGuid(), Nombre = "Todo lo del plan Estándar", Descripcion = "" },
                new() { Id = Guid.NewGuid(), Nombre = "Barberos Ilimitados", Descripcion = "Sin límite de barberos afiliados" },
                new() { Id = Guid.NewGuid(), Nombre = "Estadísticas Completas", Descripcion = "Dashboard avanzado y reportes" }
            },
            Limites = new LimitesPlan
            {
                Id = Guid.NewGuid(),
                MaxBarberosAfiliados = 50, // Considerado ilimitado
                MaxReservasMensuales = -1, // Ilimitado
                MaxProductosVenta = -1, // Ilimitado
                PermiteEstadisticas = true,
                PermiteReservas = true,
                PermiteProductos = true
            }
        };

        // Agregar todos los planes
        await context.Set<PlanSuscripcion>().AddRangeAsync(
            planFreeBarbero,
            planPopularBarbero,
            planPremiumBarbero,
            planBasicoBarberia,
            planEstandarBarberia,
            planEnterpriseBarberia
        );

        // ==================== CATEGORÍAS DE SERVICIOS ====================
        
        var categoriasServicio = new List<CategoriaServicio>
        {
            new() { Id = Guid.NewGuid(), Nombre = "Corte de Cabello", Descripcion = "Cortes y estilos de cabello", IconoUrl = "/icons/corte.svg" },
            new() { Id = Guid.NewGuid(), Nombre = "Barba", Descripcion = "Arreglos y cuidado de barba", IconoUrl = "/icons/barba.svg" },
            new() { Id = Guid.NewGuid(), Nombre = "Color", Descripcion = "Tintes y tratamientos de color", IconoUrl = "/icons/color.svg" },
            new() { Id = Guid.NewGuid(), Nombre = "Tratamientos", Descripcion = "Tratamientos capilares especializados", IconoUrl = "/icons/tratamiento.svg" },
            new() { Id = Guid.NewGuid(), Nombre = "Peinado", Descripcion = "Peinados y estilismo", IconoUrl = "/icons/peinado.svg" },
            new() { Id = Guid.NewGuid(), Nombre = "Rasurado", Descripcion = "Rasurado tradicional y moderno", IconoUrl = "/icons/rasurado.svg" }
        };
        
        await context.Set<CategoriaServicio>().AddRangeAsync(categoriasServicio);

        // ==================== CATEGORÍAS DE PRODUCTOS ====================
        
        var categoriasProducto = new List<CategoriaProducto>
        {
            new() { Id = Guid.NewGuid(), Nombre = "Ceras y Pomadas", Descripcion = "Productos para estilizar cabello" },
            new() { Id = Guid.NewGuid(), Nombre = "Aceites", Descripcion = "Aceites para barba y cabello" },
            new() { Id = Guid.NewGuid(), Nombre = "Shampoos", Descripcion = "Limpieza y cuidado capilar" },
            new() { Id = Guid.NewGuid(), Nombre = "Herramientas", Descripcion = "Peines, tijeras y accesorios" },
            new() { Id = Guid.NewGuid(), Nombre = "Aftershaves", Descripcion = "Productos post-rasurado" }
        };
        
        await context.Set<CategoriaProducto>().AddRangeAsync(categoriasProducto);

        // Guardar cambios
        await context.SaveChangesAsync();
    }
}
