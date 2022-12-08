namespace CoreLib.FileReader;

public interface IFileReader
{
    FileReaderResult GetFileStatistics(string filePath);
}