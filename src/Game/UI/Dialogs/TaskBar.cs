#nullable disable
// Translated from AoWTBWindow.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// indicator frame/step, world/combat clock delays, chat flash countdown
public partial class TaskBar(UIContext ui) : UIModule(ui)
{
    public Window TaskWindow { get; private set; }
    public MenuBar WorldStartMenuBar { get; private set; }
    public Edit WorldMessageEdit { get; set; }
    public Label WorldGoldIncomeLabel { get; private set; }
    public Label WorldGoldTotalLabel { get; private set; }
    public TurnButton WorldTurnButton { get; set; }
    public Button PartyButton { get; private set; }
    public Button StructureButton { get; private set; }
    public Button ChatButton { get; private set; }
    public Label WorldTimeLabel { get; private set; }
    public Frame PlayerIndicatorFrame { get; set; }
    public Image CurrentPlayerImage { get; private set; }
    public Panel WorldPanel { get; private set; }
    public MenuBar CombatStartMenuBar { get; private set; }
    public Edit CombatMessageEdit { get; private set; }
    public TurnButton CombatTurnButton { get; private set; }
    public Button CombatScannerButton { get; private set; }
    public Button CombatUnitButton { get; private set; }
    public Label CombatTimeLabel { get; private set; }
    public Frame CombatPlayerIndicatorFrame { get; private set; }
    public Image CombatCurrentPlayerImage { get; private set; }
    public Panel CombatPanel { get; private set; }
    public Button CombatChatButton { get; private set; }
    public Label WorldGoldSignLabel { get; private set; }
    public Label WorldManaIncomeLabel { get; private set; }
    public Label WorldManaTotalLabel { get; private set; }
    public Label WorldManaSignLabel { get; private set; }
    public Label CombatGoldIncomeLabel { get; private set; }
    public Label CombatGoldTotalLabel { get; private set; }
    public Label CombatGoldSignLabel { get; private set; }
    public Label CombatManaIncomeLabel { get; private set; }
    public Label CombatManaTotalLabel { get; private set; }
    public Label CombatManaSignLabel { get; private set; }
    public CheckBox AutoCombatCheckBox { get; private set; }

    private int _indicatorFrame;
    private int _indicatorStep;

    public short WorldClockDelay { get; set; }

    private short _combatClockDelay;

    public short ChatFlash { get; set; }

    // AoWTBWindow.pas:122
    public void WorldTurnButtonAnimationDone()
    {
        WorldTurnButton.Checked = false;
        CombatMap.Current?.EndTurnPending = true;
    }

    // AoWTBWindow.pas:128
    public void WMButtonClick()
    {
        UI.General.PlayButtonSound(0);
        UI.ScannerWindow.MainWindow.Visible = !UI.ScannerWindow.MainWindow.Visible;
    }

    // AoWTBWindow.pas:134
    public void OVButtonClick()
    {
        UI.General.PlayButtonSound(0);
        UI.RealmWindow.Window.Visible = !UI.RealmWindow.Window.Visible;
        if (UI.RealmWindow.Window.Visible)
        {
            UI.RealmWindow.UpdateOverview();
        }
    }

    // AoWTBWindow.pas:141
    public void ConButtonClick()
    {
        UI.General.PlayButtonSound(0);
        UI.ControlWindow.MainWindow.Visible = !UI.ControlWindow.MainWindow.Visible;
    }

    // AoWTBWindow.pas:147
    public void StructButtonClick()
    {
        UI.General.PlayButtonSound(0);
        UI.InfoWindow.MainWindow.Visible = !UI.InfoWindow.MainWindow.Visible;
    }

    // AoWTBWindow.pas:153
    public void TBNextButtonClick()
    {
        UI.General.PlayButtonSound(0);
        _ = StrategicMap.Current.SelectNextArmy();
    }

    // AoWTBWindow.pas:159
    public void UnitButtonClick()
    {
        UI.General.PlayButtonSound(0);
        UI.UnitWindow.MainWindow.Visible = true;
    }

    // AoWTBWindow.pas:165
    public void InfoBtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (UI.PlayerInfoWindow.MainWindow.Visible)
        {
            UI.PlayerInfoWindow.MainWindow.Visible = false;
        }
        else
        {
            UI.PlayerInfoWindow.UpdatePlayer();
        }
    }

    // AoWTBWindow.pas:172
    public void ProdButtonClick() => UI.General.PlayButtonSound(0);

    // AoWTBWindow.pas:177
    public void COButtonClick() => UI.General.PlayButtonSound(0);

    // AoWTBWindow.pas:182
    public void ChatBtnClick()
    {
        UI.General.PlayButtonSound(0);
        UI.ChatWindow.MainWindow.Visible = !UI.ChatWindow.MainWindow.Visible;
    }

    // AoWTBWindow.pas:188
    public void TurnButtonChange()
    {
        if (!StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).SingleProcessCycle)
        {
            bool endTurn = StrategicMap.Current.EndTurn();
            WorldTurnButton.Checked = endTurn;
        }
        else
        {
            bool cancelEndTurn = StrategicMap.Current.CancelEndTurn();
            WorldTurnButton.Checked = !cancelEndTurn;
        }
    }

    // AoWTBWindow.pas:195
    public void PIndDraw(LockedSurface surface)
    {
        int position;
        int race;
        position = 0;
        if (StrategicMap.Current != null)
        {
            for (int i = 0, last = StrategicMap.Current.Players.GetCount() - 1; i <= last; i++)
            {
                if (StrategicMap.Current.Players.GetPlayers(i).Status == PlayerStatus.Playing)
                {
                    race = StrategicMap.Current.Players.GetPlayers(i).PlayerRace;
                    if (race == -1)
                    {
                        race = 12;
                    }

                    UI.Graphics.TaskBar[race + 32].ShowClipped(PlayerIndicatorFrame.WinLeft + 3 + (position * 5), PlayerIndicatorFrame.WinTop + 2, surface);
                    if (StrategicMap.Current.PlayMode == PlayMode.Turn)
                    {
                        if (i == StrategicMap.Current.CurrentPlayer)
                        {
                            CurrentPlayerImage.WinLeft = PlayerIndicatorFrame.WinLeft + 2 + (position * 5);
                            CurrentPlayerImage.WinTop = PlayerIndicatorFrame.WinTop + 11;
                        }
                    }
                    else
                    {
                        if (StrategicMap.Current.Players.GetPlayers(i).SingleProcessCycle)
                        {
                            UI.Graphics.TaskBar[50].ShowClipped(PlayerIndicatorFrame.WinLeft + 2 + (position * 5), PlayerIndicatorFrame.WinTop + 11, surface);
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

        CurrentPlayerImage.CurrentImage = _indicatorFrame + 45;
        PlayerIndicatorFrame.Drawn = false;
    }

    // AoWTBWindow.pas:231
    public void TCTurnButtonChange()
    {
        if (CombatTurnButton.Checked)
        {
            bool endTurn = CombatMap.Current.EndTurn(false);
            CombatTurnButton.Checked = endTurn;
        }
        else
        {
            CombatMap.Current.CancelEndTurn();
        }
    }

    // AoWTBWindow.pas:237
    public void CombatTurnButtonAnimationDone()
    {
        UI.General.Manager.OverlayPriority = 0;
        CombatTurnButton.Checked = false;
        CombatMap.Current.EndTurnPending = true;
    }

    // AoWTBWindow.pas:244
    public void TCPIndDraw(LockedSurface surface)
    {
        int position;
        int race;
        position = 0;
        for (int i = 0, last = CombatMap.Current.Combat.CombatData.TurnOrder.Count - 1; i <= last; i++)
        {
            byte turnOrder = CombatMap.Current.Combat.CombatData.TurnOrder[i];
            TacticalBattlePlayer player = CombatMap.Current.Players.GetPlayers((sbyte)turnOrder);
            race = StrategicMap.Current.Players.GetPlayers(player.Player).PlayerRace;
            if (race == -1)
            {
                race = 12;
            }

            UI.Graphics.TaskBar[race + 32].ShowClipped(CombatPlayerIndicatorFrame.WinLeft + 3 + (position * 5), CombatPlayerIndicatorFrame.WinTop + 2, surface);
            if (CombatMap.Current.Combat.CombatData.TurnOrder[i] == CombatMap.Current.CurrentCombatPlayer)
            {
                CombatCurrentPlayerImage.WinLeft = CombatPlayerIndicatorFrame.WinLeft + 2 + (position * 5);
                CombatCurrentPlayerImage.WinTop = CombatPlayerIndicatorFrame.WinTop + 11;
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

        CombatCurrentPlayerImage.CurrentImage = _indicatorFrame + 45;
        CombatPlayerIndicatorFrame.Drawn = false;
    }

    // AoWTBWindow.pas:273
    public void TCScannerBtnClick()
    {
        UI.General.PlayButtonSound(0);
        UI.CombatScanner.CombatScannerWindow.Visible = !UI.CombatScanner.CombatScannerWindow.Visible;
    }

    // AoWTBWindow.pas:279
    public void TCNextBtnClick()
    {
        UI.General.PlayButtonSound(0);
        CombatMap.Current.SelectNextUnit(true);
    }

    // AoWTBWindow.pas:285
    public void TBWorldUpdate()
    {
        if (WorldPanel.Visible && StrategicMap.Current != null)
        {
            WorldClockDelay = (short)(WorldClockDelay - 1);
            if (WorldClockDelay < 0)
            {
                ushort minute;
                ushort second;
                if (StrategicMap.Current.TurnTime != 0)
                {
                    WorldClockDelay = 10;
                    minute = (ushort)(StrategicMap.Current.GetTurnTimeLeft() / 60000);
                    second = (ushort)(StrategicMap.Current.GetTurnTimeLeft() % 60000 / 1000);
                    WorldTimeLabel.TextColor = minute == 0 && second > 0 && second <= 10 ? 255 : 0;

                    WorldTimeLabel.Text = IntToStr(minute) + ":";
                    if (second < 10)
                    {
                        WorldTimeLabel.Text = WorldTimeLabel.Text + "0";
                    }

                    WorldTimeLabel.Text = WorldTimeLabel.Text + IntToStr(second);
                }
                else
                {
                    WorldClockDelay = 900;
                    DecodeTime(Time(), out ushort hour, out minute, out _, out _);
                    if (hour > 12)
                    {
                        hour = (ushort)(hour - 12);
                    }

                    if (hour == 0)
                    {
                        hour = 12;
                    }

                    WorldTimeLabel.Text = IntToStr(hour) + ":";
                    if (minute < 10)
                    {
                        WorldTimeLabel.Text = WorldTimeLabel.Text + "0";
                    }

                    WorldTimeLabel.Text = WorldTimeLabel.Text + IntToStr(minute);
                }
            }
        }
    }

    // AoWTBWindow.pas:317
    public void TBCombatUpdate()
    {
        if (CombatPanel.Visible && CombatMap.Current != null)
        {
            _combatClockDelay = (short)(_combatClockDelay - 1);
            if (_combatClockDelay < 0)
            {
                _combatClockDelay = 900;
                DecodeTime(Time(), out ushort hour, out ushort minute, out _, out _);
                if (hour > 12)
                {
                    hour = (ushort)(hour - 12);
                }

                if (hour == 0)
                {
                    hour = 12;
                }

                CombatTimeLabel.Text = IntToStr(hour) + ":";
                if (minute < 10)
                {
                    CombatTimeLabel.Text = CombatTimeLabel.Text + "0";
                }

                CombatTimeLabel.Text = CombatTimeLabel.Text + IntToStr(minute);
            }
        }
    }

    // AoWTBWindow.pas:337
    public void OV2BtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (UI.MagicWindow.Window.Visible)
        {
            MagicWindow.HideMagic(UI.MagicWindow);
        }
        else
        {
            ShowMagicWindow(UI.MagicWindow);
        }
    }

    // AoWTBWindow.pas:344
    public void TCUnitBtnClick()
    {
        UI.General.PlayButtonSound(0);
        UI.CombatUnitInfo.MainWindow.Visible = !UI.CombatUnitInfo.MainWindow.Visible;
    }

    // AoWTBWindow.pas:350
    public void StartMenuBarChange() => UI.General.PlayButtonSound(3);

    // AoWTBWindow.pas:355
    public void RelationsBtnClick()
    {
        UI.General.PlayButtonSound(0);
        RelationWindow relationWindow = UI.RelationWindow;
        if (relationWindow.MainWindow.Visible)
        {
            relationWindow.MainWindow.Visible = false;
        }
        else
        {
            UI.RelationWindow.ShowRelations(StrategicMap.Current.SeatedPlayer);
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
    public void AutoCombatCheckBoxChange()
    {
        if (CombatMap.Current.SeatedCombatPlayer > -1)
        {
            CombatMap.Current.Players.GetPlayers(CombatMap.Current.SeatedCombatPlayer).SetAuto(AutoCombatCheckBox.Checked);
        }
    }

    internal static void ShowMagicWindow(MagicWindow window)
    {
        if (!window.Window.Visible)
        {
            MagicWindow.RefreshMagicWindow(window);
            MagicWindow.RefreshGlobalMagic(window);
        }
        window.Window.Visible = true;
    }
}
