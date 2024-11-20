using Microsoft.ML.Data;

namespace CRC.IA.Entities;

public class FaturaPrediction
{
    [ColumnName("Score")]
    public float QtdConsumida;
}