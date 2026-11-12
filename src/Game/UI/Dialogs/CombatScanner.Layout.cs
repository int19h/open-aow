// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class CombatScanner : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "TCScanner";
        TCScannerWin = new Window(this);
        session.BeginRead(TCScannerWin);
        TCScannerWin.Name = "TCScannerWin";
        TCScannerWin.Visible = false;
        TCScannerWin.Priority = 10;
        TCScannerWin.WinLeft = 490;
        TCScannerWin.WinTop = 0;
        TCScannerWin.WinWidth = 150;
        TCScannerWin.Masked = false;
        TCScannerWin.Alignment.AlignWidth = AlignWidth.Right;
        TCScannerWin.Alignment.AlignHeight = AlignHeight.Top;
        TCScannerWin.Alignment.HeightPercent = 0;
        TCScannerWin.Alignment.WidthPercent = 0;
        TCScannerWin.Alignment.LeftOffsetPercent = 0;
        TCScannerWin.Alignment.RightOffsetPercent = 0;
        TCScannerWin.Alignment.TopOffsetPercent = 0;
        TCScannerWin.Alignment.BottomOffsetPercent = 0;
        TCScannerWin.Alignment.LeftOffset = 0;
        TCScannerWin.Alignment.RightOffset = 0;
        TCScannerWin.Alignment.TopOffset = 0;
        TCScannerWin.Alignment.BottomOffset = 0;
        TCScannerWin.UseVideoMemory = true;
        TCScannerWin.BorderWidth = 4;
        TCScannerWin.BorderHeight = 4;
        TCScannerWin.OverlapTopCorners = false;
        TCScannerWin.Cursor = 0;
        TCScannerWin.DoubleBuffered = true;
        TCScannerWin.MaskColor = 0;
        TCScannerWin.FreeSurfaceOnHide = false;
        TCScannerWin.ImageLibraryIndexFrame = 0;
        TCScannerWin.ImageLibraryIndexTexture = 2;
        TCScannerWin.DragHeight = 20;
        TCScannerWin.Draggable = true;
        TCScannerWin.Resizable = true;
        TCScannerWin.OnDrag = _ => Layout_TCScannerWinDrag_0();
        TCScannerWin.OnResize = _ => Layout_TCScannerWinDrag_0();
        TCScannerWin.MinWidth = 150;
        TCScannerWin.MinHeight = 170;
        TCScannerWin.MaxWidth = 0;
        TCScannerWin.MaxHeight = 0;
        TCScannerWin.UseManagerBorders = true;
        TCScannerWin.Modal = false;
        TCScannerWin.WinHeight = 170;
        TCScannerWin.Dockable = false;
        session.EndRead(TCScannerWin);
        Frame scannerFr = new Frame(this);
        session.BeginRead(scannerFr);
        scannerFr.Name = "ScannerFr";
        scannerFr.Priority = 0;
        scannerFr.WinLeft = 6;
        scannerFr.WinTop = 20;
        scannerFr.CaptureInput = false;
        scannerFr.Alignment.AlignWidth = AlignWidth.Both;
        scannerFr.Alignment.AlignHeight = AlignHeight.Both;
        scannerFr.Alignment.HeightPercent = 0;
        scannerFr.Alignment.WidthPercent = 0;
        scannerFr.Alignment.LeftOffsetPercent = 0;
        scannerFr.Alignment.RightOffsetPercent = 0;
        scannerFr.Alignment.TopOffsetPercent = 0;
        scannerFr.Alignment.BottomOffsetPercent = 0;
        scannerFr.Alignment.LeftOffset = 6;
        scannerFr.Alignment.RightOffset = 6;
        scannerFr.Alignment.TopOffset = 20;
        scannerFr.Alignment.BottomOffset = 6;
        scannerFr.Masked = false;
        scannerFr.WinWidth = 138;
        scannerFr.WinHeight = 144;
        scannerFr.FrameType = FrameType.HollowBox;
        scannerFr.ImageLibraryIndexFrame = 40;
        scannerFr.ImageLibraryIndexLine = 0;
        scannerFr.ImageLibraryIndexTexture = 0;
        session.EndRead(scannerFr);
        Scanner = new Scanner(this);
        session.BeginRead(Scanner);
        Scanner.Name = "Scanner";
        Scanner.Priority = 5;
        Scanner.WinLeft = 7;
        Scanner.WinTop = 21;
        Scanner.CaptureInput = true;
        Scanner.Alignment.AlignWidth = AlignWidth.Both;
        Scanner.Alignment.AlignHeight = AlignHeight.Both;
        Scanner.Alignment.HeightPercent = 0;
        Scanner.Alignment.WidthPercent = 0;
        Scanner.Alignment.LeftOffsetPercent = 0;
        Scanner.Alignment.RightOffsetPercent = 0;
        Scanner.Alignment.TopOffsetPercent = 0;
        Scanner.Alignment.BottomOffsetPercent = 0;
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
        Scanner.OnChangeTileBox = _ => Layout_ScannerChangeTileBox_1();
        Scanner.MapDrawSteps = 5;
        Scanner.ImageLibraryIndexFrame = 20;
        session.EndRead(Scanner);
        Image sTitleIcon = new Image(this);
        session.BeginRead(sTitleIcon);
        sTitleIcon.Name = "STitleIcon";
        sTitleIcon.Priority = 0;
        sTitleIcon.WinLeft = 2;
        sTitleIcon.WinTop = 2;
        sTitleIcon.CaptureInput = false;
        sTitleIcon.Alignment.AlignWidth = AlignWidth.Left;
        sTitleIcon.Alignment.AlignHeight = AlignHeight.Top;
        sTitleIcon.Alignment.HeightPercent = 0;
        sTitleIcon.Alignment.WidthPercent = 0;
        sTitleIcon.Alignment.LeftOffsetPercent = 0;
        sTitleIcon.Alignment.RightOffsetPercent = 0;
        sTitleIcon.Alignment.TopOffsetPercent = 0;
        sTitleIcon.Alignment.BottomOffsetPercent = 0;
        sTitleIcon.Alignment.LeftOffset = 2;
        sTitleIcon.Alignment.RightOffset = 0;
        sTitleIcon.Alignment.TopOffset = 2;
        sTitleIcon.Alignment.BottomOffset = 0;
        sTitleIcon.Masked = false;
        sTitleIcon.AnimationStart = 0;
        sTitleIcon.CurrentImage = 9;
        sTitleIcon.AnimationEnd = 11;
        sTitleIcon.WinWidth = 19;
        sTitleIcon.WinHeight = 16;
        sTitleIcon.Animate = false;
        sTitleIcon.AnimationFrameDelay = 0;
        session.EndRead(sTitleIcon);
        Label sTitleLabel = new Label(this);
        session.BeginRead(sTitleLabel);
        sTitleLabel.Name = "STitleLabel";
        sTitleLabel.Priority = 0;
        sTitleLabel.WinLeft = 25;
        sTitleLabel.WinTop = 4;
        sTitleLabel.CaptureInput = false;
        sTitleLabel.Alignment.AlignWidth = AlignWidth.Left;
        sTitleLabel.Alignment.AlignHeight = AlignHeight.Top;
        sTitleLabel.Alignment.HeightPercent = 0;
        sTitleLabel.Alignment.WidthPercent = 0;
        sTitleLabel.Alignment.LeftOffsetPercent = 0;
        sTitleLabel.Alignment.RightOffsetPercent = 0;
        sTitleLabel.Alignment.TopOffsetPercent = 0;
        sTitleLabel.Alignment.BottomOffsetPercent = 0;
        sTitleLabel.Alignment.LeftOffset = 25;
        sTitleLabel.Alignment.RightOffset = 0;
        sTitleLabel.Alignment.TopOffset = 4;
        sTitleLabel.Alignment.BottomOffset = 0;
        sTitleLabel.Masked = true;
        sTitleLabel.Text = "Battle Map";
        sTitleLabel.TextColor = 0;
        session.EndRead(sTitleLabel);
        Button sMinIcon = new Button(this);
        session.BeginRead(sMinIcon);
        sMinIcon.Name = "SMinIcon";
        sMinIcon.Priority = 1000;
        sMinIcon.WinLeft = 130;
        sMinIcon.WinTop = 1;
        sMinIcon.LayoutToolTipText = new LayoutStrings()
        {
            "Minimizes the window."
        };
        sMinIcon.CaptureInput = true;
        sMinIcon.Alignment.AlignWidth = AlignWidth.Right;
        sMinIcon.Alignment.AlignHeight = AlignHeight.Top;
        sMinIcon.Alignment.HeightPercent = 0;
        sMinIcon.Alignment.WidthPercent = 0;
        sMinIcon.Alignment.LeftOffsetPercent = 0;
        sMinIcon.Alignment.RightOffsetPercent = 0;
        sMinIcon.Alignment.TopOffsetPercent = 0;
        sMinIcon.Alignment.BottomOffsetPercent = 0;
        sMinIcon.Alignment.LeftOffset = 0;
        sMinIcon.Alignment.RightOffset = 5;
        sMinIcon.Alignment.TopOffset = 1;
        sMinIcon.Alignment.BottomOffset = 0;
        sMinIcon.Masked = false;
        sMinIcon.BorderHeight = 0;
        sMinIcon.BorderHeightSelected = 0;
        sMinIcon.ImageLibraryIndexDown = 4;
        sMinIcon.ImageLibraryIndexLit = 5;
        sMinIcon.ImageLibraryIndexUp = 3;
        sMinIcon.TextColor = 0;
        sMinIcon.TextSelectedColor = 0;
        sMinIcon.WinWidth = 15;
        sMinIcon.OnClick = _ => Layout_SMinIconClick_2();
        session.EndRead(sMinIcon);
        LayoutIvTranslator translator = new LayoutIvTranslator(this);
        session.BeginRead(translator);
        translator.Name = "Translator";
        translator.DictionaryName = "AoW";
        translator.TargetsData.Add(new TranslationTarget("", "Items", 0));
        translator.TargetsData.Add(new TranslationTarget("", "Text", 0));
        translator.TargetsData.Add(new TranslationTarget("", "ToolTipText", 0));
        session.EndRead(translator);
        session.EndRead(this);
        scannerFr.Window = TCScannerWin;
        Scanner.Window = TCScannerWin;
        sTitleIcon.Window = TCScannerWin;
        sTitleLabel.Window = TCScannerWin;
        sMinIcon.Window = TCScannerWin;
        Loaded();
        TCScannerWin.Loaded();
        scannerFr.Loaded();
        Scanner.Loaded();
        sTitleIcon.Loaded();
        sTitleLabel.Loaded();
        sMinIcon.Loaded();
        translator.Loaded();
        session.Resolve(this);
        TCScannerWin.WindowManager = DialogHosts.General.Manager;
        TCScannerWin.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        TCScannerWin.ImageLibraryTexture = DialogHosts.UIGraphics.GenericT;
        scannerFr.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        Scanner.TilesImageLibrary = DialogHosts.UIGraphics.ScanTiles;
        Scanner.HighlightsImageLibrary = DialogHosts.UIGraphics.ScanHighlights;
        Scanner.FogImageLibrary = DialogHosts.UIGraphics.ScanFog;
        Scanner.ImageLibraryObjects = DialogHosts.UIGraphics.ScanObj;
        Scanner.ImageLibraryFrame = DialogHosts.UIGraphics.ScanWin;
        sTitleIcon.ImageLibrary = DialogHosts.UIGraphics.TitleIcons;
        sTitleLabel.Font = DialogHosts.UIFonts.Age10;
        sMinIcon.ToolTip = DialogHosts.General.ToolTip;
        sMinIcon.ImageLibraryIcon = DialogHosts.UIGraphics.GenericI;
    }

    private void Layout_TCScannerWinDrag_0()
    {
        TCScannerWinDrag();
    }

    private void Layout_ScannerChangeTileBox_1()
    {
        ScannerChangeTileBox();
    }

    private void Layout_SMinIconClick_2()
    {
        SMinIconClick();
    }
}
