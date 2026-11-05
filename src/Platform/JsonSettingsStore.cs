using System.Text.Json;
using Aow.Rtl;

namespace Aow.Platform;

/// <summary>Typed values used directly by the shim's TRegistry and TAoWRegistry.</summary>
public sealed class JsonSettingsStore : ISettingsStore
{
    private const string Prefix = "Software/Triumph Studios/Age of Wonders/";
    // The settings whose value the game opens as a path keep the host's chars. Every other string
    // setting is game text and enters through the code page 1252 table (strings.md 2.5).
    private static readonly string[] PathKeys = ["General/Root Directory", "General/Startup Directory", "Email/Attachment Directory"];
    private readonly string path;
    private readonly Dictionary<string, object> values = new(StringComparer.OrdinalIgnoreCase);

    public JsonSettingsStore(string userDirectory)
    {
        path = Path.Combine(Path.GetFullPath(userDirectory), "settings.json");
        if (!File.Exists(path)) return;
        using var json = JsonDocument.Parse(File.ReadAllText(path));
        foreach (var property in json.RootElement.EnumerateObject())
        {
            string key = Normalize(property.Name);
            if (Allowed(key)) values[key] = Decode(key, property.Value);
        }
    }
    private static string Normalize(string key)
    {
        key = key.Replace('\\', '/').Trim('/');
        return key.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase) ? key[Prefix.Length..] : key;
    }
    /// <summary>A string setting read from a file: a path keeps its chars, text is normalized.</summary>
    public static string ReadText(string key, string value) => PathKeys.Contains(Normalize(key), StringComparer.OrdinalIgnoreCase) ? value : Cp1252.Normalize(value);
    private static bool Allowed(string key) => !key.StartsWith("Software/", StringComparison.OrdinalIgnoreCase) && !key.StartsWith('{');
    public object? Read(string key) => values.GetValueOrDefault(Normalize(key)) is { } value ? value is TRect rect
        ? $"{rect.Left},{rect.Top},{rect.Right},{rect.Bottom}" : Clone(value) : null;
    public void Write(string key, object value)
    {
        key = Normalize(key);
        if (!Allowed(key)) return;
        object stored = Clone(value);
        _ = Encode(key, stored);
        var next = new Dictionary<string, object>(values, StringComparer.OrdinalIgnoreCase) { [key] = stored };
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        string temporary = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            File.WriteAllText(temporary, JsonSerializer.Serialize(next.OrderBy(p => p.Key, StringComparer.Ordinal).ToDictionary(p => p.Key, p => Encode(p.Key, p.Value)), new JsonSerializerOptions { WriteIndented = true }) + "\n");
            File.Move(temporary, path, true);
            values[key] = stored;
        }
        finally { if (File.Exists(temporary)) File.Delete(temporary); }
    }
    private static object Clone(object value) => value switch { byte[] bytes => bytes.ToArray(), string text => text, _ => value };
    private static object Encode(string key, object value)
    {
        if (value is string rectangleText && (key.StartsWith("General/rectangles/", StringComparison.OrdinalIgnoreCase) || key.Equals("Workspace/Main Window Position", StringComparison.OrdinalIgnoreCase) || key.Equals("Workspace/Map Window Position", StringComparison.OrdinalIgnoreCase)))
        {
            int[] coordinates = rectangleText.Split(',').Select(int.Parse).ToArray();
            if (coordinates.Length != 4) throw new ArgumentException("A rectangle has four coordinates.");
            return new { kind = "rectangle", value = coordinates };
        }
        return value switch
        {
            int number => new { kind = "integer", value = (object)number },
            bool boolean => new { kind = "boolean", value = (object)boolean },
            string text => new { kind = "string", value = (object)text },
            byte[] bytes => new { kind = "bytes", value = (object)Convert.ToBase64String(bytes) },
            TRect rect => new { kind = "rectangle", value = (object)new[] { rect.Left, rect.Top, rect.Right, rect.Bottom } },
            _ => throw new ArgumentException("Unsupported registry value type.")
        };
    }
    private static object Decode(string key, JsonElement item)
    {
        if (item.ValueKind != JsonValueKind.Object) return item.ValueKind switch
        {
            JsonValueKind.Number => item.GetInt32(), JsonValueKind.True => true, JsonValueKind.False => false,
            JsonValueKind.String => ReadText(key, item.GetString()!),
            JsonValueKind.Array => item.EnumerateArray().Select(e => e.GetByte()).ToArray(),
            _ => throw new InvalidDataException("Unsupported setting.")
        };
        var value = item.GetProperty("value");
        return item.GetProperty("kind").GetString() switch
        {
            "integer" => value.GetInt32(), "boolean" => value.GetBoolean(), "string" => ReadText(key, value.GetString()!),
            "bytes" => Convert.FromBase64String(value.GetString()!),
            // TAoWRegistry.rwRect uses a comma separated string through TRegistry.ReadString.
            "rectangle" => string.Join(",", value.EnumerateArray().Select(e => e.GetInt32())),
            _ => throw new InvalidDataException("Unsupported setting kind.")
        };
    }
}
