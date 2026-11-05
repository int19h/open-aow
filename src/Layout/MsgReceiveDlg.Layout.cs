// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;
using Aow.Layout;
using Aow.Units.AoWWindow;
using Aow.Units.AoWComp;
using Aow.Units.AoWMemo;
using Aow.Units.AoWButton;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWImage;

namespace Aow.Units.AoWMsgReceiveDlg
{
    public partial class TMsgReceiveDlg : ILayoutModule
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "MsgReceiveDlg";
            MsgReceiveWin = new TAOWWindow(this);
            session.BeginRead(MsgReceiveWin);
            MsgReceiveWin.Name = "MsgReceiveWin";
            MsgReceiveWin.Visible = false;
            MsgReceiveWin.Priority = 300;
            MsgReceiveWin.WinLeft = 128;
            MsgReceiveWin.WinTop = 85;
            MsgReceiveWin.WinWidth = 384;
            MsgReceiveWin.Masked = true;
            MsgReceiveWin.OnHide = Layout_MsgReceiveWinHide_0;
            MsgReceiveWin.Alignment.AlignWidth = TAOWAlignWidth.awCenter;
            MsgReceiveWin.Alignment.AlignHeight = TAOWAlignHeight.ahCenter;
            MsgReceiveWin.Alignment.HeightPercent = 0;
            MsgReceiveWin.Alignment.WidthPercent = 0;
            MsgReceiveWin.Alignment.LeftOffPercent = 0;
            MsgReceiveWin.Alignment.RightOffPercent = 0;
            MsgReceiveWin.Alignment.TopOffPercent = 0;
            MsgReceiveWin.Alignment.BottomOffPercent = 0;
            MsgReceiveWin.Alignment.LeftOffset = 0;
            MsgReceiveWin.Alignment.RightOffset = 0;
            MsgReceiveWin.Alignment.TopOffset = 0;
            MsgReceiveWin.Alignment.BottomOffset = 0;
            MsgReceiveWin.UseVMem = false;
            MsgReceiveWin.BorderWidth = 0;
            MsgReceiveWin.BorderHeight = 0;
            MsgReceiveWin.OverlapTopCorners = false;
            MsgReceiveWin.Cursor = 0;
            MsgReceiveWin.DoubleBuffered = true;
            MsgReceiveWin.MaskColor = 5808200;
            MsgReceiveWin.FreeSurfaceOnHide = false;
            MsgReceiveWin.ILIndexFrame = 80;
            MsgReceiveWin.ILIndexTexture = 7;
            MsgReceiveWin.DragHeight = 0;
            MsgReceiveWin.Draggable = false;
            MsgReceiveWin.Resizable = false;
            MsgReceiveWin.MinWidth = 0;
            MsgReceiveWin.MinHeight = 0;
            MsgReceiveWin.MaxWidth = 0;
            MsgReceiveWin.MaxHeight = 0;
            MsgReceiveWin.UseManagerBorders = true;
            MsgReceiveWin.Modal = true;
            MsgReceiveWin.WinHeight = 262;
            MsgReceiveWin.Dockable = false;
            session.EndRead(MsgReceiveWin);
            MessageMemo = new TAOWMemo(this);
            session.BeginRead(MessageMemo);
            MessageMemo.Name = "MessageMemo";
            MessageMemo.Priority = 200;
            MessageMemo.WinLeft = 47;
            MessageMemo.WinTop = 100;
            MessageMemo.CaptureInput = false;
            MessageMemo.Alignment.AlignWidth = TAOWAlignWidth.awBoth;
            MessageMemo.Alignment.AlignHeight = TAOWAlignHeight.ahBoth;
            MessageMemo.Alignment.HeightPercent = 0;
            MessageMemo.Alignment.WidthPercent = 0;
            MessageMemo.Alignment.LeftOffPercent = 0;
            MessageMemo.Alignment.RightOffPercent = 0;
            MessageMemo.Alignment.TopOffPercent = 0;
            MessageMemo.Alignment.BottomOffPercent = 0;
            MessageMemo.Alignment.LeftOffset = 47;
            MessageMemo.Alignment.RightOffset = 47;
            MessageMemo.Alignment.TopOffset = 100;
            MessageMemo.Alignment.BottomOffset = 80;
            MessageMemo.Masked = true;
            MessageMemo.WinWidth = 290;
            MessageMemo.WinHeight = 82;
            MessageMemo.LayoutItems = new LayoutStrings()
            {
                "##############",
                "##############"
            };
            MessageMemo.BorderWidth = 10;
            MessageMemo.BorderHeight = 5;
            MessageMemo.TextColor = 0;
            MessageMemo.CursorChar = 124;
            MessageMemo.BlinkDelay = 0;
            MessageMemo.Editable = false;
            MessageMemo.ILIndexFrame = 40;
            MessageMemo.ILIndexTexture = 0;
            MessageMemo.ItemHeight = 20;
            MessageMemo.ColorChangeChar = 0;
            MessageMemo.TextAlign = TaowTextAlign.taCenter;
            session.EndRead(MessageMemo);
            Close = new TAOWButton(this);
            session.BeginRead(Close);
            Close.Name = "Close";
            Close.Priority = 0;
            Close.WinLeft = 122;
            Close.WinTop = 211;
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
            Close.Text = "Close";
            Close.TextColor = 855324;
            Close.TextSelColor = 16777215;
            Close.WinWidth = 140;
            Close.OnClick = Layout_CloseClick_1;
            session.EndRead(Close);
            MessageSB = new TAOWVScrollBar(this);
            session.BeginRead(MessageSB);
            MessageSB.Name = "MessageSB";
            MessageSB.Visible = false;
            MessageSB.Priority = 200;
            MessageSB.WinLeft = 301;
            MessageSB.WinTop = 45;
            MessageSB.CaptureInput = true;
            MessageSB.Alignment.AlignWidth = TAOWAlignWidth.awNone;
            MessageSB.Alignment.AlignHeight = TAOWAlignHeight.ahNone;
            MessageSB.Alignment.HeightPercent = 0;
            MessageSB.Alignment.WidthPercent = 0;
            MessageSB.Alignment.LeftOffPercent = 0;
            MessageSB.Alignment.RightOffPercent = 0;
            MessageSB.Alignment.TopOffPercent = 0;
            MessageSB.Alignment.BottomOffPercent = 0;
            MessageSB.Alignment.LeftOffset = 0;
            MessageSB.Alignment.RightOffset = 0;
            MessageSB.Alignment.TopOffset = 0;
            MessageSB.Alignment.BottomOffset = 0;
            MessageSB.Masked = true;
            MessageSB.Min = 0;
            MessageSB.Max = 0;
            MessageSB.SmallChange = 1;
            MessageSB.LargeChange = 5;
            MessageSB.Position = 0;
            MessageSB.WinHeight = 137;
            MessageSB.WinWidth = 13;
            MessageSB.NumberVisible = 1;
            MessageSB.VanishWhenFull = true;
            MessageSB.ILIndexIncUp = 42;
            MessageSB.ILIndexIncDown = 43;
            MessageSB.ILIndexDecUp = 40;
            MessageSB.ILIndexDecDown = 41;
            MessageSB.ILIndexBack = 47;
            MessageSB.ILIndexTab = 44;
            session.EndRead(MessageSB);
            LShield = new TAOWImage(this);
            session.BeginRead(LShield);
            LShield.Name = "LShield";
            LShield.Priority = 1000;
            LShield.WinLeft = 45;
            LShield.WinTop = 40;
            LShield.LayoutToolTipText = new LayoutStrings()
            {
                "Player's Shield"
            };
            LShield.CaptureInput = false;
            LShield.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
            LShield.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            LShield.Alignment.HeightPercent = 0;
            LShield.Alignment.WidthPercent = 0;
            LShield.Alignment.LeftOffPercent = 0;
            LShield.Alignment.RightOffPercent = 0;
            LShield.Alignment.TopOffPercent = 0;
            LShield.Alignment.BottomOffPercent = 0;
            LShield.Alignment.LeftOffset = 45;
            LShield.Alignment.RightOffset = 0;
            LShield.Alignment.TopOffset = 40;
            LShield.Alignment.BottomOffset = 0;
            LShield.Masked = true;
            LShield.AnimStart = 0;
            LShield.CurrentImage = 22;
            LShield.AnimEnd = 38;
            LShield.WinWidth = 40;
            LShield.WinHeight = 53;
            LShield.Animate = false;
            LShield.AnimFrameDelay = 0;
            session.EndRead(LShield);
            RShield = new TAOWImage(this);
            session.BeginRead(RShield);
            RShield.Name = "RShield";
            RShield.Priority = 1000;
            RShield.WinLeft = 299;
            RShield.WinTop = 45;
            RShield.LayoutToolTipText = new LayoutStrings()
            {
                "Player's Shield"
            };
            RShield.CaptureInput = false;
            RShield.Alignment.AlignWidth = TAOWAlignWidth.awRight;
            RShield.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            RShield.Alignment.HeightPercent = 0;
            RShield.Alignment.WidthPercent = 0;
            RShield.Alignment.LeftOffPercent = 0;
            RShield.Alignment.RightOffPercent = 0;
            RShield.Alignment.TopOffPercent = 0;
            RShield.Alignment.BottomOffPercent = 0;
            RShield.Alignment.LeftOffset = 80;
            RShield.Alignment.RightOffset = 45;
            RShield.Alignment.TopOffset = 45;
            RShield.Alignment.BottomOffset = 0;
            RShield.Masked = true;
            RShield.AnimStart = 0;
            RShield.CurrentImage = 16;
            RShield.AnimEnd = 38;
            RShield.WinWidth = 40;
            RShield.WinHeight = 53;
            RShield.Animate = false;
            RShield.AnimFrameDelay = 0;
            session.EndRead(RShield);
            HeaderMemo = new TAOWMemo(this);
            session.BeginRead(HeaderMemo);
            HeaderMemo.Name = "HeaderMemo";
            HeaderMemo.Priority = 200;
            HeaderMemo.WinLeft = 70;
            HeaderMemo.WinTop = 45;
            HeaderMemo.CaptureInput = false;
            HeaderMemo.Alignment.AlignWidth = TAOWAlignWidth.awBoth;
            HeaderMemo.Alignment.AlignHeight = TAOWAlignHeight.ahBoth;
            HeaderMemo.Alignment.HeightPercent = 0;
            HeaderMemo.Alignment.WidthPercent = 0;
            HeaderMemo.Alignment.LeftOffPercent = 0;
            HeaderMemo.Alignment.RightOffPercent = 0;
            HeaderMemo.Alignment.TopOffPercent = 0;
            HeaderMemo.Alignment.BottomOffPercent = 0;
            HeaderMemo.Alignment.LeftOffset = 70;
            HeaderMemo.Alignment.RightOffset = 70;
            HeaderMemo.Alignment.TopOffset = 45;
            HeaderMemo.Alignment.BottomOffset = 80;
            HeaderMemo.Masked = true;
            HeaderMemo.WinWidth = 244;
            HeaderMemo.WinHeight = 137;
            HeaderMemo.LayoutItems = new LayoutStrings()
            {
                "##############"
            };
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
            MessageMemo.AOWWindow = MsgReceiveWin;
            MessageMemo.VScrollBar = MessageSB;
            Close.AOWWindow = MsgReceiveWin;
            MessageSB.AOWWindow = MsgReceiveWin;
            LShield.AOWWindow = MsgReceiveWin;
            RShield.AOWWindow = MsgReceiveWin;
            HeaderMemo.AOWWindow = MsgReceiveWin;
            HeaderMemo.VScrollBar = MessageSB;
            Loaded();
            MsgReceiveWin.Loaded();
            MessageMemo.Loaded();
            Close.Loaded();
            MessageSB.Loaded();
            LShield.Loaded();
            RShield.Loaded();
            HeaderMemo.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            MsgReceiveWin.WinManager = AoWWinMan.AoWWinMan.General.Manager;
            MsgReceiveWin.ILFrame = aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            MsgReceiveWin.ILTexture = aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            MessageMemo.AOWFont = AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            MessageMemo.ILFrame = aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Close.AOWFont = AoWExeForms.AoWExeForms.FontModule.Age10b;
            Close.ILButton = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            MessageSB.ILIInc = aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            MessageSB.ILIDec = aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            MessageSB.ILTBack = aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            MessageSB.ILLTab = aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            LShield.ToolTip = AoWWinMan.AoWWinMan.General.ToolTip;
            LShield.ImageLib = aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields;
            RShield.ToolTip = AoWWinMan.AoWWinMan.General.ToolTip;
            RShield.ImageLib = aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields;
            HeaderMemo.AOWFont = AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            HeaderMemo.ILFrame = aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
        }

        private void Layout_MsgReceiveWinHide_0(TObject p0)
        {
            MsgReceiveWinHide();
        }

        private void Layout_CloseClick_1(TObject p0)
        {
            CloseClick();
        }
    }
}
