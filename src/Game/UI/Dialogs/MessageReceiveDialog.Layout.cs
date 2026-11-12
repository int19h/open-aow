// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class MessageReceiveDialog : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "MsgReceiveDlg";
        MsgReceiveWin = new Window(this);
        session.BeginRead(MsgReceiveWin);
        MsgReceiveWin.Name = "MsgReceiveWin";
        MsgReceiveWin.Visible = false;
        MsgReceiveWin.Priority = 300;
        MsgReceiveWin.WinLeft = 128;
        MsgReceiveWin.WinTop = 85;
        MsgReceiveWin.WinWidth = 384;
        MsgReceiveWin.Masked = true;
        MsgReceiveWin.OnHide = _ => Layout_MsgReceiveWinHide_0();
        MsgReceiveWin.Alignment.AlignWidth = AlignWidth.Center;
        MsgReceiveWin.Alignment.AlignHeight = AlignHeight.Center;
        MsgReceiveWin.Alignment.HeightPercent = 0;
        MsgReceiveWin.Alignment.WidthPercent = 0;
        MsgReceiveWin.Alignment.LeftOffsetPercent = 0;
        MsgReceiveWin.Alignment.RightOffsetPercent = 0;
        MsgReceiveWin.Alignment.TopOffsetPercent = 0;
        MsgReceiveWin.Alignment.BottomOffsetPercent = 0;
        MsgReceiveWin.Alignment.LeftOffset = 0;
        MsgReceiveWin.Alignment.RightOffset = 0;
        MsgReceiveWin.Alignment.TopOffset = 0;
        MsgReceiveWin.Alignment.BottomOffset = 0;
        MsgReceiveWin.UseVideoMemory = false;
        MsgReceiveWin.BorderWidth = 0;
        MsgReceiveWin.BorderHeight = 0;
        MsgReceiveWin.OverlapTopCorners = false;
        MsgReceiveWin.Cursor = 0;
        MsgReceiveWin.DoubleBuffered = true;
        MsgReceiveWin.MaskColor = 5808200;
        MsgReceiveWin.FreeSurfaceOnHide = false;
        MsgReceiveWin.ImageLibraryIndexFrame = 80;
        MsgReceiveWin.ImageLibraryIndexTexture = 7;
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
        MessageMemo = new Memo(this);
        session.BeginRead(MessageMemo);
        MessageMemo.Name = "MessageMemo";
        MessageMemo.Priority = 200;
        MessageMemo.WinLeft = 47;
        MessageMemo.WinTop = 100;
        MessageMemo.CaptureInput = false;
        MessageMemo.Alignment.AlignWidth = AlignWidth.Both;
        MessageMemo.Alignment.AlignHeight = AlignHeight.Both;
        MessageMemo.Alignment.HeightPercent = 0;
        MessageMemo.Alignment.WidthPercent = 0;
        MessageMemo.Alignment.LeftOffsetPercent = 0;
        MessageMemo.Alignment.RightOffsetPercent = 0;
        MessageMemo.Alignment.TopOffsetPercent = 0;
        MessageMemo.Alignment.BottomOffsetPercent = 0;
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
        MessageMemo.ImageLibraryIndexFrame = 40;
        MessageMemo.ImageLibraryIndexTexture = 0;
        MessageMemo.ItemHeight = 20;
        MessageMemo.ColorChangeChar = 0;
        MessageMemo.TextAlign = TextAlignment.Center;
        session.EndRead(MessageMemo);
        Button close = new Button(this);
        session.BeginRead(close);
        close.Name = "Close";
        close.Priority = 0;
        close.WinLeft = 122;
        close.WinTop = 211;
        close.CaptureInput = true;
        close.Alignment.AlignWidth = AlignWidth.Center;
        close.Alignment.AlignHeight = AlignHeight.Bottom;
        close.Alignment.HeightPercent = 0;
        close.Alignment.WidthPercent = 0;
        close.Alignment.LeftOffsetPercent = 0;
        close.Alignment.RightOffsetPercent = 0;
        close.Alignment.TopOffsetPercent = 0;
        close.Alignment.BottomOffsetPercent = 0;
        close.Alignment.LeftOffset = 0;
        close.Alignment.RightOffset = 0;
        close.Alignment.TopOffset = 1;
        close.Alignment.BottomOffset = 25;
        close.Masked = true;
        close.BorderHeight = 8;
        close.BorderHeightSelected = 8;
        close.ImageLibraryIndexDown = 6;
        close.ImageLibraryIndexLit = 3;
        close.ImageLibraryIndexUp = 0;
        close.Text = "Close";
        close.TextColor = 855324;
        close.TextSelectedColor = 16777215;
        close.WinWidth = 140;
        close.OnClick = _ => Layout_CloseClick_1();
        session.EndRead(close);
        VerticalScrollBar messageSB = new VerticalScrollBar(this);
        session.BeginRead(messageSB);
        messageSB.Name = "MessageSB";
        messageSB.Visible = false;
        messageSB.Priority = 200;
        messageSB.WinLeft = 301;
        messageSB.WinTop = 45;
        messageSB.CaptureInput = true;
        messageSB.Alignment.AlignWidth = AlignWidth.None;
        messageSB.Alignment.AlignHeight = AlignHeight.None;
        messageSB.Alignment.HeightPercent = 0;
        messageSB.Alignment.WidthPercent = 0;
        messageSB.Alignment.LeftOffsetPercent = 0;
        messageSB.Alignment.RightOffsetPercent = 0;
        messageSB.Alignment.TopOffsetPercent = 0;
        messageSB.Alignment.BottomOffsetPercent = 0;
        messageSB.Alignment.LeftOffset = 0;
        messageSB.Alignment.RightOffset = 0;
        messageSB.Alignment.TopOffset = 0;
        messageSB.Alignment.BottomOffset = 0;
        messageSB.Masked = true;
        messageSB.Min = 0;
        messageSB.Max = 0;
        messageSB.SmallChange = 1;
        messageSB.LargeChange = 5;
        messageSB.Position = 0;
        messageSB.WinHeight = 137;
        messageSB.WinWidth = 13;
        messageSB.NumberVisible = 1;
        messageSB.VanishWhenFull = true;
        messageSB.ImageLibraryIndexIncrementUp = 42;
        messageSB.ImageLibraryIndexIncrementDown = 43;
        messageSB.ImageLibraryIndexDecrementUp = 40;
        messageSB.ImageLibraryIndexDecrementDown = 41;
        messageSB.ImageLibraryIndexBack = 47;
        messageSB.ThumbImageIndex = 44;
        session.EndRead(messageSB);
        LShield = new Image(this);
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
        LShield.Alignment.AlignWidth = AlignWidth.Left;
        LShield.Alignment.AlignHeight = AlignHeight.Top;
        LShield.Alignment.HeightPercent = 0;
        LShield.Alignment.WidthPercent = 0;
        LShield.Alignment.LeftOffsetPercent = 0;
        LShield.Alignment.RightOffsetPercent = 0;
        LShield.Alignment.TopOffsetPercent = 0;
        LShield.Alignment.BottomOffsetPercent = 0;
        LShield.Alignment.LeftOffset = 45;
        LShield.Alignment.RightOffset = 0;
        LShield.Alignment.TopOffset = 40;
        LShield.Alignment.BottomOffset = 0;
        LShield.Masked = true;
        LShield.AnimationStart = 0;
        LShield.CurrentImage = 22;
        LShield.AnimationEnd = 38;
        LShield.WinWidth = 40;
        LShield.WinHeight = 53;
        LShield.Animate = false;
        LShield.AnimationFrameDelay = 0;
        session.EndRead(LShield);
        RShield = new Image(this);
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
        RShield.Alignment.AlignWidth = AlignWidth.Right;
        RShield.Alignment.AlignHeight = AlignHeight.Top;
        RShield.Alignment.HeightPercent = 0;
        RShield.Alignment.WidthPercent = 0;
        RShield.Alignment.LeftOffsetPercent = 0;
        RShield.Alignment.RightOffsetPercent = 0;
        RShield.Alignment.TopOffsetPercent = 0;
        RShield.Alignment.BottomOffsetPercent = 0;
        RShield.Alignment.LeftOffset = 80;
        RShield.Alignment.RightOffset = 45;
        RShield.Alignment.TopOffset = 45;
        RShield.Alignment.BottomOffset = 0;
        RShield.Masked = true;
        RShield.AnimationStart = 0;
        RShield.CurrentImage = 16;
        RShield.AnimationEnd = 38;
        RShield.WinWidth = 40;
        RShield.WinHeight = 53;
        RShield.Animate = false;
        RShield.AnimationFrameDelay = 0;
        session.EndRead(RShield);
        HeaderMemo = new Memo(this);
        session.BeginRead(HeaderMemo);
        HeaderMemo.Name = "HeaderMemo";
        HeaderMemo.Priority = 200;
        HeaderMemo.WinLeft = 70;
        HeaderMemo.WinTop = 45;
        HeaderMemo.CaptureInput = false;
        HeaderMemo.Alignment.AlignWidth = AlignWidth.Both;
        HeaderMemo.Alignment.AlignHeight = AlignHeight.Both;
        HeaderMemo.Alignment.HeightPercent = 0;
        HeaderMemo.Alignment.WidthPercent = 0;
        HeaderMemo.Alignment.LeftOffsetPercent = 0;
        HeaderMemo.Alignment.RightOffsetPercent = 0;
        HeaderMemo.Alignment.TopOffsetPercent = 0;
        HeaderMemo.Alignment.BottomOffsetPercent = 0;
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
        HeaderMemo.ImageLibraryIndexFrame = 40;
        HeaderMemo.ImageLibraryIndexTexture = 0;
        HeaderMemo.ItemHeight = 20;
        HeaderMemo.ColorChangeChar = 0;
        HeaderMemo.TextAlign = TextAlignment.Center;
        session.EndRead(HeaderMemo);
        LayoutIvTranslator translator = new LayoutIvTranslator(this);
        session.BeginRead(translator);
        translator.Name = "Translator";
        translator.DictionaryName = "AoW";
        translator.TargetsData.Add(new TranslationTarget("", "Items", 0));
        translator.TargetsData.Add(new TranslationTarget("", "Text", 0));
        translator.TargetsData.Add(new TranslationTarget("", "ToolTipText", 0));
        session.EndRead(translator);
        session.EndRead(this);
        MessageMemo.Window = MsgReceiveWin;
        MessageMemo.VerticalScrollBar = messageSB;
        close.Window = MsgReceiveWin;
        messageSB.Window = MsgReceiveWin;
        LShield.Window = MsgReceiveWin;
        RShield.Window = MsgReceiveWin;
        HeaderMemo.Window = MsgReceiveWin;
        HeaderMemo.VerticalScrollBar = messageSB;
        Loaded();
        MsgReceiveWin.Loaded();
        MessageMemo.Loaded();
        close.Loaded();
        messageSB.Loaded();
        LShield.Loaded();
        RShield.Loaded();
        HeaderMemo.Loaded();
        translator.Loaded();
        session.Resolve(this);
        MsgReceiveWin.WindowManager = DialogHosts.General.Manager;
        MsgReceiveWin.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        MsgReceiveWin.ImageLibraryTexture = DialogHosts.UIGraphics.GenericT;
        MessageMemo.Font = DialogHosts.UIFonts.Font15BlackBrown;
        MessageMemo.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        close.Font = DialogHosts.UIFonts.Age10b;
        close.ImageLibraryButton = DialogHosts.UIGraphics.CampaignTrans;
        messageSB.ImageLibraryImageIncrement = DialogHosts.UIGraphics.GenericS;
        messageSB.ImageLibraryIDecrement = DialogHosts.UIGraphics.GenericS;
        messageSB.ImageLibraryTileBack = DialogHosts.UIGraphics.GenericS;
        messageSB.ThumbStrip = DialogHosts.UIGraphics.GenericS;
        LShield.ToolTip = DialogHosts.General.ToolTip;
        LShield.ImageLibrary = DialogHosts.UIGraphics.Shields;
        RShield.ToolTip = DialogHosts.General.ToolTip;
        RShield.ImageLibrary = DialogHosts.UIGraphics.Shields;
        HeaderMemo.Font = DialogHosts.UIFonts.Font15BlackBrown;
        HeaderMemo.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
    }

    private void Layout_MsgReceiveWinHide_0()
    {
        MsgReceiveWinHide();
    }

    private void Layout_CloseClick_1()
    {
        CloseClick();
    }
}
