// Translated from AoWCampaignJoin.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.AoWButton;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.aowUserDrawBox;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.E2Globals.E2Globals;
using static Aow.Units.E1GameFlow.E1GameFlow;
using static Aow.Units.AoWExeForms.AoWExeForms;

namespace Aow.Units.AoWCampaignJoin
{
    // stage, faction, leader, scroll delay, scroll position, text
    public partial class TCampaignJoinDlg : TDataModule
    {
        public TCampaignJoinDlg(TComponent owner) : base(owner)
        { }

        public TAOWPanel CharGenPnl;
        public TAOWPanel LeaderTypePnl;
        public TAOWButton CustomBtn;
        public TAOWButton DefaultBtn;
        public TAOWMemo LeaderTypeMsg;
        public TAOWPanel DefaultFacePnl;
        public TAOWPanel CustomFacePnl;
        public TaowUserDrawBox ScrollTextDB;
        public TIvTranslator Translator;
        public byte FStage;
        public int FFaction;
        public TLeader FLeader;
        public int FScrollDelay;
        public int FScrollPosition;
        public TStringList FScrollText;
        // AoWCampaignJoin.pas:160
        public void CustomizeLeader()
        {
            TLeaderSetupControl Control = default;
            FStage = 2;
            CharGenPnl.Visible = false;
            Control = new TLeaderSetupControl(null);
            Control.FLeader.Assign(FLeader, AoWEngine);
            Control.FOriginalLeader.Assign(FLeader, AoWEngine);
            Control.FMode = Control.FMode + TSet<TLeaderSetupModeElement>.Of(TLeaderSetupModeElement.lsmCampaign);
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

            AoWLeaderSetup.AoWLeaderSetup.LeaderSetupWin.Execute(Control);
            Control.FOnDone = LeaderCustomized;
            Control.Release();
            AoWLeaderSetup.AoWLeaderSetup.LeaderSetupWin.LeaderPnl.ILTexture = aowIntGfxMod.aowIntGfxMod.IntGfxMod.FactionWin;
            AoWLeaderSetup.AoWLeaderSetup.LeaderSetupWin.LeaderPnl.ILIndexTexture = FFaction + 10;
        }

        // AoWCampaignJoin.pas:145
        public void LeaderCustomized(TObject Sender)
        {
            TLeader Leader = default;
            var leaderSetupControl = (TLeaderSetupControl)Sender;
            if (leaderSetupControl.FStatus == TLeaderSetupStatus.lssDone)
            {
                var call__2 = leaderSetupControl.FOriginalLeader.Copy(AoWEngine);
                Leader = (TLeader)call__2;
                var call__3 = AoWWorldMap.AoWWorldMap.WorldMap.GetCampaign();
                call__3.FTransferSettings.SetLeader(Leader);
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
            TitleWindow.TitleWin.Visible = true;
            ScrollTextDB.Visible = true;
            FScrollDelay = 0;
            FScrollPosition = 0;
            FScrollText.Clear();
        }

        // AoWCampaignJoin.pas:200
        public void FinishSelection()
        {
            CharGenPnl.Visible = false;
            ShowCampaignMap(AoWWorldMap.AoWWorldMap.WorldMap);
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
            var call__4 = new TLeader(null);
            FLeader = call__4;
            var call__5 = new TStringList();
            FScrollText = call__5;
        }

        // AoWCampaignJoin.pas:64
        public void CampaignJoinDlgDestroy()
        {
            FLeader.Release();
            FScrollText.Free();
        }

        // AoWCampaignJoin.pas:71
        public void CharGenPnlMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            if (FStage == 0)
            {
                ShowLeaderChoices();
            }
        }

        // AoWCampaignJoin.pas:77
        public void DefaultFacePnlDraw(TObject Sender, TSurfaceDesc SD)
        {
            TRect R = default;
            var call__6 = Rect(DefaultFacePnl.WinLeft + 1, DefaultFacePnl.WinTop + 1, DefaultFacePnl.WinLeft + DefaultFacePnl.WinWidth - 2, DefaultFacePnl.WinTop + DefaultFacePnl.WinHeight - 2);
            R = call__6;
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
            var call__7 = AoWWorldMap.AoWWorldMap.WorldMap.GetCampaign();
            call__7.FTransferSettings.SetLeader(FLeader);
            FinishSelection();
        }

        // AoWCampaignJoin.pas:97
        public void ScrollTextDBUpdate()
        {
            if (FStage == 0)
            {
                var call__8 = FScrollText.Count;
                if (FScrollPosition < call__8 * 20 + ScrollTextDB.WinHeight)
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
        public void ScrollTextDBDraw(TObject Sender, TSurfaceDesc SD)
        {
            int I = default;
            int X = default;
            int Y = default;
            SD.ClipRect.Left = CharGenPnl.WinLeft + ScrollTextDB.WinLeft;
            SD.ClipRect.Right = SD.ClipRect.Left + ScrollTextDB.WinWidth;
            SD.ClipRect.Top = CharGenPnl.WinTop + ScrollTextDB.WinTop;
            SD.ClipRect.Bottom = SD.ClipRect.Top + ScrollTextDB.WinHeight;
            X = CharGenPnl.WinLeft + ScrollTextDB.WinLeft + ScrollTextDB.WinWidth / 2;
            Y = SD.ClipRect.Top + ScrollTextDB.WinHeight - FScrollPosition;
            {
                I = 0;
                int end__9 = FScrollText.Count - 1;
                for (; I <= end__9; I += 1)
                {
                    var call__10 = FScrollText[I];
                    FontModule.AoW15WhiteGrey.SetTextCentered(X, Y, call__10, SD);
                    Y += 20;
                }
            }
        }

        // AoWCampaignJoin.pas:132
        public void DefaultFacePnlMouseDown(int A1, int A2, int A3, int A4, int A5)
        {
        }
    }

    public static partial class AoWCampaignJoin
    {
    }
}
