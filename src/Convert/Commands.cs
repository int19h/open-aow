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
        {
            throw new ConversionException(new FormatDiagnostic(Codes.Format, Message: "The input is an own document, not an original file. Use normalize.") { File = path });
        }

        Classification c = GameFiles.Classify(bytes, extension: Path.GetExtension(path), kind, profileName: profile);
        return c.Format != DetectedFormat.Original || c.Profile is null || c.Kind is null
            ? throw new ConversionException(new FormatDiagnostic(Codes.Unknown,
                Message: "The input is not a recognized original file. Give --kind and --profile for headerless or ambiguous input.")
            { File = path })
            : GameFiles.Import(bytes, c.Profile, c.Kind, fileName: path);
    }

    internal static int ToOwn(Options o, TextWriter output, TextWriter errors)
    {
        if (o.Directory is not null)
        {
            return ToOwnDirectory(o, output, errors);
        }

        if (o.Positional.Count != 2)
        {
            throw new UsageException("to-own needs <in> and <out>.");
        }

        string input = o.Positional[0], target = o.Positional[1];
        if (SafeOutput.CheckFile(input, output: target, o.Overwrite) is { } refusal)
        {
            return Fail(errors, message: refusal);
        }

        try
        {
            ImportResult result = ImportOriginal(File.ReadAllBytes(input), path: input, o.Kind, o.Profile);
            SafeOutput.Replace(target, bytes: GameFiles.Write(result.Document));
            output.WriteLine($"{input}: {Describe(result.Document)} -> {target}");
            return Program.Success;
        }
        catch (ConversionException error)
        {
            return Fail(errors, error);
        }
    }

    internal static int ToOriginal(Options o, TextWriter output, TextWriter errors)
    {
        if (o.Directory is not null)
        {
            return ToOriginalDirectory(o, output, errors);
        }

        if (o.Positional.Count != 2)
        {
            throw new UsageException("to-original needs <in> and <out>.");
        }

        string input = o.Positional[0], target = o.Positional[1];
        if (SafeOutput.CheckFile(input, output: target, o.Overwrite) is { } refusal)
        {
            return Fail(errors, message: refusal);
        }

        try
        {
            ReadResult read = GameFiles.Read(File.ReadAllBytes(input), fileName: input);
            ExportResult exported = GameFiles.Export(read.Document, fileName: input);
            SafeOutput.Replace(target, exported.Bytes);
            output.WriteLine($"{input}: {Describe(read.Document)} -> {target}, 0 losses");
            return Program.Success;
        }
        catch (ConversionException error)
        {
            return Fail(errors, error);
        }
    }

    internal static int Normalize(Options o, TextWriter output, TextWriter errors)
    {
        if (o.Positional.Count != 2)
        {
            throw new UsageException("normalize needs <in> and <out>.");
        }

        string input = o.Positional[0], target = o.Positional[1];
        if (SafeOutput.CheckFile(input, output: target, o.Overwrite) is { } refusal)
        {
            return Fail(errors, message: refusal);
        }

        try
        {
            byte[] bytes = GameFiles.Normalize(File.ReadAllBytes(input), fileName: input);
            SafeOutput.Replace(target, bytes);
            output.WriteLine($"{input}: normalized -> {target}");
            return Program.Success;
        }
        catch (ConversionException error)
        {
            return Fail(errors, error);
        }
    }

    private static string Describe(Formats.Json.JsonObject document)
    {
        (FileProfile? profile, string? kind, Formats.Json.JsonObject? container, Formats.Json.JsonValue _) = GameFiles.Envelope(document, fileName: null);
        string compression = ((Formats.Json.JsonString)container["compression"]!).Value;
        string level = container["level"] is Formats.Json.JsonInteger l ? $" level {l.Value}" : "";
        return $"profile {profile.Name}, kind {kind}, {compression}{level}";
    }

    /// <summary>Sorted relative paths of every file under a directory.</summary>
    internal static List<string> Enumerate(string directory)
        => [.. Directory.EnumerateFiles(directory, searchPattern: "*", searchOption: SearchOption.AllDirectories)
            .Select(f => Path.GetRelativePath(directory, path: f))
            .Order(StringComparer.Ordinal)];

    private static Dictionary<string, (string? Kind, string? Profile)> Manifest(string? path)
    {
        Dictionary<string, (string?, string?)> result = [with(StringComparer.OrdinalIgnoreCase)];
        if (path is null)
        {
            return result;
        }

        using JsonDocument doc = JsonDocument.Parse(File.ReadAllBytes(path));
        foreach (JsonProperty p in doc.RootElement.EnumerateObject())
        {
            result[p.Name.Replace('/', Path.DirectorySeparatorChar)] = (
                p.Value.TryGetProperty("kind", out JsonElement k) ? k.GetString() : null,
                p.Value.TryGetProperty("profile", out JsonElement f) ? f.GetString() : null);
        }

        return result;
    }

    private static int ToOwnDirectory(Options o, TextWriter output, TextWriter errors)
    {
        if (o.Out is null || o.Positional.Count != 0)
        {
            throw new UsageException("to-own --dir needs --out and no other paths.");
        }

        if (SafeOutput.Overlaps(o.Directory!, o.Out))
        {
            return Fail(errors, message: "The input and output directory trees overlap.");
        }

        Dictionary<string, (string? Kind, string? Profile)> manifest = Manifest(o.Profiles);
        int done = 0, failed = 0, skipped = 0;
        foreach (string relative in Enumerate(o.Directory!))
        {
            string input = Path.Combine(o.Directory!, relative);
            _ = manifest.TryGetValue(relative, out (string? Kind, string? Profile) entry);
            string extension = Path.GetExtension(relative);
            if (entry.Kind is null && (DocumentKind.ForExtension(extension) is not { } byName || byName.IsOwnExtension(extension)))
            {
                output.WriteLine($"{relative}: skipped, not an in-scope original");
                skipped++;
                continue;
            }
            try
            {
                ImportResult result = ImportOriginal(File.ReadAllBytes(input), path: input, entry.Kind, entry.Profile);
                // A derived output name takes the kind's own extension (own-formats.md 3.2).
                string target = Path.Combine(o.Out, DerivedName(relative, result.Document, own: true));
                if (SafeOutput.CheckFile(input, output: target, o.Overwrite) is { } refusal)
                {
                    errors.WriteLine($"{relative}: {refusal}");
                    failed++;
                    continue;
                }
                SafeOutput.Replace(target, bytes: GameFiles.Write(result.Document));
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
        if (o.Out is null || o.Positional.Count != 0)
        {
            throw new UsageException("to-original --dir needs --out and no other paths.");
        }

        if (SafeOutput.Overlaps(o.Directory!, o.Out))
        {
            return Fail(errors, message: "The input and output directory trees overlap.");
        }

        int done = 0, failed = 0, skipped = 0;
        Dictionary<string, string> sources = [with(StringComparer.OrdinalIgnoreCase)];
        foreach (string relative in Enumerate(o.Directory!))
        {
            string input = Path.Combine(o.Directory!, relative);
            byte[] bytes = File.ReadAllBytes(input);
            if (!GameFiles.IsGzip(bytes) && !GameFiles.IsJsonCandidate(bytes))
            {
                output.WriteLine($"{relative}: skipped, not an own document");
                skipped++;
                continue;
            }
            try
            {
                ReadResult read = GameFiles.Read(bytes, fileName: input);
                ExportResult exported = GameFiles.Export(read.Document, fileName: input);
                // A derived output name takes the kind's original extension (own-formats.md 3.2).
                string target = Path.Combine(o.Out, DerivedName(relative, read.Document, own: false));
                // Foo.asg and Foo.oasg both export to Foo.asg: the second is refused by name.
                if (sources.TryGetValue(target, out string? first))
                {
                    errors.WriteLine($"{relative}: exports to the same name as {first}");
                    failed++;
                    continue;
                }
                sources[target] = relative;
                if (SafeOutput.CheckFile(input, output: target, o.Overwrite) is { } refusal)
                {
                    errors.WriteLine($"{relative}: {refusal}");
                    failed++;
                    continue;
                }

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
    internal static string DerivedName(string relative, Formats.Json.JsonObject document, bool own)
    {
        (FileProfile _, string? kindName, Formats.Json.JsonObject _, Formats.Json.JsonValue _) = GameFiles.Envelope(document, fileName: relative);
        DocumentKind kind = DocumentKind.Find(kindName)!;
        return own ? kind.OwnName(relative) : kind.OriginalName(relative);
    }

    /// <summary>Profile, kind, version, representability, residue, coverage and the cost of one file.</summary>
    internal static int Info(Options o, TextWriter output, TextWriter errors)
    {
        if (o.Positional.Count != 1)
        {
            throw new UsageException("info needs one <file>.");
        }

        string input = o.Positional[0];
        try
        {
            byte[] bytes = File.ReadAllBytes(input);
            Stopwatch watch = Stopwatch.StartNew();
            Formats.Json.JsonObject document;
            string format;
            IReadOnlyDictionary<string, int> legacyRules;
            double importMilliseconds, writeMilliseconds, parseMilliseconds;
            byte[] text;
            if (GameFiles.IsGzip(bytes) || GameFiles.IsJsonCandidate(bytes))
            {
                ReadResult read = GameFiles.Read(bytes, fileName: input);
                parseMilliseconds = watch.Elapsed.TotalMilliseconds;
                document = read.Document;
                format = $"{(read.Gzip ? "gzip own JSON" : "own JSON")}, declared version {read.DeclaredVersion}";
                legacyRules = new Dictionary<string, int>(StringComparer.Ordinal);
                importMilliseconds = 0;
                watch.Restart();
                text = GameFiles.Write(document);
                writeMilliseconds = watch.Elapsed.TotalMilliseconds;
            }
            else
            {
                ImportResult result = ImportOriginal(bytes, path: input, o.Kind, o.Profile);
                importMilliseconds = watch.Elapsed.TotalMilliseconds;
                document = result.Document;
                legacyRules = result.LegacyRules;
                format = "original";
                watch.Restart();
                text = GameFiles.Write(document);
                writeMilliseconds = watch.Elapsed.TotalMilliseconds;
                watch.Restart();
                _ = GameFiles.Read(text, fileName: input);
                parseMilliseconds = watch.Elapsed.TotalMilliseconds;
            }
            watch.Restart();
            ExportResult exported = GameFiles.Export(document, fileName: input);
            double exportMilliseconds = watch.Elapsed.TotalMilliseconds;
            (FileProfile? profile, string? kind, _, _) = GameFiles.Envelope(document, fileName: input);
            output.WriteLine($"File: {input}");
            output.WriteLine($"Format: {format}");
            output.WriteLine($"Profile: {profile.Name}, kind {kind}, version {OwnFormat.CurrentVersion}");
            output.WriteLine("Representable: yes, strict export lost nothing");
            output.WriteLine(legacyRules.Count == 0
                ? "Legacy rules: none"
                : $"Legacy rules: {string.Join(", ", values: legacyRules.OrderBy(c => c.Key, comparer: StringComparer.Ordinal).Select(c => $"{c.Key} {c.Value}"))}");
            output.WriteLine("Validation: structural (schema, codecs, limits). Game content validation arrives with 9.7-3.");
            output.WriteLine(string.Create(CultureInfo.InvariantCulture,
                handler: $"Size: original {bytes.Length} bytes, exported {exported.Bytes.Length} bytes, own JSON {text.Length} bytes"));
            output.WriteLine(string.Create(CultureInfo.InvariantCulture,
                handler: $"Latency: import {importMilliseconds:F1} ms, write {writeMilliseconds:F1} ms, parse {parseMilliseconds:F1} ms, export {exportMilliseconds:F1} ms"));
            using Process process = Process.GetCurrentProcess();
            output.WriteLine(string.Create(CultureInfo.InvariantCulture,
                handler: $"Memory: peak working set {process.PeakWorkingSet64 / (1024.0 * 1024.0):F1} MiB, managed heap {GC.GetTotalMemory(false) / (1024.0 * 1024.0):F1} MiB"));
            return Program.Success;
        }
        catch (ConversionException error)
        {
            return Fail(errors, error);
        }
    }
}
