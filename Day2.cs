namespace aoc2024;

public class Day2(int day, int input, bool isTest) : DayBase(day, input, isTest)
{
    protected override int Run1()
    {
        var input = GetInput();
        var numSafe = 0;
        foreach (var line in input)
        {
            var numbers = line.Split(" ").Select(int.Parse).ToList();
            var (isSafe, _) = IsReportSafe(numbers);
            if (isSafe) numSafe++;
        }
        return numSafe;
    }

    protected override int Run2()
    {
        var input = GetInput();
        var numSafe = 0;
        foreach (var line in input)
        {
            var numbers = line.Split(" ").Select(int.Parse).ToList();
            var (isSafe, unsafeIndex) = IsReportSafe(numbers);
            
            if (!isSafe && unsafeIndex != null)
            {
                var startIndex = Math.Max(0, unsafeIndex.Value - 2);
                var endIndex = Math.Min(numbers.Count, unsafeIndex.Value + 1);
                for (var i = startIndex; i<endIndex; i++)
                {
                    var numbersCopy = numbers.ToList();
                    numbersCopy.RemoveAt(i);
                    (isSafe, _) = IsReportSafe(numbersCopy);
                    if (isSafe) break;
                }
            }

            if (isSafe) numSafe++;
        }
        return numSafe;
    }

    private static (bool, int?) IsReportSafe(List<int> numbers)
    {
        if (numbers[1] == numbers[0]) return (false, 1);
        var isAscending = numbers[1] > numbers[0];
        var index = 0;

        while (index < numbers.Count - 1)
        {
            var value = numbers[index];
            var nextValue = numbers[index + 1];
            index++;
            
            var diff = nextValue - value;
            var distance = Math.Abs(diff);
            if (distance is > 0 and < 4 &&
                (!isAscending || diff > 0) &&
                (isAscending || diff < 0))
            {
                continue;
            }

            return (false, index);
        }

        return (true, null);
    }
}