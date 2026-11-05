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
            int RaceIndex;
            if (AoWHSMap.FSeatedPlayer > -1)
            {
                var name = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).GetName();
                AoWPInfoWin.PInfoW.Player.Text = name;
                if (AoWHSMap.FSeatedPlayer != 0)
                {
                    var player = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    var raceNamesPlural = TranslateRStr(RaceNamesPluralRStr[player.FPlayerRace]);
                    AoWPInfoWin.PInfoW.Race.Text = raceNamesPlural;
                }

                var day = TranslateRStr(DayRStr);
                AoWPInfoWin.PInfoW.Turn.Text = day + " " + IntToStr(AoWHSMap.FDay);
                RaceIndex = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FPlayerRace;
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
                AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FEventLogbook.ListEvents(EventList.Items);
            }

            var count = EventList.Items.Count;
            EventList.ListOffset = count - 1;
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
            var objectValue = EventList.Items.Objects[EventList.Selected];
            AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue)).Execute(true, TExecuteTrigger.etUser, null);
        }

        // AoWPInfoWin.pas:78
        public void EventListPreDrawItem(TAoWComponent Sender, in TRect DrawRect, int DrawItem, ref TSurfaceDesc SD)
        {
            if (DrawItem != EventList.Selected)
            {
                var objectValue = EventList.Items.Objects[DrawItem];
                var textColor = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue)).GetTextColor();
                EventList.TextColor = textColor;
            }
        }

        // AoWPInfoWin.pas:85
        public void EventListDrawItem(TAoWComponent Sender, in TRect DrawRect, int DrawItem, ref TSurfaceDesc SD)
        {
            int LeftIcon;
            int MiddleIcon;
            int RightIcon;
            var objectValue = EventList.Items.Objects[DrawItem];
            LeftIcon = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue)).GetLeftIcon();
            var objectValue2 = EventList.Items.Objects[DrawItem];
            MiddleIcon = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue2)).GetMiddleIcon();
            var objectValue3 = EventList.Items.Objects[DrawItem];
            RightIcon = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue3)).GetRightIcon();
            if (LeftIcon > -1)
            {
                if (MiddleIcon == -1)
                {
                    var eventIcon = aowIntGfxMod.aowIntGfxMod.IntGfxMod.EventIcons[LeftIcon];
                    aowIntGfxMod.aowIntGfxMod.IntGfxMod.EventIcons[LeftIcon].ShowClipped(DrawRect.Left + (36 - eventIcon.Width) / 2, DrawRect.Top + 1, SD);
                }
                else
                {
                    aowIntGfxMod.aowIntGfxMod.IntGfxMod.EventIcons[LeftIcon].ShowClipped(DrawRect.Left, DrawRect.Top + 1, SD);
                }
            }

            if (MiddleIcon > -1)
            {
                aowIntGfxMod.aowIntGfxMod.IntGfxMod.EventIcons[MiddleIcon].ShowClipped(DrawRect.Left + 12, DrawRect.Top + 1, SD);
            }

            if (RightIcon > -1)
            {
                aowIntGfxMod.aowIntGfxMod.IntGfxMod.EventIcons[RightIcon].ShowClipped(DrawRect.Left + 24, DrawRect.Top + 1, SD);
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
                var objectValue = EventList.Items.Objects[EventList.Selected];
                AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue)).ViewLocation();
            }
        }

        // AoWPInfoWin.pas:116
        public void LeaderBtnClick(TObject Sender)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            General.PlayButtonSound(0);
            if (AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FLeader != null)
            {
                AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).GetLeaderLocation(ref X, ref Y, ref L);
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
            TRect R;
            if (AoWHSMap.FSeatedPlayer > -1)
            {
                R = Rect(FaceFr.WinLeft + 3, FaceFr.WinTop + 3, FaceFr.WinLeft + FaceFr.WinWidth - 3, FaceFr.WinTop + FaceFr.WinHeight - 3);
                if (AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).GetFace() != null)
                {
                    AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).GetFace().ShowStretched(R, SD);
                }
            }
        }

        // AoWPInfoWin.pas:149
        public void FaceFrMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            sbyte Xhx = default;
            sbyte Yhx = default;
            sbyte Lhx = default;
            if (AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FLeader != null)
            {
                AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).GetLeaderLocation(ref Xhx, ref Yhx, ref Lhx);
                if (Xhx > -1)
                {
                    var player = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    General.FSelectedUnit = player.FLeader;
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
