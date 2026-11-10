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
            if (!Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDemo)
            {
                // A refused campaign keeps the current map, which is not started again.
                var Previous = Aow.Game.World.Map.TAoWHSMap.AoWHSMap;
                Aow.Game.Ui.Dialogs.TFileDlgWindow.LoadCampaign(Aow.Game.Ui.Dialogs.DialogHosts.General, Name);
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null && Aow.Game.World.Map.TAoWHSMap.AoWHSMap != Previous)
                {
                    Aow.Game.Ui.Dialogs.TGeneral.StartGame(Aow.Game.Ui.Dialogs.DialogHosts.General);
                }
            }
        }
        else
        {
            Map = (TAoWHSMap)Aow.Game.Magic.Abilities.GameTables.AoWEngine.CreatePreviewHSM(Name, Aow.Formats.GameProfile.WorldMap);
            if (Map != null)
            {
                Aow.Game.Magic.Abilities.GameTables.AoWEngine.CloseCampaign();
                Aow.Game.Magic.Abilities.GameTables.AoWEngine.CloseHSM(null);
                try
                {
                    if (Map.FConnectionType != TPlayerConnectionType.pcNetwork)
                    {
                        Aow.Game.Ui.Dialogs.TGeneral.LoadGame(Aow.Game.Ui.Dialogs.DialogHosts.General, Name);
                        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
                        {
                            Aow.Game.Ui.Dialogs.TGeneral.StartGame(Aow.Game.Ui.Dialogs.DialogHosts.General);
                        }
                    }
                    else
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.TitleScene.HideScene();
                        Aow.Game.Ui.Dialogs.DialogHosts.General.HideWindows();
                        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.Visible = false;
                        var relativePath = ExtractRelativePath(Aow.Game.Magic.Abilities.GameTables.AoWEngine.StartupDirectory, Name);
                        Aow.Game.Ui.Dialogs.DialogHosts.MultiWindow.Execute(relativePath);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
                (DateLB.Items.Objects[I] as IDisposable)?.Dispose();
            }
        }

        Aow.Game.Ui.Dialogs.TFileDlgWindow.Directories.Clear();
        Aow.Game.Ui.Dialogs.TFileDlgWindow.Files.Clear();
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
                        Aow.Game.Ui.Dialogs.TFileDlgWindow.Directories.Add(Search.Name);
                    }
                    else
                    {
                        Extension = LowerCase(ExtractFileExt(Search.Name));
                        IncludeFile = ListsFile(FMode, Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign != null, Extension);
                        if (IncludeFile)
                        {
                            Stamp = new TFileTime();
                            Stamp.FTime = FileDateToDateTime(Search.Time);
                            Aow.Game.Ui.Dialogs.TFileDlgWindow.Files.AddObject(Search.Name, Stamp);
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
        Aow.Game.Ui.Dialogs.TFileDlgWindow.Directories.Sort();
        Aow.Game.Ui.Dialogs.TFileDlgWindow.Files.Sort();
        FileLB.Items.AddStrings(Aow.Game.Ui.Dialogs.TFileDlgWindow.Directories);
        FileLB.Items.AddStrings(Aow.Game.Ui.Dialogs.TFileDlgWindow.Files);
        SaveLB.Items.AddStrings(FileLB.Items);
        PathEdit.Text = Directory + "\\";
        for (int I = 0, last = Aow.Game.Ui.Dialogs.TFileDlgWindow.Directories.Count - 1; I <= last; I++)
        {
            DateLB.Items.Add("");
        }

        for (int I = 0, last = Aow.Game.Ui.Dialogs.TFileDlgWindow.Files.Count - 1; I <= last; I++)
        {
            var dateTimeToStr = DateTimeToStr(((TFileTime)Aow.Game.Ui.Dialogs.TFileDlgWindow.Files.Objects[I]).FTime);
            var objectValue = Aow.Game.Ui.Dialogs.TFileDlgWindow.Files.Objects[I];
            DateLB.Items.AddObject(dateTimeToStr, objectValue);
        }

        RestoreSortOrder();
    }

    // AoWFileDialog.pas:296
    public void FileDlgWindowCreate()
    {
        Aow.Game.Ui.Dialogs.TFileDlgWindow.Drives = new TStringList();
        Aow.Game.Ui.Dialogs.TFileDlgWindow.Directories = new TStringList();
        Aow.Game.Ui.Dialogs.TFileDlgWindow.Files = new TStringList();
        FDirectory = Aow.Game.Magic.Abilities.GameTables.AoWEngine.StartupDirectory + "Save\\";
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

        Aow.Game.Ui.Dialogs.TFileDlgWindow.Drives?.Dispose();
        Aow.Game.Ui.Dialogs.TFileDlgWindow.Directories?.Dispose();
        Aow.Game.Ui.Dialogs.TFileDlgWindow.Files?.Dispose();
    }

    // AoWFileDialog.pas:316
    public void OpenbutClick()
    {
        string Path = "";
        string Directory = "";
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        var count = Aow.Game.Ui.Dialogs.TFileDlgWindow.Directories.Count;
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
                        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
                        {
                            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombatMap != null)
                            {
                                Aow.Game.Ui.Dialogs.DialogHosts.General.ReturnToTitle();
                            }
                            else
                            {
                                if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign != null)
                                {
                                    if (CompareText(Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign.FFileName, FDirectory + SFileName.Text) == 0)
                                    {
                                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ExitGame(TExitGameMode.egmReload, ExitForLoad);
                                    }
                                    else
                                    {
                                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ExitGame(TExitGameMode.egmLoad, ExitForLoad);
                                    }
                                }
                                else
                                {
                                    if (CompareText(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FileName, FDirectory + SFileName.Text) == 0)
                                    {
                                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ExitGame(TExitGameMode.egmReload, ExitForLoad);
                                    }
                                    else
                                    {
                                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ExitGame(TExitGameMode.egmLoad, ExitForLoad);
                                    }
                                }
                            }
                        }

                        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap == null || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FExitPending)
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

                        if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign != null)
                        {
                            FileDialog.Visible = false;
                            // The save takes the extension the host writes, beside an original of the same name
                            // (own-formats.md 4.2).
                            SFileName.Text = FileBoundary.WriteName(SFileName.Text + Aow.Formats.DocumentKind.CampaignSave.OriginalExtension);
                            Aow.Game.Ui.Dialogs.TFileDlgWindow.SaveCampaign(Aow.Game.Ui.Dialogs.DialogHosts.General, SFileName.Text);
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
                                Aow.Game.Ui.Dialogs.TFileDlgWindow.SaveGame(Aow.Game.Ui.Dialogs.DialogHosts.General, SFileName.Text);
                            }
                        }

                        FLastFileName = SFileName.Text;
                        FDirectory = Path;
                    }

                    break;
                case 2:
                    if (!IsEmpty(FileName.Text) && FileExists(FileName.Text))
                    {
                        Aow.Game.Ui.Dialogs.TFileDlgWindow.PickScenarioMap(Aow.Game.Ui.Dialogs.DialogHosts.ScenSetup, FileName.Text);
                        FLastFileName = FileName.Text;
                    }

                    break;
            }
        }
    }

    // AoWFileDialog.pas:394
    public void UpDirBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        if (Aow.Game.Ui.Dialogs.TFileDlgWindow.Directories.Count - 1 < this.FileLB.Selected)
        {
            var item = FileLB.Items[FileLB.Selected];
            SFileName.Text = item;
            if (!Aow.Formats.DocumentKind.CampaignSave.HasExtension(ExtractFileExt(SFileName.Text)))
            {
                Map = (TAoWHSMap)Aow.Game.Magic.Abilities.GameTables.AoWEngine.CreatePreviewHSM(SFileName.Text, Aow.Formats.GameProfile.WorldMap);
                if (Map != null)
                {
                    var count = Map.FPlayers.GetCount();
                    LPlayers.Text = IntToStr(count - 1);
                    switch (Map.MapContainer.Width)
                    {
                        case 48:
                            var translateRStr = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr("S");
                            LSize.Text = translateRStr;
                            break;
                        case 64:
                            var translateRStr2 = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr("M");
                            LSize.Text = translateRStr2;
                            break;
                        case 96:
                            var translateRStr3 = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr("L");
                            LSize.Text = translateRStr3;
                            break;
                        case 127:
                        case 128:
                            var extraLargeMapAbr = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TFileDlgWindow.ExtraLargeMapAbrRStr);
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
    public void FileLBDrawItem(in TRect DrawRect, int DrawItem, ref TSurfaceDesc SurfDesc)
    {
        if (Aow.Game.Ui.Dialogs.TFileDlgWindow.Directories.Count > DrawItem)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.LBIcons[0].ShowClipped(DrawRect.Left + 2, DrawRect.Top + 1, SurfDesc);
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
                var selectAFileToLoad = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TFileDlgWindow.SelectAFileToLoadRStr);
                Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.Title.Text = selectAFileToLoad;
                Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.SFileName.Editable = false;
                var open = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TFileDlgWindow.OpenRStr);
                Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.Openbut.Text = open;
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
                    if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign != null)
                    {
                        SFileName.Text = FileBoundary.WriteName(SFileName.Text + Aow.Formats.DocumentKind.CampaignSave.OriginalExtension);
                    }
                    else
                    {
                        SFileName.Text = FileBoundary.WriteName(SFileName.Text + Aow.Formats.DocumentKind.Save.OriginalExtension);
                    }
                }

                FileName.Text = SFileName.Text;
                var saveGameAs = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TFileDlgWindow.SaveGameAsRStr);
                Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.Title.Text = saveGameAs;
                Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.SFileName.Editable = true;
                var save = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.SaveRStr);
                Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.Openbut.Text = save;
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

                SetCurrentDir(Aow.Game.Magic.Abilities.GameTables.AoWEngine.StartupDirectory + "Scenario\\Custom\\");
                var selectAMap = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TFileDlgWindow.SelectAMapRStr);
                Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.Title.Text = selectAMap;
                var select = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TFileDlgWindow.SelectRStr);
                Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.Openbut.Text = select;
                SavePnl.Visible = false;
                NewPnl.Visible = true;
                break;
        }

        Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FindDrives();
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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

        for (int I = 0, last = Aow.Game.Ui.Dialogs.TFileDlgWindow.Directories.Count - 1; I <= last; I++)
        {
            SaveLB.Items.Delete(0);
            DateLB.Items.Delete(0);
        }

        if (FMode == 1 || FMode == 0)
        {
            Aow.Game.Engine.Nodes.StringSorting.SortStringsAscending(SaveLB.Items, [DateLB.Items, FileLB.Items]);
            FileLB.Items.Assign(SaveLB.Items);
        }
        else
        {
            if (FMode == 2)
            {
                Aow.Game.Engine.Nodes.StringSorting.SortStringsAscending(FileLB.Items, [DateLB.Items, SaveLB.Items]);
            }
        }

        for (int I = 0, last = Aow.Game.Ui.Dialogs.TFileDlgWindow.Directories.Count - 1; I <= last; I++)
        {
            var directory = Aow.Game.Ui.Dialogs.TFileDlgWindow.Directories[I];
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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

            Aow.Game.Engine.Nodes.StringSorting.SortStringsEx(SaveLB.Items, Aow.Game.Ui.Dialogs.TFileDlgWindow.CompareFileDates, [DateLB.Items, FileLB.Items]);
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

                Aow.Game.Engine.Nodes.StringSorting.SortStringsEx(FileLB.Items, Aow.Game.Ui.Dialogs.TFileDlgWindow.CompareFileDates, [DateLB.Items, SaveLB.Items]);
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
    public void FileDialogKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            FileDialog.Visible = false;
        }
    }

    public void FindDrives()
    {
        Aow.Game.Ui.Dialogs.TFileDlgWindow.Drives.Clear();
        string drive = RtlUnit.Files.VirtualDrive + "\\";
        string label = OperatingSystem.IsWindows() ? new DriveInfo(drive).VolumeLabel + " " : "";
        Aow.Game.Ui.Dialogs.TFileDlgWindow.Drives.Add($"{label}({drive})");
        DriveCmb.Items.Clear();
        DriveCmb.Items.AddStrings(Aow.Game.Ui.Dialogs.TFileDlgWindow.Drives);
        DriveCmb.Selected = 0;
        FindFiles();
    }

    private static readonly string ExtraLargeMapAbrRStr = "XL";

    private static readonly string OpenRStr = "Open";

    private static readonly string SaveGameAsRStr = "Save game as...";

    private static readonly string SelectAFileToLoadRStr = "Select a File to Load...";

    private static readonly string SelectAMapRStr = "Select a Map...";

    private static readonly string SelectRStr = "Select";

    internal static TStringList Drives;

    internal static TStringList Directories;

    internal static TStringList Files;

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

    private static void LoadCampaign(TGeneral Controller, string Name)
    {
        byte[] Image = FileBoundary.Load(Name, Aow.Formats.GameProfile.Campaign);
        if (Image == null)
        {
            return;
        }

        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
        {
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.CloseHSM(null);
        }

        Aow.Game.Magic.Abilities.GameTables.AoWEngine.LoadCampaign(Name, Image);
        if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign != null)
        {
            if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign.FScenarioStarted)
            {
                var e1Loading = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LayerText.E1LoadingRStr);
                var fileName = ExtractFileName(Name);
                Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.Title.Text = e1Loading + " " + fileName + "...";
                Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.StatusLbl.Text = "";
                Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.Bar.Position = 0;
                Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.Bar.Max = 100;
                Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.ProgDlg.Visible = true;
                if (!Aow.Game.Ui.Dialogs.DialogHosts.TitleScene.Window.Visible)
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 5000;
                }

                Aow.Game.Audio.AudioHost.SongPlayer.Stop(false);
                Aow.Game.Audio.AudioHost.SongPlayer.Songs.Clear();
                Aow.Game.Audio.AudioPlayback.FadeMusicVolume(0, 1);
                Aow.Game.Magic.Abilities.GameTables.AoWEngine.LoadCampaignMap((_, Message, Progression) => Controller.MapProgress(Message, Progression));
                Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 0;
                Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.ProgDlg.Visible = false;
            }
            else
            {
                Aow.Game.Ui.Dialogs.DialogHosts.TitleScene.HideScene();
                Aow.Game.Ui.Dialogs.TWorldMap.ShowCampaignMap(Aow.Game.Ui.Dialogs.DialogHosts.WorldMap);
            }
        }
    }

    private static void SaveGame(TGeneral Controller, string Name)
    {
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.SaveGame(Name);
    }

    private static void SaveCampaign(TGeneral Controller, string Name)
    {
        // { Original campaign-save command uses the map save routine. }
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.SaveGame(Name);
    }

    private static void PickScenarioMap(TScenSetup Dialog, string Name)
    {
        var currentDir = GetCurrentDir();
        Name = ExtractRelativePath(Aow.Game.Magic.Abilities.GameTables.AoWEngine.StartupDirectory, currentDir + "\\" + Name);
        Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FileDialog.Visible = false;
        Dialog.FSetupControl.hPickMap(Name);
    }

    internal static void OpenSelectedGame(TFileDlgWindow Dialog)
    {
        Dialog.LoadSelectedGame(Dialog.FLastFileName);
    }

    internal static void ShowSaveGameEvent(TFileDlgWindow Dialog, TSaveGameEventLog Event, TExecuteDoneEvent Done)
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
