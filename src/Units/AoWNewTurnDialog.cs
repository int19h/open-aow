// Translated from AoWNewTurnDialog.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWNewTurnDialog
{
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
            TExecuteDoneEvent Done = default;
            TNewTurnEventLog Event = default;
            General.Manager.OverlayPriority = 0;
            if (FNewTurn == null)
            {
                AoWHSMap.SetSeatedPlayer(AoWHSMap.FCurrentPlayer);
                if (AoWHSMap.FSeatedPlayer > -1)
                {
                    AoWPInfoWin.AoWPInfoWin.PInfoW.UpdateEvents();
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
            sbyte Race = default;
            int Day = default;
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
                    var call__1 = AoWHSMap.FPlayers.GetPlayers(Event.FTurnPlayer);
                    var call__2 = call__1.GetName();
                    PlayerName.Text = call__2;
                    if (Event.FTurnPlayer != 0)
                    {
                        var call__3 = AoWHSMap.FPlayers.GetPlayers(Event.FTurnPlayer);
                        var call__4 = TranslateRStr(RaceNamesPluralRStr[call__3.FPlayerRace]);
                        PlayerRace.Text = call__4;
                    }
                    else
                    {
                        PlayerRace.Text = (AnsiString)"";
                    }

                    if (AoWHSMap.FDay > 0)
                    {
                        Day = AoWHSMap.FDay;
                    }
                    else
                    {
                        Day = 1;
                    }

                    var call__5 = TranslateRStr(DayRStr);
                    Turn.Text = call__5 + AnsiString.FromBytes(32) + IntToStr(Day);
                    var call__6 = AoWHSMap.FPlayers.GetPlayers(Event.FTurnPlayer);
                    Race = call__6.FPlayerRace;
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
                        var call__7 = TranslateRStr(ClickToBeginRStr);
                        ClickMsg.Text = call__7;
                    }
                    else
                    {
                        var call__8 = TranslateRStr(HiddenmovementPleaseWaitRStr);
                        ClickMsg.Text = call__8;
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
            TRect R = default;
            if (AoWCombatMap != null)
            {
                var call__9 = AoWHSMap.FPlayers.GetPlayers(AoWCombatMap.FSeatedPlayer);
                if (call__9.FLeader != null)
                {
                    var call__10 = Rect(NewTDlg.WinLeft + 188, NewTDlg.WinTop + 40, NewTDlg.WinLeft + 188 + 103, NewTDlg.WinTop + 40 + 128);
                    R = call__10;
                    var call__11 = AoWHSMap.FPlayers.GetPlayers(AoWCombatMap.FSeatedPlayer);
                    call__11.FLeader.ShowFace(R, Surface);
                }
            }
            else
            {
                var call__12 = AoWHSMap.FPlayers.GetPlayers(FNewTurn.FTurnPlayer);
                if (call__12.FLeader != null)
                {
                    var call__13 = Rect(NewTDlg.WinLeft + 188, NewTDlg.WinTop + 40, NewTDlg.WinLeft + 188 + 103, NewTDlg.WinTop + 40 + 128);
                    R = call__13;
                    var call__14 = AoWHSMap.FPlayers.GetPlayers(FNewTurn.FTurnPlayer);
                    call__14.FLeader.ShowFace(R, Surface);
                }
            }
        }

        // AoWNewTurnDialog.pas:136
        public void NewTDlgHide()
        {
            FinishDialog();
        }
    }

    public static partial class AoWNewTurnDialog
    {
    }
}
