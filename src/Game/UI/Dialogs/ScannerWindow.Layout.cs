// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class ScannerWindow : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "SWindow";
        MainWindow = new Window(this);
        session.BeginRead(MainWindow);
        MainWindow.Name = "ScannerWindow";
        MainWindow.Visible = false;
        MainWindow.Priority = 10;
        MainWindow.WinLeft = 440;
        MainWindow.WinTop = 0;
        MainWindow.WinWidth = 200;
        MainWindow.Masked = false;
        MainWindow.Alignment.AlignWidth = AlignWidth.Right;
        MainWindow.Alignment.AlignHeight = AlignHeight.Top;
        MainWindow.Alignment.HeightPercent = 0;
        MainWindow.Alignment.WidthPercent = 0;
        MainWindow.Alignment.LeftOffsetPercent = 0;
        MainWindow.Alignment.RightOffsetPercent = 0;
        MainWindow.Alignment.TopOffsetPercent = 0;
        MainWindow.Alignment.BottomOffsetPercent = 0;
        MainWindow.Alignment.LeftOffset = 0;
        MainWindow.Alignment.RightOffset = 0;
        MainWindow.Alignment.TopOffset = 0;
        MainWindow.Alignment.BottomOffset = 0;
        MainWindow.UseVideoMemory = true;
        MainWindow.BorderWidth = 4;
        MainWindow.BorderHeight = 4;
        MainWindow.OverlapTopCorners = false;
        MainWindow.Cursor = 0;
        MainWindow.DoubleBuffered = true;
        MainWindow.MaskColor = 0;
        MainWindow.FreeSurfaceOnHide = false;
        MainWindow.ImageLibraryIndexFrame = 0;
        MainWindow.ImageLibraryIndexTexture = 2;
        MainWindow.DragHeight = 20;
        MainWindow.Draggable = true;
        MainWindow.Resizable = true;
        MainWindow.OnDrag = _ => Layout_ScannerWindowResize_0();
        MainWindow.OnResize = _ => Layout_ScannerWindowResize_0();
        MainWindow.MinWidth = 200;
        MainWindow.MinHeight = 216;
        MainWindow.MaxWidth = 0;
        MainWindow.MaxHeight = 0;
        MainWindow.UseManagerBorders = true;
        MainWindow.Modal = false;
        MainWindow.WinHeight = 216;
        MainWindow.Dockable = false;
        session.EndRead(MainWindow);
        Frame scannerFrame = new Frame(this);
        session.BeginRead(scannerFrame);
        scannerFrame.Name = "ScannerFrame";
        scannerFrame.Priority = 50;
        scannerFrame.WinLeft = -2;
        scannerFrame.WinTop = 10;
        scannerFrame.CaptureInput = false;
        scannerFrame.Alignment.AlignWidth = AlignWidth.Both;
        scannerFrame.Alignment.AlignHeight = AlignHeight.Both;
        scannerFrame.Alignment.HeightPercent = 0;
        scannerFrame.Alignment.WidthPercent = 0;
        scannerFrame.Alignment.LeftOffsetPercent = 0;
        scannerFrame.Alignment.RightOffsetPercent = 0;
        scannerFrame.Alignment.TopOffsetPercent = 0;
        scannerFrame.Alignment.BottomOffsetPercent = 0;
        scannerFrame.Alignment.LeftOffset = -2;
        scannerFrame.Alignment.RightOffset = 6;
        scannerFrame.Alignment.TopOffset = 10;
        scannerFrame.Alignment.BottomOffset = 4;
        scannerFrame.Masked = false;
        scannerFrame.WinWidth = 196;
        scannerFrame.WinHeight = 202;
        scannerFrame.FrameType = FrameType.HollowBox;
        scannerFrame.ImageLibraryIndexFrame = 0;
        scannerFrame.ImageLibraryIndexLine = 0;
        scannerFrame.ImageLibraryIndexTexture = 0;
        session.EndRead(scannerFrame);
        ScannerTab = new TabPanel(this);
        session.BeginRead(ScannerTab);
        ScannerTab.Name = "ScannerTab";
        ScannerTab.OnMouseDown = (_, button, shift, x, y) => ScannerTabMouseDown(button, shift, x, y);
        ScannerTab.Priority = 10000;
        ScannerTab.WinLeft = 31;
        ScannerTab.WinTop = 182;
        ScannerTab.LayoutToolTipText = new LayoutStrings()
        {
            "Selects one of the three map layers."
        };
        ScannerTab.CaptureInput = true;
        ScannerTab.Alignment.AlignWidth = AlignWidth.Both;
        ScannerTab.Alignment.AlignHeight = AlignHeight.Bottom;
        ScannerTab.Alignment.HeightPercent = 0;
        ScannerTab.Alignment.WidthPercent = 0;
        ScannerTab.Alignment.LeftOffsetPercent = 0;
        ScannerTab.Alignment.RightOffsetPercent = 0;
        ScannerTab.Alignment.TopOffsetPercent = 0;
        ScannerTab.Alignment.BottomOffsetPercent = 0;
        ScannerTab.Alignment.LeftOffset = 31;
        ScannerTab.Alignment.RightOffset = 24;
        ScannerTab.Alignment.TopOffset = 0;
        ScannerTab.Alignment.BottomOffset = 19;
        ScannerTab.Masked = false;
        ScannerTab.BorderHeight = 3;
        ScannerTab.OnChange = _ => Layout_ScannerTabChange_1();
        ScannerTab.Selected = 1;
        ScannerTab.WinWidth = 145;
        ScannerTab.TextColor = 1777703;
        ScannerTab.TextSelectedColor = 0;
        ScannerTab.ImageLibraryIndexDown = 17;
        ScannerTab.ImageLibraryIndexUp = 14;
        ScannerTab.BorderHeightSelected = 3;
        session.EndRead(ScannerTab);
        BuildingCheck = new CheckBox(this);
        session.BeginRead(BuildingCheck);
        BuildingCheck.Name = "BuildingCheck";
        BuildingCheck.Priority = 1001;
        BuildingCheck.WinLeft = 160;
        BuildingCheck.WinTop = 37;
        BuildingCheck.LayoutToolTipText = new LayoutStrings()
        {
            "Toggles player structures on and off."
        };
        BuildingCheck.CaptureInput = true;
        BuildingCheck.Alignment.AlignWidth = AlignWidth.Right;
        BuildingCheck.Alignment.AlignHeight = AlignHeight.Top;
        BuildingCheck.Alignment.HeightPercent = 0;
        BuildingCheck.Alignment.WidthPercent = 0;
        BuildingCheck.Alignment.LeftOffsetPercent = 0;
        BuildingCheck.Alignment.RightOffsetPercent = 0;
        BuildingCheck.Alignment.TopOffsetPercent = 0;
        BuildingCheck.Alignment.BottomOffsetPercent = 0;
        BuildingCheck.Alignment.LeftOffset = 0;
        BuildingCheck.Alignment.RightOffset = 25;
        BuildingCheck.Alignment.TopOffset = 37;
        BuildingCheck.Alignment.BottomOffset = 0;
        BuildingCheck.Masked = false;
        BuildingCheck.BorderHeight = 0;
        BuildingCheck.BorderHeightSelected = 0;
        BuildingCheck.ImageLibraryIndexDown = 9;
        BuildingCheck.ImageLibraryIndexLit = 0;
        BuildingCheck.ImageLibraryIndexUp = 8;
        BuildingCheck.TextColor = 0;
        BuildingCheck.TextSelectedColor = 0;
        BuildingCheck.WinWidth = 15;
        BuildingCheck.Checked = true;
        BuildingCheck.OnChange = BuildingCheckChange;
        BuildingCheck.RadioIndex = 0;
        session.EndRead(BuildingCheck);
        ArmyCheck = new CheckBox(this);
        session.BeginRead(ArmyCheck);
        ArmyCheck.Name = "ArmyCheck";
        ArmyCheck.Priority = 1002;
        ArmyCheck.WinLeft = 160;
        ArmyCheck.WinTop = 52;
        ArmyCheck.LayoutToolTipText = new LayoutStrings()
        {
            "Toggles parties on and off."
        };
        ArmyCheck.CaptureInput = true;
        ArmyCheck.Alignment.AlignWidth = AlignWidth.Right;
        ArmyCheck.Alignment.AlignHeight = AlignHeight.Top;
        ArmyCheck.Alignment.HeightPercent = 0;
        ArmyCheck.Alignment.WidthPercent = 0;
        ArmyCheck.Alignment.LeftOffsetPercent = 0;
        ArmyCheck.Alignment.RightOffsetPercent = 0;
        ArmyCheck.Alignment.TopOffsetPercent = 0;
        ArmyCheck.Alignment.BottomOffsetPercent = 0;
        ArmyCheck.Alignment.LeftOffset = 0;
        ArmyCheck.Alignment.RightOffset = 25;
        ArmyCheck.Alignment.TopOffset = 52;
        ArmyCheck.Alignment.BottomOffset = 0;
        ArmyCheck.Masked = false;
        ArmyCheck.BorderHeight = 0;
        ArmyCheck.BorderHeightSelected = 0;
        ArmyCheck.ImageLibraryIndexDown = 11;
        ArmyCheck.ImageLibraryIndexLit = 0;
        ArmyCheck.ImageLibraryIndexUp = 10;
        ArmyCheck.TextColor = 0;
        ArmyCheck.TextSelectedColor = 0;
        ArmyCheck.WinWidth = 15;
        ArmyCheck.Checked = true;
        ArmyCheck.OnChange = ArmyCheckChange;
        ArmyCheck.RadioIndex = 0;
        session.EndRead(ArmyCheck);
        NaturalCheck = new CheckBox(this);
        session.BeginRead(NaturalCheck);
        NaturalCheck.Name = "NaturalCheck";
        NaturalCheck.Priority = 1003;
        NaturalCheck.WinLeft = 160;
        NaturalCheck.WinTop = 67;
        NaturalCheck.LayoutToolTipText = new LayoutStrings()
        {
            "Toggles natural structures on and off."
        };
        NaturalCheck.CaptureInput = true;
        NaturalCheck.Alignment.AlignWidth = AlignWidth.Right;
        NaturalCheck.Alignment.AlignHeight = AlignHeight.Top;
        NaturalCheck.Alignment.HeightPercent = 0;
        NaturalCheck.Alignment.WidthPercent = 0;
        NaturalCheck.Alignment.LeftOffsetPercent = 0;
        NaturalCheck.Alignment.RightOffsetPercent = 0;
        NaturalCheck.Alignment.TopOffsetPercent = 0;
        NaturalCheck.Alignment.BottomOffsetPercent = 0;
        NaturalCheck.Alignment.LeftOffset = 0;
        NaturalCheck.Alignment.RightOffset = 25;
        NaturalCheck.Alignment.TopOffset = 67;
        NaturalCheck.Alignment.BottomOffset = 0;
        NaturalCheck.Masked = false;
        NaturalCheck.BorderHeight = 0;
        NaturalCheck.BorderHeightSelected = 0;
        NaturalCheck.ImageLibraryIndexDown = 13;
        NaturalCheck.ImageLibraryIndexLit = 0;
        NaturalCheck.ImageLibraryIndexUp = 12;
        NaturalCheck.TextColor = 0;
        NaturalCheck.TextSelectedColor = 0;
        NaturalCheck.WinWidth = 15;
        NaturalCheck.Checked = true;
        NaturalCheck.OnChange = NaturalCheckChange;
        NaturalCheck.RadioIndex = 0;
        session.EndRead(NaturalCheck);
        Scanner = new Scanner(this);
        session.BeginRead(Scanner);
        Scanner.Name = "Scanner";
        Scanner.Priority = 5;
        Scanner.WinLeft = 30;
        Scanner.WinTop = 32;
        Scanner.CaptureInput = true;
        Scanner.Alignment.AlignWidth = AlignWidth.Both;
        Scanner.Alignment.AlignHeight = AlignHeight.Both;
        Scanner.Alignment.HeightPercent = 0;
        Scanner.Alignment.WidthPercent = 0;
        Scanner.Alignment.LeftOffsetPercent = 0;
        Scanner.Alignment.RightOffsetPercent = 0;
        Scanner.Alignment.TopOffsetPercent = 0;
        Scanner.Alignment.BottomOffsetPercent = 0;
        Scanner.Alignment.LeftOffset = 30;
        Scanner.Alignment.RightOffset = 39;
        Scanner.Alignment.TopOffset = 32;
        Scanner.Alignment.BottomOffset = 32;
        Scanner.Masked = false;
        Scanner.WinWidth = 131;
        Scanner.WinHeight = 152;
        Scanner.TileBoxTop = -1;
        Scanner.TileBoxLeft = -1;
        Scanner.TileBoxWidth = -1;
        Scanner.TileBoxHeight = -1;
        Scanner.HighLights = true;
        Scanner.Buildings = true;
        Scanner.Armies = true;
        Scanner.Naturals = true;
        Scanner.OnChangeTileBox = _ => Layout_ScannerChangeTileBox_5();
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
        sTitleIcon.CurrentImage = 0;
        sTitleIcon.AnimationEnd = 11;
        sTitleIcon.WinWidth = 19;
        sTitleIcon.WinHeight = 16;
        sTitleIcon.Animate = false;
        sTitleIcon.AnimationFrameDelay = 0;
        session.EndRead(sTitleIcon);
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
        STitleLabel.Text = "World Map";
        STitleLabel.TextColor = 0;
        session.EndRead(STitleLabel);
        Button sMinIcon = new Button(this);
        session.BeginRead(sMinIcon);
        sMinIcon.Name = "SMinIcon";
        sMinIcon.Priority = 1000;
        sMinIcon.WinLeft = 180;
        sMinIcon.WinTop = 1;
        sMinIcon.LayoutToolTipText = new LayoutStrings()
        {
            "Minimizes the window"
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
        sMinIcon.OnClick = _ => Layout_SMinIconClick_6();
        session.EndRead(sMinIcon);
        Button help = new Button(this);
        session.BeginRead(help);
        help.Name = "Help";
        help.Priority = 0;
        help.WinLeft = 165;
        help.WinTop = 1;
        help.LayoutToolTipText = new LayoutStrings()
        {
            "Displays Help for this window."
        };
        help.CaptureInput = true;
        help.Alignment.AlignWidth = AlignWidth.Right;
        help.Alignment.AlignHeight = AlignHeight.Top;
        help.Alignment.HeightPercent = 0;
        help.Alignment.WidthPercent = 0;
        help.Alignment.LeftOffsetPercent = 0;
        help.Alignment.RightOffsetPercent = 0;
        help.Alignment.TopOffsetPercent = 0;
        help.Alignment.BottomOffsetPercent = 0;
        help.Alignment.LeftOffset = 0;
        help.Alignment.RightOffset = 20;
        help.Alignment.TopOffset = 1;
        help.Alignment.BottomOffset = 0;
        help.Masked = false;
        help.BorderHeight = 0;
        help.BorderHeightSelected = 0;
        help.ImageLibraryIndexDown = 60;
        help.ImageLibraryIndexLit = 61;
        help.ImageLibraryIndexUp = 59;
        help.TextColor = 0;
        help.TextSelectedColor = 0;
        help.WinWidth = 15;
        help.OnClick = _ => Layout_HelpClick_7();
        session.EndRead(help);
        LayoutIvTranslator translator = new LayoutIvTranslator(this);
        session.BeginRead(translator);
        translator.Name = "Translator";
        translator.DictionaryName = "AoW";
        translator.TargetsData.Add(new TranslationTarget("", "Items", 0));
        translator.TargetsData.Add(new TranslationTarget("", "Text", 0));
        translator.TargetsData.Add(new TranslationTarget("", "ToolTipText", 0));
        session.EndRead(translator);
        session.EndRead(this);
        scannerFrame.Window = MainWindow;
        ScannerTab.Window = MainWindow;
        BuildingCheck.Window = MainWindow;
        ArmyCheck.Window = MainWindow;
        NaturalCheck.Window = MainWindow;
        Scanner.Window = MainWindow;
        sTitleIcon.Window = MainWindow;
        STitleLabel.Window = MainWindow;
        sMinIcon.Window = MainWindow;
        help.Window = MainWindow;
        Loaded();
        MainWindow.Loaded();
        scannerFrame.Loaded();
        ScannerTab.Loaded();
        BuildingCheck.Loaded();
        ArmyCheck.Loaded();
        NaturalCheck.Loaded();
        Scanner.Loaded();
        sTitleIcon.Loaded();
        STitleLabel.Loaded();
        sMinIcon.Loaded();
        help.Loaded();
        translator.Loaded();
        session.Resolve(this);
        MainWindow.WindowManager = DialogHosts.General.Manager;
        MainWindow.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        MainWindow.ImageLibraryTexture = DialogHosts.UIGraphics.GenericT;
        scannerFrame.ImageLibraryFrame = DialogHosts.UIGraphics.ScanWin;
        ScannerTab.ToolTip = DialogHosts.General.ToolTip;
        ScannerTab.Font = DialogHosts.UIFonts.Age8;
        ScannerTab.ImageLibraryButton = DialogHosts.UIGraphics.ScanWin;
        BuildingCheck.ToolTip = DialogHosts.General.ToolTip;
        BuildingCheck.ImageLibraryIcon = DialogHosts.UIGraphics.ScanWin;
        ArmyCheck.ToolTip = DialogHosts.General.ToolTip;
        ArmyCheck.ImageLibraryIcon = DialogHosts.UIGraphics.ScanWin;
        NaturalCheck.ToolTip = DialogHosts.General.ToolTip;
        NaturalCheck.ImageLibraryIcon = DialogHosts.UIGraphics.ScanWin;
        Scanner.TilesImageLibrary = DialogHosts.UIGraphics.ScanTiles;
        Scanner.HighlightsImageLibrary = DialogHosts.UIGraphics.ScanHighlights;
        Scanner.FogImageLibrary = DialogHosts.UIGraphics.ScanFog;
        Scanner.ImageLibraryObjects = DialogHosts.UIGraphics.ScanObj;
        Scanner.ImageLibraryFrame = DialogHosts.UIGraphics.ScanWin;
        sTitleIcon.ImageLibrary = DialogHosts.UIGraphics.TitleIcons;
        STitleLabel.Font = DialogHosts.UIFonts.Age10;
        sMinIcon.ToolTip = DialogHosts.General.ToolTip;
        sMinIcon.ImageLibraryIcon = DialogHosts.UIGraphics.GenericI;
        help.ToolTip = DialogHosts.General.ToolTip;
        help.ImageLibraryIcon = DialogHosts.UIGraphics.GenericI;
    }

    private void Layout_ScannerWindowResize_0()
    {
        ScannerWindowResize();
    }

    private void Layout_ScannerTabChange_1()
    {
        ScannerTabChange();
    }

    private void Layout_ScannerChangeTileBox_5()
    {
        ScannerChangeTileBox();
    }

    private void Layout_SMinIconClick_6()
    {
        SMinIconClick();
    }

    private void Layout_HelpClick_7()
    {
        HelpClick();
    }
}
