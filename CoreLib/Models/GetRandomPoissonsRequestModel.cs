using CoreLib.FileReader;

namespace CoreLib.Models;

public class GetRandomPoissonsRequestModel
{
    public GetRandomPoissonsRequestModel(FileReaderResult result, double[] lami, double[] tempFaults,
        double totItFaults)
    {
        Result = result;
        LaMi = lami;
        TempFaults = tempFaults;
        TotItFaults = totItFaults;
    }

    public FileReaderResult Result { get; set; }
    public double[] LaMi { get; set; }
    public double[] TempFaults { get; set; }
    public double TotItFaults { get; set; }
}