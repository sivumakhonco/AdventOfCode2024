// See https://aka.ms/new-console-template for more information

const string filePath = "input.txt";
var fileLines = File.ReadAllLines(filePath);

Console.WriteLine($"There are {ComputeSafeReports(fileLines)} valid reports in the input file.");
Console.WriteLine($"There are {ComputeSafeReports(fileLines, 1)} valid reports in the input file with the dampener.");

return;

static int ComputeSafeReports(string[] fileLines, int tolerance = 0)
{
    int safeReports = 0;
    foreach (var line in fileLines)
    {
        var report = line.Split(' ').Select(int.Parse).ToArray();
        if (IsReportSafe(report, tolerance))
        {
            safeReports++;
            continue;
        }

        if (tolerance == 0) continue;

        int index = 0;
        while (index < report.Length - 1)
        {
            var alteredList = new List<int>(report);
            alteredList.RemoveAt(index);
            if(IsReportSafe(alteredList.ToArray(), 0))
            {
                safeReports++;
                break;
            }
            index++;
        }
    }

    return safeReports;
}

static bool IsReportSafe(int[] report, int tolerance)
{
    if(report.Length == 1) return true;
    bool increasing = report[0] < report[1];

    for (int i = 0; i < report.Length - 1; i++)
    {
        var difference = report[i + 1] - report[i];
        switch (increasing)
        {
            case true when difference < 0:
            case false when difference > 0:
                if(tolerance == 0) return false;
                var removedLeftItem = report[..i].Concat(report[(i + 1)..]).ToArray();
                var removedRightItem = report[..(i + 1)].Concat(report[(i + 2)..]).ToArray();
                return IsReportSafe(removedRightItem, tolerance - 1) ||
                       IsReportSafe(removedLeftItem, tolerance - 1);
        }

        if (difference == 0)
        {
            if(tolerance == 0) return false;
            var removedLeftItem = report[..i].Concat(report[(i + 1)..]).ToArray();
            var removedRightItem = report[..(i + 1)].Concat(report[(i + 2)..]).ToArray();
            return IsReportSafe(removedRightItem, tolerance - 1) ||
                   IsReportSafe(removedLeftItem, tolerance - 1);
        }

        if (Math.Abs(difference) is < 1 or > 3)
        {
            if (tolerance == 0) return false;
            var removedLeftItem = report[..i].Concat(report[(i + 1)..]).ToArray();
            var removedRightItem = report[..(i + 1)].Concat(report[(i + 2)..]).ToArray();
            return IsReportSafe(removedRightItem, tolerance - 1) ||
                   IsReportSafe(removedLeftItem, tolerance - 1);
        }
    }
    return true;
}
