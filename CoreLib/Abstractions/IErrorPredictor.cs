using CoreLib.Models;

namespace CoreLib.Abstractions;

public interface IErrorPredictor
{
    ErrorPredictionResult PredictErrors(GetErrorPredictionRequestModel requestModel);
}