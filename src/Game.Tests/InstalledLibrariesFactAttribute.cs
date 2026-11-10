
namespace Aow.Game.Tests;

/// <summary>Skips with the resolved path when the installed image libraries are absent.</summary>
public sealed class InstalledLibrariesFactAttribute : FactAttribute
{
    internal static string[] Folders => [Path.Combine(InstalledFontsFactAttribute.Root, "Images"), Path.Combine(InstalledFontsFactAttribute.Root, "Int")];

    public InstalledLibrariesFactAttribute()
    {
        if (Folders.Any(folder => !Directory.Exists(folder) || !Directory.EnumerateFiles(folder, "*.ILB", SearchOption.AllDirectories).Any()))
            Skip = $"Installed image libraries are absent: {string.Join(", ", Folders)}. Set AOW_GAME_ROOT to the game installation.";
    }
}
