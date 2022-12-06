namespace CoreLib.Abstractions;

public interface ICoreLibAbstractFactory
{
    IErrorPredictor CreateErrorPredictor();

    IInitialValueCalculator CreateInitialValueCalculator();

    ILambdaGenerator CreateLambdaGenerator();

    IPoissonsRandomValuesGenerator CreatePoissonsRandomValuesGenerator();

    ISampleBootstrappingEstimator CreateSampleBootstrappingEstimator();
}