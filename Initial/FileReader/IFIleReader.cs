namespace Initial.FileReader;

public interface IFIleReader
{
    FileReaderResult GetFileStatistics(string filePath);
}