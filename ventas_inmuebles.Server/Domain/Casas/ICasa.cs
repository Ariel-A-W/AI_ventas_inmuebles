namespace ventas_inmuebles.Server.Domain.Casas;

public interface ICasa
{
    public IEnumerable<CasaData> GetAll();
}
