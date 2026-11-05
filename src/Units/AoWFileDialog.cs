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
            TExecuteDoneEvent Done;
            TSaveGameEventLog Event;
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
                Map = (TAoWHSMap)AoWEngine.CreatePreviewHSM(Name);
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
                            var relativePath = ExtractRelativePath(AoWEngine.StartupDirectory, Name);
                            MultiWindow.Execute(relativePath);
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
            bool IncludeFile;
            AnsiString Extension;
            AnsiString Directory;
            TSearchRec Search = default;
            TFileTime Stamp;
            for (int I = 0, last = DateLB.Items.Count - 1; I <= last; I++)
            {
                if (DateLB.Items.Objects[I] != null)
                {
                    DateLB.Items.Objects[I].Free();
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
                            Extension = LowerCase(ExtractFileExt(Search.Name));
                            IncludeFile = IncludeFile || (FMode == 1 && AoWEngine.FCampaign != null && Extension == (AnsiString)".acg");
                            IncludeFile = IncludeFile || (FMode == 1 && AoWEngine.FCampaign == null && Extension == (AnsiString)".asg");
                            IncludeFile = IncludeFile || (FMode == 0 && (Extension == (AnsiString)".asg" || Extension == (AnsiString)".acg"));
                            // { Original accepts .asg in every mode, including map selection. }
                            IncludeFile = IncludeFile || (FMode == 2 && Extension == (AnsiString)".hsm") || Extension == (AnsiString)".asg";
                            if (IncludeFile)
                            {
                                Stamp = new TFileTime();
                                Stamp.FTime = FileDateToDateTime(Search.Time);
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
            for (int I = 0, last = AoWFileDialog.Directories.Count - 1; I <= last; I++)
            {
                DateLB.Items.Add((AnsiString)"");
            }

            for (int I = 0, last = AoWFileDialog.Files.Count - 1; I <= last; I++)
            {
                var dateTimeToStr = DateTimeToStr(((TFileTime)AoWFileDialog.Files.Objects[I]).FTime);
                var objectValue = AoWFileDialog.Files.Objects[I];
                DateLB.Items.AddObject(dateTimeToStr, objectValue);
            }
        }

        // AoWFileDialog.pas:296
        public void FileDlgWindowCreate()
        {
            AoWFileDialog.Drives = new TStringList();
            AoWFileDialog.Directories = new TStringList();
            AoWFileDialog.Files = new TStringList();
            FDirectory = AoWEngine.StartupDirectory + (AnsiString)"Save\\";
            FLastFileName = (AnsiString)"";
        }

        // AoWFileDialog.pas:305
        public void FileDlgWindowDestroy()
        {
            for (int I = 0, last = DateLB.Items.Count - 1; I <= last; I++)
            {
                if (DateLB.Items.Objects[I] != null)
                {
                    DateLB.Items.Objects[I].Free();
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
            var count = AoWFileDialog.Directories.Count;
            if (FileLB.Selected < count && FileLB.Selected > -1)
            {
                Directory = GetCurrentDir();
                if (Copy(Directory, Directory.Length, 1) == AnsiString.FromBytes(92))
                {
                    Directory = Copy(Directory, 1, Directory.Length - 1);
                }

                var item = FileLB.Items[FileLB.Selected];
                if (SetCurrentDir(Directory + AnsiString.FromBytes(92) + item))
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
                        if (SFileName.Text != (AnsiString)"" && FileExists(SFileName.Text))
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
                        if (FileName.Text != (AnsiString)"" && FileExists(FileName.Text))
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
            var item = DriveCmb.Items[DriveCmb.Selected];
            if (SetCurrentDir(Copy(DriveCmb.Items[DriveCmb.Selected], item.Length - 3, 3)))
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
            TAoWHSMap Map;
            TAOWListBox List;
            List = Sender;
            FileLB.Selected = List.Selected;
            SaveLB.Selected = List.Selected;
            DateLB.Selected = List.Selected;
            if (AoWFileDialog.Directories.Count - 1 < this.FileLB.Selected)
            {
                var item = FileLB.Items[FileLB.Selected];
                SFileName.Text = item;
                if (LowerCase(ExtractFileExt(SFileName.Text)) != (AnsiString)".acg")
                {
                    Map = (TAoWHSMap)AoWEngine.CreatePreviewHSM(SFileName.Text);
                    if (Map != null)
                    {
                        var count = Map.FPlayers.GetCount();
                        LPlayers.Text = IntToStr(count - 1);
                        switch (Map.MapContainer.Width)
                        {
                            case 48:
                                var translateRStr = TranslateRStr(AnsiString.FromBytes(SmallMapAbrRStr));
                                LSize.Text = translateRStr;
                                break;
                            case 64:
                                var translateRStr2 = TranslateRStr(AnsiString.FromBytes(MediumMapAbrRStr));
                                LSize.Text = translateRStr2;
                                break;
                            case 96:
                                var translateRStr3 = TranslateRStr(AnsiString.FromBytes(LargeMapAbrRStr));
                                LSize.Text = translateRStr3;
                                break;
                            case 127:
                            case 128:
                                var extraLargeMapAbr = TranslateRStr(ExtraLargeMapAbrRStr);
                                LSize.Text = extraLargeMapAbr;
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
            if (AoWFileDialog.Directories.Count > DrawItem)
            {
                aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons[0].ShowClipped(DrawRect.Left + 2, DrawRect.Top + 1, SurfDesc);
            }
        }

        // AoWFileDialog.pas:462
        public void FileDialogShow()
        {
            switch (FMode)
            {
                case 0:
                    SetCurrentDir(FDirectory);
                    var fileName = ExtractFileName(FLastFileName);
                    SFileName.Text = fileName;
                    FileName.Text = SFileName.Text;
                    var selectAFileToLoad = TranslateRStr(SelectAFileToLoadRStr);
                    AoWFileDialog.FileDlgWindow.Title.Text = selectAFileToLoad;
                    AoWFileDialog.FileDlgWindow.SFileName.Editable = false;
                    var open = TranslateRStr(OpenRStr);
                    AoWFileDialog.FileDlgWindow.Openbut.Text = open;
                    SavePnl.Visible = true;
                    NewPnl.Visible = false;
                    break;
                case 1:
                    SetCurrentDir(FDirectory);
                    var fileName2 = ExtractFileName(FLastFileName);
                    SFileName.Text = fileName2;
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
                    var saveGameAs = TranslateRStr(SaveGameAsRStr);
                    AoWFileDialog.FileDlgWindow.Title.Text = saveGameAs;
                    AoWFileDialog.FileDlgWindow.SFileName.Editable = true;
                    var save = TranslateRStr(SaveRStr);
                    AoWFileDialog.FileDlgWindow.Openbut.Text = save;
                    SavePnl.Visible = true;
                    NewPnl.Visible = false;
                    break;
                case 2:
                    if (ExtractFileExt(FLastFileName) == (AnsiString)".hsm")
                    {
                        var fileName3 = ExtractFileName(FLastFileName);
                        FileName.Text = fileName3;
                    }
                    else
                    {
                        FileName.Text = (AnsiString)"";
                    }

                    SetCurrentDir(AoWEngine.StartupDirectory + (AnsiString)"Scenario\\Custom\\");
                    var selectAMap = TranslateRStr(SelectAMapRStr);
                    AoWFileDialog.FileDlgWindow.Title.Text = selectAMap;
                    var select = TranslateRStr(SelectRStr);
                    AoWFileDialog.FileDlgWindow.Openbut.Text = select;
                    SavePnl.Visible = false;
                    NewPnl.Visible = true;
                    break;
            }

            AoWFileDialog.FileDlgWindow.FindDrives();
        }

        // AoWFileDialog.pas:502
        public void mSBarChange(TAOWVScrollBar Sender)
        {
            TAOWVScrollBar Bar;
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
            General.PlayButtonSound(0);
            if (FileLB.Selected != -1)
            {
                Selected = FileLB.Items[FileLB.Selected];
            }

            for (int I = 0, last = AoWFileDialog.Directories.Count - 1; I <= last; I++)
            {
                SaveLB.Items.Delete(0);
                DateLB.Items.Delete(0);
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

            for (int I = 0, last = AoWFileDialog.Directories.Count - 1; I <= last; I++)
            {
                var directory = AoWFileDialog.Directories[I];
                SaveLB.Items.Insert(I, directory);
                DateLB.Items.Insert(I, (AnsiString)"");
            }

            if (Selected != (AnsiString)"")
            {
                var indexOf = FileLB.Items.IndexOf(Selected);
                FileLB.Selected = indexOf;
            }

            FileLBChange(FileLB);
        }

        // AoWFileDialog.pas:549
        public void DateBtnClick(TObject Sender)
        {
            AnsiString Selected = default;
            General.PlayButtonSound(0);
            if (FileLB.Selected != -1)
            {
                Selected = FileLB.Items[FileLB.Selected];
            }

            if (FMode == 1 || FMode == 0)
            {
                for (int I = 0, last = SaveLB.Items.Count - 1; I <= last; I++)
                {
                    var objectValue = DateLB.Items.Objects[I];
                    SaveLB.Items.Objects[I] = objectValue;
                }

                SortStringsEx(SaveLB.Items, AoWFileDialog.CompareFileDates, [DateLB.Items, FileLB.Items]);
            }
            else
            {
                if (FMode == 2)
                {
                    for (int I = 0, last = FileLB.Items.Count - 1; I <= last; I++)
                    {
                        var objectValue = DateLB.Items.Objects[I];
                        FileLB.Items.Objects[I] = objectValue;
                    }

                    SortStringsEx(FileLB.Items, AoWFileDialog.CompareFileDates, [DateLB.Items, SaveLB.Items]);
                }
            }

            if (Selected != (AnsiString)"")
            {
                var indexOf = FileLB.Items.IndexOf(Selected);
                FileLB.Selected = indexOf;
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
            int Result;
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
