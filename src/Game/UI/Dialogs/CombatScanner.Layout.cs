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
        CombatScannerWindow = new Window(this);
        session.BeginRead(CombatScannerWindow);
        CombatScannerWindow.Name = "TCScannerWin";
        CombatScannerWindow.Visible = false;
        CombatScannerWindow.Priority = 10;
        CombatScannerWindow.WinLeft = 490;
        CombatScannerWindow.WinTop = 0;
        CombatScannerWindow.WinWidth = 150;
        CombatScannerWindow.Masked = false;
        CombatScannerWindow.Alignment.AlignWidth = AlignWidth.Right;
        CombatScannerWindow.Alignment.AlignHeight = AlignHeight.Top;
        CombatScannerWindow.Alignment.HeightPercent = 0;
        CombatScannerWindow.Alignment.WidthPercent = 0;
        CombatScannerWindow.Alignment.LeftOffsetPercent = 0;
        CombatScannerWindow.Alignment.RightOffsetPercent = 0;
        CombatScannerWindow.Alignment.TopOffsetPercent = 0;
        CombatScannerWindow.Alignment.BottomOffsetPercent = 0;
        CombatScannerWindow.Alignment.LeftOffset = 0;
        CombatScannerWindow.Alignment.RightOffset = 0;
        CombatScannerWindow.Alignment.TopOffset = 0;
        CombatScannerWindow.Alignment.BottomOffset = 0;
        CombatScannerWindow.UseVideoMemory = true;
        CombatScannerWindow.BorderWidth = 4;
        CombatScannerWindow.BorderHeight = 4;
        CombatScannerWindow.OverlapTopCorners = false;
        CombatScannerWindow.Cursor = 0;
        CombatScannerWindow.DoubleBuffered = true;
        CombatScannerWindow.MaskColor = 0;
        CombatScannerWindow.FreeSurfaceOnHide = false;
        CombatScannerWindow.ImageLibraryIndexFrame = 0;
        CombatScannerWindow.ImageLibraryIndexTexture = 2;
        CombatScannerWindow.DragHeight = 20;
        CombatScannerWindow.Draggable = true;
        CombatScannerWindow.Resizable = true;
        CombatScannerWindow.OnDrag = _ => Layout_TCScannerWinDrag_0();
        CombatScannerWindow.OnResize = _ => Layout_TCScannerWinDrag_0();
        CombatScannerWindow.MinWidth = 150;
        CombatScannerWindow.MinHeight = 170;
        CombatScannerWindow.MaxWidth = 0;
        CombatScannerWindow.MaxHeight = 0;
        CombatScannerWindow.UseManagerBorders = true;
        CombatScannerWindow.Modal = false;
        CombatScannerWindow.WinHeight = 170;
        CombatScannerWindow.Dockable = false;
        session.EndRead(CombatScannerWindow);
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
        scannerFr.Window = CombatScannerWindow;
        Scanner.Window = CombatScannerWindow;
        sTitleIcon.Window = CombatScannerWindow;
        sTitleLabel.Window = CombatScannerWindow;
        sMinIcon.Window = CombatScannerWindow;
        Loaded();
        CombatScannerWindow.Loaded();
        scannerFr.Loaded();
        Scanner.Loaded();
        sTitleIcon.Loaded();
        sTitleLabel.Loaded();
        sMinIcon.Loaded();
        translator.Loaded();
        session.Resolve(this);
        CombatScannerWindow.WindowManager = DialogHosts.General.Manager;
        CombatScannerWindow.ImageLibraryFrame = DialogHosts.UIGraphics.GenericFrames;
        CombatScannerWindow.ImageLibraryTexture = DialogHosts.UIGraphics.GenericTextures;
        scannerFr.ImageLibraryFrame = DialogHosts.UIGraphics.GenericFrames;
        Scanner.TilesImageLibrary = DialogHosts.UIGraphics.ScannerTiles;
        Scanner.HighlightsImageLibrary = DialogHosts.UIGraphics.ScannerHighlights;
        Scanner.FogImageLibrary = DialogHosts.UIGraphics.ScannerFog;
        Scanner.ImageLibraryObjects = DialogHosts.UIGraphics.ScannerObjects;
        Scanner.ImageLibraryFrame = DialogHosts.UIGraphics.ScannerWindow;
        sTitleIcon.ImageLibrary = DialogHosts.UIGraphics.TitleIcons;
        sTitleLabel.Font = DialogHosts.UIFonts.Age10;
        sMinIcon.ToolTip = DialogHosts.General.ToolTip;
        sMinIcon.ImageLibraryIcon = DialogHosts.UIGraphics.GenericIcons;
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
