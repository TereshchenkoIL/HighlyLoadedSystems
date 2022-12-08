using CoreLib.FileReader;

namespace CoreLib.Models;

public class GetErrorPredictionRequestModel
{
    public FileReaderResult FileResult { get; set; }
    public int PredictTime { get; set; }
    public double ConfidenceInterval { get; set; }
    public int Iterations { get; set; }
}