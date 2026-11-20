#nullable disable
// Translated from AoWCampaignJoin.pas.
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// stage, faction, leader, scroll delay, scroll position, text
public partial class CampaignJoinDialog(UIContext ui) : UIModule(ui)
{
    public Panel CharacterGenerationPanel { get; private set; }
    public Panel LeaderTypePanel { get; private set; }
    public Panel DefaultFacePanel { get; private set; }
    public UserDrawBox ScrollTextView { get; private set; }

    public byte Stage { get; set; }
    public int Faction { get; set; }
    public Leader Leader { get; private set; }

    private int _scrollDelay;
    private int _scrollPosition;

    public DelphiStringList ScrollText { get; private set; }

    // AoWCampaignJoin.pas:160
    public void CustomizeLeader()
    {
        Stage = 2;
        CharacterGenerationPanel.Visible = false;
        LeaderSetupControl control = new(null) { Holder = UI.Engine };
        control.Leader.Assign(Leader, UI.Engine);
        control.OriginalLeader.Assign(Leader, UI.Engine);
        control.Mode += Set<LeaderSetupModeElement>.Of(LeaderSetupModeElement.Campaign);
        switch (Faction)
        {
            case 0:
                _ = control.Items.Add(4);
                _ = control.Items.Add(6);
                _ = control.Items.Add(5);
                _ = control.Items.Add(2);
                _ = control.Items.Add(1);
                _ = control.Items.Add(3);
                break;
            case 1:
                _ = control.Items.Add(8);
                _ = control.Items.Add(9);
                _ = control.Items.Add(10);
                _ = control.Items.Add(2);
                _ = control.Items.Add(1);
                _ = control.Items.Add(3);
                break;
            default:
                break;
        }

        UI.LeaderSetupWindow.Execute(control);
        control.OnDone += LeaderCustomized;
        _ = control.Release();
        UI.LeaderSetupWindow.LeaderPanel.ImageLibraryTexture = UI.Graphics.FactionWindow;
        UI.LeaderSetupWindow.LeaderPanel.ImageLibraryIndexTexture = Faction + 10;
    }

    // AoWCampaignJoin.pas:145
    public void LeaderCustomized(LeaderSetupControl sender)
    {
        LeaderSetupControl leaderSetupControl = sender;
        if (leaderSetupControl.Status == LeaderSetupControl.LeaderSetupStatus.Done)
        {
            Leader leader = leaderSetupControl.OriginalLeader.Copy<Leader>(UI.Engine);
            UI.WorldMap.GetCampaign().TransferSettings.SetLeader(leader);
            _ = leader.Release();
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
        Leader = new Leader(null) { Holder = UI.Engine };
        ScrollText = new DelphiStringList();
    }

    // AoWCampaignJoin.pas:64
    public void CampaignJoinDialogDestroy()
    {
        _ = Leader.Release();
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
        Rect r = new(DefaultFacePanel.AbsoluteLeft + 1, DefaultFacePanel.AbsoluteTop + 1, DefaultFacePanel.AbsoluteLeft + DefaultFacePanel.WinWidth - 2, DefaultFacePanel.AbsoluteTop + DefaultFacePanel.WinHeight - 2);
        Leader.ShowFace(r, lockedSurface);
    }

    // AoWCampaignJoin.pas:88
    public void CustomBtnClick() => CustomizeLeader();

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
            int count = ScrollText.Count;
            if (_scrollPosition < (count * 20) + ScrollTextView.WinHeight)
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
        Rect clipRectangle = lockedSurface.ClipRectangle;
        clipRectangle.Left = CharacterGenerationPanel.AbsoluteLeft + ScrollTextView.WinLeft;
        lockedSurface.ClipRectangle = clipRectangle;
        Rect clipRectangle2 = lockedSurface.ClipRectangle;
        clipRectangle2.Right = clipRectangle2.Left + ScrollTextView.WinWidth;
        lockedSurface.ClipRectangle = clipRectangle2;
        Rect clipRectangle3 = lockedSurface.ClipRectangle;
        clipRectangle3.Top = CharacterGenerationPanel.AbsoluteTop + ScrollTextView.WinTop;
        lockedSurface.ClipRectangle = clipRectangle3;
        Rect clipRectangle4 = lockedSurface.ClipRectangle;
        clipRectangle4.Bottom = clipRectangle4.Top + ScrollTextView.WinHeight;
        lockedSurface.ClipRectangle = clipRectangle4;
        int x = CharacterGenerationPanel.AbsoluteLeft + ScrollTextView.WinLeft + (ScrollTextView.WinWidth / 2);
        int y = lockedSurface.ClipRectangle.Top + ScrollTextView.WinHeight - _scrollPosition;
        for (int i = 0, last = ScrollText.Count - 1; i <= last; i++)
        {
            string scrollText = ScrollText[i];
            UI.Fonts.Age15WhiteOnGrey.SetTextCentered(x, y, scrollText, lockedSurface);
            y += 20;
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
