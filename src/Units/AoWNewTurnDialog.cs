// Translated from AoWNewTurnDialog.pas.
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// done callback, new-turn event
public partial class NewTurnDlg : DataModule
{
    public NewTurnDlg(global::Aow.Rtl.Component owner) : base(owner)
    { }

    public AOWWindow NewTDlg;
    public AoWFrame TCeltB;
    public AOWLabel ClickMsg;
    public AOWImage LShield;
    public AOWLabel PlayerName;
    public AOWLabel TurnMsg;
    public AOWLabel PlayerRace;
    public AOWLabel Turn;
    public AOWImage RShield;
    public AoWFrame BlueFr;
    public AOWImage TCeltLineL;
    public AOWImage TCeltLineR;
    public AOWImage FaceFrame;
    public ExecuteDoneEvent FDone;
    public NewTurnEventLog FNewTurn;

    // AoWNewTurnDialog.pas:50
    public void FinishDialog()
    {
        ExecuteDoneEvent Done;
        NewTurnEventLog Event;
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 0;
        if (FNewTurn == null)
        {
            Aow.Game.World.Map.AoWHSMap.Current.SetSeatedPlayer(Aow.Game.World.Map.AoWHSMap.Current.FCurrentPlayer);
            if (Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer > -1)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.UpdateEvents();
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
            case Aow.Game.Events.NewTurnEventLog.Mode.Dialog:
                if (FNewTurn != null)
                {
                    FNewTurn.Release();
                    FNewTurn = null;
                }

                FDone = Done;
                FNewTurn = Event;
                FNewTurn.AddRef();
                TurnMsg.Text = Event.FText;
                var name = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Event.FTurnPlayer).GetName();
                PlayerName.Text = name;
                if (Event.FTurnPlayer != 0)
                {
                    var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Event.FTurnPlayer);
                    var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.RaceNamesPluralRStr[player.FPlayerRace]);
                    PlayerRace.Text = raceNamesPlural;
                }
                else
                {
                    PlayerRace.Text = "";
                }

                if (Aow.Game.World.Map.AoWHSMap.Current.FDay > 0)
                {
                    Day = Aow.Game.World.Map.AoWHSMap.Current.FDay;
                }
                else
                {
                    Day = 1;
                }

                var day = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.DayRStr);
                Turn.Text = day + " " + IntToStr(Day);
                Race = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Event.FTurnPlayer).FPlayerRace;
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
                    var clickToBegin = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.NewTurnDlg.ClickToBeginRStr);
                    ClickMsg.Text = clickToBegin;
                }
                else
                {
                    var hiddenmovementPleaseWait = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.NewTurnDlg.HiddenmovementPleaseWaitRStr);
                    ClickMsg.Text = hiddenmovementPleaseWait;
                }

                break;
            case Aow.Game.Events.NewTurnEventLog.Mode.Close:
                NewTDlg.Visible = false;
                break;
        }
    }

    // AoWNewTurnDialog.pas:106
    public void NewTDlgMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (Aow.Game.Combat.AoWCombatMap.Current == null && FNewTurn.FDialogStyle.Contains(NewTurnEventLogDialogStyleElement.Closeable))
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
    public void FaceFramePreDraw(SurfaceDesc Surface)
    {
        global::Aow.Rtl.Rect R;
        if (Aow.Game.Combat.AoWCombatMap.Current != null)
        {
            if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Aow.Game.Combat.AoWCombatMap.Current.FSeatedPlayer).FLeader != null)
            {
                R = global::Aow.Rtl.RtlUnit.Rect(NewTDlg.WinLeft + 188, NewTDlg.WinTop + 40, NewTDlg.WinLeft + 188 + 103, NewTDlg.WinTop + 40 + 128);
                Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Aow.Game.Combat.AoWCombatMap.Current.FSeatedPlayer).FLeader.ShowFace(R, Surface);
            }
        }
        else
        {
            if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FNewTurn.FTurnPlayer).FLeader != null)
            {
                R = global::Aow.Rtl.RtlUnit.Rect(NewTDlg.WinLeft + 188, NewTDlg.WinTop + 40, NewTDlg.WinLeft + 188 + 103, NewTDlg.WinTop + 40 + 128);
                Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FNewTurn.FTurnPlayer).FLeader.ShowFace(R, Surface);
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
