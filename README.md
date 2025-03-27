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
