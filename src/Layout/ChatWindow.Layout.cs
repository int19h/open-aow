// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;
using Aow.Layout;
using Aow.Units.AoWWindow;
using Aow.Units.AoWComp;
using Aow.Units.AoWMemo;
using Aow.Units.AoWEdit;
using Aow.Units.AoWListBox;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWButton;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.GFXE;
using Aow.Units.AoWWinMan;
using Aow.Units.aowIntGfxMod;
using Aow.Units.AoWExeForms;

namespace Aow.Units.AOWChatWin
{
    public partial class TChatWindow : ILayoutModule
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "ChatWindow";
            ChatWin = new TAOWWindow(this);
            session.BeginRead(ChatWin);
            ChatWin.Name = "ChatWin";
            ChatWin.Visible = false;
            ChatWin.Priority = 10;
            ChatWin.WinLeft = 0;
            ChatWin.WinTop = 272;
            ChatWin.WinWidth = 440;
            ChatWin.Masked = false;
            ChatWin.OnHide = Layout_ChatWinHide_0;
            ChatWin.OnShow = Layout_ChatWinShow_1;
            ChatWin.Alignment.AlignWidth = TAOWAlignWidth.awBoth;
            ChatWin.Alignment.AlignHeight = TAOWAlignHeight.ahBottom;
            ChatWin.Alignment.HeightPercent = 0;
            ChatWin.Alignment.WidthPercent = 0;
            ChatWin.Alignment.LeftOffPercent = 0;
            ChatWin.Alignment.RightOffPercent = 0;
            ChatWin.Alignment.TopOffPercent = 0;
            ChatWin.Alignment.BottomOffPercent = 0;
            ChatWin.Alignment.LeftOffset = 0;
            ChatWin.Alignment.RightOffset = 200;
            ChatWin.Alignment.TopOffset = 0;
            ChatWin.Alignment.BottomOffset = 0;
            ChatWin.UseVMem = false;
            ChatWin.BorderWidth = 4;
            ChatWin.BorderHeight = 4;
            ChatWin.OverlapTopCorners = false;
            ChatWin.Cursor = 0;
            ChatWin.DoubleBuffered = true;
            ChatWin.MaskColor = 0;
            ChatWin.FreeSurfaceOnHide = true;
            ChatWin.ILIndexFrame = 0;
            ChatWin.ILIndexTexture = 2;
            ChatWin.DragHeight = 20;
            ChatWin.Draggable = true;
            ChatWin.Resizable = true;
            ChatWin.OnDrag = Layout_ChatWinResize_2;
            ChatWin.OnResize = Layout_ChatWinResize_2;
            ChatWin.MinWidth = 440;
            ChatWin.MinHeight = 160;
            ChatWin.MaxWidth = 0;
            ChatWin.MaxHeight = 0;
            ChatWin.UseManagerBorders = true;
            ChatWin.Modal = false;
            ChatWin.WinHeight = 160;
            ChatWin.Dockable = false;
            session.EndRead(ChatWin);
            ChatMemo = new TAOWMemo(this);
            session.BeginRead(ChatMemo);
            ChatMemo.Name = "ChatMemo";
            ChatMemo.OnUpdate = Layout_ChatMemoUpdate_3;
            ChatMemo.Priority = 0;
            ChatMemo.WinLeft = 1;
            ChatMemo.WinTop = 0;
            ChatMemo.CaptureInput = true;
            ChatMemo.Alignment.AlignWidth = TAOWAlignWidth.awBoth;
            ChatMemo.Alignment.AlignHeight = TAOWAlignHeight.ahBoth;
            ChatMemo.Alignment.HeightPercent = 0;
            ChatMemo.Alignment.WidthPercent = 0;
            ChatMemo.Alignment.LeftOffPercent = 0;
            ChatMemo.Alignment.RightOffPercent = 0;
            ChatMemo.Alignment.TopOffPercent = 0;
            ChatMemo.Alignment.BottomOffPercent = 0;
            ChatMemo.Alignment.LeftOffset = 1;
            ChatMemo.Alignment.RightOffset = 117;
            ChatMemo.Alignment.TopOffset = 0;
            ChatMemo.Alignment.BottomOffset = 21;
            ChatMemo.Masked = false;
            ChatMemo.WinWidth = 306;
            ChatMemo.WinHeight = 108;
            ChatMemo.BorderWidth = 8;
            ChatMemo.BorderHeight = 5;
            ChatMemo.TextColor = 12632256;
            ChatMemo.CursorChar = 124;
            ChatMemo.BlinkDelay = 0;
            ChatMemo.Editable = false;
            ChatMemo.ILIndexFrame = 113;
            ChatMemo.ILIndexTexture = 17;
            ChatMemo.ItemHeight = 15;
            ChatMemo.ColorChangeChar = 0;
            ChatMemo.TextAlign = TaowTextAlign.taLeft;
            session.EndRead(ChatMemo);
            ChatEdit = new TAOWEdit(this);
            session.BeginRead(ChatEdit);
            ChatEdit.Name = "ChatEdit";
            ChatEdit.Priority = 0;
            ChatEdit.WinLeft = 1;
            ChatEdit.WinTop = 107;
            ChatEdit.CaptureInput = true;
            ChatEdit.Alignment.AlignWidth = TAOWAlignWidth.awBoth;
            ChatEdit.Alignment.AlignHeight = TAOWAlignHeight.ahBottom;
            ChatEdit.Alignment.HeightPercent = 0;
            ChatEdit.Alignment.WidthPercent = 0;
            ChatEdit.Alignment.LeftOffPercent = 0;
            ChatEdit.Alignment.RightOffPercent = 0;
            ChatEdit.Alignment.TopOffPercent = 0;
            ChatEdit.Alignment.BottomOffPercent = 0;
            ChatEdit.Alignment.LeftOffset = 1;
            ChatEdit.Alignment.RightOffset = 117;
            ChatEdit.Alignment.TopOffset = 0;
            ChatEdit.Alignment.BottomOffset = 1;
            ChatEdit.Masked = false;
            ChatEdit.WinWidth = 306;
            ChatEdit.BorderWidth = 8;
            ChatEdit.BorderHeight = 6;
            ChatEdit.MaxChars = 0;
            ChatEdit.CursorChar = 95;
            ChatEdit.BlinkDelay = 1;
            ChatEdit.TextColor = 11002593;
            ChatEdit.Editable = true;
            ChatEdit.ILIndex = 15;
            ChatEdit.OnReturn = Layout_ChatEditReturn_4;
            ChatEdit.PassWordChar = 0;
            session.EndRead(ChatEdit);
            PlayerList = new TAOWListBox(this);
            session.BeginRead(PlayerList);
            PlayerList.Name = "PlayerList";
            PlayerList.Priority = 0;
            PlayerList.WinLeft = 307;
            PlayerList.WinTop = 0;
            PlayerList.CaptureInput = true;
            PlayerList.Alignment.AlignWidth = TAOWAlignWidth.awRight;
            PlayerList.Alignment.AlignHeight = TAOWAlignHeight.ahBoth;
            PlayerList.Alignment.HeightPercent = 0;
            PlayerList.Alignment.WidthPercent = 0;
            PlayerList.Alignment.LeftOffPercent = 70;
            PlayerList.Alignment.RightOffPercent = 0;
            PlayerList.Alignment.TopOffPercent = 0;
            PlayerList.Alignment.BottomOffPercent = 0;
            PlayerList.Alignment.LeftOffset = 11;
            PlayerList.Alignment.RightOffset = 0;
            PlayerList.Alignment.TopOffset = 0;
            PlayerList.Alignment.BottomOffset = 1;
            PlayerList.Masked = false;
            PlayerList.WinWidth = 117;
            PlayerList.WinHeight = 128;
            PlayerList.BorderWidth = 8;
            PlayerList.BorderHeight = 2;
            PlayerList.Selected = -1;
            PlayerList.OnPreDrawItem = Layout_PlayerListPreDrawItem_5;
            PlayerList.TextColor = 9415072;
            PlayerList.TextSelColor = 16777215;
            PlayerList.ILIndexFrame = 113;
            PlayerList.ILIndexTNormal = 21;
            PlayerList.ILIndexTSelected = 8;
            PlayerList.ItemHeight = 17;
            PlayerList.TextAlign = TaowTextAlign.taLeft;
            session.EndRead(PlayerList);
            SIcon = new TAOWImage(this);
            session.BeginRead(SIcon);
            SIcon.Name = "SIcon";
            SIcon.Priority = 0;
            SIcon.WinLeft = 2;
            SIcon.WinTop = 1;
            SIcon.CaptureInput = false;
            SIcon.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
            SIcon.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            SIcon.Alignment.HeightPercent = 0;
            SIcon.Alignment.WidthPercent = 0;
            SIcon.Alignment.LeftOffPercent = 0;
            SIcon.Alignment.RightOffPercent = 0;
            SIcon.Alignment.TopOffPercent = 0;
            SIcon.Alignment.BottomOffPercent = 0;
            SIcon.Alignment.LeftOffset = 2;
            SIcon.Alignment.RightOffset = 0;
            SIcon.Alignment.TopOffset = 1;
            SIcon.Alignment.BottomOffset = 0;
            SIcon.Masked = false;
            SIcon.AnimStart = 0;
            SIcon.CurrentImage = 6;
            SIcon.AnimEnd = 11;
            SIcon.WinWidth = 19;
            SIcon.WinHeight = 16;
            SIcon.Animate = false;
            SIcon.AnimFrameDelay = 0;
            session.EndRead(SIcon);
            IWTitle = new TAOWLabel(this);
            session.BeginRead(IWTitle);
            IWTitle.Name = "IWTitle";
            IWTitle.Priority = 0;
            IWTitle.WinLeft = 25;
            IWTitle.WinTop = 4;
            IWTitle.CaptureInput = false;
            IWTitle.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
            IWTitle.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            IWTitle.Alignment.HeightPercent = 0;
            IWTitle.Alignment.WidthPercent = 0;
            IWTitle.Alignment.LeftOffPercent = 0;
            IWTitle.Alignment.RightOffPercent = 0;
            IWTitle.Alignment.TopOffPercent = 0;
            IWTitle.Alignment.BottomOffPercent = 0;
            IWTitle.Alignment.LeftOffset = 25;
            IWTitle.Alignment.RightOffset = 10;
            IWTitle.Alignment.TopOffset = 4;
            IWTitle.Alignment.BottomOffset = 0;
            IWTitle.Masked = true;
            IWTitle.Text = "Chat";
            IWTitle.TextColor = 0;
            session.EndRead(IWTitle);
            Min = new TAOWButton(this);
            session.BeginRead(Min);
            Min.Name = "Min";
            Min.Priority = 0;
            Min.WinLeft = 421;
            Min.WinTop = 1;
            Min.LayoutToolTipText = new LayoutStrings()
            {
                "Minimizes the window."
            };
            Min.CaptureInput = true;
            Min.Alignment.AlignWidth = TAOWAlignWidth.awRight;
            Min.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            Min.Alignment.HeightPercent = 0;
            Min.Alignment.WidthPercent = 0;
            Min.Alignment.LeftOffPercent = 0;
            Min.Alignment.RightOffPercent = 0;
            Min.Alignment.TopOffPercent = 0;
            Min.Alignment.BottomOffPercent = 0;
            Min.Alignment.LeftOffset = 0;
            Min.Alignment.RightOffset = 4;
            Min.Alignment.TopOffset = 1;
            Min.Alignment.BottomOffset = 0;
            Min.Masked = false;
            Min.BorderHeight = 0;
            Min.BorderHeightSelected = 0;
            Min.ILIndexDown = 4;
            Min.ILIndexLit = 5;
            Min.ILIndexUp = 3;
            Min.TextColor = 0;
            Min.TextSelColor = 0;
            Min.WinWidth = 15;
            Min.OnClick = Layout_MinClick_6;
            session.EndRead(Min);
            ChatPnl = new TAOWPanel(this);
            session.BeginRead(ChatPnl);
            ChatPnl.Name = "ChatPnl";
            ChatPnl.Priority = 0;
            ChatPnl.WinLeft = 8;
            ChatPnl.WinTop = 23;
            ChatPnl.WinWidth = 424;
            ChatPnl.Masked = true;
            ChatPnl.Alignment.AlignWidth = TAOWAlignWidth.awBoth;
            ChatPnl.Alignment.AlignHeight = TAOWAlignHeight.ahBoth;
            ChatPnl.Alignment.HeightPercent = 0;
            ChatPnl.Alignment.WidthPercent = 0;
            ChatPnl.Alignment.LeftOffPercent = 0;
            ChatPnl.Alignment.RightOffPercent = 0;
            ChatPnl.Alignment.TopOffPercent = 0;
            ChatPnl.Alignment.BottomOffPercent = 0;
            ChatPnl.Alignment.LeftOffset = 8;
            ChatPnl.Alignment.RightOffset = 8;
            ChatPnl.Alignment.TopOffset = 23;
            ChatPnl.Alignment.BottomOffset = 8;
            ChatPnl.WinHeight = 129;
            ChatPnl.ILIndexFrame = 105;
            ChatPnl.ILIndexTexture = 0;
            ChatPnl.OwnerItem = 0;
            ChatPnl.MinWidth = 0;
            ChatPnl.MinHeight = 0;
            session.EndRead(ChatPnl);
            NSBar = new TAOWVScrollBar(this);
            session.BeginRead(NSBar);
            NSBar.Name = "NSBar";
            NSBar.Visible = false;
            NSBar.Priority = 0;
            NSBar.WinLeft = 407;
            NSBar.WinTop = 0;
            NSBar.CaptureInput = true;
            NSBar.Alignment.AlignWidth = TAOWAlignWidth.awNone;
            NSBar.Alignment.AlignHeight = TAOWAlignHeight.ahNone;
            NSBar.Alignment.HeightPercent = 0;
            NSBar.Alignment.WidthPercent = 0;
            NSBar.Alignment.LeftOffPercent = 0;
            NSBar.Alignment.RightOffPercent = 0;
            NSBar.Alignment.TopOffPercent = 0;
            NSBar.Alignment.BottomOffPercent = 0;
            NSBar.Alignment.LeftOffset = 0;
            NSBar.Alignment.RightOffset = 0;
            NSBar.Alignment.TopOffset = 0;
            NSBar.Alignment.BottomOffset = 0;
            NSBar.Masked = false;
            NSBar.Min = 0;
            NSBar.Max = 2;
            NSBar.SmallChange = 1;
            NSBar.LargeChange = 2;
            NSBar.Position = 0;
            NSBar.WinHeight = 128;
            NSBar.WinWidth = 17;
            NSBar.NumberVisible = 2;
            NSBar.VanishWhenFull = true;
            NSBar.ILIndexIncUp = 96;
            NSBar.ILIndexIncDown = 97;
            NSBar.ILIndexDecUp = 94;
            NSBar.ILIndexDecDown = 95;
            NSBar.ILIndexBack = 101;
            NSBar.ILIndexTab = 98;
            session.EndRead(NSBar);
            ChatSB = new TAOWVScrollBar(this);
            session.BeginRead(ChatSB);
            ChatSB.Name = "ChatSB";
            ChatSB.Visible = false;
            ChatSB.Priority = 0;
            ChatSB.WinLeft = 290;
            ChatSB.WinTop = 0;
            ChatSB.CaptureInput = true;
            ChatSB.Alignment.AlignWidth = TAOWAlignWidth.awNone;
            ChatSB.Alignment.AlignHeight = TAOWAlignHeight.ahNone;
            ChatSB.Alignment.HeightPercent = 0;
            ChatSB.Alignment.WidthPercent = 0;
            ChatSB.Alignment.LeftOffPercent = 0;
            ChatSB.Alignment.RightOffPercent = 0;
            ChatSB.Alignment.TopOffPercent = 0;
            ChatSB.Alignment.BottomOffPercent = 0;
            ChatSB.Alignment.LeftOffset = 0;
            ChatSB.Alignment.RightOffset = 0;
            ChatSB.Alignment.TopOffset = 0;
            ChatSB.Alignment.BottomOffset = 0;
            ChatSB.Masked = false;
            ChatSB.Min = 0;
            ChatSB.Max = 2;
            ChatSB.SmallChange = 1;
            ChatSB.LargeChange = 5;
            ChatSB.Position = 0;
            ChatSB.WinHeight = 108;
            ChatSB.WinWidth = 17;
            ChatSB.NumberVisible = 2;
            ChatSB.VanishWhenFull = true;
            ChatSB.ILIndexIncUp = 96;
            ChatSB.ILIndexIncDown = 97;
            ChatSB.ILIndexDecUp = 94;
            ChatSB.ILIndexDecDown = 95;
            ChatSB.ILIndexBack = 101;
            ChatSB.ILIndexTab = 98;
            session.EndRead(ChatSB);
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
            ChatMemo.AOWWindow = ChatPnl;
            ChatMemo.VScrollBar = ChatSB;
            ChatEdit.AOWWindow = ChatPnl;
            PlayerList.AOWWindow = ChatPnl;
            PlayerList.VScrollBar = NSBar;
            SIcon.AOWWindow = ChatWin;
            IWTitle.AOWWindow = ChatWin;
            Min.AOWWindow = ChatWin;
            ChatPnl.AOWWindow = ChatWin;
            NSBar.AOWWindow = ChatPnl;
            ChatSB.AOWWindow = ChatPnl;
            Loaded();
            ChatWin.Loaded();
            ChatMemo.Loaded();
            ChatEdit.Loaded();
            PlayerList.Loaded();
            SIcon.Loaded();
            IWTitle.Loaded();
            Min.Loaded();
            ChatPnl.Loaded();
            NSBar.Loaded();
            ChatSB.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            ChatWin.WinManager = AoWWinManUnit.General.Manager;
            ChatWin.ILFrame = aowIntGfxModUnit.IntGfxMod.GenericF;
            ChatWin.ILTexture = aowIntGfxModUnit.IntGfxMod.GenericT;
            ChatMemo.AOWFont = AoWExeFormsUnit.FontModule.Age10s;
            ChatMemo.ILFrame = aowIntGfxModUnit.IntGfxMod.GenericF;
            ChatMemo.ILTexture = aowIntGfxModUnit.IntGfxMod.GenericT;
            ChatEdit.AOWFont = AoWExeFormsUnit.FontModule.Age10s;
            ChatEdit.ILEdit = aowIntGfxModUnit.IntGfxMod.GenericE;
            PlayerList.AOWFont = AoWExeFormsUnit.FontModule.Age10;
            PlayerList.ILFrame = aowIntGfxModUnit.IntGfxMod.GenericF;
            PlayerList.ILTextureNormal = aowIntGfxModUnit.IntGfxMod.GenericT;
            PlayerList.ILTextureSelected = aowIntGfxModUnit.IntGfxMod.Menus;
            SIcon.ImageLib = aowIntGfxModUnit.IntGfxMod.TitleIcons;
            IWTitle.AOWFont = AoWExeFormsUnit.FontModule.Age10;
            Min.ToolTip = AoWWinManUnit.General.ToolTip;
            Min.ILIcon = aowIntGfxModUnit.IntGfxMod.GenericI;
            ChatPnl.ILFrame = aowIntGfxModUnit.IntGfxMod.GenericF;
            NSBar.ILIInc = aowIntGfxModUnit.IntGfxMod.GenericS;
            NSBar.ILIDec = aowIntGfxModUnit.IntGfxMod.GenericS;
            NSBar.ILTBack = aowIntGfxModUnit.IntGfxMod.GenericS;
            NSBar.ILLTab = aowIntGfxModUnit.IntGfxMod.GenericS;
            ChatSB.ILIInc = aowIntGfxModUnit.IntGfxMod.GenericS;
            ChatSB.ILIDec = aowIntGfxModUnit.IntGfxMod.GenericS;
            ChatSB.ILTBack = aowIntGfxModUnit.IntGfxMod.GenericS;
            ChatSB.ILLTab = aowIntGfxModUnit.IntGfxMod.GenericS;
        }

        private void Layout_ChatWinHide_0(TObject p0)
        {
            ChatWinHide();
        }

        private void Layout_ChatWinShow_1(TObject p0)
        {
            ChatWinShow();
        }

        private void Layout_ChatWinResize_2(TObject p0)
        {
            ChatWinResize();
        }

        private void Layout_ChatMemoUpdate_3(TObject p0)
        {
            ChatMemoUpdate();
        }

        private void Layout_ChatEditReturn_4(TObject p0)
        {
            ChatEditReturn();
        }

        private void Layout_PlayerListPreDrawItem_5(TAoWComponent p0, in TRect p1, int p2, ref TSurfaceDesc p3)
        {
            PlayerListPreDrawItem(default(TObject), default(TRect), p2, default(TSurfaceDesc));
        }

        private void Layout_MinClick_6(TObject p0)
        {
            MinClick();
        }
    }
}
