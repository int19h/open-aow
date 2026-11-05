// Translated from AoWPInfoWin.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWComp;
using Aow.Units.EventLog;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.aowHelpWin.aowHelpWin;

namespace Aow.Units.AoWPInfoWin
{
    public partial class TPInfoW : TDataModule
    {
        public TPInfoW(TComponent owner) : base(owner)
        { }

        public TAOWWindow PInfoWin;
        public TAOWButton Min;
        public TAOWLabel IWTitle;
        public TAOWImage SIcon;
        public TAOWListBox EventList;
        public TAOWLabel LeadL;
        public TAOWLabel Turn;
        public TAOWImage RShield;
        public TAOWPanel EventFr;
        public TAOWVScrollBar EventSB;
        public TAOWPanel PInfoPnl;
        public TAOWCheckBox LeaderCB;
        public TAOWPanel FaceFr;
        public TAOWLabel EventLbl;
        public TAOWPanel ShieldFr;
        public TAOWPanel TurnFr;
        public TAOWPanel InfoFr;
        public TAOWLabel Player;
        public TAOWLabel Race;
        public TAOWCheckBox RaceCB;
        public TAOWButton Center;
        public TAOWButton Help;
        public TIvTranslator Translator;
        // AoWPInfoWin.pas:173
        public void UpdatePlayer()
        {
            int RaceIndex = default;
            if (AoWHSMap.FSeatedPlayer > -1)
            {
                var call__1 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__2 = call__1.GetName();
                AoWPInfoWin.PInfoW.Player.Text = call__2;
                if (AoWHSMap.FSeatedPlayer != 0)
                {
                    var call__3 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    var call__4 = TranslateRStr(RaceNamesPluralRStr[call__3.FPlayerRace]);
                    AoWPInfoWin.PInfoW.Race.Text = call__4;
                }

                var call__5 = TranslateRStr(DayRStr);
                AoWPInfoWin.PInfoW.Turn.Text = call__5 + AnsiString.FromBytes(32) + IntToStr(AoWHSMap.FDay);
                var call__6 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                RaceIndex = call__6.FPlayerRace;
                if (RaceIndex == -1)
                {
                    RaceIndex = 12;
                }

                AoWPInfoWin.PInfoW.RShield.CurrentImage = RaceIndex + 13;
                AoWPInfoWin.PInfoW.PInfoWin.Visible = true;
            }
        }

        // AoWPInfoWin.pas:190
        public void UpdateEvents()
        {
            EventList.Items.Clear();
            if (AoWHSMap.FSeatedPlayer > -1)
            {
                var call__7 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                call__7.FEventLogbook.ListEvents(EventList.Items);
            }

            var call__8 = EventList.Items.Count;
            EventList.ListOffset = call__8 - 1;
        }

        // AoWPInfoWin.pas:67
        public void MinClick()
        {
            General.PlayButtonSound(0);
            PInfoWin.Visible = false;
        }

        // AoWPInfoWin.pas:72
        public void EventListDoubleClick()
        {
            var call__9 = EventList.Items.Objects[EventList.Selected];
            var call__10 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__11 = call__10.FEventLogbook.GetLogs(ObjectToInt(call__9));
            call__11.Execute(true, TExecuteTrigger.etUser, null);
        }

        // AoWPInfoWin.pas:78
        public void EventListPreDrawItem(TAoWComponent Sender, in TRect DrawRect, int DrawItem, ref TSurfaceDesc SD)
        {
            if (DrawItem != EventList.Selected)
            {
                var call__12 = EventList.Items.Objects[DrawItem];
                var call__13 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__14 = call__13.FEventLogbook.GetLogs(ObjectToInt(call__12));
                var call__15 = call__14.GetTextColor();
                EventList.TextColor = call__15;
            }
        }

        // AoWPInfoWin.pas:85
        public void EventListDrawItem(TAoWComponent Sender, in TRect DrawRect, int DrawItem, ref TSurfaceDesc SD)
        {
            int LeftIcon = default;
            int MiddleIcon = default;
            int RightIcon = default;
            var call__16 = EventList.Items.Objects[DrawItem];
            var call__17 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__18 = call__17.FEventLogbook.GetLogs(ObjectToInt(call__16));
            var call__19 = call__18.GetLeftIcon();
            LeftIcon = call__19;
            var call__20 = EventList.Items.Objects[DrawItem];
            var call__21 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__22 = call__21.FEventLogbook.GetLogs(ObjectToInt(call__20));
            var call__23 = call__22.GetMiddleIcon();
            MiddleIcon = call__23;
            var call__24 = EventList.Items.Objects[DrawItem];
            var call__25 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__26 = call__25.FEventLogbook.GetLogs(ObjectToInt(call__24));
            var call__27 = call__26.GetRightIcon();
            RightIcon = call__27;
            if (LeftIcon > -1)
            {
                if (MiddleIcon == -1)
                {
                    var call__28 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.EventIcons[LeftIcon];
                    var call__29 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.EventIcons[LeftIcon];
                    call__29.ShowClipped(DrawRect.Left + (36 - call__28.Width) / 2, DrawRect.Top + 1, SD);
                }
                else
                {
                    var call__30 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.EventIcons[LeftIcon];
                    call__30.ShowClipped(DrawRect.Left, DrawRect.Top + 1, SD);
                }
            }

            if (MiddleIcon > -1)
            {
                var call__31 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.EventIcons[MiddleIcon];
                call__31.ShowClipped(DrawRect.Left + 12, DrawRect.Top + 1, SD);
            }

            if (RightIcon > -1)
            {
                var call__32 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.EventIcons[RightIcon];
                call__32.ShowClipped(DrawRect.Left + 24, DrawRect.Top + 1, SD);
            }
        }

        // AoWPInfoWin.pas:100
        public void PInfoWinResize()
        {
            this.PInfoWin.Alignment.RightOffset = this.PInfoWin.WinManager.WinWidth - this.PInfoWin.WinLeft - this.PInfoWin.WinWidth;
            this.PInfoWin.Alignment.BottomOffset = General.Manager.WinHeight - this.PInfoWin.WinTop - this.PInfoWin.WinHeight;
        }

        // AoWPInfoWin.pas:109
        public void EventListMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            if (EventList.Selected > -1 && Button == TMouseButton.mbRight)
            {
                var call__34 = EventList.Items.Objects[EventList.Selected];
                var call__35 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__36 = call__35.FEventLogbook.GetLogs(ObjectToInt(call__34));
                call__36.ViewLocation();
            }
        }

        // AoWPInfoWin.pas:116
        public void LeaderBtnClick(TObject Sender)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            General.PlayButtonSound(0);
            var call__37 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            if (call__37.FLeader != null)
            {
                var call__38 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                call__38.GetLeaderLocation(ref X, ref Y, ref L);
                if (X > -1)
                {
                    AoWHSMap.CenterViewTo(X, Y, L);
                    AoWHSMap.PlayCenterAnimation(X, Y, L);
                }
            }
        }

        // AoWPInfoWin.pas:132
        public void RaceBtnClick()
        {
            General.PlayButtonSound(0);
        }

        // AoWPInfoWin.pas:137
        public void FaceFrDraw(TAoWComponent Sender, TSurfaceDesc SD)
        {
            TRect R = default;
            if (AoWHSMap.FSeatedPlayer > -1)
            {
                var call__39 = Rect(FaceFr.WinLeft + 3, FaceFr.WinTop + 3, FaceFr.WinLeft + FaceFr.WinWidth - 3, FaceFr.WinTop + FaceFr.WinHeight - 3);
                R = call__39;
                var call__40 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__41 = call__40.GetFace();
                if (call__41 != null)
                {
                    var call__42 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    var call__43 = call__42.GetFace();
                    call__43.ShowStretched(R, SD);
                }
            }
        }

        // AoWPInfoWin.pas:149
        public void FaceFrMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            sbyte Xhx = default;
            sbyte Yhx = default;
            sbyte Lhx = default;
            var call__44 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            if (call__44.FLeader != null)
            {
                var call__45 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                call__45.GetLeaderLocation(ref Xhx, ref Yhx, ref Lhx);
                if (Xhx > -1)
                {
                    var call__46 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    General.FSelectedUnit = call__46.FLeader;
                    switch (Button)
                    {
                        case TMouseButton.mbLeft:
                            AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
                            break;
                        case TMouseButton.mbRight:
                            LeaderBtnClick(this);
                            break;
                    }
                }
            }
        }

        // AoWPInfoWin.pas:167
        public void HelpClick()
        {
            ShowHelp(502);
        }
    }

    public static partial class AoWPInfoWin
    {
        public static TPInfoW PInfoW;
    }
}
