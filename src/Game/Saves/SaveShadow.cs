#nullable enable
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Rtl;

namespace Aow.Game.Saves;

/// <summary>
/// The save shadow of the gate (issue 185, PE-3-f, persistence-tree.md 12). When
/// <c>AOW_SAVE_SHADOW</c> names a file, every game save also writes its root through
/// <see cref="TreeWriter"/> and <see cref="DocumentBackend"/> and compares that document with the
/// root the 9.7 path imported from the engine image. Each comparison appends one JSON line to the
/// file, with the document path of the first difference. The gate reads the file and fails a
/// layer with a difference. The saved file and the game are unchanged. Loads stay on the engine
/// image. The switch of PE-3-g removes this class.
/// </summary>
public static class SaveShadow
{
    public const string Variable = "AOW_SAVE_SHADOW";

    // Empty when the variable is unset, since no static string of the library holds null (strings.md 2.2).
    private static readonly string s_path = Environment.GetEnvironmentVariable(Variable) is { Length: > 0 } path ? Path.GetFullPath(path) : "";

    public static bool Active => s_path.Length != 0;

    /// <summary>Compares the tree document of a saved root with the document the boundary imported, and records the result.</summary>
    public static void Compare(string fileName, GameProfile profile, EngineObject root, JsonObject document)
    {
        if (!Active) return;
        var watch = Stopwatch.StartNew();
        ShadowResult result;
        try
        {
            result = Check(profile, root, document);
        }
        catch (Exception error) when (error is ConversionException or InvalidOperationException or NotSupportedException)
        {
            result = new ShadowResult(Difference: "root: the tree write failed: " + error.Message, EngineWriteChange: null, LayoutResidues: 0);
        }
        Record(fileName, profile, result, watch.Elapsed.TotalMilliseconds);
    }

    /// <summary>
    /// What a comparison found. <c>Difference</c> is the first difference that stands, null
    /// when the documents agree. <c>EngineWriteChange</c> is the first difference from the
    /// imported document when only a second engine write of the root agrees with the tree: the
    /// engine write changed a stored value of the object after it stored it, and the tree write
    /// that follows reads the changed value. <c>LayoutResidues</c> counts the <c>$layout</c> members
    /// of the imported document the comparison skipped.
    /// </summary>
    public readonly record struct ShadowResult(string? Difference, string? EngineWriteChange, int LayoutResidues);

    /// <summary>
    /// Compares the tree document of the root with the imported root. When they differ, the tree
    /// document is compared with a second engine write of the root, which reads the state the tree
    /// write read (persistence-tree.md 12).
    /// </summary>
    public static ShadowResult Check(GameProfile profile, EngineObject root, JsonObject document)
    {
        var expected = (JsonObject)document["root"]!;
        var schema = FormatSchema.Current;
        string className = NameTable.Stored(root.GetType());
        SchemaClass classValue;
        if (profile.Profile.RootHasClass)
        {
            if (expected["$class"] is not JsonString { Value: var stored } || stored != className)
                return new ShadowResult(Difference: "root.$class: the document stores " + (expected["$class"] is JsonString name ? name.Value : "none") + ", the object is " + className, EngineWriteChange: null, LayoutResidues: 0);
            classValue = schema.Get(className);
        }
        else classValue = schema.Get(profile.Profile.RootFamily);
        var engine = root.GetEngine();
        var tree = TreeWriter.Write(root, new Factory(engine));
        var actual = DocumentBackend.Write(tree, classValue, schema, profile.Profile.Engine);
        int residues = 0;
        if (FirstDifference(expected, actual, "root", ref residues, skipClass: profile.Profile.RootHasClass) is not { } first)
            return new ShadowResult(Difference: null, EngineWriteChange: null, LayoutResidues: residues);
        using var memory = new MemoryStream();
        using (var writer = new StorageWriter(memory, engine, partial: false))
            root.MainReadWrite(writer);
        var again = GameBoundary.ImportObjectImage(memory.ToArray(), className: classValue.Name, engine: profile.Profile.Engine);
        int againResidues = 0;
        return FirstDifference(again, actual, "root", ref againResidues) is { } second
            ? new ShadowResult(Difference: second, EngineWriteChange: first, LayoutResidues: residues)
            : new ShadowResult(Difference: null, EngineWriteChange: first, LayoutResidues: residues);
    }

    // The path is built only on the way back from a difference, so an equal document allocates none.
    // The imported side skips $layout, the residue that reproduces the engine's bytes (own-formats.md
    // 2.5): the tree holds the state, never the encoding. Each skipped residue is counted.
    private static string? FirstDifference(JsonValue expected, JsonValue actual, string path, ref int residues, bool skipClass = false)
    {
        switch (expected, actual)
        {
            case (JsonObject left, JsonObject right):
                var leftMembers = left.Members;
                if (skipClass || left.Find("$layout") is not null)
                {
                    leftMembers = [.. left.Members.Where(m => m.Name != "$layout" && !(skipClass && m.Name == "$class"))];
                    if (left.Find("$layout") is not null) residues++;
                }
                for (int i = 0; i < Math.Max(leftMembers.Count, right.Members.Count); i++)
                {
                    if (i >= leftMembers.Count) return path + "." + right.Members[i].Name + ": only in the tree document";
                    if (i >= right.Members.Count) return path + "." + leftMembers[i].Name + ": only in the imported document";
                    var (one, other) = (leftMembers[i], right.Members[i]);
                    if (one.Name != other.Name)
                        return path + ": member " + i.ToString(CultureInfo.InvariantCulture) + " is " + one.Name + " imported and " + other.Name + " in the tree document";
                    if (FirstDifference(one.Value, other.Value, "", ref residues) is { } nested) return path + "." + one.Name + nested;
                }
                return null;
            case (JsonArray left, JsonArray right):
                for (int i = 0; i < Math.Max(left.Items.Count, right.Items.Count); i++)
                {
                    if (i < left.Items.Count && i < right.Items.Count)
                    {
                        if (FirstDifference(left.Items[i], right.Items[i], "", ref residues) is { } nested) return path + Index(i) + nested;
                        continue;
                    }
                    return path + Index(i) + (i >= left.Items.Count ? ": only in the tree document" : ": only in the imported document");
                }
                return null;
            default:
                return ScalarEqual(expected, actual) ? null : path + ": imported " + Compact(expected) + ", tree " + Compact(actual);
        }
    }

    private static bool ScalarEqual(JsonValue expected, JsonValue actual) => (expected, actual) switch
    {
        (JsonString left, JsonString right) => left.Value == right.Value,
        (JsonInteger left, JsonInteger right) => left.Value == right.Value,
        (JsonNumber left, JsonNumber right) => left.Text == right.Text,
        (JsonBoolean left, JsonBoolean right) => left.Value == right.Value,
        (JsonNull, JsonNull) => true,
        _ => false,
    };

    private static string Compact(JsonValue value)
    {
        var builder = new StringBuilder();
        JsonWriter.WriteCompact(builder, value);
        return builder.ToString();
    }

    private static string Index(int index) => "[" + index.ToString(CultureInfo.InvariantCulture) + "]";

    private static void Record(string fileName, GameProfile profile, ShadowResult result, double milliseconds)
    {
        using var buffer = new MemoryStream();
        using (var json = new Utf8JsonWriter(buffer))
        {
            json.WriteStartObject();
            json.WriteString("path", fileName);
            json.WriteString("use", profile.Name);
            json.WriteBoolean("equal", result.Difference is null);
            json.WriteString("difference", result.Difference);
            json.WriteString("engine_write_change", result.EngineWriteChange);
            json.WriteNumber("layout_residues", result.LayoutResidues);
            json.WriteNumber("milliseconds", Math.Round(milliseconds, 1));
            json.WriteEndObject();
        }
        Append(buffer.ToArray());
        if (result.Difference is not null) Console.Error.WriteLine("Save shadow difference in " + fileName + " at " + result.Difference);
    }

    /// <summary>
    /// Appends one line under a named mutex, so that parallel hosts never interleave their lines.
    /// A retry on a sharing violation would raise first-chance exceptions, which the frame and
    /// headless hosts count as failures of the run.
    /// </summary>
    private static void Append(byte[] line)
    {
        string name = "OpenAow.SaveShadow." + Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(s_path)));
        using var mutex = new Mutex(initiallyOwned: false, name);
        mutex.WaitOne();
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(s_path)!);
            using var stream = new FileStream(s_path, FileMode.Append, FileAccess.Write, FileShare.Read);
            stream.Write(line);
            stream.Write("\n"u8);
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }

    /// <summary>The factory of a save. A write only names classes, it creates none.</summary>
    private sealed class Factory(GameEngine engine) : IEnginePersistableFactory
    {
        public GameEngine Engine { get; } = engine;

        public string ClassName(IPersistable objectValue) => NameTable.Stored(objectValue.GetType());

        public IPersistable Create(string className) => throw new InvalidOperationException("A save creates no object.");
    }
}
