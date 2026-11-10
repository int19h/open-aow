
namespace Aow.Game.Tests;

/// <summary>Whether the installed game is present at AOW_GAME_ROOT or the default root.</summary>
internal static class InstalledSave
{
    public static string Root => Path.GetFullPath(Environment.GetEnvironmentVariable("AOW_GAME_ROOT") is { Length: > 0 } root ? root : @"D:\Games\GOG\Age of Wonders");

    /// <summary>A shipped original save, the source of the edited documents.</summary>
    public static string Save => Path.Combine(Root, "Save", "After the Fall.asg");

    public static string? SkipReason => File.Exists(Save) ? null : $"The installed game is absent: {Root}. Set AOW_GAME_ROOT to the game installation.";
}
