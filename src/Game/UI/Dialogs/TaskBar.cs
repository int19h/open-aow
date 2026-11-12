// Translated from AoWTBWindow.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// indicator frame/step, world/combat clock delays, chat flash countdown
public partial class TaskBar : DataModule
{
    public TaskBar(Component owner) : base(owner)
    { }

    public Window TaskWindow;
    public MenuBar WorldStartMenuBar;
    public Edit WorldMessageEdit;
    public Label WorldGoldIncomeLabel;
    public Label WorldGoldTotalLabel;
    public TurnButton WorldTurnButton;
    public Button PartyButton;
    public Button StructureButton;
    public Button ChatButton;
    public Label WorldTimeLabel;
    public Frame PlayerIndicatorFrame;
    public Image CurrentPlayerImage;
    public Panel WorldPanel;
    public MenuBar CombatStartMenuBar;
    public Edit CombatMessageEdit;
    public TurnButton CombatTurnButton;
    public Button CombatScannerButton;
    public Button CombatUnitButton;
    public Label CombatTimeLabel;
    public Frame CombatPlayerIndicatorFrame;
    public Image CombatCurrentPlayerImage;
    public Panel CombatPanel;
    public Button CombatChatButton;
    public Label WorldGoldSignLabel;
    public Label WorldManaIncomeLabel;
    public Label WorldManaTotalLabel;
    public Label WorldManaSignLabel;
    public Label CombatGoldIncomeLabel;
    public Label CombatGoldTotalLabel;
    public Label CombatGoldSignLabel;
    public Label CombatManaIncomeLabel;
    public Label CombatManaTotalLabel;
    public Label CombatManaSignLabel;
    public CheckBox AutoCombatCheckBox;
    private int _indicatorFrame;
    private int _indicatorStep;

    public short WorldClockDelay { get; set; }

    private short _combatClockDelay;

    public short ChatFlash { get; set; }

    // AoWTBWindow.pas:122
    public void WorldTurnButtonAnimationDone()
    {
        WorldTurnButton.Checked = false;
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
        DialogHosts.RealmWindow.Window.Visible = !DialogHosts.RealmWindow.Window.Visible;
        if (DialogHosts.RealmWindow.Window.Visible)
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
        DialogHosts.InfoWindow.MainWindow.Visible = !DialogHosts.InfoWindow.MainWindow.Visible;
    }

    // AoWTBWindow.pas:153
    public void TBNextButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        StrategicMap.Current.SelectNextArmy();
    }

    // AoWTBWindow.pas:159
    public void UnitButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.UnitWindow.MainWindow.Visible = true;
    }

    // AoWTBWindow.pas:165
    public void InfoBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (DialogHosts.PlayerInfoWindow.MainWindow.Visible)
        {
            DialogHosts.PlayerInfoWindow.MainWindow.Visible = false;
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
        DialogHosts.ChatWindow.MainWindow.Visible = !DialogHosts.ChatWindow.MainWindow.Visible;
    }

    // AoWTBWindow.pas:188
    public void TurnButtonChange()
    {
        if (!StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).SingleProcessCycle)
        {
            var endTurn = StrategicMap.Current.EndTurn();
            WorldTurnButton.Checked = endTurn;
        }
        else
        {
            var cancelEndTurn = StrategicMap.Current.CancelEndTurn();
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
            for (int I = 0, last = StrategicMap.Current.Players.GetCount() - 1; I <= last; I++)
            {
                if (StrategicMap.Current.Players.GetPlayers(I).Status == PlayerStatus.Playing)
                {
                    race = StrategicMap.Current.Players.GetPlayers(I).PlayerRace;
                    if (race == -1)
                    {
                        race = 12;
                    }

                    DialogHosts.UIGraphics.TaskBar[race + 32].ShowClipped(PlayerIndicatorFrame.WinLeft + 3 + position * 5, PlayerIndicatorFrame.WinTop + 2, surface);
                    if (StrategicMap.Current.PlayMode == PlayMode.Turn)
                    {
                        if (I == StrategicMap.Current.CurrentPlayer)
                        {
                            CurrentPlayerImage.WinLeft = PlayerIndicatorFrame.WinLeft + 2 + position * 5;
                            CurrentPlayerImage.WinTop = PlayerIndicatorFrame.WinTop + 11;
                        }
                    }
                    else
                    {
                        if (StrategicMap.Current.Players.GetPlayers(I).SingleProcessCycle)
                        {
                            DialogHosts.UIGraphics.TaskBar[50].ShowClipped(PlayerIndicatorFrame.WinLeft + 2 + position * 5, PlayerIndicatorFrame.WinTop + 11, surface);
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
            var endTurn = CombatMap.Current.EndTurn(false);
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
        DialogHosts.General.Manager.OverlayPriority = 0;
        CombatTurnButton.Checked = false;
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
            race = StrategicMap.Current.Players.GetPlayers(player.Player).PlayerRace;
            if (race == -1)
            {
                race = 12;
            }

            DialogHosts.UIGraphics.TaskBar[race + 32].ShowClipped(CombatPlayerIndicatorFrame.WinLeft + 3 + position * 5, CombatPlayerIndicatorFrame.WinTop + 2, surface);
            if (CombatMap.Current.Combat.CombatData.TurnOrder[I] == CombatMap.Current.CurrentCombatPlayer)
            {
                CombatCurrentPlayerImage.WinLeft = CombatPlayerIndicatorFrame.WinLeft + 2 + position * 5;
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
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.CombatScanner.CombatScannerWindow.Visible = !DialogHosts.CombatScanner.CombatScannerWindow.Visible;
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
        if (WorldPanel.Visible && StrategicMap.Current != null)
        {
            WorldClockDelay = (short)(WorldClockDelay - 1);
            if (WorldClockDelay < 0)
            {
                if (StrategicMap.Current.TurnTime != 0)
                {
                    WorldClockDelay = 10;
                    minute = (ushort)(StrategicMap.Current.GetTurnTimeLeft() / 60000);
                    second = (ushort)(StrategicMap.Current.GetTurnTimeLeft() % 60000 / 1000);
                    if (minute == 0 && second > 0 && second <= 10)
                    {
                        WorldTimeLabel.TextColor = 255;
                    }
                    else
                    {
                        WorldTimeLabel.TextColor = 0;
                    }

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
                    DecodeTime(Time(), ref hour, ref minute, ref second, ref millisecond);
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
        ushort hour = default;
        ushort minute = default;
        ushort second = default;
        ushort millisecond = default;
        if (CombatPanel.Visible && CombatMap.Current != null)
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
        DialogHosts.General.PlayButtonSound(0);
        if (DialogHosts.MagicWindow.Window.Visible)
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
        DialogHosts.CombatUnitInfo.MainWindow.Visible = !DialogHosts.CombatUnitInfo.MainWindow.Visible;
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
        if (relationWindow.MainWindow.Visible)
        {
            relationWindow.MainWindow.Visible = false;
        }
        else
        {
            DialogHosts.RelationWindow.ShowRelations(StrategicMap.Current.SeatedPlayer);
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
