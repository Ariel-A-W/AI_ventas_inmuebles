using Microsoft.AspNetCore.Mvc;
using ventas_inmuebles.Server.Application.DTOs.Casas;
using ventas_inmuebles.Server.Application.UsesCases.Casas;
using ventas_inmuebles.Server.Domain.Casas;

namespace ventas_inmuebles.Server.Application.Controller;

[ApiController]
[Route("api/[controller]")]
public class CasasController : ControllerBase
{
    private readonly CasasUseCase _casas;

    public CasasController(CasasUseCase casas)
    {
        _casas = casas;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CasaResponseDTO>> GetAll()
    {
        var casas = _casas.GetAll();

        if (casas == null)
            return NotFound("No Existen Valores.");

        return Ok(casas);
    }

    [HttpGet]
    [Route("getcasaprediction")]
    public ActionResult<CasaResponseDTO> GetCasaPrediction(
        [FromQuery] CasasPredictionRequestDTO value
    )
    {
        var casaprediction = _casas.GetCasaPrediction(value);

        if (casaprediction == null)
            return NotFound("No se ha podido procesar la predicción.");

        return Ok(casaprediction);
    }
}

