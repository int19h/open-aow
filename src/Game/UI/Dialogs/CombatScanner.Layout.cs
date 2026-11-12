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
        ScannerFr = new Frame(this);
        session.BeginRead(ScannerFr);
        ScannerFr.Name = "ScannerFr";
        ScannerFr.Priority = 0;
        ScannerFr.WinLeft = 6;
        ScannerFr.WinTop = 20;
        ScannerFr.CaptureInput = false;
        ScannerFr.Alignment.AlignWidth = AlignWidth.Both;
        ScannerFr.Alignment.AlignHeight = AlignHeight.Both;
        ScannerFr.Alignment.HeightPercent = 0;
        ScannerFr.Alignment.WidthPercent = 0;
        ScannerFr.Alignment.LeftOffsetPercent = 0;
        ScannerFr.Alignment.RightOffsetPercent = 0;
        ScannerFr.Alignment.TopOffsetPercent = 0;
        ScannerFr.Alignment.BottomOffsetPercent = 0;
        ScannerFr.Alignment.LeftOffset = 6;
        ScannerFr.Alignment.RightOffset = 6;
        ScannerFr.Alignment.TopOffset = 20;
        ScannerFr.Alignment.BottomOffset = 6;
        ScannerFr.Masked = false;
        ScannerFr.WinWidth = 138;
        ScannerFr.WinHeight = 144;
        ScannerFr.FrameType = FrameType.HollowBox;
        ScannerFr.ImageLibraryIndexFrame = 40;
        ScannerFr.ImageLibraryIndexLine = 0;
        ScannerFr.ImageLibraryIndexTexture = 0;
        session.EndRead(ScannerFr);
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
        STitleIcon = new Image(this);
        session.BeginRead(STitleIcon);
        STitleIcon.Name = "STitleIcon";
        STitleIcon.Priority = 0;
        STitleIcon.WinLeft = 2;
        STitleIcon.WinTop = 2;
        STitleIcon.CaptureInput = false;
        STitleIcon.Alignment.AlignWidth = AlignWidth.Left;
        STitleIcon.Alignment.AlignHeight = AlignHeight.Top;
        STitleIcon.Alignment.HeightPercent = 0;
        STitleIcon.Alignment.WidthPercent = 0;
        STitleIcon.Alignment.LeftOffsetPercent = 0;
        STitleIcon.Alignment.RightOffsetPercent = 0;
        STitleIcon.Alignment.TopOffsetPercent = 0;
        STitleIcon.Alignment.BottomOffsetPercent = 0;
        STitleIcon.Alignment.LeftOffset = 2;
        STitleIcon.Alignment.RightOffset = 0;
        STitleIcon.Alignment.TopOffset = 2;
        STitleIcon.Alignment.BottomOffset = 0;
        STitleIcon.Masked = false;
        STitleIcon.AnimationStart = 0;
        STitleIcon.CurrentImage = 9;
        STitleIcon.AnimationEnd = 11;
        STitleIcon.WinWidth = 19;
        STitleIcon.WinHeight = 16;
        STitleIcon.Animate = false;
        STitleIcon.AnimationFrameDelay = 0;
        session.EndRead(STitleIcon);
        STitleLabel = new Label(this);
        session.BeginRead(STitleLabel);
        STitleLabel.Name = "STitleLabel";
        STitleLabel.Priority = 0;
        STitleLabel.WinLeft = 25;
        STitleLabel.WinTop = 4;
        STitleLabel.CaptureInput = false;
        STitleLabel.Alignment.AlignWidth = AlignWidth.Left;
        STitleLabel.Alignment.AlignHeight = AlignHeight.Top;
        STitleLabel.Alignment.HeightPercent = 0;
        STitleLabel.Alignment.WidthPercent = 0;
        STitleLabel.Alignment.LeftOffsetPercent = 0;
        STitleLabel.Alignment.RightOffsetPercent = 0;
        STitleLabel.Alignment.TopOffsetPercent = 0;
        STitleLabel.Alignment.BottomOffsetPercent = 0;
        STitleLabel.Alignment.LeftOffset = 25;
        STitleLabel.Alignment.RightOffset = 0;
        STitleLabel.Alignment.TopOffset = 4;
        STitleLabel.Alignment.BottomOffset = 0;
        STitleLabel.Masked = true;
        STitleLabel.Text = "Battle Map";
        STitleLabel.TextColor = 0;
        session.EndRead(STitleLabel);
        SMinIcon = new Button(this);
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
        SMinIcon.Alignment.AlignWidth = AlignWidth.Right;
        SMinIcon.Alignment.AlignHeight = AlignHeight.Top;
        SMinIcon.Alignment.HeightPercent = 0;
        SMinIcon.Alignment.WidthPercent = 0;
        SMinIcon.Alignment.LeftOffsetPercent = 0;
        SMinIcon.Alignment.RightOffsetPercent = 0;
        SMinIcon.Alignment.TopOffsetPercent = 0;
        SMinIcon.Alignment.BottomOffsetPercent = 0;
        SMinIcon.Alignment.LeftOffset = 0;
        SMinIcon.Alignment.RightOffset = 5;
        SMinIcon.Alignment.TopOffset = 1;
        SMinIcon.Alignment.BottomOffset = 0;
        SMinIcon.Masked = false;
        SMinIcon.BorderHeight = 0;
        SMinIcon.BorderHeightSelected = 0;
        SMinIcon.ImageLibraryIndexDown = 4;
        SMinIcon.ImageLibraryIndexLit = 5;
        SMinIcon.ImageLibraryIndexUp = 3;
        SMinIcon.TextColor = 0;
        SMinIcon.TextSelectedColor = 0;
        SMinIcon.WinWidth = 15;
        SMinIcon.OnClick = _ => Layout_SMinIconClick_2();
        session.EndRead(SMinIcon);
        LayoutIvTranslator createdTranslator = new LayoutIvTranslator(this);
        Translator = createdTranslator;
        session.BeginRead(createdTranslator);
        createdTranslator.Name = "Translator";
        createdTranslator.DictionaryName = "AoW";
        createdTranslator.TargetsData.Add(new TranslationTarget("", "Items", 0));
        createdTranslator.TargetsData.Add(new TranslationTarget("", "Text", 0));
        createdTranslator.TargetsData.Add(new TranslationTarget("", "ToolTipText", 0));
        session.EndRead(createdTranslator);
        session.EndRead(this);
        ScannerFr.Window = TCScannerWin;
        Scanner.Window = TCScannerWin;
        STitleIcon.Window = TCScannerWin;
        STitleLabel.Window = TCScannerWin;
        SMinIcon.Window = TCScannerWin;
        Loaded();
        TCScannerWin.Loaded();
        ScannerFr.Loaded();
        Scanner.Loaded();
        STitleIcon.Loaded();
        STitleLabel.Loaded();
        SMinIcon.Loaded();
        Translator.Loaded();
        session.Resolve(this);
        TCScannerWin.WindowManager = DialogHosts.General.Manager;
        TCScannerWin.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        TCScannerWin.ImageLibraryTexture = DialogHosts.UIGraphics.GenericT;
        ScannerFr.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        Scanner.TilesImageLibrary = DialogHosts.UIGraphics.ScanTiles;
        Scanner.HighlightsImageLibrary = DialogHosts.UIGraphics.ScanHighlights;
        Scanner.FogImageLibrary = DialogHosts.UIGraphics.ScanFog;
        Scanner.ImageLibraryObjects = DialogHosts.UIGraphics.ScanObj;
        Scanner.ImageLibraryFrame = DialogHosts.UIGraphics.ScanWin;
        STitleIcon.ImageLibrary = DialogHosts.UIGraphics.TitleIcons;
        STitleLabel.Font = DialogHosts.UIFonts.Age10;
        SMinIcon.ToolTip = DialogHosts.General.ToolTip;
        SMinIcon.ImageLibraryIcon = DialogHosts.UIGraphics.GenericI;
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
