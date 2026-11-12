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
        SIcon = new Image(this);
        session.BeginRead(SIcon);
        SIcon.Name = "SIcon";
        SIcon.Priority = 0;
        SIcon.WinLeft = 2;
        SIcon.WinTop = 1;
        SIcon.CaptureInput = false;
        SIcon.Alignment.AlignWidth = AlignWidth.Left;
        SIcon.Alignment.AlignHeight = AlignHeight.Top;
        SIcon.Alignment.HeightPercent = 0;
        SIcon.Alignment.WidthPercent = 0;
        SIcon.Alignment.LeftOffsetPercent = 0;
        SIcon.Alignment.RightOffsetPercent = 0;
        SIcon.Alignment.TopOffsetPercent = 0;
        SIcon.Alignment.BottomOffsetPercent = 0;
        SIcon.Alignment.LeftOffset = 2;
        SIcon.Alignment.RightOffset = 0;
        SIcon.Alignment.TopOffset = 1;
        SIcon.Alignment.BottomOffset = 0;
        SIcon.Masked = false;
        SIcon.AnimationStart = 0;
        SIcon.CurrentImage = 6;
        SIcon.AnimationEnd = 11;
        SIcon.WinWidth = 19;
        SIcon.WinHeight = 16;
        SIcon.Animate = false;
        SIcon.AnimationFrameDelay = 0;
        session.EndRead(SIcon);
        IWTitle = new Label(this);
        session.BeginRead(IWTitle);
        IWTitle.Name = "IWTitle";
        IWTitle.Priority = 0;
        IWTitle.WinLeft = 25;
        IWTitle.WinTop = 4;
        IWTitle.CaptureInput = false;
        IWTitle.Alignment.AlignWidth = AlignWidth.Left;
        IWTitle.Alignment.AlignHeight = AlignHeight.Top;
        IWTitle.Alignment.HeightPercent = 0;
        IWTitle.Alignment.WidthPercent = 0;
        IWTitle.Alignment.LeftOffsetPercent = 0;
        IWTitle.Alignment.RightOffsetPercent = 0;
        IWTitle.Alignment.TopOffsetPercent = 0;
        IWTitle.Alignment.BottomOffsetPercent = 0;
        IWTitle.Alignment.LeftOffset = 25;
        IWTitle.Alignment.RightOffset = 10;
        IWTitle.Alignment.TopOffset = 4;
        IWTitle.Alignment.BottomOffset = 0;
        IWTitle.Masked = true;
        IWTitle.Text = "Chat";
        IWTitle.TextColor = 0;
        session.EndRead(IWTitle);
        Min = new Button(this);
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
        Min.Alignment.AlignWidth = AlignWidth.Right;
        Min.Alignment.AlignHeight = AlignHeight.Top;
        Min.Alignment.HeightPercent = 0;
        Min.Alignment.WidthPercent = 0;
        Min.Alignment.LeftOffsetPercent = 0;
        Min.Alignment.RightOffsetPercent = 0;
        Min.Alignment.TopOffsetPercent = 0;
        Min.Alignment.BottomOffsetPercent = 0;
        Min.Alignment.LeftOffset = 0;
        Min.Alignment.RightOffset = 4;
        Min.Alignment.TopOffset = 1;
        Min.Alignment.BottomOffset = 0;
        Min.Masked = false;
        Min.BorderHeight = 0;
        Min.BorderHeightSelected = 0;
        Min.ImageLibraryIndexDown = 4;
        Min.ImageLibraryIndexLit = 5;
        Min.ImageLibraryIndexUp = 3;
        Min.TextColor = 0;
        Min.TextSelectedColor = 0;
        Min.WinWidth = 15;
        Min.OnClick = _ => Layout_MinClick_6();
        session.EndRead(Min);
        ChatPnl = new Panel(this);
        session.BeginRead(ChatPnl);
        ChatPnl.Name = "ChatPnl";
        ChatPnl.Priority = 0;
        ChatPnl.WinLeft = 8;
        ChatPnl.WinTop = 23;
        ChatPnl.WinWidth = 424;
        ChatPnl.Masked = true;
        ChatPnl.Alignment.AlignWidth = AlignWidth.Both;
        ChatPnl.Alignment.AlignHeight = AlignHeight.Both;
        ChatPnl.Alignment.HeightPercent = 0;
        ChatPnl.Alignment.WidthPercent = 0;
        ChatPnl.Alignment.LeftOffsetPercent = 0;
        ChatPnl.Alignment.RightOffsetPercent = 0;
        ChatPnl.Alignment.TopOffsetPercent = 0;
        ChatPnl.Alignment.BottomOffsetPercent = 0;
        ChatPnl.Alignment.LeftOffset = 8;
        ChatPnl.Alignment.RightOffset = 8;
        ChatPnl.Alignment.TopOffset = 23;
        ChatPnl.Alignment.BottomOffset = 8;
        ChatPnl.WinHeight = 129;
        ChatPnl.ImageLibraryIndexFrame = 105;
        ChatPnl.ImageLibraryIndexTexture = 0;
        ChatPnl.OwnerItem = 0;
        ChatPnl.MinWidth = 0;
        ChatPnl.MinHeight = 0;
        session.EndRead(ChatPnl);
        NSBar = new VerticalScrollBar(this);
        session.BeginRead(NSBar);
        NSBar.Name = "NSBar";
        NSBar.Visible = false;
        NSBar.Priority = 0;
        NSBar.WinLeft = 407;
        NSBar.WinTop = 0;
        NSBar.CaptureInput = true;
        NSBar.Alignment.AlignWidth = AlignWidth.None;
        NSBar.Alignment.AlignHeight = AlignHeight.None;
        NSBar.Alignment.HeightPercent = 0;
        NSBar.Alignment.WidthPercent = 0;
        NSBar.Alignment.LeftOffsetPercent = 0;
        NSBar.Alignment.RightOffsetPercent = 0;
        NSBar.Alignment.TopOffsetPercent = 0;
        NSBar.Alignment.BottomOffsetPercent = 0;
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
        NSBar.ImageLibraryIndexIncrementUp = 96;
        NSBar.ImageLibraryIndexIncrementDown = 97;
        NSBar.ImageLibraryIndexDecrementUp = 94;
        NSBar.ImageLibraryIndexDecrementDown = 95;
        NSBar.ImageLibraryIndexBack = 101;
        NSBar.ThumbImageIndex = 98;
        session.EndRead(NSBar);
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
        ChatMemo.Window = ChatPnl;
        ChatMemo.VerticalScrollBar = ChatSB;
        ChatEdit.Window = ChatPnl;
        PlayerList.Window = ChatPnl;
        PlayerList.VerticalScrollBar = NSBar;
        SIcon.Window = ChatWin;
        IWTitle.Window = ChatWin;
        Min.Window = ChatWin;
        ChatPnl.Window = ChatWin;
        NSBar.Window = ChatPnl;
        ChatSB.Window = ChatPnl;
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
        SIcon.ImageLibrary = DialogHosts.UIGraphics.TitleIcons;
        IWTitle.Font = DialogHosts.UIFonts.Age10;
        Min.ToolTip = DialogHosts.General.ToolTip;
        Min.ImageLibraryIcon = DialogHosts.UIGraphics.GenericI;
        ChatPnl.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        NSBar.ImageLibraryImageIncrement = DialogHosts.UIGraphics.GenericS;
        NSBar.ImageLibraryIDecrement = DialogHosts.UIGraphics.GenericS;
        NSBar.ImageLibraryTileBack = DialogHosts.UIGraphics.GenericS;
        NSBar.ThumbStrip = DialogHosts.UIGraphics.GenericS;
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
