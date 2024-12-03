namespace aoc2024;

public abstract class DayBase(int day, int input, bool isTest)
{
    protected List<string> GetInput()
    {
        return File.ReadAllLines($"input/day{day}/{input}{(isTest ? "test" : "")}.txt").ToList();
    }

    public int Run()
    {
        return input switch
        {
            1 => Run1(),
            2 => Run2(),
            _ => throw new ArgumentException()
        };
    }

    protected abstract int Run1();
    protected abstract int Run2();
}