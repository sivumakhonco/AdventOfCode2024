// See https://aka.ms/new-console-template for more information

/*
Console.WriteLine($"{AssessReportSafety([27, 29, 30, 33, 34, 35, 37, 35])}");
var safe = AssessReportSafety([1, 1, 1, 1]);
Console.WriteLine($"Is the report safe? {safe}");
Console.WriteLine($"{AssessReportSafety([64, 63, 65, 65, 63])}");
Console.WriteLine($"{AssessReportSafety([51, 53, 54, 55, 57, 60, 63, 63])}");
Console.WriteLine($"{AssessReportSafety([32, 33, 36, 37, 34, 36, 39, 37])}");
Console.WriteLine($"{AssessReportSafety([7, 10, 8, 10, 11])}");
safe = IsReportSafe([7, 6, 4, 2, 1]);
Console.WriteLine($"Is the report safe? {safe}");
safe = IsReportSafe([1, 2, 7, 8, 9]);
Console.WriteLine($"Is the report safe? {safe}");
safe = IsReportSafe([9, 7, 6, 2, 1]);
Console.WriteLine($"Is the report safe? {safe}");
safe = IsReportSafe([1, 3, 2, 4, 5]);
Console.WriteLine($"Is the report safe? {safe}");
safe = IsReportSafe([8, 6, 4, 4, 1]);
Console.WriteLine($"Is the report safe? {safe}");
safe = IsReportSafe([1, 3, 6, 7, 9]);
Console.WriteLine($"Is the report safe? {safe}");
*/

const string filePath = "input.txt";
var fileLines = File.ReadAllLines(filePath);
Console.WriteLine($"There are {ComputeSafeReports(fileLines)} valid reports in the input file.");
Console.WriteLine($"There are {ComputeSafeReportsWithTolerance(fileLines)} valid reports in the input file with a tolerance of 1.");

return;

static int ComputeSafeReports(string[] fileLines)
{
    int safeReports = 0;
    foreach (var line in fileLines)
    {
        var report = line.Split(' ').Select(int.Parse).ToArray();
        var increasing = report[0] < report[1];
        safeReports += 1;
        for (int i = 0; i < report.Length - 1; i++)
        {
            var difference = increasing ? report[i + 1] - report[i] : report[i] - report[i + 1];
            if (difference == 0)
            {
                safeReports--;
                break;
            }

            if (difference is < 1 or > 3)
            {
                safeReports--;
                break;
            }
        }
    }

    return safeReports;
}

static int ComputeSafeReportsWithTolerance(string[] fileLines, int tolerance = 1)
{
    var safeReports = 0;
    foreach (var line in fileLines)
    {
        var report = line.Split(' ').Select(int.Parse).ToArray();

        if (AssessReportSafety(report, tolerance) is true)
        {
            safeReports++;
            continue;
        }
        Console.WriteLine(line);
    }

    return safeReports;
}

static (bool isSafe, bool? increasing) IsValidPair(int a, int b, bool? expectIncreasingTrend = null)
{
    if (a == b) return (false, null);
    bool increasing = b > a;
    int difference = Math.Abs(b - a);
    bool isSafe = difference is > 0 and < 4;
    if(expectIncreasingTrend is null) return (isSafe, increasing);
    if(expectIncreasingTrend.Value && !increasing) return (false, increasing);
    if(!expectIncreasingTrend.Value && increasing) return (false, increasing);
    return (isSafe, increasing);
}

static bool AssessReportSafety(int[] report, int tolerance = 1)
{
    var distinctValues = report.Distinct().ToArray();
    if (distinctValues.Length == 1) return false;

    bool increasingTrend = distinctValues[0] < distinctValues[1];
    for (int i = 0; i < report.Length - 1; i++)
    {
        var (isSafe, increasing) = IsValidPair(report[i], report[i + 1], increasingTrend);
        if (increasing is null)
        {
            if (i + 2 == report.Length)
            {
                return tolerance > 0;
            }

            if (tolerance == 0)
            {
                return false;
            }
            return AssessReportSafety(report[..i].Concat(report[(i + 1)..]).ToArray(), tolerance);
        }

        if (increasingTrend != increasing.Value && tolerance <= 0)
        {
            return false;
        }
        if (isSafe) continue;

        var array = report[..i].Concat(report[(i + 1)..]).ToArray(); ;
        return array.Length <= 1 || AssessReportSafety(array, tolerance - 1);
    }
    return true;
}
