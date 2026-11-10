
namespace Aow.Game.Tests;

/// <summary>Skips with the resolved path when the installed fonts are absent.</summary>
public sealed class InstalledFontsFactAttribute : FactAttribute
{
    // The gate sets AOW_GAME_ROOT. The German language frames run from a copy of the same install,
    // so its fonts are the same files.
    internal static string Root => Path.GetFullPath(Environment.GetEnvironmentVariable("AOW_GAME_ROOT") is { Length: > 0 } root
        ? root : @"D:\Games\GOG\Age of Wonders");

    internal static string Fonts => Path.Combine(Root, "Int", "Fonts");

    public InstalledFontsFactAttribute()
    {
        if (!Directory.Exists(Fonts) || !Directory.EnumerateFiles(Fonts, "*.ILB").Any())
            Skip = $"Installed fonts are absent: {Fonts}. Set AOW_GAME_ROOT to the game installation.";
    }
}
