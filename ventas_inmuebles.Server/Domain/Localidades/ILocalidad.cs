namespace ventas_inmuebles.Server.Domain.Localidades;

public interface ILocalidad
{
    public IEnumerable<Localidad> GetAll();
    public Localidad GetById(int id);
}
