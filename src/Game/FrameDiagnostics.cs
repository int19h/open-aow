using System.Diagnostics;
using System.Text.Json;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Ui.Windows;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow;

internal sealed class FrameDiagnostics : IDisposable
{
    private readonly string output;
    private readonly StreamWriter writer;
    private readonly Action<RandomTraceEvent>? previousRandom;
    private readonly Action<int, int>? previousCost;

    private FrameDiagnostics(string output)
    {
        this.output = output;
        writer = new(Path.Combine(output, "diagnostic-trace.jsonl")) { NewLine = "\n", AutoFlush = true };
        previousRandom = RtlUnit.RandomTrace;
        previousCost = HeadlessHooks.ProcessCost;
        RtlUnit.RandomTrace = Random;
        HeadlessHooks.ProcessCost = Cost;
    }

    public static FrameDiagnostics? Start(string output) =>
        Environment.GetEnvironmentVariable("AOW_FRAME_DIAGNOSTICS") == "1" ? new(output) : null;

    private void Random(RandomTraceEvent entry)
    {
        if (entry.Kind == "random" && (Aow.Game.World.Map.TAoWHSMap.AoWHSMap is null || Aow.Game.World.Map.TAoWHSMap.AoWHSMap is { FDay: 1, Frame: <= 1 }))
            writer.WriteLine(JsonSerializer.Serialize(new
            {
                kind = "startup-random",
                argument = entry.Argument,
                before = unchecked((uint)entry.Before),
                after = unchecked((uint)entry.After),
                result = entry.Result,
                stack = new StackTrace(1, false).GetFrames().Select(frame => frame.GetMethod())
                    .Where(method => method?.DeclaringType?.Assembly.GetName().Name == "Aow.Units")
                    .Select(method => $"{method!.DeclaringType!.Name}.{method.Name}")
            }));
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap is not { FDay: 2 } map || entry.Kind != "random") return;
        var stack = new StackTrace(1, false).GetFrames().Select(frame => frame.GetMethod())
            .Where(method => method?.DeclaringType?.Assembly.GetName().Name == "Aow.Units").ToArray();
        if (!stack.Any(method => method is { Name: "Random", DeclaringType.Name: "TAoWHSMap" })) return;
        writer.WriteLine(JsonSerializer.Serialize(new
        {
            kind = "random",
            day = map.FDay,
            frame = map.Frame,
            current_player = map.FCurrentPlayer,
            ai_player = map.FAIExecuter.FPlayer,
            argument = entry.Argument,
            before = unchecked((uint)entry.Before),
            after = unchecked((uint)entry.After),
            result = entry.Result,
            stack = stack.Select(method => $"{method!.DeclaringType!.Name}.{method.Name}")
        }));
    }

    private void Cost(int cost, int used)
    {
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap is not { FDay: 2 } map) return;
        writer.WriteLine(JsonSerializer.Serialize(new
        {
            kind = "cost",
            day = map.FDay,
            frame = map.Frame,
            ai_player = map.FAIExecuter.FPlayer,
            cost,
            used,
            stack = new StackTrace(1, false).GetFrames().Select(frame => frame.GetMethod())
                .Where(method => method?.DeclaringType?.Assembly.GetName().Name == "Aow.Units")
                .Take(10).Select(method => $"{method!.DeclaringType!.Name}.{method.Name}")
        }));
    }

    public void Snapshot(string name)
    {
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap is not { } map) return;
        var viewer = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer;
        var window = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow;
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
                    var resource = (child as TMapObject)?.Resource;
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
                                    lastFrame = typeof(Aow.Game.Draw.Images.TImageSequence).GetField("FLastFrame", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(sequence),
                                    counter = typeof(Aow.Game.Draw.Images.TImageSequence).GetField("FFrameCounter", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(sequence)
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
        File.WriteAllText(Path.Combine(output, "diagnostic-" + name + ".json"), JsonSerializer.Serialize(new
        {
            map.FDay,
            map.Frame,
            viewer.SceneX,
            viewer.SceneY,
            viewer.SceneL,
            objects
        }, new JsonSerializerOptions { WriteIndented = true }) + "\n");
    }

    public void Dispose()
    {
        RtlUnit.RandomTrace = previousRandom;
        HeadlessHooks.ProcessCost = previousCost;
        writer.Dispose();
    }
}
