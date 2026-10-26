// Generated from PlugInM.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Aow.Units.PlugInM
{
    public delegate void TLoadPlugInEvent(Aow.Rtl.TObject Sender, Aow.Rtl.AnsiString FileName);
    // { PlugInM.TPlugInManager (instance size 60): +$24 FPlugIns (file names, Objects the package
//     handles), +$28 FPlugInsLoaded, +$2C FPlugInDirectory, +$30 FPlugInExtension ('dpl'),
//     +$34 FOnLoadPlugIn. }
    public partial class TPlugInManager : Aow.Rtl.TComponent
    {
        internal Aow.Rtl.TStringList FPlugIns;
        internal bool FPlugInsLoaded;
        internal Aow.Rtl.AnsiString FPlugInDirectory;
        internal Aow.Rtl.AnsiString FPlugInExtension;
        internal Aow.Units.PlugInM.TLoadPlugInEvent FOnLoadPlugIn;
        // PlugInM.pas:60
        internal void ExecutePlugInSearch(Aow.Rtl.AnsiString Directory)
        {
            Aow.Rtl.AnsiString Dir = default;
            void SearchDir(Aow.Rtl.AnsiString Dir)
            {
                Aow.Rtl.TSearchRec SR = default;
                Aow.Rtl.TStringList Dirs = default;
                int I = default;
                bool Found = default;
                var call__1 = Aow.Rtl.Rtl.FindFirst((Dir + ((AnsiString)"*.")) + FPlugInExtension, 63 - 16, ref SR);
                Found = call__1 == 0;
                while (Found)
                {
                    FPlugIns.Add(Dir + SR.Name);
                    Found = Aow.Rtl.Rtl.FindNext(ref SR) == 0;
                }

                Aow.Rtl.Rtl.FindClose(ref SR);
                Dirs = new Aow.Rtl.TStringList();
                Found = Aow.Rtl.Rtl.FindFirst(Dir + ((AnsiString)"*.*"), 16, ref SR) == 0;
                while (Found)
                {
                    if ((((SR.Attr & 16) != 0) && (SR.Name != AnsiString.FromBytes(46))) && (SR.Name != ((AnsiString)"..")))
                    {
                        Dirs.Add((Dir + SR.Name) + AnsiString.FromBytes(92));
                    }

                    Found = Aow.Rtl.Rtl.FindNext(ref SR) == 0;
                }

                Aow.Rtl.Rtl.FindClose(ref SR);
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
        // { TPlugInManager }
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
            var call__4 = new Aow.Rtl.TStringList();
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
        public void LoadPlugIns(Aow.Rtl.AnsiString Directory)
        {
            int I = default;
            ReleasePlugIns();
            var call__5 = Aow.Rtl.Rtl.ExpandFileName(Directory);
            FPlugInDirectory = call__5;
            if ((FPlugInDirectory != ((AnsiString)"")) && (FPlugInDirectory[FPlugInDirectory.Length] != 92))
            {
                FPlugInDirectory = FPlugInDirectory + AnsiString.FromBytes(92);
            }

            ExecutePlugInSearch(FPlugInDirectory);
            FPlugInsLoaded = false;
            {
                var w__6 = FPlugIns;
                {
                    I = 0;
                    int end__7 = w__6.Count - 1;
                    for (; I <= end__7; I += 1)
                    {
                        try
                        {
                            if (FOnLoadPlugIn != null)
                            {
                                var call__9 = w__6[I];
                                FOnLoadPlugIn(this, call__9);
                            }

                            /* TODO(T-5.4) Native Delphi packages have no managed loader */
                            throw new System.NotImplementedException();
                            FPlugInsLoaded = true;
                        }
                        catch (System.Exception)
                        {
                            var call__8 = w__6[I];
                            Aow.Rtl.Rtl.ShowMessage(((AnsiString)"Unable to load PlugIn: ") + call__8);
                            w__6.Objects[I] = null;
                        }
                    }
                }
            }
        }

        // PlugInM.pas:120
        public void ReleasePlugIns()
        {
            int I = default;
            {
                var w__12 = FPlugIns;
                {
                    I = w__12.Count - 1;
                    byte end__13 = 0;
                    for (; I >= end__13; I -= 1)
                    {
                        try
                        {
                            var call__15 = w__12.Objects[I];
                            if (call__15 != null)
                            {
                                /* TODO(T-5.4) Native Delphi packages have no managed loader */
                                throw new System.NotImplementedException();
                                w__12.Objects[I] = null;
                            }
                        }
                        catch (System.Exception)
                        {
                            var call__14 = w__12[I];
                            Aow.Rtl.Rtl.ShowMessage(((AnsiString)"Could not release PlugIn: ") + call__14);
                            w__12.Objects[I] = null;
                        }

                        w__12.Delete(I);
                    }
                }
            }
        }

        public Aow.Rtl.TStringList PlugIns { get => FPlugIns; }

        [Published]
        public bool PlugInsLoaded { get => FPlugInsLoaded; }

        [Published]
        public Aow.Rtl.AnsiString PlugInDirectory { get => FPlugInDirectory; }

        [Published]
        public Aow.Rtl.AnsiString PlugInExtension { get => FPlugInExtension; set => FPlugInExtension = value; }

        [Published]
        public Aow.Units.PlugInM.TLoadPlugInEvent OnLoadPlugIn { get => FOnLoadPlugIn; set => FOnLoadPlugIn = value; }

        // PlugInM.pas:28
        public static Aow.Units.PlugInM.TPlugInManager Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.PlugInM.TPlugInManager o = new Aow.Units.PlugInM.TPlugInManager();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public static partial class PlugInM
    {
        // PlugInM.pas:141
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"Engine", [typeof(Aow.Units.PlugInM.TPlugInManager)]);
        }
    }
}
