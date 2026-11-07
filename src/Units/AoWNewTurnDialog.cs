// Translated from AoWNewTurnDialog.pas.
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWPInfoWin;
using Aow.Units.AoWWindow;
using Aow.Units.EventLog;
using Aow.Units.GFXE;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWTC.AoWTCUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;

namespace Aow.Units.AoWNewTurnDialog;

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
        General.Manager.OverlayPriority = 0;
        if (FNewTurn == null)
        {
            AoWHSMap.SetSeatedPlayer(AoWHSMap.FCurrentPlayer);
            if (AoWHSMap.FSeatedPlayer > -1)
            {
                AoWPInfoWinUnit.PInfoW.UpdateEvents();
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
                var name = AoWHSMap.FPlayers.GetPlayers(Event.FTurnPlayer).GetName();
                PlayerName.Text = name;
                if (Event.FTurnPlayer != 0)
                {
                    var player = AoWHSMap.FPlayers.GetPlayers(Event.FTurnPlayer);
                    var raceNamesPlural = TranslateRStr(RaceNamesPluralRStr[player.FPlayerRace]);
                    PlayerRace.Text = raceNamesPlural;
                }
                else
                {
                    PlayerRace.Text = "";
                }

                if (AoWHSMap.FDay > 0)
                {
                    Day = AoWHSMap.FDay;
                }
                else
                {
                    Day = 1;
                }

                var day = TranslateRStr(DayRStr);
                Turn.Text = day + " " + IntToStr(Day);
                Race = AoWHSMap.FPlayers.GetPlayers(Event.FTurnPlayer).FPlayerRace;
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
                    var clickToBegin = TranslateRStr(ClickToBeginRStr);
                    ClickMsg.Text = clickToBegin;
                }
                else
                {
                    var hiddenmovementPleaseWait = TranslateRStr(HiddenmovementPleaseWaitRStr);
                    ClickMsg.Text = hiddenmovementPleaseWait;
                }

                break;
            case TNewTurnEventLogMode.ntClose:
                NewTDlg.Visible = false;
                break;
        }
    }

    // AoWNewTurnDialog.pas:106
    public void NewTDlgMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (AoWCombatMap == null && FNewTurn.FDialogStyle.Contains(TNewTurnEventLogDialogStyleElement.nsCloseable))
        {
            NewTDlg.Visible = false;
        }
    }

    // AoWNewTurnDialog.pas:111
    public void NewTDlgKeyPress(TObject Sender, ref byte Key)
    {
        if (FNewTurn.FDialogStyle.Contains(TNewTurnEventLogDialogStyleElement.nsCloseable) && (Key == 32 || Key == 13 || Key == 27))
        {
            NewTDlg.Visible = false;
        }
    }

    // AoWNewTurnDialog.pas:116
    public void FaceFramePreDraw(TObject Sender, TSurfaceDesc Surface)
    {
        TRect R;
        if (AoWCombatMap != null)
        {
            if (AoWHSMap.FPlayers.GetPlayers(AoWCombatMap.FSeatedPlayer).FLeader != null)
            {
                R = Rect(NewTDlg.WinLeft + 188, NewTDlg.WinTop + 40, NewTDlg.WinLeft + 188 + 103, NewTDlg.WinTop + 40 + 128);
                AoWHSMap.FPlayers.GetPlayers(AoWCombatMap.FSeatedPlayer).FLeader.ShowFace(R, Surface);
            }
        }
        else
        {
            if (AoWHSMap.FPlayers.GetPlayers(FNewTurn.FTurnPlayer).FLeader != null)
            {
                R = Rect(NewTDlg.WinLeft + 188, NewTDlg.WinTop + 40, NewTDlg.WinLeft + 188 + 103, NewTDlg.WinTop + 40 + 128);
                AoWHSMap.FPlayers.GetPlayers(FNewTurn.FTurnPlayer).FLeader.ShowFace(R, Surface);
            }
        }
    }

    // AoWNewTurnDialog.pas:136
    public void NewTDlgHide()
    {
        FinishDialog();
    }
}

public static partial class AoWNewTurnDialogUnit
{
}
