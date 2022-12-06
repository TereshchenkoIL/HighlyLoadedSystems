using CoreLib.Abstractions;
using CoreLib.Models;

namespace CoreLib.Implementation;

public class SampleBootstrappingEstimator : ISampleBootstrappingEstimator
{
    public double GetEstimation(SampleBootstrapping requestModel)
    {
        double munew = default;
        double x = default;
        double t4 = default;

        var mucurrent = -1 * Math.Log(requestModel.Xone) / requestModel.TotalTime;

        while (Math.Abs(munew - mucurrent) <= Math.E)
        {
            for (var i = 1; i < requestModel.Result.ItemsCount; i++)
            {
                var t1 = requestModel.Result.Times[i] * Math.Exp(-1 * mucurrent * requestModel.Result.Times[i]);
                var t2 = requestModel.Result.Times[i - 1] * Math.Exp(-1 * mucurrent * requestModel.Result.Times[i - 1]);
                var t3 = Math.Exp(-1 * mucurrent * requestModel.Result.Times[i - 1]) -
                         Math.Exp(-1 * mucurrent * requestModel.Result.Times[i]);
                t4 = t4 + requestModel.F[i] * ((t1 - t2) / t3);
            }

            if (t4 < 0.01)
            {
                t4 = 0.01;
                var s = t4 / (requestModel.TFaults * requestModel.TotalTime);
                x = s / (1 + s);
                t4 = 0;
            }

            munew = -1 * Math.Log(x) / requestModel.TotalTime;
        }

        return mucurrent;
    }
}