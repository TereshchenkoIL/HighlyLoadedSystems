namespace CoreLib.Abstractions;

public interface IErrorPredictorFactory
{
    IErrorPredictor CreateErrorPredictor();
}