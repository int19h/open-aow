// Translated from AoWVideoDlg.pas.
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
// Translated from AoWVideoDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs
{

    // full screen, change flag, display size
    public partial class VideoDialog : DataModule
    {
        public VideoDialog(Component owner) : base(owner)
        { }

        public AOWWindow Dlg;
        public AOWButton CloseBut;
        public AoWFrame BCeltL;
        public AOWLabel Title;
        public AOWButton Close;
        public AOWImage TCeltLR;
        public AoWFrame TopLine;
        public AOWLabel VideoLbl;
        public AOWImage TCeltPattern;
        public AOWImage TitleIcon;
        public AOWImage TCeltLL;
        public AOWListBox DisplayModesLB;
        public AOWCheckBox ResCB;
        public AOWLabel ResSwLbl;
        public AOWButton ApplyBtn;
        public AOWPanel T3LBPnl;
        public AOWVScrollBar VideoSB;
        public IvTranslator Translator;
        public DisplayModeList FDisplayModes;
        public ScreenMode FFullScreen;
        public bool FModeChanged;
        public int FSceneWidth;
        public int FSceneHeight;
        public int FDisplayWidth;
        public int FDisplayHeight;

        // AoWVideoDlg.pas:366
        public void SetSceneSize(int W, int H)
        {
            FSceneWidth = W;
            FSceneHeight = H;
            if (FFullScreen == ScreenMode.Title)
            {
                FModeChanged = true;
            }
        }

        // AoWVideoDlg.pas:416
        public void ShowModes()
        {
            Dlg.Visible = true;
            UpdateModes();
        }

        // AoWVideoDlg.pas:172
        public void UpdateModes()
        {
            DisplayModeItem Mode;
            string S;
            DisplayModesLB.Items.Clear();
            var desktopWindow = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(DesktopWindowRStr);
            DisplayModesLB.Items.AddObject(desktopWindow, IntToObject(-1));
            for (int I = 0, last = FDisplayModes.Count - 1; I <= last; I++)
            {
                Mode = FDisplayModes.Items[I];
                if (Mode.Width >= 640 && Mode.Height >= 480)
                {
                    var fullScreen = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(FullScreenRStr);
                    DisplayModesLB.Items.AddObject(fullScreen + ":" + IntToStr(Mode.Width) + "x" + IntToStr(Mode.Height), IntToObject(I));
                }
            }

            var currentMode = CurrentMode();
            DisplayModesLB.Selected = currentMode;
            if (DisplayModesLB.Selected != -1)
            {
                S = DisplayModesLB.Items[DisplayModesLB.Selected];
                S = S + " (" + LowerCase(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(CurrentRStr)) + ")";
                DisplayModesLB.Items[DisplayModesLB.Selected] = S;
            }
        }

        // AoWVideoDlg.pas:195
        public void VideoDlgWindowCreate()
        {
            FDisplayModes = new DisplayModeList();
            Aow.Game.Ui.Dialogs.VideoDialog.ListDisplayModes(FDisplayModes, 0, 0, 16);
        }

        // AoWVideoDlg.pas:201
        public void VideoDlgWindowDestroy()
        {
            FDisplayModes?.Dispose();
        }

        // AoWVideoDlg.pas:206
        public void CloseButClick()
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWVideoDlg.pas:212
        public void ApplyBtnClick()
        {
            int I;
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
            if (DisplayModesLB.Selected != -1)
            {
                I = ObjectToInt(DisplayModesLB.Items.Objects[DisplayModesLB.Selected]);
                if (I != -1)
                {
                    FDisplayWidth = FDisplayModes.Items[I].Width;
                    FDisplayHeight = FDisplayModes.Items[I].Height;
                    ChangeResolution(FDisplayWidth, FDisplayHeight);
                    FFullScreen = ScreenMode.Game;
                }
                else
                {
                    FFullScreen = ScreenMode.Windowed;
                    ChangeResolution(0, 0);
                }
            }
        }

        // AoWVideoDlg.pas:233
        public void ResCBChange(AOWCheckBox Sender)
        {
            if (Sender.Checked)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
            }

            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetAllowResolutionSwitches(ResCB.Checked);
        }

        // AoWVideoDlg.pas:239
        public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
        {
            DialogHosts.General.Manager.FAligned = false;
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }

        // AoWVideoDlg.pas:245
        public void DlgShow()
        {
            var allowResolutionSwitches = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetAllowResolutionSwitches();
            ResCB.Checked = allowResolutionSwitches;
        }

        private static readonly string CurrentRStr = "Current";

        private static readonly string DesktopWindowRStr = "Desktop Window";

        private static readonly string FullScreenRStr = "Full Screen";

        internal static Func<(int Width, int Height)> DesktopSize { get; set; } = () => (1280, 1024);

        internal static Func<IReadOnlyList<(int Width, int Height)>> FullscreenSizes { get; set; } = () => null;

        private static readonly (int Width, int Height)[] FixedSizes = [(640, 480), (800, 600), (1024, 768), (1280, 1024)];

        private static void ListDisplayModes(DisplayModeList list, int width, int height, int pixelFormatID)
        {
            var desktop = DesktopSize();
            var sizes = FullscreenSizes() ?? [.. FixedSizes.Where(size => size.Width <= desktop.Width && size.Height <= desktop.Height)];
            foreach (var size in sizes)
                if ((width == 0 || width == size.Width) && (height == 0 || height == size.Height) &&
                    (pixelFormatID == 0 || pixelFormatID == 16 || pixelFormatID == PixelFormats.pfRGB555))
                    list.Add(size.Width, size.Height, PixelFormats.pfRGB555);
        }

        internal static void SetSceneSize(VideoDialog Dialog, int Width, int Height)
        {
            Dialog.FSceneWidth = Width;
            Dialog.FSceneHeight = Height;
            if (Dialog.FFullScreen == ScreenMode.Title)
            {
                Dialog.FModeChanged = true;
            }
        }

        internal static void ApplyVideoSettings(VideoDialog dialog) => dialog.ApplyMode();

        internal static void SetVideoSize(VideoDialog Dialog, int Width, int Height)
        {
            Dialog.FDisplayWidth = Width;
            Dialog.FDisplayHeight = Height;
            if (Dialog.FFullScreen == ScreenMode.Game)
            {
                Dialog.FModeChanged = true;
            }
        }

        internal static void ShowVideoSettings(VideoDialog Dialog)
        {
            Dialog.Dlg.Visible = true;
            Dialog.UpdateModes();
        }

        internal static void SetVideoMode(VideoDialog Dialog, ScreenMode FullScreen)
        {
            if (FullScreen != Dialog.FFullScreen)
            {
                Dialog.FFullScreen = FullScreen;
                Dialog.FModeChanged = true;
            }
        }

        public enum ScreenMode : byte
        {
            Windowed = 0, Game = 1, Title = 2
        }

    }

    // image library, image index, heading, text
    public partial class WMCampaignScenarioPage : EObject
    {
        // AoWVideoDlg.pas:264
        public new const int ClassID = 2248706;

        public string FImageLibrary = "";
        public int FImageIndex;
        public string FHeading = "";
        public StringList FText;

        // AoWVideoDlg.pas:257
        public override void Dispose()
        {
            FText?.Dispose();
            base.Dispose();
        }

        // AoWVideoDlg.pas:268
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FText, 10);
            Stream.rwString(ref FHeading, 11);
            if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
            {
                Stream.rwString(ref FImageLibrary, 12);
            }

            Stream.rwInteger(ref FImageIndex, 13);
        }

        // AoWVideoDlg.pas:250
        public WMCampaignScenarioPage(ECustomNode owner) : base(owner)
        {
            FText = new StringList();
        }

    }

    public partial class WMCampaignScenarioPageList : ENode
    {
        public WMCampaignScenarioPageList(ECustomNode owner) : base(owner)
        { }

    }
}

namespace Aow.Game.Session
{

    // image library, path text, description, flag, position, paths, pages, map kind
    public partial class WMCampaignScenario : AoWCampaignScenario
    {
        // AoWVideoDlg.pas:304
        public new const int ClassID = 2248705;

        public string FImageLibrary = "";
        public string FPathDescription = "";
        public string FDescription = "";
        public bool F_WMCampaignScenario_20;
        // rwBool
        public int FMapX;
        public int FMapY;
        public IntegerList FPaths;
        public WMCampaignScenarioPageList FIntroPages;
        public WMCampaignScenarioPageList FOutroPages;
        public byte FMapKind;

        // AoWVideoDlg.pas:295
        public override void Dispose()
        {
            FIntroPages?.Dispose();
            FOutroPages?.Dispose();
            FPaths?.Dispose();
            base.Dispose();
        }

        // AoWVideoDlg.pas:308
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
            {
                Stream.rwString(ref FImageLibrary, 12);
            }

            Stream.rwString(ref FPathDescription, 13);
            Stream.rwString(ref FDescription, 14);
            Stream.rwBool(ref F_WMCampaignScenario_20, 15);
            Stream.rwInteger(ref FMapX, 16);
            Stream.rwInteger(ref FMapY, 17);
            Stream.rwEObject(FIntroPages, 19);
            Stream.rwEObject(FOutroPages, 20);
            Stream.rwStreamProc(21, FPaths.LoadFromStream, FPaths.SaveToStream);
            Stream.rwData(Bytes.Of(ref FMapKind, 1), 1, 22);
        }

        // AoWVideoDlg.pas:278
        public WMCampaignScenario(ECustomNode owner) : base(owner)
        {
            FIntroPages = new WMCampaignScenarioPageList(null);
            FOutroPages = new WMCampaignScenarioPageList(null);
            for (int I = 0; I <= 9; I++)
            {
                new WMCampaignScenarioPage(FIntroPages);
                new WMCampaignScenarioPage(FOutroPages);
            }

            FPaths = new IntegerList();
            FPaths.Default = -1;
        }

    }

    // faction, state, page
    public partial class WMCampaign : AoWCampaign
    {
        // AoWVideoDlg.pas:334
        public new const int ClassID = 2248704;

        public int FFaction;
        public int FState;
        public int FPage;

        // AoWVideoDlg.pas:337
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FFaction, 30);
        }

        // AoWVideoDlg.pas:324
        public WMCampaign(ECustomNode owner) : base(owner)
        {
        }

        // AoWVideoDlg.pas:348
        public override void Clear()
        {
            base.Clear();
        }

        // AoWVideoDlg.pas:343
        public override AoWCampaignScenario CreateScenario() => new WMCampaignScenario(null);

        // AoWVideoDlg.pas:353
        public override string GetAutoSaveFileName(CampaignAutoSaveType Auto)
        {
            string Result;
            if (Auto == CampaignAutoSaveType.ScenarioFinished)
            {
                Result = AoWHSMap.Current.FMapName;
                if (FFaction == 0)
                {
                    Result = Result + "_" + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(WonRStr);
                }
                else
                {
                    // { Original uses Won for both factions. }
                    Result = Result + "-" + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(WonRStr);
                }

                // The campaign autosave takes the extension the host writes (own-formats.md 4.2).
                Result = Result + FileBoundary.WriteExtension(Aow.Formats.DocumentKind.CampaignSave);
            }
            else
            {
                Result = base.GetAutoSaveFileName(Auto);
            }

            return Result;
        }

        private static readonly string WonRStr = "Won";

        internal static WMCampaignScenario GetCampaignScenario(WMCampaign Campaign, int Index) => (WMCampaignScenario)Campaign.FScenarios.GetScenario(Index);

    }
}
