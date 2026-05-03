using API.Application.Dtos.Multibarbero.CategoriaProducto;

namespace API.Application.Dtos.Multibarbero.CategoriaProducto;

public class CategoriaProductoListadoPaginadoDto
{
    public List<CategoriaProductoDto> Items { get; set; } = new();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int CantidadPorPagina { get; set; }
    public int TotalPaginas => (int)Math.Ceiling(Total / (double)CantidadPorPagina);
}
