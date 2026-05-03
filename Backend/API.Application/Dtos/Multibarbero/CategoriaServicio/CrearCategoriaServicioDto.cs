namespace API.Application.Dtos.Multibarbero.CategoriaServicio;

public class CrearCategoriaServicioDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;
}
