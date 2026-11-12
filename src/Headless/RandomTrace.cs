using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.Commands;
using Aow.Rtl;

namespace Aow.Headless;

internal sealed class RandomTrace(string path, Func<long> frame) : IDisposable
{
    private readonly StreamWriter _writer = new(path) { NewLine = "\n", AutoFlush = true };
    private readonly Dictionary<MethodBase, string?> _sources = [];

    public void Save(int day) => _writer.WriteLine(JsonSerializer.Serialize(new { kind = "save", day, frame = frame() }));

    public void Line(int location, int direction, int over, int count) => _writer.WriteLine(JsonSerializer.Serialize(new
    { kind = "line", frame = frame(), location, direction, over, count }));

    public void Cost(int cost, int used) => _writer.WriteLine(JsonSerializer.Serialize(new
    { kind = "cost", frame = frame(), cost, used }));

    public void Path(MoveTokenEvent move) => _writer.WriteLine(JsonSerializer.Serialize(new
    {
        kind = "path",
        frame = frame(),
        path = Enumerable.Range(0, move.Path.Count).Select(i => unchecked((uint)move.Path.List[i].Packed))
    }));

    public void Sort(DelphiList<EngineObject> list, ListSortCompare<EngineObject> compare)
    {
        if (compare.Method.Name is not ("CompareAIGroupTargetPriority" or "CompareTargetSetupPriority")) return;
        _writer.WriteLine(JsonSerializer.Serialize(new
        {
            kind = "targets",
            frame = frame(),
            compare = compare.Method.Name,
            targets = Enumerable.Range(0, list.Count).Select(i =>
            {
                var target = (AIGroupControlTarget)list[i]!;
                return new
                {
                    location = target.Location.Packed,
                    points = target.MovePoints,
                    priority = target.Data.Priority,
                    a = target.DataA,
                    b = target.DataB,
                    status = target.Status.Image
                };
            })
        }));
    }

    public void Rounding(double value, int result, int singleResult)
    {
        var caller = new StackTrace(1, true).GetFrames()
            .FirstOrDefault(f => f.GetMethod()?.DeclaringType?.Assembly.GetName().Name == "Aow.Game");
        _writer.WriteLine(JsonSerializer.Serialize(new
        {
            kind = "rounding",
            frame = frame(),
            value,
            result,
            singleResult,
            caller = Name(caller),
            source = Source(caller)
        }));
    }

    public void Record(RandomTraceEvent entry)
    {
        var stack = new StackTrace(1, true).GetFrames()
            .Where(f => f.GetMethod()?.DeclaringType?.Assembly.GetName().Name == "Aow.Game")
            .ToArray();
        bool map = stack.FirstOrDefault()?.GetMethod() is { Name: "Random", DeclaringType.Name: "AoWHSMap" };
        var caller = stack.Skip(map ? 1 : 0).FirstOrDefault();
        _writer.WriteLine(JsonSerializer.Serialize(new
        {
            frame = frame(),
            kind = entry.Kind,
            map,
            argument = entry.Argument,
            result = entry.Result,
            before = unchecked((uint)entry.Before),
            after = unchecked((uint)entry.After),
            caller = Name(caller),
            source = Source(caller),
            stack = stack.Take(8).Select(f => new { method = Name(f), source = Source(f) })
        }));
    }

    private static string? Name(StackFrame? frame) => frame?.GetMethod() is { } method
        ? $"{method.DeclaringType?.Name}.{method.Name}" : null;

    private string? Source(StackFrame? frame)
    {
        if (frame?.GetMethod() is not { } method) return null;
        if (_sources.TryGetValue(method, out var source)) return source;
        if (frame.GetFileName() is { } file && File.Exists(file))
        {
            var lines = File.ReadAllLines(file);
            for (int i = Math.Min(frame.GetFileLineNumber() - 1, lines.Length - 1); i >= 0; i--)
            {
                var match = Regex.Match(lines[i], @"// (\S+\.pas:\d+)", RegexOptions.IgnoreCase);
                if (match.Success) { source = match.Groups[1].Value; break; }
            }
        }
        _sources[method] = source;
        return source;
    }

    public void Dispose() => _writer.Dispose();
}
