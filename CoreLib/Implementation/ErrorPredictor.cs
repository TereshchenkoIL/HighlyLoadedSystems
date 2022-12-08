using CoreLib.Abstractions;
using CoreLib.Models;

namespace CoreLib.Implementation;

public class ErrorPredictor : IErrorPredictor
{
    private readonly ICoreLibAbstractFactory _coreLibAbstractFactory;
    private readonly IInitialValueCalculator _initialValueCalculator;
    private readonly ILambdaGenerator _lambdaGenerator;
    private readonly IPoissonsRandomValuesGenerator _poissonsRandomValuesGenerator;
    private readonly ISampleBootstrappingEstimator _sampleBootstrappingEstimator;

    public ErrorPredictor(ICoreLibAbstractFactory coreLibAbstractFactory)
    {
        _coreLibAbstractFactory = coreLibAbstractFactory ?? new CoreLibAbstractFactory();
        _initialValueCalculator = _coreLibAbstractFactory.CreateInitialValueCalculator();
        _lambdaGenerator = _coreLibAbstractFactory.CreateLambdaGenerator();
        _sampleBootstrappingEstimator = _coreLibAbstractFactory.CreateSampleBootstrappingEstimator();
        _poissonsRandomValuesGenerator = _coreLibAbstractFactory.CreatePoissonsRandomValuesGenerator();
    }

    public ErrorPredictionResult PredictErrors(GetErrorPredictionRequestModel requestModel)
    {
        var laMi = new double[requestModel.FileResult.ItemsCount];
        var faults = new double[requestModel.FileResult.ItemsCount];
        var tempFaults = new double[requestModel.FileResult.ItemsCount];

        var lamboot = new double[requestModel.Iterations];
        var muboot = new double[requestModel.Iterations];
        var muprob = new double[requestModel.Iterations];
        var mean = new double[requestModel.Iterations];

        double xFirst = 0, MU = 0, LAMBDA = 0, TotalTime = 0, TotalFaults = 0;
        double TotItFaults = 0, A = 0, B = 0, key = 0, percentile = 0, temp = 0;
        int low = 0, high = 0, other = 0;


        for (var i = 1; i < requestModel.FileResult.ItemsCount; i++)
            faults[i] = requestModel.FileResult.FaultsEnd[i] - requestModel.FileResult.FaultsEnd[i - 1];

        TotalTime = requestModel.FileResult.Times.Last();
        TotalFaults = requestModel.FileResult.FaultsEnd.Last();

        xFirst = _initialValueCalculator.Calculate(requestModel.FileResult);

        MU = _sampleBootstrappingEstimator.GetEstimation(new SampleBootstrapping(xFirst, TotalFaults, faults,
            TotalTime, requestModel.FileResult));
        LAMBDA = TotalFaults / (1 - Math.Exp(-1 * MU * TotalTime));

        laMi = _lambdaGenerator.Generate(new GenerateLambdaRequestModel(requestModel.FileResult, laMi, LAMBDA,
            MU));

        for (var i = 1; i < requestModel.Iterations; i++)
        {
            TotItFaults = 0;
            var poissonsResult = _poissonsRandomValuesGenerator.Generate(
                new GetRandomPoissonsRequestModel(requestModel.FileResult, laMi, tempFaults, TotItFaults));

            TotItFaults = poissonsResult.TotItFaults;
            tempFaults = poissonsResult.TempFaults;

            muboot[i] = _sampleBootstrappingEstimator.GetEstimation(
                new SampleBootstrapping(xFirst, TotalFaults, tempFaults, TotalTime, requestModel.FileResult));
            lamboot[i] = TotalFaults / (1 - Math.Exp(-1 * muboot[i] * TotalTime));
        }

        low = (int) Math.Round(requestModel.Iterations * (1 - requestModel.ConfidenceInterval));
        high = (int) Math.Round(requestModel.Iterations * requestModel.ConfidenceInterval);


        for (var i = 1; i < requestModel.Iterations; i++)
            mean[i] = lamboot[i] * Math.Exp(-1 * muboot[i]);
        // Not complete file. Should be code here   

        mean = mean.OrderBy(x => x).ToArray();


        return new ErrorPredictionResult
        {
            MeanErrors = mean,
            MaxErrorCount = mean[high],
            MinErrorCount = mean[1]
        };
    }
}