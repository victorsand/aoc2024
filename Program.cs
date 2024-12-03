
var day = int.Parse(args[0]);
var input = int.Parse(args[1]);
var isTest = args.Length > 2 && args[2] == "test";
var type = Type.GetType($"aoc2024.Day{day}");
var obj = Activator.CreateInstance(type!, day, input, isTest);
var method = type!.GetMethod("Run");
var result = method!.Invoke(obj, null);
Console.WriteLine(result!);
Environment.Exit(0);