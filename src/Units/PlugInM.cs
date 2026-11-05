// Translated from PlugInM.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;

namespace Aow.Units.PlugInM;

public delegate void TLoadPlugInEvent(TObject Sender, string FileName);
// FPlugIns (file names, Objects the package handles), FPlugInsLoaded, FPlugInDirectory,
// FPlugInExtension ('dpl'), FOnLoadPlugIn
public partial class TPlugInManager : TComponent
{
    internal TStringList FPlugIns;
    internal bool FPlugInsLoaded;
    internal string FPlugInDirectory = "";
    internal string FPlugInExtension = "";
    internal TLoadPlugInEvent FOnLoadPlugIn;
    // PlugInM.pas:60
    internal void ExecutePlugInSearch(string Directory)
    {
        string Dir;
        void SearchDir(string Dir)
        {
            TSearchRec SR = default;
            TStringList Dirs;
            bool Found;
            Found = FindFirst(Dir + "*." + FPlugInExtension, 63 - 16, ref SR) == 0;
            while (Found)
            {
                FPlugIns.Add(Dir + SR.Name);
                Found = FindNext(ref SR) == 0;
            }

            FindClose(ref SR);
            Dirs = new TStringList();
            Found = FindFirst(Dir + "*.*", 16, ref SR) == 0;
            while (Found)
            {
                if ((SR.Attr & 16) != 0 && SR.Name != "." && SR.Name != "..")
                {
                    Dirs.Add(Dir + SR.Name + "\\");
                }

                Found = FindNext(ref SR) == 0;
            }

            FindClose(ref SR);
            for (int I = 0, last = Dirs.Count - 1; I <= last; I++)
            {
                SearchDir(Dirs[I]);
            }

            Dirs.Free();
        }

        Dir = Directory;
        SearchDir(Dir);
    }

    // PlugInM.pas:46
    public TPlugInManager(TComponent owner) : base(owner)
    {
        FPlugIns = new TStringList();
        FPlugInExtension = "dpl";
    }

    // PlugInM.pas:53
    public override void Destroy()
    {
        ReleasePlugIns();
        FPlugIns.Free();
        base.Destroy();
    }

    // PlugInM.pas:96
    public void LoadPlugIns(string Directory)
    {
        ReleasePlugIns();
        FPlugInDirectory = ExpandFileName(Directory);
        if (!IsEmpty(FPlugInDirectory) && FPlugInDirectory[FPlugInDirectory.Length - 1] != 92)
        {
            FPlugInDirectory = FPlugInDirectory + "\\";
        }

        ExecutePlugInSearch(FPlugInDirectory);
        FPlugInsLoaded = false;
        var stringList = FPlugIns;
        for (int I = 0, last = stringList.Count - 1; I <= last; I++)
        {
            try
            {
                if (FOnLoadPlugIn != null)
                {
                    var stringValue = stringList[I];
                    FOnLoadPlugIn(this, stringValue);
                }

                /* TODO: Implement the native Delphi package loader. */
                throw new NotImplementedException();
                FPlugInsLoaded = true;
            }
            catch
            {
                ShowMessage("Unable to load PlugIn: " + stringList[I]);
                stringList.Objects[I] = null;
            }
        }
    }

    // PlugInM.pas:120
    public void ReleasePlugIns()
    {
        for (int I = this.FPlugIns.Count - 1; I >= 0; I--)
        {
            try
            {
                if (this.FPlugIns.Objects[I] != null)
                {
                    /* TODO: Implement the native Delphi package loader. */
                    throw new NotImplementedException();
                    this.FPlugIns.Objects[I] = null;
                }
            }
            catch
            {
                ShowMessage("Could not release PlugIn: " + this.FPlugIns[I]);
                this.FPlugIns.Objects[I] = null;
            }

            this.FPlugIns.Delete(I);
        }
    }

    public TStringList PlugIns { get => FPlugIns; }

    [Published]
    public bool PlugInsLoaded { get => FPlugInsLoaded; }

    [Published]
    public string PlugInDirectory { get => FPlugInDirectory; }

    [Published]
    public string PlugInExtension { get => FPlugInExtension; set => FPlugInExtension = value; }

    [Published]
    public TLoadPlugInEvent OnLoadPlugIn { get => FOnLoadPlugIn; set => FOnLoadPlugIn = value; }
}

public static partial class PlugInM
{
    // PlugInM.pas:141
    public static void Register()
    {
        RegisterComponents("Engine", [typeof(TPlugInManager)]);
    }
}
