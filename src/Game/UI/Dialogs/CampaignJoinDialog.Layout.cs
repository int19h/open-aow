// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class CampaignJoinDialog : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "CampaignJoinDlg";
        LayoutOnCreate = _ => Layout_CampaignJoinDlgCreate_0();
        LayoutOnDestroy = _ => Layout_CampaignJoinDlgDestroy_1();
        CharGenPnl = new Panel(this);
        session.BeginRead(CharGenPnl);
        CharGenPnl.Name = "CharGenPnl";
        CharGenPnl.OnMouseDown = (_, button, shift, x, y) => CharGenPnlMouseDown(button, shift, x, y);
        CharGenPnl.Visible = false;
        CharGenPnl.Priority = 5;
        CharGenPnl.WinLeft = 0;
        CharGenPnl.WinTop = 0;
        CharGenPnl.WinWidth = 640;
        CharGenPnl.Masked = false;
        CharGenPnl.Alignment.AlignWidth = AlignWidth.Center;
        CharGenPnl.Alignment.AlignHeight = AlignHeight.Center;
        CharGenPnl.Alignment.HeightPercent = 0;
        CharGenPnl.Alignment.WidthPercent = 0;
        CharGenPnl.Alignment.LeftOffsetPercent = 0;
        CharGenPnl.Alignment.RightOffsetPercent = 0;
        CharGenPnl.Alignment.TopOffsetPercent = 0;
        CharGenPnl.Alignment.BottomOffsetPercent = 0;
        CharGenPnl.Alignment.LeftOffset = 0;
        CharGenPnl.Alignment.RightOffset = 0;
        CharGenPnl.Alignment.TopOffset = 0;
        CharGenPnl.Alignment.BottomOffset = 0;
        CharGenPnl.WinHeight = 480;
        CharGenPnl.ImageLibraryIndexFrame = 0;
        CharGenPnl.ImageLibraryIndexTexture = 11;
        CharGenPnl.OwnerItem = 0;
        CharGenPnl.MinWidth = 640;
        CharGenPnl.MinHeight = 480;
        session.EndRead(CharGenPnl);
        LeaderTypePnl = new Panel(this);
        session.BeginRead(LeaderTypePnl);
        LeaderTypePnl.Name = "LeaderTypePnl";
        LeaderTypePnl.Visible = false;
        LeaderTypePnl.Priority = 5;
        LeaderTypePnl.WinLeft = 120;
        LeaderTypePnl.WinTop = 90;
        LeaderTypePnl.WinWidth = 400;
        LeaderTypePnl.Masked = false;
        LeaderTypePnl.Alignment.AlignWidth = AlignWidth.Center;
        LeaderTypePnl.Alignment.AlignHeight = AlignHeight.Center;
        LeaderTypePnl.Alignment.HeightPercent = 0;
        LeaderTypePnl.Alignment.WidthPercent = 0;
        LeaderTypePnl.Alignment.LeftOffsetPercent = 0;
        LeaderTypePnl.Alignment.RightOffsetPercent = 0;
        LeaderTypePnl.Alignment.TopOffsetPercent = 0;
        LeaderTypePnl.Alignment.BottomOffsetPercent = 0;
        LeaderTypePnl.Alignment.LeftOffset = 0;
        LeaderTypePnl.Alignment.RightOffset = 0;
        LeaderTypePnl.Alignment.TopOffset = 0;
        LeaderTypePnl.Alignment.BottomOffset = 0;
        LeaderTypePnl.WinHeight = 300;
        LeaderTypePnl.ImageLibraryIndexFrame = 32;
        LeaderTypePnl.ImageLibraryIndexTexture = 2;
        LeaderTypePnl.OwnerItem = 0;
        LeaderTypePnl.MinWidth = 0;
        LeaderTypePnl.MinHeight = 0;
        session.EndRead(LeaderTypePnl);
        CustomBtn = new Button(this);
        session.BeginRead(CustomBtn);
        CustomBtn.Name = "CustomBtn";
        CustomBtn.Priority = 250;
        CustomBtn.WinLeft = 210;
        CustomBtn.WinTop = 258;
        CustomBtn.CaptureInput = true;
        CustomBtn.Alignment.AlignWidth = AlignWidth.Left;
        CustomBtn.Alignment.AlignHeight = AlignHeight.Bottom;
        CustomBtn.Alignment.HeightPercent = 0;
        CustomBtn.Alignment.WidthPercent = 0;
        CustomBtn.Alignment.LeftOffsetPercent = 50;
        CustomBtn.Alignment.RightOffsetPercent = 32;
        CustomBtn.Alignment.TopOffsetPercent = 0;
        CustomBtn.Alignment.BottomOffsetPercent = 0;
        CustomBtn.Alignment.LeftOffset = 10;
        CustomBtn.Alignment.RightOffset = 0;
        CustomBtn.Alignment.TopOffset = 0;
        CustomBtn.Alignment.BottomOffset = 20;
        CustomBtn.Masked = true;
        CustomBtn.BorderHeight = 6;
        CustomBtn.BorderHeightSelected = 7;
        CustomBtn.ImageLibraryIndexDown = 46;
        CustomBtn.ImageLibraryIndexLit = 49;
        CustomBtn.ImageLibraryIndexUp = 43;
        CustomBtn.Text = "Custom";
        CustomBtn.TextColor = 0;
        CustomBtn.TextSelectedColor = 16777215;
        CustomBtn.WinWidth = 150;
        CustomBtn.OnClick = _ => Layout_CustomBtnClick_2();
        session.EndRead(CustomBtn);
        DefaultBtn = new Button(this);
        session.BeginRead(DefaultBtn);
        DefaultBtn.Name = "DefaultBtn";
        DefaultBtn.Priority = 250;
        DefaultBtn.WinLeft = 40;
        DefaultBtn.WinTop = 258;
        DefaultBtn.CaptureInput = true;
        DefaultBtn.Alignment.AlignWidth = AlignWidth.Right;
        DefaultBtn.Alignment.AlignHeight = AlignHeight.Bottom;
        DefaultBtn.Alignment.HeightPercent = 0;
        DefaultBtn.Alignment.WidthPercent = 0;
        DefaultBtn.Alignment.LeftOffsetPercent = 10;
        DefaultBtn.Alignment.RightOffsetPercent = 50;
        DefaultBtn.Alignment.TopOffsetPercent = 0;
        DefaultBtn.Alignment.BottomOffsetPercent = 0;
        DefaultBtn.Alignment.LeftOffset = 0;
        DefaultBtn.Alignment.RightOffset = 10;
        DefaultBtn.Alignment.TopOffset = 0;
        DefaultBtn.Alignment.BottomOffset = 20;
        DefaultBtn.Masked = true;
        DefaultBtn.BorderHeight = 6;
        DefaultBtn.BorderHeightSelected = 7;
        DefaultBtn.ImageLibraryIndexDown = 46;
        DefaultBtn.ImageLibraryIndexLit = 49;
        DefaultBtn.ImageLibraryIndexUp = 43;
        DefaultBtn.Text = "Default";
        DefaultBtn.TextColor = 0;
        DefaultBtn.TextSelectedColor = 16777215;
        DefaultBtn.WinWidth = 150;
        DefaultBtn.OnClick = _ => Layout_DefaultBtnClick_3();
        session.EndRead(DefaultBtn);
        LeaderTypeMsg = new Memo(this);
        session.BeginRead(LeaderTypeMsg);
        LeaderTypeMsg.Name = "LeaderTypeMsg";
        LeaderTypeMsg.Priority = 0;
        LeaderTypeMsg.WinLeft = 10;
        LeaderTypeMsg.WinTop = 38;
        LeaderTypeMsg.CaptureInput = false;
        LeaderTypeMsg.Alignment.AlignWidth = AlignWidth.Both;
        LeaderTypeMsg.Alignment.AlignHeight = AlignHeight.Top;
        LeaderTypeMsg.Alignment.HeightPercent = 0;
        LeaderTypeMsg.Alignment.WidthPercent = 0;
        LeaderTypeMsg.Alignment.LeftOffsetPercent = 0;
        LeaderTypeMsg.Alignment.RightOffsetPercent = 0;
        LeaderTypeMsg.Alignment.TopOffsetPercent = 0;
        LeaderTypeMsg.Alignment.BottomOffsetPercent = 0;
        LeaderTypeMsg.Alignment.LeftOffset = 10;
        LeaderTypeMsg.Alignment.RightOffset = 10;
        LeaderTypeMsg.Alignment.TopOffset = 38;
        LeaderTypeMsg.Alignment.BottomOffset = 0;
        LeaderTypeMsg.Masked = true;
        LeaderTypeMsg.WinWidth = 380;
        LeaderTypeMsg.WinHeight = 65;
        LeaderTypeMsg.LayoutItems = new LayoutStrings()
        {
            "Do you wish to start with a default leader or custom made character?"
        };
        LeaderTypeMsg.BorderWidth = 20;
        LeaderTypeMsg.BorderHeight = 0;
        LeaderTypeMsg.TextColor = 16777215;
        LeaderTypeMsg.CursorChar = 124;
        LeaderTypeMsg.BlinkDelay = 0;
        LeaderTypeMsg.Editable = false;
        LeaderTypeMsg.ImageLibraryIndexFrame = 0;
        LeaderTypeMsg.ImageLibraryIndexTexture = 0;
        LeaderTypeMsg.ItemHeight = 20;
        LeaderTypeMsg.ColorChangeChar = 0;
        LeaderTypeMsg.TextAlign = TextAlignment.Center;
        session.EndRead(LeaderTypeMsg);
        DefaultFacePnl = new Panel(this);
        session.BeginRead(DefaultFacePnl);
        DefaultFacePnl.Name = "DefaultFacePnl";
        DefaultFacePnl.OnMouseDown = (_, button, shift, x, y) => Layout_DefaultFacePnlMouseDown_4(button, shift, x, y);
        DefaultFacePnl.OnDraw = (_, lockedSurface) => DefaultFacePnlDraw(lockedSurface);
        DefaultFacePnl.Priority = 0;
        DefaultFacePnl.WinLeft = 62;
        DefaultFacePnl.WinTop = 103;
        DefaultFacePnl.WinWidth = 103;
        DefaultFacePnl.Masked = true;
        DefaultFacePnl.Alignment.AlignWidth = AlignWidth.Left;
        DefaultFacePnl.Alignment.AlignHeight = AlignHeight.Top;
        DefaultFacePnl.Alignment.HeightPercent = 0;
        DefaultFacePnl.Alignment.WidthPercent = 0;
        DefaultFacePnl.Alignment.LeftOffsetPercent = 0;
        DefaultFacePnl.Alignment.RightOffsetPercent = 0;
        DefaultFacePnl.Alignment.TopOffsetPercent = 0;
        DefaultFacePnl.Alignment.BottomOffsetPercent = 0;
        DefaultFacePnl.Alignment.LeftOffset = 62;
        DefaultFacePnl.Alignment.RightOffset = 0;
        DefaultFacePnl.Alignment.TopOffset = 103;
        DefaultFacePnl.Alignment.BottomOffset = 0;
        DefaultFacePnl.WinHeight = 128;
        DefaultFacePnl.ImageLibraryIndexFrame = 40;
        DefaultFacePnl.ImageLibraryIndexTexture = 0;
        DefaultFacePnl.OwnerItem = 0;
        DefaultFacePnl.MinWidth = 0;
        DefaultFacePnl.MinHeight = 0;
        session.EndRead(DefaultFacePnl);
        CustomFacePnl = new Panel(this);
        session.BeginRead(CustomFacePnl);
        CustomFacePnl.Name = "CustomFacePnl";
        CustomFacePnl.Priority = 0;
        CustomFacePnl.WinLeft = 235;
        CustomFacePnl.WinTop = 103;
        CustomFacePnl.WinWidth = 103;
        CustomFacePnl.Masked = true;
        CustomFacePnl.Alignment.AlignWidth = AlignWidth.Right;
        CustomFacePnl.Alignment.AlignHeight = AlignHeight.Top;
        CustomFacePnl.Alignment.HeightPercent = 0;
        CustomFacePnl.Alignment.WidthPercent = 0;
        CustomFacePnl.Alignment.LeftOffsetPercent = 0;
        CustomFacePnl.Alignment.RightOffsetPercent = 0;
        CustomFacePnl.Alignment.TopOffsetPercent = 0;
        CustomFacePnl.Alignment.BottomOffsetPercent = 0;
        CustomFacePnl.Alignment.LeftOffset = 62;
        CustomFacePnl.Alignment.RightOffset = 62;
        CustomFacePnl.Alignment.TopOffset = 103;
        CustomFacePnl.Alignment.BottomOffset = 0;
        CustomFacePnl.WinHeight = 128;
        CustomFacePnl.ImageLibraryIndexFrame = 40;
        CustomFacePnl.ImageLibraryIndexTexture = 12;
        CustomFacePnl.OwnerItem = 0;
        CustomFacePnl.MinWidth = 0;
        CustomFacePnl.MinHeight = 0;
        session.EndRead(CustomFacePnl);
        ScrollTextDB = new UserDrawBox(this);
        session.BeginRead(ScrollTextDB);
        ScrollTextDB.Name = "ScrollTextDB";
        ScrollTextDB.OnDraw = (_, lockedSurface) => ScrollTextDBDraw(lockedSurface);
        ScrollTextDB.OnUpdate = _ => Layout_ScrollTextDBUpdate_5();
        ScrollTextDB.Priority = 0;
        ScrollTextDB.WinLeft = 150;
        ScrollTextDB.WinTop = 380;
        ScrollTextDB.CaptureInput = true;
        ScrollTextDB.Alignment.AlignWidth = AlignWidth.Both;
        ScrollTextDB.Alignment.AlignHeight = AlignHeight.Bottom;
        ScrollTextDB.Alignment.HeightPercent = 0;
        ScrollTextDB.Alignment.WidthPercent = 0;
        ScrollTextDB.Alignment.LeftOffsetPercent = 0;
        ScrollTextDB.Alignment.RightOffsetPercent = 0;
        ScrollTextDB.Alignment.TopOffsetPercent = 0;
        ScrollTextDB.Alignment.BottomOffsetPercent = 0;
        ScrollTextDB.Alignment.LeftOffset = 150;
        ScrollTextDB.Alignment.RightOffset = 150;
        ScrollTextDB.Alignment.TopOffset = 0;
        ScrollTextDB.Alignment.BottomOffset = 0;
        ScrollTextDB.Masked = true;
        ScrollTextDB.WinWidth = 340;
        ScrollTextDB.WinHeight = 100;
        session.EndRead(ScrollTextDB);
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
        LeaderTypePnl.Window = CharGenPnl;
        CustomBtn.Window = LeaderTypePnl;
        DefaultBtn.Window = LeaderTypePnl;
        LeaderTypeMsg.Window = LeaderTypePnl;
        DefaultFacePnl.Window = LeaderTypePnl;
        CustomFacePnl.Window = LeaderTypePnl;
        ScrollTextDB.Window = CharGenPnl;
        Loaded();
        CharGenPnl.Loaded();
        LeaderTypePnl.Loaded();
        CustomBtn.Loaded();
        DefaultBtn.Loaded();
        LeaderTypeMsg.Loaded();
        DefaultFacePnl.Loaded();
        CustomFacePnl.Loaded();
        ScrollTextDB.Loaded();
        Translator.Loaded();
        session.Resolve(this);
        CharGenPnl.Window = DialogHosts.TitleWindow.TitleWin;
        CharGenPnl.ImageLibraryTexture = DialogHosts.UIGraphics.FactionWin;
        LeaderTypePnl.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        LeaderTypePnl.ImageLibraryTexture = DialogHosts.UIGraphics.GenericT;
        CustomBtn.Font = DialogHosts.UIFonts.Age10;
        CustomBtn.ImageLibraryButton = DialogHosts.UIGraphics.GenericB;
        DefaultBtn.Font = DialogHosts.UIFonts.Age10;
        DefaultBtn.ImageLibraryButton = DialogHosts.UIGraphics.GenericB;
        LeaderTypeMsg.Font = DialogHosts.UIFonts.Font15WhiteGrey;
        DefaultFacePnl.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        DefaultFacePnl.ImageLibraryTexture = DialogHosts.UIGraphics.GenericT;
        CustomFacePnl.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        CustomFacePnl.ImageLibraryTexture = DialogHosts.UIGraphics.FactionWin;
        if (session.FireCreateEvents)
            LayoutOnCreate?.Invoke(this);
    }

    private void Layout_CampaignJoinDlgCreate_0()
    {
        CampaignJoinDlgCreate();
    }

    private void Layout_CampaignJoinDlgDestroy_1()
    {
        CampaignJoinDlgDestroy();
    }

    private void Layout_CustomBtnClick_2()
    {
        CustomBtnClick();
    }

    private void Layout_DefaultBtnClick_3()
    {
        DefaultBtnClick();
    }

    private void Layout_DefaultFacePnlMouseDown_4(MouseButton p1, Set<ShiftState> p2, int p3, int p4)
    {
        DefaultFacePnlMouseDown(default(int), default(int), default(int), default(int), default(int));
    }

    private void Layout_ScrollTextDBUpdate_5()
    {
        ScrollTextDBUpdate();
    }
}
