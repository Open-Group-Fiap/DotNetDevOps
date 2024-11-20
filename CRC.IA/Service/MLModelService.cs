using System.Net;
using CRC.IA.Entities;
using Microsoft.ML;

namespace CRC.IA.Service;

public class MLModelService
{
    private static readonly string ModelPath = "model.zip";
    private static readonly MLContext MlContext = new MLContext(seed: 1);
    private static ITransformer _model;
    private static PredictionEngine<FaturaData, FaturaPrediction> _predictionEngine;

    static MLModelService()
    {
        LoadOrTrainModel();
        _predictionEngine = MlContext.Model.CreatePredictionEngine<FaturaData, FaturaPrediction>(_model);
    }

    private static void LoadOrTrainModel()
    {
        if (File.Exists(ModelPath))
        {
            _model = MlContext.Model.Load(ModelPath, out var _);
            return;
        }

        TrainModel();
    }

    private static void TrainModel()
    {
        string url = "https://cidastore.blob.core.windows.net/crc/dataset.csv";
        string dataPath = "dataset.csv";
        using (WebClient client = new WebClient())
        {
            client.DownloadFile(url, dataPath);
        }

        IDataView dataView = MlContext.Data.LoadFromTextFile<FaturaData>(
            dataPath, hasHeader: true, separatorChar: ',');

        var pipeline = MlContext.Transforms.Categorical.OneHotEncoding("RegiaoEncoded", "Regiao")
            .Append(MlContext.Transforms.Concatenate("Features", "QtdMoradores", "RegiaoEncoded"))
            .Append(MlContext.Regression.Trainers.Sdca(labelColumnName: "QtdConsumida", maximumNumberOfIterations: 100));

        _model = pipeline.Fit(dataView);
        MlContext.Model.Save(_model, dataView.Schema, ModelPath);
    }

    public static FaturaPrediction PredictQtdConsumida(FaturaData data)
    {
        return _predictionEngine.Predict(data);
    }

    public static (double RSquared, double RootMeanSquaredError) EvaluateModel()
    {
        var dataPath = "dataset.csv";
        var dataView = MlContext.Data.LoadFromTextFile<FaturaData>(
            dataPath, hasHeader: true, separatorChar: ',');
        var predictions = _model.Transform(dataView);
        var metrics = MlContext.Regression.Evaluate(
            predictions, labelColumnName: "QtdConsumida");

        return (metrics.RSquared, metrics.RootMeanSquaredError);
    }
}