using CoreLib.Abstractions;
using CoreLib.Models;

namespace CoreLib.Implementation;

public class LambdaGenerator : ILambdaGenerator
{
    public double[] Generate(GenerateLambdaRequestModel requestModel)
    {
        for (var i = 2; i < requestModel.Result.ItemsCount; i++)
            requestModel.LaMi[i] =
                requestModel.Lambda * Math.Exp(-1 * requestModel.Mu * requestModel.Result.Times[i - 1]) -
                Math.Exp(-1 * requestModel.Mu * requestModel.Result.Times[i]);

        requestModel.LaMi[1] =
            requestModel.Lambda * (1 - Math.Exp(-1 * requestModel.Mu * requestModel.Result.Times[1]));

        return requestModel.LaMi;
    }
}