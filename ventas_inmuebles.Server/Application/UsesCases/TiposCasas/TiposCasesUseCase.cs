using ventas_inmuebles.Server.Domain.TiposCases;

namespace ventas_inmuebles.Server.Application.UsesCases.TiposCasas;

public class TiposCasasUseCase
{
    private readonly ITipoCasa _tipos;

    public TiposCasasUseCase(ITipoCasa tipos)
    {
        _tipos = tipos;
    }

    public IEnumerable<TipoCasa> GetAll()
    {
        return _tipos.GetAll();
    }

    public TipoCasa GetById(int id)
    {
        return _tipos.GetById(id);
    }
}
