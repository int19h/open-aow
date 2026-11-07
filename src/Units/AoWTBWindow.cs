// Translated from AoWTBWindow.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AOWControlWin;
using Aow.Units.AoWE;
using Aow.Units.AoWEdit;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWInfoWin;
using Aow.Units.aowIntGfxMod;
using Aow.Units.AoWLabel;
using Aow.Units.AOWMenuBar;
using Aow.Units.AoWPanel;
using Aow.Units.AoWSWindow;
using Aow.Units.AoWUnitWindow;
using Aow.Units.AoWWindow;
using Aow.Units.GFXE;
using Aow.Units.TurnButton;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.aowMapEvents.aowMapEventsUnit;
using static Aow.Units.AoWPInfoWin.AoWPInfoWinUnit;
using static Aow.Units.AoWRealm.AoWRealmUnit;
using static Aow.Units.AoWTC.AoWTCUnit;
using static Aow.Units.AoWTCUnit.AoWTCUnitUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using static Aow.Units.E1GameFlow.E1GameFlowUnit;
using static Aow.Units.E1Globals.E1GlobalsUnit;

namespace Aow.Units.AoWTBWindow;

// indicator frame/step, world/combat clock delays, chat flash countdown
public partial class TTBWindow : TDataModule
{
    public TTBWindow(TComponent owner) : base(owner)
    { }

    public TAOWWindow TaskWindow;
    public TAOWButton TBNextButton;
    public TAOWMenuBar StartMenuBar;
    public TAOWEdit TBMessEdit;
    public TAOWLabel WGILbl;
    public TAOWLabel WGTLbl;
    public TAOWTurnButton TurnButton;
    public TAOWButton WMButton;
    public TAOWButton OVButton;
    public TAOWButton ConButton;
    public TAOWButton StructButton;
    public TAOWButton OV2Btn;
    public TAOWButton InfoBtn;
    public TAOWButton ChatBtn;
    public TAOWPanel TaskPanel;
    public TAOWPanel WGIPnl;
    public TAoWFrame TBbk;
    public TAOWImage TimerImg;
    public TAOWImage TimeIcn;
    public TAOWLabel TimeLbl;
    public TAoWFrame PInd;
    public TAOWImage CurP;
    public TAOWPanel TBWorld;
    public TAOWButton TCNextBtn;
    public TAOWMenuBar TCStartBar;
    public TAOWEdit TCMsg;
    public TAOWTurnButton TCTurnButton;
    public TAOWButton TCScannerBtn;
    public TAOWButton TCUnitBtn;
    public TAoWFrame AoWFrame1;
    public TAOWImage TCTimerImg;
    public TAOWImage TCTimeImg;
    public TAOWLabel TCTimeLbl;
    public TAoWFrame TCPInd;
    public TAOWImage TCCurP;
    public TAOWPanel TBCombat;
    public TAOWPanel TCPnl;
    public TAOWButton TCChatBtn;
    public TAOWPanel WGoldPnl;
    public TAOWLabel WGSign;
    public TAOWLabel WMILbl;
    public TAOWLabel WMTLbl;
    public TAOWPanel WMIPnl;
    public TAOWPanel WManaPnl;
    public TAOWLabel WMTSign;
    public TAOWLabel TGILbl;
    public TAOWLabel TGTLbl;
    public TAOWImage TGIcn;
    public TAOWPanel TGIPnl;
    public TAOWPanel TGoldPnl;
    public TAOWLabel TGSign;
    public TAOWLabel TMILbl;
    public TAOWLabel TMTLbl;
    public TAOWImage TWMIcn;
    public TAOWPanel TMIPnl;
    public TAOWPanel TManaPnl;
    public TAOWLabel TMTSign;
    public TAOWButton RelationsBtn;
    public TAOWCheckBox AutoBtn;
    public TIvTranslator Translator;
    public int FIndicatorFrame;
    public int FIndicatorStep;
    public short FWorldClockDelay;
    public short FCombatClockDelay;
    public short FChatFlash;

    // AoWTBWindow.pas:122
    public void TurnButtonAnimDone()
    {
        TurnButton.Checked = false;
        if (AoWCombatMap != null)
        {
            AoWCombatMap.FEndTurnPending = true;
        }
    }

    // AoWTBWindow.pas:128
    public void WMButtonClick()
    {
        General.PlayButtonSound(0);
        AoWSWindowUnit.SWindow.ScannerWindow.Visible = !AoWSWindowUnit.SWindow.ScannerWindow.Visible;
    }

    // AoWTBWindow.pas:134
    public void OVButtonClick(TObject Sender)
    {
        if (Sender != null)
        {
            General.PlayButtonSound(0);
        }

        RealmWin.Dlg.Visible = !RealmWin.Dlg.Visible;
        if (RealmWin.Dlg.Visible)
        {
            RealmWin.UpdateOverview();
        }
    }

    // AoWTBWindow.pas:141
    public void ConButtonClick()
    {
        General.PlayButtonSound(0);
        AOWControlWinUnit.ControlWin.ControlWindow.Visible = !AOWControlWinUnit.ControlWin.ControlWindow.Visible;
    }

    // AoWTBWindow.pas:147
    public void StructButtonClick()
    {
        General.PlayButtonSound(0);
        AoWInfoWinUnit.IWindow.InfoWin.Visible = !AoWInfoWinUnit.IWindow.InfoWin.Visible;
    }

    // AoWTBWindow.pas:153
    public void TBNextButtonClick()
    {
        General.PlayButtonSound(0);
        AoWHSMap.SelectNextArmy();
    }

    // AoWTBWindow.pas:159
    public void UnitBtnClick()
    {
        General.PlayButtonSound(0);
        AoWUnitWindowUnit.UnitWindow.UnitWin.Visible = true;
    }

    // AoWTBWindow.pas:165
    public void InfoBtnClick(TObject Sender)
    {
        General.PlayButtonSound(0);
        if (PInfoW.PInfoWin.Visible)
        {
            PInfoW.PInfoWin.Visible = false;
        }
        else
        {
            PInfoW.UpdatePlayer();
        }
    }

    // AoWTBWindow.pas:172
    public void ProdBtnClick()
    {
        General.PlayButtonSound(0);
    }

    // AoWTBWindow.pas:177
    public void COBtnClick()
    {
        General.PlayButtonSound(0);
    }

    // AoWTBWindow.pas:182
    public void ChatBtnClick()
    {
        General.PlayButtonSound(0);
        ChatWindow.ChatWin.Visible = !ChatWindow.ChatWin.Visible;
    }

    // AoWTBWindow.pas:188
    public void TurnButtonChange()
    {
        if (!AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FSingleProcessCycle)
        {
            var endTurn = AoWHSMap.EndTurn();
            TurnButton.Checked = endTurn;
        }
        else
        {
            var cancelEndTurn = AoWHSMap.CancelEndTurn();
            TurnButton.Checked = !cancelEndTurn;
        }
    }

    // AoWTBWindow.pas:195
    public void PIndDraw(TObject Sender, TSurfaceDesc Surface)
    {
        int Position;
        int Race;
        Position = 0;
        if (AoWHSMap != null)
        {
            for (int I = 0, last = AoWHSMap.FPlayers.GetCount() - 1; I <= last; I++)
            {
                if (AoWHSMap.FPlayers.GetPlayers(I).FStatus == TPlayerStatus.psPlaying)
                {
                    Race = AoWHSMap.FPlayers.GetPlayers(I).FPlayerRace;
                    if (Race == -1)
                    {
                        Race = 12;
                    }

                    aowIntGfxModUnit.IntGfxMod.TBWin[Race + 32].ShowClipped(PInd.WinLeft + 3 + Position * 5, PInd.WinTop + 2, Surface);
                    if (AoWHSMap.FPlayMode == TPlayMode.pmTurn)
                    {
                        if (I == AoWHSMap.FCurrentPlayer)
                        {
                            CurP.WinLeft = PInd.WinLeft + 2 + Position * 5;
                            CurP.WinTop = PInd.WinTop + 11;
                        }
                    }
                    else
                    {
                        if (AoWHSMap.FPlayers.GetPlayers(I).FSingleProcessCycle)
                        {
                            aowIntGfxModUnit.IntGfxMod.TBWin[50].ShowClipped(PInd.WinLeft + 2 + Position * 5, PInd.WinTop + 11, Surface);
                        }
                    }

                    Position += 1;
                }
            }
        }
    }

    // AoWTBWindow.pas:222
    public void CurPUpdate()
    {
        FIndicatorFrame += FIndicatorStep;
        if (FIndicatorFrame > 4)
        {
            FIndicatorStep = -1;
        }

        if (FIndicatorFrame < 1)
        {
            FIndicatorStep = 1;
        }

        CurP.CurrentImage = FIndicatorFrame + 45;
        PInd.FDrawn = false;
    }

    // AoWTBWindow.pas:231
    public void TCTurnButtonChange()
    {
        if (TCTurnButton.Checked)
        {
            var endTurn = AoWCombatMap.EndTurn(false);
            TCTurnButton.Checked = endTurn;
        }
        else
        {
            AoWCombatMap.CancelEndTurn();
        }
    }

    // AoWTBWindow.pas:237
    public void TCTurnButtonAnimDone()
    {
        General.Manager.OverlayPriority = 0;
        TCTurnButton.Checked = false;
        AoWCombatMap.FEndTurnPending = true;
    }

    // AoWTBWindow.pas:244
    public void TCPIndDraw(TObject Sender, TSurfaceDesc Surface)
    {
        int Position;
        int Race;
        Position = 0;
        for (int I = 0, last = AoWCombatMap.FCombat.FCombatData.FTurnOrder.Count - 1; I <= last; I++)
        {
            var turnOrder = AoWCombatMap.FCombat.FCombatData.FTurnOrder[I];
            var player = AoWCombatMap.FPlayers.GetPlayers((sbyte)turnOrder);
            Race = AoWHSMap.FPlayers.GetPlayers(player.FPlayer).FPlayerRace;
            if (Race == -1)
            {
                Race = 12;
            }

            aowIntGfxModUnit.IntGfxMod.TBWin[Race + 32].ShowClipped(TCPInd.WinLeft + 3 + Position * 5, TCPInd.WinTop + 2, Surface);
            if (AoWCombatMap.FCombat.FCombatData.FTurnOrder[I] == AoWCombatMap.FCurrentCombatPlayer)
            {
                TCCurP.WinLeft = TCPInd.WinLeft + 2 + Position * 5;
                TCCurP.WinTop = TCPInd.WinTop + 11;
            }

            Position += 1;
        }
    }

    // AoWTBWindow.pas:264
    public void TCCurPUpdate()
    {
        FIndicatorFrame += FIndicatorStep;
        if (FIndicatorFrame > 4)
        {
            FIndicatorStep = -1;
        }

        if (FIndicatorFrame < 1)
        {
            FIndicatorStep = 1;
        }

        TCCurP.CurrentImage = FIndicatorFrame + 45;
        TCPInd.FDrawn = false;
    }

    // AoWTBWindow.pas:273
    public void TCScannerBtnClick()
    {
        General.PlayButtonSound(0);
        TCScanner.TCScannerWin.Visible = !TCScanner.TCScannerWin.Visible;
    }

    // AoWTBWindow.pas:279
    public void TCNextBtnClick()
    {
        General.PlayButtonSound(0);
        AoWCombatMap.SelectNextUnit(true);
    }

    // AoWTBWindow.pas:285
    public void TBWorldUpdate(TObject Sender)
    {
        ushort Hour = default;
        ushort Minute = default;
        ushort Second = default;
        ushort Millisecond = default;
        if (TBWorld.Visible && AoWHSMap != null)
        {
            FWorldClockDelay = (short)(FWorldClockDelay - 1);
            if (FWorldClockDelay < 0)
            {
                if (AoWHSMap.FTurnTime != 0)
                {
                    FWorldClockDelay = 10;
                    Minute = (ushort)(AoWHSMap.GetTurnTimeLeft() / 60000);
                    Second = (ushort)(AoWHSMap.GetTurnTimeLeft() % 60000 / 1000);
                    if (Minute == 0 && Second > 0 && Second <= 10)
                    {
                        TimeLbl.TextColor = 255;
                    }
                    else
                    {
                        TimeLbl.TextColor = 0;
                    }

                    TimeLbl.Text = IntToStr(Minute) + ":";
                    if (Second < 10)
                    {
                        TimeLbl.Text = TimeLbl.Text + "0";
                    }

                    TimeLbl.Text = TimeLbl.Text + IntToStr(Second);
                }
                else
                {
                    FWorldClockDelay = 900;
                    DecodeTime(Time(), ref Hour, ref Minute, ref Second, ref Millisecond);
                    if (Hour > 12)
                    {
                        Hour = (ushort)(Hour - 12);
                    }

                    if (Hour == 0)
                    {
                        Hour = 12;
                    }

                    TimeLbl.Text = IntToStr(Hour) + ":";
                    if (Minute < 10)
                    {
                        TimeLbl.Text = TimeLbl.Text + "0";
                    }

                    TimeLbl.Text = TimeLbl.Text + IntToStr(Minute);
                }
            }
        }
    }

    // AoWTBWindow.pas:317
    public void TBCombatUpdate(TObject Sender)
    {
        ushort Hour = default;
        ushort Minute = default;
        ushort Second = default;
        ushort Millisecond = default;
        if (TBCombat.Visible && AoWCombatMap != null)
        {
            FCombatClockDelay = (short)(FCombatClockDelay - 1);
            if (FCombatClockDelay < 0)
            {
                FCombatClockDelay = 900;
                DecodeTime(Time(), ref Hour, ref Minute, ref Second, ref Millisecond);
                if (Hour > 12)
                {
                    Hour = (ushort)(Hour - 12);
                }

                if (Hour == 0)
                {
                    Hour = 12;
                }

                TCTimeLbl.Text = IntToStr(Hour) + ":";
                if (Minute < 10)
                {
                    TCTimeLbl.Text = TCTimeLbl.Text + "0";
                }

                TCTimeLbl.Text = TCTimeLbl.Text + IntToStr(Minute);
            }
        }
    }

    // AoWTBWindow.pas:337
    public void OV2BtnClick()
    {
        General.PlayButtonSound(0);
        if (MagicWin.Dlg.Visible)
        {
            HideMagic(MagicWin);
        }
        else
        {
            ShowMagicWindow(MagicWin);
        }
    }

    // AoWTBWindow.pas:344
    public void TCUnitBtnClick()
    {
        General.PlayButtonSound(0);
        TCUnit.TCUnitWin.Visible = !TCUnit.TCUnitWin.Visible;
    }

    // AoWTBWindow.pas:350
    public void StartMenuBarChange()
    {
        General.PlayButtonSound(3);
    }

    // AoWTBWindow.pas:355
    public void RelationsBtnClick(TObject Sender)
    {
        General.PlayButtonSound(0);
        var relWindow = RelWindow;
        if (relWindow.RelWin.Visible)
        {
            relWindow.RelWin.Visible = false;
        }
        else
        {
            RelWindow.ShowRelations(AoWHSMap.FSeatedPlayer);
        }
    }

    // AoWTBWindow.pas:363
    public void ChatBtnUpdate(TObject Sender)
    {
        if (FChatFlash > 0)
        {
            switch (FChatFlash)
            {
                case 1:
                case 31:
                case 61:
                case 91:
                case 121:
                    {
                        var button = (TAOWButton)Sender;
                        button.FDrawn = false;
                        button.ILIndexUp = 14;
                    }

                    break;
                case 16:
                case 46:
                case 76:
                case 106:
                case 136:
                    {
                        var button = (TAOWButton)Sender;
                        button.FDrawn = false;
                        button.ILIndexUp = 20;
                    }

                    break;
            }

            FChatFlash = (short)(FChatFlash - 1);
        }
    }

    // AoWTBWindow.pas:377
    public void AutoBtn2Click()
    {
        if (AoWCombatMap.FSeatedCombatPlayer > -1)
        {
            AoWCombatMap.FPlayers.GetPlayers(AoWCombatMap.FSeatedCombatPlayer).SetAuto(AutoBtn.Checked);
        }
    }
}

public static partial class AoWTBWindowUnit
{
}
