using Microsoft.ML.Data;

namespace ventas_inmuebles.Server.Domain.Casas;

public class CasaResponseDTO
{
    [ColumnName("Score")]
    public float Precio { get; set; }
}
