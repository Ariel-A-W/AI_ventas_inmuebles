using ventas_inmuebles.Server.Domain.Localidades;

namespace ventas_inmuebles.Server.Application.UsesCases.Localidades;

public class LocalidadesUseCase
{
    private readonly ILocalidad _localidades;

    public LocalidadesUseCase(ILocalidad localidad)
    {
        _localidades = localidad;
    }

    public IEnumerable<Localidad> GetAll()
    {
        return _localidades.GetAll();
    }

    public Localidad GetById(int id)
    {
        return _localidades.GetById(id);
    }
}
