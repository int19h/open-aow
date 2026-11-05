using Aow.Convert;

namespace Aow.Formats.Tests;

/// <summary>Whether the installed game is present at AOW_GAME_ROOT or the default root.</summary>
internal static class InstalledGame
{
    public static string Root => Path.GetFullPath(Checker.GameRoot);

    public static bool Present =>
        File.Exists(Path.Combine(Root, "Release", "Release.hss")) && Directory.Exists(Path.Combine(Root, "Scenario", "Custom"));

    public static string? SkipReason =>
        Present ? null : $"The installed game is absent: {Root}. Set AOW_GAME_ROOT to the game installation.";
}

/// <summary>A fact that skips with the resolved root when the installed game is absent.</summary>
public sealed class GameFactAttribute : FactAttribute
{
    public GameFactAttribute()
    {
        if (InstalledGame.SkipReason is string reason) Skip = reason;
    }
}

/// <summary>A theory that skips with the resolved root when the installed game is absent.</summary>
public sealed class GameTheoryAttribute : TheoryAttribute
{
    public GameTheoryAttribute()
    {
        if (InstalledGame.SkipReason is string reason) Skip = reason;
    }
}
