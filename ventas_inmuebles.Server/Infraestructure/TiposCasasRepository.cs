using ventas_inmuebles.Server.Domain.TiposCases;

namespace ventas_inmuebles.Server.Infraestructure;

public class TiposCasasRepository : ITipoCasa
{
    private readonly List<TipoCasa> _tipos;

    public TiposCasasRepository()
    {
        _tipos = new List<TipoCasa>();
        var tiposCasas = new List<string>
        { "Casa", "Departamento", "PH", "Duplex", "Quinta" };
        int i = 1;
        foreach (var item in tiposCasas)
        {
            _tipos.Add(
                new TipoCasa()
                {
                    TipoCasaId = i++,
                    Tipo = item.ToString()
                }
            );
        }
    }

    public IEnumerable<TipoCasa> GetAll()
    {
        return _tipos.ToList();
    }

    public TipoCasa GetById(int id)
    {
        return _tipos.FirstOrDefault(x => x.TipoCasaId == id)!;
    }
}
