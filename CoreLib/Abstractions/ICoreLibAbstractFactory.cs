namespace CoreLib.Abstractions;

public interface ICoreLibAbstractFactory
{
    IInitialValueCalculator CreateInitialValueCalculator();

    ILambdaGenerator CreateLambdaGenerator();

    IPoissonsRandomValuesGenerator CreatePoissonsRandomValuesGenerator();

    ISampleBootstrappingEstimator CreateSampleBootstrappingEstimator();
}