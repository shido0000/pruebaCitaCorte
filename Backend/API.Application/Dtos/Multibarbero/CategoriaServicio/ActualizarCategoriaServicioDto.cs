namespace API.Application.Dtos.Multibarbero.CategoriaServicio;

public class ActualizarCategoriaServicioDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;
}
