# Cotizador de Inmuebles Basado en Intenigencia Artificial AI

Esta aplicación demostrativa utiliza la AI (Artificial Intelligense) o Inteligencia Artificial para realizar un procesamiento de los datos que posee una inmobiliaria, a los efectos de poder cotizar el valor de un inmueble en función a todos los datos que posee la empresa. Para este caso, contamos con una API que proporciona una serie de servicios en los que figura un sistema de predicción para la cotización, a través de la librería ML (Mqchine Learning) de Microsoft.

![Captura 1.](/docs/capture1.png "Imagen del cotizador.")

## El Algoritmo Utilizado

En este algoritmo estás utilizando aprendizaje automático (Machine Learning), específicamente un modelo de regresión para predecir el precio de una casa basado en características como el tamaño, el número de habitaciones, baños, antigüedad y ubicación de la vivienda. Este tipo de algoritmo cae bajo el área de predicción de precios inmobiliarios o modelos de predicción en bienes raíces.

## Descripción del Algoritmo

En este algoritmo, se emplea aprendizaje supervisado mediante un modelo de regresión para predecir el precio de una vivienda. La técnica utilizada en este caso es el modelo de regresión con LightGBM (Light Gradient Boosting Machine), que es un algoritmo eficiente y poderoso para problemas de predicción de valores continuos.

### El proceso se lleva a cabo de la siguiente manera:

**Cargar los Datos**: Los datos sobre las casas (_tamaño_, _habitaciones_, _baños_, _antigüedad_, _ubicación_ y _precio_) se cargan en memoria a través de la clase MLContext. Se utiliza una lista de datos de casas (_lstCasasData_), la cual se obtiene previamente de la base de datos o almacenamiento.

**División de Datos en Entrenamiento y Prueba**: Se dividen los datos en dos conjuntos: uno para entrenar el modelo **80%** de los datos y otro para probar su precisión **20%** de los datos. Esto es esencial para evitar el sobreajuste y para validar la capacidad del modelo para generalizar.

**Transformación de Datos**: Se aplica una serie de transformaciones para preparar los datos:

**OneHotEncoding**: Para la variable categórica **_Ubicacion_**, que convierte la ubicación de la vivienda en un formato adecuado para el algoritmo.

**Normalización**: Las variables numéricas como **_TamanioM2_**, **_Habitaciones_**, **_Banios_** y **_Antiguedad_** se normalizan para que tengan una escala similar, lo que mejora el rendimiento del modelo.

**Concatenación de Características**: Finalmente, se combinan todas las características (_tamaño_, _habitaciones_, _baños_, _antigüedad_ y _ubicación_) en una sola columna llamada **Features**.

**Entrenamiento del Modelo**: El modelo de regresión se entrena utilizando el algoritmo LightGBM, que es un algoritmo basado en árboles de decisión y es muy eficiente para tareas de predicción.

**Evaluación del Modelo**: Se evalúa el rendimiento del modelo utilizando el conjunto de datos de prueba (_testData_). La evaluación se realiza con métricas como el error cuadrático medio (RMSE) o el coeficiente de determinación (R²).

**Predicción de Precios**: Una vez entrenado y evaluado el modelo, se utiliza el Predictor para predecir el precio de una nueva vivienda, basándose en sus características (_tamaño_, _habitaciones_, _baños_, _antigüedad_, _ubicación_).

**Resultado de la Predicción**: El modelo devuelve el precio estimado de la vivienda, que se presenta en un formato de respuesta DTO, junto con las características de la casa.

## Tipo de Inteligencia Artificial Utilizada

El tipo de inteligencia artificial que estás utilizando en este algoritmo es aprendizaje supervisado, con un enfoque en regresión. Más específicamente, el algoritmo LightGBM (_Light Gradient Boosting Machine_) es utilizado para realizar la tarea de predicción. Este tipo de algoritmo es ampliamente utilizado para problemas de predicción numérica y es conocido por su eficiencia y rapidez en el procesamiento de grandes volúmenes de datos.

En resumen, este es un modelo de regresión basado en árboles de decisión que predice el precio de una vivienda en función de las características ingresadas. Se basa en el principio de que se pueden aprender patrones complejos en los datos históricos de ventas inmobiliarias para predecir precios de futuras propiedades.

### ¿Por qué utilizar LightGBM?

**Eficiencia**: LightGBM es conocido por su velocidad y eficiencia al manejar grandes cantidades de datos.

**Escalabilidad**: Este modelo es escalable, lo que significa que puede manejar tanto conjuntos de datos pequeños como grandes sin perder rendimiento.

**Precisión**: Al ser un modelo basado en Gradient Boosting, tiene un alto grado de precisión en tareas de predicción, especialmente en problemas con datos estructurados como los de bienes raíces.

Este tipo de IA es ideal para predicciones en tiempo real sobre el valor de propiedades, lo cual es muy útil en aplicaciones inmobiliarias, aplicaciones de tasación y plataformas de análisis de bienes raíces.

Puedes agregar este texto a tu blog y en tu repositorio de GitHub para explicar la lógica y la inteligencia detrás del algoritmo de predicción de precios de casas.

# Código 

El siguiente es el corazón operativo de esta aplicación. Analicemos entonces. 

```C#
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
```

Como podemos ver estamos utilizando la librería ML de Microsoft. La función *LoadFromEnumerable<>()* es utilizada para cargar los datos desde el orígen de base de datos. En este caso es proporcionada por la función *GetAll()* que se encarga de hacer la consulta y traer todos los datos. En mi caso he hardcodeado estos datos en un archivo JSON donde se encuentran todos los datos. Convencionalmente este origen de datos suele ser una base de datos. Aquí lo he creado con JSON por cuestiones de practicidad y pedagógicas. 

El siguiente paso es entonces pasar a dividir los procesos, tanto para el entrenamiento como la prueba de los datos. El proceso de división lo realiza la función *TrainTestSplit()*. A continuación, la variable *trainTestSplit* permite separar físicamente los datos haciendo uso de dos propiedades, *TrainSet* y *TestSet*. Por otro lado, ambas propiedades son asignadas a sus respectivas variables *traingingData* y *testData*. 

El siguiente paso es crear un pipeline "*entubado*". El pipeline se encarga de hacer un agrupamiento de varios recursos para poder controlar todo el proceso. Desde crear una columna específica para el caso de la función *OneHotEncvoding()*, seguido luego *NormalizeMinMax()* que se encarga de normalizar valores entre ciertos mínimos y máximos. Luego tenemos un concatenador *Concatenate()* que añade cada uno de los campos importantes desde la fuente de orígenes de datos incluso un campo adicional que es el saliente. En este caso llamado como *Features*. Para finalizar, tenemos la función *LightGbm()*. Esta función precisamente se encarga entonces de predecir los sados saliente haciendo uso de un gradiente inicial que resulta clave para la toma decisiva del *modelo de árbol de regresión*.

El siguiente paso es realizar el ajuste. Esta etapa es crucial. El ajuste evita que el producto del entrenamiento y prueba, no sufra un efecto de *Overfitting* sobreajustes y de *Underfitting* subjuste. Estos términos se refieren a la incapacidad de un modelo para generalizar, ya sea por aprender demasiado bien los datos de entrenamiento *sobreajuste* o por no capturar suficientes patrones *subajuste*. 

La última etapa es la de las evaluaciones. En breve, la que nos arroja los resultados finales. La función *Evaluate()*, que no la usamos aquí, nos sirve precisamente para evaluar las métricas del algoritmo. Por otro lado, este si lo usamos aquí, es el valor de predicción saliente. Este proceso lo podemos obtener desde la función *CreatePredictionEngine()*. A su vez, esta función es asignada como un objeto hacia una variable llamada *predictor*. Gracias a esto objeto finalmente podremos utilizar la función *Predict()* que será la que ejecute el proceso para la predicción. Observa que esta función le pasa un arreglo donde pasaremos los valores de cada uno de los campos que son necesarios para la prediccióm. Son los campos de entrada que luego son pasados como argumentos para la función *Predict*. 