// Translated from Sound.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

// _searchPath, _sfx. ExecutePathSearch lists every wave file under a directory (with its
// subdirectories) relative to the search path
public partial class SfxManager : Component
{
    private string _searchPath = "";
    private SfxList _sfx;

    // Sound.pas:407
    public SfxManager(Component owner) : base(owner)
    {
        _sfx = new SfxList();
    }

    // Sound.pas:413
    public override void Dispose()
    {
        Clear();
        _sfx?.Dispose();
        base.Dispose();
    }

    // Sound.pas:420
    public void Clear()
    {
        _sfx.Clear();
    }

    // Sound.pas:425
    public void ExecutePathSearch(string path)
    {
        string oldDirectory;
        void Search(string directory)
        {
            SearchRec sr = default;
            StringList directories;
            bool found;
            found = FindFirst(_searchPath + directory + "*." + s_sfxExtension, 63 & ~16, ref sr) == 0;
            while (found)
            {
                _sfx.Add(UpperCase(directory + sr.Name));
                found = FindNext(ref sr) == 0;
            }

            FindClose(ref sr);
            directories = new StringList();
            found = FindFirst(directory + "*.*", 16, ref sr) == 0;
            while (found)
            {
                if ((sr.Attr & 16) != 0 && sr.Name != "." && sr.Name != "..")
                {
                    directories.Add(directory + UpperCase(sr.Name) + "\\");
                }

                found = FindNext(ref sr) == 0;
            }

            FindClose(ref sr);
            for (int I = 0, last = directories.Count - 1; I <= last; I++)
            {
                Search(directories[I]);
            }

            directories?.Dispose();
        }

        _sfx.Clear();
        _searchPath = UpperCase(path);
        if (_searchPath[_searchPath.Length - 1] != 92)
        {
            _searchPath = _searchPath + "\\";
        }

        oldDirectory = GetCurrentDir();
        SetCurrentDir(path);
        Search("");
        SetCurrentDir(oldDirectory);
        _searchPath = UpperCase(ExtractRelativePath(AudioHost.BaseSoundEnginePath, _searchPath));
    }

    [Published]
    public string SearchPath { get => _searchPath; set => _searchPath = value; }

    [Published]
    public SfxList Sfx { get => _sfx; }

    private static readonly string s_sfxExtension = "WAV";

}
