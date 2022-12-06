using CoreLib.Models;

namespace CoreLib.Abstractions;

public interface ILambdaGenerator
{
    double[] Generate(GenerateLambdaRequestModel requestModel);
}