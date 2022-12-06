// See https://aka.ms/new-console-template for more information

using CoreLib.FileReader;
using CoreLib.Implementation;
using CoreLib.Models;

FileReaderResult result = new FileReaderResult
{
    Times = new double[] {1, 5, 12, 17, 25, 26, 32, 37, 52, 67},
    FaultsEnd = new double[]{1, 3, 4, 6, 10, 6, 7, 3, 4, 2},
    ItemsCount = 10
};

int iteration = 10;

double[] LAMi = new double[result.ItemsCount];
double[] Faults = new double[result.ItemsCount];
double[] tempFaults = new double[result.ItemsCount];

var lamboot = new double[iteration];
var muboot = new double[iteration];
var muprob =  new double[iteration];
var mean =  new double[iteration];

double XFirst = 0, MU= 0, LAMBDA= 0, TotalTime= 0, TotalFaults= 0;
double TotItFaults= 0, A= 0, B= 0, key= 0, percentile = 0, temp= 0;
int low = 0, high = 0, T = 0, other = 0;

Console.WriteLine("Считываем из файла данные об ошибках");

for (int i = 1; i < result.ItemsCount; i++)
{
    Faults[i] = result.FaultsEnd[i] - result.FaultsEnd[i - 1];
}

TotalTime = result.Times.Last();
TotalFaults = result.FaultsEnd.Last();

XFirst = new InitialValueCalculator().Calculate(result);

MU = new SampleBootstrappingEstimator().GetEstimation(new SampleBootstrapping(XFirst, TotalFaults, Faults, TotalTime, result));
LAMBDA = (TotalFaults / (1 - Math.Exp((-1) * MU * TotalTime)));

LAMi = new LambdaGenerator().Generate(new GenerateLambdaRequestModel(result, LAMi, LAMBDA, MU));

for (int i = 1; i < iteration; i++)
{
    TotItFaults = 0;
    var poissonsResult = new PoissonsRandomValuesGenerator().Generate(new GetRandomPoissonsRequestModel(result, LAMi, tempFaults, TotItFaults));

    TotItFaults = poissonsResult.TotItFaults;
    tempFaults = poissonsResult.TempFaults;

    muboot[i] = new SampleBootstrappingEstimator().GetEstimation(new SampleBootstrapping(XFirst, TotalFaults, tempFaults, TotalTime, result));
    lamboot[i] = (TotalFaults / (1 - Math.Exp((-1) * muboot[i] * TotalTime)));
}

low = (int)Math.Round(iteration * 0.05);
high =  (int)Math.Round(iteration * 0.95);

Console.WriteLine("Введите время прогнозирования - любое положительное число");
Console.WriteLine("ВНИМАНИЕ");
Console.WriteLine("Все еденицы времени должны иметь одинаковую размерность");

T = int.Parse(Console.ReadLine());

for (int i = 1; i < iteration; i++)
{
    mean[i] = lamboot[i] * (Math.Exp((-1) * muboot[i]));
    // Not complete file. Should be code here
}

mean = mean.OrderBy(x => x).ToArray();


Console.WriteLine("Введен доверительный интервал 95%");

Console.WriteLine($"Через следующие {T} едениц времени");

Console.WriteLine($"Ожидается максимальное число ошибок {mean[high]} и минимальное число ошибое {mean[1]}\n\n");

Console.WriteLine("Хотите ввести другой доверительный интервал?");

Console.WriteLine("Да - нажмите 1, иначе любое другое число");

var answer = Console.ReadLine();

if (answer == "1")
{
    Console.WriteLine("Введите величину доверительного интервала, например 0.90");

    percentile = double.Parse(Console.ReadLine());
    
    low = (int)Math.Round(iteration * (1 - percentile));
    high =  (int)Math.Round(iteration * percentile);

    Console.WriteLine($"С {percentile * 100} $ доверительным интервалом");
    
    Console.WriteLine($"Через следующие {T} едениц времени");
    
    Console.WriteLine($"Ожидается максимальное число ошибок {mean[high]} и минимальное число ошибое {mean[1]}\n\n");
}