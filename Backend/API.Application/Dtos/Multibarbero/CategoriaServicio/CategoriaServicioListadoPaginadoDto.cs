namespace API.Application.Dtos.Multibarbero.CategoriaServicio;

public class CategoriaServicioListadoPaginadoDto
{
    public List<CategoriaServicioDto> Items { get; set; } = new();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int CantidadPorPagina { get; set; }
    public int TotalPaginas => (int)Math.Ceiling(Total / (double)CantidadPorPagina);
}
