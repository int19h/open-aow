// Translated from AoWFileDialog.pas.
using Aow.Engine.Nodes;
using Aow.Formats;

using Aow.Game.Audio;

using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Host;

using Aow.Game.Magic.Abilities;

using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// directory, last filename, save event, done callback, dialog mode
public partial class FileDialogWindow : DataModule
{
    public FileDialogWindow(Component owner) : base(owner)
    { }

    public Window FileDialog;
    public Button CancelBut;
    public Frame BCeltL;
    public Button Openbut;
    public Label Title;
    public ListBox FileLB;
    public Edit FileName;
    public Label ScenLbl;
    public ComboBox DriveCmb;
    public Label DriveL;
    public Button Close;
    public Button UpDirBtn;
    public Label PathL;
    public Image TitleIcon;
    public VerticalScrollBar DriveSB;
    public Image CeltM;
    public Image LPlayersI;
    public Panel LSavedInfo;
    public Image LSizeI;
    public Image LTownsI;
    public Image LTurnsI;
    public Label LPlayers;
    public Label LSize;
    public Label LTowns;
    public Label LTurns;
    public Image CeltL;
    public Image CeltR;
    public Frame TCeltL;
    public Image TCelt;
    public Image SPlayersI;
    public Panel NewInfo;
    public Image SSizeI;
    public Image STownsI;
    public Label SPlayers;
    public Label SSize;
    public Label STowns;
    public Panel NewPnl;
    public ListBox SaveLB;
    public Edit SFileName;
    public Label SavedInfo;
    public Panel SavePnl;
    public ListBox DateLB;
    public Button SaveBtn;
    public Button DateBtn;
    public VerticalScrollBar NSBar;
    public Panel DriveFr;
    public Panel TopPnl;
    public Panel PathFr;
    public Edit PathEdit;
    public Panel NewLBFr;
    public Panel NewInfo1;
    public Panel NewInfo2;
    public Panel NewInfo3;
    public Panel SaveLBFr;
    public VerticalScrollBar SSBar;
    public Panel SavedInfo1;
    public Panel SavedInfo2;
    public Panel SavedInfo3;
    public Panel SavedInfo4;
    public IvTranslator Translator;
    public string Directory = "";
    public string LastFileName = "";
    public SaveGameEventLog SaveEvent;
    public ExecuteDoneEvent Done;
    // is the data pointer of FDone
    public int Mode;

    // AoWFileDialog.pas:175
    // 0 load, 1 save, 2 select map
    public void FinishDialog()
    {
        ExecuteDoneEvent done;
        SaveGameEventLog eventValue;
        done = Done;
        eventValue = SaveEvent;
        Done = null;
        SaveEvent = null;
        if (done != null)
        {
            done(eventValue);
        }

        if (eventValue != null)
        {
            eventValue.Release();
        }
    }

    // AoWFileDialog.pas:132
    public void ExitForLoad(ExitGameMode mode, bool success)
    {
        if (success)
        {
            GameHost.DeferredSetupAction = 2;
        }
    }

    // AoWFileDialog.pas:137
    // The current game closes only after the file passes its preflight, so a refused file leaves
    // it intact (own-formats.md 2.7). AoWWinMan.LoadCampaign closes the map after its preflight and
    // TAoWEngine.LoadCampaign closes the campaign. The original closed both first.
    public void LoadSelectedGame(string name)
    {
        AoWHSMap map = default;
        if (DocumentKind.CampaignSave.HasExtension(ExtractFileExt(name)))
        {
            if (!GameTables.GameEngine.Demo)
            {
                // A refused campaign keeps the current map, which is not started again.
                var previous = AoWHSMap.Current;
                LoadCampaign(DialogHosts.General, name);
                if (AoWHSMap.Current != null && AoWHSMap.Current != previous)
                {
                    General.StartGame(DialogHosts.General);
                }
            }
        }
        else
        {
            map = (AoWHSMap)GameTables.GameEngine.CreatePreviewHsm(name, GameProfile.WorldMap);
            if (map != null)
            {
                GameTables.GameEngine.CloseCampaign();
                GameTables.GameEngine.CloseHsm(null);
                try
                {
                    if (map.ConnectionType != PlayerConnectionType.Network)
                    {
                        General.LoadGame(DialogHosts.General, name);
                        if (AoWHSMap.Current != null)
                        {
                            General.StartGame(DialogHosts.General);
                        }
                    }
                    else
                    {
                        DialogHosts.TitleScene.HideScene();
                        DialogHosts.General.HideWindows();
                        DialogHosts.TaskBar.TaskWindow.Visible = false;
                        var relativePath = ExtractRelativePath(GameTables.GameEngine.StartupDirectory, name);
                        DialogHosts.MultiWindow.Execute(relativePath);
                    }
                }
                finally
                {
                    map?.Dispose();
                }
            }
        }
    }

    // AoWFileDialog.pas:188
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (SaveEvent != null)
        {
            if (!SaveEvent.Mode.Contains(SaveGameEventLogModeElement.DisableCancel))
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
    public static bool ListsFile(int mode, bool campaign, string extension)
    {
        bool isCampaignSave = DocumentKind.CampaignSave.HasExtension(extension);
        bool isSave = DocumentKind.Save.HasExtension(extension);
        bool includeFile = false;
        includeFile = includeFile || mode == 1 && campaign && isCampaignSave;
        includeFile = includeFile || mode == 1 && !campaign && isSave;
        includeFile = includeFile || mode == 0 && (isSave || isCampaignSave);
        // { Original accepts .asg in every mode, including map selection. }
        includeFile = includeFile || mode == 2 && DocumentKind.Map.HasExtension(extension) || isSave;
        return includeFile;
    }

    // AoWFileDialog.pas:233
    public void FindFiles()
    {
        bool includeFile;
        string extension;
        string directory;
        SearchRec search = default;
        FileTime stamp;
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
        directory = GetCurrentDir();
        if (Copy(directory, directory.Length, 1) == "\\")
        {
            directory = Copy(directory, 1, directory.Length - 1);
        }

        if (FindFirst(directory + "\\**", 16, ref search) == 0)
        {
            do
            {
                if (search.Name != "." && search.Name != "..")
                {
                    if ((search.Attr & 16) == 16)
                    {
                        Directories.Add(search.Name);
                    }
                    else
                    {
                        extension = LowerCase(ExtractFileExt(search.Name));
                        includeFile = ListsFile(Mode, GameTables.GameEngine.Campaign != null, extension);
                        if (includeFile)
                        {
                            stamp = new FileTime();
                            stamp.Time = FileDateToDateTime(search.Time);
                            Files.AddObject(search.Name, stamp);
                        }
                    }
                }
            }
            while (!(FindNext(ref search) != 0));
        }

        FindClose(ref search);
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
        PathEdit.Text = directory + "\\";
        for (int I = 0, last = Directories.Count - 1; I <= last; I++)
        {
            DateLB.Items.Add("");
        }

        for (int I = 0, last = Files.Count - 1; I <= last; I++)
        {
            var dateTimeToString = DateTimeToStr(((FileTime)Files.Objects[I]).Time);
            var objectValue = Files.Objects[I];
            DateLB.Items.AddObject(dateTimeToString, objectValue);
        }

        RestoreSortOrder();
    }

    // AoWFileDialog.pas:296
    public void FileDlgWindowCreate()
    {
        Drives = new StringList();
        Directories = new StringList();
        Files = new StringList();
        Directory = GameTables.GameEngine.StartupDirectory + "Save\\";
        LastFileName = "";
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
        string path = "";
        string directory = "";
        DialogHosts.General.PlayButtonSound(0);
        var count = Directories.Count;
        if (FileLB.Selected < count && FileLB.Selected > -1)
        {
            directory = GetCurrentDir();
            if (Copy(directory, directory.Length, 1) == "\\")
            {
                directory = Copy(directory, 1, directory.Length - 1);
            }

            var item = FileLB.Items[FileLB.Selected];
            if (SetCurrentDir(directory + "\\" + item))
            {
                FindFiles();
            }

            FileLB.Selected = -1;
        }
        else
        {
            path = GetCurrentDir();
            if (path.Length > 0 && path[path.Length - 1] != 92)
            {
                path = path + "\\";
            }

            switch (Mode)
            {
                case 0:
                    if (!IsEmpty(SFileName.Text) && FileExists(SFileName.Text))
                    {
                        if (AoWHSMap.Current != null)
                        {
                            if (AoWHSMap.Current.CombatMap != null)
                            {
                                DialogHosts.General.ReturnToTitle();
                            }
                            else
                            {
                                if (GameTables.GameEngine.Campaign != null)
                                {
                                    if (CompareText(GameTables.GameEngine.Campaign.FileName, Directory + SFileName.Text) == 0)
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
                                    if (CompareText(AoWHSMap.Current.FileName, Directory + SFileName.Text) == 0)
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

                        if (AoWHSMap.Current == null || AoWHSMap.Current.ExitPending)
                        {
                            FileDialog.Visible = false;
                            LoadSelectedGame(path + SFileName.Text);
                        }

                        LastFileName = path + SFileName.Text;
                        Directory = path;
                    }

                    break;
                case 1:
                    if (!IsEmpty(SFileName.Text))
                    {
                        Directory = path;
                        if (Pos(".", SFileName.Text) != 0)
                        {
                            SFileName.Text = Copy(SFileName.Text, 1, Pos(".", SFileName.Text) - 1);
                        }

                        if (GameTables.GameEngine.Campaign != null)
                        {
                            FileDialog.Visible = false;
                            // The save takes the extension the host writes, beside an original of the same name
                            // (own-formats.md 4.2).
                            SFileName.Text = FileBoundary.WriteName(SFileName.Text + DocumentKind.CampaignSave.OriginalExtension);
                            SaveCampaign(DialogHosts.General, SFileName.Text);
                        }
                        else
                        {
                            if (SaveEvent != null)
                            {
                                SaveEvent.FileName = FileBoundary.WriteName(SFileName.Text + DocumentKind.Save.OriginalExtension, Directory);
                                FileDialog.Visible = false;
                            }
                            else
                            {
                                FileDialog.Visible = false;
                                SFileName.Text = FileBoundary.WriteName(SFileName.Text + DocumentKind.Save.OriginalExtension);
                                SaveGame(DialogHosts.General, SFileName.Text);
                            }
                        }

                        LastFileName = SFileName.Text;
                        Directory = path;
                    }

                    break;
                case 2:
                    if (!IsEmpty(FileName.Text) && FileExists(FileName.Text))
                    {
                        PickScenarioMap(DialogHosts.ScenarioSetup, FileName.Text);
                        LastFileName = FileName.Text;
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
    public void FileLBChange(ListBox sender)
    {
        AoWHSMap map;
        ListBox list;
        list = sender;
        FileLB.Selected = list.Selected;
        SaveLB.Selected = list.Selected;
        DateLB.Selected = list.Selected;
        if (Directories.Count - 1 < this.FileLB.Selected)
        {
            var item = FileLB.Items[FileLB.Selected];
            SFileName.Text = item;
            if (!DocumentKind.CampaignSave.HasExtension(ExtractFileExt(SFileName.Text)))
            {
                map = (AoWHSMap)GameTables.GameEngine.CreatePreviewHsm(SFileName.Text, GameProfile.WorldMap);
                if (map != null)
                {
                    var count = map.Players.GetCount();
                    LPlayers.Text = IntToStr(count - 1);
                    switch (map.MapContainer.Width)
                    {
                        case 48:
                            var translatedText = AbilityRules.TranslateResourceString("S");
                            LSize.Text = translatedText;
                            break;
                        case 64:
                            var translateResourceString2 = AbilityRules.TranslateResourceString("M");
                            LSize.Text = translateResourceString2;
                            break;
                        case 96:
                            var translateResourceString3 = AbilityRules.TranslateResourceString("L");
                            LSize.Text = translateResourceString3;
                            break;
                        case 127:
                        case 128:
                            var extraLargeMapLetter = AbilityRules.TranslateResourceString(s_extraLargeMapLetter);
                            LSize.Text = extraLargeMapLetter;
                            break;
                        default:
                            LSize.Text = "?";
                            break;
                    }

                    LTowns.Text = IntToStr(map.StructureControl.CityCount);
                    LTurns.Text = IntToStr(map.Day);
                    map?.Dispose();
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
    public void FileLBDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        if (Directories.Count > drawItem)
        {
            DialogHosts.UIGraphics.LBIcons[0].ShowClipped(drawRectangle.Left + 2, drawRectangle.Top + 1, lockedSurface);
        }
    }

    // AoWFileDialog.pas:462
    public void FileDialogShow()
    {
        switch (Mode)
        {
            case 0:
                SetCurrentDir(Directory);
                var fileName = ExtractFileName(LastFileName);
                SFileName.Text = fileName;
                FileName.Text = SFileName.Text;
                var selectAFileToLoad = AbilityRules.TranslateResourceString(s_selectFileToLoadText);
                DialogHosts.FileDialogWindow.Title.Text = selectAFileToLoad;
                DialogHosts.FileDialogWindow.SFileName.Editable = false;
                var open = AbilityRules.TranslateResourceString(s_openText);
                DialogHosts.FileDialogWindow.Openbut.Text = open;
                SavePnl.Visible = true;
                NewPnl.Visible = false;
                break;
            case 1:
                SetCurrentDir(Directory);
                var fileName2 = ExtractFileName(LastFileName);
                SFileName.Text = fileName2;
                if (Pos(".", SFileName.Text) != 0)
                {
                    SFileName.Text = Copy(SFileName.Text, 1, Pos(".", SFileName.Text) - 1);
                }

                if (!IsEmpty(SFileName.Text))
                {
                    // The default name shows the file the save writes (own-formats.md 4.2).
                    if (GameTables.GameEngine.Campaign != null)
                    {
                        SFileName.Text = FileBoundary.WriteName(SFileName.Text + DocumentKind.CampaignSave.OriginalExtension);
                    }
                    else
                    {
                        SFileName.Text = FileBoundary.WriteName(SFileName.Text + DocumentKind.Save.OriginalExtension);
                    }
                }

                FileName.Text = SFileName.Text;
                var saveGameAs = AbilityRules.TranslateResourceString(s_saveGameAsText);
                DialogHosts.FileDialogWindow.Title.Text = saveGameAs;
                DialogHosts.FileDialogWindow.SFileName.Editable = true;
                var save = AbilityRules.TranslateResourceString(GameText.s_saveText);
                DialogHosts.FileDialogWindow.Openbut.Text = save;
                SavePnl.Visible = true;
                NewPnl.Visible = false;
                break;
            case 2:
                var lastExtension = ExtractFileExt(LastFileName);
                if (lastExtension == DocumentKind.Map.OriginalExtension || lastExtension == DocumentKind.Map.OwnExtension)
                {
                    var fileName3 = ExtractFileName(LastFileName);
                    FileName.Text = fileName3;
                }
                else
                {
                    FileName.Text = "";
                }

                SetCurrentDir(GameTables.GameEngine.StartupDirectory + "Scenario\\Custom\\");
                var selectAMap = AbilityRules.TranslateResourceString(s_selectMapText);
                DialogHosts.FileDialogWindow.Title.Text = selectAMap;
                var select = AbilityRules.TranslateResourceString(s_selectText);
                DialogHosts.FileDialogWindow.Openbut.Text = select;
                SavePnl.Visible = false;
                NewPnl.Visible = true;
                break;
        }

        DialogHosts.FileDialogWindow.FindDrives();
    }

    // AoWFileDialog.pas:502
    public void MSBarChange(VerticalScrollBar sender)
    {
        VerticalScrollBar bar;
        bar = sender;
        FileLB.ListOffset = bar.Position;
        SaveLB.ListOffset = bar.Position;
        DateLB.ListOffset = bar.Position;
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
        string selected = "";
        if (FileLB.Selected != -1)
        {
            selected = FileLB.Items[FileLB.Selected];
        }

        for (int I = 0, last = Directories.Count - 1; I <= last; I++)
        {
            SaveLB.Items.Delete(0);
            DateLB.Items.Delete(0);
        }

        if (Mode == 1 || Mode == 0)
        {
            StringSorting.SortStringsAscending(SaveLB.Items, [DateLB.Items, FileLB.Items]);
            FileLB.Items.Assign(SaveLB.Items);
        }
        else
        {
            if (Mode == 2)
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

        if (!IsEmpty(selected))
        {
            var indexOf = FileLB.Items.IndexOf(selected);
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
        string selected = "";
        if (FileLB.Selected != -1)
        {
            selected = FileLB.Items[FileLB.Selected];
        }

        if (Mode == 1 || Mode == 0)
        {
            for (int I = 0, last = SaveLB.Items.Count - 1; I <= last; I++)
            {
                var objectValue = DateLB.Items.Objects[I];
                SaveLB.Items.Objects[I] = objectValue;
            }

            StringSorting.SortStrings(SaveLB.Items, CompareFileDates, [DateLB.Items, FileLB.Items]);
        }
        else
        {
            if (Mode == 2)
            {
                for (int I = 0, last = FileLB.Items.Count - 1; I <= last; I++)
                {
                    var objectValue = DateLB.Items.Objects[I];
                    FileLB.Items.Objects[I] = objectValue;
                }

                StringSorting.SortStrings(FileLB.Items, CompareFileDates, [DateLB.Items, SaveLB.Items]);
            }
        }

        if (!IsEmpty(selected))
        {
            var indexOf = FileLB.Items.IndexOf(selected);
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
    public void FileDialogKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
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

    private static readonly string s_extraLargeMapLetter = "XL";

    private static readonly string s_openText = "Open";

    private static readonly string s_saveGameAsText = "Save game as...";

    private static readonly string s_selectFileToLoadText = "Select a File to Load...";

    private static readonly string s_selectMapText = "Select a Map...";

    private static readonly string s_selectText = "Select";

    internal static StringList Drives;

    internal static StringList Directories;

    internal static StringList Files;

    internal static int CompareFileDates(string s1, string s2, object o1, object o2)
    {
        int result;
        if (o1 == null && o2 == null)
        {
            result = CompareText(s1, s2);
        }
        else
        {
            if (o1 == null)
            {
                result = -1;
            }
            else
            {
                if (o2 == null)
                {
                    result = 1;
                }
                else
                {
                    if ((double)((FileTime)o2).Time > ((FileTime)o1).Time)
                    {
                        result = 1;
                    }
                    else
                    {
                        if ((double)((FileTime)o2).Time < ((FileTime)o1).Time)
                        {
                            result = -1;
                        }
                        else
                        {
                            result = 0;
                        }
                    }
                }
            }
        }

        return result;
    }

    private static void LoadCampaign(General controller, string name)
    {
        byte[] image = FileBoundary.Load(name, GameProfile.Campaign);
        if (image == null)
        {
            return;
        }

        if (AoWHSMap.Current != null)
        {
            GameTables.GameEngine.CloseHsm(null);
        }

        GameTables.GameEngine.LoadCampaign(name, image);
        if (GameTables.GameEngine.Campaign != null)
        {
            if (GameTables.GameEngine.Campaign.ScenarioStarted)
            {
                var loading = AbilityRules.TranslateResourceString(LayerText.s_loadingText);
                var fileName = ExtractFileName(name);
                DialogHosts.ProgressDialog.Title.Text = loading + " " + fileName + "...";
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
                GameTables.GameEngine.LoadCampaignMap((_, message, progression) => controller.MapProgress(message, progression));
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

    private static void SaveGame(General controller, string name)
    {
        AoWHSMap.Current.SaveGame(name);
    }

    private static void SaveCampaign(General controller, string name)
    {
        // { Original campaign-save command uses the map save routine. }
        AoWHSMap.Current.SaveGame(name);
    }

    private static void PickScenarioMap(ScenarioSetup dialog, string name)
    {
        var currentDirectory = GetCurrentDir();
        name = ExtractRelativePath(GameTables.GameEngine.StartupDirectory, currentDirectory + "\\" + name);
        DialogHosts.FileDialogWindow.FileDialog.Visible = false;
        dialog.SetupControl.PickMap(name);
    }

    internal static void OpenSelectedGame(FileDialogWindow dialog)
    {
        dialog.LoadSelectedGame(dialog.LastFileName);
    }

    internal static void ShowSaveGameEvent(FileDialogWindow dialog, SaveGameEventLog eventValue, ExecuteDoneEvent done)
    {
        dialog.Done = done;
        dialog.SaveEvent = eventValue;
        dialog.SaveEvent.AddRef();
        dialog.Mode = 1;
        dialog.FileDialog.Visible = true;
        if (!IsEmpty(eventValue.Directory))
        {
            SetCurrentDir(eventValue.Directory);
        }

        dialog.SFileName.Text = eventValue.DefaultName;
        dialog.Title.Text = eventValue.Title;
        dialog.FindFiles();
    }

}
