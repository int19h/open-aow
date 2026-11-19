using System.Security.Cryptography;
using System.Text.Json;

namespace Aow.Convert;

public sealed class ExpectationException(string message) : Exception(message);

/// <summary>One in-scope row of the accepted census manifest.</summary>
public sealed record CensusRow(string Identity, string SourceRoot, string RelativePath, string Profile, string Kind, string Sha256);

/// <summary>G1 outcome counts for one profile.</summary>
public readonly record struct ProfileG1Counts(int Exact, int Masked, int Document);

/// <summary>The census facts check uses: its rows, its unexplained site count and its own hash.</summary>
public sealed class CensusFacts
{
    public required IReadOnlyList<CensusRow> Rows { get; init; }
    public required int UnexplainedSites { get; init; }
    public required string Sha256 { get; init; }

    public static CensusFacts Read(byte[] bytes)
    {
        using JsonDocument census = JsonDocument.Parse(bytes, new JsonDocumentOptions { MaxDepth = 256 });
        List<CensusRow> rows = [];
        foreach (JsonElement row in census.RootElement.GetProperty("corpus").EnumerateArray())
        {
            if (row.GetProperty("scope").GetString() != "convert")
            {
                continue;
            }

            rows.Add(new CensusRow(row.GetProperty("identity").GetString()!, row.GetProperty("sourceRoot").GetString()!,
                row.GetProperty("relativePath").GetString()!, row.GetProperty("profile").GetString()!,
                row.GetProperty("kind").GetString()!, row.GetProperty("sha256").GetString()!));
        }
        return new CensusFacts
        {
            Rows = rows,
            UnexplainedSites = census.RootElement.GetProperty("summary").GetProperty("unexplainedSites").GetInt32(),
            Sha256 = System.Convert.ToHexStringLower(SHA256.HashData(bytes)),
        };
    }
}

/// <summary>
/// The pinned G1 expectation of a census run: files byte-exact, files exact outside declared
/// marks (with the bytes masked per rule), files compared as documents, per-profile counts and
/// legacy rules with their files. It names the census it belongs to by path and SHA-256. Any
/// difference is drift and fails the check.
/// </summary>
public sealed class Expectation
{
    public required string Sha256 { get; init; }
    public required int Exact { get; init; }
    public required int Masked { get; init; }
    public required int Document { get; init; }
    public required SortedDictionary<string, long> Rules { get; init; }
    public required SortedDictionary<string, ProfileG1Counts> Profiles { get; init; }
    public required SortedDictionary<string, (int Occurrences, List<string> Files)> LegacyRules { get; init; }

    /// <summary>
    /// Loads --expect: a census manifest alone (its rows, no G1 pin), or an expectation file
    /// that names its census by a path relative to itself and its pinned SHA-256.
    /// </summary>
    public static (CensusFacts Census, Expectation? Expectation) Load(string path)
    {
        byte[] bytes = File.ReadAllBytes(path);
        using JsonDocument doc = JsonDocument.Parse(bytes, new JsonDocumentOptions { MaxDepth = 256 });
        JsonElement root = doc.RootElement;
        if (root.TryGetProperty("corpus", out _))
        {
            return (CensusFacts.Read(bytes), null);
        }

        if (!root.TryGetProperty("format", out JsonElement format) || format.GetString() != "aowconv-check-expectation")
        {
            throw new ExpectationException("The file is neither a census manifest nor an aowconv check expectation.");
        }

        JsonElement reference = root.GetProperty("census");
        string censusPath = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(path))!, reference.GetProperty("path").GetString()!);
        if (!File.Exists(censusPath))
        {
            throw new ExpectationException($"The census {censusPath} is missing.");
        }

        CensusFacts census = CensusFacts.Read(File.ReadAllBytes(censusPath));
        if (census.Sha256 != reference.GetProperty("sha256").GetString())
        {
            throw new ExpectationException($"The census {censusPath} differs from its pinned SHA-256.");
        }

        JsonElement g1 = root.GetProperty("g1");
        SortedDictionary<string, long> maskedRules = [with(StringComparer.Ordinal)];
        foreach (JsonProperty r in g1.GetProperty("rules").EnumerateObject())
        {
            maskedRules[r.Name] = r.Value.GetInt64();
        }

        SortedDictionary<string, ProfileG1Counts> profiles = [with(StringComparer.Ordinal)];
        foreach (JsonProperty p in g1.GetProperty("profiles").EnumerateObject())
        {
            profiles[p.Name] = new ProfileG1Counts(
                Exact: p.Value.GetProperty("exact").GetInt32(),
                Masked: p.Value.GetProperty("masked").GetInt32(),
                Document: p.Value.GetProperty("document").GetInt32());
        }

        SortedDictionary<string, (int, List<string>)> legacy = [with(StringComparer.Ordinal)];
        foreach (JsonProperty r in root.GetProperty("legacyRules").EnumerateObject())
        {
            legacy[r.Name] = (r.Value.GetProperty("occurrences").GetInt32(),
                [.. r.Value.GetProperty("files").EnumerateArray().Select(f => f.GetString()!)]);
        }

        return (census, new Expectation
        {
            Sha256 = System.Convert.ToHexStringLower(SHA256.HashData(bytes)),
            Exact = g1.GetProperty("exact").GetInt32(),
            Masked = g1.GetProperty("masked").GetInt32(),
            Document = g1.GetProperty("document").GetInt32(),
            Rules = maskedRules,
            Profiles = profiles,
            LegacyRules = legacy,
        });
    }

    /// <summary>Every difference between the pinned and the measured census counts.</summary>
    public List<string> Compare(
        int exact,
        int masked,
        int document,
        SortedDictionary<string, long> maskedRules,
        SortedDictionary<string, ProfileG1Counts> profiles,
        SortedDictionary<string, (int Occurrences, List<string> Files)> legacyRules)
    {
        List<string> problems = [];
        void Check(string what, object pinned, object measured)
        {
            if (!Equals(pinned, measured))
            {
                problems.Add($"{what}: pinned {pinned}, measured {measured}");
            }
        }

        Check("G1 exact files", Exact, exact);
        Check("G1 masked files", Masked, masked);
        Check("G1 document files", Document, document);
        foreach (string key in Rules.Keys.Union(maskedRules.Keys).Order(StringComparer.Ordinal))
        {
            Check("masked rule " + key, Rules.GetValueOrDefault(key), maskedRules.GetValueOrDefault(key));
        }

        foreach (string key in Profiles.Keys.Union(profiles.Keys).Order(StringComparer.Ordinal))
        {
            Check("profile " + key, Profiles.GetValueOrDefault(key), profiles.GetValueOrDefault(key));
        }

        foreach (string key in LegacyRules.Keys.Union(legacyRules.Keys).Order(StringComparer.Ordinal))
        {
            (int pinnedOccurrences, List<string> pinnedFiles) = LegacyRules.GetValueOrDefault(key, (0, []));
            (int measuredOccurrences, List<string> measuredFiles) = legacyRules.GetValueOrDefault(key, (0, []));
            Check($"legacy rule {key} occurrences", pinnedOccurrences, measuredOccurrences);
            Check($"legacy rule {key} files", string.Join(", ", pinnedFiles.Order(StringComparer.Ordinal)), string.Join(", ", measuredFiles.Order(StringComparer.Ordinal)));
        }

        return problems;
    }
}
