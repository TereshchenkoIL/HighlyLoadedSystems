using CoreLib.Abstractions;

namespace CoreLib.Implementation;

public class CoreLibAbstractFactory : ICoreLibAbstractFactory
{
    public IInitialValueCalculator CreateInitialValueCalculator()
    {
        return new InitialValueCalculator();
    }

    public ILambdaGenerator CreateLambdaGenerator()
    {
        return new LambdaGenerator();
    }

    public IPoissonsRandomValuesGenerator CreatePoissonsRandomValuesGenerator()
    {
        return new PoissonsRandomValuesGenerator();
    }

    public ISampleBootstrappingEstimator CreateSampleBootstrappingEstimator()
    {
        return new SampleBootstrappingEstimator();
    }
}