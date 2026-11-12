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
        Button customBtn = new Button(this);
        session.BeginRead(customBtn);
        customBtn.Name = "CustomBtn";
        customBtn.Priority = 250;
        customBtn.WinLeft = 210;
        customBtn.WinTop = 258;
        customBtn.CaptureInput = true;
        customBtn.Alignment.AlignWidth = AlignWidth.Left;
        customBtn.Alignment.AlignHeight = AlignHeight.Bottom;
        customBtn.Alignment.HeightPercent = 0;
        customBtn.Alignment.WidthPercent = 0;
        customBtn.Alignment.LeftOffsetPercent = 50;
        customBtn.Alignment.RightOffsetPercent = 32;
        customBtn.Alignment.TopOffsetPercent = 0;
        customBtn.Alignment.BottomOffsetPercent = 0;
        customBtn.Alignment.LeftOffset = 10;
        customBtn.Alignment.RightOffset = 0;
        customBtn.Alignment.TopOffset = 0;
        customBtn.Alignment.BottomOffset = 20;
        customBtn.Masked = true;
        customBtn.BorderHeight = 6;
        customBtn.BorderHeightSelected = 7;
        customBtn.ImageLibraryIndexDown = 46;
        customBtn.ImageLibraryIndexLit = 49;
        customBtn.ImageLibraryIndexUp = 43;
        customBtn.Text = "Custom";
        customBtn.TextColor = 0;
        customBtn.TextSelectedColor = 16777215;
        customBtn.WinWidth = 150;
        customBtn.OnClick = _ => Layout_CustomBtnClick_2();
        session.EndRead(customBtn);
        Button defaultBtn = new Button(this);
        session.BeginRead(defaultBtn);
        defaultBtn.Name = "DefaultBtn";
        defaultBtn.Priority = 250;
        defaultBtn.WinLeft = 40;
        defaultBtn.WinTop = 258;
        defaultBtn.CaptureInput = true;
        defaultBtn.Alignment.AlignWidth = AlignWidth.Right;
        defaultBtn.Alignment.AlignHeight = AlignHeight.Bottom;
        defaultBtn.Alignment.HeightPercent = 0;
        defaultBtn.Alignment.WidthPercent = 0;
        defaultBtn.Alignment.LeftOffsetPercent = 10;
        defaultBtn.Alignment.RightOffsetPercent = 50;
        defaultBtn.Alignment.TopOffsetPercent = 0;
        defaultBtn.Alignment.BottomOffsetPercent = 0;
        defaultBtn.Alignment.LeftOffset = 0;
        defaultBtn.Alignment.RightOffset = 10;
        defaultBtn.Alignment.TopOffset = 0;
        defaultBtn.Alignment.BottomOffset = 20;
        defaultBtn.Masked = true;
        defaultBtn.BorderHeight = 6;
        defaultBtn.BorderHeightSelected = 7;
        defaultBtn.ImageLibraryIndexDown = 46;
        defaultBtn.ImageLibraryIndexLit = 49;
        defaultBtn.ImageLibraryIndexUp = 43;
        defaultBtn.Text = "Default";
        defaultBtn.TextColor = 0;
        defaultBtn.TextSelectedColor = 16777215;
        defaultBtn.WinWidth = 150;
        defaultBtn.OnClick = _ => Layout_DefaultBtnClick_3();
        session.EndRead(defaultBtn);
        Memo leaderTypeMsg = new Memo(this);
        session.BeginRead(leaderTypeMsg);
        leaderTypeMsg.Name = "LeaderTypeMsg";
        leaderTypeMsg.Priority = 0;
        leaderTypeMsg.WinLeft = 10;
        leaderTypeMsg.WinTop = 38;
        leaderTypeMsg.CaptureInput = false;
        leaderTypeMsg.Alignment.AlignWidth = AlignWidth.Both;
        leaderTypeMsg.Alignment.AlignHeight = AlignHeight.Top;
        leaderTypeMsg.Alignment.HeightPercent = 0;
        leaderTypeMsg.Alignment.WidthPercent = 0;
        leaderTypeMsg.Alignment.LeftOffsetPercent = 0;
        leaderTypeMsg.Alignment.RightOffsetPercent = 0;
        leaderTypeMsg.Alignment.TopOffsetPercent = 0;
        leaderTypeMsg.Alignment.BottomOffsetPercent = 0;
        leaderTypeMsg.Alignment.LeftOffset = 10;
        leaderTypeMsg.Alignment.RightOffset = 10;
        leaderTypeMsg.Alignment.TopOffset = 38;
        leaderTypeMsg.Alignment.BottomOffset = 0;
        leaderTypeMsg.Masked = true;
        leaderTypeMsg.WinWidth = 380;
        leaderTypeMsg.WinHeight = 65;
        leaderTypeMsg.LayoutItems = new LayoutStrings()
        {
            "Do you wish to start with a default leader or custom made character?"
        };
        leaderTypeMsg.BorderWidth = 20;
        leaderTypeMsg.BorderHeight = 0;
        leaderTypeMsg.TextColor = 16777215;
        leaderTypeMsg.CursorChar = 124;
        leaderTypeMsg.BlinkDelay = 0;
        leaderTypeMsg.Editable = false;
        leaderTypeMsg.ImageLibraryIndexFrame = 0;
        leaderTypeMsg.ImageLibraryIndexTexture = 0;
        leaderTypeMsg.ItemHeight = 20;
        leaderTypeMsg.ColorChangeChar = 0;
        leaderTypeMsg.TextAlign = TextAlignment.Center;
        session.EndRead(leaderTypeMsg);
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
        Panel customFacePnl = new Panel(this);
        session.BeginRead(customFacePnl);
        customFacePnl.Name = "CustomFacePnl";
        customFacePnl.Priority = 0;
        customFacePnl.WinLeft = 235;
        customFacePnl.WinTop = 103;
        customFacePnl.WinWidth = 103;
        customFacePnl.Masked = true;
        customFacePnl.Alignment.AlignWidth = AlignWidth.Right;
        customFacePnl.Alignment.AlignHeight = AlignHeight.Top;
        customFacePnl.Alignment.HeightPercent = 0;
        customFacePnl.Alignment.WidthPercent = 0;
        customFacePnl.Alignment.LeftOffsetPercent = 0;
        customFacePnl.Alignment.RightOffsetPercent = 0;
        customFacePnl.Alignment.TopOffsetPercent = 0;
        customFacePnl.Alignment.BottomOffsetPercent = 0;
        customFacePnl.Alignment.LeftOffset = 62;
        customFacePnl.Alignment.RightOffset = 62;
        customFacePnl.Alignment.TopOffset = 103;
        customFacePnl.Alignment.BottomOffset = 0;
        customFacePnl.WinHeight = 128;
        customFacePnl.ImageLibraryIndexFrame = 40;
        customFacePnl.ImageLibraryIndexTexture = 12;
        customFacePnl.OwnerItem = 0;
        customFacePnl.MinWidth = 0;
        customFacePnl.MinHeight = 0;
        session.EndRead(customFacePnl);
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
        LayoutIvTranslator translator = new LayoutIvTranslator(this);
        session.BeginRead(translator);
        translator.Name = "Translator";
        translator.DictionaryName = "AoW";
        translator.TargetsData.Add(new TranslationTarget("", "Items", 0));
        translator.TargetsData.Add(new TranslationTarget("", "Text", 0));
        translator.TargetsData.Add(new TranslationTarget("", "ToolTipText", 0));
        session.EndRead(translator);
        session.EndRead(this);
        LeaderTypePnl.Window = CharGenPnl;
        customBtn.Window = LeaderTypePnl;
        defaultBtn.Window = LeaderTypePnl;
        leaderTypeMsg.Window = LeaderTypePnl;
        DefaultFacePnl.Window = LeaderTypePnl;
        customFacePnl.Window = LeaderTypePnl;
        ScrollTextDB.Window = CharGenPnl;
        Loaded();
        CharGenPnl.Loaded();
        LeaderTypePnl.Loaded();
        customBtn.Loaded();
        defaultBtn.Loaded();
        leaderTypeMsg.Loaded();
        DefaultFacePnl.Loaded();
        customFacePnl.Loaded();
        ScrollTextDB.Loaded();
        translator.Loaded();
        session.Resolve(this);
        CharGenPnl.Window = DialogHosts.TitleWindow.TitleWin;
        CharGenPnl.ImageLibraryTexture = DialogHosts.UIGraphics.FactionWin;
        LeaderTypePnl.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        LeaderTypePnl.ImageLibraryTexture = DialogHosts.UIGraphics.GenericT;
        customBtn.Font = DialogHosts.UIFonts.Age10;
        customBtn.ImageLibraryButton = DialogHosts.UIGraphics.GenericB;
        defaultBtn.Font = DialogHosts.UIFonts.Age10;
        defaultBtn.ImageLibraryButton = DialogHosts.UIGraphics.GenericB;
        leaderTypeMsg.Font = DialogHosts.UIFonts.Font15WhiteGrey;
        DefaultFacePnl.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        DefaultFacePnl.ImageLibraryTexture = DialogHosts.UIGraphics.GenericT;
        customFacePnl.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        customFacePnl.ImageLibraryTexture = DialogHosts.UIGraphics.FactionWin;
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
