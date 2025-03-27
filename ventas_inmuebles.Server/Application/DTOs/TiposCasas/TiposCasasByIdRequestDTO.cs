using System.ComponentModel.DataAnnotations;

namespace ventas_inmuebles.Server.Application.DTOs.TiposCasas;

public record TiposCasasByIdRequestDTO
{
    [Required(ErrorMessage = "{0}: Este valor es requerido.")]
    public int TipoCasaId { get; set; }
}
