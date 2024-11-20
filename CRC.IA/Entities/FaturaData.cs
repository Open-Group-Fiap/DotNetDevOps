using Microsoft.ML.Data;

namespace CRC.IA.Entities;

public class FaturaData
{
    [LoadColumn(0)]
    public float QtdConsumida;

    [LoadColumn(1)]
    public float QtdMoradores;

    [LoadColumn(2)]
    public string Regiao;
}