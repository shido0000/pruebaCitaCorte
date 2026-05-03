using API.Application.Dtos.Multibarbero.Producto;

namespace API.Application.Dtos.Multibarbero.Producto;

public class ProductoListadoPaginadoDto
{
    public List<ProductoDto> Items { get; set; } = new();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int CantidadPorPagina { get; set; }
    public int TotalPaginas => (int)Math.Ceiling(Total / (double)CantidadPorPagina);
}
