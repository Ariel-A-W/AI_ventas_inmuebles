using Microsoft.AspNetCore.Mvc;
using ventas_inmuebles.Server.Application.DTOs.Localidades;
using ventas_inmuebles.Server.Application.UsesCases.Localidades;
using ventas_inmuebles.Server.Domain.Localidades;

namespace ventas_inmuebles.Server.Application.Controller;

[ApiController]
[Route("api/[controller]")]
public class LocalidadesController : ControllerBase
{
    private readonly LocalidadesUseCase _localidades;

    public LocalidadesController(LocalidadesUseCase localidades)
    {
        _localidades = localidades;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Localidad>> GetAll()
    {
        var lstLocalidades = _localidades.GetAll();

        if (lstLocalidades == null)
            return NotFound("No Existen Valores.");

        return Ok(lstLocalidades);
    }

    [HttpGet]
    [Route("getbyid")]
    public ActionResult<Localidad> GetById(
        [FromQuery] LocalidadesByIdRequestDTO value
    )
    {
        var localidad = _localidades.GetById(value.LocalidadId);

        if (localidad == null)
            return NotFound("No Existen Valores.");

        return Ok(localidad);
    }
}
