namespace ventas_inmuebles.Server.Domain.TiposCases;

public interface ITipoCasa
{
    public IEnumerable<TipoCasa> GetAll();
    public TipoCasa GetById(int id);
}
