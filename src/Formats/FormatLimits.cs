namespace Aow.Formats;

/// <summary>
/// Allocation limits (own-formats.md 2.7). Sizes, offsets and dimension products are checked
/// against them before allocation, and a limit error is located and never yields a partial load.
/// </summary>
public sealed record FormatLimits
{
    public long DecompressedBytes { get; init; } = 256L * 1024 * 1024;
    public int Depth { get; init; } = 128;
    public long Nodes { get; init; } = 4_000_000;
    public int CollectionItems { get; init; } = 1_000_000;
    public int StringBytes { get; init; } = 16 * 1024 * 1024;

    public static FormatLimits Default { get; } = new();
}
