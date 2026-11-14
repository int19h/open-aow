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
using Aow.Rtl.Host;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// directory, last filename, save event, done callback, dialog mode
public partial class FileDialogWindow : UIModule
{
    public FileDialogWindow(UIContext ui) : base(ui)
    { }

    public Window Window;
    public Button OpenButton;
    public Label TitleLabel;
    public ListBox<FileTime> FileList;
    public Edit FileNameEdit;
    public ComboBox DriveComboBox;
    public Label SavedPlayersLabel;
    public Label SavedSizeLabel;
    public Label SavedTownsLabel;
    public Label SavedTurnsLabel;
    public Label ScenarioPlayersLabel;
    public Label ScenarioSizeLabel;
    public Label ScenarioTownsLabel;
    public Panel NewPanel;
    public ListBox<FileTime> SaveList;
    public Edit SaveFileNameEdit;
    public Panel SavePanel;
    public ListBox<FileTime> DateList;
    public Edit PathEdit;
    public Panel SaveListFrame;
    public VerticalScrollBar SaveScrollBar;

    public string Directory { get; set; } = "";
    public string LastFileName { get; set; } = "";

    private SaveGameEventLog _saveEvent;
    private event ExecuteDoneEvent Done;

    // is the data pointer of FDone
    public int Mode { get; set; }

    // AoWFileDialog.pas:175
    // 0 load, 1 save, 2 select map
    public void FinishDialog()
    {
        ExecuteDoneEvent done;
        SaveGameEventLog eventValue;
        done = Done;
        eventValue = _saveEvent;
        Done = null;
        _saveEvent = null;
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
        StrategicMap map = default;
        if (DocumentKind.CampaignSave.HasExtension(ExtractFileExt(name)))
        {
            if (!UI.Engine.Demo)
            {
                // A refused campaign keeps the current map, which is not started again.
                var previous = StrategicMap.Current;
                LoadCampaign(UI.General, name);
                if (StrategicMap.Current != null && StrategicMap.Current != previous)
                {
                    General.StartGame(UI.General);
                }
            }
        }
        else
        {
            map = (StrategicMap)UI.Engine.CreatePreviewHsm(name, GameProfile.WorldMap);
            if (map != null)
            {
                UI.Engine.CloseCampaign();
                UI.Engine.CloseHsm(null);
                try
                {
                    if (map.ConnectionType != PlayerConnectionType.Network)
                    {
                        General.LoadGame(UI.General, name);
                        if (StrategicMap.Current != null)
                        {
                            General.StartGame(UI.General);
                        }
                    }
                    else
                    {
                        UI.TitleScene.HideScene();
                        UI.General.HideWindows();
                        UI.TaskBar.TaskWindow.Visible = false;
                        var relativePath = ExtractRelativePath(UI.Engine.StartupDirectory, name);
                        UI.MultiWindow.Execute(relativePath);
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
        UI.General.PlayButtonSound(0);
        if (_saveEvent != null)
        {
            if (!_saveEvent.Mode.Contains(SaveGameEventLogModeElement.DisableCancel))
            {
                Window.Visible = false;
            }
        }
        else
        {
            Window.Visible = false;
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
        FileTime stamp;
        for (int I = 0, last = DateList.Items.Count - 1; I <= last; I++)
        {
            if (DateList.Items.Objects[I] != null)
            {
                (DateList.Items.Objects[I] as IDisposable)?.Dispose();
            }
        }

        Directories.Clear();
        Files.Clear();
        ScenarioPlayersLabel.Text = "";
        ScenarioSizeLabel.Text = "";
        ScenarioTownsLabel.Text = "";
        SavedPlayersLabel.Text = "";
        SavedSizeLabel.Text = "";
        SavedTownsLabel.Text = "";
        SavedTurnsLabel.Text = "";
        directory = GetCurrentDir();
        if (Copy(directory, directory.Length, 1) == "\\")
        {
            directory = Copy(directory, 1, directory.Length - 1);
        }

        foreach (var entry in RtlUnit.Files.FindEntries(directory + "\\**", attributes: 16))
        {
            if (entry.Name != "." && entry.Name != "..")
            {
                if ((entry.Attribute & 16) == 16)
                {
                    Directories.Add(entry.Name);
                }
                else
                {
                    extension = LowerCase(ExtractFileExt(entry.Name));
                    includeFile = ListsFile(Mode, UI.Engine.Campaign != null, extension);
                    if (includeFile)
                    {
                        stamp = new FileTime();
                        stamp.Time = FileDateToDateTime(entry.FileDate);
                        Files.AddObject(entry.Name, stamp);
                    }
                }
            }
        }

        FileList.Selected = -1;
        SaveList.Selected = -1;
        DateList.Selected = -1;
        FileList.ListOffset = 0;
        SaveList.ListOffset = 0;
        DateList.ListOffset = 0;
        FileList.Items.Clear();
        SaveList.Items.Clear();
        DateList.Items.Clear();
        Directories.Sort();
        Files.Sort();
        FileList.Items.AddStrings(Directories);
        FileList.Items.AddStrings(Files);
        SaveList.Items.AddStrings(FileList.Items);
        PathEdit.Text = directory + "\\";
        for (int I = 0, last = Directories.Count - 1; I <= last; I++)
        {
            DateList.Items.Add("");
        }

        for (int I = 0, last = Files.Count - 1; I <= last; I++)
        {
            var dateTimeToString = DateTimeToStr(Files.Objects[I].Time);
            var objectValue = Files.Objects[I];
            DateList.Items.AddObject(dateTimeToString, objectValue);
        }

        RestoreSortOrder();
    }

    // AoWFileDialog.pas:296
    public void FileDialogWindowCreate()
    {
        Drives = new StringList();
        Directories = new StringList();
        Files = new StringList<FileTime>();
        Directory = UI.Engine.StartupDirectory + "Save\\";
        LastFileName = "";
    }

    // AoWFileDialog.pas:305
    public void FileDialogWindowDestroy()
    {
        for (int I = 0, last = DateList.Items.Count - 1; I <= last; I++)
        {
            if (DateList.Items.Objects[I] != null)
            {
                (DateList.Items.Objects[I] as IDisposable)?.Dispose();
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
        UI.General.PlayButtonSound(0);
        var count = Directories.Count;
        if (FileList.Selected < count && FileList.Selected > -1)
        {
            directory = GetCurrentDir();
            if (Copy(directory, directory.Length, 1) == "\\")
            {
                directory = Copy(directory, 1, directory.Length - 1);
            }

            var item = FileList.Items[FileList.Selected];
            if (SetCurrentDir(directory + "\\" + item))
            {
                FindFiles();
            }

            FileList.Selected = -1;
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
                    if (!IsEmpty(SaveFileNameEdit.Text) && FileExists(SaveFileNameEdit.Text))
                    {
                        if (StrategicMap.Current != null)
                        {
                            if (StrategicMap.Current.CombatMap != null)
                            {
                                UI.General.ReturnToTitle();
                            }
                            else
                            {
                                if (UI.Engine.Campaign != null)
                                {
                                    if (CompareText(UI.Engine.Campaign.FileName, Directory + SaveFileNameEdit.Text) == 0)
                                    {
                                        StrategicMap.Current.ExitGame(ExitGameMode.Reload, ExitForLoad);
                                    }
                                    else
                                    {
                                        StrategicMap.Current.ExitGame(ExitGameMode.Load, ExitForLoad);
                                    }
                                }
                                else
                                {
                                    if (CompareText(StrategicMap.Current.FileName, Directory + SaveFileNameEdit.Text) == 0)
                                    {
                                        StrategicMap.Current.ExitGame(ExitGameMode.Reload, ExitForLoad);
                                    }
                                    else
                                    {
                                        StrategicMap.Current.ExitGame(ExitGameMode.Load, ExitForLoad);
                                    }
                                }
                            }
                        }

                        if (StrategicMap.Current == null || StrategicMap.Current.ExitPending)
                        {
                            Window.Visible = false;
                            LoadSelectedGame(path + SaveFileNameEdit.Text);
                        }

                        LastFileName = path + SaveFileNameEdit.Text;
                        Directory = path;
                    }

                    break;
                case 1:
                    if (!IsEmpty(SaveFileNameEdit.Text))
                    {
                        Directory = path;
                        if (Pos(".", SaveFileNameEdit.Text) != 0)
                        {
                            SaveFileNameEdit.Text = Copy(SaveFileNameEdit.Text, 1, Pos(".", SaveFileNameEdit.Text) - 1);
                        }

                        if (UI.Engine.Campaign != null)
                        {
                            Window.Visible = false;
                            // The save takes the extension the host writes, beside an original of the same name
                            // (own-formats.md 4.2).
                            SaveFileNameEdit.Text = FileBoundary.WriteName(SaveFileNameEdit.Text + DocumentKind.CampaignSave.OriginalExtension);
                            SaveCampaign(UI.General, SaveFileNameEdit.Text);
                        }
                        else
                        {
                            if (_saveEvent != null)
                            {
                                _saveEvent.FileName = FileBoundary.WriteName(SaveFileNameEdit.Text + DocumentKind.Save.OriginalExtension, Directory);
                                Window.Visible = false;
                            }
                            else
                            {
                                Window.Visible = false;
                                SaveFileNameEdit.Text = FileBoundary.WriteName(SaveFileNameEdit.Text + DocumentKind.Save.OriginalExtension);
                                SaveGame(UI.General, SaveFileNameEdit.Text);
                            }
                        }

                        LastFileName = SaveFileNameEdit.Text;
                        Directory = path;
                    }

                    break;
                case 2:
                    if (!IsEmpty(FileNameEdit.Text) && FileExists(FileNameEdit.Text))
                    {
                        PickScenarioMap(UI.ScenarioSetup, FileNameEdit.Text);
                        LastFileName = FileNameEdit.Text;
                    }

                    break;
            }
        }
    }

    // AoWFileDialog.pas:394
    public void UpDirBtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (SetCurrentDir(".."))
        {
            FindFiles();
        }
    }

    // AoWFileDialog.pas:400
    public void DriveCmbChange()
    {
        var item = DriveComboBox.Items[DriveComboBox.Selected];
        if (SetCurrentDir(Copy(DriveComboBox.Items[DriveComboBox.Selected], item.Length - 3, 3)))
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
        StrategicMap map;
        ListBox list;
        list = sender;
        FileList.Selected = list.Selected;
        SaveList.Selected = list.Selected;
        DateList.Selected = list.Selected;
        if (Directories.Count - 1 < this.FileList.Selected)
        {
            var item = FileList.Items[FileList.Selected];
            SaveFileNameEdit.Text = item;
            if (!DocumentKind.CampaignSave.HasExtension(ExtractFileExt(SaveFileNameEdit.Text)))
            {
                map = (StrategicMap)UI.Engine.CreatePreviewHsm(SaveFileNameEdit.Text, GameProfile.WorldMap);
                if (map != null)
                {
                    var count = map.Players.GetCount();
                    SavedPlayersLabel.Text = IntToStr(count - 1);
                    switch (map.MapContainer.Width)
                    {
                        case 48:
                            var translatedText = UI.Engine.TranslateResourceString("S");
                            SavedSizeLabel.Text = translatedText;
                            break;
                        case 64:
                            var translateResourceString2 = UI.Engine.TranslateResourceString("M");
                            SavedSizeLabel.Text = translateResourceString2;
                            break;
                        case 96:
                            var translateResourceString3 = UI.Engine.TranslateResourceString("L");
                            SavedSizeLabel.Text = translateResourceString3;
                            break;
                        case 127:
                        case 128:
                            var extraLargeMapLetter = UI.Engine.TranslateResourceString(s_extraLargeMapLetter);
                            SavedSizeLabel.Text = extraLargeMapLetter;
                            break;
                        default:
                            SavedSizeLabel.Text = "?";
                            break;
                    }

                    SavedTownsLabel.Text = IntToStr(map.StructureControl.CityCount);
                    SavedTurnsLabel.Text = IntToStr(map.Day);
                    map?.Dispose();
                }
                else
                {
                    SavedPlayersLabel.Text = "";
                    SavedSizeLabel.Text = "";
                    SavedTownsLabel.Text = "";
                    SavedTurnsLabel.Text = "";
                }
            }
            else
            {
                SavedPlayersLabel.Text = "";
                SavedSizeLabel.Text = "";
                SavedTownsLabel.Text = "";
                SavedTurnsLabel.Text = "";
            }
        }
        else
        {
            SavedPlayersLabel.Text = "";
            SavedSizeLabel.Text = "";
            SavedTownsLabel.Text = "";
            SavedTurnsLabel.Text = "";
        }

        FileNameEdit.Text = SaveFileNameEdit.Text;
        ScenarioPlayersLabel.Text = SavedPlayersLabel.Text;
        ScenarioSizeLabel.Text = SavedSizeLabel.Text;
        ScenarioTownsLabel.Text = SavedTownsLabel.Text;
    }

    // AoWFileDialog.pas:586
    public void FileLBDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        if (Directories.Count > drawItem)
        {
            UI.Graphics.ListIcons[0].ShowClipped(drawRectangle.Left + 2, drawRectangle.Top + 1, lockedSurface);
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
                SaveFileNameEdit.Text = fileName;
                FileNameEdit.Text = SaveFileNameEdit.Text;
                var selectAFileToLoad = UI.Engine.TranslateResourceString(s_selectFileToLoadText);
                UI.FileDialogWindow.TitleLabel.Text = selectAFileToLoad;
                UI.FileDialogWindow.SaveFileNameEdit.Editable = false;
                var open = UI.Engine.TranslateResourceString(s_openText);
                UI.FileDialogWindow.OpenButton.Text = open;
                SavePanel.Visible = true;
                NewPanel.Visible = false;
                break;
            case 1:
                SetCurrentDir(Directory);
                var fileName2 = ExtractFileName(LastFileName);
                SaveFileNameEdit.Text = fileName2;
                if (Pos(".", SaveFileNameEdit.Text) != 0)
                {
                    SaveFileNameEdit.Text = Copy(SaveFileNameEdit.Text, 1, Pos(".", SaveFileNameEdit.Text) - 1);
                }

                if (!IsEmpty(SaveFileNameEdit.Text))
                {
                    // The default name shows the file the save writes (own-formats.md 4.2).
                    if (UI.Engine.Campaign != null)
                    {
                        SaveFileNameEdit.Text = FileBoundary.WriteName(SaveFileNameEdit.Text + DocumentKind.CampaignSave.OriginalExtension);
                    }
                    else
                    {
                        SaveFileNameEdit.Text = FileBoundary.WriteName(SaveFileNameEdit.Text + DocumentKind.Save.OriginalExtension);
                    }
                }

                FileNameEdit.Text = SaveFileNameEdit.Text;
                var saveGameAs = UI.Engine.TranslateResourceString(s_saveGameAsText);
                UI.FileDialogWindow.TitleLabel.Text = saveGameAs;
                UI.FileDialogWindow.SaveFileNameEdit.Editable = true;
                var save = UI.Engine.TranslateResourceString(GameText.SaveText);
                UI.FileDialogWindow.OpenButton.Text = save;
                SavePanel.Visible = true;
                NewPanel.Visible = false;
                break;
            case 2:
                var lastExtension = ExtractFileExt(LastFileName);
                if (lastExtension == DocumentKind.Map.OriginalExtension || lastExtension == DocumentKind.Map.OwnExtension)
                {
                    var fileName3 = ExtractFileName(LastFileName);
                    FileNameEdit.Text = fileName3;
                }
                else
                {
                    FileNameEdit.Text = "";
                }

                SetCurrentDir(UI.Engine.StartupDirectory + "Scenario\\Custom\\");
                var selectAMap = UI.Engine.TranslateResourceString(s_selectMapText);
                UI.FileDialogWindow.TitleLabel.Text = selectAMap;
                var select = UI.Engine.TranslateResourceString(s_selectText);
                UI.FileDialogWindow.OpenButton.Text = select;
                SavePanel.Visible = false;
                NewPanel.Visible = true;
                break;
        }

        UI.FileDialogWindow.FindDrives();
    }

    // AoWFileDialog.pas:502
    public void MSBarChange(VerticalScrollBar sender)
    {
        VerticalScrollBar bar;
        bar = sender;
        FileList.ListOffset = bar.Position;
        SaveList.ListOffset = bar.Position;
        DateList.ListOffset = bar.Position;
    }

    // AoWFileDialog.pas:511
    public void SFileNameReturn()
    {
        OpenbutClick();
    }

    // AoWFileDialog.pas:516
    public void SFileNameChange()
    {
        FileList.Selected = -1;
        FileLBChange(FileList);
    }

    // AoWFileDialog.pas:522
    public void SaveBtnClick()
    {
        UI.General.PlayButtonSound(0);
        RememberSortByDate(false);
        SortByName();
    }

    internal void SortByName()
    {
        string selected = "";
        if (FileList.Selected != -1)
        {
            selected = FileList.Items[FileList.Selected];
        }

        for (int I = 0, last = Directories.Count - 1; I <= last; I++)
        {
            SaveList.Items.Delete(0);
            DateList.Items.Delete(0);
        }

        if (Mode == 1 || Mode == 0)
        {
            StringSorting.SortStringsAscending(SaveList.Items, [DateList.Items, FileList.Items]);
            FileList.Items.Assign(SaveList.Items);
        }
        else
        {
            if (Mode == 2)
            {
                StringSorting.SortStringsAscending(FileList.Items, [DateList.Items, SaveList.Items]);
            }
        }

        for (int I = 0, last = Directories.Count - 1; I <= last; I++)
        {
            var directory = Directories[I];
            SaveList.Items.Insert(I, directory);
            DateList.Items.Insert(I, "");
        }

        if (!IsEmpty(selected))
        {
            var indexOf = FileList.Items.IndexOf(selected);
            FileList.Selected = indexOf;
        }

        FileLBChange(FileList);
    }

    // AoWFileDialog.pas:549
    public void DateBtnClick()
    {
        UI.General.PlayButtonSound(0);
        RememberSortByDate(true);
        SortByDate();
    }

    internal void SortByDate()
    {
        string selected = "";
        if (FileList.Selected != -1)
        {
            selected = FileList.Items[FileList.Selected];
        }

        if (Mode == 1 || Mode == 0)
        {
            for (int I = 0, last = SaveList.Items.Count - 1; I <= last; I++)
            {
                var objectValue = DateList.Items.Objects[I];
                SaveList.Items.Objects[I] = objectValue;
            }

            StringSorting.SortStrings(SaveList.Items, CompareFileDates, [DateList.Items, FileList.Items]);
        }
        else
        {
            if (Mode == 2)
            {
                for (int I = 0, last = FileList.Items.Count - 1; I <= last; I++)
                {
                    var objectValue = DateList.Items.Objects[I];
                    FileList.Items.Objects[I] = objectValue;
                }

                StringSorting.SortStrings(FileList.Items, CompareFileDates, [DateList.Items, SaveList.Items]);
            }
        }

        if (!IsEmpty(selected))
        {
            var indexOf = FileList.Items.IndexOf(selected);
            FileList.Selected = indexOf;
        }

        FileLBChange(FileList);
    }

    // AoWFileDialog.pas:570
    public void SSBarChange()
    {
        SaveList.ListOffset = SaveScrollBar.Position;
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
            Window.Visible = false;
        }
    }

    public void FindDrives()
    {
        Drives.Clear();
        string drive = RtlUnit.Files.VirtualDrive + "\\";
        string label = OperatingSystem.IsWindows() ? new DriveInfo(drive).VolumeLabel + " " : "";
        Drives.Add($"{label}({drive})");
        DriveComboBox.Items.Clear();
        DriveComboBox.Items.AddStrings(Drives);
        DriveComboBox.Selected = 0;
        FindFiles();
    }

    private static readonly string s_extraLargeMapLetter = "XL";

    private static readonly string s_openText = "Open";

    private static readonly string s_saveGameAsText = "Save game as...";

    private static readonly string s_selectFileToLoadText = "Select a File to Load...";

    private static readonly string s_selectMapText = "Select a Map...";

    private static readonly string s_selectText = "Select";

    internal static StringList Drives { get; set; }

    internal static StringList Directories { get; set; }

    internal static StringList<FileTime> Files { get; set; }

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
        var tree = FileBoundary.Open(name, profile: GameProfile.Campaign);
        if (tree == null)
        {
            return;
        }

        if (StrategicMap.Current != null)
        {
            controller.UI.Engine.CloseHsm(null);
        }

        controller.UI.Engine.LoadCampaign(name, tree: tree);
        if (controller.UI.Engine.Campaign != null)
        {
            if (controller.UI.Engine.Campaign.ScenarioStarted)
            {
                var loading = controller.UI.Engine.TranslateResourceString(LayerText.LoadingText);
                var fileName = ExtractFileName(name);
                controller.UI.ProgressDialog.TitleLabel.Text = loading + " " + fileName + "...";
                controller.UI.ProgressDialog.StatusLabel.Text = "";
                controller.UI.ProgressDialog.ProgressBar.Position = 0;
                controller.UI.ProgressDialog.ProgressBar.Max = 100;
                controller.UI.ProgressDialog.Window.Visible = true;
                if (!controller.UI.TitleScene.Window.Visible)
                {
                    controller.UI.General.Manager.OverlayPriority = 5000;
                }

                AudioHost.SongPlayer.Stop(false);
                AudioHost.SongPlayer.Songs.Clear();
                AudioPlayback.FadeMusicVolume(0, 1);
                controller.UI.Engine.LoadCampaignMap((_, message, progression) => controller.MapProgress(message, progression));
                controller.UI.General.Manager.OverlayPriority = 0;
                controller.UI.ProgressDialog.Window.Visible = false;
            }
            else
            {
                controller.UI.TitleScene.HideScene();
                WorldMap.ShowCampaignMap(controller.UI.WorldMap);
            }
        }
    }

    private static void SaveGame(General controller, string name)
    {
        StrategicMap.Current.SaveGame(name);
    }

    private static void SaveCampaign(General controller, string name)
    {
        // { Original campaign-save command uses the map save routine. }
        StrategicMap.Current.SaveGame(name);
    }

    private static void PickScenarioMap(ScenarioSetup dialog, string name)
    {
        var currentDirectory = GetCurrentDir();
        name = ExtractRelativePath(dialog.UI.Engine.StartupDirectory, currentDirectory + "\\" + name);
        dialog.UI.FileDialogWindow.Window.Visible = false;
        dialog.SetupControl.PickMap(name);
    }

    internal static void OpenSelectedGame(FileDialogWindow dialog)
    {
        dialog.LoadSelectedGame(dialog.LastFileName);
    }

    internal static void ShowSaveGameEvent(FileDialogWindow dialog, SaveGameEventLog eventValue, ExecuteDoneEvent done)
    {
        dialog.Done = done;
        dialog._saveEvent = eventValue;
        dialog._saveEvent.AddRef();
        dialog.Mode = 1;
        dialog.Window.Visible = true;
        if (!IsEmpty(eventValue.Directory))
        {
            SetCurrentDir(eventValue.Directory);
        }

        dialog.SaveFileNameEdit.Text = eventValue.DefaultName;
        dialog.TitleLabel.Text = eventValue.Title;
        dialog.FindFiles();
    }

}
