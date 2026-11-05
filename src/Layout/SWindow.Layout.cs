// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;
using Aow.Layout;
using Aow.Units.AoWWindow;
using Aow.Units.AoWComp;
using Aow.Units.AoWFrame;
using Aow.Units.AoWTabPanel;
using Aow.Units.AoWCheckBox;
using Aow.Units.Scanner;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWButton;

namespace Aow.Units.AoWSWindow
{
    public partial class TSWindow : ILayoutModule
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "SWindow";
            ScannerWindow = new TAOWWindow(this);
            session.BeginRead(ScannerWindow);
            ScannerWindow.Name = "ScannerWindow";
            ScannerWindow.Visible = false;
            ScannerWindow.Priority = 10;
            ScannerWindow.WinLeft = 440;
            ScannerWindow.WinTop = 0;
            ScannerWindow.WinWidth = 200;
            ScannerWindow.Masked = false;
            ScannerWindow.Alignment.AlignWidth = TAOWAlignWidth.awRight;
            ScannerWindow.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            ScannerWindow.Alignment.HeightPercent = 0;
            ScannerWindow.Alignment.WidthPercent = 0;
            ScannerWindow.Alignment.LeftOffPercent = 0;
            ScannerWindow.Alignment.RightOffPercent = 0;
            ScannerWindow.Alignment.TopOffPercent = 0;
            ScannerWindow.Alignment.BottomOffPercent = 0;
            ScannerWindow.Alignment.LeftOffset = 0;
            ScannerWindow.Alignment.RightOffset = 0;
            ScannerWindow.Alignment.TopOffset = 0;
            ScannerWindow.Alignment.BottomOffset = 0;
            ScannerWindow.UseVMem = true;
            ScannerWindow.BorderWidth = 4;
            ScannerWindow.BorderHeight = 4;
            ScannerWindow.OverlapTopCorners = false;
            ScannerWindow.Cursor = 0;
            ScannerWindow.DoubleBuffered = true;
            ScannerWindow.MaskColor = 0;
            ScannerWindow.FreeSurfaceOnHide = false;
            ScannerWindow.ILIndexFrame = 0;
            ScannerWindow.ILIndexTexture = 2;
            ScannerWindow.DragHeight = 20;
            ScannerWindow.Draggable = true;
            ScannerWindow.Resizable = true;
            ScannerWindow.OnDrag = Layout_ScannerWindowResize_0;
            ScannerWindow.OnResize = Layout_ScannerWindowResize_0;
            ScannerWindow.MinWidth = 200;
            ScannerWindow.MinHeight = 216;
            ScannerWindow.MaxWidth = 0;
            ScannerWindow.MaxHeight = 0;
            ScannerWindow.UseManagerBorders = true;
            ScannerWindow.Modal = false;
            ScannerWindow.WinHeight = 216;
            ScannerWindow.Dockable = false;
            session.EndRead(ScannerWindow);
            ScannerFrame = new TAoWFrame(this);
            session.BeginRead(ScannerFrame);
            ScannerFrame.Name = "ScannerFrame";
            ScannerFrame.Priority = 50;
            ScannerFrame.WinLeft = -2;
            ScannerFrame.WinTop = 10;
            ScannerFrame.CaptureInput = false;
            ScannerFrame.Alignment.AlignWidth = TAOWAlignWidth.awBoth;
            ScannerFrame.Alignment.AlignHeight = TAOWAlignHeight.ahBoth;
            ScannerFrame.Alignment.HeightPercent = 0;
            ScannerFrame.Alignment.WidthPercent = 0;
            ScannerFrame.Alignment.LeftOffPercent = 0;
            ScannerFrame.Alignment.RightOffPercent = 0;
            ScannerFrame.Alignment.TopOffPercent = 0;
            ScannerFrame.Alignment.BottomOffPercent = 0;
            ScannerFrame.Alignment.LeftOffset = -2;
            ScannerFrame.Alignment.RightOffset = 6;
            ScannerFrame.Alignment.TopOffset = 10;
            ScannerFrame.Alignment.BottomOffset = 4;
            ScannerFrame.Masked = false;
            ScannerFrame.WinWidth = 196;
            ScannerFrame.WinHeight = 202;
            ScannerFrame.FrameType = TFrameType.frHollowBox;
            ScannerFrame.ILIndexFrame = 0;
            ScannerFrame.ILIndexLine = 0;
            ScannerFrame.ILIndexTexture = 0;
            session.EndRead(ScannerFrame);
            ScannerTab = new TAOWTabPanel(this);
            session.BeginRead(ScannerTab);
            ScannerTab.Name = "ScannerTab";
            ScannerTab.OnMouseDown = ScannerTabMouseDown;
            ScannerTab.Priority = 10000;
            ScannerTab.WinLeft = 31;
            ScannerTab.WinTop = 182;
            ScannerTab.LayoutToolTipText = new LayoutStrings()
            {
                "Selects one of the three map layers."
            };
            ScannerTab.CaptureInput = true;
            ScannerTab.Alignment.AlignWidth = TAOWAlignWidth.awBoth;
            ScannerTab.Alignment.AlignHeight = TAOWAlignHeight.ahBottom;
            ScannerTab.Alignment.HeightPercent = 0;
            ScannerTab.Alignment.WidthPercent = 0;
            ScannerTab.Alignment.LeftOffPercent = 0;
            ScannerTab.Alignment.RightOffPercent = 0;
            ScannerTab.Alignment.TopOffPercent = 0;
            ScannerTab.Alignment.BottomOffPercent = 0;
            ScannerTab.Alignment.LeftOffset = 31;
            ScannerTab.Alignment.RightOffset = 24;
            ScannerTab.Alignment.TopOffset = 0;
            ScannerTab.Alignment.BottomOffset = 19;
            ScannerTab.Masked = false;
            ScannerTab.BorderHeight = 3;
            ScannerTab.OnChange = Layout_ScannerTabChange_1;
            ScannerTab.Selected = 1;
            ScannerTab.WinWidth = 145;
            ScannerTab.TextColor = 1777703;
            ScannerTab.TextSelColor = 0;
            ScannerTab.ILIndexDown = 17;
            ScannerTab.ILIndexUp = 14;
            ScannerTab.BorderHeightSelected = 3;
            session.EndRead(ScannerTab);
            BuildingCheck = new TAOWCheckBox(this);
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
            BuildingCheck.Alignment.AlignWidth = TAOWAlignWidth.awRight;
            BuildingCheck.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            BuildingCheck.Alignment.HeightPercent = 0;
            BuildingCheck.Alignment.WidthPercent = 0;
            BuildingCheck.Alignment.LeftOffPercent = 0;
            BuildingCheck.Alignment.RightOffPercent = 0;
            BuildingCheck.Alignment.TopOffPercent = 0;
            BuildingCheck.Alignment.BottomOffPercent = 0;
            BuildingCheck.Alignment.LeftOffset = 0;
            BuildingCheck.Alignment.RightOffset = 25;
            BuildingCheck.Alignment.TopOffset = 37;
            BuildingCheck.Alignment.BottomOffset = 0;
            BuildingCheck.Masked = false;
            BuildingCheck.BorderHeight = 0;
            BuildingCheck.BorderHeightSelected = 0;
            BuildingCheck.ILIndexDown = 9;
            BuildingCheck.ILIndexLit = 0;
            BuildingCheck.ILIndexUp = 8;
            BuildingCheck.TextColor = 0;
            BuildingCheck.TextSelColor = 0;
            BuildingCheck.WinWidth = 15;
            BuildingCheck.Checked = true;
            BuildingCheck.OnChange = Layout_BuildingCheckChange_2;
            BuildingCheck.RadioIndex = 0;
            session.EndRead(BuildingCheck);
            ArmyCheck = new TAOWCheckBox(this);
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
            ArmyCheck.Alignment.AlignWidth = TAOWAlignWidth.awRight;
            ArmyCheck.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            ArmyCheck.Alignment.HeightPercent = 0;
            ArmyCheck.Alignment.WidthPercent = 0;
            ArmyCheck.Alignment.LeftOffPercent = 0;
            ArmyCheck.Alignment.RightOffPercent = 0;
            ArmyCheck.Alignment.TopOffPercent = 0;
            ArmyCheck.Alignment.BottomOffPercent = 0;
            ArmyCheck.Alignment.LeftOffset = 0;
            ArmyCheck.Alignment.RightOffset = 25;
            ArmyCheck.Alignment.TopOffset = 52;
            ArmyCheck.Alignment.BottomOffset = 0;
            ArmyCheck.Masked = false;
            ArmyCheck.BorderHeight = 0;
            ArmyCheck.BorderHeightSelected = 0;
            ArmyCheck.ILIndexDown = 11;
            ArmyCheck.ILIndexLit = 0;
            ArmyCheck.ILIndexUp = 10;
            ArmyCheck.TextColor = 0;
            ArmyCheck.TextSelColor = 0;
            ArmyCheck.WinWidth = 15;
            ArmyCheck.Checked = true;
            ArmyCheck.OnChange = Layout_ArmyCheckChange_3;
            ArmyCheck.RadioIndex = 0;
            session.EndRead(ArmyCheck);
            NaturalCheck = new TAOWCheckBox(this);
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
            NaturalCheck.Alignment.AlignWidth = TAOWAlignWidth.awRight;
            NaturalCheck.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            NaturalCheck.Alignment.HeightPercent = 0;
            NaturalCheck.Alignment.WidthPercent = 0;
            NaturalCheck.Alignment.LeftOffPercent = 0;
            NaturalCheck.Alignment.RightOffPercent = 0;
            NaturalCheck.Alignment.TopOffPercent = 0;
            NaturalCheck.Alignment.BottomOffPercent = 0;
            NaturalCheck.Alignment.LeftOffset = 0;
            NaturalCheck.Alignment.RightOffset = 25;
            NaturalCheck.Alignment.TopOffset = 67;
            NaturalCheck.Alignment.BottomOffset = 0;
            NaturalCheck.Masked = false;
            NaturalCheck.BorderHeight = 0;
            NaturalCheck.BorderHeightSelected = 0;
            NaturalCheck.ILIndexDown = 13;
            NaturalCheck.ILIndexLit = 0;
            NaturalCheck.ILIndexUp = 12;
            NaturalCheck.TextColor = 0;
            NaturalCheck.TextSelColor = 0;
            NaturalCheck.WinWidth = 15;
            NaturalCheck.Checked = true;
            NaturalCheck.OnChange = Layout_NaturalCheckChange_4;
            NaturalCheck.RadioIndex = 0;
            session.EndRead(NaturalCheck);
            Scanner = new TScanner(this);
            session.BeginRead(Scanner);
            Scanner.Name = "Scanner";
            Scanner.Priority = 5;
            Scanner.WinLeft = 30;
            Scanner.WinTop = 32;
            Scanner.CaptureInput = true;
            Scanner.Alignment.AlignWidth = TAOWAlignWidth.awBoth;
            Scanner.Alignment.AlignHeight = TAOWAlignHeight.ahBoth;
            Scanner.Alignment.HeightPercent = 0;
            Scanner.Alignment.WidthPercent = 0;
            Scanner.Alignment.LeftOffPercent = 0;
            Scanner.Alignment.RightOffPercent = 0;
            Scanner.Alignment.TopOffPercent = 0;
            Scanner.Alignment.BottomOffPercent = 0;
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
            Scanner.OnChangeTileBox = Layout_ScannerChangeTileBox_5;
            Scanner.MapDrawSteps = 5;
            Scanner.ILIndexFrame = 20;
            session.EndRead(Scanner);
            STitleIcon = new TAOWImage(this);
            session.BeginRead(STitleIcon);
            STitleIcon.Name = "STitleIcon";
            STitleIcon.Priority = 0;
            STitleIcon.WinLeft = 2;
            STitleIcon.WinTop = 2;
            STitleIcon.CaptureInput = false;
            STitleIcon.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
            STitleIcon.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            STitleIcon.Alignment.HeightPercent = 0;
            STitleIcon.Alignment.WidthPercent = 0;
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
            STitleIcon.CurrentImage = 0;
            STitleIcon.AnimEnd = 11;
            STitleIcon.WinWidth = 19;
            STitleIcon.WinHeight = 16;
            STitleIcon.Animate = false;
            STitleIcon.AnimFrameDelay = 0;
            session.EndRead(STitleIcon);
            STitleLabel = new TAOWLabel(this);
            session.BeginRead(STitleLabel);
            STitleLabel.Name = "STitleLabel";
            STitleLabel.Priority = 0;
            STitleLabel.WinLeft = 25;
            STitleLabel.WinTop = 4;
            STitleLabel.CaptureInput = false;
            STitleLabel.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
            STitleLabel.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            STitleLabel.Alignment.HeightPercent = 0;
            STitleLabel.Alignment.WidthPercent = 0;
            STitleLabel.Alignment.LeftOffPercent = 0;
            STitleLabel.Alignment.RightOffPercent = 0;
            STitleLabel.Alignment.TopOffPercent = 0;
            STitleLabel.Alignment.BottomOffPercent = 0;
            STitleLabel.Alignment.LeftOffset = 25;
            STitleLabel.Alignment.RightOffset = 0;
            STitleLabel.Alignment.TopOffset = 4;
            STitleLabel.Alignment.BottomOffset = 0;
            STitleLabel.Masked = true;
            STitleLabel.Text = "World Map";
            STitleLabel.TextColor = 0;
            session.EndRead(STitleLabel);
            SMinIcon = new TAOWButton(this);
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
            SMinIcon.Alignment.AlignWidth = TAOWAlignWidth.awRight;
            SMinIcon.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            SMinIcon.Alignment.HeightPercent = 0;
            SMinIcon.Alignment.WidthPercent = 0;
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
            SMinIcon.OnClick = Layout_SMinIconClick_6;
            session.EndRead(SMinIcon);
            Help = new TAOWButton(this);
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
            Help.Alignment.AlignWidth = TAOWAlignWidth.awRight;
            Help.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            Help.Alignment.HeightPercent = 0;
            Help.Alignment.WidthPercent = 0;
            Help.Alignment.LeftOffPercent = 0;
            Help.Alignment.RightOffPercent = 0;
            Help.Alignment.TopOffPercent = 0;
            Help.Alignment.BottomOffPercent = 0;
            Help.Alignment.LeftOffset = 0;
            Help.Alignment.RightOffset = 20;
            Help.Alignment.TopOffset = 1;
            Help.Alignment.BottomOffset = 0;
            Help.Masked = false;
            Help.BorderHeight = 0;
            Help.BorderHeightSelected = 0;
            Help.ILIndexDown = 60;
            Help.ILIndexLit = 61;
            Help.ILIndexUp = 59;
            Help.TextColor = 0;
            Help.TextSelColor = 0;
            Help.WinWidth = 15;
            Help.OnClick = Layout_HelpClick_7;
            session.EndRead(Help);
            var created__Translator = new LayoutTIvTranslator(this);
            Translator = created__Translator;
            session.BeginRead(created__Translator);
            created__Translator.Name = "Translator";
            created__Translator.DictionaryName = "AoW";
            created__Translator.TargetsData.Add(new TranslationTarget("", "Items", 0));
            created__Translator.TargetsData.Add(new TranslationTarget("", "Text", 0));
            created__Translator.TargetsData.Add(new TranslationTarget("", "ToolTipText", 0));
            session.EndRead(created__Translator);
            session.EndRead(this);
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
            ScannerWindow.WinManager = AoWWinMan.AoWWinMan.General.Manager;
            ScannerWindow.ILFrame = aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ScannerWindow.ILTexture = aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ScannerFrame.ILFrame = aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanWin;
            ScannerTab.ToolTip = AoWWinMan.AoWWinMan.General.ToolTip;
            ScannerTab.AOWFont = AoWExeForms.AoWExeForms.FontModule.Age8;
            ScannerTab.ILButton = aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanWin;
            BuildingCheck.ToolTip = AoWWinMan.AoWWinMan.General.ToolTip;
            BuildingCheck.ILIcon = aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanWin;
            ArmyCheck.ToolTip = AoWWinMan.AoWWinMan.General.ToolTip;
            ArmyCheck.ILIcon = aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanWin;
            NaturalCheck.ToolTip = AoWWinMan.AoWWinMan.General.ToolTip;
            NaturalCheck.ILIcon = aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanWin;
            Scanner.TilesImageLib = aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanTiles;
            Scanner.HiLightsImageLib = aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanHiLites;
            Scanner.FogImageLib = aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanFog;
            Scanner.ILObjects = aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanObj;
            Scanner.ILFrame = aowIntGfxMod.aowIntGfxMod.IntGfxMod.ScanWin;
            STitleIcon.ImageLib = aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            STitleLabel.AOWFont = AoWExeForms.AoWExeForms.FontModule.Age10;
            SMinIcon.ToolTip = AoWWinMan.AoWWinMan.General.ToolTip;
            SMinIcon.ILIcon = aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Help.ToolTip = AoWWinMan.AoWWinMan.General.ToolTip;
            Help.ILIcon = aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
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
            BuildingCheckChange((TAOWCheckBox)p0);
        }

        private void Layout_ArmyCheckChange_3(TObject p0)
        {
            ArmyCheckChange((TAOWCheckBox)p0);
        }

        private void Layout_NaturalCheckChange_4(TObject p0)
        {
            NaturalCheckChange((TAOWCheckBox)p0);
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
