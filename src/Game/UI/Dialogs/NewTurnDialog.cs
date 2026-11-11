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
    public Frame TCeltB;
    public Label ClickMsg;
    public Image LShield;
    public Label PlayerName;
    public Label TurnMsg;
    public Label PlayerRace;
    public Label Turn;
    public Image RShield;
    public Frame BlueFr;
    public Image TCeltLineL;
    public Image TCeltLineR;
    public Image FaceFrame;
    public ExecuteDoneEvent FDone;
    public NewTurnEventLog FNewTurn;

    // AoWNewTurnDialog.pas:50
    public void FinishDialog()
    {
        ExecuteDoneEvent Done;
        NewTurnEventLog Event;
        DialogHosts.General.Manager.OverlayPriority = 0;
        if (FNewTurn == null)
        {
            AoWHSMap.Current.SetSeatedPlayer(AoWHSMap.Current.FCurrentPlayer);
            if (AoWHSMap.Current.FSeatedPlayer > -1)
            {
                DialogHosts.PInfoW.UpdateEvents();
            }
        }

        Done = FDone;
        Event = FNewTurn;
        FDone = null;
        FNewTurn = null;
        if (Done != null)
        {
            Done(Event);
        }

        if (Event != null)
        {
            Event.Release();
        }
    }

    // AoWNewTurnDialog.pas:69
    public void ShowNewTurn(NewTurnEventLog Event, ExecuteDoneEvent Done)
    {
        sbyte Race;
        int Day;
        switch (Event.FMode)
        {
            case NewTurnEventLog.Mode.Dialog:
                if (FNewTurn != null)
                {
                    FNewTurn.Release();
                    FNewTurn = null;
                }

                FDone = Done;
                FNewTurn = Event;
                FNewTurn.AddRef();
                TurnMsg.Text = Event.FText;
                var name = AoWHSMap.Current.FPlayers.GetPlayers(Event.FTurnPlayer).GetName();
                PlayerName.Text = name;
                if (Event.FTurnPlayer != 0)
                {
                    var player = AoWHSMap.Current.FPlayers.GetPlayers(Event.FTurnPlayer);
                    var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[player.FPlayerRace]);
                    PlayerRace.Text = raceNamesPlural;
                }
                else
                {
                    PlayerRace.Text = "";
                }

                if (AoWHSMap.Current.FDay > 0)
                {
                    Day = AoWHSMap.Current.FDay;
                }
                else
                {
                    Day = 1;
                }

                var day = AbilityRules.TranslateRStr(GameText.DayRStr);
                Turn.Text = day + " " + IntToStr(Day);
                Race = AoWHSMap.Current.FPlayers.GetPlayers(Event.FTurnPlayer).FPlayerRace;
                if (Race == -1)
                {
                    Race = 12;
                }

                LShield.CurrentImage = Race + 26;
                RShield.CurrentImage = Race + 26;
                NewTDlg.FDrawn = false;
                NewTDlg.Visible = true;
                if (FNewTurn.FDialogStyle.Contains(NewTurnEventLogDialogStyleElement.Closeable))
                {
                    var clickToBegin = AbilityRules.TranslateRStr(ClickToBeginRStr);
                    ClickMsg.Text = clickToBegin;
                }
                else
                {
                    var hiddenmovementPleaseWait = AbilityRules.TranslateRStr(HiddenmovementPleaseWaitRStr);
                    ClickMsg.Text = hiddenmovementPleaseWait;
                }

                break;
            case NewTurnEventLog.Mode.Close:
                NewTDlg.Visible = false;
                break;
        }
    }

    // AoWNewTurnDialog.pas:106
    public void NewTDlgMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (CombatMap.Current == null && FNewTurn.FDialogStyle.Contains(NewTurnEventLogDialogStyleElement.Closeable))
        {
            NewTDlg.Visible = false;
        }
    }

    // AoWNewTurnDialog.pas:111
    public void NewTDlgKeyPress(ref byte Key)
    {
        if (FNewTurn.FDialogStyle.Contains(NewTurnEventLogDialogStyleElement.Closeable) && (Key == 32 || Key == 13 || Key == 27))
        {
            NewTDlg.Visible = false;
        }
    }

    // AoWNewTurnDialog.pas:116
    public void FaceFramePreDraw(LockedSurface Surface)
    {
        Rect R;
        if (CombatMap.Current != null)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(CombatMap.Current.FSeatedPlayer).FLeader != null)
            {
                R = new Rect(NewTDlg.WinLeft + 188, NewTDlg.WinTop + 40, NewTDlg.WinLeft + 188 + 103, NewTDlg.WinTop + 40 + 128);
                AoWHSMap.Current.FPlayers.GetPlayers(CombatMap.Current.FSeatedPlayer).FLeader.ShowFace(R, Surface);
            }
        }
        else
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(FNewTurn.FTurnPlayer).FLeader != null)
            {
                R = new Rect(NewTDlg.WinLeft + 188, NewTDlg.WinTop + 40, NewTDlg.WinLeft + 188 + 103, NewTDlg.WinTop + 40 + 128);
                AoWHSMap.Current.FPlayers.GetPlayers(FNewTurn.FTurnPlayer).FLeader.ShowFace(R, Surface);
            }
        }
    }

    // AoWNewTurnDialog.pas:136
    public void NewTDlgHide()
    {
        FinishDialog();
    }

    private static readonly string ClickToBeginRStr = "Click to begin";
    private static readonly string HiddenmovementPleaseWaitRStr = "Hidden movement, please wait";
}
