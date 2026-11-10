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
public partial class TNewTurnDlg : TDataModule
{
    public TNewTurnDlg(TComponent owner) : base(owner)
    { }

    public TAOWWindow NewTDlg;
    public TAoWFrame TCeltB;
    public TAOWLabel ClickMsg;
    public TAOWImage LShield;
    public TAOWLabel PlayerName;
    public TAOWLabel TurnMsg;
    public TAOWLabel PlayerRace;
    public TAOWLabel Turn;
    public TAOWImage RShield;
    public TAoWFrame BlueFr;
    public TAOWImage TCeltLineL;
    public TAOWImage TCeltLineR;
    public TAOWImage FaceFrame;
    public TExecuteDoneEvent FDone;
    public TNewTurnEventLog FNewTurn;

    // AoWNewTurnDialog.pas:50
    public void FinishDialog()
    {
        TExecuteDoneEvent Done;
        TNewTurnEventLog Event;
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 0;
        if (FNewTurn == null)
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.SetSeatedPlayer(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCurrentPlayer);
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer > -1)
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
    public void ShowNewTurn(TNewTurnEventLog Event, TExecuteDoneEvent Done)
    {
        sbyte Race;
        int Day;
        switch (Event.FMode)
        {
            case TNewTurnEventLogMode.ntDialog:
                if (FNewTurn != null)
                {
                    FNewTurn.Release();
                    FNewTurn = null;
                }

                FDone = Done;
                FNewTurn = Event;
                FNewTurn.AddRef();
                TurnMsg.Text = Event.FText;
                var name = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Event.FTurnPlayer).GetName();
                PlayerName.Text = name;
                if (Event.FTurnPlayer != 0)
                {
                    var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Event.FTurnPlayer);
                    var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.RaceNamesPluralRStr[player.FPlayerRace]);
                    PlayerRace.Text = raceNamesPlural;
                }
                else
                {
                    PlayerRace.Text = "";
                }

                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FDay > 0)
                {
                    Day = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FDay;
                }
                else
                {
                    Day = 1;
                }

                var day = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.DayRStr);
                Turn.Text = day + " " + IntToStr(Day);
                Race = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Event.FTurnPlayer).FPlayerRace;
                if (Race == -1)
                {
                    Race = 12;
                }

                LShield.CurrentImage = Race + 26;
                RShield.CurrentImage = Race + 26;
                NewTDlg.FDrawn = false;
                NewTDlg.Visible = true;
                if (FNewTurn.FDialogStyle.Contains(TNewTurnEventLogDialogStyleElement.nsCloseable))
                {
                    var clickToBegin = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TNewTurnDlg.ClickToBeginRStr);
                    ClickMsg.Text = clickToBegin;
                }
                else
                {
                    var hiddenmovementPleaseWait = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TNewTurnDlg.HiddenmovementPleaseWaitRStr);
                    ClickMsg.Text = hiddenmovementPleaseWait;
                }

                break;
            case TNewTurnEventLogMode.ntClose:
                NewTDlg.Visible = false;
                break;
        }
    }

    // AoWNewTurnDialog.pas:106
    public void NewTDlgMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap == null && FNewTurn.FDialogStyle.Contains(TNewTurnEventLogDialogStyleElement.nsCloseable))
        {
            NewTDlg.Visible = false;
        }
    }

    // AoWNewTurnDialog.pas:111
    public void NewTDlgKeyPress(ref byte Key)
    {
        if (FNewTurn.FDialogStyle.Contains(TNewTurnEventLogDialogStyleElement.nsCloseable) && (Key == 32 || Key == 13 || Key == 27))
        {
            NewTDlg.Visible = false;
        }
    }

    // AoWNewTurnDialog.pas:116
    public void FaceFramePreDraw(TSurfaceDesc Surface)
    {
        TRect R;
        if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap != null)
        {
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FSeatedPlayer).FLeader != null)
            {
                R = Rect(NewTDlg.WinLeft + 188, NewTDlg.WinTop + 40, NewTDlg.WinLeft + 188 + 103, NewTDlg.WinTop + 40 + 128);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FSeatedPlayer).FLeader.ShowFace(R, Surface);
            }
        }
        else
        {
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FNewTurn.FTurnPlayer).FLeader != null)
            {
                R = Rect(NewTDlg.WinLeft + 188, NewTDlg.WinTop + 40, NewTDlg.WinLeft + 188 + 103, NewTDlg.WinTop + 40 + 128);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FNewTurn.FTurnPlayer).FLeader.ShowFace(R, Surface);
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
