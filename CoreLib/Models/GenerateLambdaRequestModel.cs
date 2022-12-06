using CoreLib.FileReader;

namespace CoreLib.Models;

public class GenerateLambdaRequestModel
{
    public GenerateLambdaRequestModel(FileReaderResult result, double[] lAMi, double lambda, double mU)
    {
        Result = result;
        LaMi = lAMi;
        Lambda = lambda;
        Mu = mU;
    }

    public FileReaderResult Result { get; set; }

    public double[] LaMi { get; set; }

    public double Lambda { get; set; }

    public double Mu { get; set; }
}