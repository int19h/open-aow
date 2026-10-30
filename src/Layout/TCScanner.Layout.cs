// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWTCScanner
{
    public partial class TTCScanner : ILayoutModule
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "TCScanner";
            TCScannerWin = Aow.Units.AoWWindow.TAOWWindow.Create(this);
            session.BeginRead(TCScannerWin);
            TCScannerWin.Name = "TCScannerWin";
            TCScannerWin.Visible = false;
            TCScannerWin.Priority = 10;
            TCScannerWin.WinLeft = 490;
            TCScannerWin.WinTop = 0;
            TCScannerWin.WinWidth = 150;
            TCScannerWin.Masked = false;
            TCScannerWin.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight;
            TCScannerWin.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
            TCScannerWin.Alignment.HeightPercent = (byte)(0);
            TCScannerWin.Alignment.WidthPercent = (byte)(0);
            TCScannerWin.Alignment.LeftOffPercent = 0;
            TCScannerWin.Alignment.RightOffPercent = 0;
            TCScannerWin.Alignment.TopOffPercent = 0;
            TCScannerWin.Alignment.BottomOffPercent = 0;
            TCScannerWin.Alignment.LeftOffset = 0;
            TCScannerWin.Alignment.RightOffset = 0;
            TCScannerWin.Alignment.TopOffset = 0;
            TCScannerWin.Alignment.BottomOffset = 0;
            TCScannerWin.UseVMem = true;
            TCScannerWin.BorderWidth = 4;
            TCScannerWin.BorderHeight = 4;
            TCScannerWin.OverlapTopCorners = false;
            TCScannerWin.Cursor = 0;
            TCScannerWin.DoubleBuffered = true;
            TCScannerWin.MaskColor = 0;
            TCScannerWin.FreeSurfaceOnHide = false;
            TCScannerWin.ILIndexFrame = 0;
            TCScannerWin.ILIndexTexture = 2;
            TCScannerWin.DragHeight = 20;
            TCScannerWin.Draggable = true;
            TCScannerWin.Resizable = true;
            TCScannerWin.OnDrag = Layout_TCScannerWinDrag_0;
            TCScannerWin.OnResize = Layout_TCScannerWinDrag_0;
            TCScannerWin.MinWidth = 150;
            TCScannerWin.MinHeight = 170;
            TCScannerWin.MaxWidth = 0;
            TCScannerWin.MaxHeight = 0;
            TCScannerWin.UseManagerBorders = true;
            TCScannerWin.Modal = false;
            TCScannerWin.WinHeight = 170;
            TCScannerWin.Dockable = false;
            session.EndRead(TCScannerWin);
            ScannerFr = new Aow.Units.AoWFrame.TAoWFrame(this);
            session.BeginRead(ScannerFr);
            ScannerFr.Name = "ScannerFr";
            ScannerFr.Priority = 0;
            ScannerFr.WinLeft = 6;
            ScannerFr.WinTop = 20;
            ScannerFr.CaptureInput = false;
            ScannerFr.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
            ScannerFr.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth;
            ScannerFr.Alignment.HeightPercent = (byte)(0);
            ScannerFr.Alignment.WidthPercent = (byte)(0);
            ScannerFr.Alignment.LeftOffPercent = 0;
            ScannerFr.Alignment.RightOffPercent = 0;
            ScannerFr.Alignment.TopOffPercent = 0;
            ScannerFr.Alignment.BottomOffPercent = 0;
            ScannerFr.Alignment.LeftOffset = 6;
            ScannerFr.Alignment.RightOffset = 6;
            ScannerFr.Alignment.TopOffset = 20;
            ScannerFr.Alignment.BottomOffset = 6;
            ScannerFr.Masked = false;
            ScannerFr.WinWidth = 138;
            ScannerFr.WinHeight = 144;
            ScannerFr.FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox;
            ScannerFr.ILIndexFrame = 40;
            ScannerFr.ILIndexLine = 0;
            ScannerFr.ILIndexTexture = 0;
            session.EndRead(ScannerFr);
            Scanner = Aow.Units.Scanner.TScanner.Create(this);
            session.BeginRead(Scanner);
            Scanner.Name = "Scanner";
            Scanner.Priority = 5;
            Scanner.WinLeft = 7;
            Scanner.WinTop = 21;
            Scanner.CaptureInput = true;
            Scanner.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
            Scanner.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth;
            Scanner.Alignment.HeightPercent = (byte)(0);
            Scanner.Alignment.WidthPercent = (byte)(0);
            Scanner.Alignment.LeftOffPercent = 0;
            Scanner.Alignment.RightOffPercent = 0;
            Scanner.Alignment.TopOffPercent = 0;
            Scanner.Alignment.BottomOffPercent = 0;
            Scanner.Alignment.LeftOffset = 7;
            Scanner.Alignment.RightOffset = 7;
            Scanner.Alignment.TopOffset = 21;
            Scanner.Alignment.BottomOffset = 7;
            Scanner.Masked = false;
            Scanner.WinWidth = 136;
            Scanner.WinHeight = 142;
            Scanner.TileBoxTop = -1;
            Scanner.TileBoxLeft = -1;
            Scanner.TileBoxWidth = -1;
            Scanner.TileBoxHeight = -1;
            Scanner.HighLights = true;
            Scanner.Buildings = true;
            Scanner.Armies = true;
            Scanner.Naturals = true;
            Scanner.OnChangeTileBox = Layout_ScannerChangeTileBox_1;
            Scanner.MapDrawSteps = 5;
            Scanner.ILIndexFrame = 20;
            session.EndRead(Scanner);
            STitleIcon = new Aow.Units.AoWImage.TAOWImage(this);
            session.BeginRead(STitleIcon);
            STitleIcon.Name = "STitleIcon";
            STitleIcon.Priority = 0;
            STitleIcon.WinLeft = 2;
            STitleIcon.WinTop = 2;
            STitleIcon.CaptureInput = false;
            STitleIcon.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
            STitleIcon.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
            STitleIcon.Alignment.HeightPercent = (byte)(0);
            STitleIcon.Alignment.WidthPercent = (byte)(0);
            STitleIcon.Alignment.LeftOffPercent = 0;
            STitleIcon.Alignment.RightOffPercent = 0;
            STitleIcon.Alignment.TopOffPercent = 0;
            STitleIcon.Alignment.BottomOffPercent = 0;
            STitleIcon.Alignment.LeftOffset = 2;
            STitleIcon.Alignment.RightOffset = 0;
            STitleIcon.Alignment.TopOffset = 2;
            STitleIcon.Alignment.BottomOffset = 0;
            STitleIcon.Masked = false;
            STitleIcon.AnimStart = 0;
            STitleIcon.CurrentImage = 9;
            STitleIcon.AnimEnd = 11;
            STitleIcon.WinWidth = 19;
            STitleIcon.WinHeight = 16;
            STitleIcon.Animate = false;
            STitleIcon.AnimFrameDelay = 0;
            session.EndRead(STitleIcon);
            STitleLabel = new Aow.Units.AoWLabel.TAOWLabel(this);
            session.BeginRead(STitleLabel);
            STitleLabel.Name = "STitleLabel";
            STitleLabel.Priority = 0;
            STitleLabel.WinLeft = 25;
            STitleLabel.WinTop = 4;
            STitleLabel.CaptureInput = false;
            STitleLabel.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
            STitleLabel.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
            STitleLabel.Alignment.HeightPercent = (byte)(0);
            STitleLabel.Alignment.WidthPercent = (byte)(0);
            STitleLabel.Alignment.LeftOffPercent = 0;
            STitleLabel.Alignment.RightOffPercent = 0;
            STitleLabel.Alignment.TopOffPercent = 0;
            STitleLabel.Alignment.BottomOffPercent = 0;
            STitleLabel.Alignment.LeftOffset = 25;
            STitleLabel.Alignment.RightOffset = 0;
            STitleLabel.Alignment.TopOffset = 4;
            STitleLabel.Alignment.BottomOffset = 0;
            STitleLabel.Masked = true;
            STitleLabel.Text = "Battle Map";
            STitleLabel.TextColor = 0;
            session.EndRead(STitleLabel);
            SMinIcon = new Aow.Units.AoWButton.TAOWButton(this);
            session.BeginRead(SMinIcon);
            SMinIcon.Name = "SMinIcon";
            SMinIcon.Priority = 1000;
            SMinIcon.WinLeft = 130;
            SMinIcon.WinTop = 1;
            SMinIcon.LayoutToolTipText = new LayoutStrings()
            {
                "Minimizes the window."
            };
            SMinIcon.CaptureInput = true;
            SMinIcon.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight;
            SMinIcon.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
            SMinIcon.Alignment.HeightPercent = (byte)(0);
            SMinIcon.Alignment.WidthPercent = (byte)(0);
            SMinIcon.Alignment.LeftOffPercent = 0;
            SMinIcon.Alignment.RightOffPercent = 0;
            SMinIcon.Alignment.TopOffPercent = 0;
            SMinIcon.Alignment.BottomOffPercent = 0;
            SMinIcon.Alignment.LeftOffset = 0;
            SMinIcon.Alignment.RightOffset = 5;
            SMinIcon.Alignment.TopOffset = 1;
            SMinIcon.Alignment.BottomOffset = 0;
            SMinIcon.Masked = false;
            SMinIcon.BorderHeight = 0;
            SMinIcon.BorderHeightSelected = 0;
            SMinIcon.ILIndexDown = 4;
            SMinIcon.ILIndexLit = 5;
            SMinIcon.ILIndexUp = 3;
            SMinIcon.TextColor = 0;
            SMinIcon.TextSelColor = 0;
            SMinIcon.WinWidth = 15;
            SMinIcon.OnClick = Layout_SMinIconClick_2;
            session.EndRead(SMinIcon);
            var created__Translator = new Aow.Layout.LayoutTIvTranslator(this);
            Translator = created__Translator;
            session.BeginRead(created__Translator);
            created__Translator.Name = "Translator";
            created__Translator.DictionaryName = "AoW";
            created__Translator.TargetsData.Add(new TranslationTarget("", "Items", 0));
            created__Translator.TargetsData.Add(new TranslationTarget("", "Text", 0));
            created__Translator.TargetsData.Add(new TranslationTarget("", "ToolTipText", 0));
            session.EndRead(created__Translator);
            session.EndRead(this);
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
