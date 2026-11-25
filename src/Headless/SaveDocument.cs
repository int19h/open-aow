using Aow.Formats;

namespace Aow.Headless;

/// <summary>Which game a loaded file is. The decision is the document profile, never the extension.</summary>
internal enum SaveKind
{
    WorldMap,
    Campaign,
}

internal static class SaveDocument
{
    public static SaveKind Kind(string path) => Kind(File.ReadAllBytes(path));

    public static SaveKind Kind(ReadOnlySpan<byte> bytes)
    {
        // An original image has no profile of its own. The host has always opened that as a world map.
        if (!GameFiles.IsGzip(bytes) && !GameFiles.IsJsonCandidate(bytes))
        {
            return SaveKind.WorldMap;
        }

        Classification classification = GameFiles.Classify(bytes.ToArray(), extension: null);
        if (classification.Error is { } error)
        {
            throw new InvalidDataException(error.Message);
        }

        string? profile = classification.Profile?.Name;
        return profile == GameProfile.Campaign.Profile.Name
            ? SaveKind.Campaign
            : profile == GameProfile.WorldMap.Profile.Name
                ? SaveKind.WorldMap
                : throw new InvalidDataException(profile is null
                    ? "The document has no profile."
                    : $"The document profile {profile} is not a game the host plays.");
    }
}
