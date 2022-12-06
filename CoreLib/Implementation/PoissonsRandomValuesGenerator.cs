using CoreLib.Abstractions;
using CoreLib.Models;

namespace CoreLib.Implementation;

public class PoissonsRandomValuesGenerator : IPoissonsRandomValuesGenerator
{
    public PoissonsValuesResult Generate(GetRandomPoissonsRequestModel requestModel)
    {
        for (var i = 1; i < requestModel.Result.ItemsCount; i++)
        {
            var w = Math.Exp(-1 * requestModel.LaMi[i]);
            double x = 0;
            var a = w;
            var b = a;
            var u = new Random().NextDouble() * 1000 / 1000;

            while (u > a && a <= 0.9999)
            {
                x = x + 1;
                b = b * requestModel.LaMi[i] / x;
                a = a + b;
            }

            requestModel.TempFaults[i] = x;
            requestModel.TotItFaults += requestModel.TempFaults[i];
        }

        return new PoissonsValuesResult
        {
            TotItFaults = requestModel.TotItFaults,
            TempFaults = requestModel.TempFaults
        };
    }
}