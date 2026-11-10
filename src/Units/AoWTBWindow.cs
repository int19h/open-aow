// Translated from AoWTBWindow.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWTBWindow.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

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
        if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap != null)
        {
            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FEndTurnPending = true;
        }
    }

    // AoWTBWindow.pas:128
    public void WMButtonClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.Visible = !Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.Visible;
    }

    // AoWTBWindow.pas:134
    public void OVButtonClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible = !Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible;
        if (Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.UpdateOverview();
        }
    }

    // AoWTBWindow.pas:141
    public void ConButtonClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.Visible = !Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.Visible;
    }

    // AoWTBWindow.pas:147
    public void StructButtonClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin.Visible = !Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin.Visible;
    }

    // AoWTBWindow.pas:153
    public void TBNextButtonClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.SelectNextArmy();
    }

    // AoWTBWindow.pas:159
    public void UnitBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Visible = true;
    }

    // AoWTBWindow.pas:165
    public void InfoBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin.Visible)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin.Visible = false;
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.UpdatePlayer();
        }
    }

    // AoWTBWindow.pas:172
    public void ProdBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
    }

    // AoWTBWindow.pas:177
    public void COBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
    }

    // AoWTBWindow.pas:182
    public void ChatBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Visible = !Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Visible;
    }

    // AoWTBWindow.pas:188
    public void TurnButtonChange()
    {
        if (!Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FSingleProcessCycle)
        {
            var endTurn = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.EndTurn();
            TurnButton.Checked = endTurn;
        }
        else
        {
            var cancelEndTurn = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.CancelEndTurn();
            TurnButton.Checked = !cancelEndTurn;
        }
    }

    // AoWTBWindow.pas:195
    public void PIndDraw(TSurfaceDesc Surface)
    {
        int Position;
        int Race;
        Position = 0;
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
        {
            for (int I = 0, last = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetCount() - 1; I <= last; I++)
            {
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(I).FStatus == TPlayerStatus.psPlaying)
                {
                    Race = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(I).FPlayerRace;
                    if (Race == -1)
                    {
                        Race = 12;
                    }

                    Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.TBWin[Race + 32].ShowClipped(PInd.WinLeft + 3 + Position * 5, PInd.WinTop + 2, Surface);
                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayMode == TPlayMode.pmTurn)
                    {
                        if (I == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCurrentPlayer)
                        {
                            CurP.WinLeft = PInd.WinLeft + 2 + Position * 5;
                            CurP.WinTop = PInd.WinTop + 11;
                        }
                    }
                    else
                    {
                        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(I).FSingleProcessCycle)
                        {
                            Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.TBWin[50].ShowClipped(PInd.WinLeft + 2 + Position * 5, PInd.WinTop + 11, Surface);
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
            var endTurn = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.EndTurn(false);
            TCTurnButton.Checked = endTurn;
        }
        else
        {
            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.CancelEndTurn();
        }
    }

    // AoWTBWindow.pas:237
    public void TCTurnButtonAnimDone()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 0;
        TCTurnButton.Checked = false;
        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FEndTurnPending = true;
    }

    // AoWTBWindow.pas:244
    public void TCPIndDraw(TSurfaceDesc Surface)
    {
        int Position;
        int Race;
        Position = 0;
        for (int I = 0, last = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.FTurnOrder.Count - 1; I <= last; I++)
        {
            var turnOrder = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.FTurnOrder[I];
            var player = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlayers.GetPlayers((sbyte)turnOrder);
            Race = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(player.FPlayer).FPlayerRace;
            if (Race == -1)
            {
                Race = 12;
            }

            Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.TBWin[Race + 32].ShowClipped(TCPInd.WinLeft + 3 + Position * 5, TCPInd.WinTop + 2, Surface);
            if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.FTurnOrder[I] == Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentCombatPlayer)
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin.Visible = !Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin.Visible;
    }

    // AoWTBWindow.pas:279
    public void TCNextBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.SelectNextUnit(true);
    }

    // AoWTBWindow.pas:285
    public void TBWorldUpdate()
    {
        ushort Hour = default;
        ushort Minute = default;
        ushort Second = default;
        ushort Millisecond = default;
        if (TBWorld.Visible && Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
        {
            FWorldClockDelay = (short)(FWorldClockDelay - 1);
            if (FWorldClockDelay < 0)
            {
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTurnTime != 0)
                {
                    FWorldClockDelay = 10;
                    Minute = (ushort)(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.GetTurnTimeLeft() / 60000);
                    Second = (ushort)(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.GetTurnTimeLeft() % 60000 / 1000);
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
    public void TBCombatUpdate()
    {
        ushort Hour = default;
        ushort Minute = default;
        ushort Second = default;
        ushort Millisecond = default;
        if (TBCombat.Visible && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap != null)
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.Visible)
        {
            Aow.Game.Ui.Dialogs.TMagicWin.HideMagic(Aow.Game.Ui.Dialogs.DialogHosts.MagicWin);
        }
        else
        {
            Aow.Game.Ui.Dialogs.TTBWindow.ShowMagicWindow(Aow.Game.Ui.Dialogs.DialogHosts.MagicWin);
        }
    }

    // AoWTBWindow.pas:344
    public void TCUnitBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin.Visible = !Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin.Visible;
    }

    // AoWTBWindow.pas:350
    public void StartMenuBarChange()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(3);
    }

    // AoWTBWindow.pas:355
    public void RelationsBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        var relWindow = Aow.Game.Ui.Dialogs.DialogHosts.RelWindow;
        if (relWindow.RelWin.Visible)
        {
            relWindow.RelWin.Visible = false;
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.RelWindow.ShowRelations(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
        }
    }

    // AoWTBWindow.pas:363
    public void ChatBtnUpdate(TAOWButton Sender)
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
                    Sender.FDrawn = false;
                    Sender.ILIndexUp = 14;
                    break;
                case 16:
                case 46:
                case 76:
                case 106:
                case 136:
                    Sender.FDrawn = false;
                    Sender.ILIndexUp = 20;
                    break;
            }

            FChatFlash = (short)(FChatFlash - 1);
        }
    }

    // AoWTBWindow.pas:377
    public void AutoBtn2Click()
    {
        if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FSeatedCombatPlayer > -1)
        {
            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlayers.GetPlayers(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FSeatedCombatPlayer).SetAuto(AutoBtn.Checked);
        }
    }

    internal static void ShowMagicWindow(global::Aow.Game.Ui.Dialogs.TMagicWin Window)
    {
        if (!Window.Dlg.Visible)
        {
            global::Aow.Game.Ui.Dialogs.TMagicWin.RefreshMagicWindow(Window);
            global::Aow.Game.Ui.Dialogs.TMagicWin.RefreshGlobalMagic(Window);
        }
        Window.Dlg.Visible = true;
    }
}
