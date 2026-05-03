namespace API.Application.Dtos.Multibarbero.CategoriaProducto;

public class CrearCategoriaProductoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;
}
