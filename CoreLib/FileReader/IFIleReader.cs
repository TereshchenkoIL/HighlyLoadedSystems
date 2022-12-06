namespace CoreLib.FileReader;

public interface IFIleReader
{
    FileReaderResult GetFileStatistics(string filePath);
}