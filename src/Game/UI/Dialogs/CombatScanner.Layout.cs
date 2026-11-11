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
        ScannerFr.FrameType = FrameType.HollowBox;
        ScannerFr.ILIndexFrame = 40;
        ScannerFr.ILIndexLine = 0;
        ScannerFr.ILIndexTexture = 0;
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
        Scanner.OnChangeTileBox = _ => Layout_ScannerChangeTileBox_1();
        Scanner.MapDrawSteps = 5;
        Scanner.ILIndexFrame = 20;
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
        SMinIcon.OnClick = _ => Layout_SMinIconClick_2();
        session.EndRead(SMinIcon);
        LayoutIvTranslator created__Translator = new LayoutIvTranslator(this);
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
        TCScannerWin.WinManager = DialogHosts.General.Manager;
        TCScannerWin.ILFrame = DialogHosts.IntGfxMod.GenericF;
        TCScannerWin.ILTexture = DialogHosts.IntGfxMod.GenericT;
        ScannerFr.ILFrame = DialogHosts.IntGfxMod.GenericF;
        Scanner.TilesImageLib = DialogHosts.IntGfxMod.ScanTiles;
        Scanner.HiLightsImageLib = DialogHosts.IntGfxMod.ScanHiLites;
        Scanner.FogImageLib = DialogHosts.IntGfxMod.ScanFog;
        Scanner.ILObjects = DialogHosts.IntGfxMod.ScanObj;
        Scanner.ILFrame = DialogHosts.IntGfxMod.ScanWin;
        STitleIcon.ImageLib = DialogHosts.IntGfxMod.TitleIcons;
        STitleLabel.AOWFont = DialogHosts.FontModule.Age10;
        SMinIcon.ToolTip = DialogHosts.General.ToolTip;
        SMinIcon.ILIcon = DialogHosts.IntGfxMod.GenericI;
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
