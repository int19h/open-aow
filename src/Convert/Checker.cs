using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;

using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Game.Saves;

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
        public int Files, OriginalRoundTrips, DocumentRoundTrips, StableNormalizations, Residue, Parts;
    }

    private readonly SortedDictionary<string, ProfileTally> _tallies = new(StringComparer.Ordinal);
    private int _originals, _originalRoundTrips, _documentRoundTrips, _stableNormalizations, _ownDocuments, _unknownProperties, _failures, _differences, _layoutParts, _residueFiles;
    private int _caseResidues, _validated, _refused;
    private readonly SortedDictionary<string, (int Occurrences, List<string> Files)> _legacy = new(StringComparer.Ordinal);
    private readonly SortedDictionary<string, int> _categories = new(StringComparer.Ordinal);
    // The census corpus alone, compared with the pinned expectation.
    private int _corpusParts, _corpusResidueFiles, _corpusCaseResidues;
    private readonly SortedDictionary<string, int> _corpusCategories = new(StringComparer.Ordinal);
    private readonly SortedDictionary<string, (int Parts, int Files)> _corpusProfiles = new(StringComparer.Ordinal);
    private readonly SortedDictionary<string, (int Occurrences, List<string> Files)> _corpusLegacy = new(StringComparer.Ordinal);
    private readonly List<(string Name, string Profile, long Size, long Inflated, double ImportMs, double ExportMs, long Json)> _timings = [];

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
        output.WriteLine($"Corpus: {_originals} original files, {_originalRoundTrips} G1 equal, {_documentRoundTrips} imported documents G2 equal, {_unknownProperties} unknown properties");
        output.WriteLine(fixtures.Failed == 0
            ? $"Fixtures: {fixtures.Cases} cases, all expected results matched"
            : $"Fixtures: {fixtures.Cases} cases, {fixtures.Failed} did not match");
        int ownTotal = _ownDocuments + fixtures.OwnDocuments, ownEqual = _stableNormalizations + fixtures.OwnDocuments - fixtures.OwnFailed;
        output.WriteLine(ownEqual == ownTotal
            ? $"Own: all eligible documents G-own equal ({ownTotal} documents), 0 strict export losses"
            : $"Own: {ownEqual} of {ownTotal} documents G-own equal, 0 strict export losses");
        output.WriteLine($"Layout: {_layoutParts} parts in {_residueFiles} original files, {_caseResidues} unexpected case residues");
        output.WriteLine($"Validation: {_validated} imported documents pass the unit validators ({string.Join(", ", FormatMapping.Validators.Select(v => v.Name))}, {FormatMapping.All.Count} mapped classes), {_refused} refused");
        output.WriteLine((census is null
            ? "Schema: unclassified sites not checked without a census, "
            : $"Schema: {census.UnexplainedSites} unclassified sites (census {census.Sha256[..12]}), ") +
            $"{schema.RegistrationAmbiguities.Count} registration ambiguities in {schema.Engines.Count} engine tables (measured at schema load, {schema.Classes.Count} classes, schema {schema.Hash[..12]})");
        foreach (var a in schema.RegistrationAmbiguities) errors.WriteLine("registration ambiguity: " + a);
        if (census is not null) output.WriteLine($"Pinned: {pinned} of {expected} census files present with their pinned SHA-256");
        foreach (var (profile, t) in _tallies)
            output.WriteLine($"Profile {profile}: {t.Files} files, {t.OriginalRoundTrips} G1, {t.DocumentRoundTrips} G2, {t.StableNormalizations} G-own, {t.Parts} layout parts in {t.Residue} files");
        if (_categories.Count > 0)
            output.WriteLine("Layout parts by category: " + string.Join(", ", _categories.Select(c => $"{c.Key} {c.Value}")));
        output.WriteLine(_legacy.Count == 0 ? "Legacy rules: none fired" :
            "Legacy rules: " + string.Join(", ", _legacy.Select(l => $"{l.Key} {l.Value.Occurrences} in {l.Value.Files.Count} file(s) ({string.Join(", ", l.Value.Files)})")));
        output.WriteLine("G3: unavailable in aowconv. The independent reader is tools/oracle/asg.py ModelReader (world-hsm and tactical-hsm payloads), run by the 9.7-2 gate. Other profiles have no independent reader yet.");
        foreach (var t in _timings.OrderByDescending(t => t.Inflated).Take(3))
            output.WriteLine(string.Create(CultureInfo.InvariantCulture,
                $"Measure: {t.Name} ({t.Profile}), {t.Size} bytes, {t.Inflated} payload bytes, own JSON {t.Json} bytes, import {t.ImportMs:F0} ms, export {t.ExportMs:F0} ms"));
        bool drift = false;
        if (expectation is not null)
        {
            var problems = expectation.Compare(_corpusParts, _corpusResidueFiles, _corpusCategories, _corpusProfiles, _corpusLegacy, _corpusCaseResidues);
            foreach (var p in problems) errors.WriteLine("expectation drift: " + p);
            drift = problems.Count > 0;
            output.WriteLine(drift
                ? $"Expect: {problems.Count} pinned layout counts differ from {o.Expect}"
                : $"Expect: layout parts, residue files, categories, per-profile residue, legacy rules and case residues match {Path.GetFileName(o.Expect)} ({expectation.Sha256[..12]})");
        }
        else if (census is not null)
            output.WriteLine("Expect: census only, layout counts are not pinned. Pass the 9.7-1 check expectation file to pin them.");
        if (_failures > 0 || fixtures.Failed > 0 || drift || schema.RegistrationAmbiguities.Count > 0 || (census is not null && pinned != expected)) return Program.Error;
        return _differences > 0 ? Program.Difference : Program.Success;
    }

    private void Failure(string name, string message)
    {
        _failures++;
        errors.WriteLine($"{name}: {message}");
    }

    private ProfileTally Tally(string profile)
    {
        if (!_tallies.TryGetValue(profile, out var t)) _tallies[profile] = t = new ProfileTally();
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
        _ownDocuments++;
        try
        {
            var n1 = GameFiles.Normalize(bytes, name).Bytes;
            var n2 = GameFiles.Normalize(n1, name).Bytes;
            if (n1.AsSpan().SequenceEqual(n2)) _stableNormalizations++;
            else
            {
                _differences++;
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
        _originals++;
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
            double importMilliseconds = watch.Elapsed.TotalMilliseconds;
            var text = GameFiles.Write(imported.Document);
            var read = GameFiles.Read(text, name);
            watch.Restart();
            var exported = GameFiles.Export(read.Document, null, name);
            double exportMilliseconds = watch.Elapsed.TotalMilliseconds;
            var original = profile.Comparable(bytes, FormatLimits.Default);
            var roundTrip = profile.Comparable(exported.Bytes, FormatLimits.Default);
            _timings.Add((name, profile.Name, bytes.Length, original.Length, importMilliseconds, exportMilliseconds, text.Length));
            int residues = CountCaseResidues(imported.Document);
            // The unit validators the game runs before it loads (own-formats.md 2.7). A refusal of
            // an original the game loads is a failure of the validators.
            var context = new ValidationContext(imported.Document, profile, kind, name);
            var findings = FormatMapping.Validators.SelectMany(v => v.Validate(context)).ToList();
            if (findings.Count == 0) _validated++;
            else
            {
                _refused++;
                foreach (var finding in findings) Failure(name, "unit validation: " + finding);
            }
            _caseResidues += residues;
            if (imported.Counts.Parts > 0)
            {
                _residueFiles++;
                _layoutParts += imported.Counts.Parts;
                tally.Residue++;
                tally.Parts += imported.Counts.Parts;
            }
            foreach (var (category, count) in imported.Counts.Categories) _categories[category] = _categories.GetValueOrDefault(category) + count;
            foreach (var (rule, count) in imported.Counts.LegacyRules) Add(_legacy, rule, count, name);
            if (inCensus)
            {
                _corpusCaseResidues += residues;
                if (imported.Counts.Parts > 0)
                {
                    _corpusResidueFiles++;
                    _corpusParts += imported.Counts.Parts;
                    var (parts, files) = _corpusProfiles.GetValueOrDefault(profile.Name);
                    _corpusProfiles[profile.Name] = (parts + imported.Counts.Parts, files + 1);
                }
                foreach (var (category, count) in imported.Counts.Categories) _corpusCategories[category] = _corpusCategories.GetValueOrDefault(category) + count;
                foreach (var (rule, count) in imported.Counts.LegacyRules) Add(_corpusLegacy, rule, count, name);
            }
            if (!original.AsSpan().SequenceEqual(roundTrip))
            {
                _differences++;
                int at = original.AsSpan().CommonPrefixLength(roundTrip);
                errors.WriteLine($"{name}: G1 differs at byte {at} of B ({original.Length} and {roundTrip.Length} bytes)");
                return;
            }
            _originalRoundTrips++;
            tally.OriginalRoundTrips++;
            var again = GameFiles.Import(exported.Bytes, profile, kind, name);
            var text2 = GameFiles.Write(again.Document);
            if (!text.AsSpan().SequenceEqual(text2))
            {
                _differences++;
                int at = text.AsSpan().CommonPrefixLength(text2);
                errors.WriteLine($"{name}: G2 differs at document byte {at}");
                return;
            }
            _documentRoundTrips++;
            tally.DocumentRoundTrips++;
            _stableNormalizations++;
            _ownDocuments++;
            tally.StableNormalizations++;
        }
        catch (ConversionException error)
        {
            if (error.Diagnostic.Code == Codes.UnknownProperty) _unknownProperties++;
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
                case JsonObject objectValue:
                    foreach (var m in objectValue.Members)
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
