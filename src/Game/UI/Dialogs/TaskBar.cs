// Translated from AoWTBWindow.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// indicator frame/step, world/combat clock delays, chat flash countdown
public partial class TaskBar : DataModule
{
    public TaskBar(Component owner) : base(owner)
    { }

    public Window TaskWindow;
    public Button TBNextButton;
    public MenuBar StartMenuBar;
    public Edit TBMessEdit;
    public Label WGILbl;
    public Label WGTLbl;
    public TurnButton TurnButton;
    public Button WMButton;
    public Button OVButton;
    public Button ConButton;
    public Button StructButton;
    public Button OV2Btn;
    public Button InfoBtn;
    public Button ChatBtn;
    public Panel TaskPanel;
    public Panel WGIPnl;
    public Frame TBbk;
    public Image TimerImg;
    public Image TimeIcn;
    public Label TimeLbl;
    public Frame PInd;
    public Image CurP;
    public Panel TBWorld;
    public Button TCNextBtn;
    public MenuBar TCStartBar;
    public Edit TCMsg;
    public TurnButton TCTurnButton;
    public Button TCScannerBtn;
    public Button TCUnitBtn;
    public Frame AoWFrame1;
    public Image TCTimerImg;
    public Image TCTimeImg;
    public Label TCTimeLbl;
    public Frame TCPInd;
    public Image TCCurP;
    public Panel TBCombat;
    public Panel TCPnl;
    public Button TCChatBtn;
    public Panel WGoldPnl;
    public Label WGSign;
    public Label WMILbl;
    public Label WMTLbl;
    public Panel WMIPnl;
    public Panel WManaPnl;
    public Label WMTSign;
    public Label TGILbl;
    public Label TGTLbl;
    public Image TGIcn;
    public Panel TGIPnl;
    public Panel TGoldPnl;
    public Label TGSign;
    public Label TMILbl;
    public Label TMTLbl;
    public Image TWMIcn;
    public Panel TMIPnl;
    public Panel TManaPnl;
    public Label TMTSign;
    public Button RelationsBtn;
    public CheckBox AutoBtn;
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
        if (CombatMap.Current != null)
        {
            CombatMap.Current.FEndTurnPending = true;
        }
    }

    // AoWTBWindow.pas:128
    public void WMButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.SWindow.MainWindow.Visible = !DialogHosts.SWindow.MainWindow.Visible;
    }

    // AoWTBWindow.pas:134
    public void OVButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.RealmWin.Dlg.Visible = !DialogHosts.RealmWin.Dlg.Visible;
        if (DialogHosts.RealmWin.Dlg.Visible)
        {
            DialogHosts.RealmWin.UpdateOverview();
        }
    }

    // AoWTBWindow.pas:141
    public void ConButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.ControlWin.MainWindow.Visible = !DialogHosts.ControlWin.MainWindow.Visible;
    }

    // AoWTBWindow.pas:147
    public void StructButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.IWindow.InfoWin.Visible = !DialogHosts.IWindow.InfoWin.Visible;
    }

    // AoWTBWindow.pas:153
    public void TBNextButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        AoWHSMap.Current.SelectNextArmy();
    }

    // AoWTBWindow.pas:159
    public void UnitBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.UnitWindow.UnitWin.Visible = true;
    }

    // AoWTBWindow.pas:165
    public void InfoBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (DialogHosts.PInfoW.PInfoWin.Visible)
        {
            DialogHosts.PInfoW.PInfoWin.Visible = false;
        }
        else
        {
            DialogHosts.PInfoW.UpdatePlayer();
        }
    }

    // AoWTBWindow.pas:172
    public void ProdBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWTBWindow.pas:177
    public void COBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWTBWindow.pas:182
    public void ChatBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.ChatWindow.ChatWin.Visible = !DialogHosts.ChatWindow.ChatWin.Visible;
    }

    // AoWTBWindow.pas:188
    public void TurnButtonChange()
    {
        if (!AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FSingleProcessCycle)
        {
            var endTurn = AoWHSMap.Current.EndTurn();
            TurnButton.Checked = endTurn;
        }
        else
        {
            var cancelEndTurn = AoWHSMap.Current.CancelEndTurn();
            TurnButton.Checked = !cancelEndTurn;
        }
    }

    // AoWTBWindow.pas:195
    public void PIndDraw(LockedSurface Surface)
    {
        int Position;
        int Race;
        Position = 0;
        if (AoWHSMap.Current != null)
        {
            for (int I = 0, last = AoWHSMap.Current.FPlayers.GetCount() - 1; I <= last; I++)
            {
                if (AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus == Player.Status.Playing)
                {
                    Race = AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerRace;
                    if (Race == -1)
                    {
                        Race = 12;
                    }

                    DialogHosts.IntGfxMod.TBWin[Race + 32].ShowClipped(PInd.WinLeft + 3 + Position * 5, PInd.WinTop + 2, Surface);
                    if (AoWHSMap.Current.FPlayMode == PlayMode.Turn)
                    {
                        if (I == AoWHSMap.Current.FCurrentPlayer)
                        {
                            CurP.WinLeft = PInd.WinLeft + 2 + Position * 5;
                            CurP.WinTop = PInd.WinTop + 11;
                        }
                    }
                    else
                    {
                        if (AoWHSMap.Current.FPlayers.GetPlayers(I).FSingleProcessCycle)
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
            var endTurn = CombatMap.Current.EndTurn(false);
            TCTurnButton.Checked = endTurn;
        }
        else
        {
            CombatMap.Current.CancelEndTurn();
        }
    }

    // AoWTBWindow.pas:237
    public void TCTurnButtonAnimDone()
    {
        DialogHosts.General.Manager.OverlayPriority = 0;
        TCTurnButton.Checked = false;
        CombatMap.Current.FEndTurnPending = true;
    }

    // AoWTBWindow.pas:244
    public void TCPIndDraw(LockedSurface Surface)
    {
        int Position;
        int Race;
        Position = 0;
        for (int I = 0, last = CombatMap.Current.FCombat.FCombatData.FTurnOrder.Count - 1; I <= last; I++)
        {
            var turnOrder = CombatMap.Current.FCombat.FCombatData.FTurnOrder[I];
            var player = CombatMap.Current.FPlayers.GetPlayers((sbyte)turnOrder);
            Race = AoWHSMap.Current.FPlayers.GetPlayers(player.FPlayer).FPlayerRace;
            if (Race == -1)
            {
                Race = 12;
            }

            DialogHosts.IntGfxMod.TBWin[Race + 32].ShowClipped(TCPInd.WinLeft + 3 + Position * 5, TCPInd.WinTop + 2, Surface);
            if (CombatMap.Current.FCombat.FCombatData.FTurnOrder[I] == CombatMap.Current.FCurrentCombatPlayer)
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
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.TCScanner.TCScannerWin.Visible = !DialogHosts.TCScanner.TCScannerWin.Visible;
    }

    // AoWTBWindow.pas:279
    public void TCNextBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        CombatMap.Current.SelectNextUnit(true);
    }

    // AoWTBWindow.pas:285
    public void TBWorldUpdate()
    {
        ushort Hour = default;
        ushort Minute = default;
        ushort Second = default;
        ushort Millisecond = default;
        if (TBWorld.Visible && AoWHSMap.Current != null)
        {
            FWorldClockDelay = (short)(FWorldClockDelay - 1);
            if (FWorldClockDelay < 0)
            {
                if (AoWHSMap.Current.FTurnTime != 0)
                {
                    FWorldClockDelay = 10;
                    Minute = (ushort)(AoWHSMap.Current.GetTurnTimeLeft() / 60000);
                    Second = (ushort)(AoWHSMap.Current.GetTurnTimeLeft() % 60000 / 1000);
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
        if (TBCombat.Visible && CombatMap.Current != null)
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
        DialogHosts.General.PlayButtonSound(0);
        if (DialogHosts.MagicWin.Dlg.Visible)
        {
            MagicWindow.HideMagic(DialogHosts.MagicWin);
        }
        else
        {
            ShowMagicWindow(DialogHosts.MagicWin);
        }
    }

    // AoWTBWindow.pas:344
    public void TCUnitBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.TCUnit.TCUnitWin.Visible = !DialogHosts.TCUnit.TCUnitWin.Visible;
    }

    // AoWTBWindow.pas:350
    public void StartMenuBarChange()
    {
        DialogHosts.General.PlayButtonSound(3);
    }

    // AoWTBWindow.pas:355
    public void RelationsBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var relWindow = DialogHosts.RelWindow;
        if (relWindow.RelWin.Visible)
        {
            relWindow.RelWin.Visible = false;
        }
        else
        {
            DialogHosts.RelWindow.ShowRelations(AoWHSMap.Current.FSeatedPlayer);
        }
    }

    // AoWTBWindow.pas:363
    public void ChatBtnUpdate(Button Sender)
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
        if (CombatMap.Current.FSeatedCombatPlayer > -1)
        {
            CombatMap.Current.FPlayers.GetPlayers(CombatMap.Current.FSeatedCombatPlayer).SetAuto(AutoBtn.Checked);
        }
    }

    internal static void ShowMagicWindow(MagicWindow Window)
    {
        if (!Window.Dlg.Visible)
        {
            MagicWindow.RefreshMagicWindow(Window);
            MagicWindow.RefreshGlobalMagic(Window);
        }
        Window.Dlg.Visible = true;
    }
}
