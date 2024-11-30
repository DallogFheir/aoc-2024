using System.Reflection;

var day = DateTime.Now.Day;
string[] parts = ["1", "2"];
switch (args.Length)
{
    case 1:
        day = int.Parse(args[0]);
        break;
    case > 1:
        parts = [args[1]];
        goto case 1;
}

string className = $"Aoc2024.Day{day:00}.Solution";
var methodNames = parts.Select(part => $"Part{part}");

Type? type = Type.GetType(className);

if (type == null)
{
    Console.WriteLine($"Class {className} not found.");
    return;
}

Console.WriteLine($"DAY {day}");
foreach (var methodName in methodNames)
{
    MethodInfo? method = type.GetMethod(methodName);

    if (method == null)
    {
        Console.WriteLine($"Method {methodName} not found on type {className}.");
        continue;
    }

    method.Invoke(null, null);
}
