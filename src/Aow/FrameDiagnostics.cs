using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Aow.Game.Draw.Images;

using Aow.Game.Host;
using Aow.Game.UI.Dialogs;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow;

internal sealed class FrameDiagnostics : IDisposable
{
    private readonly string _output;
    private readonly StreamWriter _writer;
    private readonly Action<RandomTraceEvent>? _previousRandom;
    private readonly Action<int, int>? _previousCost;

    private FrameDiagnostics(string output)
    {
        this._output = output;
        _writer = new(Path.Combine(output, "diagnostic-trace.jsonl")) { NewLine = "\n", AutoFlush = true };
        _previousRandom = RtlUnit.RandomTrace;
        _previousCost = HeadlessHooks.ProcessCost;
        RtlUnit.RandomTrace = Random;
        HeadlessHooks.ProcessCost = Cost;
    }

    public static FrameDiagnostics? Start(string output) =>
        Environment.GetEnvironmentVariable("AOW_FRAME_DIAGNOSTICS") == "1" ? new(output) : null;

    private void Random(RandomTraceEvent entry)
    {
        if (entry.Kind == "random" && (StrategicMap.Current is null || StrategicMap.Current is { Day: 1, Frame: <= 1 }))
            _writer.WriteLine(JsonSerializer.Serialize(new
            {
                kind = "startup-random",
                argument = entry.Argument,
                before = unchecked((uint)entry.Before),
                after = unchecked((uint)entry.After),
                result = entry.Result,
                stack = new StackTrace(1, false).GetFrames().Select(frame => frame.GetMethod())
                    .Where(method => method?.DeclaringType?.Assembly.GetName().Name == "Aow.Game")
                    .Select(method => $"{method!.DeclaringType!.Name}.{method.Name}")
            }));
        if (StrategicMap.Current is not { Day: 2 } map || entry.Kind != "random") return;
        var stack = new StackTrace(1, false).GetFrames().Select(frame => frame.GetMethod())
            .Where(method => method?.DeclaringType?.Assembly.GetName().Name == "Aow.Game").ToArray();
        if (!stack.Any(method => method is { Name: "Random", DeclaringType.Name: nameof(StrategicMap) })) return;
        _writer.WriteLine(JsonSerializer.Serialize(new
        {
            kind = "random",
            day = map.Day,
            frame = map.Frame,
            current_player = map.CurrentPlayer,
            ai_player = map.AIExecutor.Player,
            argument = entry.Argument,
            before = unchecked((uint)entry.Before),
            after = unchecked((uint)entry.After),
            result = entry.Result,
            stack = stack.Select(method => $"{method!.DeclaringType!.Name}.{method.Name}")
        }));
    }

    private void Cost(int cost, int used)
    {
        if (StrategicMap.Current is not { Day: 2 } map) return;
        _writer.WriteLine(JsonSerializer.Serialize(new
        {
            kind = "cost",
            day = map.Day,
            frame = map.Frame,
            ai_player = map.AIExecutor.Player,
            cost,
            used,
            stack = new StackTrace(1, false).GetFrames().Select(frame => frame.GetMethod())
                .Where(method => method?.DeclaringType?.Assembly.GetName().Name == "Aow.Game")
                .Take(10).Select(method => $"{method!.DeclaringType!.Name}.{method.Name}")
        }));
    }

    public void Snapshot(string name, UIContext ui)
    {
        if (StrategicMap.Current is not { } map) return;
        var viewer = ui.MapWindow.MapViewer;
        var window = ui.MapWindow.MainWindow;
        var level = map.MapContainer.GetMapLevel(viewer.SceneL);
        var objects = new List<object>();
        for (int y = 0; y < level.Height; y++)
            for (int x = 0; x < level.Width; x++)
            {
                int px = x * 32 + 8 - viewer.SceneX + window.WinLeft;
                int py = y * 32 + (x & 1) * 16 - viewer.SceneY + window.WinTop;
                if (px < -128 || py < -128 || px > window.WinWidth + 128 || py > window.WinHeight + 128) continue;
                var field = level.GetField(x, y);
                for (int i = 0; i < field.GetCount(); i++)
                {
                    var child = field.GetChild(i);
                    var resource = (child as MapObject)?.Resource;
                    var sequences = new List<object>();
                    if (resource?.ImageSequenceList is { } list)
                        for (int index = 0; index < list.Count; index++)
                            for (var sequence = list.GetImageSequence(index); sequence is not null; sequence = sequence.Next)
                                sequences.Add(new
                                {
                                    index,
                                    name = sequence.ImageLibraryName,
                                    mode = sequence.SequenceMode.ToString(),
                                    sequence.SpeedCount,
                                    sequence.ImageIndex,
                                    lastFrame = typeof(ImageSequence).GetField("_lastFrame", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(sequence),
                                    counter = typeof(ImageSequence).GetField("_frameCounter", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(sequence)
                                });
                    objects.Add(new
                    {
                        x,
                        y,
                        px,
                        py,
                        type = child.GetType().Name,
                        resource = resource?.GetType().Name,
                        name = resource?.Name,
                        sequences
                    });
                }
            }
        File.WriteAllText(Path.Combine(_output, "diagnostic-" + name + ".json"), JsonSerializer.Serialize(new
        {
            FDay = map.Day,
            map.Frame,
            viewer.SceneX,
            viewer.SceneY,
            viewer.SceneL,
            objects
        }, new JsonSerializerOptions { WriteIndented = true }) + "\n");
    }

    public void Dispose()
    {
        RtlUnit.RandomTrace = _previousRandom;
        HeadlessHooks.ProcessCost = _previousCost;
        _writer.Dispose();
    }
}
