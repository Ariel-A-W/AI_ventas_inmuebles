using Newtonsoft.Json;
using ventas_inmuebles.Server.Domain.Casas;

namespace ventas_inmuebles.Server.Infraestructure;

public class CasasRepository : ICasa
{
    private readonly List<CasaData> _casas;

    public CasasRepository()
    {
        string relativePath = Path.Combine("Data", "inmuebles.json");
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

        if (!File.Exists(filePath))
            throw new Exception("Archivo JSON no encontrador");
        string jsonContent = File.ReadAllText(filePath);

        List<Casa> casas = JsonConvert.DeserializeObject<List<Casa>>(jsonContent)!;

        _casas = new List<CasaData>();
        foreach (var item in casas)
        {
            _casas.Add(item);
        }
    }

    public IEnumerable<CasaData> GetAll()
    {
        return _casas.ToList();
    }
}
