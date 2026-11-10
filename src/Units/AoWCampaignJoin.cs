// Translated from AoWCampaignJoin.pas.
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Saves;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
// Translated from AoWCampaignJoin.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// stage, faction, leader, scroll delay, scroll position, text
public partial class CampaignJoinDlg : DataModule
{
    public CampaignJoinDlg(Component owner) : base(owner)
    { }

    public AOWPanel CharGenPnl;
    public AOWPanel LeaderTypePnl;
    public AOWButton CustomBtn;
    public AOWButton DefaultBtn;
    public AOWMemo LeaderTypeMsg;
    public AOWPanel DefaultFacePnl;
    public AOWPanel CustomFacePnl;
    public TaowUserDrawBox ScrollTextDB;
    public IvTranslator Translator;
    public byte FStage;
    public int FFaction;
    public Leader FLeader;
    public int FScrollDelay;
    public int FScrollPosition;
    public StringList FScrollText;

    // AoWCampaignJoin.pas:160
    public void CustomizeLeader()
    {
        LeaderSetupControl Control = default;
        FStage = 2;
        CharGenPnl.Visible = false;
        Control = new LeaderSetupControl(null);
        Control.FLeader.Assign(FLeader, GameTables.AoWEngine);
        Control.FOriginalLeader.Assign(FLeader, GameTables.AoWEngine);
        Control.FMode = Control.FMode + Set<LeaderSetupModeElement>.Of(LeaderSetupModeElement.Campaign);
        switch (FFaction)
        {
            case 0:
                Control.FItems.Add(4);
                Control.FItems.Add(6);
                Control.FItems.Add(5);
                Control.FItems.Add(2);
                Control.FItems.Add(1);
                Control.FItems.Add(3);
                break;
            case 1:
                Control.FItems.Add(8);
                Control.FItems.Add(9);
                Control.FItems.Add(10);
                Control.FItems.Add(2);
                Control.FItems.Add(1);
                Control.FItems.Add(3);
                break;
        }

        Aow.Game.Ui.Dialogs.DialogHosts.LeaderSetupWin.Execute(Control);
        Control.FOnDone = LeaderCustomized;
        Control.Release();
        DialogHosts.LeaderSetupWin.LeaderPnl.ILTexture = DialogHosts.IntGfxMod.FactionWin;
        DialogHosts.LeaderSetupWin.LeaderPnl.ILIndexTexture = FFaction + 10;
    }

    // AoWCampaignJoin.pas:145
    public void LeaderCustomized(LeaderSetupControl Sender)
    {
        Leader Leader;
        var leaderSetupControl = Sender;
        if (leaderSetupControl.FStatus == LeaderSetupControl.LeaderSetupStatus.Done)
        {
            Leader = (Leader)leaderSetupControl.FOriginalLeader.Copy(GameTables.AoWEngine);
            Aow.Game.Ui.Dialogs.DialogHosts.WorldMap.GetCampaign().FTransferSettings.SetLeader(Leader);
            Leader.Release();
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
        FScrollDelay = 0;
        FScrollPosition = 0;
        FScrollText.Clear();
    }

    // AoWCampaignJoin.pas:200
    public void FinishSelection()
    {
        CharGenPnl.Visible = false;
        Aow.Game.Ui.Dialogs.WorldMap.ShowCampaignMap(DialogHosts.WorldMap);
    }

    // AoWCampaignJoin.pas:137
    public void ShowLeaderChoices()
    {
        FStage = 1;
        LeaderTypePnl.Visible = true;
        ScrollTextDB.Visible = false;
        CharGenPnl.Visible = true;
    }

    // AoWCampaignJoin.pas:57
    public void CampaignJoinDlgCreate()
    {
        FLeader = new Leader(null);
        FScrollText = new StringList();
    }

    // AoWCampaignJoin.pas:64
    public void CampaignJoinDlgDestroy()
    {
        FLeader.Release();
        FScrollText?.Dispose();
    }

    // AoWCampaignJoin.pas:71
    public void CharGenPnlMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (FStage == 0)
        {
            ShowLeaderChoices();
        }
    }

    // AoWCampaignJoin.pas:77
    public void DefaultFacePnlDraw(SurfaceDesc SD)
    {
        Rect R;
        R = new Rect(DefaultFacePnl.WinLeft + 1, DefaultFacePnl.WinTop + 1, DefaultFacePnl.WinLeft + DefaultFacePnl.WinWidth - 2, DefaultFacePnl.WinTop + DefaultFacePnl.WinHeight - 2);
        FLeader.ShowFace(R, SD);
    }

    // AoWCampaignJoin.pas:88
    public void CustomBtnClick()
    {
        CustomizeLeader();
    }

    // AoWCampaignJoin.pas:91
    public void DefaultBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.WorldMap.GetCampaign().FTransferSettings.SetLeader(FLeader);
        FinishSelection();
    }

    // AoWCampaignJoin.pas:97
    public void ScrollTextDBUpdate()
    {
        if (FStage == 0)
        {
            var count = FScrollText.Count;
            if (FScrollPosition < count * 20 + ScrollTextDB.WinHeight)
            {
                FScrollDelay -= 1;
                if (FScrollDelay <= 0)
                {
                    FScrollPosition += 1;
                    ScrollTextDB.FDrawn = false;
                    FScrollDelay = 3;
                }
            }
            else
            {
                ShowLeaderChoices();
            }
        }
    }

    // AoWCampaignJoin.pas:114
    public void ScrollTextDBDraw(SurfaceDesc SD)
    {
        int X;
        int Y;
        SD.ClipRect.Left = CharGenPnl.WinLeft + ScrollTextDB.WinLeft;
        SD.ClipRect.Right = SD.ClipRect.Left + ScrollTextDB.WinWidth;
        SD.ClipRect.Top = CharGenPnl.WinTop + ScrollTextDB.WinTop;
        SD.ClipRect.Bottom = SD.ClipRect.Top + ScrollTextDB.WinHeight;
        X = CharGenPnl.WinLeft + ScrollTextDB.WinLeft + ScrollTextDB.WinWidth / 2;
        Y = SD.ClipRect.Top + ScrollTextDB.WinHeight - FScrollPosition;
        for (int I = 0, last = FScrollText.Count - 1; I <= last; I++)
        {
            var scrollText = FScrollText[I];
            Aow.Game.Ui.Dialogs.DialogHosts.FontModule.AoW15WhiteGrey.SetTextCentered(X, Y, scrollText, SD);
            Y += 20;
        }
    }

    // AoWCampaignJoin.pas:132
    public void DefaultFacePnlMouseDown(int Sender, int Button, int Shift, int X, int Y)
    {
    }

    internal static void InitializeCampaignJoin(CampaignJoinDlg Dialog)
    {
        Dialog.CharGenPnl.Visible = true;
        Dialog.LeaderTypePnl.Visible = false;
        DialogHosts.TitleWindow.TitleWin.Visible = true;
        Dialog.ScrollTextDB.Visible = true;
        Dialog.FScrollDelay = 0;
        Dialog.FScrollPosition = 0;
        Dialog.FScrollText.Clear();
    }

}
