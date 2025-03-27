namespace ventas_inmuebles.Server.Application.DTOs.Casas;

public record CasasResponseDTO
{
    public string? TipoVivienda { get; set; }
    public string? Ubicación { get; set; }
    public float TamanioM2 { get; set; }
    public int Habitaciones { get; set; }
    public int Banios { get; set; }
    public int Antiguedad { get; set; }
    public float Precio { get; set; }
}
