// See https://aka.ms/new-console-template for more information

using Initial;

/*
 *


 */
double Findmu(double Xone, double Tfaults, double[] F, double Totaltime, FileReaderResult result)
{
    double Mucurrent= 0.0, Munew= 0.0, s= 0.0, x= 0.0, t1= 0.0, t2= 0.0, t3= 0.0, t4 = 0.0;
    int i;
    
    Mucurrent = (((-1) * Math.Log(Xone)) / Totaltime);

    while ((Math.Abs(Munew - Mucurrent)) <= Math.E)
    {
        for (i = 1; i < result.ItemsCount; i++)
        {
            t1 = result.Times[i] * Math.Exp((-1) * Mucurrent * result.Times[i]);
            t2 = result.Times[i - 1] * Math.Exp((-1) * Mucurrent * result.Times[i - 1]);
            t3 = Math.Exp((-1) * Mucurrent * result.Times[i - 1]) - Math.Exp( (-1) * Mucurrent * result.Times[i]);
            t4 = t4 + (F[i] * ((t1 - t2) / t3)); 
        }

        if (t4 < (0.01))
        {
            t4 = 0.01;
            s = (t4 / (Tfaults * Totaltime));
            x = (s / (1 + s)); 
            t4 = 0;
        }
        Munew = (((-1) * Math.Log(x)) / Totaltime);
    }

    return Mucurrent;
}

double Firstxvalue(FileReaderResult result)
{
    double firstHalfValue = 0;
    double secondHalfValue = 0;
    int i = 1;
    double TotalTime = 0;
    double  searchValue = (TotalTime / 2);

    var Times = result.Times;
    var Faultsend = result.FaultsEnd;
    while (result.Times[i] >= searchValue)
    {
        i += 1;
    }
    
    firstHalfValue =  (Faultsend[i - 1] + (Faultsend[i] -Faultsend[i -  1]) * (searchValue - Times[i - 1]) / (Times[i] -Times[i - 1]));
    secondHalfValue = (Faultsend[result.ItemsCount] -firstHalfValue);
    return ((secondHalfValue / firstHalfValue) * (secondHalfValue / firstHalfValue));

}

void GenerateLambdas(FileReaderResult result, double[] LAMi, double LAMBDA, double MU)
{
    for (int i = 2; i < result.ItemsCount; i++)
    {
        LAMi[i] = (LAMBDA * Math.Exp(-1 * MU * result.Times[i - 1]) - Math.Exp(-1 * MU * result.Times[i]));
    }

    LAMi[1] = (LAMBDA * (1 -Math.Exp(-1 * MU * result.Times[1])));
}

void AssignPoissons(FileReaderResult result, double[] LAMi, double[] tempFaults, double TotItFaults)
{
    double A, B, W, X, U;
    for (int i = 1; i < result.ItemsCount; i++)
    {
        W = Math.Exp((-1) * LAMi[i]);
        X = 0;
        A = W;
        B = A;
        U = new Random().NextDouble() * 1000 / 1000;

        while (U > A && A <= 0.9999)
        {
            X = X + 1;
            B = (B * LAMi[i]) / X;
            A = A + B;
        }

        tempFaults[i] = X;
        TotItFaults = TotItFaults + tempFaults[i];
    }
}

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
int j = 0, k = 0, low = 0, high = 0, T = 0, other = 0;
string Cumfail;

Console.WriteLine("Считываем из файла данные об ошибках");

for (int i = 1; i < result.ItemsCount; i++)
{
    Faults[i] = result.FaultsEnd[i] - result.FaultsEnd[i - 1];
}

TotalTime = result.Times.Last();
TotalFaults = result.FaultsEnd.Last();

XFirst = Firstxvalue(result);

MU = Findmu(XFirst, TotalFaults, Faults, TotalTime, result);
LAMBDA = (TotalFaults / (1 - Math.Exp((-1) * MU * TotalTime)));

GenerateLambdas(result, LAMi, LAMBDA, MU);

for (int i = 1; i < iteration; i++)
{
    TotItFaults = 0;
    AssignPoissons(result, LAMi, tempFaults, TotItFaults);

    muboot[i] = Findmu(XFirst, TotalFaults, tempFaults, TotalTime, result);
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