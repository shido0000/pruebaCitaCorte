using System.Text.Json.Serialization;
using API.Data.Entidades;
using API.Data.Entidades.Multibarbero;
using API.Data.Entidades.Seguridad;
using API.Data.Enum.Multibarbero;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Perfil del barbero que extiende Usuario
    /// </summary>
    public class PerfilBarbero : EntidadBase
    {
        [ForeignKey(nameof(Usuario))]
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public string? Biografia { get; set; }
        public int ExperienciaAnios { get; set; }
        public string EspecialidadesJson { get; set; } = "[]"; // JSON array of especialidades
        public string? FotoPerfil { get; set; }
        public string? Ubicacion { get; set; }
        public decimal CalificacionPromedio { get; set; }
        public int TotalReseñas { get; set; }

        // Navegación a barbería si aplica
        public Guid? PerfilBarberiaId { get; set; }
        [ForeignKey(nameof(PerfilBarberiaId))]
        public PerfilBarberia? PerfilBarberia { get; set; }

        // Horarios (puede ser colección separada)
        // public ICollection<HorarioBarbero> Horarios { get; set; } = new List<HorarioBarbero>();
    }
}
