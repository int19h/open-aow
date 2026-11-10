#nullable enable

namespace Aow.Game.Saves;

/// <summary>
/// The identity of a cached preview (own-formats.md 4.3): the resolved layer, the logical path,
/// the profile, the schema version and the SHA-256 of the bytes just read. No change token is
/// trusted, so every lookup reads and hashes the file first. A same-length hand edit changes the
/// hash and a new shadowing file changes the layer.
/// </summary>
public readonly record struct PreviewKey(string Layer, string Path, string Profile, int Version, string Sha256);
