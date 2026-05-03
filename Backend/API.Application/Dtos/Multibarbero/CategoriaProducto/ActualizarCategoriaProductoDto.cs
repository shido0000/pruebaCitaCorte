namespace API.Application.Dtos.Multibarbero.CategoriaProducto;

public class ActualizarCategoriaProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;
}
