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
public partial class CampaignJoinDialog : UIModule
{
    public CampaignJoinDialog(UIContext ui) : base(ui)
    { }

    public Panel CharacterGenerationPanel;
    public Panel LeaderTypePanel;
    public Panel DefaultFacePanel;
    public UserDrawBox ScrollTextView;

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
        CharacterGenerationPanel.Visible = false;
        control = new LeaderSetupControl(null);
        control.Leader.Assign(Leader, UI.Engine);
        control.OriginalLeader.Assign(Leader, UI.Engine);
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

        UI.LeaderSetupWindow.Execute(control);
        control.OnDone += LeaderCustomized;
        control.Release();
        UI.LeaderSetupWindow.LeaderPanel.ImageLibraryTexture = UI.Graphics.FactionWindow;
        UI.LeaderSetupWindow.LeaderPanel.ImageLibraryIndexTexture = Faction + 10;
    }

    // AoWCampaignJoin.pas:145
    public void LeaderCustomized(LeaderSetupControl sender)
    {
        Leader leader;
        var leaderSetupControl = sender;
        if (leaderSetupControl.Status == LeaderSetupControl.LeaderSetupStatus.Done)
        {
            leader = (Leader)leaderSetupControl.OriginalLeader.Copy(UI.Engine);
            UI.WorldMap.GetCampaign().TransferSettings.SetLeader(leader);
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
        CharacterGenerationPanel.Visible = true;
        LeaderTypePanel.Visible = false;
        UI.TitleWindow.MainWindow.Visible = true;
        ScrollTextView.Visible = true;
        _scrollDelay = 0;
        _scrollPosition = 0;
        ScrollText.Clear();
    }

    // AoWCampaignJoin.pas:200
    public void FinishSelection()
    {
        CharacterGenerationPanel.Visible = false;
        WorldMap.ShowCampaignMap(UI.WorldMap);
    }

    // AoWCampaignJoin.pas:137
    public void ShowLeaderChoices()
    {
        Stage = 1;
        LeaderTypePanel.Visible = true;
        ScrollTextView.Visible = false;
        CharacterGenerationPanel.Visible = true;
    }

    // AoWCampaignJoin.pas:57
    public void CampaignJoinDialogCreate()
    {
        Leader = new Leader(null);
        ScrollText = new StringList();
    }

    // AoWCampaignJoin.pas:64
    public void CampaignJoinDialogDestroy()
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
        R = new Rect(DefaultFacePanel.WinLeft + 1, DefaultFacePanel.WinTop + 1, DefaultFacePanel.WinLeft + DefaultFacePanel.WinWidth - 2, DefaultFacePanel.WinTop + DefaultFacePanel.WinHeight - 2);
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
        UI.WorldMap.GetCampaign().TransferSettings.SetLeader(Leader);
        FinishSelection();
    }

    // AoWCampaignJoin.pas:97
    public void ScrollTextDBUpdate()
    {
        if (Stage == 0)
        {
            var count = ScrollText.Count;
            if (_scrollPosition < count * 20 + ScrollTextView.WinHeight)
            {
                _scrollDelay -= 1;
                if (_scrollDelay <= 0)
                {
                    _scrollPosition += 1;
                    ScrollTextView.Drawn = false;
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
        lockedSurface.ClipRectangle.Left = CharacterGenerationPanel.WinLeft + ScrollTextView.WinLeft;
        lockedSurface.ClipRectangle.Right = lockedSurface.ClipRectangle.Left + ScrollTextView.WinWidth;
        lockedSurface.ClipRectangle.Top = CharacterGenerationPanel.WinTop + ScrollTextView.WinTop;
        lockedSurface.ClipRectangle.Bottom = lockedSurface.ClipRectangle.Top + ScrollTextView.WinHeight;
        X = CharacterGenerationPanel.WinLeft + ScrollTextView.WinLeft + ScrollTextView.WinWidth / 2;
        Y = lockedSurface.ClipRectangle.Top + ScrollTextView.WinHeight - _scrollPosition;
        for (int I = 0, last = ScrollText.Count - 1; I <= last; I++)
        {
            var scrollText = ScrollText[I];
            UI.Fonts.Age15WhiteOnGrey.SetTextCentered(X, Y, scrollText, lockedSurface);
            Y += 20;
        }
    }

    // AoWCampaignJoin.pas:132
    public void DefaultFacePnlMouseDown(int sender, int button, int shift, int X, int Y)
    {
    }

    internal static void InitializeCampaignJoin(CampaignJoinDialog dialog)
    {
        dialog.CharacterGenerationPanel.Visible = true;
        dialog.LeaderTypePanel.Visible = false;
        dialog.UI.TitleWindow.MainWindow.Visible = true;
        dialog.ScrollTextView.Visible = true;
        dialog._scrollDelay = 0;
        dialog._scrollPosition = 0;
        dialog.ScrollText.Clear();
    }

}
