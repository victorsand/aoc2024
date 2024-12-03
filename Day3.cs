using System.Text.RegularExpressions;

namespace aoc2024;

public class Day3(int day, int input, bool isTest) : DayBase(day, input, isTest)
{
    protected override int Run1()
    {
        var input = GetInput();
        var sum = 0;
        foreach (var instructions in input)
        {
            var operations = Regex.Matches(instructions, @"mul\(\d+,\d+\)");
            foreach (Match match in operations)
            {
                sum += CalculateInstruction(match.Value);
            }
        }
        return sum;
    }

    private static int CalculateInstruction(string instruction)
    {
        return instruction
            .Replace("mul(", "")
            .Replace(")", "")
            .Split(",")
            .Select(int.Parse)
            .Aggregate(1, (x, y) => x * y);
    }

    protected override int Run2()
    {
        var input = GetInput();
        var instructions = $@"don't()do(){string.Join("", input)}";
        var sum = 0;
        var doIndexes = Regex.Matches(instructions, @"do\(\)").Select(m => m.Index).Order().ToList();
        var dontIndexes = Regex.Matches(instructions, @"don't\(\)").Select(m => m.Index).Order().ToList();
        var operations = Regex.Matches(instructions, @"mul\(\d+,\d+\)").OrderBy(m => m.Index).ToList();
        foreach (var operation in operations)
        {
            var operationIndex = operation.Index;
            var closestDoIndex = doIndexes.Last(i => i < operationIndex);
            var closestDontIndex = dontIndexes.Last(i => i < operationIndex);
            if (closestDoIndex > closestDontIndex) sum += CalculateInstruction(operation.Value);
        }
        return sum;
    }
}