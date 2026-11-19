using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;

using Aow.Formats;
using Aow.Formats.Original;
using Aow.Formats.Schema;
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
        public int Files { get; set; }
        public int OriginalRoundTrips { get; set; }
        public int Exact { get; set; }
        public int Masked { get; set; }
        public int Document { get; set; }
        public int DocumentRoundTrips { get; set; }
        public int StableNormalizations { get; set; }
    }

    private readonly SortedDictionary<string, ProfileTally> _tallies = [with(StringComparer.Ordinal)];
    private int _originals, _originalRoundTrips, _exactRoundTrips, _maskedRoundTrips, _documentG1RoundTrips, _documentRoundTrips, _stableNormalizations, _ownDocuments, _unknownProperties, _failures, _differences;
    private int _validated, _refused;
    private readonly SortedDictionary<string, long> _maskedBytesByRule = [with(StringComparer.Ordinal)];
    private readonly SortedDictionary<string, (int Occurrences, List<string> Files)> _legacy = [with(StringComparer.Ordinal)];
    // The census corpus alone, compared with the pinned expectation.
    private int _corpusExact, _corpusMasked, _corpusDocument;
    private readonly SortedDictionary<string, long> _corpusMaskedRules = [with(StringComparer.Ordinal)];
    private readonly SortedDictionary<string, ProfileG1Counts> _corpusProfiles = [with(StringComparer.Ordinal)];
    private readonly SortedDictionary<string, (int Occurrences, List<string> Files)> _corpusLegacy = [with(StringComparer.Ordinal)];
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
            foreach (CensusRow row in census.Rows)
            {
                expected++;
                string root = row.SourceRoot == "install" ? GameRoot : OracleRoot;
                string path = Path.Combine(root, row.RelativePath.Replace('/', Path.DirectorySeparatorChar));
                if (!File.Exists(path))
                {
                    Failure(row.Identity, $"pinned file {path} is missing");
                    continue;
                }
                byte[] bytes = File.ReadAllBytes(path);
                if (System.Convert.ToHexStringLower(SHA256.HashData(bytes)) != row.Sha256)
                {
                    Failure(row.Identity, "the file differs from its pinned SHA-256");
                    continue;
                }
                pinned++;
                CheckOriginal(row.Identity, bytes, FileProfile.Find(row.Profile), row.Kind, inCensus: true);
            }
        }
        foreach (string target in o.Positional)
        {
            List<string> files = Directory.Exists(target) ? [.. Commands.Enumerate(target).Select(r => Path.Combine(target, r))] : [target];
            foreach (string file in files)
            {
                CheckFile(file, o.Kind, o.Profile);
            }
        }
        Fixtures.Summary fixtures = Fixtures.Run(errors);

        FormatSchema schema = GameFiles.Schema;
        output.WriteLine($"Corpus: {_originals} original files, {_originalRoundTrips} G1 passed ({_exactRoundTrips} exact, {_maskedRoundTrips} masked, {_documentG1RoundTrips} document), {_documentRoundTrips} imported documents G2 equal, {_unknownProperties} unknown properties");
        output.WriteLine(fixtures.Failed == 0
            ? $"Fixtures: {fixtures.Cases} cases, all expected results matched"
            : $"Fixtures: {fixtures.Cases} cases, {fixtures.Failed} did not match");
        int ownTotal = _ownDocuments + fixtures.OwnDocuments, ownEqual = _stableNormalizations + fixtures.OwnDocuments - fixtures.OwnFailed;
        output.WriteLine(ownEqual == ownTotal
            ? $"Own: all eligible documents G-own equal ({ownTotal} documents), 0 strict export losses"
            : $"Own: {ownEqual} of {ownTotal} documents G-own equal, 0 strict export losses");
        output.WriteLine($"G1: {_exactRoundTrips} byte-exact, {_maskedRoundTrips} exact outside marks, {_documentG1RoundTrips} compared as documents");
        if (_maskedBytesByRule.Count > 0)
        {
            output.WriteLine($"Masked bytes by rule: {string.Join(", ", _maskedBytesByRule.Select(r => $"{r.Key} {r.Value}"))}");
        }
        output.WriteLine($"Validation: {_validated} imported documents pass the unit validators ({string.Join(", ", FormatMapping.Validators.Select(v => v.Name))}, {FormatMapping.All.Count} mapped classes), {_refused} refused");
        output.WriteLine((census is null
            ? "Schema: unclassified sites not checked without a census, "
            : $"Schema: {census.UnexplainedSites} unclassified sites (census {census.Sha256[..12]}), ") +
            $"{schema.RegistrationAmbiguities.Count} registration ambiguities in {schema.Engines.Count} engine tables (measured at schema load, {schema.Classes.Count} classes, schema {schema.Hash[..12]})");
        foreach (string a in schema.RegistrationAmbiguities)
        {
            errors.WriteLine($"registration ambiguity: {a}");
        }

        if (census is not null)
        {
            output.WriteLine($"Pinned: {pinned} of {expected} census files present with their pinned SHA-256");
        }

        foreach ((string profile, ProfileTally t) in _tallies)
        {
            output.WriteLine($"Profile {profile}: {t.Files} files, {t.OriginalRoundTrips} G1 ({t.Exact} exact, {t.Masked} masked, {t.Document} document), {t.DocumentRoundTrips} G2, {t.StableNormalizations} G-own");
        }

        output.WriteLine(_legacy.Count == 0 ? "Legacy rules: none fired" :
            $"Legacy rules: {string.Join(", ", _legacy.Select(l => $"{l.Key} {l.Value.Occurrences} in {l.Value.Files.Count} file(s) ({string.Join(", ", l.Value.Files)})"))}");
        output.WriteLine("G3: unavailable in aowconv. The independent reader is tools/oracle/asg.py ModelReader (world-hsm and tactical-hsm payloads), run by the 9.7-2 gate. Other profiles have no independent reader yet.");
        foreach ((string name, string profile, long size, long inflated, double importMs, double exportMs, long json) in _timings.OrderByDescending(t => t.Inflated).Take(3))
        {
            output.WriteLine(string.Create(CultureInfo.InvariantCulture,
                $"Measure: {name} ({profile}), {size} bytes, {inflated} payload bytes, own JSON {json} bytes, import {importMs:F0} ms, export {exportMs:F0} ms"));
        }

        bool drift = false;
        if (expectation is not null)
        {
            List<string> problems = expectation.Compare(_corpusExact, _corpusMasked, _corpusDocument, _corpusMaskedRules, _corpusProfiles, _corpusLegacy);
            foreach (string p in problems)
            {
                errors.WriteLine($"expectation drift: {p}");
            }

            drift = problems.Count > 0;
            output.WriteLine(drift
                ? $"Expect: {problems.Count} pinned G1 counts differ from {o.Expect}"
                : $"Expect: G1 counts, masked bytes by rule, per-profile counts and legacy rules match {Path.GetFileName(o.Expect)} ({expectation.Sha256[..12]})");
        }
        else if (census is not null)
        {
            output.WriteLine("Expect: census only, G1 counts are not pinned. Pass the check expectation file to pin them.");
        }

        return _failures > 0 || fixtures.Failed > 0 || drift || schema.RegistrationAmbiguities.Count > 0 || (census is not null && pinned != expected)
            ? Program.Error
            : _differences > 0 ? Program.Difference : Program.Success;
    }

    private void Failure(string name, string message)
    {
        _failures++;
        errors.WriteLine($"{name}: {message}");
    }

    private ProfileTally Tally(string profile)
    {
        if (!_tallies.TryGetValue(profile, out ProfileTally? t))
        {
            _tallies[profile] = t = new ProfileTally();
        }

        return t;
    }

    private void CheckFile(string path, string? kind, string? profile)
    {
        byte[] bytes = File.ReadAllBytes(path);
        Classification c = GameFiles.Classify(bytes, Path.GetExtension(path), kind, profile);
        switch (c.Format)
        {
            case DetectedFormat.Original:
                CheckOriginal(path, bytes, c.Profile, c.Kind!, inCensus: false);
                break;
            case DetectedFormat.OwnJson or DetectedFormat.GzipOwnJson:
                CheckOwn(path, bytes);
                break;
            case DetectedFormat.MalformedOwn:
            case DetectedFormat.UnsupportedVersion:
            case DetectedFormat.UnsupportedKind:
            case DetectedFormat.Unknown:
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
            byte[] n1 = GameFiles.Normalize(bytes, name);
            byte[] n2 = GameFiles.Normalize(n1, name);
            if (n1.AsSpan().SequenceEqual(n2))
            {
                _stableNormalizations++;
            }
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
    /// G1: the exported original matches the file under rule-based comparison. G2: importing the
    /// export gives the same document bytes. G-own: normalizing the document twice equals
    /// normalizing it once.
    /// </summary>
    private void CheckOriginal(string name, byte[] bytes, FileProfile? profile, string kind, bool inCensus)
    {
        _originals++;
        if (profile is null)
        {
            Failure(name, "no profile");
            return;
        }
        ProfileTally tally = Tally(profile.Name);
        tally.Files++;
        try
        {
            Stopwatch watch = Stopwatch.StartNew();
            ImportResult imported = GameFiles.Import(bytes, profile, kind, name);
            double importMilliseconds = watch.Elapsed.TotalMilliseconds;
            byte[] text = GameFiles.Write(imported.Document);
            ReadResult read = GameFiles.Read(text, name);
            watch.Restart();
            ExportResult exported = GameFiles.Export(read.Document, name);
            double exportMilliseconds = watch.Elapsed.TotalMilliseconds;
            byte[] original = profile.Comparable(bytes, FormatLimits.Default);
            _timings.Add((name, profile.Name, bytes.Length, original.Length, importMilliseconds, exportMilliseconds, text.Length));
            // The unit validators the game runs before it loads (own-formats.md 2.7). A refusal of
            // an original the game loads is a failure of the validators.
            ValidationContext context = new(imported.Document, profile, kind, name);
            List<FormatDiagnostic> findings = [.. FormatMapping.Validators.SelectMany(v => v.Validate(context))];
            if (findings.Count == 0)
            {
                _validated++;
            }
            else
            {
                _refused++;
                foreach (FormatDiagnostic finding in findings)
                {
                    Failure(name, $"unit validation: {finding}");
                }
            }

            foreach ((string rule, int count) in imported.LegacyRules)
            {
                Add(_legacy, rule, count, name);
            }

            OriginalComparisonResult comparison = GameFiles.CompareG1(bytes, exported: exported.Bytes, profile: profile, document: imported.Document, kind: kind);
            if (comparison.Kind == OriginalMatchKind.Mismatch)
            {
                _differences++;
                errors.WriteLine($"{name}: G1 differs at byte {comparison.FirstDifferenceOffset ?? 0} of B ({comparison.Detail ?? "outside declared rules"})");
                return;
            }

            _originalRoundTrips++;
            tally.OriginalRoundTrips++;
            switch (comparison.Kind)
            {
                case OriginalMatchKind.Exact:
                    _exactRoundTrips++;
                    tally.Exact++;
                    break;
                case OriginalMatchKind.Masked:
                    _maskedRoundTrips++;
                    tally.Masked++;
                    foreach ((string rule, long maskedBytes) in comparison.MaskedBytesByRule)
                    {
                        _maskedBytesByRule[rule] = _maskedBytesByRule.GetValueOrDefault(rule) + maskedBytes;
                    }

                    break;
                case OriginalMatchKind.Document:
                    _documentG1RoundTrips++;
                    tally.Document++;
                    break;
                case OriginalMatchKind.Mismatch:
                default:
                    break;
            }

            if (inCensus)
            {
                foreach ((string rule, int count) in imported.LegacyRules)
                {
                    Add(_corpusLegacy, rule, count, name);
                }

                ProfileG1Counts profileCounts = _corpusProfiles.GetValueOrDefault(profile.Name);
                switch (comparison.Kind)
                {
                    case OriginalMatchKind.Exact:
                        _corpusExact++;
                        _corpusProfiles[profile.Name] = profileCounts with { Exact = profileCounts.Exact + 1 };
                        break;
                    case OriginalMatchKind.Masked:
                        _corpusMasked++;
                        _corpusProfiles[profile.Name] = profileCounts with { Masked = profileCounts.Masked + 1 };
                        foreach ((string rule, long maskedBytes) in comparison.MaskedBytesByRule)
                        {
                            _corpusMaskedRules[rule] = _corpusMaskedRules.GetValueOrDefault(rule) + maskedBytes;
                        }

                        break;
                    case OriginalMatchKind.Document:
                        _corpusDocument++;
                        _corpusProfiles[profile.Name] = profileCounts with { Document = profileCounts.Document + 1 };
                        break;
                    case OriginalMatchKind.Mismatch:
                    default:
                        break;
                }
            }

            ImportResult again = GameFiles.Import(exported.Bytes, profile, kind, name);
            byte[] text2 = GameFiles.Write(again.Document);
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
            if (error.Diagnostic.Code == Codes.UnknownProperty)
            {
                _unknownProperties++;
            }

            Failure(name, error.Diagnostic.ToString());
        }
    }

    private static void Add(SortedDictionary<string, (int Occurrences, List<string> Files)> map, string rule, int count, string file)
    {
        if (!map.TryGetValue(rule, out (int Occurrences, List<string> Files) entry))
        {
            entry = (0, []);
        }

        entry.Files.Add(file);
        map[rule] = (entry.Occurrences + count, entry.Files);
    }
}
