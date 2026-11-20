// Translated from Sound.pas.
using Aow.Rtl;
using Aow.Rtl.Host;
using static Aow.Rtl.DelphiTextCompare;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.Audio;

// _searchPath, _sfx. ExecutePathSearch lists every wave file under a directory (with its
// subdirectories) relative to the search path
public sealed class SfxManager(Component owner) : Component(owner)
{
    // Sound.pas:413
    public override void Dispose()
    {
        Clear();
        Sfx?.Dispose();
        base.Dispose();
    }

    // Sound.pas:420
    public void Clear() => Sfx.Clear();

    // Sound.pas:425
    public void ExecutePathSearch(string path)
    {
        void Search(string directory)
        {
            foreach (RtlFileEntry entry in Files.FindEntries($"{SearchPath}{directory}*.{s_sfxExtension}", attributes: 0b0011_1111 & ~0b0001_0000))
            {
                _ = Sfx.Add(UpperCase(directory + entry.Name));
            }

            DelphiStringList directories = new();
            foreach (RtlFileEntry entry in Files.FindEntries($"{directory}*.*", attributes: 16))
            {
                if ((entry.Attribute & 0b0001_0000) != 0 && entry.Name != "." && entry.Name != "..")
                {
                    _ = directories.Add($"{directory}{UpperCase(entry.Name)}\\");
                }
            }

            for (int i = 0, last = directories.Count - 1; i <= last; i++)
            {
                Search(directories[i]);
            }

            directories?.Dispose();
        }

        Sfx.Clear();
        SearchPath = UpperCase(path);
        if (SearchPath[^1] != 92)
        {
            SearchPath += "\\";
        }

        string oldDirectory = Files.CurrentDirectory;
        _ = Files.SetCurrentDirectory(path);
        Search("");
        _ = Files.SetCurrentDirectory(oldDirectory);
        SearchPath = UpperCase(Files.ExtractRelativePath(AudioHost.BaseSoundEnginePath, destination: SearchPath));
    }

    [Published]
    public string SearchPath { get; private set; } = "";

    [Published]
    public SfxList Sfx { get; } = new SfxList();

    private static readonly string s_sfxExtension = "WAV";

}
