namespace API.Application.Dtos.Multibarbero.CategoriaProducto;

public class CategoriaProductoFiltroDto
{
    public string? Nombre { get; set; }
    public bool? Activo { get; set; }
    public int Pagina { get; set; } = 1;
    public int CantidadPorPagina { get; set; } = 10;
}
