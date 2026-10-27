// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWTCScanner
{
    public partial class TTCScanner
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "TCScanner";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            TCScannerWin = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "TCScannerWin",
                Visible = false,
                Priority = 10,
                WinLeft = 490,
                WinTop = 0,
                WinWidth = 150,
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
                OnDrag = Layout_TCScannerWinDrag_0,
                OnResize = Layout_TCScannerWinDrag_0,
                MinWidth = 150,
                MinHeight = 170,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = false,
                WinHeight = 170,
                Dockable = false
            };
            LayoutSession.EndRead(TCScannerWin);
            ScannerFr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "ScannerFr",
                Priority = 0,
                WinLeft = 6,
                WinTop = 20,
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
                    LeftOffset = 6,
                    RightOffset = 6,
                    TopOffset = 20,
                    BottomOffset = 6
                },
                Masked = false,
                WinWidth = 138,
                WinHeight = 144,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(ScannerFr);
            Scanner = new Aow.Units.Scanner.TScanner(this)
            {
                Name = "Scanner",
                Priority = 5,
                WinLeft = 7,
                WinTop = 21,
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
                    LeftOffset = 7,
                    RightOffset = 7,
                    TopOffset = 21,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 136,
                WinHeight = 142,
                TileBoxTop = -1,
                TileBoxLeft = -1,
                TileBoxWidth = -1,
                TileBoxHeight = -1,
                HighLights = true,
                Buildings = true,
                Armies = true,
                Naturals = true,
                OnChangeTileBox = Layout_ScannerChangeTileBox_1,
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
                CurrentImage = 9,
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
                Text = "Battle Map",
                TextColor = 0
            };
            LayoutSession.EndRead(STitleLabel);
            SMinIcon = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SMinIcon",
                Priority = 1000,
                WinLeft = 130,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Minimizes the window."
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
                OnClick = Layout_SMinIconClick_2
            };
            LayoutSession.EndRead(SMinIcon);
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
            ScannerFr.AOWWindow = TCScannerWin;
            Scanner.AOWWindow = TCScannerWin;
            STitleIcon.AOWWindow = TCScannerWin;
            STitleLabel.AOWWindow = TCScannerWin;
            SMinIcon.AOWWindow = TCScannerWin;
            Loaded();
            TCScannerWin.Loaded();
            ScannerFr.Loaded();
            Scanner.Loaded();
            STitleIcon.Loaded();
            STitleLabel.Loaded();
            SMinIcon.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            TCScannerWin.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            TCScannerWin.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            TCScannerWin.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ScannerFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Scanner.TilesImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanTiles;
            Scanner.HiLightsImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanHiLites;
            Scanner.FogImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanFog;
            Scanner.ILObjects = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanObj;
            Scanner.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanWin;
            STitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            STitleLabel.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SMinIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SMinIcon.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
        }

        private void Layout_TCScannerWinDrag_0(TObject p0)
        {
            TCScannerWinDrag();
        }

        private void Layout_ScannerChangeTileBox_1(TObject p0)
        {
            ScannerChangeTileBox();
        }

        private void Layout_SMinIconClick_2(TObject p0)
        {
            SMinIconClick();
        }
    }
}
