using System.Security.Cryptography;
using System.Text.Json;

namespace Aow.Convert;

public sealed class ExpectationException(string message) : System.Exception(message);

/// <summary>One in-scope row of the accepted census manifest.</summary>
public sealed record CensusRow(string Identity, string SourceRoot, string RelativePath, string Profile, string Kind, string Sha256);

/// <summary>The census facts check uses: its rows, its unexplained site count and its own hash.</summary>
public sealed class CensusFacts
{
    public required IReadOnlyList<CensusRow> Rows { get; init; }
    public required int UnexplainedSites { get; init; }
    public required string Sha256 { get; init; }

    public static CensusFacts Read(byte[] bytes)
    {
        using var census = JsonDocument.Parse(bytes, new JsonDocumentOptions { MaxDepth = 256 });
        var rows = new List<CensusRow>();
        foreach (var row in census.RootElement.GetProperty("corpus").EnumerateArray())
        {
            if (row.GetProperty("scope").GetString() != "convert") continue;
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
/// The pinned layout expectation of a census run: layout parts, files with residue, parts per
/// category and per profile, legacy rules with their files, and semantic case residues. It names
/// the census it belongs to by path and SHA-256. Any difference is drift and fails the check.
/// </summary>
public sealed class Expectation
{
    public required string Sha256 { get; init; }
    public required int Parts { get; init; }
    public required int Files { get; init; }
    public required SortedDictionary<string, int> Categories { get; init; }
    public required SortedDictionary<string, (int Parts, int Files)> Profiles { get; init; }
    public required SortedDictionary<string, (int Occurrences, List<string> Files)> LegacyRules { get; init; }
    public required int CaseResidues { get; init; }

    /// <summary>
    /// Loads --expect: a census manifest alone (its rows, no layout pin), or an expectation file
    /// that names its census by a path relative to itself and its pinned SHA-256.
    /// </summary>
    public static (CensusFacts Census, Expectation? Expectation) Load(string path)
    {
        var bytes = File.ReadAllBytes(path);
        using var doc = JsonDocument.Parse(bytes, new JsonDocumentOptions { MaxDepth = 256 });
        var root = doc.RootElement;
        if (root.TryGetProperty("corpus", out _)) return (CensusFacts.Read(bytes), null);
        if (!root.TryGetProperty("format", out var format) || format.GetString() != "aowconv-check-expectation")
            throw new ExpectationException("The file is neither a census manifest nor an aowconv check expectation.");
        var reference = root.GetProperty("census");
        string censusPath = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(path))!, reference.GetProperty("path").GetString()!);
        if (!File.Exists(censusPath)) throw new ExpectationException($"The census {censusPath} is missing.");
        var census = CensusFacts.Read(File.ReadAllBytes(censusPath));
        if (census.Sha256 != reference.GetProperty("sha256").GetString())
            throw new ExpectationException($"The census {censusPath} differs from its pinned SHA-256.");
        var layout = root.GetProperty("layout");
        var categories = new SortedDictionary<string, int>(StringComparer.Ordinal);
        foreach (var c in layout.GetProperty("categories").EnumerateObject()) categories[c.Name] = c.Value.GetInt32();
        var profiles = new SortedDictionary<string, (int, int)>(StringComparer.Ordinal);
        foreach (var p in layout.GetProperty("profiles").EnumerateObject())
            profiles[p.Name] = (p.Value.GetProperty("parts").GetInt32(), p.Value.GetProperty("files").GetInt32());
        var rules = new SortedDictionary<string, (int, List<string>)>(StringComparer.Ordinal);
        foreach (var r in root.GetProperty("legacyRules").EnumerateObject())
            rules[r.Name] = (r.Value.GetProperty("occurrences").GetInt32(),
                r.Value.GetProperty("files").EnumerateArray().Select(f => f.GetString()!).ToList());
        return (census, new Expectation
        {
            Sha256 = System.Convert.ToHexStringLower(SHA256.HashData(bytes)),
            Parts = layout.GetProperty("parts").GetInt32(),
            Files = layout.GetProperty("files").GetInt32(),
            Categories = categories,
            Profiles = profiles,
            LegacyRules = rules,
            CaseResidues = root.GetProperty("caseResidues").GetInt32(),
        });
    }

    /// <summary>Every difference between the pinned and the measured census counts.</summary>
    public List<string> Compare(int parts, int files, SortedDictionary<string, int> categories,
        SortedDictionary<string, (int Parts, int Files)> profiles,
        SortedDictionary<string, (int Occurrences, List<string> Files)> rules, int caseResidues)
    {
        var problems = new List<string>();
        void Check(string what, object pinned, object measured)
        {
            if (!Equals(pinned, measured)) problems.Add($"{what}: pinned {pinned}, measured {measured}");
        }
        Check("layout parts", Parts, parts);
        Check("files with residue", Files, files);
        Check("case residues", CaseResidues, caseResidues);
        foreach (var key in Categories.Keys.Union(categories.Keys).Order(StringComparer.Ordinal))
            Check("category " + key, Categories.GetValueOrDefault(key), categories.GetValueOrDefault(key));
        foreach (var key in Profiles.Keys.Union(profiles.Keys).Order(StringComparer.Ordinal))
            Check("profile " + key, Profiles.GetValueOrDefault(key), profiles.GetValueOrDefault(key));
        foreach (var key in LegacyRules.Keys.Union(rules.Keys).Order(StringComparer.Ordinal))
        {
            var pinned = LegacyRules.GetValueOrDefault(key, (0, []));
            var measured = rules.GetValueOrDefault(key, (0, []));
            Check($"legacy rule {key} occurrences", pinned.Occurrences, measured.Occurrences);
            Check($"legacy rule {key} files", string.Join(", ", pinned.Files.Order(StringComparer.Ordinal)), string.Join(", ", measured.Files.Order(StringComparer.Ordinal)));
        }
        return problems;
    }
}
