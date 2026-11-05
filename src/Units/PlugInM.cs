// Translated from PlugInM.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;

namespace Aow.Units.PlugInM
{
    public delegate void TLoadPlugInEvent(TObject Sender, AnsiString FileName);
    // FPlugIns (file names, Objects the package handles), FPlugInsLoaded, FPlugInDirectory,
    // FPlugInExtension ('dpl'), FOnLoadPlugIn
    public partial class TPlugInManager : TComponent
    {
        internal TStringList FPlugIns;
        internal bool FPlugInsLoaded;
        internal AnsiString FPlugInDirectory;
        internal AnsiString FPlugInExtension;
        internal TLoadPlugInEvent FOnLoadPlugIn;
        // PlugInM.pas:60
        internal void ExecutePlugInSearch(AnsiString Directory)
        {
            AnsiString Dir;
            void SearchDir(AnsiString Dir)
            {
                TSearchRec SR = default;
                TStringList Dirs;
                bool Found;
                Found = FindFirst(Dir + (AnsiString)"*." + FPlugInExtension, 63 - 16, ref SR) == 0;
                while (Found)
                {
                    FPlugIns.Add(Dir + SR.Name);
                    Found = FindNext(ref SR) == 0;
                }

                FindClose(ref SR);
                Dirs = new TStringList();
                Found = FindFirst(Dir + (AnsiString)"*.*", 16, ref SR) == 0;
                while (Found)
                {
                    if ((SR.Attr & 16) != 0 && SR.Name != AnsiString.FromBytes(46) && SR.Name != (AnsiString)"..")
                    {
                        Dirs.Add(Dir + SR.Name + AnsiString.FromBytes(92));
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
            FPlugInExtension = (AnsiString)"dpl";
        }

        // PlugInM.pas:53
        public override void Destroy()
        {
            ReleasePlugIns();
            FPlugIns.Free();
            base.Destroy();
        }

        // PlugInM.pas:96
        public void LoadPlugIns(AnsiString Directory)
        {
            ReleasePlugIns();
            FPlugInDirectory = ExpandFileName(Directory);
            if (FPlugInDirectory != (AnsiString)"" && FPlugInDirectory[FPlugInDirectory.Length] != 92)
            {
                FPlugInDirectory = FPlugInDirectory + AnsiString.FromBytes(92);
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
                catch (System.Exception)
                {
                    ShowMessage((AnsiString)"Unable to load PlugIn: " + stringList[I]);
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
                catch (System.Exception)
                {
                    ShowMessage((AnsiString)"Could not release PlugIn: " + this.FPlugIns[I]);
                    this.FPlugIns.Objects[I] = null;
                }

                this.FPlugIns.Delete(I);
            }
        }

        public TStringList PlugIns { get => FPlugIns; }

        [Published]
        public bool PlugInsLoaded { get => FPlugInsLoaded; }

        [Published]
        public AnsiString PlugInDirectory { get => FPlugInDirectory; }

        [Published]
        public AnsiString PlugInExtension { get => FPlugInExtension; set => FPlugInExtension = value; }

        [Published]
        public TLoadPlugInEvent OnLoadPlugIn { get => FOnLoadPlugIn; set => FOnLoadPlugIn = value; }
    }

    public static partial class PlugInM
    {
        // PlugInM.pas:141
        public static void Register()
        {
            RegisterComponents((AnsiString)"Engine", [typeof(TPlugInManager)]);
        }
    }
}
