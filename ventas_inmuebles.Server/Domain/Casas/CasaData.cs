using Microsoft.ML.Data;

namespace ventas_inmuebles.Server.Domain.Casas;

public class CasaData
{
    [LoadColumn(0)]
    public string? Ubicacion { get; set; }

    [LoadColumn(1)]
    public float TamanioM2 { get; set; }

    [LoadColumn(2)]
    public float Habitaciones { get; set; }

    [LoadColumn(3)]
    public float Banios { get; set; }

    [LoadColumn(4)]
    public string? TipoVivienda { get; set; }

    [LoadColumn(5)]
    public float Antiguedad { get; set; }

    [LoadColumn(6)]
    public float Precio { get; set; }
}
