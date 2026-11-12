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
    public Frame Frame1;
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
    private int _indicatorFrame;
    private int _indicatorStep;

    public short WorldClockDelay { get; set; }

    private short _combatClockDelay;

    public short ChatFlash { get; set; }

    // AoWTBWindow.pas:122
    public void TurnButtonAnimDone()
    {
        TurnButton.Checked = false;
        if (CombatMap.Current != null)
        {
            CombatMap.Current.EndTurnPending = true;
        }
    }

    // AoWTBWindow.pas:128
    public void WMButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.ScannerWindow.MainWindow.Visible = !DialogHosts.ScannerWindow.MainWindow.Visible;
    }

    // AoWTBWindow.pas:134
    public void OVButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.RealmWindow.Dlg.Visible = !DialogHosts.RealmWindow.Dlg.Visible;
        if (DialogHosts.RealmWindow.Dlg.Visible)
        {
            DialogHosts.RealmWindow.UpdateOverview();
        }
    }

    // AoWTBWindow.pas:141
    public void ConButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.ControlWindow.MainWindow.Visible = !DialogHosts.ControlWindow.MainWindow.Visible;
    }

    // AoWTBWindow.pas:147
    public void StructButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.InfoWindow.InfoWin.Visible = !DialogHosts.InfoWindow.InfoWin.Visible;
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
        if (DialogHosts.PlayerInfoWindow.PInfoWin.Visible)
        {
            DialogHosts.PlayerInfoWindow.PInfoWin.Visible = false;
        }
        else
        {
            DialogHosts.PlayerInfoWindow.UpdatePlayer();
        }
    }

    // AoWTBWindow.pas:172
    public void ProdButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWTBWindow.pas:177
    public void COButtonClick()
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
        if (!AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).SingleProcessCycle)
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
    public void PIndDraw(LockedSurface surface)
    {
        int position;
        int race;
        position = 0;
        if (AoWHSMap.Current != null)
        {
            for (int I = 0, last = AoWHSMap.Current.Players.GetCount() - 1; I <= last; I++)
            {
                if (AoWHSMap.Current.Players.GetPlayers(I).Status == PlayerStatus.Playing)
                {
                    race = AoWHSMap.Current.Players.GetPlayers(I).PlayerRace;
                    if (race == -1)
                    {
                        race = 12;
                    }

                    DialogHosts.UIGraphics.TBWin[race + 32].ShowClipped(PInd.WinLeft + 3 + position * 5, PInd.WinTop + 2, surface);
                    if (AoWHSMap.Current.PlayMode == PlayMode.Turn)
                    {
                        if (I == AoWHSMap.Current.CurrentPlayer)
                        {
                            CurP.WinLeft = PInd.WinLeft + 2 + position * 5;
                            CurP.WinTop = PInd.WinTop + 11;
                        }
                    }
                    else
                    {
                        if (AoWHSMap.Current.Players.GetPlayers(I).SingleProcessCycle)
                        {
                            DialogHosts.UIGraphics.TBWin[50].ShowClipped(PInd.WinLeft + 2 + position * 5, PInd.WinTop + 11, surface);
                        }
                    }

                    position += 1;
                }
            }
        }
    }

    // AoWTBWindow.pas:222
    public void CurPUpdate()
    {
        _indicatorFrame += _indicatorStep;
        if (_indicatorFrame > 4)
        {
            _indicatorStep = -1;
        }

        if (_indicatorFrame < 1)
        {
            _indicatorStep = 1;
        }

        CurP.CurrentImage = _indicatorFrame + 45;
        PInd.Drawn = false;
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
        CombatMap.Current.EndTurnPending = true;
    }

    // AoWTBWindow.pas:244
    public void TCPIndDraw(LockedSurface surface)
    {
        int position;
        int race;
        position = 0;
        for (int I = 0, last = CombatMap.Current.Combat.CombatData.TurnOrder.Count - 1; I <= last; I++)
        {
            var turnOrder = CombatMap.Current.Combat.CombatData.TurnOrder[I];
            var player = CombatMap.Current.Players.GetPlayers((sbyte)turnOrder);
            race = AoWHSMap.Current.Players.GetPlayers(player.Player).PlayerRace;
            if (race == -1)
            {
                race = 12;
            }

            DialogHosts.UIGraphics.TBWin[race + 32].ShowClipped(TCPInd.WinLeft + 3 + position * 5, TCPInd.WinTop + 2, surface);
            if (CombatMap.Current.Combat.CombatData.TurnOrder[I] == CombatMap.Current.CurrentCombatPlayer)
            {
                TCCurP.WinLeft = TCPInd.WinLeft + 2 + position * 5;
                TCCurP.WinTop = TCPInd.WinTop + 11;
            }

            position += 1;
        }
    }

    // AoWTBWindow.pas:264
    public void TCCurPUpdate()
    {
        _indicatorFrame += _indicatorStep;
        if (_indicatorFrame > 4)
        {
            _indicatorStep = -1;
        }

        if (_indicatorFrame < 1)
        {
            _indicatorStep = 1;
        }

        TCCurP.CurrentImage = _indicatorFrame + 45;
        TCPInd.Drawn = false;
    }

    // AoWTBWindow.pas:273
    public void TCScannerBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.CombatScanner.TCScannerWin.Visible = !DialogHosts.CombatScanner.TCScannerWin.Visible;
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
        ushort hour = default;
        ushort minute = default;
        ushort second = default;
        ushort millisecond = default;
        if (TBWorld.Visible && AoWHSMap.Current != null)
        {
            WorldClockDelay = (short)(WorldClockDelay - 1);
            if (WorldClockDelay < 0)
            {
                if (AoWHSMap.Current.TurnTime != 0)
                {
                    WorldClockDelay = 10;
                    minute = (ushort)(AoWHSMap.Current.GetTurnTimeLeft() / 60000);
                    second = (ushort)(AoWHSMap.Current.GetTurnTimeLeft() % 60000 / 1000);
                    if (minute == 0 && second > 0 && second <= 10)
                    {
                        TimeLbl.TextColor = 255;
                    }
                    else
                    {
                        TimeLbl.TextColor = 0;
                    }

                    TimeLbl.Text = IntToStr(minute) + ":";
                    if (second < 10)
                    {
                        TimeLbl.Text = TimeLbl.Text + "0";
                    }

                    TimeLbl.Text = TimeLbl.Text + IntToStr(second);
                }
                else
                {
                    WorldClockDelay = 900;
                    DecodeTime(Time(), ref hour, ref minute, ref second, ref millisecond);
                    if (hour > 12)
                    {
                        hour = (ushort)(hour - 12);
                    }

                    if (hour == 0)
                    {
                        hour = 12;
                    }

                    TimeLbl.Text = IntToStr(hour) + ":";
                    if (minute < 10)
                    {
                        TimeLbl.Text = TimeLbl.Text + "0";
                    }

                    TimeLbl.Text = TimeLbl.Text + IntToStr(minute);
                }
            }
        }
    }

    // AoWTBWindow.pas:317
    public void TBCombatUpdate()
    {
        ushort hour = default;
        ushort minute = default;
        ushort second = default;
        ushort millisecond = default;
        if (TBCombat.Visible && CombatMap.Current != null)
        {
            _combatClockDelay = (short)(_combatClockDelay - 1);
            if (_combatClockDelay < 0)
            {
                _combatClockDelay = 900;
                DecodeTime(Time(), ref hour, ref minute, ref second, ref millisecond);
                if (hour > 12)
                {
                    hour = (ushort)(hour - 12);
                }

                if (hour == 0)
                {
                    hour = 12;
                }

                TCTimeLbl.Text = IntToStr(hour) + ":";
                if (minute < 10)
                {
                    TCTimeLbl.Text = TCTimeLbl.Text + "0";
                }

                TCTimeLbl.Text = TCTimeLbl.Text + IntToStr(minute);
            }
        }
    }

    // AoWTBWindow.pas:337
    public void OV2BtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (DialogHosts.MagicWindow.Dlg.Visible)
        {
            MagicWindow.HideMagic(DialogHosts.MagicWindow);
        }
        else
        {
            ShowMagicWindow(DialogHosts.MagicWindow);
        }
    }

    // AoWTBWindow.pas:344
    public void TCUnitBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.CombatUnitInfo.TCUnitWin.Visible = !DialogHosts.CombatUnitInfo.TCUnitWin.Visible;
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
        var relationWindow = DialogHosts.RelationWindow;
        if (relationWindow.RelWin.Visible)
        {
            relationWindow.RelWin.Visible = false;
        }
        else
        {
            DialogHosts.RelationWindow.ShowRelations(AoWHSMap.Current.SeatedPlayer);
        }
    }

    // AoWTBWindow.pas:363
    public void ChatBtnUpdate(Button sender)
    {
        if (ChatFlash > 0)
        {
            switch (ChatFlash)
            {
                case 1:
                case 31:
                case 61:
                case 91:
                case 121:
                    sender.Drawn = false;
                    sender.ImageLibraryIndexUp = 14;
                    break;
                case 16:
                case 46:
                case 76:
                case 106:
                case 136:
                    sender.Drawn = false;
                    sender.ImageLibraryIndexUp = 20;
                    break;
            }

            ChatFlash = (short)(ChatFlash - 1);
        }
    }

    // AoWTBWindow.pas:377
    public void AutoBtn2Click()
    {
        if (CombatMap.Current.SeatedCombatPlayer > -1)
        {
            CombatMap.Current.Players.GetPlayers(CombatMap.Current.SeatedCombatPlayer).SetAuto(AutoBtn.Checked);
        }
    }

    internal static void ShowMagicWindow(MagicWindow window)
    {
        if (!window.Dlg.Visible)
        {
            MagicWindow.RefreshMagicWindow(window);
            MagicWindow.RefreshGlobalMagic(window);
        }
        window.Dlg.Visible = true;
    }
}
