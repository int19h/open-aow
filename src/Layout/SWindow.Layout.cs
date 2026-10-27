// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWSWindow
{
    public partial class TSWindow
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "SWindow";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            ScannerWindow = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "ScannerWindow",
                Visible = false,
                Priority = 10,
                WinLeft = 440,
                WinTop = 0,
                WinWidth = 200,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = true,
                BorderWidth = 4,
                BorderHeight = 4,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 0,
                FreeSurfaceOnHide = false,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                DragHeight = 20,
                Draggable = true,
                Resizable = true,
                OnDrag = Layout_ScannerWindowResize_0,
                OnResize = Layout_ScannerWindowResize_0,
                MinWidth = 200,
                MinHeight = 216,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = false,
                WinHeight = 216,
                Dockable = false
            };
            LayoutSession.EndRead(ScannerWindow);
            ScannerFrame = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "ScannerFrame",
                Priority = 50,
                WinLeft = -2,
                WinTop = 10,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = -2,
                    RightOffset = 6,
                    TopOffset = 10,
                    BottomOffset = 4
                },
                Masked = false,
                WinWidth = 196,
                WinHeight = 202,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 0,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(ScannerFrame);
            ScannerTab = new Aow.Units.AoWTabPanel.TAOWTabPanel(this)
            {
                Name = "ScannerTab",
                OnMouseDown = ScannerTabMouseDown,
                Priority = 10000,
                WinLeft = 31,
                WinTop = 182,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects one of the three map layers."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 31,
                    RightOffset = 24,
                    TopOffset = 0,
                    BottomOffset = 19
                },
                Masked = false,
                BorderHeight = 3,
                OnChange = Layout_ScannerTabChange_1,
                Selected = 1,
                WinWidth = 145,
                TextColor = 1777703,
                TextSelColor = 0,
                ILIndexDown = 17,
                ILIndexUp = 14,
                BorderHeightSelected = 3
            };
            LayoutSession.EndRead(ScannerTab);
            BuildingCheck = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "BuildingCheck",
                Priority = 1001,
                WinLeft = 160,
                WinTop = 37,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Toggles player structures on and off."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 25,
                    TopOffset = 37,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 9,
                ILIndexLit = 0,
                ILIndexUp = 8,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 15,
                Checked = true,
                OnChange = Layout_BuildingCheckChange_2,
                RadioIndex = 0
            };
            LayoutSession.EndRead(BuildingCheck);
            ArmyCheck = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "ArmyCheck",
                Priority = 1002,
                WinLeft = 160,
                WinTop = 52,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Toggles parties on and off."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 25,
                    TopOffset = 52,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 11,
                ILIndexLit = 0,
                ILIndexUp = 10,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 15,
                Checked = true,
                OnChange = Layout_ArmyCheckChange_3,
                RadioIndex = 0
            };
            LayoutSession.EndRead(ArmyCheck);
            NaturalCheck = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "NaturalCheck",
                Priority = 1003,
                WinLeft = 160,
                WinTop = 67,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Toggles natural structures on and off."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 25,
                    TopOffset = 67,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 13,
                ILIndexLit = 0,
                ILIndexUp = 12,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 15,
                Checked = true,
                OnChange = Layout_NaturalCheckChange_4,
                RadioIndex = 0
            };
            LayoutSession.EndRead(NaturalCheck);
            Scanner = new Aow.Units.Scanner.TScanner(this)
            {
                Name = "Scanner",
                Priority = 5,
                WinLeft = 30,
                WinTop = 32,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 30,
                    RightOffset = 39,
                    TopOffset = 32,
                    BottomOffset = 32
                },
                Masked = false,
                WinWidth = 131,
                WinHeight = 152,
                TileBoxTop = -1,
                TileBoxLeft = -1,
                TileBoxWidth = -1,
                TileBoxHeight = -1,
                HighLights = true,
                Buildings = true,
                Armies = true,
                Naturals = true,
                OnChangeTileBox = Layout_ScannerChangeTileBox_5,
                MapDrawSteps = 5,
                ILIndexFrame = 20
            };
            LayoutSession.EndRead(Scanner);
            STitleIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "STitleIcon",
                Priority = 0,
                WinLeft = 2,
                WinTop = 2,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 0,
                    TopOffset = 2,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(STitleIcon);
            STitleLabel = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "STitleLabel",
                Priority = 0,
                WinLeft = 25,
                WinTop = 4,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 25,
                    RightOffset = 0,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "World Map",
                TextColor = 0
            };
            LayoutSession.EndRead(STitleLabel);
            SMinIcon = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SMinIcon",
                Priority = 1000,
                WinLeft = 180,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Minimizes the window"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 5,
                    TopOffset = 1,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 4,
                ILIndexLit = 5,
                ILIndexUp = 3,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 15,
                OnClick = Layout_SMinIconClick_6
            };
            LayoutSession.EndRead(SMinIcon);
            Help = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Help",
                Priority = 0,
                WinLeft = 165,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Displays Help for this window."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 20,
                    TopOffset = 1,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 60,
                ILIndexLit = 61,
                ILIndexUp = 59,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 15,
                OnClick = Layout_HelpClick_7
            };
            LayoutSession.EndRead(Help);
            Translator = new Aow.Layout.LayoutTIvTranslator(this)
            {
                Name = "Translator",
                DictionaryName = "AoW",
                TargetsData =
                {
                    new TranslationTarget("", "Items", 0),
                    new TranslationTarget("", "Text", 0),
                    new TranslationTarget("", "ToolTipText", 0)
                }
            };
            LayoutSession.EndRead(Translator);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            ScannerFrame.AOWWindow = ScannerWindow;
            ScannerTab.AOWWindow = ScannerWindow;
            BuildingCheck.AOWWindow = ScannerWindow;
            ArmyCheck.AOWWindow = ScannerWindow;
            NaturalCheck.AOWWindow = ScannerWindow;
            Scanner.AOWWindow = ScannerWindow;
            STitleIcon.AOWWindow = ScannerWindow;
            STitleLabel.AOWWindow = ScannerWindow;
            SMinIcon.AOWWindow = ScannerWindow;
            Help.AOWWindow = ScannerWindow;
            Loaded();
            ScannerWindow.Loaded();
            ScannerFrame.Loaded();
            ScannerTab.Loaded();
            BuildingCheck.Loaded();
            ArmyCheck.Loaded();
            NaturalCheck.Loaded();
            Scanner.Loaded();
            STitleIcon.Loaded();
            STitleLabel.Loaded();
            SMinIcon.Loaded();
            Help.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            ScannerWindow.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            ScannerWindow.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ScannerWindow.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ScannerFrame.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanWin;
            ScannerTab.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ScannerTab.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ScannerTab.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanWin;
            BuildingCheck.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            BuildingCheck.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanWin;
            ArmyCheck.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ArmyCheck.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanWin;
            NaturalCheck.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            NaturalCheck.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanWin;
            Scanner.TilesImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanTiles;
            Scanner.HiLightsImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanHiLites;
            Scanner.FogImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanFog;
            Scanner.ILObjects = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanObj;
            Scanner.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanWin;
            STitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            STitleLabel.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SMinIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SMinIcon.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Help.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Help.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
        }

        private void Layout_ScannerWindowResize_0(TObject p0)
        {
            ScannerWindowResize();
        }

        private void Layout_ScannerTabChange_1(TObject p0)
        {
            ScannerTabChange();
        }

        private void Layout_BuildingCheckChange_2(TObject p0)
        {
            BuildingCheckChange((Aow.Units.AoWCheckBox.TAOWCheckBox)p0);
        }

        private void Layout_ArmyCheckChange_3(TObject p0)
        {
            ArmyCheckChange((Aow.Units.AoWCheckBox.TAOWCheckBox)p0);
        }

        private void Layout_NaturalCheckChange_4(TObject p0)
        {
            NaturalCheckChange((Aow.Units.AoWCheckBox.TAOWCheckBox)p0);
        }

        private void Layout_ScannerChangeTileBox_5(TObject p0)
        {
            ScannerChangeTileBox();
        }

        private void Layout_SMinIconClick_6(TObject p0)
        {
            SMinIconClick();
        }

        private void Layout_HelpClick_7(TObject p0)
        {
            HelpClick();
        }
    }
}
