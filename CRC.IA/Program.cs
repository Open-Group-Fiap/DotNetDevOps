using System.Net;
using CRC.IA.Entities;
using Microsoft.ML;

namespace CRC.IA;

public class Program
{
    public static void Main(string[] args)
    {
        
        
        
        string url = "https://cidastore.blob.core.windows.net/crc/dataset.csv";
        string dataPath = "dataset.csv";
        

        using (WebClient client = new WebClient())
        {
            client.DownloadFile(url, dataPath);
        }
        
        Console.WriteLine("Arquivo CSV baixado com sucesso.");
        
        // // Create context
        var mlContext = new MLContext();

        IDataView dataView = mlContext.Data.LoadFromTextFile<FaturaData>(dataPath, hasHeader: true, separatorChar: ',');
        
        
        // Config the train
        var pipeline = mlContext.Transforms.Categorical.OneHotEncoding("RegiaoEncoded", "Regiao")
            .Append(mlContext.Transforms.Concatenate("Features", "QtdMoradores", "RegiaoEncoded"))
            .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "QtdConsumida", maximumNumberOfIterations: 100));

        // train
        var model = pipeline.Fit(dataView);

        // avaliate the model
        var predictions = model.Transform(dataView);
        var metrics = mlContext.Regression.Evaluate(predictions, labelColumnName: "QtdConsumida");

        Console.WriteLine($"R^2: {metrics.RSquared}");
        Console.WriteLine($"RMSE: {metrics.RootMeanSquaredError}");

        // make the prediction
        var predictionEngine = mlContext.Model.CreatePredictionEngine<FaturaData, FaturaPrediction>(model);

        var sampleData = new FaturaData { QtdMoradores = 4, Regiao = "Sul" };
        var prediction = predictionEngine.Predict(sampleData);

        Console.WriteLine($"Previsão de variação para Verão na região Sul: {prediction.QtdConsumida}");
        
    }
}