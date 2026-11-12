using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Aow.Game.Tests;

// Keep the complete constructor recording readable, including JSON embedded in event strings.
internal static class ConstructionSnapshot
{
    private static readonly Dictionary<string, JsonObject> s_recordings = [];
    private static readonly JsonSerializerOptions s_format = new() { WriteIndented = true };

    private static JsonObject Load(string name)
    {
        if (s_recordings.TryGetValue(name, out var value)) return value;
        using var stream = typeof(ConstructionSnapshot).Assembly.GetManifestResourceStream(
            "Aow.Game.Tests.Snapshots." + name + "Snapshot.json")!;
        value = JsonNode.Parse(stream)!.AsObject();
        s_recordings.Add(name, value);
        return value;
    }

    internal static IEnumerable<string> Keys(string name) => Load(name).Select(pair => pair.Key).Order(StringComparer.Ordinal);

    private static JsonNode? Entry(JsonObject recording, string key)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        while (recording[key] is JsonObject { Count: 1 } entry && entry["$sameAs"] is { } reference)
        {
            Assert.True(seen.Add(key), "Snapshot reference cycle: " + key);
            key = reference.GetValue<string>();
            Assert.True(recording.ContainsKey(key), "Snapshot reference is absent: " + key);
        }
        return recording[key];
    }

    internal static void Check(string name, string key, string actual, string? member = null)
    {
        var recording = Load(name);
        Assert.True(recording.ContainsKey(key), name + " has no recording for " + key);
        JsonNode? expected = member is null ? Entry(recording, key) : Entry(recording, key)![member];
        JsonNode? value = Readable(actual);
        string? target = Environment.GetEnvironmentVariable("AOW_CONSTRUCTION_RECORD");
        if (!string.IsNullOrEmpty(target))
        {
            if (member is null) recording[key] = value?.DeepClone();
            else
            {
                recording[key] = Entry(recording, key)?.DeepClone();
                recording[key]![member] = value?.DeepClone();
            }
            Directory.CreateDirectory(target);
            var sorted = new JsonObject();
            var shared = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (string entryKey in Keys(name))
            {
                var entryValue = Entry(recording, entryKey);
                string text = entryValue?.ToJsonString() ?? "null";
                sorted[entryKey] = shared.TryGetValue(text, out string? first)
                    ? new JsonObject { ["$sameAs"] = first } : entryValue?.DeepClone();
                shared.TryAdd(text, entryKey);
            }
            File.WriteAllText(Path.Combine(target, name + "Snapshot.json"),
                sorted.ToJsonString(s_format).Replace("\r\n", "\n", StringComparison.Ordinal) + "\n", new UTF8Encoding(false));
            return;
        }
        Compare(expected, value, name + "." + key + (member is null ? "" : "." + member));
    }

    private static void Compare(JsonNode? expected, JsonNode? actual, string path)
    {
        if (expected is JsonObject left && actual is JsonObject right)
        {
            Assert.True(left.Select(pair => pair.Key).Order(StringComparer.Ordinal)
                .SequenceEqual(right.Select(pair => pair.Key).Order(StringComparer.Ordinal)), path + " field names differ: expected "
                    + string.Join(", ", left.Select(pair => pair.Key)) + ", actual " + string.Join(", ", right.Select(pair => pair.Key)));
            foreach (var pair in left) Compare(pair.Value, right[pair.Key], path + "." + pair.Key);
        }
        else if (expected is JsonArray before && actual is JsonArray after)
        {
            Assert.True(before.Count == after.Count, path + $" length: expected {before.Count}, actual {after.Count}");
            for (int i = 0; i < before.Count; i++) Compare(before[i], after[i], path + $"[{i}]");
        }
        else
            Assert.True(JsonNode.DeepEquals(expected, actual), path + ": expected "
                + (expected?.ToJsonString() ?? "null") + ", actual " + (actual?.ToJsonString() ?? "null"));
    }

    private static JsonNode? Readable(string text)
    {
        if (text.StartsWith('{') || text.StartsWith('['))
        {
            try { return new JsonObject { ["$jsonText"] = Expand(JsonNode.Parse(text)) }; }
            catch (JsonException) { }
        }
        if (text.Contains('\n'))
            return new JsonObject { ["$lines"] = new JsonArray(text.Split('\n').Select(Readable).ToArray()) };
        if (text.StartsWith("success:", StringComparison.Ordinal))
            return new JsonObject { ["$prefix"] = "success:", ["$value"] = Readable(text[8..]) };
        return JsonValue.Create(text);
    }

    private static JsonNode? Expand(JsonNode? node) => node switch
    {
        JsonObject objectValue => new JsonObject(objectValue.Select(pair => KeyValuePair.Create(pair.Key, Expand(pair.Value)))),
        JsonArray array => new JsonArray(array.Select(Expand).ToArray()),
        JsonValue value when value.TryGetValue<string>(out string? text) => Readable(text),
        _ => node?.DeepClone()
    };
}
