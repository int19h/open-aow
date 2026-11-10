// Translated from Sound.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

// FSearchPath, FSFX. ExecutePathSearch lists every wave file under a directory (with its
// subdirectories) relative to the search path
public partial class SFXManager : Component
{
    internal string FSearchPath = "";
    internal SFXList FSFX;

    // Sound.pas:407
    public SFXManager(Component owner) : base(owner)
    {
        FSFX = new SFXList();
    }

    // Sound.pas:413
    public override void Dispose()
    {
        Clear();
        FSFX?.Dispose();
        base.Dispose();
    }

    // Sound.pas:420
    public void Clear()
    {
        FSFX.Clear();
    }

    // Sound.pas:425
    public void ExecutePathSearch(string Path)
    {
        string OldDir;
        void Search(string Dir)
        {
            SearchRec SR = default;
            StringList Dirs;
            bool Found;
            Found = FindFirst(FSearchPath + Dir + "*." + SFXExtension, 63 & ~16, ref SR) == 0;
            while (Found)
            {
                FSFX.Add(UpperCase(Dir + SR.Name));
                Found = FindNext(ref SR) == 0;
            }

            FindClose(ref SR);
            Dirs = new StringList();
            Found = FindFirst(Dir + "*.*", 16, ref SR) == 0;
            while (Found)
            {
                if ((SR.Attr & 16) != 0 && SR.Name != "." && SR.Name != "..")
                {
                    Dirs.Add(Dir + UpperCase(SR.Name) + "\\");
                }

                Found = FindNext(ref SR) == 0;
            }

            FindClose(ref SR);
            for (int I = 0, last = Dirs.Count - 1; I <= last; I++)
            {
                Search(Dirs[I]);
            }

            Dirs?.Dispose();
        }

        FSFX.Clear();
        FSearchPath = UpperCase(Path);
        if (FSearchPath[FSearchPath.Length - 1] != 92)
        {
            FSearchPath = FSearchPath + "\\";
        }

        OldDir = GetCurrentDir();
        SetCurrentDir(Path);
        Search("");
        SetCurrentDir(OldDir);
        FSearchPath = UpperCase(ExtractRelativePath(AudioHost.BaseSoundEnginePath, FSearchPath));
    }

    [Published]
    public string SearchPath { get => FSearchPath; set => FSearchPath = value; }

    [Published]
    public SFXList SFX { get => FSFX; }

    private static readonly string SFXExtension = "WAV";

}
