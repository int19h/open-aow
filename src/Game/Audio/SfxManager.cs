// Translated from Sound.pas.
using Aow.Rtl;
using Aow.Rtl.Host;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

// _searchPath, _sfx. ExecutePathSearch lists every wave file under a directory (with its
// subdirectories) relative to the search path
public partial class SfxManager : Component
{
    private string _searchPath = "";
    private readonly SfxList _sfx;

    // Sound.pas:407
    public SfxManager(Component owner) : base(owner) => _sfx = new SfxList();

    // Sound.pas:413
    public override void Dispose()
    {
        Clear();
        _sfx?.Dispose();
        base.Dispose();
    }

    // Sound.pas:420
    public void Clear() => _sfx.Clear();

    // Sound.pas:425
    public void ExecutePathSearch(string path)
    {
        string oldDirectory;
        void Search(string directory)
        {
            StringList directories;
            foreach (RtlFileEntry entry in Files.FindEntries(_searchPath + directory + "*." + s_sfxExtension, attributes: 63 & ~16))
            {
                _ = _sfx.Add(UpperCase(directory + entry.Name));
            }

            directories = new StringList();
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

        _sfx.Clear();
        _searchPath = UpperCase(path);
        if (_searchPath[^1] != 92)
        {
            _searchPath += "\\";
        }

        oldDirectory = GetCurrentDir();
        _ = SetCurrentDir(path);
        Search("");
        _ = SetCurrentDir(oldDirectory);
        _searchPath = UpperCase(ExtractRelativePath(AudioHost.BaseSoundEnginePath, _searchPath));
    }

    [Published]
    public string SearchPath { get => _searchPath; set => _searchPath = value; }

    [Published]
    public SfxList Sfx => _sfx;

    private static readonly string s_sfxExtension = "WAV";

}
