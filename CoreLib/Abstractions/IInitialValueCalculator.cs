using CoreLib.FileReader;

namespace CoreLib.Abstractions;

public interface IInitialValueCalculator
{
    double Calculate(FileReaderResult result);
}