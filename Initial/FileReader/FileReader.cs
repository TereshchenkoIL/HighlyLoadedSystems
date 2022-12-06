namespace Initial.FileReader;

public class FIleReader : IFIleReader
{
    public FileReaderResult GetFileStatistics(string filePath)
    {
        return new FileReaderResult
        {
            Times = new double[] {1, 5, 12, 17, 25, 26, 32, 37, 52, 67},
            FaultsEnd = new double[]{1, 3, 4, 6, 10, 6, 7, 3, 4, 2},
            ItemsCount = 10
        };
    }
}