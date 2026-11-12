#nullable enable
using System.Globalization;

using System.Text;
using System.Text.Json;

using Aow.Formats;

namespace Aow.Game.Saves;

/// <summary>One line of a host's file log (own-formats.md 4.2). Physical paths appear only here.</summary>
public sealed class FileLogRecord(string path, string layer, GameFileReport report, bool cached = false)
{
    public string Path { get; } = path;
    public string Layer { get; } = layer;
    public GameFileReport Report { get; } = report;

    /// <summary>A preview served from the session cache after the bytes hashed equal.</summary>
    public bool Cached { get; } = cached;
    public bool Succeeded => Report.Error is null;

    /// <summary>One JSON line: operation, logical path, layer, profile, kind, format, version, G1, validation and error.</summary>
    public string ToJson()
    {
        using var buffer = new MemoryStream();
        using (var json = new Utf8JsonWriter(buffer))
        {
            json.WriteStartObject();
            json.WriteString("operation", Report.Operation);
            json.WriteString("path", Path);
            json.WriteString("layer", Layer);
            json.WriteString("profile", Report.Profile.Profile.Name);
            json.WriteString("use", Report.Profile.Name);
            WriteOptional(json, "kind", Report.Kind);
            WriteOptional(json, "format", Report.Format);
            if (Report.Version is int version) json.WriteNumber("version", version);
            else json.WriteNull("version");
            json.WriteString("g1", Report.OriginalRoundTrip);
            json.WriteString("validation", Report.Validation);
            WriteOptional(json, "error", Report.Error?.Code);
            WriteOptional(json, "diagnostic", Report.Error?.ToString());
            json.WriteBoolean("cached", Cached);
            json.WriteEndObject();
        }
        return Encoding.UTF8.GetString(buffer.ToArray());
    }

    private static void WriteOptional(Utf8JsonWriter json, string name, string? value)
    {
        if (value is null) json.WriteNull(name);
        else json.WriteString(name, value);
    }

    public override string ToString() => Report.Error?.ToString() ?? string.Create(CultureInfo.InvariantCulture, $"{Report.Operation} {Path}");
}
