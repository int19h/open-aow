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
        Close = new Button(this);
        session.BeginRead(Close);
        Close.Name = "Close";
        Close.Priority = 0;
        Close.WinLeft = 122;
        Close.WinTop = 211;
        Close.CaptureInput = true;
        Close.Alignment.AlignWidth = AlignWidth.Center;
        Close.Alignment.AlignHeight = AlignHeight.Bottom;
        Close.Alignment.HeightPercent = 0;
        Close.Alignment.WidthPercent = 0;
        Close.Alignment.LeftOffsetPercent = 0;
        Close.Alignment.RightOffsetPercent = 0;
        Close.Alignment.TopOffsetPercent = 0;
        Close.Alignment.BottomOffsetPercent = 0;
        Close.Alignment.LeftOffset = 0;
        Close.Alignment.RightOffset = 0;
        Close.Alignment.TopOffset = 1;
        Close.Alignment.BottomOffset = 25;
        Close.Masked = true;
        Close.BorderHeight = 8;
        Close.BorderHeightSelected = 8;
        Close.ImageLibraryIndexDown = 6;
        Close.ImageLibraryIndexLit = 3;
        Close.ImageLibraryIndexUp = 0;
        Close.Text = "Close";
        Close.TextColor = 855324;
        Close.TextSelectedColor = 16777215;
        Close.WinWidth = 140;
        Close.OnClick = _ => Layout_CloseClick_1();
        session.EndRead(Close);
        MessageSB = new VerticalScrollBar(this);
        session.BeginRead(MessageSB);
        MessageSB.Name = "MessageSB";
        MessageSB.Visible = false;
        MessageSB.Priority = 200;
        MessageSB.WinLeft = 301;
        MessageSB.WinTop = 45;
        MessageSB.CaptureInput = true;
        MessageSB.Alignment.AlignWidth = AlignWidth.None;
        MessageSB.Alignment.AlignHeight = AlignHeight.None;
        MessageSB.Alignment.HeightPercent = 0;
        MessageSB.Alignment.WidthPercent = 0;
        MessageSB.Alignment.LeftOffsetPercent = 0;
        MessageSB.Alignment.RightOffsetPercent = 0;
        MessageSB.Alignment.TopOffsetPercent = 0;
        MessageSB.Alignment.BottomOffsetPercent = 0;
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
        MessageSB.ImageLibraryIndexIncrementUp = 42;
        MessageSB.ImageLibraryIndexIncrementDown = 43;
        MessageSB.ImageLibraryIndexDecrementUp = 40;
        MessageSB.ImageLibraryIndexDecrementDown = 41;
        MessageSB.ImageLibraryIndexBack = 47;
        MessageSB.ThumbImageIndex = 44;
        session.EndRead(MessageSB);
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
        MessageMemo.Window = MsgReceiveWin;
        MessageMemo.VerticalScrollBar = MessageSB;
        Close.Window = MsgReceiveWin;
        MessageSB.Window = MsgReceiveWin;
        LShield.Window = MsgReceiveWin;
        RShield.Window = MsgReceiveWin;
        HeaderMemo.Window = MsgReceiveWin;
        HeaderMemo.VerticalScrollBar = MessageSB;
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
        MsgReceiveWin.WindowManager = DialogHosts.General.Manager;
        MsgReceiveWin.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        MsgReceiveWin.ImageLibraryTexture = DialogHosts.UIGraphics.GenericT;
        MessageMemo.Font = DialogHosts.UIFonts.Font15BlackBrown;
        MessageMemo.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        Close.Font = DialogHosts.UIFonts.Age10b;
        Close.ImageLibraryButton = DialogHosts.UIGraphics.CampaignTrans;
        MessageSB.ImageLibraryImageIncrement = DialogHosts.UIGraphics.GenericS;
        MessageSB.ImageLibraryIDecrement = DialogHosts.UIGraphics.GenericS;
        MessageSB.ImageLibraryTileBack = DialogHosts.UIGraphics.GenericS;
        MessageSB.ThumbStrip = DialogHosts.UIGraphics.GenericS;
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
