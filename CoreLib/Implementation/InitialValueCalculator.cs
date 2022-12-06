using CoreLib.Abstractions;
using CoreLib.FileReader;

namespace CoreLib.Implementation;

public class InitialValueCalculator : IInitialValueCalculator
{
    public double Calculate(FileReaderResult result)
    {
        double firstHalfValue = 0;
        double secondHalfValue = 0;
        var i = 1;
        const double totalTime = 0;
        var searchValue = totalTime / 2;

        var times = result.Times;
        var faultSend = result.FaultsEnd;
        while (result.Times[i] >= searchValue) i += 1;

        firstHalfValue = faultSend[i - 1] + (faultSend[i] - faultSend[i - 1]) * (searchValue - times[i - 1]) /
            (times[i] - times[i - 1]);
        secondHalfValue = faultSend[result.ItemsCount] - firstHalfValue;

        return secondHalfValue / firstHalfValue * (secondHalfValue / firstHalfValue);
    }
}