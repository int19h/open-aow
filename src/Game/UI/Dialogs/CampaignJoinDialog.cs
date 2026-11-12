// Translated from AoWCampaignJoin.pas.
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;

using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

// stage, faction, leader, scroll delay, scroll position, text
public partial class CampaignJoinDialog : DataModule
{
    public CampaignJoinDialog(Component owner) : base(owner)
    { }

    public Panel CharGenPnl;
    public Panel LeaderTypePnl;
    public Button CustomBtn;
    public Button DefaultBtn;
    public Memo LeaderTypeMsg;
    public Panel DefaultFacePnl;
    public Panel CustomFacePnl;
    public UserDrawBox ScrollTextDB;
    public IvTranslator Translator;

    public byte Stage { get; set; }
    public int Faction { get; set; }
    public Leader Leader { get; private set; }

    private int _scrollDelay;
    private int _scrollPosition;

    public StringList ScrollText { get; private set; }

    // AoWCampaignJoin.pas:160
    public void CustomizeLeader()
    {
        LeaderSetupControl control = default;
        Stage = 2;
        CharGenPnl.Visible = false;
        control = new LeaderSetupControl(null);
        control.Leader.Assign(Leader, GameTables.GameEngine);
        control.OriginalLeader.Assign(Leader, GameTables.GameEngine);
        control.Mode = control.Mode + Set<LeaderSetupModeElement>.Of(LeaderSetupModeElement.Campaign);
        switch (Faction)
        {
            case 0:
                control.Items.Add(4);
                control.Items.Add(6);
                control.Items.Add(5);
                control.Items.Add(2);
                control.Items.Add(1);
                control.Items.Add(3);
                break;
            case 1:
                control.Items.Add(8);
                control.Items.Add(9);
                control.Items.Add(10);
                control.Items.Add(2);
                control.Items.Add(1);
                control.Items.Add(3);
                break;
        }

        DialogHosts.LeaderSetupWindow.Execute(control);
        control.OnDone = LeaderCustomized;
        control.Release();
        DialogHosts.LeaderSetupWindow.LeaderPnl.ImageLibraryTexture = DialogHosts.UIGraphics.FactionWin;
        DialogHosts.LeaderSetupWindow.LeaderPnl.ImageLibraryIndexTexture = Faction + 10;
    }

    // AoWCampaignJoin.pas:145
    public void LeaderCustomized(LeaderSetupControl sender)
    {
        Leader leader;
        var leaderSetupControl = sender;
        if (leaderSetupControl.Status == LeaderSetupControl.LeaderSetupStatus.Done)
        {
            leader = (Leader)leaderSetupControl.OriginalLeader.Copy(GameTables.GameEngine);
            DialogHosts.WorldMap.GetCampaign().TransferSettings.SetLeader(leader);
            leader.Release();
            FinishSelection();
        }
        else
        {
            ShowLeaderChoices();
        }
    }

    // AoWCampaignJoin.pas:189
    public void ResetCampaignJoin()
    {
        CharGenPnl.Visible = true;
        LeaderTypePnl.Visible = false;
        DialogHosts.TitleWindow.TitleWin.Visible = true;
        ScrollTextDB.Visible = true;
        _scrollDelay = 0;
        _scrollPosition = 0;
        ScrollText.Clear();
    }

    // AoWCampaignJoin.pas:200
    public void FinishSelection()
    {
        CharGenPnl.Visible = false;
        WorldMap.ShowCampaignMap(DialogHosts.WorldMap);
    }

    // AoWCampaignJoin.pas:137
    public void ShowLeaderChoices()
    {
        Stage = 1;
        LeaderTypePnl.Visible = true;
        ScrollTextDB.Visible = false;
        CharGenPnl.Visible = true;
    }

    // AoWCampaignJoin.pas:57
    public void CampaignJoinDlgCreate()
    {
        Leader = new Leader(null);
        ScrollText = new StringList();
    }

    // AoWCampaignJoin.pas:64
    public void CampaignJoinDlgDestroy()
    {
        Leader.Release();
        ScrollText?.Dispose();
    }

    // AoWCampaignJoin.pas:71
    public void CharGenPnlMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (Stage == 0)
        {
            ShowLeaderChoices();
        }
    }

    // AoWCampaignJoin.pas:77
    public void DefaultFacePnlDraw(LockedSurface lockedSurface)
    {
        Rect R;
        R = new Rect(DefaultFacePnl.WinLeft + 1, DefaultFacePnl.WinTop + 1, DefaultFacePnl.WinLeft + DefaultFacePnl.WinWidth - 2, DefaultFacePnl.WinTop + DefaultFacePnl.WinHeight - 2);
        Leader.ShowFace(R, lockedSurface);
    }

    // AoWCampaignJoin.pas:88
    public void CustomBtnClick()
    {
        CustomizeLeader();
    }

    // AoWCampaignJoin.pas:91
    public void DefaultBtnClick()
    {
        DialogHosts.WorldMap.GetCampaign().TransferSettings.SetLeader(Leader);
        FinishSelection();
    }

    // AoWCampaignJoin.pas:97
    public void ScrollTextDBUpdate()
    {
        if (Stage == 0)
        {
            var count = ScrollText.Count;
            if (_scrollPosition < count * 20 + ScrollTextDB.WinHeight)
            {
                _scrollDelay -= 1;
                if (_scrollDelay <= 0)
                {
                    _scrollPosition += 1;
                    ScrollTextDB.Drawn = false;
                    _scrollDelay = 3;
                }
            }
            else
            {
                ShowLeaderChoices();
            }
        }
    }

    // AoWCampaignJoin.pas:114
    public void ScrollTextDBDraw(LockedSurface lockedSurface)
    {
        int X;
        int Y;
        lockedSurface.ClipRectangle.Left = CharGenPnl.WinLeft + ScrollTextDB.WinLeft;
        lockedSurface.ClipRectangle.Right = lockedSurface.ClipRectangle.Left + ScrollTextDB.WinWidth;
        lockedSurface.ClipRectangle.Top = CharGenPnl.WinTop + ScrollTextDB.WinTop;
        lockedSurface.ClipRectangle.Bottom = lockedSurface.ClipRectangle.Top + ScrollTextDB.WinHeight;
        X = CharGenPnl.WinLeft + ScrollTextDB.WinLeft + ScrollTextDB.WinWidth / 2;
        Y = lockedSurface.ClipRectangle.Top + ScrollTextDB.WinHeight - _scrollPosition;
        for (int I = 0, last = ScrollText.Count - 1; I <= last; I++)
        {
            var scrollText = ScrollText[I];
            DialogHosts.UIFonts.Font15WhiteGrey.SetTextCentered(X, Y, scrollText, lockedSurface);
            Y += 20;
        }
    }

    // AoWCampaignJoin.pas:132
    public void DefaultFacePnlMouseDown(int sender, int button, int shift, int X, int Y)
    {
    }

    internal static void InitializeCampaignJoin(CampaignJoinDialog dialog)
    {
        dialog.CharGenPnl.Visible = true;
        dialog.LeaderTypePnl.Visible = false;
        DialogHosts.TitleWindow.TitleWin.Visible = true;
        dialog.ScrollTextDB.Visible = true;
        dialog._scrollDelay = 0;
        dialog._scrollPosition = 0;
        dialog.ScrollText.Clear();
    }

}
