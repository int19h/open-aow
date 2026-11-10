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
public partial class TaskBar : DataModule
{
    public TaskBar(Component owner) : base(owner)
    { }

    public AOWWindow TaskWindow;
    public AOWButton TBNextButton;
    public AOWMenuBar StartMenuBar;
    public AOWEdit TBMessEdit;
    public AOWLabel WGILbl;
    public AOWLabel WGTLbl;
    public AOWTurnButton TurnButton;
    public AOWButton WMButton;
    public AOWButton OVButton;
    public AOWButton ConButton;
    public AOWButton StructButton;
    public AOWButton OV2Btn;
    public AOWButton InfoBtn;
    public AOWButton ChatBtn;
    public AOWPanel TaskPanel;
    public AOWPanel WGIPnl;
    public AoWFrame TBbk;
    public AOWImage TimerImg;
    public AOWImage TimeIcn;
    public AOWLabel TimeLbl;
    public AoWFrame PInd;
    public AOWImage CurP;
    public AOWPanel TBWorld;
    public AOWButton TCNextBtn;
    public AOWMenuBar TCStartBar;
    public AOWEdit TCMsg;
    public AOWTurnButton TCTurnButton;
    public AOWButton TCScannerBtn;
    public AOWButton TCUnitBtn;
    public AoWFrame AoWFrame1;
    public AOWImage TCTimerImg;
    public AOWImage TCTimeImg;
    public AOWLabel TCTimeLbl;
    public AoWFrame TCPInd;
    public AOWImage TCCurP;
    public AOWPanel TBCombat;
    public AOWPanel TCPnl;
    public AOWButton TCChatBtn;
    public AOWPanel WGoldPnl;
    public AOWLabel WGSign;
    public AOWLabel WMILbl;
    public AOWLabel WMTLbl;
    public AOWPanel WMIPnl;
    public AOWPanel WManaPnl;
    public AOWLabel WMTSign;
    public AOWLabel TGILbl;
    public AOWLabel TGTLbl;
    public AOWImage TGIcn;
    public AOWPanel TGIPnl;
    public AOWPanel TGoldPnl;
    public AOWLabel TGSign;
    public AOWLabel TMILbl;
    public AOWLabel TMTLbl;
    public AOWImage TWMIcn;
    public AOWPanel TMIPnl;
    public AOWPanel TManaPnl;
    public AOWLabel TMTSign;
    public AOWButton RelationsBtn;
    public AOWCheckBox AutoBtn;
    public IvTranslator Translator;
    public int FIndicatorFrame;
    public int FIndicatorStep;
    public short FWorldClockDelay;
    public short FCombatClockDelay;
    public short FChatFlash;

    // AoWTBWindow.pas:122
    public void TurnButtonAnimDone()
    {
        TurnButton.Checked = false;
        if (AoWCombatMap.Current != null)
        {
            AoWCombatMap.Current.FEndTurnPending = true;
        }
    }

    // AoWTBWindow.pas:128
    public void WMButtonClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        DialogHosts.SWindow.ScannerWindow.Visible = !DialogHosts.SWindow.ScannerWindow.Visible;
    }

    // AoWTBWindow.pas:134
    public void OVButtonClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        DialogHosts.RealmWin.Dlg.Visible = !DialogHosts.RealmWin.Dlg.Visible;
        if (DialogHosts.RealmWin.Dlg.Visible)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.UpdateOverview();
        }
    }

    // AoWTBWindow.pas:141
    public void ConButtonClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        DialogHosts.ControlWin.ControlWindow.Visible = !DialogHosts.ControlWin.ControlWindow.Visible;
    }

    // AoWTBWindow.pas:147
    public void StructButtonClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        DialogHosts.IWindow.InfoWin.Visible = !DialogHosts.IWindow.InfoWin.Visible;
    }

    // AoWTBWindow.pas:153
    public void TBNextButtonClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.World.Map.AoWHSMap.Current.SelectNextArmy();
    }

    // AoWTBWindow.pas:159
    public void UnitBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        DialogHosts.UnitWindow.UnitWin.Visible = true;
    }

    // AoWTBWindow.pas:165
    public void InfoBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (DialogHosts.PInfoW.PInfoWin.Visible)
        {
            DialogHosts.PInfoW.PInfoWin.Visible = false;
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
        DialogHosts.ChatWindow.ChatWin.Visible = !DialogHosts.ChatWindow.ChatWin.Visible;
    }

    // AoWTBWindow.pas:188
    public void TurnButtonChange()
    {
        if (!Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FSingleProcessCycle)
        {
            var endTurn = Aow.Game.World.Map.AoWHSMap.Current.EndTurn();
            TurnButton.Checked = endTurn;
        }
        else
        {
            var cancelEndTurn = Aow.Game.World.Map.AoWHSMap.Current.CancelEndTurn();
            TurnButton.Checked = !cancelEndTurn;
        }
    }

    // AoWTBWindow.pas:195
    public void PIndDraw(SurfaceDesc Surface)
    {
        int Position;
        int Race;
        Position = 0;
        if (World.Map.AoWHSMap.Current != null)
        {
            for (int I = 0, last = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetCount() - 1; I <= last; I++)
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus == Player.Status.Playing)
                {
                    Race = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerRace;
                    if (Race == -1)
                    {
                        Race = 12;
                    }

                    DialogHosts.IntGfxMod.TBWin[Race + 32].ShowClipped(PInd.WinLeft + 3 + Position * 5, PInd.WinTop + 2, Surface);
                    if (World.Map.AoWHSMap.Current.FPlayMode == PlayMode.Turn)
                    {
                        if (I == World.Map.AoWHSMap.Current.FCurrentPlayer)
                        {
                            CurP.WinLeft = PInd.WinLeft + 2 + Position * 5;
                            CurP.WinTop = PInd.WinTop + 11;
                        }
                    }
                    else
                    {
                        if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(I).FSingleProcessCycle)
                        {
                            DialogHosts.IntGfxMod.TBWin[50].ShowClipped(PInd.WinLeft + 2 + Position * 5, PInd.WinTop + 11, Surface);
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
            var endTurn = Aow.Game.Combat.AoWCombatMap.Current.EndTurn(false);
            TCTurnButton.Checked = endTurn;
        }
        else
        {
            Aow.Game.Combat.AoWCombatMap.Current.CancelEndTurn();
        }
    }

    // AoWTBWindow.pas:237
    public void TCTurnButtonAnimDone()
    {
        DialogHosts.General.Manager.OverlayPriority = 0;
        TCTurnButton.Checked = false;
        AoWCombatMap.Current.FEndTurnPending = true;
    }

    // AoWTBWindow.pas:244
    public void TCPIndDraw(SurfaceDesc Surface)
    {
        int Position;
        int Race;
        Position = 0;
        for (int I = 0, last = AoWCombatMap.Current.FCombat.FCombatData.FTurnOrder.Count - 1; I <= last; I++)
        {
            var turnOrder = AoWCombatMap.Current.FCombat.FCombatData.FTurnOrder[I];
            var player = Aow.Game.Combat.AoWCombatMap.Current.FPlayers.GetPlayers((sbyte)turnOrder);
            Race = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(player.FPlayer).FPlayerRace;
            if (Race == -1)
            {
                Race = 12;
            }

            DialogHosts.IntGfxMod.TBWin[Race + 32].ShowClipped(TCPInd.WinLeft + 3 + Position * 5, TCPInd.WinTop + 2, Surface);
            if (AoWCombatMap.Current.FCombat.FCombatData.FTurnOrder[I] == AoWCombatMap.Current.FCurrentCombatPlayer)
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
        DialogHosts.TCScanner.TCScannerWin.Visible = !DialogHosts.TCScanner.TCScannerWin.Visible;
    }

    // AoWTBWindow.pas:279
    public void TCNextBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.Combat.AoWCombatMap.Current.SelectNextUnit(true);
    }

    // AoWTBWindow.pas:285
    public void TBWorldUpdate()
    {
        ushort Hour = default;
        ushort Minute = default;
        ushort Second = default;
        ushort Millisecond = default;
        if (TBWorld.Visible && World.Map.AoWHSMap.Current != null)
        {
            FWorldClockDelay = (short)(FWorldClockDelay - 1);
            if (FWorldClockDelay < 0)
            {
                if (World.Map.AoWHSMap.Current.FTurnTime != 0)
                {
                    FWorldClockDelay = 10;
                    Minute = (ushort)(Aow.Game.World.Map.AoWHSMap.Current.GetTurnTimeLeft() / 60000);
                    Second = (ushort)(Aow.Game.World.Map.AoWHSMap.Current.GetTurnTimeLeft() % 60000 / 1000);
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
        if (TBCombat.Visible && AoWCombatMap.Current != null)
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
        if (DialogHosts.MagicWin.Dlg.Visible)
        {
            Aow.Game.Ui.Dialogs.MagicWin.HideMagic(DialogHosts.MagicWin);
        }
        else
        {
            Aow.Game.Ui.Dialogs.TaskBar.ShowMagicWindow(DialogHosts.MagicWin);
        }
    }

    // AoWTBWindow.pas:344
    public void TCUnitBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        DialogHosts.TCUnit.TCUnitWin.Visible = !DialogHosts.TCUnit.TCUnitWin.Visible;
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
        var relWindow = DialogHosts.RelWindow;
        if (relWindow.RelWin.Visible)
        {
            relWindow.RelWin.Visible = false;
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.RelWindow.ShowRelations(World.Map.AoWHSMap.Current.FSeatedPlayer);
        }
    }

    // AoWTBWindow.pas:363
    public void ChatBtnUpdate(AOWButton Sender)
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
        if (AoWCombatMap.Current.FSeatedCombatPlayer > -1)
        {
            Aow.Game.Combat.AoWCombatMap.Current.FPlayers.GetPlayers(AoWCombatMap.Current.FSeatedCombatPlayer).SetAuto(AutoBtn.Checked);
        }
    }

    internal static void ShowMagicWindow(MagicWin Window)
    {
        if (!Window.Dlg.Visible)
        {
            global::Aow.Game.Ui.Dialogs.MagicWin.RefreshMagicWindow(Window);
            global::Aow.Game.Ui.Dialogs.MagicWin.RefreshGlobalMagic(Window);
        }
        Window.Dlg.Visible = true;
    }
}
