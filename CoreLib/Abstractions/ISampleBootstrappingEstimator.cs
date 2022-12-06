using CoreLib.Models;

namespace CoreLib.Abstractions;

public interface ISampleBootstrappingEstimator
{
    double GetEstimation(SampleBootstrapping requestModel);
}