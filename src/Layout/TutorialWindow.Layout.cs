// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Layout;
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWComp;
using Aow.Units.AoWExeForms;
using Aow.Units.aowIntGfxMod;
using Aow.Units.AoWMemo;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using Aow.Units.AoWWinMan;

namespace Aow.Units.AoWTutorialWin
{
    public partial class TTutorialWindow : ILayoutModule
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "TutorialWindow";
            Dlg = new TAOWWindow(this);
            session.BeginRead(Dlg);
            Dlg.Name = "Dlg";
            Dlg.Visible = false;
            Dlg.Priority = 9999;
            Dlg.WinLeft = 125;
            Dlg.WinTop = 85;
            Dlg.WinWidth = 390;
            Dlg.Masked = true;
            Dlg.OnHide = Layout_DlgHide_0;
            Dlg.Alignment.AlignWidth = TAOWAlignWidth.awCenter;
            Dlg.Alignment.AlignHeight = TAOWAlignHeight.ahCenter;
            Dlg.Alignment.HeightPercent = 0;
            Dlg.Alignment.WidthPercent = 0;
            Dlg.Alignment.LeftOffPercent = 0;
            Dlg.Alignment.RightOffPercent = 0;
            Dlg.Alignment.TopOffPercent = 0;
            Dlg.Alignment.BottomOffPercent = 0;
            Dlg.Alignment.LeftOffset = 0;
            Dlg.Alignment.RightOffset = 0;
            Dlg.Alignment.TopOffset = 0;
            Dlg.Alignment.BottomOffset = 0;
            Dlg.UseVMem = false;
            Dlg.BorderWidth = 0;
            Dlg.BorderHeight = 0;
            Dlg.OverlapTopCorners = false;
            Dlg.Cursor = 0;
            Dlg.DoubleBuffered = true;
            Dlg.MaskColor = 5808200;
            Dlg.FreeSurfaceOnHide = true;
            Dlg.ILIndexFrame = 80;
            Dlg.ILIndexTexture = 34;
            Dlg.DragHeight = 20;
            Dlg.Draggable = true;
            Dlg.Resizable = false;
            Dlg.MinWidth = 0;
            Dlg.MinHeight = 0;
            Dlg.MaxWidth = 0;
            Dlg.MaxHeight = 0;
            Dlg.UseManagerBorders = true;
            Dlg.Modal = true;
            Dlg.WinHeight = 262;
            Dlg.Dockable = false;
            session.EndRead(Dlg);
            MessageMemo = new TAOWMemo(this);
            session.BeginRead(MessageMemo);
            MessageMemo.Name = "MessageMemo";
            MessageMemo.Priority = 200;
            MessageMemo.WinLeft = 52;
            MessageMemo.WinTop = 80;
            MessageMemo.CaptureInput = false;
            MessageMemo.Alignment.AlignWidth = TAOWAlignWidth.awBoth;
            MessageMemo.Alignment.AlignHeight = TAOWAlignHeight.ahBoth;
            MessageMemo.Alignment.HeightPercent = 0;
            MessageMemo.Alignment.WidthPercent = 0;
            MessageMemo.Alignment.LeftOffPercent = 0;
            MessageMemo.Alignment.RightOffPercent = 0;
            MessageMemo.Alignment.TopOffPercent = 0;
            MessageMemo.Alignment.BottomOffPercent = 0;
            MessageMemo.Alignment.LeftOffset = 52;
            MessageMemo.Alignment.RightOffset = 52;
            MessageMemo.Alignment.TopOffset = 80;
            MessageMemo.Alignment.BottomOffset = 60;
            MessageMemo.Masked = true;
            MessageMemo.WinWidth = 286;
            MessageMemo.WinHeight = 122;
            MessageMemo.BorderWidth = 15;
            MessageMemo.BorderHeight = 5;
            MessageMemo.TextColor = 1052711;
            MessageMemo.CursorChar = 124;
            MessageMemo.BlinkDelay = 0;
            MessageMemo.Editable = false;
            MessageMemo.ILIndexFrame = 40;
            MessageMemo.ILIndexTexture = 0;
            MessageMemo.ItemHeight = 17;
            MessageMemo.ColorChangeChar = 0;
            MessageMemo.TextAlign = TaowTextAlign.taCenter;
            session.EndRead(MessageMemo);
            Close = new TAOWButton(this);
            session.BeginRead(Close);
            Close.Name = "Close";
            Close.Priority = 0;
            Close.WinLeft = 125;
            Close.WinTop = 211;
            Close.LayoutToolTipText = new LayoutStrings()
            {
                "Closes the Tutorial Window. You can reopen it by pressing F1 on the keyboard."
            };
            Close.CaptureInput = true;
            Close.Alignment.AlignWidth = TAOWAlignWidth.awCenter;
            Close.Alignment.AlignHeight = TAOWAlignHeight.ahBottom;
            Close.Alignment.HeightPercent = 0;
            Close.Alignment.WidthPercent = 0;
            Close.Alignment.LeftOffPercent = 0;
            Close.Alignment.RightOffPercent = 0;
            Close.Alignment.TopOffPercent = 0;
            Close.Alignment.BottomOffPercent = 0;
            Close.Alignment.LeftOffset = 0;
            Close.Alignment.RightOffset = 0;
            Close.Alignment.TopOffset = 1;
            Close.Alignment.BottomOffset = 25;
            Close.Masked = true;
            Close.BorderHeight = 8;
            Close.BorderHeightSelected = 8;
            Close.ILIndexDown = 6;
            Close.ILIndexLit = 3;
            Close.ILIndexUp = 0;
            Close.Text = "OK";
            Close.TextColor = 855324;
            Close.TextSelColor = 16777215;
            Close.WinWidth = 140;
            Close.OnClick = Layout_CloseClick_1;
            session.EndRead(Close);
            HeaderMemo = new TAOWMemo(this);
            session.BeginRead(HeaderMemo);
            HeaderMemo.Name = "HeaderMemo";
            HeaderMemo.Priority = 200;
            HeaderMemo.WinLeft = 70;
            HeaderMemo.WinTop = 35;
            HeaderMemo.CaptureInput = false;
            HeaderMemo.Alignment.AlignWidth = TAOWAlignWidth.awBoth;
            HeaderMemo.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            HeaderMemo.Alignment.HeightPercent = 0;
            HeaderMemo.Alignment.WidthPercent = 0;
            HeaderMemo.Alignment.LeftOffPercent = 0;
            HeaderMemo.Alignment.RightOffPercent = 0;
            HeaderMemo.Alignment.TopOffPercent = 0;
            HeaderMemo.Alignment.BottomOffPercent = 0;
            HeaderMemo.Alignment.LeftOffset = 70;
            HeaderMemo.Alignment.RightOffset = 70;
            HeaderMemo.Alignment.TopOffset = 35;
            HeaderMemo.Alignment.BottomOffset = 80;
            HeaderMemo.Masked = true;
            HeaderMemo.WinWidth = 250;
            HeaderMemo.WinHeight = 44;
            HeaderMemo.BorderWidth = 10;
            HeaderMemo.BorderHeight = 5;
            HeaderMemo.TextColor = 0;
            HeaderMemo.CursorChar = 124;
            HeaderMemo.BlinkDelay = 0;
            HeaderMemo.Editable = false;
            HeaderMemo.ILIndexFrame = 40;
            HeaderMemo.ILIndexTexture = 0;
            HeaderMemo.ItemHeight = 20;
            HeaderMemo.ColorChangeChar = 0;
            HeaderMemo.TextAlign = TaowTextAlign.taCenter;
            session.EndRead(HeaderMemo);
            AbVSB = new TAOWVScrollBar(this);
            session.BeginRead(AbVSB);
            AbVSB.Name = "AbVSB";
            AbVSB.Visible = false;
            AbVSB.Priority = 200;
            AbVSB.WinLeft = 325;
            AbVSB.WinTop = 80;
            AbVSB.CaptureInput = true;
            AbVSB.Alignment.AlignWidth = TAOWAlignWidth.awNone;
            AbVSB.Alignment.AlignHeight = TAOWAlignHeight.ahNone;
            AbVSB.Alignment.HeightPercent = 0;
            AbVSB.Alignment.WidthPercent = 0;
            AbVSB.Alignment.LeftOffPercent = 0;
            AbVSB.Alignment.RightOffPercent = 0;
            AbVSB.Alignment.TopOffPercent = 0;
            AbVSB.Alignment.BottomOffPercent = 0;
            AbVSB.Alignment.LeftOffset = 0;
            AbVSB.Alignment.RightOffset = 0;
            AbVSB.Alignment.TopOffset = 0;
            AbVSB.Alignment.BottomOffset = 0;
            AbVSB.Masked = true;
            AbVSB.Min = 0;
            AbVSB.Max = 0;
            AbVSB.SmallChange = 1;
            AbVSB.LargeChange = 6;
            AbVSB.Position = 0;
            AbVSB.WinHeight = 122;
            AbVSB.WinWidth = 13;
            AbVSB.NumberVisible = 1;
            AbVSB.VanishWhenFull = true;
            AbVSB.ILIndexIncUp = 42;
            AbVSB.ILIndexIncDown = 43;
            AbVSB.ILIndexDecUp = 40;
            AbVSB.ILIndexDecDown = 41;
            AbVSB.ILIndexBack = 47;
            AbVSB.ILIndexTab = 44;
            session.EndRead(AbVSB);
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
            MessageMemo.AOWWindow = Dlg;
            MessageMemo.VScrollBar = AbVSB;
            Close.AOWWindow = Dlg;
            HeaderMemo.AOWWindow = Dlg;
            AbVSB.AOWWindow = Dlg;
            Loaded();
            Dlg.Loaded();
            MessageMemo.Loaded();
            Close.Loaded();
            HeaderMemo.Loaded();
            AbVSB.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            Dlg.WinManager = AoWWinManUnit.General.Manager;
            Dlg.ILTexture = aowIntGfxModUnit.IntGfxMod.MiscArt;
            MessageMemo.AOWFont = AoWExeFormsUnit.FontModule.Age10;
            Close.ToolTip = AoWWinManUnit.General.ToolTip;
            Close.AOWFont = AoWExeFormsUnit.FontModule.Age10b;
            Close.ILButton = aowIntGfxModUnit.IntGfxMod.CampaignTrans;
            HeaderMemo.AOWFont = AoWExeFormsUnit.FontModule.AoW15BlackBrown;
            HeaderMemo.ILFrame = aowIntGfxModUnit.IntGfxMod.GenericF;
            AbVSB.ILIInc = aowIntGfxModUnit.IntGfxMod.GenericS;
            AbVSB.ILIDec = aowIntGfxModUnit.IntGfxMod.GenericS;
            AbVSB.ILTBack = aowIntGfxModUnit.IntGfxMod.GenericS;
            AbVSB.ILLTab = aowIntGfxModUnit.IntGfxMod.GenericS;
        }

        private void Layout_DlgHide_0(TObject p0)
        {
            DlgHide();
        }

        private void Layout_CloseClick_1(TObject p0)
        {
            CloseClick();
        }
    }
}
