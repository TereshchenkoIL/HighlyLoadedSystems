namespace CoreLib.Models;

public class ErrorPredictionResult
{
    public double MinErrorCount { get; set; }

    public double MaxErrorCount { get; set; }

    public double[] MeanErrors { get; set; }
}