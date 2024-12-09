// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, Advent of Code!");
var (leftLocations, rightLocations) = ReadLocationsInput();
var totalDifference = 0;
for (int i = 0; i < leftLocations.Count; i++)
{
    var difference = Math.Abs(leftLocations[i] - rightLocations[i]);
    totalDifference += difference;
}
Console.WriteLine($"Total distance is: {totalDifference}");
Console.WriteLine($"Total similarity score is: {CalculateSimilarityScore(leftLocations, rightLocations)}");
return;

static (List<int>, List<int>) ReadLocationsInput()
{
    var fileText = File.ReadAllLines("locations.txt");
    var (leftLocations, rightLocations) = (new List<int>(), new List<int>());
    foreach (var line in fileText)
    {
        var lineParts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var leftPart = lineParts[0].Trim();
        var rightPart = lineParts[1].Trim();
        leftLocations.Add(int.Parse(leftPart));
        rightLocations.Add(int.Parse(rightPart));
    }
    leftLocations.Sort();
    rightLocations.Sort();
    return (leftLocations, rightLocations);
}

static int CalculateSimilarityScore(List<int> left, List<int> right)
{
    int score = 0;
    foreach (var item in left)
    {
        var occurenceCount = right.Count(x => x == item);
        score += (occurenceCount * item);
    }
    return score;
}