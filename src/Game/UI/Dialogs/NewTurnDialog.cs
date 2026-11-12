// Translated from AoWNewTurnDialog.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// done callback, new-turn event
public partial class NewTurnDialog : DataModule
{
    public NewTurnDialog(Component owner) : base(owner)
    { }

    public Window Window;
    public Label ClickMessageLabel;
    public Image LeftShield;
    public Label PlayerNameLabel;
    public Label TurnMessageLabel;
    public Label PlayerRaceLabel;
    public Label TurnLabel;
    public Image RightShield;
    private ExecuteDoneEvent _done;
    private NewTurnEventLog _newTurn;

    // AoWNewTurnDialog.pas:50
    public void FinishDialog()
    {
        ExecuteDoneEvent done;
        NewTurnEventLog eventValue;
        DialogHosts.General.Manager.OverlayPriority = 0;
        if (_newTurn == null)
        {
            AoWHSMap.Current.SetSeatedPlayer(AoWHSMap.Current.CurrentPlayer);
            if (AoWHSMap.Current.SeatedPlayer > -1)
            {
                DialogHosts.PlayerInfoWindow.UpdateEvents();
            }
        }

        done = _done;
        eventValue = _newTurn;
        _done = null;
        _newTurn = null;
        if (done != null)
        {
            done(eventValue);
        }

        if (eventValue != null)
        {
            eventValue.Release();
        }
    }

    // AoWNewTurnDialog.pas:69
    public void ShowNewTurn(NewTurnEventLog eventValue, ExecuteDoneEvent done)
    {
        sbyte race;
        int Day;
        switch (eventValue.Mode)
        {
            case NewTurnEventLogMode.Dialog:
                if (_newTurn != null)
                {
                    _newTurn.Release();
                    _newTurn = null;
                }

                _done = done;
                _newTurn = eventValue;
                _newTurn.AddRef();
                TurnMessageLabel.Text = eventValue.Text;
                var name = AoWHSMap.Current.Players.GetPlayers(eventValue.TurnPlayer).GetName();
                PlayerNameLabel.Text = name;
                if (eventValue.TurnPlayer != 0)
                {
                    var player = AoWHSMap.Current.Players.GetPlayers(eventValue.TurnPlayer);
                    var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[player.PlayerRace]);
                    PlayerRaceLabel.Text = raceNamesPlural;
                }
                else
                {
                    PlayerRaceLabel.Text = "";
                }

                if (AoWHSMap.Current.Day > 0)
                {
                    Day = AoWHSMap.Current.Day;
                }
                else
                {
                    Day = 1;
                }

                var day = AbilityRules.TranslateResourceString(GameText.DayText);
                TurnLabel.Text = day + " " + IntToStr(Day);
                race = AoWHSMap.Current.Players.GetPlayers(eventValue.TurnPlayer).PlayerRace;
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
                    var clickToBegin = AbilityRules.TranslateResourceString(s_clickToBeginText);
                    ClickMessageLabel.Text = clickToBegin;
                }
                else
                {
                    var hiddenMovementPleaseWait = AbilityRules.TranslateResourceString(s_hiddenMovementPleaseWaitText);
                    ClickMessageLabel.Text = hiddenMovementPleaseWait;
                }

                break;
            case NewTurnEventLogMode.Close:
                Window.Visible = false;
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
        Rect R;
        if (CombatMap.Current != null)
        {
            if (AoWHSMap.Current.Players.GetPlayers(CombatMap.Current.SeatedPlayer).Leader != null)
            {
                R = new Rect(Window.WinLeft + 188, Window.WinTop + 40, Window.WinLeft + 188 + 103, Window.WinTop + 40 + 128);
                AoWHSMap.Current.Players.GetPlayers(CombatMap.Current.SeatedPlayer).Leader.ShowFace(R, surface);
            }
        }
        else
        {
            if (AoWHSMap.Current.Players.GetPlayers(_newTurn.TurnPlayer).Leader != null)
            {
                R = new Rect(Window.WinLeft + 188, Window.WinTop + 40, Window.WinLeft + 188 + 103, Window.WinTop + 40 + 128);
                AoWHSMap.Current.Players.GetPlayers(_newTurn.TurnPlayer).Leader.ShowFace(R, surface);
            }
        }
    }

    // AoWNewTurnDialog.pas:136
    public void NewTDlgHide()
    {
        FinishDialog();
    }

    private static readonly string s_clickToBeginText = "Click to begin";
    private static readonly string s_hiddenMovementPleaseWaitText = "Hidden movement, please wait";
}
