// Translated from AoWCampaignJoin.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.aowUserDrawBox;
using static Aow.Units.aowUserDrawBox.aowUserDrawBox;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.E2Globals;
using static Aow.Units.E2Globals.E2Globals;
using Aow.Units.E1GameFlow;
using static Aow.Units.E1GameFlow.E1GameFlow;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;

namespace Aow.Units.AoWCampaignJoin
{
    // { TCampaignJoinDlg (instance size $80, VMT 00418020): $68 stage, $6C faction, $70 leader, $74 scroll delay, $78 scroll position, $7C text. }
    public partial class TCampaignJoinDlg : Aow.Rtl.TDataModule
    {
        public TCampaignJoinDlg(Aow.Rtl.TComponent AOwner) : base(AOwner) { }

        public Aow.Units.AoWPanel.TAOWPanel CharGenPnl;
        // { +0044 }
        public Aow.Units.AoWPanel.TAOWPanel LeaderTypePnl;
        // { +0048 }
        public Aow.Units.AoWButton.TAOWButton CustomBtn;
        // { +004C }
        public Aow.Units.AoWButton.TAOWButton DefaultBtn;
        // { +0050 }
        public Aow.Units.AoWMemo.TAOWMemo LeaderTypeMsg;
        // { +0054 }
        public Aow.Units.AoWPanel.TAOWPanel DefaultFacePnl;
        // { +0058 }
        public Aow.Units.AoWPanel.TAOWPanel CustomFacePnl;
        // { +005C }
        public Aow.Units.aowUserDrawBox.TaowUserDrawBox ScrollTextDB;
        // { +0060 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0064 }
        public byte FStage;
        // { $68 }
        public Aow.Units.FixedArray_byte_3 F_CampaignJoinDlg_69;
        public int FFaction;
        // { $6C }
        public Aow.Units.AoWE.TLeader FLeader;
        // { $70 }
        public int FScrollDelay;
        // { $74 }
        public int FScrollPosition;
        // { $78 }
        public Aow.Rtl.TStringList FScrollText;
        // AoWCampaignJoin.pas:160
        // { $7C }
        public void CustomizeLeader()
        {
            Aow.Units.AoWE.TLeaderSetupControl Control = default;
            FStage = 2;
            CharGenPnl.Visible = false;
            Control = new Aow.Units.AoWE.TLeaderSetupControl(null);
            Control.FLeader.Assign(FLeader, Aow.Units.AoWE.AoWE.AoWEngine);
            Control.FOriginalLeader.Assign(FLeader, Aow.Units.AoWE.AoWE.AoWEngine);
            Control.FMode = Control.FMode + TSet<Aow.Units.AoWE.TLeaderSetupModeElement>.Of(Aow.Units.AoWE.TLeaderSetupModeElement.lsmCampaign);
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

            Aow.Units.AoWLeaderSetup.AoWLeaderSetup.LeaderSetupWin.Execute(Control);
            Control.FOnDone = LeaderCustomized;
            Control.Release();
            Aow.Units.AoWLeaderSetup.AoWLeaderSetup.LeaderSetupWin.LeaderPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FactionWin;
            Aow.Units.AoWLeaderSetup.AoWLeaderSetup.LeaderSetupWin.LeaderPnl.ILIndexTexture = FFaction + 10;
        }

        // AoWCampaignJoin.pas:145
        public void LeaderCustomized(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TLeader Leader = default;
            var leaderSetupControl = (Aow.Units.AoWE.TLeaderSetupControl)Sender;
            if (leaderSetupControl.FStatus == Aow.Units.AoWE.TLeaderSetupStatus.lssDone)
            {
                var call__2 = leaderSetupControl.FOriginalLeader.Copy(Aow.Units.AoWE.AoWE.AoWEngine);
                Leader = (Aow.Units.AoWE.TLeader)call__2;
                var call__3 = Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap.GetCampaign();
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
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.Visible = true;
            ScrollTextDB.Visible = true;
            FScrollDelay = 0;
            FScrollPosition = 0;
            FScrollText.Clear();
        }

        // AoWCampaignJoin.pas:200
        public void FinishSelection()
        {
            CharGenPnl.Visible = false;
            Aow.Units.E1GameFlow.E1GameFlow.ShowCampaignMap(Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap);
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
        // { draft }
        public void CampaignJoinDlgCreate()
        {
            var call__4 = new Aow.Units.AoWE.TLeader(null);
            FLeader = call__4;
            var call__5 = new Aow.Rtl.TStringList();
            FScrollText = call__5;
        }

        // AoWCampaignJoin.pas:64
        public void CampaignJoinDlgDestroy()
        {
            FLeader.Release();
            FScrollText.Free();
        }

        // AoWCampaignJoin.pas:71
        public void CharGenPnlMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (FStage == 0)
            {
                ShowLeaderChoices();
            }
        }

        // AoWCampaignJoin.pas:77
        public void DefaultFacePnlDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Rtl.TRect R = default;
            var call__6 = Aow.Rtl.Rtl.Rect(DefaultFacePnl.WinLeft + 1, DefaultFacePnl.WinTop + 1, (DefaultFacePnl.WinLeft + DefaultFacePnl.WinWidth) - 2, (DefaultFacePnl.WinTop + DefaultFacePnl.WinHeight) - 2);
            R = call__6;
            FLeader.ShowFace(R, SD);
        }

        // AoWCampaignJoin.pas:88
        public void CustomBtnClick()
        {
            CustomizeLeader();
        }

        // AoWCampaignJoin.pas:91
        // { draft }
        public void DefaultBtnClick()
        {
            var call__7 = Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap.GetCampaign();
            call__7.FTransferSettings.SetLeader(FLeader);
            FinishSelection();
        }

        // AoWCampaignJoin.pas:97
        // { draft }
        public void ScrollTextDBUpdate()
        {
            if (FStage == 0)
            {
                var call__8 = FScrollText.Count;
                if (FScrollPosition < ((call__8 * 20) + ScrollTextDB.WinHeight))
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
        public void ScrollTextDBDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int I = default;
            int X = default;
            int Y = default;
            SD.ClipRect.Left = CharGenPnl.WinLeft + ScrollTextDB.WinLeft;
            SD.ClipRect.Right = SD.ClipRect.Left + ScrollTextDB.WinWidth;
            SD.ClipRect.Top = CharGenPnl.WinTop + ScrollTextDB.WinTop;
            SD.ClipRect.Bottom = SD.ClipRect.Top + ScrollTextDB.WinHeight;
            X = (CharGenPnl.WinLeft + ScrollTextDB.WinLeft) + (ScrollTextDB.WinWidth / 2);
            Y = (SD.ClipRect.Top + ScrollTextDB.WinHeight) - FScrollPosition;
            {
                I = 0;
                int end__9 = FScrollText.Count - 1;
                for (; I <= end__9; I += 1)
                {
                    var call__10 = FScrollText[I];
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey.SetTextCentered(X, Y, call__10, SD);
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
