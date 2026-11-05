// Translated from AoWFileDialog.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using Aow.Units.AoWComp;
using Aow.Units.AoWButton;
using Aow.Units.AoWComboBox;
using Aow.Units.AoWEdit;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using Aow.Units.AoWWorldMap;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.E1GameFlow.E1GameFlow;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Platform;

namespace Aow.Units.AoWFileDialog
{
    // directory, last filename, save event, done callback, dialog mode
    public partial class TFileDlgWindow : TDataModule
    {
        public TFileDlgWindow(TComponent owner) : base(owner)
        { }

        public TAOWWindow FileDialog;
        public TAOWButton CancelBut;
        public TAoWFrame BCeltL;
        public TAOWButton Openbut;
        public TAOWLabel Title;
        public TAOWListBox FileLB;
        public TAOWEdit FileName;
        public TAOWLabel ScenLbl;
        public TAOWComboBox DriveCmb;
        public TAOWLabel DriveL;
        public TAOWButton Close;
        public TAOWButton UpDirBtn;
        public TAOWLabel PathL;
        public TAOWImage TitleIcon;
        public TAOWVScrollBar DriveSB;
        public TAOWImage CeltM;
        public TAOWImage LPlayersI;
        public TAOWPanel LSavedInfo;
        public TAOWImage LSizeI;
        public TAOWImage LTownsI;
        public TAOWImage LTurnsI;
        public TAOWLabel LPlayers;
        public TAOWLabel LSize;
        public TAOWLabel LTowns;
        public TAOWLabel LTurns;
        public TAOWImage CeltL;
        public TAOWImage CeltR;
        public TAoWFrame TCeltL;
        public TAOWImage TCelt;
        public TAOWImage SPlayersI;
        public TAOWPanel NewInfo;
        public TAOWImage SSizeI;
        public TAOWImage STownsI;
        public TAOWLabel SPlayers;
        public TAOWLabel SSize;
        public TAOWLabel STowns;
        public TAOWPanel NewPnl;
        public TAOWListBox SaveLB;
        public TAOWEdit SFileName;
        public TAOWLabel SavedInfo;
        public TAOWPanel SavePnl;
        public TAOWListBox DateLB;
        public TAOWButton SaveBtn;
        public TAOWButton DateBtn;
        public TAOWVScrollBar NSBar;
        public TAOWPanel DriveFr;
        public TAOWPanel TopPnl;
        public TAOWPanel PathFr;
        public TAOWEdit PathEdit;
        public TAOWPanel NewLBFr;
        public TAOWPanel NewInfo1;
        public TAOWPanel NewInfo2;
        public TAOWPanel NewInfo3;
        public TAOWPanel SaveLBFr;
        public TAOWVScrollBar SSBar;
        public TAOWPanel SavedInfo1;
        public TAOWPanel SavedInfo2;
        public TAOWPanel SavedInfo3;
        public TAOWPanel SavedInfo4;
        public TIvTranslator Translator;
        public AnsiString FDirectory;
        public AnsiString FLastFileName;
        public TSaveGameEventLog FSaveEvent;
        public TExecuteDoneEvent FDone;
        // is the data pointer of FDone
        public int FMode;
        // AoWFileDialog.pas:175
        // 0 load, 1 save, 2 select map
        public void FinishDialog()
        {
            TExecuteDoneEvent Done = default;
            TSaveGameEventLog Event = default;
            Done = FDone;
            Event = FSaveEvent;
            FDone = null;
            FSaveEvent = null;
            if (Done != null)
            {
                Done(Event);
            }

            if (Event != null)
            {
                Event.Release();
            }
        }

        // AoWFileDialog.pas:132
        public void ExitForLoad(TExitGameMode Mode, bool Success)
        {
            if (Success)
            {
                GameHost.DeferredSetupAction = 2;
            }
        }

        // AoWFileDialog.pas:137
        public void LoadSelectedGame(AnsiString Name)
        {
            TAoWHSMap Map = default;
            AoWEngine.CloseCampaign();
            AoWEngine.CloseHSM(null);
            if (LowerCase(ExtractFileExt(Name)) == (AnsiString)".acg")
            {
                if (!AoWEngine.FDemo)
                {
                    LoadCampaign(General, Name);
                    if (AoWHSMap != null)
                    {
                        StartGame(General);
                    }
                }
            }
            else
            {
                AoWEngine.CloseCampaign();
                var call__1 = AoWEngine.CreatePreviewHSM(Name);
                Map = (TAoWHSMap)call__1;
                if (Map != null)
                {
                    try
                    {
                        if (Map.FConnectionType != TPlayerConnectionType.pcNetwork)
                        {
                            LoadGame(General, Name);
                            if (AoWHSMap != null)
                            {
                                StartGame(General);
                            }
                        }
                        else
                        {
                            E2Globals.E2Globals.TitleScene.HideScene();
                            General.HideWindows();
                            TBWindow.TaskWindow.Visible = false;
                            var call__2 = ExtractRelativePath(AoWEngine.StartupDirectory, Name);
                            MultiWindow.Execute(call__2);
                        }
                    }
                    finally
                    {
                        Map.Free();
                    }
                }
            }
        }

        // AoWFileDialog.pas:188
        public void CloseClick()
        {
            General.PlayButtonSound(0);
            if (FSaveEvent != null)
            {
                if (!FSaveEvent.FMode.Contains(TSaveGameEventLogModeElement.sgmDisableCancel))
                {
                    FileDialog.Visible = false;
                }
            }
            else
            {
                FileDialog.Visible = false;
            }
        }

        // AoWFileDialog.pas:233
        public void FindFiles()
        {
            bool IncludeFile = default;
            AnsiString Extension = default;
            AnsiString Directory = default;
            int I = default;
            TSearchRec Search = default;
            TFileTime Stamp = default;
            {
                I = 0;
                int end__3 = DateLB.Items.Count - 1;
                for (; I <= end__3; I += 1)
                {
                    var call__4 = DateLB.Items.Objects[I];
                    if (call__4 != null)
                    {
                        var call__5 = DateLB.Items.Objects[I];
                        call__5.Free();
                    }
                }
            }

            AoWFileDialog.Directories.Clear();
            AoWFileDialog.Files.Clear();
            SPlayers.Text = (AnsiString)"";
            SSize.Text = (AnsiString)"";
            STowns.Text = (AnsiString)"";
            LPlayers.Text = (AnsiString)"";
            LSize.Text = (AnsiString)"";
            LTowns.Text = (AnsiString)"";
            LTurns.Text = (AnsiString)"";
            Directory = GetCurrentDir();
            if (Copy(Directory, Directory.Length, 1) == AnsiString.FromBytes(92))
            {
                Directory = Copy(Directory, 1, Directory.Length - 1);
            }

            if (FindFirst(Directory + (AnsiString)"\\**", 16, ref Search) == 0)
            {
                do
                {
                    if (Search.Name != AnsiString.FromBytes(46) && Search.Name != (AnsiString)"..")
                    {
                        if ((Search.Attr & 16) == 16)
                        {
                            AoWFileDialog.Directories.Add(Search.Name);
                        }
                        else
                        {
                            IncludeFile = false;
                            var call__6 = ExtractFileExt(Search.Name);
                            Extension = LowerCase(call__6);
                            IncludeFile = IncludeFile || (FMode == 1 && AoWEngine.FCampaign != null && Extension == (AnsiString)".acg");
                            IncludeFile = IncludeFile || (FMode == 1 && AoWEngine.FCampaign == null && Extension == (AnsiString)".asg");
                            IncludeFile = IncludeFile || (FMode == 0 && (Extension == (AnsiString)".asg" || Extension == (AnsiString)".acg"));
                            // { Original accepts .asg in every mode, including map selection. }
                            IncludeFile = IncludeFile || (FMode == 2 && Extension == (AnsiString)".hsm") || Extension == (AnsiString)".asg";
                            if (IncludeFile)
                            {
                                Stamp = new TFileTime();
                                var call__7 = FileDateToDateTime(Search.Time);
                                Stamp.FTime = call__7;
                                AoWFileDialog.Files.AddObject(Search.Name, Stamp);
                            }
                        }
                    }
                }
                while (!(FindNext(ref Search) != 0));
            }

            FindClose(ref Search);
            FileLB.Selected = -1;
            SaveLB.Selected = -1;
            DateLB.Selected = -1;
            FileLB.ListOffset = 0;
            SaveLB.ListOffset = 0;
            DateLB.ListOffset = 0;
            FileLB.Items.Clear();
            SaveLB.Items.Clear();
            DateLB.Items.Clear();
            AoWFileDialog.Directories.Sort();
            AoWFileDialog.Files.Sort();
            FileLB.Items.AddStrings(AoWFileDialog.Directories);
            FileLB.Items.AddStrings(AoWFileDialog.Files);
            SaveLB.Items.AddStrings(FileLB.Items);
            PathEdit.Text = Directory + AnsiString.FromBytes(92);
            {
                I = 0;
                int end__8 = AoWFileDialog.Directories.Count - 1;
                for (; I <= end__8; I += 1)
                {
                    DateLB.Items.Add((AnsiString)"");
                }
            }

            {
                I = 0;
                int end__9 = AoWFileDialog.Files.Count - 1;
                for (; I <= end__9; I += 1)
                {
                    var call__10 = AoWFileDialog.Files.Objects[I];
                    var call__11 = DateTimeToStr(((TFileTime)call__10).FTime);
                    var call__12 = AoWFileDialog.Files.Objects[I];
                    DateLB.Items.AddObject(call__11, call__12);
                }
            }
        }

        // AoWFileDialog.pas:296
        public void FileDlgWindowCreate()
        {
            var call__13 = new TStringList();
            AoWFileDialog.Drives = call__13;
            var call__14 = new TStringList();
            AoWFileDialog.Directories = call__14;
            var call__15 = new TStringList();
            AoWFileDialog.Files = call__15;
            FDirectory = AoWEngine.StartupDirectory + (AnsiString)"Save\\";
            FLastFileName = (AnsiString)"";
        }

        // AoWFileDialog.pas:305
        public void FileDlgWindowDestroy()
        {
            int I = default;
            {
                I = 0;
                int end__16 = DateLB.Items.Count - 1;
                for (; I <= end__16; I += 1)
                {
                    var call__17 = DateLB.Items.Objects[I];
                    if (call__17 != null)
                    {
                        var call__18 = DateLB.Items.Objects[I];
                        call__18.Free();
                    }
                }
            }

            AoWFileDialog.Drives.Free();
            AoWFileDialog.Directories.Free();
            AoWFileDialog.Files.Free();
        }

        // AoWFileDialog.pas:316
        public void OpenbutClick(TObject Sender)
        {
            AnsiString Path = default;
            AnsiString Directory = default;
            General.PlayButtonSound(0);
            var call__19 = AoWFileDialog.Directories.Count;
            if (FileLB.Selected < call__19 && FileLB.Selected > -1)
            {
                Directory = GetCurrentDir();
                if (Copy(Directory, Directory.Length, 1) == AnsiString.FromBytes(92))
                {
                    Directory = Copy(Directory, 1, Directory.Length - 1);
                }

                var call__20 = FileLB.Items[FileLB.Selected];
                var call__21 = SetCurrentDir(Directory + AnsiString.FromBytes(92) + call__20);
                if (call__21)
                {
                    FindFiles();
                }

                FileLB.Selected = -1;
            }
            else
            {
                Path = GetCurrentDir();
                if (Path.Length > 0 && Path[Path.Length] != 92)
                {
                    Path = Path + AnsiString.FromBytes(92);
                }

                switch (FMode)
                {
                    case 0:
                        bool bool__22 = SFileName.Text != (AnsiString)"";
                        if (bool__22)
                        {
                            var call__23 = FileExists(SFileName.Text);
                            bool__22 = call__23;
                        }

                        if (bool__22)
                        {
                            if (AoWHSMap != null)
                            {
                                if (AoWHSMap.FCombatMap != null)
                                {
                                    General.ReturnToTitle();
                                }
                                else
                                {
                                    if (AoWEngine.FCampaign != null)
                                    {
                                        if (CompareText(AoWEngine.FCampaign.FFileName, FDirectory + SFileName.Text) == 0)
                                        {
                                            AoWHSMap.ExitGame(TExitGameMode.egmReload, ExitForLoad);
                                        }
                                        else
                                        {
                                            AoWHSMap.ExitGame(TExitGameMode.egmLoad, ExitForLoad);
                                        }
                                    }
                                    else
                                    {
                                        if (CompareText(AoWHSMap.FileName, FDirectory + SFileName.Text) == 0)
                                        {
                                            AoWHSMap.ExitGame(TExitGameMode.egmReload, ExitForLoad);
                                        }
                                        else
                                        {
                                            AoWHSMap.ExitGame(TExitGameMode.egmLoad, ExitForLoad);
                                        }
                                    }
                                }
                            }

                            if (AoWHSMap == null || AoWHSMap.FExitPending)
                            {
                                FileDialog.Visible = false;
                                LoadSelectedGame(Path + SFileName.Text);
                            }

                            FLastFileName = Path + SFileName.Text;
                            FDirectory = Path;
                        }

                        break;
                    case 1:
                        if (SFileName.Text != (AnsiString)"")
                        {
                            FDirectory = Path;
                            if (Pos(AnsiString.FromBytes(46), SFileName.Text) != 0)
                            {
                                SFileName.Text = Copy(SFileName.Text, 1, Pos(AnsiString.FromBytes(46), SFileName.Text) - 1);
                            }

                            if (AoWEngine.FCampaign != null)
                            {
                                FileDialog.Visible = false;
                                SFileName.Text = SFileName.Text + (AnsiString)".acg";
                                SaveCampaign(General, SFileName.Text);
                            }
                            else
                            {
                                if (FSaveEvent != null)
                                {
                                    FSaveEvent.FFileName = SFileName.Text + (AnsiString)".asg";
                                    FileDialog.Visible = false;
                                }
                                else
                                {
                                    FileDialog.Visible = false;
                                    SFileName.Text = SFileName.Text + (AnsiString)".asg";
                                    SaveGame(General, SFileName.Text);
                                }
                            }

                            FLastFileName = SFileName.Text;
                            FDirectory = Path;
                        }

                        break;
                    case 2:
                        bool bool__24 = FileName.Text != (AnsiString)"";
                        if (bool__24)
                        {
                            var call__25 = FileExists(FileName.Text);
                            bool__24 = call__25;
                        }

                        if (bool__24)
                        {
                            PickScenarioMap(ScenSetup, FileName.Text);
                            FLastFileName = FileName.Text;
                        }

                        break;
                }
            }
        }

        // AoWFileDialog.pas:394
        public void UpDirBtnClick()
        {
            General.PlayButtonSound(0);
            if (SetCurrentDir((AnsiString)".."))
            {
                FindFiles();
            }
        }

        // AoWFileDialog.pas:400
        public void DriveCmbChange()
        {
            var call__26 = DriveCmb.Items[DriveCmb.Selected];
            var call__27 = DriveCmb.Items[DriveCmb.Selected];
            var call__28 = SetCurrentDir(Copy(call__27, call__26.Length - 3, 3));
            if (call__28)
            {
                FindFiles();
            }
            else
            {
                FindDrives();
            }
        }

        // AoWFileDialog.pas:406
        public void FileLBChange(TAOWListBox Sender)
        {
            TAoWHSMap Map = default;
            TAOWListBox List = default;
            List = Sender;
            FileLB.Selected = List.Selected;
            SaveLB.Selected = List.Selected;
            DateLB.Selected = List.Selected;
            var call__29 = AoWFileDialog.Directories.Count;
            if (call__29 - 1 < this.FileLB.Selected)
            {
                var call__30 = FileLB.Items[FileLB.Selected];
                SFileName.Text = call__30;
                var call__31 = ExtractFileExt(SFileName.Text);
                if (LowerCase(call__31) != (AnsiString)".acg")
                {
                    var call__32 = AoWEngine.CreatePreviewHSM(SFileName.Text);
                    Map = (TAoWHSMap)call__32;
                    if (Map != null)
                    {
                        var call__33 = Map.FPlayers.GetCount();
                        LPlayers.Text = IntToStr(call__33 - 1);
                        switch (Map.MapContainer.Width)
                        {
                            case 48:
                                var call__34 = TranslateRStr(AnsiString.FromBytes(SmallMapAbrRStr));
                                LSize.Text = call__34;
                                break;
                            case 64:
                                var call__35 = TranslateRStr(AnsiString.FromBytes(MediumMapAbrRStr));
                                LSize.Text = call__35;
                                break;
                            case 96:
                                var call__36 = TranslateRStr(AnsiString.FromBytes(LargeMapAbrRStr));
                                LSize.Text = call__36;
                                break;
                            case 127:
                            case 128:
                                var call__37 = TranslateRStr(ExtraLargeMapAbrRStr);
                                LSize.Text = call__37;
                                break;
                            default:
                                LSize.Text = AnsiString.FromBytes(63);
                                break;
                        }

                        LTowns.Text = IntToStr(Map.FStructureControl.FCityCount);
                        LTurns.Text = IntToStr(Map.FDay);
                        Map.Free();
                    }
                    else
                    {
                        LPlayers.Text = (AnsiString)"";
                        LSize.Text = (AnsiString)"";
                        LTowns.Text = (AnsiString)"";
                        LTurns.Text = (AnsiString)"";
                    }
                }
                else
                {
                    LPlayers.Text = (AnsiString)"";
                    LSize.Text = (AnsiString)"";
                    LTowns.Text = (AnsiString)"";
                    LTurns.Text = (AnsiString)"";
                }
            }
            else
            {
                LPlayers.Text = (AnsiString)"";
                LSize.Text = (AnsiString)"";
                LTowns.Text = (AnsiString)"";
                LTurns.Text = (AnsiString)"";
            }

            FileName.Text = SFileName.Text;
            SPlayers.Text = LPlayers.Text;
            SSize.Text = LSize.Text;
            STowns.Text = LTowns.Text;
        }

        // AoWFileDialog.pas:586
        public void FileLBDrawItem(TAoWComponent Sender, in TRect DrawRect, int DrawItem, ref TSurfaceDesc SurfDesc)
        {
            var call__38 = AoWFileDialog.Directories.Count;
            if (call__38 > DrawItem)
            {
                var call__39 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons[0];
                call__39.ShowClipped(DrawRect.Left + 2, DrawRect.Top + 1, SurfDesc);
            }
        }

        // AoWFileDialog.pas:462
        public void FileDialogShow()
        {
            switch (FMode)
            {
                case 0:
                    SetCurrentDir(FDirectory);
                    var call__40 = ExtractFileName(FLastFileName);
                    SFileName.Text = call__40;
                    FileName.Text = SFileName.Text;
                    var call__41 = TranslateRStr(SelectAFileToLoadRStr);
                    AoWFileDialog.FileDlgWindow.Title.Text = call__41;
                    AoWFileDialog.FileDlgWindow.SFileName.Editable = false;
                    var call__42 = TranslateRStr(OpenRStr);
                    AoWFileDialog.FileDlgWindow.Openbut.Text = call__42;
                    SavePnl.Visible = true;
                    NewPnl.Visible = false;
                    break;
                case 1:
                    SetCurrentDir(FDirectory);
                    var call__43 = ExtractFileName(FLastFileName);
                    SFileName.Text = call__43;
                    if (Pos(AnsiString.FromBytes(46), SFileName.Text) != 0)
                    {
                        SFileName.Text = Copy(SFileName.Text, 1, Pos(AnsiString.FromBytes(46), SFileName.Text) - 1);
                    }

                    if (SFileName.Text != (AnsiString)"")
                    {
                        if (AoWEngine.FCampaign != null)
                        {
                            SFileName.Text = SFileName.Text + (AnsiString)".acg";
                        }
                        else
                        {
                            SFileName.Text = SFileName.Text + (AnsiString)".asg";
                        }
                    }

                    FileName.Text = SFileName.Text;
                    var call__44 = TranslateRStr(SaveGameAsRStr);
                    AoWFileDialog.FileDlgWindow.Title.Text = call__44;
                    AoWFileDialog.FileDlgWindow.SFileName.Editable = true;
                    var call__45 = TranslateRStr(SaveRStr);
                    AoWFileDialog.FileDlgWindow.Openbut.Text = call__45;
                    SavePnl.Visible = true;
                    NewPnl.Visible = false;
                    break;
                case 2:
                    var call__46 = ExtractFileExt(FLastFileName);
                    if (call__46 == (AnsiString)".hsm")
                    {
                        var call__47 = ExtractFileName(FLastFileName);
                        FileName.Text = call__47;
                    }
                    else
                    {
                        FileName.Text = (AnsiString)"";
                    }

                    SetCurrentDir(AoWEngine.StartupDirectory + (AnsiString)"Scenario\\Custom\\");
                    var call__48 = TranslateRStr(SelectAMapRStr);
                    AoWFileDialog.FileDlgWindow.Title.Text = call__48;
                    var call__49 = TranslateRStr(SelectRStr);
                    AoWFileDialog.FileDlgWindow.Openbut.Text = call__49;
                    SavePnl.Visible = false;
                    NewPnl.Visible = true;
                    break;
            }

            AoWFileDialog.FileDlgWindow.FindDrives();
        }

        // AoWFileDialog.pas:502
        public void mSBarChange(TAOWVScrollBar Sender)
        {
            TAOWVScrollBar Bar = default;
            Bar = Sender;
            FileLB.ListOffset = Bar.Position;
            SaveLB.ListOffset = Bar.Position;
            DateLB.ListOffset = Bar.Position;
        }

        // AoWFileDialog.pas:511
        public void SFileNameReturn()
        {
            OpenbutClick(this);
        }

        // AoWFileDialog.pas:516
        public void SFileNameChange(int A1)
        {
            FileLB.Selected = -1;
            FileLBChange(FileLB);
        }

        // AoWFileDialog.pas:522
        public void SaveBtnClick(TObject Sender)
        {
            AnsiString Selected = default;
            int I = default;
            General.PlayButtonSound(0);
            if (FileLB.Selected != -1)
            {
                var call__50 = FileLB.Items[FileLB.Selected];
                Selected = call__50;
            }

            {
                I = 0;
                int end__51 = AoWFileDialog.Directories.Count - 1;
                for (; I <= end__51; I += 1)
                {
                    SaveLB.Items.Delete(0);
                    DateLB.Items.Delete(0);
                }
            }

            if (FMode == 1 || FMode == 0)
            {
                SortStringsAscending(SaveLB.Items, [DateLB.Items, FileLB.Items]);
                FileLB.Items.Assign(SaveLB.Items);
            }
            else
            {
                if (FMode == 2)
                {
                    SortStringsAscending(FileLB.Items, [DateLB.Items, SaveLB.Items]);
                }
            }

            {
                I = 0;
                int end__52 = AoWFileDialog.Directories.Count - 1;
                for (; I <= end__52; I += 1)
                {
                    var call__53 = AoWFileDialog.Directories[I];
                    SaveLB.Items.Insert(I, call__53);
                    DateLB.Items.Insert(I, (AnsiString)"");
                }
            }

            if (Selected != (AnsiString)"")
            {
                var call__54 = FileLB.Items.IndexOf(Selected);
                FileLB.Selected = call__54;
            }

            FileLBChange(FileLB);
        }

        // AoWFileDialog.pas:549
        public void DateBtnClick(TObject Sender)
        {
            AnsiString Selected = default;
            int I = default;
            General.PlayButtonSound(0);
            if (FileLB.Selected != -1)
            {
                var call__55 = FileLB.Items[FileLB.Selected];
                Selected = call__55;
            }

            if (FMode == 1 || FMode == 0)
            {
                {
                    I = 0;
                    int end__56 = SaveLB.Items.Count - 1;
                    for (; I <= end__56; I += 1)
                    {
                        var call__57 = DateLB.Items.Objects[I];
                        SaveLB.Items.Objects[I] = call__57;
                    }
                }

                SortStringsEx(SaveLB.Items, AoWFileDialog.CompareFileDates, [DateLB.Items, FileLB.Items]);
            }
            else
            {
                if (FMode == 2)
                {
                    {
                        I = 0;
                        int end__58 = FileLB.Items.Count - 1;
                        for (; I <= end__58; I += 1)
                        {
                            var call__59 = DateLB.Items.Objects[I];
                            FileLB.Items.Objects[I] = call__59;
                        }
                    }

                    SortStringsEx(FileLB.Items, AoWFileDialog.CompareFileDates, [DateLB.Items, SaveLB.Items]);
                }
            }

            if (Selected != (AnsiString)"")
            {
                var call__60 = FileLB.Items.IndexOf(Selected);
                FileLB.Selected = call__60;
            }

            FileLBChange(FileLB);
        }

        // AoWFileDialog.pas:570
        public void SSBarChange()
        {
            SaveLB.ListOffset = SSBar.Position;
        }

        // AoWFileDialog.pas:575
        public void FileDialogHide()
        {
            FinishDialog();
        }

        // AoWFileDialog.pas:580
        public void FileDialogKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
        {
            if (Key == 27)
            {
                FileDialog.Visible = false;
            }
        }
    }

    public static partial class AoWFileDialog
    {
        public static TFileDlgWindow FileDlgWindow;
        public static TStringList Drives;
        public static TStringList Directories;
        public static TStringList Files;
        // AoWFileDialog.pas:121
        public static int CompareFileDates(AnsiString S1, AnsiString S2, TObject O1, TObject O2)
        {
            int Result = default;
            if (O1 == null && O2 == null)
            {
                Result = CompareText(S1, S2);
            }
            else
            {
                if (O1 == null)
                {
                    Result = -1;
                }
                else
                {
                    if (O2 == null)
                    {
                        Result = 1;
                    }
                    else
                    {
                        if ((double)((TFileTime)O2).FTime > ((TFileTime)O1).FTime)
                        {
                            Result = 1;
                        }
                        else
                        {
                            if ((double)((TFileTime)O2).FTime < ((TFileTime)O1).FTime)
                            {
                                Result = -1;
                            }
                            else
                            {
                                Result = 0;
                            }
                        }
                    }
                }
            }

            return Result;
        }
    }
}
