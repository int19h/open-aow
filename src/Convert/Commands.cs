using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using Aow.Formats;
using Aow.Formats.Original;

namespace Aow.Convert;

internal static class Commands
{
    private static int Fail(TextWriter errors, ConversionException error)
    {
        errors.WriteLine(error.Diagnostic.ToString());
        return Program.Error;
    }

    private static int Fail(TextWriter errors, string message)
    {
        errors.WriteLine(message);
        return Program.Error;
    }

    /// <summary>Imports one original file. Own input is refused: normalize handles it.</summary>
    internal static ImportResult ImportOriginal(byte[] bytes, string path, string? kind, string? profile)
    {
        if (GameFiles.IsGzip(bytes) || GameFiles.IsJsonCandidate(bytes))
            throw new ConversionException(new FormatDiagnostic(Codes.Format, "The input is an own document, not an original file. Use normalize.") { File = path });
        var c = GameFiles.Classify(bytes, Path.GetExtension(path), kind, profile);
        if (c.Format != DetectedFormat.Original || c.Profile is null || c.Kind is null)
            throw new ConversionException(new FormatDiagnostic(Codes.Unknown,
                "The input is not a recognized original file. Give --kind and --profile for headerless or ambiguous input.")
            { File = path });
        return GameFiles.Import(bytes, c.Profile, c.Kind, path);
    }

    internal static int ToOwn(Options o, TextWriter output, TextWriter errors)
    {
        if (o.Directory is not null) return ToOwnDirectory(o, output, errors);
        if (o.Positional.Count != 2) throw new UsageException("to-own needs <in> and <out>.");
        string input = o.Positional[0], target = o.Positional[1];
        if (SafeOutput.CheckFile(input, target, o.Overwrite) is { } refusal) return Fail(errors, refusal);
        try
        {
            var result = ImportOriginal(File.ReadAllBytes(input), input, o.Kind, o.Profile);
            SafeOutput.Replace(target, GameFiles.Write(result.Document));
            output.WriteLine($"{input}: {Describe(result.Document)}, {result.Counts.Parts} layout parts -> {target}");
            return Program.Success;
        }
        catch (ConversionException error)
        {
            return Fail(errors, error);
        }
    }

    internal static int ToOriginal(Options o, TextWriter output, TextWriter errors)
    {
        if (o.Directory is not null) return ToOriginalDirectory(o, output, errors);
        if (o.Positional.Count != 2) throw new UsageException("to-original needs <in> and <out>.");
        string input = o.Positional[0], target = o.Positional[1];
        if (SafeOutput.CheckFile(input, target, o.Overwrite) is { } refusal) return Fail(errors, refusal);
        try
        {
            var read = GameFiles.Read(File.ReadAllBytes(input), input);
            var exported = GameFiles.Export(read.Document, new ExportOptions { ExactLayout = o.ExactLayout }, input);
            foreach (var d in exported.Invalidated) errors.WriteLine(d.ToString());
            SafeOutput.Replace(target, exported.Bytes);
            output.WriteLine($"{input}: {Describe(read.Document)} -> {target}, 0 losses, {exported.Invalidated.Count} invalidated layouts");
            return Program.Success;
        }
        catch (ConversionException error)
        {
            return Fail(errors, error);
        }
    }

    internal static int Normalize(Options o, TextWriter output, TextWriter errors)
    {
        if (o.Positional.Count != 2) throw new UsageException("normalize needs <in> and <out>.");
        string input = o.Positional[0], target = o.Positional[1];
        if (SafeOutput.CheckFile(input, target, o.Overwrite) is { } refusal) return Fail(errors, refusal);
        try
        {
            var (bytes, invalidated) = GameFiles.Normalize(File.ReadAllBytes(input), input);
            foreach (var d in invalidated) errors.WriteLine(d.ToString());
            SafeOutput.Replace(target, bytes);
            output.WriteLine($"{input}: normalized -> {target}, {invalidated.Count} invalidated layouts");
            return Program.Success;
        }
        catch (ConversionException error)
        {
            return Fail(errors, error);
        }
    }

    private static string Describe(Aow.Formats.Json.JsonObject document)
    {
        var (profile, kind, container, _) = GameFiles.Envelope(document, null);
        string compression = ((Aow.Formats.Json.JsonString)container["compression"]!).Value;
        string level = container["level"] is Aow.Formats.Json.JsonInteger l ? " level " + l.Value : "";
        return $"profile {profile.Name}, kind {kind}, {compression}{level}";
    }

    /// <summary>Sorted relative paths of every file under a directory.</summary>
    internal static List<string> Enumerate(string directory) =>
        Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories)
            .Select(f => Path.GetRelativePath(directory, f))
            .Order(StringComparer.Ordinal)
            .ToList();

    private static Dictionary<string, (string? Kind, string? Profile)> Manifest(string? path)
    {
        var result = new Dictionary<string, (string?, string?)>(StringComparer.OrdinalIgnoreCase);
        if (path is null) return result;
        using var doc = JsonDocument.Parse(File.ReadAllBytes(path));
        foreach (var p in doc.RootElement.EnumerateObject())
            result[p.Name.Replace('/', Path.DirectorySeparatorChar)] = (
                p.Value.TryGetProperty("kind", out var k) ? k.GetString() : null,
                p.Value.TryGetProperty("profile", out var f) ? f.GetString() : null);
        return result;
    }

    private static int ToOwnDirectory(Options o, TextWriter output, TextWriter errors)
    {
        if (o.Out is null || o.Positional.Count != 0) throw new UsageException("to-own --dir needs --out and no other paths.");
        if (SafeOutput.Overlaps(o.Directory!, o.Out)) return Fail(errors, "The input and output directory trees overlap.");
        var manifest = Manifest(o.Profiles);
        int done = 0, failed = 0, skipped = 0;
        foreach (var relative in Enumerate(o.Directory!))
        {
            string input = Path.Combine(o.Directory!, relative);
            manifest.TryGetValue(relative, out var entry);
            string extension = Path.GetExtension(relative);
            if (entry.Kind is null && (DocumentKind.ForExtension(extension) is not { } byName || byName.IsOwnExtension(extension)))
            {
                output.WriteLine($"{relative}: skipped, not an in-scope original");
                skipped++;
                continue;
            }
            try
            {
                var result = ImportOriginal(File.ReadAllBytes(input), input, entry.Kind, entry.Profile);
                // A derived output name takes the kind's own extension (own-formats.md 3.2).
                string target = Path.Combine(o.Out, DerivedName(relative, result.Document, own: true));
                if (SafeOutput.CheckFile(input, target, o.Overwrite) is { } refusal)
                {
                    errors.WriteLine($"{relative}: {refusal}");
                    failed++;
                    continue;
                }
                SafeOutput.Replace(target, GameFiles.Write(result.Document));
                output.WriteLine($"{relative}: {Describe(result.Document)}");
                done++;
            }
            catch (ConversionException error)
            {
                errors.WriteLine(error.Diagnostic.ToString());
                failed++;
            }
        }
        output.WriteLine($"to-own: {done} converted, {failed} failed, {skipped} skipped");
        return failed > 0 ? Program.Error : Program.Success;
    }

    private static int ToOriginalDirectory(Options o, TextWriter output, TextWriter errors)
    {
        if (o.Out is null || o.Positional.Count != 0) throw new UsageException("to-original --dir needs --out and no other paths.");
        if (SafeOutput.Overlaps(o.Directory!, o.Out)) return Fail(errors, "The input and output directory trees overlap.");
        int done = 0, failed = 0, skipped = 0;
        var sources = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var relative in Enumerate(o.Directory!))
        {
            string input = Path.Combine(o.Directory!, relative);
            var bytes = File.ReadAllBytes(input);
            if (!GameFiles.IsGzip(bytes) && !GameFiles.IsJsonCandidate(bytes))
            {
                output.WriteLine($"{relative}: skipped, not an own document");
                skipped++;
                continue;
            }
            try
            {
                var read = GameFiles.Read(bytes, input);
                var exported = GameFiles.Export(read.Document, null, input);
                // A derived output name takes the kind's original extension (own-formats.md 3.2).
                string target = Path.Combine(o.Out, DerivedName(relative, read.Document, own: false));
                // Foo.asg and Foo.oasg both export to Foo.asg: the second is refused by name.
                if (sources.TryGetValue(target, out var first))
                {
                    errors.WriteLine($"{relative}: exports to the same name as {first}");
                    failed++;
                    continue;
                }
                sources[target] = relative;
                if (SafeOutput.CheckFile(input, target, o.Overwrite) is { } refusal)
                {
                    errors.WriteLine($"{relative}: {refusal}");
                    failed++;
                    continue;
                }
                foreach (var d in exported.Invalidated) errors.WriteLine(d.ToString());
                SafeOutput.Replace(target, exported.Bytes);
                output.WriteLine($"{relative}: {Describe(read.Document)}, 0 losses");
                done++;
            }
            catch (ConversionException error)
            {
                errors.WriteLine(error.Diagnostic.ToString());
                failed++;
            }
        }
        output.WriteLine($"to-original: {done} converted, {failed} failed, {skipped} skipped, 0 losses");
        return failed > 0 ? Program.Error : Program.Success;
    }

    /// <summary>
    /// The output name a directory conversion derives from an input name: the document kind's own
    /// extension for an import, its original extension for an export, in place of the input's.
    /// </summary>
    internal static string DerivedName(string relative, Aow.Formats.Json.JsonObject document, bool own)
    {
        var (_, kindName, _, _) = GameFiles.Envelope(document, relative);
        var kind = DocumentKind.Find(kindName)!;
        return own ? kind.OwnName(relative) : kind.OriginalName(relative);
    }

    /// <summary>Profile, kind, version, representability, residue, coverage and the cost of one file.</summary>
    internal static int Info(Options o, TextWriter output, TextWriter errors)
    {
        if (o.Positional.Count != 1) throw new UsageException("info needs one <file>.");
        string input = o.Positional[0];
        try
        {
            var bytes = File.ReadAllBytes(input);
            var watch = Stopwatch.StartNew();
            Aow.Formats.Json.JsonObject document;
            string format;
            LayoutCounts counts;
            double importMilliseconds, writeMilliseconds, parseMilliseconds = 0;
            byte[] text;
            if (GameFiles.IsGzip(bytes) || GameFiles.IsJsonCandidate(bytes))
            {
                var read = GameFiles.Read(bytes, input);
                parseMilliseconds = watch.Elapsed.TotalMilliseconds;
                document = read.Document;
                format = (read.Gzip ? "gzip own JSON" : "own JSON") + $", declared version {read.DeclaredVersion}";
                counts = new LayoutCounts();
                importMilliseconds = 0;
                watch.Restart();
                text = GameFiles.Write(document);
                writeMilliseconds = watch.Elapsed.TotalMilliseconds;
            }
            else
            {
                var result = ImportOriginal(bytes, input, o.Kind, o.Profile);
                importMilliseconds = watch.Elapsed.TotalMilliseconds;
                document = result.Document;
                counts = result.Counts;
                format = "original";
                watch.Restart();
                text = GameFiles.Write(document);
                writeMilliseconds = watch.Elapsed.TotalMilliseconds;
                watch.Restart();
                GameFiles.Read(text, input);
                parseMilliseconds = watch.Elapsed.TotalMilliseconds;
            }
            watch.Restart();
            var exported = GameFiles.Export(document, null, input);
            double exportMilliseconds = watch.Elapsed.TotalMilliseconds;
            var (profile, kind, _, _) = GameFiles.Envelope(document, input);
            output.WriteLine($"File: {input}");
            output.WriteLine($"Format: {format}");
            output.WriteLine($"Profile: {profile.Name}, kind {kind}, version {OwnFormat.CurrentVersion}");
            output.WriteLine("Representable: yes, strict export lost nothing");
            output.WriteLine($"Residue: {counts.Parts} layout parts in {counts.Objects} objects" +
                (counts.Categories.Count > 0 ? " (" + string.Join(", ", counts.Categories.OrderBy(c => c.Key, StringComparer.Ordinal).Select(c => $"{c.Key} {c.Value}")) + ")" : "") +
                (counts.LegacyRules.Count > 0 ? ", legacy rules " + string.Join(", ", counts.LegacyRules.Select(c => $"{c.Key} {c.Value}")) : ""));
            output.WriteLine("Validation: structural (schema, codecs, limits). Game content validation arrives with 9.7-3.");
            output.WriteLine(string.Create(CultureInfo.InvariantCulture,
                $"Size: original {bytes.Length} bytes, exported {exported.Bytes.Length} bytes, own JSON {text.Length} bytes"));
            output.WriteLine(string.Create(CultureInfo.InvariantCulture,
                $"Latency: import {importMilliseconds:F1} ms, write {writeMilliseconds:F1} ms, parse {parseMilliseconds:F1} ms, export {exportMilliseconds:F1} ms"));
            using var process = Process.GetCurrentProcess();
            output.WriteLine(string.Create(CultureInfo.InvariantCulture,
                $"Memory: peak working set {process.PeakWorkingSet64 / (1024.0 * 1024.0):F1} MiB, managed heap {GC.GetTotalMemory(false) / (1024.0 * 1024.0):F1} MiB"));
            return Program.Success;
        }
        catch (ConversionException error)
        {
            return Fail(errors, error);
        }
    }
}
