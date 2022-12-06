using CoreLib.FileReader;

namespace CoreLib.Models;

public class SampleBootstrapping
{
    public SampleBootstrapping(double xone, double tFaults, double[] f, double totalTime, FileReaderResult result)
    {
        Xone = xone;
        TFaults = tFaults;
        F = f;
        TotalTime = totalTime;
        Result = result;
    }

    public double Xone { get; set; }
    public double TFaults { get; set; }
    public double[] F { get; set; }
    public double TotalTime { get; set; }
    public FileReaderResult Result { get; set; }
}