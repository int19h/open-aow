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
    private int caseResidues;
    private readonly SortedDictionary<string, (int Occurrences, List<string> Files)> legacy = new(StringComparer.Ordinal);
    private readonly SortedDictionary<string, int> categories = new(StringComparer.Ordinal);
    // The census corpus alone, compared with the pinned expectation.
    private int corpusParts, corpusResidueFiles, corpusCaseResidues;
    private readonly SortedDictionary<string, int> corpusCategories = new(StringComparer.Ordinal);
    private readonly SortedDictionary<string, (int Parts, int Files)> corpusProfiles = new(StringComparer.Ordinal);
    private readonly SortedDictionary<string, (int Occurrences, List<string> Files)> corpusLegacy = new(StringComparer.Ordinal);
    private readonly List<(string Name, string Profile, long Size, long Inflated, double ImportMs, double ExportMs, long Json)> timings = [];

    public static string GameRoot => Environment.GetEnvironmentVariable("AOW_GAME_ROOT") is { Length: > 0 } r ? r : @"D:\Games\GOG\Age of Wonders";
    public static string OracleRoot => Environment.GetEnvironmentVariable("AOW_ORACLE_ROOT") is { Length: > 0 } r ? r : @"D:\Build\oracle";

    public int Run(Options o)
    {
        int pinned = 0, expected = 0;
        Expectation? expectation = null;
        CensusFacts? census = null;
        if (o.Expect is not null)
        {
            try
            {
                (census, expectation) = Expectation.Load(o.Expect);
            }
            catch (ExpectationException error)
            {
                errors.WriteLine($"{o.Expect}: {error.Message}");
                return Program.Error;
            }
            foreach (var row in census.Rows)
            {
                expected++;
                string root = row.SourceRoot == "install" ? GameRoot : OracleRoot;
                string path = Path.Combine(root, row.RelativePath.Replace('/', Path.DirectorySeparatorChar));
                if (!File.Exists(path))
                {
                    Failure(row.Identity, $"pinned file {path} is missing");
                    continue;
                }
                var bytes = File.ReadAllBytes(path);
                if (System.Convert.ToHexStringLower(SHA256.HashData(bytes)) != row.Sha256)
                {
                    Failure(row.Identity, "the file differs from its pinned SHA-256");
                    continue;
                }
                pinned++;
                CheckOriginal(row.Identity, bytes, FileProfile.Find(row.Profile), row.Kind, true);
            }
        }
        foreach (var target in o.Positional)
        {
            var files = Directory.Exists(target) ? Commands.Enumerate(target).Select(r => Path.Combine(target, r)).ToList() : [target];
            foreach (var file in files) CheckFile(file, o.Kind, o.Profile);
        }
        var fixtures = Fixtures.Run(errors);

        var schema = GameFiles.Schema;
        output.WriteLine($"Corpus: {originals} original files, {g1} G1 equal, {g2} imported documents G2 equal, {unknownProperties} unknown properties");
        output.WriteLine(fixtures.Failed == 0
            ? $"Fixtures: {fixtures.Cases} cases, all expected results matched"
            : $"Fixtures: {fixtures.Cases} cases, {fixtures.Failed} did not match");
        int ownTotal = ownDocuments + fixtures.OwnDocuments, ownEqual = gOwn + fixtures.OwnDocuments - fixtures.OwnFailed;
        output.WriteLine(ownEqual == ownTotal
            ? $"Own: all eligible documents G-own equal ({ownTotal} documents), 0 strict export losses"
            : $"Own: {ownEqual} of {ownTotal} documents G-own equal, 0 strict export losses");
        output.WriteLine($"Layout: {layoutParts} parts in {residueFiles} original files, {caseResidues} unexpected case residues");
        output.WriteLine((census is null
            ? "Schema: unclassified sites not checked without a census, "
            : $"Schema: {census.UnexplainedSites} unclassified sites (census {census.Sha256[..12]}), ") +
            $"{schema.RegistrationAmbiguities.Count} registration ambiguities in {schema.Engines.Count} engine tables (measured at schema load, {schema.Classes.Count} classes, schema {schema.Hash[..12]})");
        foreach (var a in schema.RegistrationAmbiguities) errors.WriteLine("registration ambiguity: " + a);
        if (census is not null) output.WriteLine($"Pinned: {pinned} of {expected} census files present with their pinned SHA-256");
        foreach (var (profile, t) in tallies)
            output.WriteLine($"Profile {profile}: {t.Files} files, {t.G1} G1, {t.G2} G2, {t.GOwn} G-own, {t.Parts} layout parts in {t.Residue} files");
        if (categories.Count > 0)
            output.WriteLine("Layout parts by category: " + string.Join(", ", categories.Select(c => $"{c.Key} {c.Value}")));
        output.WriteLine(legacy.Count == 0 ? "Legacy rules: none fired" :
            "Legacy rules: " + string.Join(", ", legacy.Select(l => $"{l.Key} {l.Value.Occurrences} in {l.Value.Files.Count} file(s) ({string.Join(", ", l.Value.Files)})")));
        output.WriteLine("G3: unavailable in aowconv. The independent reader is tools/oracle/asg.py ModelReader (world-hsm and tactical-hsm payloads), run by the 9.7-2 gate. Other profiles have no independent reader yet.");
        foreach (var t in timings.OrderByDescending(t => t.Inflated).Take(3))
            output.WriteLine(string.Create(CultureInfo.InvariantCulture,
                $"Measure: {t.Name} ({t.Profile}), {t.Size} bytes, {t.Inflated} payload bytes, own JSON {t.Json} bytes, import {t.ImportMs:F0} ms, export {t.ExportMs:F0} ms"));
        bool drift = false;
        if (expectation is not null)
        {
            var problems = expectation.Compare(corpusParts, corpusResidueFiles, corpusCategories, corpusProfiles, corpusLegacy, corpusCaseResidues);
            foreach (var p in problems) errors.WriteLine("expectation drift: " + p);
            drift = problems.Count > 0;
            output.WriteLine(drift
                ? $"Expect: {problems.Count} pinned layout counts differ from {o.Expect}"
                : $"Expect: layout parts, residue files, categories, per-profile residue, legacy rules and case residues match {Path.GetFileName(o.Expect)} ({expectation.Sha256[..12]})");
        }
        else if (census is not null)
            output.WriteLine("Expect: census only, layout counts are not pinned. Pass the 9.7-1 check expectation file to pin them.");
        if (failures > 0 || fixtures.Failed > 0 || drift || schema.RegistrationAmbiguities.Count > 0 || (census is not null && pinned != expected)) return Program.Error;
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
                CheckOriginal(path, bytes, c.Profile, c.Kind!, false);
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
    private void CheckOriginal(string name, byte[] bytes, FileProfile? profile, string kind, bool inCensus)
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
            int residues = CountCaseResidues(imported.Document);
            caseResidues += residues;
            if (imported.Counts.Parts > 0)
            {
                residueFiles++;
                layoutParts += imported.Counts.Parts;
                tally.Residue++;
                tally.Parts += imported.Counts.Parts;
            }
            foreach (var (category, count) in imported.Counts.Categories) categories[category] = categories.GetValueOrDefault(category) + count;
            foreach (var (rule, count) in imported.Counts.LegacyRules) Add(legacy, rule, count, name);
            if (inCensus)
            {
                corpusCaseResidues += residues;
                if (imported.Counts.Parts > 0)
                {
                    corpusResidueFiles++;
                    corpusParts += imported.Counts.Parts;
                    var (parts, files) = corpusProfiles.GetValueOrDefault(profile.Name);
                    corpusProfiles[profile.Name] = (parts + imported.Counts.Parts, files + 1);
                }
                foreach (var (category, count) in imported.Counts.Categories) corpusCategories[category] = corpusCategories.GetValueOrDefault(category) + count;
                foreach (var (rule, count) in imported.Counts.LegacyRules) Add(corpusLegacy, rule, count, name);
            }
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

    private static void Add(SortedDictionary<string, (int Occurrences, List<string> Files)> map, string rule, int count, string file)
    {
        if (!map.TryGetValue(rule, out var entry)) entry = (0, []);
        entry.Files.Add(file);
        map[rule] = (entry.Occurrences + count, entry.Files);
    }

    /// <summary>Semantic $residue members in a document: meaningful values outside a mapped case.</summary>
    internal static int CountCaseResidues(JsonValue value)
    {
        int count = 0;
        var pending = new Stack<JsonValue>();
        pending.Push(value);
        while (pending.Count > 0)
        {
            switch (pending.Pop())
            {
                case JsonObject obj:
                    foreach (var m in obj.Members)
                    {
                        if (m.Name == "$residue") count++;
                        else pending.Push(m.Value);
                    }
                    break;
                case JsonArray array:
                    foreach (var item in array.Items) pending.Push(item);
                    break;
            }
        }
        return count;
    }
}
