// Translated from AoWFileDialog.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Global;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// directory, last filename, save event, done callback, dialog mode
public partial class FileDlgWindow : DataModule
{
    public FileDlgWindow(Component owner) : base(owner)
    { }

    public AOWWindow FileDialog;
    public AOWButton CancelBut;
    public AoWFrame BCeltL;
    public AOWButton Openbut;
    public AOWLabel Title;
    public AOWListBox FileLB;
    public AOWEdit FileName;
    public AOWLabel ScenLbl;
    public AOWComboBox DriveCmb;
    public AOWLabel DriveL;
    public AOWButton Close;
    public AOWButton UpDirBtn;
    public AOWLabel PathL;
    public AOWImage TitleIcon;
    public AOWVScrollBar DriveSB;
    public AOWImage CeltM;
    public AOWImage LPlayersI;
    public AOWPanel LSavedInfo;
    public AOWImage LSizeI;
    public AOWImage LTownsI;
    public AOWImage LTurnsI;
    public AOWLabel LPlayers;
    public AOWLabel LSize;
    public AOWLabel LTowns;
    public AOWLabel LTurns;
    public AOWImage CeltL;
    public AOWImage CeltR;
    public AoWFrame TCeltL;
    public AOWImage TCelt;
    public AOWImage SPlayersI;
    public AOWPanel NewInfo;
    public AOWImage SSizeI;
    public AOWImage STownsI;
    public AOWLabel SPlayers;
    public AOWLabel SSize;
    public AOWLabel STowns;
    public AOWPanel NewPnl;
    public AOWListBox SaveLB;
    public AOWEdit SFileName;
    public AOWLabel SavedInfo;
    public AOWPanel SavePnl;
    public AOWListBox DateLB;
    public AOWButton SaveBtn;
    public AOWButton DateBtn;
    public AOWVScrollBar NSBar;
    public AOWPanel DriveFr;
    public AOWPanel TopPnl;
    public AOWPanel PathFr;
    public AOWEdit PathEdit;
    public AOWPanel NewLBFr;
    public AOWPanel NewInfo1;
    public AOWPanel NewInfo2;
    public AOWPanel NewInfo3;
    public AOWPanel SaveLBFr;
    public AOWVScrollBar SSBar;
    public AOWPanel SavedInfo1;
    public AOWPanel SavedInfo2;
    public AOWPanel SavedInfo3;
    public AOWPanel SavedInfo4;
    public IvTranslator Translator;
    public string FDirectory = "";
    public string FLastFileName = "";
    public SaveGameEventLog FSaveEvent;
    public ExecuteDoneEvent FDone;
    // is the data pointer of FDone
    public int FMode;

    // AoWFileDialog.pas:175
    // 0 load, 1 save, 2 select map
    public void FinishDialog()
    {
        ExecuteDoneEvent Done;
        SaveGameEventLog Event;
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
    public void ExitForLoad(ExitGameMode Mode, bool Success)
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
        AoWHSMap Map = default;
        if (Aow.Formats.DocumentKind.CampaignSave.HasExtension(ExtractFileExt(Name)))
        {
            if (!GameTables.AoWEngine.FDemo)
            {
                // A refused campaign keeps the current map, which is not started again.
                var Previous = AoWHSMap.Current;
                LoadCampaign(DialogHosts.General, Name);
                if (AoWHSMap.Current != null && AoWHSMap.Current != Previous)
                {
                    General.StartGame(DialogHosts.General);
                }
            }
        }
        else
        {
            Map = (AoWHSMap)GameTables.AoWEngine.CreatePreviewHSM(Name, Aow.Formats.GameProfile.WorldMap);
            if (Map != null)
            {
                GameTables.AoWEngine.CloseCampaign();
                GameTables.AoWEngine.CloseHSM(null);
                try
                {
                    if (Map.FConnectionType != PlayerConnectionType.Network)
                    {
                        General.LoadGame(DialogHosts.General, Name);
                        if (AoWHSMap.Current != null)
                        {
                            General.StartGame(DialogHosts.General);
                        }
                    }
                    else
                    {
                        DialogHosts.TitleScene.HideScene();
                        DialogHosts.General.HideWindows();
                        DialogHosts.TBWindow.TaskWindow.Visible = false;
                        var relativePath = ExtractRelativePath(GameTables.AoWEngine.StartupDirectory, Name);
                        DialogHosts.MultiWindow.Execute(relativePath);
                    }
                }
                finally
                {
                    Map?.Dispose();
                }
            }
        }
    }

    // AoWFileDialog.pas:188
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (FSaveEvent != null)
        {
            if (!FSaveEvent.FMode.Contains(SaveGameEventLogModeElement.DisableCancel))
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
        SearchRec Search = default;
        FileTime Stamp;
        for (int I = 0, last = DateLB.Items.Count - 1; I <= last; I++)
        {
            if (DateLB.Items.Objects[I] != null)
            {
                (DateLB.Items.Objects[I] as IDisposable)?.Dispose();
            }
        }

        Directories.Clear();
        Files.Clear();
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
                        Directories.Add(Search.Name);
                    }
                    else
                    {
                        Extension = LowerCase(ExtractFileExt(Search.Name));
                        IncludeFile = ListsFile(FMode, GameTables.AoWEngine.FCampaign != null, Extension);
                        if (IncludeFile)
                        {
                            Stamp = new FileTime();
                            Stamp.FTime = FileDateToDateTime(Search.Time);
                            Files.AddObject(Search.Name, Stamp);
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
        Directories.Sort();
        Files.Sort();
        FileLB.Items.AddStrings(Directories);
        FileLB.Items.AddStrings(Files);
        SaveLB.Items.AddStrings(FileLB.Items);
        PathEdit.Text = Directory + "\\";
        for (int I = 0, last = Directories.Count - 1; I <= last; I++)
        {
            DateLB.Items.Add("");
        }

        for (int I = 0, last = Files.Count - 1; I <= last; I++)
        {
            var dateTimeToStr = DateTimeToStr(((FileTime)Files.Objects[I]).FTime);
            var objectValue = Files.Objects[I];
            DateLB.Items.AddObject(dateTimeToStr, objectValue);
        }

        RestoreSortOrder();
    }

    // AoWFileDialog.pas:296
    public void FileDlgWindowCreate()
    {
        Drives = new StringList();
        Directories = new StringList();
        Files = new StringList();
        FDirectory = GameTables.AoWEngine.StartupDirectory + "Save\\";
        FLastFileName = "";
    }

    // AoWFileDialog.pas:305
    public void FileDlgWindowDestroy()
    {
        for (int I = 0, last = DateLB.Items.Count - 1; I <= last; I++)
        {
            if (DateLB.Items.Objects[I] != null)
            {
                (DateLB.Items.Objects[I] as IDisposable)?.Dispose();
            }
        }

        Drives?.Dispose();
        Directories?.Dispose();
        Files?.Dispose();
    }

    // AoWFileDialog.pas:316
    public void OpenbutClick()
    {
        string Path = "";
        string Directory = "";
        DialogHosts.General.PlayButtonSound(0);
        var count = Directories.Count;
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
                        if (AoWHSMap.Current != null)
                        {
                            if (AoWHSMap.Current.FCombatMap != null)
                            {
                                DialogHosts.General.ReturnToTitle();
                            }
                            else
                            {
                                if (GameTables.AoWEngine.FCampaign != null)
                                {
                                    if (CompareText(GameTables.AoWEngine.FCampaign.FFileName, FDirectory + SFileName.Text) == 0)
                                    {
                                        AoWHSMap.Current.ExitGame(ExitGameMode.Reload, ExitForLoad);
                                    }
                                    else
                                    {
                                        AoWHSMap.Current.ExitGame(ExitGameMode.Load, ExitForLoad);
                                    }
                                }
                                else
                                {
                                    if (CompareText(AoWHSMap.Current.FileName, FDirectory + SFileName.Text) == 0)
                                    {
                                        AoWHSMap.Current.ExitGame(ExitGameMode.Reload, ExitForLoad);
                                    }
                                    else
                                    {
                                        AoWHSMap.Current.ExitGame(ExitGameMode.Load, ExitForLoad);
                                    }
                                }
                            }
                        }

                        if (AoWHSMap.Current == null || AoWHSMap.Current.FExitPending)
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

                        if (GameTables.AoWEngine.FCampaign != null)
                        {
                            FileDialog.Visible = false;
                            // The save takes the extension the host writes, beside an original of the same name
                            // (own-formats.md 4.2).
                            SFileName.Text = FileBoundary.WriteName(SFileName.Text + Aow.Formats.DocumentKind.CampaignSave.OriginalExtension);
                            SaveCampaign(DialogHosts.General, SFileName.Text);
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
                                SaveGame(DialogHosts.General, SFileName.Text);
                            }
                        }

                        FLastFileName = SFileName.Text;
                        FDirectory = Path;
                    }

                    break;
                case 2:
                    if (!IsEmpty(FileName.Text) && FileExists(FileName.Text))
                    {
                        PickScenarioMap(DialogHosts.ScenSetup, FileName.Text);
                        FLastFileName = FileName.Text;
                    }

                    break;
            }
        }
    }

    // AoWFileDialog.pas:394
    public void UpDirBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
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
    public void FileLBChange(AOWListBox Sender)
    {
        AoWHSMap Map;
        AOWListBox List;
        List = Sender;
        FileLB.Selected = List.Selected;
        SaveLB.Selected = List.Selected;
        DateLB.Selected = List.Selected;
        if (Directories.Count - 1 < this.FileLB.Selected)
        {
            var item = FileLB.Items[FileLB.Selected];
            SFileName.Text = item;
            if (!Aow.Formats.DocumentKind.CampaignSave.HasExtension(ExtractFileExt(SFileName.Text)))
            {
                Map = (AoWHSMap)GameTables.AoWEngine.CreatePreviewHSM(SFileName.Text, Aow.Formats.GameProfile.WorldMap);
                if (Map != null)
                {
                    var count = Map.FPlayers.GetCount();
                    LPlayers.Text = IntToStr(count - 1);
                    switch (Map.MapContainer.Width)
                    {
                        case 48:
                            var translateRStr = AbilityRules.TranslateRStr("S");
                            LSize.Text = translateRStr;
                            break;
                        case 64:
                            var translateRStr2 = AbilityRules.TranslateRStr("M");
                            LSize.Text = translateRStr2;
                            break;
                        case 96:
                            var translateRStr3 = AbilityRules.TranslateRStr("L");
                            LSize.Text = translateRStr3;
                            break;
                        case 127:
                        case 128:
                            var extraLargeMapAbr = AbilityRules.TranslateRStr(ExtraLargeMapAbrRStr);
                            LSize.Text = extraLargeMapAbr;
                            break;
                        default:
                            LSize.Text = "?";
                            break;
                    }

                    LTowns.Text = IntToStr(Map.FStructureControl.FCityCount);
                    LTurns.Text = IntToStr(Map.FDay);
                    Map?.Dispose();
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
    public void FileLBDrawItem(in Rect DrawRect, int DrawItem, ref SurfaceDesc SurfDesc)
    {
        if (Directories.Count > DrawItem)
        {
            DialogHosts.IntGfxMod.LBIcons[0].ShowClipped(DrawRect.Left + 2, DrawRect.Top + 1, SurfDesc);
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
                var selectAFileToLoad = AbilityRules.TranslateRStr(SelectAFileToLoadRStr);
                DialogHosts.FileDlgWindow.Title.Text = selectAFileToLoad;
                DialogHosts.FileDlgWindow.SFileName.Editable = false;
                var open = AbilityRules.TranslateRStr(OpenRStr);
                DialogHosts.FileDlgWindow.Openbut.Text = open;
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
                    if (GameTables.AoWEngine.FCampaign != null)
                    {
                        SFileName.Text = FileBoundary.WriteName(SFileName.Text + Aow.Formats.DocumentKind.CampaignSave.OriginalExtension);
                    }
                    else
                    {
                        SFileName.Text = FileBoundary.WriteName(SFileName.Text + Aow.Formats.DocumentKind.Save.OriginalExtension);
                    }
                }

                FileName.Text = SFileName.Text;
                var saveGameAs = AbilityRules.TranslateRStr(SaveGameAsRStr);
                DialogHosts.FileDlgWindow.Title.Text = saveGameAs;
                DialogHosts.FileDlgWindow.SFileName.Editable = true;
                var save = AbilityRules.TranslateRStr(GameText.SaveRStr);
                DialogHosts.FileDlgWindow.Openbut.Text = save;
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

                SetCurrentDir(GameTables.AoWEngine.StartupDirectory + "Scenario\\Custom\\");
                var selectAMap = AbilityRules.TranslateRStr(SelectAMapRStr);
                DialogHosts.FileDlgWindow.Title.Text = selectAMap;
                var select = AbilityRules.TranslateRStr(SelectRStr);
                DialogHosts.FileDlgWindow.Openbut.Text = select;
                SavePnl.Visible = false;
                NewPnl.Visible = true;
                break;
        }

        DialogHosts.FileDlgWindow.FindDrives();
    }

    // AoWFileDialog.pas:502
    public void mSBarChange(AOWVScrollBar Sender)
    {
        AOWVScrollBar Bar;
        Bar = Sender;
        FileLB.ListOffset = Bar.Position;
        SaveLB.ListOffset = Bar.Position;
        DateLB.ListOffset = Bar.Position;
    }

    // AoWFileDialog.pas:511
    public void SFileNameReturn()
    {
        OpenbutClick();
    }

    // AoWFileDialog.pas:516
    public void SFileNameChange()
    {
        FileLB.Selected = -1;
        FileLBChange(FileLB);
    }

    // AoWFileDialog.pas:522
    public void SaveBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        RememberSortByDate(false);
        SortByName();
    }

    internal void SortByName()
    {
        string Selected = "";
        if (FileLB.Selected != -1)
        {
            Selected = FileLB.Items[FileLB.Selected];
        }

        for (int I = 0, last = Directories.Count - 1; I <= last; I++)
        {
            SaveLB.Items.Delete(0);
            DateLB.Items.Delete(0);
        }

        if (FMode == 1 || FMode == 0)
        {
            StringSorting.SortStringsAscending(SaveLB.Items, [DateLB.Items, FileLB.Items]);
            FileLB.Items.Assign(SaveLB.Items);
        }
        else
        {
            if (FMode == 2)
            {
                StringSorting.SortStringsAscending(FileLB.Items, [DateLB.Items, SaveLB.Items]);
            }
        }

        for (int I = 0, last = Directories.Count - 1; I <= last; I++)
        {
            var directory = Directories[I];
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
    public void DateBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        RememberSortByDate(true);
        SortByDate();
    }

    internal void SortByDate()
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

            StringSorting.SortStringsEx(SaveLB.Items, CompareFileDates, [DateLB.Items, FileLB.Items]);
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

                StringSorting.SortStringsEx(FileLB.Items, CompareFileDates, [DateLB.Items, SaveLB.Items]);
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
    public void FileDialogKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            FileDialog.Visible = false;
        }
    }

    public void FindDrives()
    {
        Drives.Clear();
        string drive = RtlUnit.Files.VirtualDrive + "\\";
        string label = OperatingSystem.IsWindows() ? new DriveInfo(drive).VolumeLabel + " " : "";
        Drives.Add($"{label}({drive})");
        DriveCmb.Items.Clear();
        DriveCmb.Items.AddStrings(Drives);
        DriveCmb.Selected = 0;
        FindFiles();
    }

    private static readonly string ExtraLargeMapAbrRStr = "XL";

    private static readonly string OpenRStr = "Open";

    private static readonly string SaveGameAsRStr = "Save game as...";

    private static readonly string SelectAFileToLoadRStr = "Select a File to Load...";

    private static readonly string SelectAMapRStr = "Select a Map...";

    private static readonly string SelectRStr = "Select";

    internal static StringList Drives;

    internal static StringList Directories;

    internal static StringList Files;

    internal static int CompareFileDates(string S1, string S2, object O1, object O2)
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
                    if ((double)((FileTime)O2).FTime > ((FileTime)O1).FTime)
                    {
                        Result = 1;
                    }
                    else
                    {
                        if ((double)((FileTime)O2).FTime < ((FileTime)O1).FTime)
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

    private static void LoadCampaign(General Controller, string Name)
    {
        byte[] Image = FileBoundary.Load(Name, Aow.Formats.GameProfile.Campaign);
        if (Image == null)
        {
            return;
        }

        if (AoWHSMap.Current != null)
        {
            GameTables.AoWEngine.CloseHSM(null);
        }

        GameTables.AoWEngine.LoadCampaign(Name, Image);
        if (GameTables.AoWEngine.FCampaign != null)
        {
            if (GameTables.AoWEngine.FCampaign.FScenarioStarted)
            {
                var e1Loading = AbilityRules.TranslateRStr(LayerText.E1LoadingRStr);
                var fileName = ExtractFileName(Name);
                DialogHosts.ProgressDialog.Title.Text = e1Loading + " " + fileName + "...";
                DialogHosts.ProgressDialog.StatusLbl.Text = "";
                DialogHosts.ProgressDialog.Bar.Position = 0;
                DialogHosts.ProgressDialog.Bar.Max = 100;
                DialogHosts.ProgressDialog.ProgDlg.Visible = true;
                if (!DialogHosts.TitleScene.Window.Visible)
                {
                    DialogHosts.General.Manager.OverlayPriority = 5000;
                }

                AudioHost.SongPlayer.Stop(false);
                AudioHost.SongPlayer.Songs.Clear();
                AudioPlayback.FadeMusicVolume(0, 1);
                GameTables.AoWEngine.LoadCampaignMap((_, Message, Progression) => Controller.MapProgress(Message, Progression));
                DialogHosts.General.Manager.OverlayPriority = 0;
                DialogHosts.ProgressDialog.ProgDlg.Visible = false;
            }
            else
            {
                DialogHosts.TitleScene.HideScene();
                WorldMap.ShowCampaignMap(DialogHosts.WorldMap);
            }
        }
    }

    private static void SaveGame(General Controller, string Name)
    {
        AoWHSMap.Current.SaveGame(Name);
    }

    private static void SaveCampaign(General Controller, string Name)
    {
        // { Original campaign-save command uses the map save routine. }
        AoWHSMap.Current.SaveGame(Name);
    }

    private static void PickScenarioMap(ScenSetup Dialog, string Name)
    {
        var currentDir = GetCurrentDir();
        Name = ExtractRelativePath(GameTables.AoWEngine.StartupDirectory, currentDir + "\\" + Name);
        DialogHosts.FileDlgWindow.FileDialog.Visible = false;
        Dialog.FSetupControl.hPickMap(Name);
    }

    internal static void OpenSelectedGame(FileDlgWindow Dialog)
    {
        Dialog.LoadSelectedGame(Dialog.FLastFileName);
    }

    internal static void ShowSaveGameEvent(FileDlgWindow Dialog, SaveGameEventLog Event, ExecuteDoneEvent Done)
    {
        Dialog.FDone = Done;
        Dialog.FSaveEvent = Event;
        Dialog.FSaveEvent.AddRef();
        Dialog.FMode = 1;
        Dialog.FileDialog.Visible = true;
        if (!IsEmpty(Event.FDirectory))
        {
            SetCurrentDir(Event.FDirectory);
        }

        Dialog.SFileName.Text = Event.FDefaultName;
        Dialog.Title.Text = Event.FTitle;
        Dialog.FindFiles();
    }

}
