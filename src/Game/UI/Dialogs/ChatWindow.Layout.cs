// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Draw;
using Aow.Game.Layout;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class ChatWindow : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "ChatWindow";
        ChatWin = new Window(this);
        session.BeginRead(ChatWin);
        ChatWin.Name = "ChatWin";
        ChatWin.Visible = false;
        ChatWin.Priority = 10;
        ChatWin.WinLeft = 0;
        ChatWin.WinTop = 272;
        ChatWin.WinWidth = 440;
        ChatWin.Masked = false;
        ChatWin.OnHide = _ => Layout_ChatWinHide_0();
        ChatWin.OnShow = _ => Layout_ChatWinShow_1();
        ChatWin.Alignment.AlignWidth = AlignWidth.Both;
        ChatWin.Alignment.AlignHeight = AlignHeight.Bottom;
        ChatWin.Alignment.HeightPercent = 0;
        ChatWin.Alignment.WidthPercent = 0;
        ChatWin.Alignment.LeftOffsetPercent = 0;
        ChatWin.Alignment.RightOffsetPercent = 0;
        ChatWin.Alignment.TopOffsetPercent = 0;
        ChatWin.Alignment.BottomOffsetPercent = 0;
        ChatWin.Alignment.LeftOffset = 0;
        ChatWin.Alignment.RightOffset = 200;
        ChatWin.Alignment.TopOffset = 0;
        ChatWin.Alignment.BottomOffset = 0;
        ChatWin.UseVideoMemory = false;
        ChatWin.BorderWidth = 4;
        ChatWin.BorderHeight = 4;
        ChatWin.OverlapTopCorners = false;
        ChatWin.Cursor = 0;
        ChatWin.DoubleBuffered = true;
        ChatWin.MaskColor = 0;
        ChatWin.FreeSurfaceOnHide = true;
        ChatWin.ImageLibraryIndexFrame = 0;
        ChatWin.ImageLibraryIndexTexture = 2;
        ChatWin.DragHeight = 20;
        ChatWin.Draggable = true;
        ChatWin.Resizable = true;
        ChatWin.OnDrag = _ => Layout_ChatWinResize_2();
        ChatWin.OnResize = _ => Layout_ChatWinResize_2();
        ChatWin.MinWidth = 440;
        ChatWin.MinHeight = 160;
        ChatWin.MaxWidth = 0;
        ChatWin.MaxHeight = 0;
        ChatWin.UseManagerBorders = true;
        ChatWin.Modal = false;
        ChatWin.WinHeight = 160;
        ChatWin.Dockable = false;
        session.EndRead(ChatWin);
        ChatMemo = new Memo(this);
        session.BeginRead(ChatMemo);
        ChatMemo.Name = "ChatMemo";
        ChatMemo.OnUpdate = _ => Layout_ChatMemoUpdate_3();
        ChatMemo.Priority = 0;
        ChatMemo.WinLeft = 1;
        ChatMemo.WinTop = 0;
        ChatMemo.CaptureInput = true;
        ChatMemo.Alignment.AlignWidth = AlignWidth.Both;
        ChatMemo.Alignment.AlignHeight = AlignHeight.Both;
        ChatMemo.Alignment.HeightPercent = 0;
        ChatMemo.Alignment.WidthPercent = 0;
        ChatMemo.Alignment.LeftOffsetPercent = 0;
        ChatMemo.Alignment.RightOffsetPercent = 0;
        ChatMemo.Alignment.TopOffsetPercent = 0;
        ChatMemo.Alignment.BottomOffsetPercent = 0;
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
        ChatMemo.ImageLibraryIndexFrame = 113;
        ChatMemo.ImageLibraryIndexTexture = 17;
        ChatMemo.ItemHeight = 15;
        ChatMemo.ColorChangeChar = 0;
        ChatMemo.TextAlign = TextAlignment.Left;
        session.EndRead(ChatMemo);
        ChatEdit = new Edit(this);
        session.BeginRead(ChatEdit);
        ChatEdit.Name = "ChatEdit";
        ChatEdit.Priority = 0;
        ChatEdit.WinLeft = 1;
        ChatEdit.WinTop = 107;
        ChatEdit.CaptureInput = true;
        ChatEdit.Alignment.AlignWidth = AlignWidth.Both;
        ChatEdit.Alignment.AlignHeight = AlignHeight.Bottom;
        ChatEdit.Alignment.HeightPercent = 0;
        ChatEdit.Alignment.WidthPercent = 0;
        ChatEdit.Alignment.LeftOffsetPercent = 0;
        ChatEdit.Alignment.RightOffsetPercent = 0;
        ChatEdit.Alignment.TopOffsetPercent = 0;
        ChatEdit.Alignment.BottomOffsetPercent = 0;
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
        ChatEdit.ImageLibraryIndex = 15;
        ChatEdit.OnReturn = _ => Layout_ChatEditReturn_4();
        ChatEdit.PassWordChar = 0;
        session.EndRead(ChatEdit);
        PlayerList = new ListBox(this);
        session.BeginRead(PlayerList);
        PlayerList.Name = "PlayerList";
        PlayerList.Priority = 0;
        PlayerList.WinLeft = 307;
        PlayerList.WinTop = 0;
        PlayerList.CaptureInput = true;
        PlayerList.Alignment.AlignWidth = AlignWidth.Right;
        PlayerList.Alignment.AlignHeight = AlignHeight.Both;
        PlayerList.Alignment.HeightPercent = 0;
        PlayerList.Alignment.WidthPercent = 0;
        PlayerList.Alignment.LeftOffsetPercent = 70;
        PlayerList.Alignment.RightOffsetPercent = 0;
        PlayerList.Alignment.TopOffsetPercent = 0;
        PlayerList.Alignment.BottomOffsetPercent = 0;
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
        PlayerList.OnPreDrawItem = (_, in drawRectangle, drawItem, ref lockedSurface) => Layout_PlayerListPreDrawItem_5(in drawRectangle, drawItem, ref lockedSurface);
        PlayerList.TextColor = 9415072;
        PlayerList.TextSelectedColor = 16777215;
        PlayerList.ImageLibraryIndexFrame = 113;
        PlayerList.ImageLibraryIndexTNormal = 21;
        PlayerList.ImageLibraryIndexTSelected = 8;
        PlayerList.ItemHeight = 17;
        PlayerList.TextAlign = TextAlignment.Left;
        session.EndRead(PlayerList);
        Image sIcon = new Image(this);
        session.BeginRead(sIcon);
        sIcon.Name = "SIcon";
        sIcon.Priority = 0;
        sIcon.WinLeft = 2;
        sIcon.WinTop = 1;
        sIcon.CaptureInput = false;
        sIcon.Alignment.AlignWidth = AlignWidth.Left;
        sIcon.Alignment.AlignHeight = AlignHeight.Top;
        sIcon.Alignment.HeightPercent = 0;
        sIcon.Alignment.WidthPercent = 0;
        sIcon.Alignment.LeftOffsetPercent = 0;
        sIcon.Alignment.RightOffsetPercent = 0;
        sIcon.Alignment.TopOffsetPercent = 0;
        sIcon.Alignment.BottomOffsetPercent = 0;
        sIcon.Alignment.LeftOffset = 2;
        sIcon.Alignment.RightOffset = 0;
        sIcon.Alignment.TopOffset = 1;
        sIcon.Alignment.BottomOffset = 0;
        sIcon.Masked = false;
        sIcon.AnimationStart = 0;
        sIcon.CurrentImage = 6;
        sIcon.AnimationEnd = 11;
        sIcon.WinWidth = 19;
        sIcon.WinHeight = 16;
        sIcon.Animate = false;
        sIcon.AnimationFrameDelay = 0;
        session.EndRead(sIcon);
        Label iwTitle = new Label(this);
        session.BeginRead(iwTitle);
        iwTitle.Name = "IWTitle";
        iwTitle.Priority = 0;
        iwTitle.WinLeft = 25;
        iwTitle.WinTop = 4;
        iwTitle.CaptureInput = false;
        iwTitle.Alignment.AlignWidth = AlignWidth.Left;
        iwTitle.Alignment.AlignHeight = AlignHeight.Top;
        iwTitle.Alignment.HeightPercent = 0;
        iwTitle.Alignment.WidthPercent = 0;
        iwTitle.Alignment.LeftOffsetPercent = 0;
        iwTitle.Alignment.RightOffsetPercent = 0;
        iwTitle.Alignment.TopOffsetPercent = 0;
        iwTitle.Alignment.BottomOffsetPercent = 0;
        iwTitle.Alignment.LeftOffset = 25;
        iwTitle.Alignment.RightOffset = 10;
        iwTitle.Alignment.TopOffset = 4;
        iwTitle.Alignment.BottomOffset = 0;
        iwTitle.Masked = true;
        iwTitle.Text = "Chat";
        iwTitle.TextColor = 0;
        session.EndRead(iwTitle);
        Button min = new Button(this);
        session.BeginRead(min);
        min.Name = "Min";
        min.Priority = 0;
        min.WinLeft = 421;
        min.WinTop = 1;
        min.LayoutToolTipText = new LayoutStrings()
        {
            "Minimizes the window."
        };
        min.CaptureInput = true;
        min.Alignment.AlignWidth = AlignWidth.Right;
        min.Alignment.AlignHeight = AlignHeight.Top;
        min.Alignment.HeightPercent = 0;
        min.Alignment.WidthPercent = 0;
        min.Alignment.LeftOffsetPercent = 0;
        min.Alignment.RightOffsetPercent = 0;
        min.Alignment.TopOffsetPercent = 0;
        min.Alignment.BottomOffsetPercent = 0;
        min.Alignment.LeftOffset = 0;
        min.Alignment.RightOffset = 4;
        min.Alignment.TopOffset = 1;
        min.Alignment.BottomOffset = 0;
        min.Masked = false;
        min.BorderHeight = 0;
        min.BorderHeightSelected = 0;
        min.ImageLibraryIndexDown = 4;
        min.ImageLibraryIndexLit = 5;
        min.ImageLibraryIndexUp = 3;
        min.TextColor = 0;
        min.TextSelectedColor = 0;
        min.WinWidth = 15;
        min.OnClick = _ => Layout_MinClick_6();
        session.EndRead(min);
        Panel chatPnl = new Panel(this);
        session.BeginRead(chatPnl);
        chatPnl.Name = "ChatPnl";
        chatPnl.Priority = 0;
        chatPnl.WinLeft = 8;
        chatPnl.WinTop = 23;
        chatPnl.WinWidth = 424;
        chatPnl.Masked = true;
        chatPnl.Alignment.AlignWidth = AlignWidth.Both;
        chatPnl.Alignment.AlignHeight = AlignHeight.Both;
        chatPnl.Alignment.HeightPercent = 0;
        chatPnl.Alignment.WidthPercent = 0;
        chatPnl.Alignment.LeftOffsetPercent = 0;
        chatPnl.Alignment.RightOffsetPercent = 0;
        chatPnl.Alignment.TopOffsetPercent = 0;
        chatPnl.Alignment.BottomOffsetPercent = 0;
        chatPnl.Alignment.LeftOffset = 8;
        chatPnl.Alignment.RightOffset = 8;
        chatPnl.Alignment.TopOffset = 23;
        chatPnl.Alignment.BottomOffset = 8;
        chatPnl.WinHeight = 129;
        chatPnl.ImageLibraryIndexFrame = 105;
        chatPnl.ImageLibraryIndexTexture = 0;
        chatPnl.OwnerItem = 0;
        chatPnl.MinWidth = 0;
        chatPnl.MinHeight = 0;
        session.EndRead(chatPnl);
        VerticalScrollBar nsBar = new VerticalScrollBar(this);
        session.BeginRead(nsBar);
        nsBar.Name = "NSBar";
        nsBar.Visible = false;
        nsBar.Priority = 0;
        nsBar.WinLeft = 407;
        nsBar.WinTop = 0;
        nsBar.CaptureInput = true;
        nsBar.Alignment.AlignWidth = AlignWidth.None;
        nsBar.Alignment.AlignHeight = AlignHeight.None;
        nsBar.Alignment.HeightPercent = 0;
        nsBar.Alignment.WidthPercent = 0;
        nsBar.Alignment.LeftOffsetPercent = 0;
        nsBar.Alignment.RightOffsetPercent = 0;
        nsBar.Alignment.TopOffsetPercent = 0;
        nsBar.Alignment.BottomOffsetPercent = 0;
        nsBar.Alignment.LeftOffset = 0;
        nsBar.Alignment.RightOffset = 0;
        nsBar.Alignment.TopOffset = 0;
        nsBar.Alignment.BottomOffset = 0;
        nsBar.Masked = false;
        nsBar.Min = 0;
        nsBar.Max = 2;
        nsBar.SmallChange = 1;
        nsBar.LargeChange = 2;
        nsBar.Position = 0;
        nsBar.WinHeight = 128;
        nsBar.WinWidth = 17;
        nsBar.NumberVisible = 2;
        nsBar.VanishWhenFull = true;
        nsBar.ImageLibraryIndexIncrementUp = 96;
        nsBar.ImageLibraryIndexIncrementDown = 97;
        nsBar.ImageLibraryIndexDecrementUp = 94;
        nsBar.ImageLibraryIndexDecrementDown = 95;
        nsBar.ImageLibraryIndexBack = 101;
        nsBar.ThumbImageIndex = 98;
        session.EndRead(nsBar);
        ChatSB = new VerticalScrollBar(this);
        session.BeginRead(ChatSB);
        ChatSB.Name = "ChatSB";
        ChatSB.Visible = false;
        ChatSB.Priority = 0;
        ChatSB.WinLeft = 290;
        ChatSB.WinTop = 0;
        ChatSB.CaptureInput = true;
        ChatSB.Alignment.AlignWidth = AlignWidth.None;
        ChatSB.Alignment.AlignHeight = AlignHeight.None;
        ChatSB.Alignment.HeightPercent = 0;
        ChatSB.Alignment.WidthPercent = 0;
        ChatSB.Alignment.LeftOffsetPercent = 0;
        ChatSB.Alignment.RightOffsetPercent = 0;
        ChatSB.Alignment.TopOffsetPercent = 0;
        ChatSB.Alignment.BottomOffsetPercent = 0;
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
        ChatSB.ImageLibraryIndexIncrementUp = 96;
        ChatSB.ImageLibraryIndexIncrementDown = 97;
        ChatSB.ImageLibraryIndexDecrementUp = 94;
        ChatSB.ImageLibraryIndexDecrementDown = 95;
        ChatSB.ImageLibraryIndexBack = 101;
        ChatSB.ThumbImageIndex = 98;
        session.EndRead(ChatSB);
        LayoutIvTranslator translator = new LayoutIvTranslator(this);
        session.BeginRead(translator);
        translator.Name = "Translator";
        translator.DictionaryName = "AoW";
        translator.TargetsData.Add(new TranslationTarget("", "Items", 0));
        translator.TargetsData.Add(new TranslationTarget("", "Text", 0));
        translator.TargetsData.Add(new TranslationTarget("", "ToolTipText", 0));
        session.EndRead(translator);
        session.EndRead(this);
        ChatMemo.Window = chatPnl;
        ChatMemo.VerticalScrollBar = ChatSB;
        ChatEdit.Window = chatPnl;
        PlayerList.Window = chatPnl;
        PlayerList.VerticalScrollBar = nsBar;
        sIcon.Window = ChatWin;
        iwTitle.Window = ChatWin;
        min.Window = ChatWin;
        chatPnl.Window = ChatWin;
        nsBar.Window = chatPnl;
        ChatSB.Window = chatPnl;
        Loaded();
        ChatWin.Loaded();
        ChatMemo.Loaded();
        ChatEdit.Loaded();
        PlayerList.Loaded();
        sIcon.Loaded();
        iwTitle.Loaded();
        min.Loaded();
        chatPnl.Loaded();
        nsBar.Loaded();
        ChatSB.Loaded();
        translator.Loaded();
        session.Resolve(this);
        ChatWin.WindowManager = DialogHosts.General.Manager;
        ChatWin.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        ChatWin.ImageLibraryTexture = DialogHosts.UIGraphics.GenericT;
        ChatMemo.Font = DialogHosts.UIFonts.Age10s;
        ChatMemo.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        ChatMemo.ImageLibraryTexture = DialogHosts.UIGraphics.GenericT;
        ChatEdit.Font = DialogHosts.UIFonts.Age10s;
        ChatEdit.ImageLibraryEdit = DialogHosts.UIGraphics.GenericE;
        PlayerList.Font = DialogHosts.UIFonts.Age10;
        PlayerList.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        PlayerList.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericT;
        PlayerList.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        sIcon.ImageLibrary = DialogHosts.UIGraphics.TitleIcons;
        iwTitle.Font = DialogHosts.UIFonts.Age10;
        min.ToolTip = DialogHosts.General.ToolTip;
        min.ImageLibraryIcon = DialogHosts.UIGraphics.GenericI;
        chatPnl.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        nsBar.ImageLibraryImageIncrement = DialogHosts.UIGraphics.GenericS;
        nsBar.ImageLibraryIDecrement = DialogHosts.UIGraphics.GenericS;
        nsBar.ImageLibraryTileBack = DialogHosts.UIGraphics.GenericS;
        nsBar.ThumbStrip = DialogHosts.UIGraphics.GenericS;
        ChatSB.ImageLibraryImageIncrement = DialogHosts.UIGraphics.GenericS;
        ChatSB.ImageLibraryIDecrement = DialogHosts.UIGraphics.GenericS;
        ChatSB.ImageLibraryTileBack = DialogHosts.UIGraphics.GenericS;
        ChatSB.ThumbStrip = DialogHosts.UIGraphics.GenericS;
    }

    private void Layout_ChatWinHide_0()
    {
        ChatWinHide();
    }

    private void Layout_ChatWinShow_1()
    {
        ChatWinShow();
    }

    private void Layout_ChatWinResize_2()
    {
        ChatWinResize();
    }

    private void Layout_ChatMemoUpdate_3()
    {
        ChatMemoUpdate();
    }

    private void Layout_ChatEditReturn_4()
    {
        ChatEditReturn();
    }

    private void Layout_PlayerListPreDrawItem_5(in Rect p1, int p2, ref LockedSurface p3)
    {
        PlayerListPreDrawItem(default(Rect), p2, default(LockedSurface));
    }

    private void Layout_MinClick_6()
    {
        MinClick();
    }
}
