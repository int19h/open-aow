// Translated from Sound.pas.
using Aow.Rtl;
using Aow.Rtl.Host;
using static Aow.Rtl.DelphiFiles;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.DelphiTextCompare;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.Audio;

// _searchPath, _sfx. ExecutePathSearch lists every wave file under a directory (with its
// subdirectories) relative to the search path
public partial class SfxManager(Component owner) : Component(owner)
{
    // Sound.pas:407

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
        string oldDirectory;
        void Search(string directory)
        {
            DelphiStringList directories;
            foreach (RtlFileEntry entry in Files.FindEntries(SearchPath + directory + "*." + s_sfxExtension, attributes: 63 & ~16))
            {
                _ = Sfx.Add(UpperCase(directory + entry.Name));
            }

            directories = new DelphiStringList();
            foreach (RtlFileEntry entry in Files.FindEntries(directory + "*.*", attributes: 16))
            {
                if ((entry.Attribute & 16) != 0 && entry.Name != "." && entry.Name != "..")
                {
                    _ = directories.Add(directory + UpperCase(entry.Name) + "\\");
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

        oldDirectory = GetCurrentDir();
        _ = SetCurrentDir(path);
        Search("");
        _ = SetCurrentDir(oldDirectory);
        SearchPath = UpperCase(ExtractRelativePath(AudioHost.BaseSoundEnginePath, SearchPath));
    }

    [Published]
    public string SearchPath { get; set; } = "";

    [Published]
    public SfxList Sfx { get; } = new SfxList();

    private static readonly string s_sfxExtension = "WAV";

}
