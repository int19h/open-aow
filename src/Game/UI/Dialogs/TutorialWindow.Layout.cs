// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class TutorialWindow : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "TutorialWindow";
        Dlg = new Window(this);
        session.BeginRead(Dlg);
        Dlg.Name = "Dlg";
        Dlg.Visible = false;
        Dlg.Priority = 9999;
        Dlg.WinLeft = 125;
        Dlg.WinTop = 85;
        Dlg.WinWidth = 390;
        Dlg.Masked = true;
        Dlg.OnHide = _ => Layout_DlgHide_0();
        Dlg.Alignment.AlignWidth = AlignWidth.Center;
        Dlg.Alignment.AlignHeight = AlignHeight.Center;
        Dlg.Alignment.HeightPercent = 0;
        Dlg.Alignment.WidthPercent = 0;
        Dlg.Alignment.LeftOffsetPercent = 0;
        Dlg.Alignment.RightOffsetPercent = 0;
        Dlg.Alignment.TopOffsetPercent = 0;
        Dlg.Alignment.BottomOffsetPercent = 0;
        Dlg.Alignment.LeftOffset = 0;
        Dlg.Alignment.RightOffset = 0;
        Dlg.Alignment.TopOffset = 0;
        Dlg.Alignment.BottomOffset = 0;
        Dlg.UseVideoMemory = false;
        Dlg.BorderWidth = 0;
        Dlg.BorderHeight = 0;
        Dlg.OverlapTopCorners = false;
        Dlg.Cursor = 0;
        Dlg.DoubleBuffered = true;
        Dlg.MaskColor = 5808200;
        Dlg.FreeSurfaceOnHide = true;
        Dlg.ImageLibraryIndexFrame = 80;
        Dlg.ImageLibraryIndexTexture = 34;
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
        MessageMemo = new Memo(this);
        session.BeginRead(MessageMemo);
        MessageMemo.Name = "MessageMemo";
        MessageMemo.Priority = 200;
        MessageMemo.WinLeft = 52;
        MessageMemo.WinTop = 80;
        MessageMemo.CaptureInput = false;
        MessageMemo.Alignment.AlignWidth = AlignWidth.Both;
        MessageMemo.Alignment.AlignHeight = AlignHeight.Both;
        MessageMemo.Alignment.HeightPercent = 0;
        MessageMemo.Alignment.WidthPercent = 0;
        MessageMemo.Alignment.LeftOffsetPercent = 0;
        MessageMemo.Alignment.RightOffsetPercent = 0;
        MessageMemo.Alignment.TopOffsetPercent = 0;
        MessageMemo.Alignment.BottomOffsetPercent = 0;
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
        MessageMemo.ImageLibraryIndexFrame = 40;
        MessageMemo.ImageLibraryIndexTexture = 0;
        MessageMemo.ItemHeight = 17;
        MessageMemo.ColorChangeChar = 0;
        MessageMemo.TextAlign = TextAlignment.Center;
        session.EndRead(MessageMemo);
        Close = new Button(this);
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
        Close.Text = "OK";
        Close.TextColor = 855324;
        Close.TextSelectedColor = 16777215;
        Close.WinWidth = 140;
        Close.OnClick = _ => Layout_CloseClick_1();
        session.EndRead(Close);
        HeaderMemo = new Memo(this);
        session.BeginRead(HeaderMemo);
        HeaderMemo.Name = "HeaderMemo";
        HeaderMemo.Priority = 200;
        HeaderMemo.WinLeft = 70;
        HeaderMemo.WinTop = 35;
        HeaderMemo.CaptureInput = false;
        HeaderMemo.Alignment.AlignWidth = AlignWidth.Both;
        HeaderMemo.Alignment.AlignHeight = AlignHeight.Top;
        HeaderMemo.Alignment.HeightPercent = 0;
        HeaderMemo.Alignment.WidthPercent = 0;
        HeaderMemo.Alignment.LeftOffsetPercent = 0;
        HeaderMemo.Alignment.RightOffsetPercent = 0;
        HeaderMemo.Alignment.TopOffsetPercent = 0;
        HeaderMemo.Alignment.BottomOffsetPercent = 0;
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
        HeaderMemo.ImageLibraryIndexFrame = 40;
        HeaderMemo.ImageLibraryIndexTexture = 0;
        HeaderMemo.ItemHeight = 20;
        HeaderMemo.ColorChangeChar = 0;
        HeaderMemo.TextAlign = TextAlignment.Center;
        session.EndRead(HeaderMemo);
        AbVSB = new VerticalScrollBar(this);
        session.BeginRead(AbVSB);
        AbVSB.Name = "AbVSB";
        AbVSB.Visible = false;
        AbVSB.Priority = 200;
        AbVSB.WinLeft = 325;
        AbVSB.WinTop = 80;
        AbVSB.CaptureInput = true;
        AbVSB.Alignment.AlignWidth = AlignWidth.None;
        AbVSB.Alignment.AlignHeight = AlignHeight.None;
        AbVSB.Alignment.HeightPercent = 0;
        AbVSB.Alignment.WidthPercent = 0;
        AbVSB.Alignment.LeftOffsetPercent = 0;
        AbVSB.Alignment.RightOffsetPercent = 0;
        AbVSB.Alignment.TopOffsetPercent = 0;
        AbVSB.Alignment.BottomOffsetPercent = 0;
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
        AbVSB.ImageLibraryIndexIncrementUp = 42;
        AbVSB.ImageLibraryIndexIncrementDown = 43;
        AbVSB.ImageLibraryIndexDecrementUp = 40;
        AbVSB.ImageLibraryIndexDecrementDown = 41;
        AbVSB.ImageLibraryIndexBack = 47;
        AbVSB.ThumbImageIndex = 44;
        session.EndRead(AbVSB);
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
        MessageMemo.Window = Dlg;
        MessageMemo.VerticalScrollBar = AbVSB;
        Close.Window = Dlg;
        HeaderMemo.Window = Dlg;
        AbVSB.Window = Dlg;
        Loaded();
        Dlg.Loaded();
        MessageMemo.Loaded();
        Close.Loaded();
        HeaderMemo.Loaded();
        AbVSB.Loaded();
        Translator.Loaded();
        session.Resolve(this);
        Dlg.WindowManager = DialogHosts.General.Manager;
        Dlg.ImageLibraryTexture = DialogHosts.UIGraphics.MiscArt;
        MessageMemo.Font = DialogHosts.UIFonts.Age10;
        Close.ToolTip = DialogHosts.General.ToolTip;
        Close.Font = DialogHosts.UIFonts.Age10b;
        Close.ImageLibraryButton = DialogHosts.UIGraphics.CampaignTrans;
        HeaderMemo.Font = DialogHosts.UIFonts.Font15BlackBrown;
        HeaderMemo.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        AbVSB.ImageLibraryImageIncrement = DialogHosts.UIGraphics.GenericS;
        AbVSB.ImageLibraryIDecrement = DialogHosts.UIGraphics.GenericS;
        AbVSB.ImageLibraryTileBack = DialogHosts.UIGraphics.GenericS;
        AbVSB.ThumbStrip = DialogHosts.UIGraphics.GenericS;
    }

    private void Layout_DlgHide_0()
    {
        DlgHide();
    }

    private void Layout_CloseClick_1()
    {
        CloseClick();
    }
}
