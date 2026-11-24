#nullable disable warnings
// Translated from AoWNewTurnDialog.pas.
using System.Globalization;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// done callback, new-turn event
public sealed partial class NewTurnDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Label ClickMessageLabel { get; private set; }
    public Image LeftShield { get; private set; }
    public Label PlayerNameLabel { get; private set; }
    public Label TurnMessageLabel { get; private set; }
    public Label PlayerRaceLabel { get; private set; }
    public Label TurnLabel { get; private set; }
    public Image RightShield { get; private set; }

    private ExecuteDoneEvent? _done;
    private NewTurnEventLog? _newTurn;

    // AoWNewTurnDialog.pas:50
    public void FinishDialog()
    {
        UI.General.Manager.OverlayPriority = 0;
        if (_newTurn is null)
        {
            StrategicMap.Current.SetSeatedPlayer(StrategicMap.Current.CurrentPlayer);
            if (PlayerIdExtensions.ToOriginal(StrategicMap.Current.SeatedPlayer) > -1)
            {
                UI.PlayerInfoWindow.UpdateEvents();
            }
        }

        ExecuteDoneEvent? done = _done;
        NewTurnEventLog? eventValue = _newTurn;
        _done = null;
        _newTurn = null;
        done?.Invoke(eventValue);

        _ = (eventValue?.Release());
    }

    // AoWNewTurnDialog.pas:69
    public void ShowNewTurn(NewTurnEventLog eventValue, ExecuteDoneEvent done)
    {
        switch (eventValue.Mode)
        {
            case NewTurnEventLogMode.Dialog:
                _ = (_newTurn?.Release());
                _newTurn = null;

                _done = done;
                _newTurn = eventValue;
                _ = _newTurn.AddRef();
                TurnMessageLabel.Text = eventValue.Text;
                string name = StrategicMap.Current.Players.GetPlayers(eventValue.TurnPlayer).GetName();
                PlayerNameLabel.Text = name;
                if (eventValue.TurnPlayer != PlayerId.Independent)
                {
                    Player? player = StrategicMap.Current.Players.GetPlayers(eventValue.TurnPlayer);
                    string raceNamesPlural = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[player.PlayerRace]);
                    PlayerRaceLabel.Text = raceNamesPlural;
                }
                else
                {
                    PlayerRaceLabel.Text = "";
                }

                int day2 = StrategicMap.Current.Day > 0 ? StrategicMap.Current.Day : 1;

                string day = UI.Engine.TranslateResourceString(GameText.DayText);
                TurnLabel.Text = $"{day} {day2.ToString(CultureInfo.InvariantCulture)}";
                int race = StrategicMap.Current.Players.GetPlayers(eventValue.TurnPlayer).PlayerRace;
                if (race == -1)
                {
                    race = 12;
                }

                LeftShield.CurrentImage = race + 26;
                RightShield.CurrentImage = race + 26;
                Window.IsDrawn = false;
                Window.Visible = true;
                if (_newTurn.DialogStyle.Contains(NewTurnEventLogDialogStyles.Closeable))
                {
                    string clickToBegin = UI.Engine.TranslateResourceString(s_clickToBeginText);
                    ClickMessageLabel.Text = clickToBegin;
                }
                else
                {
                    string hiddenMovementPleaseWait = UI.Engine.TranslateResourceString(s_hiddenMovementPleaseWaitText);
                    ClickMessageLabel.Text = hiddenMovementPleaseWait;
                }

                break;
            case NewTurnEventLogMode.Close:
                Window.Visible = false;
                break;
            default:
                break;
        }
    }

    // AoWNewTurnDialog.pas:106
    public void NewTDlgMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (CombatMap.Current is null && _newTurn.DialogStyle.Contains(NewTurnEventLogDialogStyles.Closeable))
        {
            Window.Visible = false;
        }
    }

    // AoWNewTurnDialog.pas:111
    public void NewTDlgKeyPress(in byte key)
    {
        if (_newTurn.DialogStyle.Contains(NewTurnEventLogDialogStyles.Closeable) && (key is 32 or 13 or 27))
        {
            Window.Visible = false;
        }
    }

    // AoWNewTurnDialog.pas:116
    public void FaceFramePreDraw(LockedSurface surface)
    {
        Rect r;
        if (CombatMap.Current is not null)
        {
            if (StrategicMap.Current.Players.GetPlayers(CombatMap.Current.SeatedPlayer).Leader is not null)
            {
                r = new Rect(Window.WinLeft + 188, Window.WinTop + 40, Window.WinLeft + 188 + 103, Window.WinTop + 40 + 128);
                StrategicMap.Current.Players.GetPlayers(CombatMap.Current.SeatedPlayer).Leader.ShowFace(r, lockedSurface: surface);
            }
        }
        else
        {
            if (StrategicMap.Current.Players.GetPlayers(_newTurn.TurnPlayer).Leader is not null)
            {
                r = new Rect(Window.WinLeft + 188, Window.WinTop + 40, Window.WinLeft + 188 + 103, Window.WinTop + 40 + 128);
                StrategicMap.Current.Players.GetPlayers(_newTurn.TurnPlayer).Leader.ShowFace(r, lockedSurface: surface);
            }
        }
    }

    // AoWNewTurnDialog.pas:136
    public void NewTDlgHide() => FinishDialog();

    private static readonly string s_clickToBeginText = "Click to begin";
    private static readonly string s_hiddenMovementPleaseWaitText = "Hidden movement, please wait";
}
