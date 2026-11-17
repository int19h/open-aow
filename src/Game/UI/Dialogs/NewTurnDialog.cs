#nullable disable
// Translated from AoWNewTurnDialog.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// done callback, new-turn event
public partial class NewTurnDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Label ClickMessageLabel { get; private set; }
    public Image LeftShield { get; private set; }
    public Label PlayerNameLabel { get; private set; }
    public Label TurnMessageLabel { get; private set; }
    public Label PlayerRaceLabel { get; private set; }
    public Label TurnLabel { get; private set; }
    public Image RightShield { get; private set; }

    private event ExecuteDoneEvent Done;
    private NewTurnEventLog _newTurn;

    // AoWNewTurnDialog.pas:50
    public void FinishDialog()
    {
        ExecuteDoneEvent done;
        NewTurnEventLog eventValue;
        UI.General.Manager.OverlayPriority = 0;
        if (_newTurn == null)
        {
            StrategicMap.Current.SetSeatedPlayer(StrategicMap.Current.CurrentPlayer);
            if (StrategicMap.Current.SeatedPlayer > -1)
            {
                UI.PlayerInfoWindow.UpdateEvents();
            }
        }

        done = Done;
        eventValue = _newTurn;
        Done = null;
        _newTurn = null;
        done?.Invoke(eventValue);

        _ = (eventValue?.Release());
    }

    // AoWNewTurnDialog.pas:69
    public void ShowNewTurn(NewTurnEventLog eventValue, ExecuteDoneEvent done)
    {
        sbyte race;
        int day2;
        switch (eventValue.Mode)
        {
            case NewTurnEventLogMode.Dialog:
                _ = (_newTurn?.Release());
                _newTurn = null;

                Done = done;
                _newTurn = eventValue;
                _ = _newTurn.AddRef();
                TurnMessageLabel.Text = eventValue.Text;
                string name = StrategicMap.Current.Players.GetPlayers(eventValue.TurnPlayer).GetName();
                PlayerNameLabel.Text = name;
                if (eventValue.TurnPlayer != 0)
                {
                    Player player = StrategicMap.Current.Players.GetPlayers(eventValue.TurnPlayer);
                    string raceNamesPlural = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[player.PlayerRace]);
                    PlayerRaceLabel.Text = raceNamesPlural;
                }
                else
                {
                    PlayerRaceLabel.Text = "";
                }

                day2 = StrategicMap.Current.Day > 0 ? StrategicMap.Current.Day : 1;

                string day = UI.Engine.TranslateResourceString(GameText.DayText);
                TurnLabel.Text = day + " " + IntToStr(day2);
                race = StrategicMap.Current.Players.GetPlayers(eventValue.TurnPlayer).PlayerRace;
                if (race == -1)
                {
                    race = 12;
                }

                LeftShield.CurrentImage = race + 26;
                RightShield.CurrentImage = race + 26;
                Window.Drawn = false;
                Window.Visible = true;
                if (_newTurn.DialogStyle.Contains(NewTurnEventLogDialogStyleElement.Closeable))
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
    public void NewTDlgMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (CombatMap.Current == null && _newTurn.DialogStyle.Contains(NewTurnEventLogDialogStyleElement.Closeable))
        {
            Window.Visible = false;
        }
    }

    // AoWNewTurnDialog.pas:111
    public void NewTDlgKeyPress(ref byte key)
    {
        if (_newTurn.DialogStyle.Contains(NewTurnEventLogDialogStyleElement.Closeable) && (key == 32 || key == 13 || key == 27))
        {
            Window.Visible = false;
        }
    }

    // AoWNewTurnDialog.pas:116
    public void FaceFramePreDraw(LockedSurface surface)
    {
        Rect r;
        if (CombatMap.Current != null)
        {
            if (StrategicMap.Current.Players.GetPlayers(CombatMap.Current.SeatedPlayer).Leader != null)
            {
                r = new Rect(Window.WinLeft + 188, Window.WinTop + 40, Window.WinLeft + 188 + 103, Window.WinTop + 40 + 128);
                StrategicMap.Current.Players.GetPlayers(CombatMap.Current.SeatedPlayer).Leader.ShowFace(r, surface);
            }
        }
        else
        {
            if (StrategicMap.Current.Players.GetPlayers(_newTurn.TurnPlayer).Leader != null)
            {
                r = new Rect(Window.WinLeft + 188, Window.WinTop + 40, Window.WinLeft + 188 + 103, Window.WinTop + 40 + 128);
                StrategicMap.Current.Players.GetPlayers(_newTurn.TurnPlayer).Leader.ShowFace(r, surface);
            }
        }
    }

    // AoWNewTurnDialog.pas:136
    public void NewTDlgHide() => FinishDialog();

    private static readonly string s_clickToBeginText = "Click to begin";
    private static readonly string s_hiddenMovementPleaseWaitText = "Hidden movement, please wait";
}
