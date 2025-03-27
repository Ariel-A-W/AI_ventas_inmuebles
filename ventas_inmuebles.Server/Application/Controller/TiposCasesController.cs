using Microsoft.AspNetCore.Mvc;
using ventas_inmuebles.Server.Application.DTOs.TiposCasas;
using ventas_inmuebles.Server.Application.UsesCases.TiposCasas;
using ventas_inmuebles.Server.Domain.TiposCases;

namespace ventas_inmuebles.Server.Application.Controller;

[ApiController]
[Route("api/[controller]")]
public class TiposCasasController : ControllerBase
{
    private readonly TiposCasasUseCase _tipos;

    public TiposCasasController(TiposCasasUseCase tipos)
    {
        _tipos = tipos;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TipoCasa>> GetAll()
    {
        var tiposCasas = _tipos.GetAll();

        if (tiposCasas == null)
            return NotFound("No Existen Valores.");

        return Ok(tiposCasas);
    }

    [HttpGet]
    [Route("getbyid")]
    public ActionResult<TipoCasa> GetById(
        [FromQuery] TiposCasasByIdRequestDTO value
    )
    {
        var tipoCasa = _tipos.GetById(value.TipoCasaId);

        if (tipoCasa == null)
            return NotFound("Valor No Existente.");

        return Ok(tipoCasa);
    }
}

