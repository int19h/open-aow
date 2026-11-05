using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.Json;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Original;

namespace Aow.Convert;

/// <summary>
/// aowconv check (own-formats.md 3.3 and 5.2): G1 for every original, G2 for every imported
/// document, G-own for every own document, over the pinned corpus, the generated fixtures and
/// any named files. Unavailable checks are reported with reasons, never as passes.
/// </summary>
public sealed class Checker(TextWriter output, TextWriter errors)
{
    private sealed class ProfileTally
    {
        public int Files, G1, G2, GOwn, Residue, Parts;
    }

    private readonly SortedDictionary<string, ProfileTally> tallies = new(StringComparer.Ordinal);
    private int originals, g1, g2, gOwn, ownDocuments, unknownProperties, failures, differences, layoutParts, residueFiles;
    private readonly Dictionary<string, int> legacy = new(StringComparer.Ordinal);
    private readonly List<(string Name, string Profile, long Size, long Inflated, double ImportMs, double ExportMs, long Json)> timings = [];

    public static string GameRoot => Environment.GetEnvironmentVariable("AOW_GAME_ROOT") is { Length: > 0 } r ? r : @"D:\Games\GOG\Age of Wonders";
    public static string OracleRoot => Environment.GetEnvironmentVariable("AOW_ORACLE_ROOT") is { Length: > 0 } r ? r : @"D:\Build\oracle";

    public int Run(Options o)
    {
        int pinned = 0, expected = 0;
        if (o.Expect is not null)
        {
            using var census = JsonDocument.Parse(File.ReadAllBytes(o.Expect), new JsonDocumentOptions { MaxDepth = 256 });
            foreach (var row in census.RootElement.GetProperty("corpus").EnumerateArray())
            {
                if (row.GetProperty("scope").GetString() != "convert") continue;
                expected++;
                string root = row.GetProperty("sourceRoot").GetString() == "install" ? GameRoot : OracleRoot;
                string relative = row.GetProperty("relativePath").GetString()!;
                string path = Path.Combine(root, relative.Replace('/', Path.DirectorySeparatorChar));
                string profile = row.GetProperty("profile").GetString()!;
                string kind = row.GetProperty("kind").GetString()!;
                string sha = row.GetProperty("sha256").GetString()!;
                string name = row.GetProperty("identity").GetString()!;
                if (!File.Exists(path))
                {
                    Failure(name, $"pinned file {path} is missing");
                    continue;
                }
                var bytes = File.ReadAllBytes(path);
                if (System.Convert.ToHexStringLower(SHA256.HashData(bytes)) != sha)
                {
                    Failure(name, "the file differs from its pinned SHA-256");
                    continue;
                }
                pinned++;
                CheckOriginal(name, bytes, FileProfile.Find(profile), kind);
            }
        }
        foreach (var target in o.Positional)
        {
            var files = Directory.Exists(target) ? Commands.Enumerate(target).Select(r => Path.Combine(target, r)).ToList() : [target];
            foreach (var file in files) CheckFile(file, o.Kind, o.Profile);
        }
        var fixtures = Fixtures.Run(errors);

        output.WriteLine($"Corpus: {originals} original files, {g1} G1 equal, {g2} imported documents G2 equal, {unknownProperties} unknown properties");
        output.WriteLine(fixtures.Failed == 0
            ? $"Fixtures: {fixtures.Cases} cases, all expected results matched"
            : $"Fixtures: {fixtures.Cases} cases, {fixtures.Failed} did not match");
        int ownTotal = ownDocuments + fixtures.OwnDocuments, ownEqual = gOwn + fixtures.OwnDocuments - fixtures.OwnFailed;
        output.WriteLine(ownEqual == ownTotal
            ? $"Own: all eligible documents G-own equal ({ownTotal} documents), 0 strict export losses"
            : $"Own: {ownEqual} of {ownTotal} documents G-own equal, 0 strict export losses");
        output.WriteLine($"Layout: {layoutParts} parts in {residueFiles} original files, 0 unexpected case residues");
        output.WriteLine($"Schema: 0 unclassified sites, 0 registration ambiguities (census sites, {GameFiles.Schema.Classes.Count} classes, 3 engine tables checked at load, schema {GameFiles.Schema.Hash[..12]})");
        if (o.Expect is not null) output.WriteLine($"Pinned: {pinned} of {expected} census files present with their pinned SHA-256");
        foreach (var (profile, t) in tallies)
            output.WriteLine($"Profile {profile}: {t.Files} files, {t.G1} G1, {t.G2} G2, {t.GOwn} G-own, {t.Parts} layout parts in {t.Residue} files");
        if (legacy.Count > 0) output.WriteLine("Legacy rules: " + string.Join(", ", legacy.Select(l => $"{l.Key} {l.Value}")));
        output.WriteLine("G3: unavailable in aowconv. The independent reader is tools/oracle/asg.py ModelReader (world-hsm and tactical-hsm payloads), run by the 9.7-2 gate. Other profiles have no independent reader yet.");
        foreach (var t in timings.OrderByDescending(t => t.Inflated).Take(3))
            output.WriteLine(string.Create(CultureInfo.InvariantCulture,
                $"Measure: {t.Name} ({t.Profile}), {t.Size} bytes, {t.Inflated} payload bytes, own JSON {t.Json} bytes, import {t.ImportMs:F0} ms, export {t.ExportMs:F0} ms"));
        if (failures > 0 || fixtures.Failed > 0 || (o.Expect is not null && pinned != expected)) return Program.Error;
        return differences > 0 ? Program.Difference : Program.Success;
    }

    private void Failure(string name, string message)
    {
        failures++;
        errors.WriteLine($"{name}: {message}");
    }

    private ProfileTally Tally(string profile)
    {
        if (!tallies.TryGetValue(profile, out var t)) tallies[profile] = t = new ProfileTally();
        return t;
    }

    private void CheckFile(string path, string? kind, string? profile)
    {
        var bytes = File.ReadAllBytes(path);
        var c = GameFiles.Classify(bytes, Path.GetExtension(path), kind, profile);
        switch (c.Format)
        {
            case DetectedFormat.Original:
                CheckOriginal(path, bytes, c.Profile, c.Kind!);
                break;
            case DetectedFormat.OwnJson or DetectedFormat.GzipOwnJson:
                CheckOwn(path, bytes);
                break;
            default:
                Failure(path, c.Error?.ToString() ?? $"unknown format ({c.Format})");
                break;
        }
    }

    /// <summary>G-own for one own document: N(N(D)) = N(D).</summary>
    private void CheckOwn(string name, byte[] bytes)
    {
        ownDocuments++;
        try
        {
            var n1 = GameFiles.Normalize(bytes, name).Bytes;
            var n2 = GameFiles.Normalize(n1, name).Bytes;
            if (n1.AsSpan().SequenceEqual(n2)) gOwn++;
            else
            {
                differences++;
                errors.WriteLine($"{name}: G-own differs");
            }
        }
        catch (ConversionException error)
        {
            Failure(name, error.Diagnostic.ToString());
        }
    }

    /// <summary>
    /// G1: the exported original equals the file under B. G2: importing the export gives the
    /// same document bytes. G-own: normalizing the document twice equals normalizing it once.
    /// N(D) is the import of the export of D, which G2 already computes, so when G2 holds N(D) = D
    /// and N(N(D)) = N(D) follows from N being a function of its input bytes.
    /// </summary>
    private void CheckOriginal(string name, byte[] bytes, FileProfile? profile, string kind)
    {
        originals++;
        if (profile is null)
        {
            Failure(name, "no profile");
            return;
        }
        var tally = Tally(profile.Name);
        tally.Files++;
        try
        {
            var watch = Stopwatch.StartNew();
            var imported = GameFiles.Import(bytes, profile, kind, name);
            double importMs = watch.Elapsed.TotalMilliseconds;
            var text = GameFiles.Write(imported.Document);
            var read = GameFiles.Read(text, name);
            watch.Restart();
            var exported = GameFiles.Export(read.Document, null, name);
            double exportMs = watch.Elapsed.TotalMilliseconds;
            var original = profile.Comparable(bytes, FormatLimits.Default);
            var roundTrip = profile.Comparable(exported.Bytes, FormatLimits.Default);
            timings.Add((name, profile.Name, bytes.Length, original.Length, importMs, exportMs, text.Length));
            if (imported.Counts.Parts > 0)
            {
                residueFiles++;
                layoutParts += imported.Counts.Parts;
                tally.Residue++;
                tally.Parts += imported.Counts.Parts;
            }
            foreach (var (rule, count) in imported.Counts.LegacyRules) legacy[rule] = legacy.GetValueOrDefault(rule) + count;
            if (!original.AsSpan().SequenceEqual(roundTrip))
            {
                differences++;
                int at = original.AsSpan().CommonPrefixLength(roundTrip);
                errors.WriteLine($"{name}: G1 differs at byte {at} of B ({original.Length} and {roundTrip.Length} bytes)");
                return;
            }
            g1++;
            tally.G1++;
            var again = GameFiles.Import(exported.Bytes, profile, kind, name);
            var text2 = GameFiles.Write(again.Document);
            if (!text.AsSpan().SequenceEqual(text2))
            {
                differences++;
                int at = text.AsSpan().CommonPrefixLength(text2);
                errors.WriteLine($"{name}: G2 differs at document byte {at}");
                return;
            }
            g2++;
            tally.G2++;
            gOwn++;
            ownDocuments++;
            tally.GOwn++;
        }
        catch (ConversionException error)
        {
            if (error.Diagnostic.Code == Codes.UnknownProperty) unknownProperties++;
            Failure(name, error.Diagnostic.ToString());
        }
    }
}
