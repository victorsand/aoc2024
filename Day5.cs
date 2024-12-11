namespace aoc2024;

public class Day5(int day, int input, bool isTest) : DayBase(day, input, isTest)
{
    protected override int Run1()
    {
        var (orderingRules, updates) = GenerateRulesAndUpdates();
        var map = GenerateOrderingMap(orderingRules);
        return updates.Sum(u => IsUpdateInRightOrder(u, map) ? u[u.Count / 2] : 0);
    }

    protected override int Run2()
    {
        var (orderingRules, updates) = GenerateRulesAndUpdates();
        var map = GenerateOrderingMap(orderingRules);
        var sum = 0;
        foreach (var update in updates)
        {
            if (IsUpdateInRightOrder(update, map)) continue;
            var newOrder = Reorder(update, map);
            sum += newOrder[newOrder.Count / 2];
        }
        return sum;
    }
    
    private (List<(int, int)>, List<List<int>>) GenerateRulesAndUpdates()
    {
        
        var lines = GetInput();

        var orderingRules = new List<(int, int)>();
        var updates = new List<List<int>>();

        var isParsingRules = true;
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                isParsingRules = false;
                continue;
            }
            if (isParsingRules)
            {
                var rules = line.Split("|");
                orderingRules.Add((int.Parse(rules.First()), int.Parse(rules.Last())));
            }
            else
            {
                updates.Add(line.Split(",").Select(int.Parse).ToList());
            }
        }
        return (orderingRules, updates);
    }

    private List<int> Reorder(List<int> update, Dictionary<int, HashSet<int>> orderingMap)
    {
        var missingKeys = update.Where(u => !orderingMap.ContainsKey(u)).ToList();
        var mappedValues = new Dictionary<int, HashSet<int>>();
        foreach (var (k, v) in orderingMap)
        {
            if (!update.Contains(k)) continue;
            var values = new HashSet<int>(v);
            values.RemoveWhere(val => !update.Contains(val));
            mappedValues.Add(k, [..values]);
        }
        var freq = mappedValues.OrderByDescending(o => o.Value.Count).ToList();
        var result = freq.Select(f => f.Key).ToList();
        result.AddRange(missingKeys);
        return result;
    }

    private bool IsUpdateInRightOrder(List<int> update, Dictionary<int, HashSet<int>> orderingMap)
    {
        for (var i=1; i<update.Count; i++)
        {
            var num = update[i];
            if (!orderingMap.TryGetValue(num, out var map)) continue;
            for (var j = 0; j < i; j++)
            {
                var numBefore = update[j];
                if (map.Contains(numBefore))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private Dictionary<int, HashSet<int>> GenerateOrderingMap(List<(int, int)> rules)
    {
        var map = new Dictionary<int, HashSet<int>>();
        foreach (var rule in rules)
        {
            if (map.TryGetValue(rule.Item1, out var entry))
            {
                entry.Add(rule.Item2);
            }
            else
            {
                map.Add(rule.Item1, [rule.Item2]);
            }
        }
        return map;
    }

}