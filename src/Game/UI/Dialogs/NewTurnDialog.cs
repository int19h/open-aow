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

    public Window NewTDlg;
    public Label ClickMsg;
    public Image LShield;
    public Label PlayerName;
    public Label TurnMsg;
    public Label PlayerRace;
    public Label Turn;
    public Image RShield;
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
                TurnMsg.Text = eventValue.Text;
                var name = AoWHSMap.Current.Players.GetPlayers(eventValue.TurnPlayer).GetName();
                PlayerName.Text = name;
                if (eventValue.TurnPlayer != 0)
                {
                    var player = AoWHSMap.Current.Players.GetPlayers(eventValue.TurnPlayer);
                    var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[player.PlayerRace]);
                    PlayerRace.Text = raceNamesPlural;
                }
                else
                {
                    PlayerRace.Text = "";
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
                Turn.Text = day + " " + IntToStr(Day);
                race = AoWHSMap.Current.Players.GetPlayers(eventValue.TurnPlayer).PlayerRace;
                if (race == -1)
                {
                    race = 12;
                }

                LShield.CurrentImage = race + 26;
                RShield.CurrentImage = race + 26;
                NewTDlg.Drawn = false;
                NewTDlg.Visible = true;
                if (_newTurn.DialogStyle.Contains(NewTurnEventLogDialogStyleElement.Closeable))
                {
                    var clickToBegin = AbilityRules.TranslateResourceString(s_clickToBeginText);
                    ClickMsg.Text = clickToBegin;
                }
                else
                {
                    var hiddenMovementPleaseWait = AbilityRules.TranslateResourceString(s_hiddenMovementPleaseWaitText);
                    ClickMsg.Text = hiddenMovementPleaseWait;
                }

                break;
            case NewTurnEventLogMode.Close:
                NewTDlg.Visible = false;
                break;
        }
    }

    // AoWNewTurnDialog.pas:106
    public void NewTDlgMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (CombatMap.Current == null && _newTurn.DialogStyle.Contains(NewTurnEventLogDialogStyleElement.Closeable))
        {
            NewTDlg.Visible = false;
        }
    }

    // AoWNewTurnDialog.pas:111
    public void NewTDlgKeyPress(ref byte key)
    {
        if (_newTurn.DialogStyle.Contains(NewTurnEventLogDialogStyleElement.Closeable) && (key == 32 || key == 13 || key == 27))
        {
            NewTDlg.Visible = false;
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
                R = new Rect(NewTDlg.WinLeft + 188, NewTDlg.WinTop + 40, NewTDlg.WinLeft + 188 + 103, NewTDlg.WinTop + 40 + 128);
                AoWHSMap.Current.Players.GetPlayers(CombatMap.Current.SeatedPlayer).Leader.ShowFace(R, surface);
            }
        }
        else
        {
            if (AoWHSMap.Current.Players.GetPlayers(_newTurn.TurnPlayer).Leader != null)
            {
                R = new Rect(NewTDlg.WinLeft + 188, NewTDlg.WinTop + 40, NewTDlg.WinLeft + 188 + 103, NewTDlg.WinTop + 40 + 128);
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
