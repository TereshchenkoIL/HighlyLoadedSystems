using CoreLib.FileReader;
using CoreLib.Implementation;
using CoreLib.Models;

Console.WriteLine("Считываем из файла данные об ошибках");

Console.WriteLine("Введите время прогнозирования - любое положительное число");
Console.WriteLine("ВНИМАНИЕ");
Console.WriteLine("Все еденицы времени должны иметь одинаковую размерность");

var predictTime = int.Parse(Console.ReadLine());

var errorPredictorFactory = new ErrorPredictorFactory();

var fileResults = new FileReader().GetFileStatistics("errors.rxr");
var errorPredictor = errorPredictorFactory.CreateErrorPredictor();

var predictionResult = errorPredictor.PredictErrors(new GetErrorPredictionRequestModel
{
    FileResult = fileResults,
    Iterations = 10,
    PredictTime = predictTime,
    ConfidenceInterval = 0.95
});

Console.WriteLine("Введен доверительный интервал 95%");

Console.WriteLine($"Через следующие {predictTime} едениц времени");

Console.WriteLine($"Ожидается максимальное число ошибок {predictionResult.MaxErrorCount} и минимальное число ошибое {predictionResult.MinErrorCount}\n\n");

Console.WriteLine("Хотите ввести другой доверительный интервал?");

Console.WriteLine("Да - нажмите 1, иначе любое другое число");

var answer = Console.ReadLine();

if (answer == "1")
{
    Console.WriteLine("Введите величину доверительного интервала, например 0.90");

    var percentile = double.Parse(Console.ReadLine());
    
    predictionResult = errorPredictor.PredictErrors(new GetErrorPredictionRequestModel
    {
        FileResult = fileResults,
        Iterations = 10,
        PredictTime = predictTime,
        ConfidenceInterval = percentile
    });

    Console.WriteLine($"С {percentile * 100} $ доверительным интервалом");
    
    Console.WriteLine($"Через следующие {predictTime} едениц времени");
    
    Console.WriteLine($"Ожидается максимальное число ошибок {predictionResult.MaxErrorCount} и минимальное число ошибое {predictionResult.MinErrorCount}\n\n");
}