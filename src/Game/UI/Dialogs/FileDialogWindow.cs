#nullable disable
// Translated from AoWFileDialog.pas.
using System.Globalization;
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
using static Aow.Rtl.DelphiDateTime;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.UI.Dialogs;

// directory, last filename, save event, done callback, dialog mode
public sealed partial class FileDialogWindow(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Button OpenButton { get; private set; }
    public Label TitleLabel { get; private set; }
    public ListBox<FileTime> FileList { get; set; }
    public Edit FileNameEdit { get; set; }
    public ComboBox DriveComboBox { get; private set; }
    public Label SavedPlayersLabel { get; set; }
    public Label SavedSizeLabel { get; set; }
    public Label SavedTownsLabel { get; set; }
    public Label SavedTurnsLabel { get; set; }
    public Label ScenarioPlayersLabel { get; set; }
    public Label ScenarioSizeLabel { get; set; }
    public Label ScenarioTownsLabel { get; set; }
    public Panel NewPanel { get; private set; }
    public ListBox<FileTime> SaveList { get; set; }
    public Edit SaveFileNameEdit { get; set; }
    public Panel SavePanel { get; private set; }
    public ListBox<FileTime> DateList { get; set; }
    public Edit PathEdit { get; set; }
    public Panel SaveListFrame { get; private set; }
    public VerticalScrollBar SaveScrollBar { get; private set; }

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
        ExecuteDoneEvent done = Done;
        SaveGameEventLog eventValue = _saveEvent;
        Done = null;
        _saveEvent = null;
        done?.Invoke(eventValue);

        _ = (eventValue?.Release());
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
        if (DocumentKind.CampaignSave.HasExtension(ExtractFileExt(name)))
        {
            if (!UI.Engine.Demo)
            {
                // A refused campaign keeps the current map, which is not started again.
                StrategicMap previous = StrategicMap.Current;
                LoadCampaign(UI.General, name);
                if (StrategicMap.Current is not null && StrategicMap.Current != previous)
                {
                    General.StartGame(UI.General);
                }
            }
        }
        else
        {
            StrategicMap map = UI.Engine.CreatePreviewHsm<StrategicMap>(name, profile: GameProfile.WorldMap);
            if (map is not null)
            {
                UI.Engine.CloseCampaign();
                UI.Engine.CloseHsm(null);
                try
                {
                    if (map.ConnectionType != PlayerConnectionType.Network)
                    {
                        General.LoadGame(UI.General, name);
                        if (StrategicMap.Current is not null)
                        {
                            General.StartGame(UI.General);
                        }
                    }
                    else
                    {
                        UI.TitleScene.HideScene();
                        UI.General.HideWindows();
                        UI.TaskBar.TaskWindow.Visible = false;
                        string relativePath = RtlHost.Files.ExtractRelativePath(UI.Engine.StartupDirectory, destination: name);
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
        if (_saveEvent is not null)
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
        includeFile = includeFile || (mode == 1 && campaign && isCampaignSave);
        includeFile = includeFile || (mode == 1 && !campaign && isSave);
        includeFile = includeFile || (mode == 0 && (isSave || isCampaignSave));
        // { Original accepts .asg in every mode, including map selection. }
        includeFile = includeFile || (mode == 2 && DocumentKind.Map.HasExtension(extension)) || isSave;
        return includeFile;
    }

    // AoWFileDialog.pas:233
    public void FindFiles()
    {
        for (int i = 0, last = DateList.Items.Count - 1; i <= last; i++)
        {
            if (DateList.Items.Objects[i] != null)
            {
                (DateList.Items.Objects[i] as IDisposable)?.Dispose();
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
        string directory = RtlHost.Files.CurrentDirectory;
        if (Copy(directory, index: directory.Length, count: 1) == "\\")
        {
            directory = Copy(directory, index: 1, count: directory.Length - 1);
        }

        foreach (RtlFileEntry entry in RtlHost.Files.FindEntries($"{directory}\\**", attributes: 16))
        {
            if (entry.Name is not "." and not "..")
            {
                if ((entry.Attribute & 0b0001_0000) == 0b0001_0000)
                {
                    _ = Directories.Add(entry.Name);
                }
                else
                {
                    string extension = LowerCase(ExtractFileExt(entry.Name));
                    bool includeFile = ListsFile(Mode, campaign: UI.Engine.Campaign is not null, extension);
                    if (includeFile)
                    {
                        FileTime stamp = new()
                        {
                            Time = FileDateToDateTime(entry.FileDate)
                        };
                        _ = Files.AddObject(entry.Name, item: stamp);
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
        PathEdit.Text = $"{directory}\\";
        for (int i = 0, last = Directories.Count - 1; i <= last; i++)
        {
            _ = DateList.Items.Add("");
        }

        for (int i = 0, last = Files.Count - 1; i <= last; i++)
        {
            string dateTimeToString = DateTimeToStr(Files.Objects[i].Time);
            FileTime objectValue = Files.Objects[i];
            _ = DateList.Items.AddObject(dateTimeToString, item: objectValue);
        }

        RestoreSortOrder();
    }

    // AoWFileDialog.pas:296
    public void FileDialogWindowCreate()
    {
        Drives = new DelphiStringList();
        Directories = new DelphiStringList();
        Files = new DelphiStringList<FileTime>();
        Directory = $"{UI.Engine.StartupDirectory}Save\\";
        LastFileName = "";
    }

    // AoWFileDialog.pas:305
    public void FileDialogWindowDestroy()
    {
        for (int i = 0, last = DateList.Items.Count - 1; i <= last; i++)
        {
            if (DateList.Items.Objects[i] != null)
            {
                (DateList.Items.Objects[i] as IDisposable)?.Dispose();
            }
        }

        Drives?.Dispose();
        Directories?.Dispose();
        Files?.Dispose();
    }

    // AoWFileDialog.pas:316
    public void OpenbutClick()
    {
        UI.General.PlayButtonSound(0);
        int count = Directories.Count;
        if (FileList.Selected < count && FileList.Selected > -1)
        {
            string directory = RtlHost.Files.CurrentDirectory;
            if (Copy(directory, index: directory.Length, count: 1) == "\\")
            {
                directory = Copy(directory, index: 1, count: directory.Length - 1);
            }

            string item = FileList.Items[FileList.Selected];
            if (RtlHost.Files.SetCurrentDirectory($"{directory}\\{item}"))
            {
                FindFiles();
            }

            FileList.Selected = -1;
        }
        else
        {
            string path = RtlHost.Files.CurrentDirectory;
            if (path.Length > 0 && path[^1] != 92)
            {
                path += "\\";
            }

            switch (Mode)
            {
                case 0:
                    if (!IsEmpty(SaveFileNameEdit.Text) && RtlHost.Files.FileExists(SaveFileNameEdit.Text))
                    {
                        if (StrategicMap.Current is not null)
                        {
                            if (StrategicMap.Current.CombatMap is not null)
                            {
                                UI.General.ReturnToTitle();
                            }
                            else
                            {
                                if (UI.Engine.Campaign is not null)
                                {
                                    if (CompareText(UI.Engine.Campaign.FileName, Directory + SaveFileNameEdit.Text) == 0)
                                    {
                                        StrategicMap.Current.ExitGame(ExitGameMode.Reload, done: ExitForLoad);
                                    }
                                    else
                                    {
                                        StrategicMap.Current.ExitGame(ExitGameMode.Load, done: ExitForLoad);
                                    }
                                }
                                else
                                {
                                    if (CompareText(StrategicMap.Current.FileName, Directory + SaveFileNameEdit.Text) == 0)
                                    {
                                        StrategicMap.Current.ExitGame(ExitGameMode.Reload, done: ExitForLoad);
                                    }
                                    else
                                    {
                                        StrategicMap.Current.ExitGame(ExitGameMode.Load, done: ExitForLoad);
                                    }
                                }
                            }
                        }

                        if (StrategicMap.Current is null || StrategicMap.Current.ExitPending)
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
                        if (Pos(".", haystack: SaveFileNameEdit.Text) != 0)
                        {
                            SaveFileNameEdit.Text = Copy(SaveFileNameEdit.Text, index: 1, count: Pos(".", haystack: SaveFileNameEdit.Text) - 1);
                        }

                        if (UI.Engine.Campaign is not null)
                        {
                            Window.Visible = false;
                            // The save takes the extension the host writes, beside an original of the same name
                            // (own-formats.md 4.2).
                            SaveFileNameEdit.Text = FileBoundary.WriteName(SaveFileNameEdit.Text + DocumentKind.CampaignSave.OriginalExtension);
                            SaveCampaign(SaveFileNameEdit.Text);
                        }
                        else
                        {
                            if (_saveEvent is not null)
                            {
                                _saveEvent.FileName = FileBoundary.WriteName(SaveFileNameEdit.Text + DocumentKind.Save.OriginalExtension, Directory);
                                Window.Visible = false;
                            }
                            else
                            {
                                Window.Visible = false;
                                SaveFileNameEdit.Text = FileBoundary.WriteName(SaveFileNameEdit.Text + DocumentKind.Save.OriginalExtension);
                                SaveGame(SaveFileNameEdit.Text);
                            }
                        }

                        LastFileName = SaveFileNameEdit.Text;
                        Directory = path;
                    }

                    break;
                case 2:
                    if (!IsEmpty(FileNameEdit.Text) && RtlHost.Files.FileExists(FileNameEdit.Text))
                    {
                        PickScenarioMap(UI.ScenarioSetup, name: FileNameEdit.Text);
                        LastFileName = FileNameEdit.Text;
                    }

                    break;
                default:
                    break;
            }
        }
    }

    // AoWFileDialog.pas:394
    public void UpDirBtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (RtlHost.Files.SetCurrentDirectory(".."))
        {
            FindFiles();
        }
    }

    // AoWFileDialog.pas:400
    public void DriveCmbChange()
    {
        string item = DriveComboBox.Items[DriveComboBox.Selected];
        if (RtlHost.Files.SetCurrentDirectory(Copy(DriveComboBox.Items[DriveComboBox.Selected], index: item.Length - 3, count: 3)))
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
        ListBox list = sender;
        FileList.Selected = list.Selected;
        SaveList.Selected = list.Selected;
        DateList.Selected = list.Selected;
        if (Directories.Count - 1 < FileList.Selected)
        {
            string item = FileList.Items[FileList.Selected];
            SaveFileNameEdit.Text = item;
            if (!DocumentKind.CampaignSave.HasExtension(ExtractFileExt(SaveFileNameEdit.Text)))
            {
                StrategicMap map = UI.Engine.CreatePreviewHsm<StrategicMap>(SaveFileNameEdit.Text, profile: GameProfile.WorldMap);
                if (map is not null)
                {
                    int count = map.Players.GetCount();
                    SavedPlayersLabel.Text = (count - 1).ToString(CultureInfo.InvariantCulture);
                    switch (map.MapContainer.Width)
                    {
                        case 48:
                            string translatedText = UI.Engine.TranslateResourceString("S");
                            SavedSizeLabel.Text = translatedText;
                            break;
                        case 64:
                            string translateResourceString2 = UI.Engine.TranslateResourceString("M");
                            SavedSizeLabel.Text = translateResourceString2;
                            break;
                        case 96:
                            string translateResourceString3 = UI.Engine.TranslateResourceString("L");
                            SavedSizeLabel.Text = translateResourceString3;
                            break;
                        case 127:
                        case 128:
                            string extraLargeMapLetter = UI.Engine.TranslateResourceString(s_extraLargeMapLetter);
                            SavedSizeLabel.Text = extraLargeMapLetter;
                            break;
                        default:
                            SavedSizeLabel.Text = "?";
                            break;
                    }

                    SavedTownsLabel.Text = map.StructureControl.CityCount.ToString(CultureInfo.InvariantCulture);
                    SavedTurnsLabel.Text = map.Day.ToString(CultureInfo.InvariantCulture);
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
    public void FileLBDrawItem(in Rect drawRectangle, int drawItem, ref readonly LockedSurface lockedSurface)
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
                _ = RtlHost.Files.SetCurrentDirectory(Directory);
                string fileName = ExtractFileName(LastFileName);
                SaveFileNameEdit.Text = fileName;
                FileNameEdit.Text = SaveFileNameEdit.Text;
                string selectAFileToLoad = UI.Engine.TranslateResourceString(s_selectFileToLoadText);
                UI.FileDialogWindow.TitleLabel.Text = selectAFileToLoad;
                UI.FileDialogWindow.SaveFileNameEdit.Editable = false;
                string open = UI.Engine.TranslateResourceString(s_openText);
                UI.FileDialogWindow.OpenButton.Text = open;
                SavePanel.Visible = true;
                NewPanel.Visible = false;
                break;
            case 1:
                _ = RtlHost.Files.SetCurrentDirectory(Directory);
                string fileName2 = ExtractFileName(LastFileName);
                SaveFileNameEdit.Text = fileName2;
                if (Pos(".", haystack: SaveFileNameEdit.Text) != 0)
                {
                    SaveFileNameEdit.Text = Copy(SaveFileNameEdit.Text, index: 1, count: Pos(".", haystack: SaveFileNameEdit.Text) - 1);
                }

                if (!IsEmpty(SaveFileNameEdit.Text))
                {
                    // The default name shows the file the save writes (own-formats.md 4.2).
                    SaveFileNameEdit.Text = UI.Engine.Campaign is not null
                        ? FileBoundary.WriteName(SaveFileNameEdit.Text + DocumentKind.CampaignSave.OriginalExtension)
                        : FileBoundary.WriteName(SaveFileNameEdit.Text + DocumentKind.Save.OriginalExtension);
                }

                FileNameEdit.Text = SaveFileNameEdit.Text;
                string saveGameAs = UI.Engine.TranslateResourceString(s_saveGameAsText);
                UI.FileDialogWindow.TitleLabel.Text = saveGameAs;
                UI.FileDialogWindow.SaveFileNameEdit.Editable = true;
                string save = UI.Engine.TranslateResourceString(GameText.SaveText);
                UI.FileDialogWindow.OpenButton.Text = save;
                SavePanel.Visible = true;
                NewPanel.Visible = false;
                break;
            case 2:
                string lastExtension = ExtractFileExt(LastFileName);
                if (lastExtension == DocumentKind.Map.OriginalExtension || lastExtension == DocumentKind.Map.OwnExtension)
                {
                    string fileName3 = ExtractFileName(LastFileName);
                    FileNameEdit.Text = fileName3;
                }
                else
                {
                    FileNameEdit.Text = "";
                }

                _ = RtlHost.Files.SetCurrentDirectory($"{UI.Engine.StartupDirectory}Scenario\\Custom\\");
                string selectAMap = UI.Engine.TranslateResourceString(s_selectMapText);
                UI.FileDialogWindow.TitleLabel.Text = selectAMap;
                string select = UI.Engine.TranslateResourceString(s_selectText);
                UI.FileDialogWindow.OpenButton.Text = select;
                SavePanel.Visible = false;
                NewPanel.Visible = true;
                break;
            default:
                break;
        }

        UI.FileDialogWindow.FindDrives();
    }

    // AoWFileDialog.pas:502
    public void MSBarChange(VerticalScrollBar sender)
    {
        VerticalScrollBar bar = sender;
        FileList.ListOffset = bar.Position;
        SaveList.ListOffset = bar.Position;
        DateList.ListOffset = bar.Position;
    }

    // AoWFileDialog.pas:511
    public void SFileNameReturn() => OpenbutClick();

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

        for (int i = 0, last = Directories.Count - 1; i <= last; i++)
        {
            SaveList.Items.Delete(0);
            DateList.Items.Delete(0);
        }

        if (Mode is 1 or 0)
        {
            SaveList.Items.SortStringsAscending(linkedStrings: [DateList.Items, FileList.Items]);
            FileList.Items.Assign(SaveList.Items);
        }
        else
        {
            if (Mode == 2)
            {
                FileList.Items.SortStringsAscending(linkedStrings: [DateList.Items, SaveList.Items]);
            }
        }

        for (int i = 0, last = Directories.Count - 1; i <= last; i++)
        {
            string directory = Directories[i];
            SaveList.Items.Insert(i, value: directory);
            DateList.Items.Insert(i, value: "");
        }

        if (!IsEmpty(selected))
        {
            int indexOf = FileList.Items.IndexOf(selected);
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

        if (Mode is 1 or 0)
        {
            for (int i = 0, last = SaveList.Items.Count - 1; i <= last; i++)
            {
                FileTime objectValue = DateList.Items.Objects[i];
                SaveList.Items.Objects[i] = objectValue;
            }

            SaveList.Items.SortStrings(compare: CompareFileDates, linkedStrings: [DateList.Items, FileList.Items]);
        }
        else
        {
            if (Mode == 2)
            {
                for (int i = 0, last = FileList.Items.Count - 1; i <= last; i++)
                {
                    FileTime objectValue = DateList.Items.Objects[i];
                    FileList.Items.Objects[i] = objectValue;
                }

                FileList.Items.SortStrings(compare: CompareFileDates, linkedStrings: [DateList.Items, SaveList.Items]);
            }
        }

        if (!IsEmpty(selected))
        {
            int indexOf = FileList.Items.IndexOf(selected);
            FileList.Selected = indexOf;
        }

        FileLBChange(FileList);
    }

    // AoWFileDialog.pas:570
    public void SSBarChange() => SaveList.ListOffset = SaveScrollBar.Position;

    // AoWFileDialog.pas:575
    public void FileDialogHide() => FinishDialog();

    // AoWFileDialog.pas:580
    public void FileDialogKeyDown(in ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    public void FindDrives()
    {
        Drives.Clear();
        string drive = $"{RtlHost.Files.VirtualDrive}\\";
        string label = OperatingSystem.IsWindows() ? $"{new DriveInfo(drive).VolumeLabel} " : "";
        _ = Drives.Add($"{label}({drive})");
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

    internal static DelphiStringList Drives { get; set; }

    internal static DelphiStringList Directories { get; set; }

    internal static DelphiStringList<FileTime> Files { get; set; }

    internal static int CompareFileDates(string s1, string s2, object o1, object o2)
    {
        return o1 is null && o2 is null
            ? CompareText(s1, s2)
            : o1 is null
                ? -1
                : o2 is null
                    ? 1
                    : (double)((FileTime)o2).Time > ((FileTime)o1).Time ? 1 : (double)((FileTime)o2).Time < ((FileTime)o1).Time ? -1 : 0;
    }

    private static void LoadCampaign(General controller, string name)
    {
        GameFileTree tree = FileBoundary.Open(name, profile: GameProfile.Campaign);
        if (tree == null)
        {
            return;
        }

        if (StrategicMap.Current is not null)
        {
            controller.UI.Engine.CloseHsm(null);
        }

        controller.UI.Engine.LoadCampaign(name, tree: tree);
        if (controller.UI.Engine.Campaign is not null)
        {
            if (controller.UI.Engine.Campaign.ScenarioStarted)
            {
                string loading = controller.UI.Engine.TranslateResourceString(LayerText.LoadingText);
                string fileName = ExtractFileName(name);
                controller.UI.ProgressDialog.TitleLabel.Text = $"{loading} {fileName}...";
                controller.UI.ProgressDialog.StatusLabel.Text = "";
                controller.UI.ProgressDialog.ProgressBar.Position = 0;
                controller.UI.ProgressDialog.ProgressBar.Max = 100;
                controller.UI.ProgressDialog.Window.Visible = true;
                if (!controller.UI.TitleScene.Window.Visible)
                {
                    controller.UI.General.Manager.OverlayPriority = 5_000;
                }

                _ = AudioHost.SongPlayer.Stop(false);
                AudioHost.SongPlayer.Songs.Clear();
                _ = (AudioHost.SoundSystem?.FadeMusicVolume(0, speed: 1));
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

    private static void SaveGame(string name) => StrategicMap.Current.SaveGame(name);

    private static void SaveCampaign(string name)
        // { Original campaign-save command uses the map save routine. }
        => StrategicMap.Current.SaveGame(name);

    private static void PickScenarioMap(ScenarioSetup dialog, string name)
    {
        string currentDirectory = RtlHost.Files.CurrentDirectory;
        name = RtlHost.Files.ExtractRelativePath(dialog.UI.Engine.StartupDirectory, destination: $"{currentDirectory}\\{name}");
        dialog.UI.FileDialogWindow.Window.Visible = false;
        _ = dialog.SetupControl.PickMap(name);
    }

    internal static void OpenSelectedGame(FileDialogWindow dialog) => dialog.LoadSelectedGame(dialog.LastFileName);

    internal static void ShowSaveGameEvent(FileDialogWindow dialog, SaveGameEventLog eventValue, ExecuteDoneEvent done)
    {
        dialog.Done = done;
        dialog._saveEvent = eventValue;
        _ = dialog._saveEvent.AddRef();
        dialog.Mode = 1;
        dialog.Window.Visible = true;
        if (!IsEmpty(eventValue.Directory))
        {
            _ = RtlHost.Files.SetCurrentDirectory(eventValue.Directory);
        }

        dialog.SaveFileNameEdit.Text = eventValue.DefaultName;
        dialog.TitleLabel.Text = eventValue.Title;
        dialog.FindFiles();
    }
}
