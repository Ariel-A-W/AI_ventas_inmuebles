using Microsoft.ML;
using ventas_inmuebles.Server.Application.DTOs.Casas;
using ventas_inmuebles.Server.Domain.Casas;
using ventas_inmuebles.Server.Domain.Localidades;
using ventas_inmuebles.Server.Domain.TiposCases;

namespace ventas_inmuebles.Server.Application.UsesCases.Casas;

public class CasasUseCase
{
    private readonly ICasa _casas;
    private readonly ILocalidad _localidades;
    private readonly ITipoCasa _tiposcasas;

    public CasasUseCase(ICasa casas, ILocalidad localidades, ITipoCasa tiposcasas)
    {
        _casas = casas;
        _localidades = localidades;
        _tiposcasas = tiposcasas;
    }

    public IEnumerable<CasasResponseDTO> GetAll()
    {
        var lstCasas = new List<CasasResponseDTO>();
        var casas = _casas.GetAll();
        foreach (var item in casas)
        {
            lstCasas.Add(
                new CasasResponseDTO
                {
                    Ubicación = item.Ubicacion,
                    TipoVivienda = item.TipoVivienda,
                    TamanioM2 = item.TamanioM2,
                    Habitaciones = Convert.ToInt32(item.Habitaciones),
                    Banios = Convert.ToInt32(item.Banios),
                    Antiguedad = Convert.ToInt32(item.Antiguedad),
                    Precio = item.Precio
                }
            );
        }
        return lstCasas;
    }

    public CasasResponseDTO GetCasaPrediction(
        CasasPredictionRequestDTO casa
    )
    {
        var lstCasasData = _casas.GetAll();

        var ubicacion = _localidades.GetById(casa.UbicacionId);

        if (ubicacion == null)
            return new CasasResponseDTO();

        var vivienda = _tiposcasas.GetById(casa.TipoViviendaId);

        if (vivienda == null)
            return new CasasResponseDTO();

        // Crear el contexto de ML
        var mlContext = new MLContext();

        // Cargar los datos
        // var data = mlContext.Data.LoadFromTextFile<CasaData>("", hasHeader: true, separatorChar: ';');
        var data = mlContext.Data.LoadFromEnumerable<CasaData>(lstCasasData);

        // Dividir los datos en entrenamiento y prueba
        var trainTestSplit = mlContext.Data.TrainTestSplit(data, testFraction: 0.2);
        var trainingData = trainTestSplit.TrainSet;
        var testData = trainTestSplit.TestSet;

        // Construir el pipeline de entrenamiento
        var pipeline = mlContext.Transforms.Categorical.OneHotEncoding("Ubicacion")
            .Append(mlContext.Transforms.NormalizeMinMax("TamanioM2"))
            .Append(mlContext.Transforms.NormalizeMinMax("Habitaciones"))
            .Append(mlContext.Transforms.NormalizeMinMax("Banios"))
            .Append(mlContext.Transforms.NormalizeMinMax("Antiguedad"))
            .Append(mlContext.Transforms.Concatenate("Features", "TamanioM2", "Habitaciones", "Banios", "Antiguedad", "Ubicacion"))
            .Append(mlContext.Regression.Trainers.LightGbm(labelColumnName: "Precio", featureColumnName: "Features"));

        // Entrenar el modelo
        var model = pipeline.Fit(trainingData);

        // Evaluar el modelo
        var predictions = model.Transform(testData);
        var metrics = mlContext.Regression.Evaluate(predictions, labelColumnName: "Precio");

        // Realizar predicciones
        var predictor = mlContext.Model.CreatePredictionEngine<CasaData, CasaResponseDTO>(model);
        //var newHouse = new CasaData { Size = 120, Rooms = 4, Bathrooms = 2, Age = 5, Location = "Urban" };

        var newHouse = new CasaData()
        {
            Ubicacion = ubicacion!.Ubicacion,
            TamanioM2 = casa.TamanioM2,
            Habitaciones = casa.Habitaciones,
            Banios = casa.Banios,
            TipoVivienda = vivienda!.Tipo,
            Antiguedad = casa.Antiguedad
        };

        var prediction = predictor.Predict(newHouse);

        return new CasasResponseDTO()
        {
            Ubicación = newHouse.Ubicacion,
            TipoVivienda = newHouse.TipoVivienda,
            TamanioM2 = newHouse.TamanioM2,
            Habitaciones = Convert.ToInt32(newHouse.Habitaciones),
            Banios = Convert.ToInt32(newHouse.Banios),
            Antiguedad = Convert.ToInt32(newHouse.Antiguedad),
            Precio = prediction.Precio
        };
    }
}

