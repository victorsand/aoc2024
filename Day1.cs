using System.Text.RegularExpressions;

namespace aoc2024;

public class Day1(int day, int input, bool isTest) : DayBase(day, input, isTest)
{
    protected override int Run1()
    {
        var left = new List<int>();
        var right = new List<int>();

        var input = GetInput();
        
        foreach (var line in input)
        {
            var cleaned = Regex.Replace(line, @"\s+", " ");
            var nums = cleaned.Split(" ");
            left.Add(int.Parse(nums[0]));
            right.Add(int.Parse(nums[1]));
        }

        left = left.Order().ToList();
        right = right.Order().ToList();

        var distance = 0;
        var index = 0;
        while (index < left.Count)
        {
            distance += Math.Abs(left[index] - right[index]);
            index++;
        }

        return distance;
    }

    protected override int Run2()
    {
        var left = new List<int>();
        var right = new Dictionary<int, int>();

        var input = GetInput();

        foreach (var line in input)
        {
            var cleaned = Regex.Replace(line, @"\s+", " ");
            var nums = cleaned.Split(" ");
            left.Add(int.Parse(nums[0]));

            var rightVal = int.Parse(nums[1]);
            if (!right.TryAdd(rightVal, 1))
            {
                right[rightVal]++;
            }
        }
        
        var similarity = 0;
        var index = 0;
        while (index < left.Count)
        {
            var leftVal = left[index];
            if (right.TryGetValue(leftVal, out var freq))
            {
                similarity += leftVal * freq;
            }
            index++;
        }

        return similarity;
    }
}