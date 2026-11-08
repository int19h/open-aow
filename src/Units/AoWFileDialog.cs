// Translated from AoWFileDialog.pas.
using Aow.Platform;
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWComboBox;
using Aow.Units.AoWComp;
using Aow.Units.AoWE;
using Aow.Units.AoWEdit;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.aowIntGfxMod;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using Aow.Units.AoWWorldMap;
using Aow.Units.E2Globals;
using Aow.Units.EventLog;
using Aow.Units.GFXE;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using static Aow.Units.E1GameFlow.E1GameFlowUnit;
using static Aow.Units.Engine.EngineUnit;

namespace Aow.Units.AoWFileDialog;

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
    public string FDirectory = "";
    public string FLastFileName = "";
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
    // The current game closes only after the file passes its preflight, so a refused file leaves
    // it intact (own-formats.md 2.7). AoWWinMan.LoadCampaign closes the map after its preflight and
    // TAoWEngine.LoadCampaign closes the campaign. The original closed both first.
    public void LoadSelectedGame(string Name)
    {
        TAoWHSMap Map = default;
        if (Aow.Formats.DocumentKind.CampaignSave.HasExtension(ExtractFileExt(Name)))
        {
            if (!AoWEngine.FDemo)
            {
                // A refused campaign keeps the current map, which is not started again.
                var Previous = AoWHSMap;
                LoadCampaign(General, Name);
                if (AoWHSMap != null && AoWHSMap != Previous)
                {
                    StartGame(General);
                }
            }
        }
        else
        {
            Map = (TAoWHSMap)AoWEngine.CreatePreviewHSM(Name, Aow.Formats.GameProfile.WorldMap);
            if (Map != null)
            {
                AoWEngine.CloseCampaign();
                AoWEngine.CloseHSM(null);
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
                        E2GlobalsUnit.TitleScene.HideScene();
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

    // The file filter of FindFiles (AoWFileDialog.pas:233). Each family lists its original and its
    // own extension (own-formats.md 2.9), so the shown name tells the two apart.
    public static bool ListsFile(int Mode, bool Campaign, string Extension)
    {
        bool IsCampaignSave = Aow.Formats.DocumentKind.CampaignSave.HasExtension(Extension);
        bool IsSave = Aow.Formats.DocumentKind.Save.HasExtension(Extension);
        bool IncludeFile = false;
        IncludeFile = IncludeFile || Mode == 1 && Campaign && IsCampaignSave;
        IncludeFile = IncludeFile || Mode == 1 && !Campaign && IsSave;
        IncludeFile = IncludeFile || Mode == 0 && (IsSave || IsCampaignSave);
        // { Original accepts .asg in every mode, including map selection. }
        IncludeFile = IncludeFile || Mode == 2 && Aow.Formats.DocumentKind.Map.HasExtension(Extension) || IsSave;
        return IncludeFile;
    }

    // AoWFileDialog.pas:233
    public void FindFiles()
    {
        bool IncludeFile;
        string Extension;
        string Directory;
        TSearchRec Search = default;
        TFileTime Stamp;
        for (int I = 0, last = DateLB.Items.Count - 1; I <= last; I++)
        {
            if (DateLB.Items.Objects[I] != null)
            {
                DateLB.Items.Objects[I].Free();
            }
        }

        AoWFileDialogUnit.Directories.Clear();
        AoWFileDialogUnit.Files.Clear();
        SPlayers.Text = "";
        SSize.Text = "";
        STowns.Text = "";
        LPlayers.Text = "";
        LSize.Text = "";
        LTowns.Text = "";
        LTurns.Text = "";
        Directory = GetCurrentDir();
        if (Copy(Directory, Directory.Length, 1) == "\\")
        {
            Directory = Copy(Directory, 1, Directory.Length - 1);
        }

        if (FindFirst(Directory + "\\**", 16, ref Search) == 0)
        {
            do
            {
                if (Search.Name != "." && Search.Name != "..")
                {
                    if ((Search.Attr & 16) == 16)
                    {
                        AoWFileDialogUnit.Directories.Add(Search.Name);
                    }
                    else
                    {
                        Extension = LowerCase(ExtractFileExt(Search.Name));
                        IncludeFile = ListsFile(FMode, AoWEngine.FCampaign != null, Extension);
                        if (IncludeFile)
                        {
                            Stamp = new TFileTime();
                            Stamp.FTime = FileDateToDateTime(Search.Time);
                            AoWFileDialogUnit.Files.AddObject(Search.Name, Stamp);
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
        AoWFileDialogUnit.Directories.Sort();
        AoWFileDialogUnit.Files.Sort();
        FileLB.Items.AddStrings(AoWFileDialogUnit.Directories);
        FileLB.Items.AddStrings(AoWFileDialogUnit.Files);
        SaveLB.Items.AddStrings(FileLB.Items);
        PathEdit.Text = Directory + "\\";
        for (int I = 0, last = AoWFileDialogUnit.Directories.Count - 1; I <= last; I++)
        {
            DateLB.Items.Add("");
        }

        for (int I = 0, last = AoWFileDialogUnit.Files.Count - 1; I <= last; I++)
        {
            var dateTimeToStr = DateTimeToStr(((TFileTime)AoWFileDialogUnit.Files.Objects[I]).FTime);
            var objectValue = AoWFileDialogUnit.Files.Objects[I];
            DateLB.Items.AddObject(dateTimeToStr, objectValue);
        }

        RestoreSortOrder();
    }

    // AoWFileDialog.pas:296
    public void FileDlgWindowCreate()
    {
        AoWFileDialogUnit.Drives = new TStringList();
        AoWFileDialogUnit.Directories = new TStringList();
        AoWFileDialogUnit.Files = new TStringList();
        FDirectory = AoWEngine.StartupDirectory + "Save\\";
        FLastFileName = "";
        if (SaveLB != null) SaveLB.WheelScrollBar = SSBar;
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

        AoWFileDialogUnit.Drives.Free();
        AoWFileDialogUnit.Directories.Free();
        AoWFileDialogUnit.Files.Free();
    }

    // AoWFileDialog.pas:316
    public void OpenbutClick(TObject Sender)
    {
        string Path = "";
        string Directory = "";
        General.PlayButtonSound(0);
        var count = AoWFileDialogUnit.Directories.Count;
        if (FileLB.Selected < count && FileLB.Selected > -1)
        {
            Directory = GetCurrentDir();
            if (Copy(Directory, Directory.Length, 1) == "\\")
            {
                Directory = Copy(Directory, 1, Directory.Length - 1);
            }

            var item = FileLB.Items[FileLB.Selected];
            if (SetCurrentDir(Directory + "\\" + item))
            {
                FindFiles();
            }

            FileLB.Selected = -1;
        }
        else
        {
            Path = GetCurrentDir();
            if (Path.Length > 0 && Path[Path.Length - 1] != 92)
            {
                Path = Path + "\\";
            }

            switch (FMode)
            {
                case 0:
                    if (!IsEmpty(SFileName.Text) && FileExists(SFileName.Text))
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
                    if (!IsEmpty(SFileName.Text))
                    {
                        FDirectory = Path;
                        if (Pos(".", SFileName.Text) != 0)
                        {
                            SFileName.Text = Copy(SFileName.Text, 1, Pos(".", SFileName.Text) - 1);
                        }

                        if (AoWEngine.FCampaign != null)
                        {
                            FileDialog.Visible = false;
                            // The save takes the extension the host writes, beside an original of the same name
                            // (own-formats.md 4.2).
                            SFileName.Text = FileBoundary.WriteName(SFileName.Text + Aow.Formats.DocumentKind.CampaignSave.OriginalExtension);
                            SaveCampaign(General, SFileName.Text);
                        }
                        else
                        {
                            if (FSaveEvent != null)
                            {
                                FSaveEvent.FFileName = FileBoundary.WriteName(SFileName.Text + Aow.Formats.DocumentKind.Save.OriginalExtension, FDirectory);
                                FileDialog.Visible = false;
                            }
                            else
                            {
                                FileDialog.Visible = false;
                                SFileName.Text = FileBoundary.WriteName(SFileName.Text + Aow.Formats.DocumentKind.Save.OriginalExtension);
                                SaveGame(General, SFileName.Text);
                            }
                        }

                        FLastFileName = SFileName.Text;
                        FDirectory = Path;
                    }

                    break;
                case 2:
                    if (!IsEmpty(FileName.Text) && FileExists(FileName.Text))
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
        if (SetCurrentDir(".."))
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
        if (AoWFileDialogUnit.Directories.Count - 1 < this.FileLB.Selected)
        {
            var item = FileLB.Items[FileLB.Selected];
            SFileName.Text = item;
            if (!Aow.Formats.DocumentKind.CampaignSave.HasExtension(ExtractFileExt(SFileName.Text)))
            {
                Map = (TAoWHSMap)AoWEngine.CreatePreviewHSM(SFileName.Text, Aow.Formats.GameProfile.WorldMap);
                if (Map != null)
                {
                    var count = Map.FPlayers.GetCount();
                    LPlayers.Text = IntToStr(count - 1);
                    switch (Map.MapContainer.Width)
                    {
                        case 48:
                            var translateRStr = TranslateRStr("S");
                            LSize.Text = translateRStr;
                            break;
                        case 64:
                            var translateRStr2 = TranslateRStr("M");
                            LSize.Text = translateRStr2;
                            break;
                        case 96:
                            var translateRStr3 = TranslateRStr("L");
                            LSize.Text = translateRStr3;
                            break;
                        case 127:
                        case 128:
                            var extraLargeMapAbr = TranslateRStr(ExtraLargeMapAbrRStr);
                            LSize.Text = extraLargeMapAbr;
                            break;
                        default:
                            LSize.Text = "?";
                            break;
                    }

                    LTowns.Text = IntToStr(Map.FStructureControl.FCityCount);
                    LTurns.Text = IntToStr(Map.FDay);
                    Map.Free();
                }
                else
                {
                    LPlayers.Text = "";
                    LSize.Text = "";
                    LTowns.Text = "";
                    LTurns.Text = "";
                }
            }
            else
            {
                LPlayers.Text = "";
                LSize.Text = "";
                LTowns.Text = "";
                LTurns.Text = "";
            }
        }
        else
        {
            LPlayers.Text = "";
            LSize.Text = "";
            LTowns.Text = "";
            LTurns.Text = "";
        }

        FileName.Text = SFileName.Text;
        SPlayers.Text = LPlayers.Text;
        SSize.Text = LSize.Text;
        STowns.Text = LTowns.Text;
    }

    // AoWFileDialog.pas:586
    public void FileLBDrawItem(TAoWComponent Sender, in TRect DrawRect, int DrawItem, ref TSurfaceDesc SurfDesc)
    {
        if (AoWFileDialogUnit.Directories.Count > DrawItem)
        {
            aowIntGfxModUnit.IntGfxMod.LBIcons[0].ShowClipped(DrawRect.Left + 2, DrawRect.Top + 1, SurfDesc);
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
                AoWFileDialogUnit.FileDlgWindow.Title.Text = selectAFileToLoad;
                AoWFileDialogUnit.FileDlgWindow.SFileName.Editable = false;
                var open = TranslateRStr(OpenRStr);
                AoWFileDialogUnit.FileDlgWindow.Openbut.Text = open;
                SavePnl.Visible = true;
                NewPnl.Visible = false;
                break;
            case 1:
                SetCurrentDir(FDirectory);
                var fileName2 = ExtractFileName(FLastFileName);
                SFileName.Text = fileName2;
                if (Pos(".", SFileName.Text) != 0)
                {
                    SFileName.Text = Copy(SFileName.Text, 1, Pos(".", SFileName.Text) - 1);
                }

                if (!IsEmpty(SFileName.Text))
                {
                    // The default name shows the file the save writes (own-formats.md 4.2).
                    if (AoWEngine.FCampaign != null)
                    {
                        SFileName.Text = FileBoundary.WriteName(SFileName.Text + Aow.Formats.DocumentKind.CampaignSave.OriginalExtension);
                    }
                    else
                    {
                        SFileName.Text = FileBoundary.WriteName(SFileName.Text + Aow.Formats.DocumentKind.Save.OriginalExtension);
                    }
                }

                FileName.Text = SFileName.Text;
                var saveGameAs = TranslateRStr(SaveGameAsRStr);
                AoWFileDialogUnit.FileDlgWindow.Title.Text = saveGameAs;
                AoWFileDialogUnit.FileDlgWindow.SFileName.Editable = true;
                var save = TranslateRStr(SaveRStr);
                AoWFileDialogUnit.FileDlgWindow.Openbut.Text = save;
                SavePnl.Visible = true;
                NewPnl.Visible = false;
                break;
            case 2:
                var lastExtension = ExtractFileExt(FLastFileName);
                if (lastExtension == Aow.Formats.DocumentKind.Map.OriginalExtension || lastExtension == Aow.Formats.DocumentKind.Map.OwnExtension)
                {
                    var fileName3 = ExtractFileName(FLastFileName);
                    FileName.Text = fileName3;
                }
                else
                {
                    FileName.Text = "";
                }

                SetCurrentDir(AoWEngine.StartupDirectory + "Scenario\\Custom\\");
                var selectAMap = TranslateRStr(SelectAMapRStr);
                AoWFileDialogUnit.FileDlgWindow.Title.Text = selectAMap;
                var select = TranslateRStr(SelectRStr);
                AoWFileDialogUnit.FileDlgWindow.Openbut.Text = select;
                SavePnl.Visible = false;
                NewPnl.Visible = true;
                break;
        }

        AoWFileDialogUnit.FileDlgWindow.FindDrives();
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
    public void SFileNameChange(TObject Sender)
    {
        FileLB.Selected = -1;
        FileLBChange(FileLB);
    }

    // AoWFileDialog.pas:522
    public void SaveBtnClick(TObject Sender)
    {
        General.PlayButtonSound(0);
        RememberSortByDate(false);
        SortByName();
    }

    public void SortByName()
    {
        string Selected = "";
        if (FileLB.Selected != -1)
        {
            Selected = FileLB.Items[FileLB.Selected];
        }

        for (int I = 0, last = AoWFileDialogUnit.Directories.Count - 1; I <= last; I++)
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

        for (int I = 0, last = AoWFileDialogUnit.Directories.Count - 1; I <= last; I++)
        {
            var directory = AoWFileDialogUnit.Directories[I];
            SaveLB.Items.Insert(I, directory);
            DateLB.Items.Insert(I, "");
        }

        if (!IsEmpty(Selected))
        {
            var indexOf = FileLB.Items.IndexOf(Selected);
            FileLB.Selected = indexOf;
        }

        FileLBChange(FileLB);
    }

    // AoWFileDialog.pas:549
    public void DateBtnClick(TObject Sender)
    {
        General.PlayButtonSound(0);
        RememberSortByDate(true);
        SortByDate();
    }

    public void SortByDate()
    {
        string Selected = "";
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

            SortStringsEx(SaveLB.Items, AoWFileDialogUnit.CompareFileDates, [DateLB.Items, FileLB.Items]);
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

                SortStringsEx(FileLB.Items, AoWFileDialogUnit.CompareFileDates, [DateLB.Items, SaveLB.Items]);
            }
        }

        if (!IsEmpty(Selected))
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

    public void FindDrives()
    {
        AoWFileDialogUnit.Drives.Clear();
        string drive = Files.VirtualDrive + "\\";
        string label = OperatingSystem.IsWindows() ? new DriveInfo(drive).VolumeLabel + " " : "";
        AoWFileDialogUnit.Drives.Add($"{label}({drive})");
        DriveCmb.Items.Clear();
        DriveCmb.Items.AddStrings(AoWFileDialogUnit.Drives);
        DriveCmb.Selected = 0;
        FindFiles();
    }
}

public static partial class AoWFileDialogUnit
{
    public static TFileDlgWindow FileDlgWindow;
    public static TStringList Drives;
    public static TStringList Directories;
    public static TStringList Files;

    // AoWFileDialog.pas:121
    public static int CompareFileDates(string S1, string S2, TObject O1, TObject O2)
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
