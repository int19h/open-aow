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
        ScannerFrame = new Frame(this);
        session.BeginRead(ScannerFrame);
        ScannerFrame.Name = "ScannerFrame";
        ScannerFrame.Priority = 50;
        ScannerFrame.WinLeft = -2;
        ScannerFrame.WinTop = 10;
        ScannerFrame.CaptureInput = false;
        ScannerFrame.Alignment.AlignWidth = AlignWidth.Both;
        ScannerFrame.Alignment.AlignHeight = AlignHeight.Both;
        ScannerFrame.Alignment.HeightPercent = 0;
        ScannerFrame.Alignment.WidthPercent = 0;
        ScannerFrame.Alignment.LeftOffsetPercent = 0;
        ScannerFrame.Alignment.RightOffsetPercent = 0;
        ScannerFrame.Alignment.TopOffsetPercent = 0;
        ScannerFrame.Alignment.BottomOffsetPercent = 0;
        ScannerFrame.Alignment.LeftOffset = -2;
        ScannerFrame.Alignment.RightOffset = 6;
        ScannerFrame.Alignment.TopOffset = 10;
        ScannerFrame.Alignment.BottomOffset = 4;
        ScannerFrame.Masked = false;
        ScannerFrame.WinWidth = 196;
        ScannerFrame.WinHeight = 202;
        ScannerFrame.FrameType = FrameType.HollowBox;
        ScannerFrame.ImageLibraryIndexFrame = 0;
        ScannerFrame.ImageLibraryIndexLine = 0;
        ScannerFrame.ImageLibraryIndexTexture = 0;
        session.EndRead(ScannerFrame);
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
        STitleIcon.CurrentImage = 0;
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
        STitleLabel.Text = "World Map";
        STitleLabel.TextColor = 0;
        session.EndRead(STitleLabel);
        SMinIcon = new Button(this);
        session.BeginRead(SMinIcon);
        SMinIcon.Name = "SMinIcon";
        SMinIcon.Priority = 1000;
        SMinIcon.WinLeft = 180;
        SMinIcon.WinTop = 1;
        SMinIcon.LayoutToolTipText = new LayoutStrings()
        {
            "Minimizes the window"
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
        SMinIcon.OnClick = _ => Layout_SMinIconClick_6();
        session.EndRead(SMinIcon);
        Help = new Button(this);
        session.BeginRead(Help);
        Help.Name = "Help";
        Help.Priority = 0;
        Help.WinLeft = 165;
        Help.WinTop = 1;
        Help.LayoutToolTipText = new LayoutStrings()
        {
            "Displays Help for this window."
        };
        Help.CaptureInput = true;
        Help.Alignment.AlignWidth = AlignWidth.Right;
        Help.Alignment.AlignHeight = AlignHeight.Top;
        Help.Alignment.HeightPercent = 0;
        Help.Alignment.WidthPercent = 0;
        Help.Alignment.LeftOffsetPercent = 0;
        Help.Alignment.RightOffsetPercent = 0;
        Help.Alignment.TopOffsetPercent = 0;
        Help.Alignment.BottomOffsetPercent = 0;
        Help.Alignment.LeftOffset = 0;
        Help.Alignment.RightOffset = 20;
        Help.Alignment.TopOffset = 1;
        Help.Alignment.BottomOffset = 0;
        Help.Masked = false;
        Help.BorderHeight = 0;
        Help.BorderHeightSelected = 0;
        Help.ImageLibraryIndexDown = 60;
        Help.ImageLibraryIndexLit = 61;
        Help.ImageLibraryIndexUp = 59;
        Help.TextColor = 0;
        Help.TextSelectedColor = 0;
        Help.WinWidth = 15;
        Help.OnClick = _ => Layout_HelpClick_7();
        session.EndRead(Help);
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
        ScannerFrame.Window = MainWindow;
        ScannerTab.Window = MainWindow;
        BuildingCheck.Window = MainWindow;
        ArmyCheck.Window = MainWindow;
        NaturalCheck.Window = MainWindow;
        Scanner.Window = MainWindow;
        STitleIcon.Window = MainWindow;
        STitleLabel.Window = MainWindow;
        SMinIcon.Window = MainWindow;
        Help.Window = MainWindow;
        Loaded();
        MainWindow.Loaded();
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
        MainWindow.WindowManager = DialogHosts.General.Manager;
        MainWindow.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        MainWindow.ImageLibraryTexture = DialogHosts.UIGraphics.GenericT;
        ScannerFrame.ImageLibraryFrame = DialogHosts.UIGraphics.ScanWin;
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
        STitleIcon.ImageLibrary = DialogHosts.UIGraphics.TitleIcons;
        STitleLabel.Font = DialogHosts.UIFonts.Age10;
        SMinIcon.ToolTip = DialogHosts.General.ToolTip;
        SMinIcon.ImageLibraryIcon = DialogHosts.UIGraphics.GenericI;
        Help.ToolTip = DialogHosts.General.ToolTip;
        Help.ImageLibraryIcon = DialogHosts.UIGraphics.GenericI;
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
