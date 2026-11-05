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
            AnsiString Dir = default;
            void SearchDir(AnsiString Dir)
            {
                TSearchRec SR = default;
                TStringList Dirs = default;
                int I = default;
                bool Found = default;
                var call__1 = FindFirst(Dir + (AnsiString)"*." + FPlugInExtension, 63 - 16, ref SR);
                Found = call__1 == 0;
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
                {
                    I = 0;
                    int end__2 = Dirs.Count - 1;
                    for (; I <= end__2; I += 1)
                    {
                        var call__3 = Dirs[I];
                        SearchDir(call__3);
                    }
                }

                Dirs.Free();
            }

            Dir = Directory;
            SearchDir(Dir);
        }

        // PlugInM.pas:46
        public TPlugInManager(TComponent owner) : base(owner)
        {
            var call__4 = new TStringList();
            FPlugIns = call__4;
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
            int I = default;
            ReleasePlugIns();
            var call__5 = ExpandFileName(Directory);
            FPlugInDirectory = call__5;
            if (FPlugInDirectory != (AnsiString)"" && FPlugInDirectory[FPlugInDirectory.Length] != 92)
            {
                FPlugInDirectory = FPlugInDirectory + AnsiString.FromBytes(92);
            }

            ExecutePlugInSearch(FPlugInDirectory);
            FPlugInsLoaded = false;
            var stringList = FPlugIns;
            {
                I = 0;
                int end__7 = stringList.Count - 1;
                for (; I <= end__7; I += 1)
                {
                    try
                    {
                        if (FOnLoadPlugIn != null)
                        {
                            var call__9 = stringList[I];
                            FOnLoadPlugIn(this, call__9);
                        }

                        /* TODO: Implement the native Delphi package loader. */
                        throw new NotImplementedException();
                        FPlugInsLoaded = true;
                    }
                    catch (System.Exception)
                    {
                        var call__8 = stringList[I];
                        ShowMessage((AnsiString)"Unable to load PlugIn: " + call__8);
                        stringList.Objects[I] = null;
                    }
                }
            }
        }

        // PlugInM.pas:120
        public void ReleasePlugIns()
        {
            int I = default;
            {
                I = this.FPlugIns.Count - 1;
                byte end__13 = 0;
                for (; I >= end__13; I -= 1)
                {
                    try
                    {
                        var call__15 = this.FPlugIns.Objects[I];
                        if (call__15 != null)
                        {
                            /* TODO: Implement the native Delphi package loader. */
                            throw new NotImplementedException();
                            this.FPlugIns.Objects[I] = null;
                        }
                    }
                    catch (System.Exception)
                    {
                        var call__14 = this.FPlugIns[I];
                        ShowMessage((AnsiString)"Could not release PlugIn: " + call__14);
                        this.FPlugIns.Objects[I] = null;
                    }

                    this.FPlugIns.Delete(I);
                }
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
