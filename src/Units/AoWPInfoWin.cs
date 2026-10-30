// Translated from AoWPInfoWin.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.aowHelpWin;
using static Aow.Units.aowHelpWin.aowHelpWin;

namespace Aow.Units.AoWPInfoWin
{
    // { TPInfoW (instance size 160, VMT 00422EC8) }
    public partial class TPInfoW : Aow.Rtl.TDataModule
    {
        public TPInfoW(Aow.Rtl.TComponent AOwner) : base(AOwner) { }

        public Aow.Units.AoWWindow.TAOWWindow PInfoWin;
        // { +0044 }
        public Aow.Units.AoWButton.TAOWButton Min;
        // { +0048 }
        public Aow.Units.AoWLabel.TAOWLabel IWTitle;
        // { +004C }
        public Aow.Units.AoWImage.TAOWImage SIcon;
        // { +0050 }
        public Aow.Units.AoWListBox.TAOWListBox EventList;
        // { +0054 }
        public Aow.Units.AoWLabel.TAOWLabel LeadL;
        // { +0058 }
        public Aow.Units.AoWLabel.TAOWLabel Turn;
        // { +005C }
        public Aow.Units.AoWImage.TAOWImage RShield;
        // { +0060 }
        public Aow.Units.AoWPanel.TAOWPanel EventFr;
        // { +0064 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar EventSB;
        // { +0068 }
        public Aow.Units.AoWPanel.TAOWPanel PInfoPnl;
        // { +006C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox LeaderCB;
        // { +0070 }
        public Aow.Units.AoWPanel.TAOWPanel FaceFr;
        // { +0074 }
        public Aow.Units.AoWLabel.TAOWLabel EventLbl;
        // { +0078 }
        public Aow.Units.AoWPanel.TAOWPanel ShieldFr;
        // { +007C }
        public Aow.Units.AoWPanel.TAOWPanel TurnFr;
        // { +0080 }
        public Aow.Units.AoWPanel.TAOWPanel InfoFr;
        // { +0084 }
        public Aow.Units.AoWLabel.TAOWLabel Player;
        // { +0088 }
        public Aow.Units.AoWLabel.TAOWLabel Race;
        // { +008C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox RaceCB;
        // { +0090 }
        public Aow.Units.AoWButton.TAOWButton Center;
        // { +0094 }
        public Aow.Units.AoWButton.TAOWButton Help;
        // { +0098 }
        public Aow.Rtl.TIvTranslator Translator;
        // AoWPInfoWin.pas:173
        // { +009C }
        public void UpdatePlayer()
        {
            int RaceIndex = default;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer > -1)
            {
                var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__2 = call__1.GetName();
                Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.Player.Text = call__2;
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer != 0)
                {
                    var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    var call__4 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[call__3.FPlayerRace]);
                    Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.Race.Text = call__4;
                }

                var call__5 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DayRStr);
                Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.Turn.Text = (call__5 + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(Aow.Units.AoWE.AoWE.AoWHSMap.FDay);
                var call__6 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                RaceIndex = call__6.FPlayerRace;
                if (RaceIndex == -1)
                {
                    RaceIndex = 12;
                }

                Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.RShield.CurrentImage = RaceIndex + 13;
                Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Visible = true;
            }
        }

        // AoWPInfoWin.pas:190
        public void UpdateEvents()
        {
            EventList.Items.Clear();
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer > -1)
            {
                var call__7 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                call__7.FEventLogbook.ListEvents(EventList.Items);
            }

            var call__8 = EventList.Items.Count;
            EventList.ListOffset = call__8 - 1;
        }

        // AoWPInfoWin.pas:67
        public void MinClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            PInfoWin.Visible = false;
        }

        // AoWPInfoWin.pas:72
        public void EventListDoubleClick()
        {
            var call__9 = EventList.Items.Objects[EventList.Selected];
            var call__10 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__11 = call__10.FEventLogbook.GetLogs(Aow.Rtl.Rtl.ObjectToInt(call__9));
            call__11.Execute(true, Aow.Units.EventLog.TExecuteTrigger.etUser, null);
        }

        // AoWPInfoWin.pas:78
        public void EventListPreDrawItem(Aow.Units.AoWComp.TAoWComponent Sender, in Aow.Rtl.TRect DrawRect, int DrawItem, ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            if (DrawItem != EventList.Selected)
            {
                var call__12 = EventList.Items.Objects[DrawItem];
                var call__13 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__14 = call__13.FEventLogbook.GetLogs(Aow.Rtl.Rtl.ObjectToInt(call__12));
                var call__15 = call__14.GetTextColor();
                EventList.TextColor = call__15;
            }
        }

        // AoWPInfoWin.pas:85
        public void EventListDrawItem(Aow.Units.AoWComp.TAoWComponent Sender, in Aow.Rtl.TRect DrawRect, int DrawItem, ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int LeftIcon = default;
            int MiddleIcon = default;
            int RightIcon = default;
            var call__16 = EventList.Items.Objects[DrawItem];
            var call__17 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__18 = call__17.FEventLogbook.GetLogs(Aow.Rtl.Rtl.ObjectToInt(call__16));
            var call__19 = call__18.GetLeftIcon();
            LeftIcon = call__19;
            var call__20 = EventList.Items.Objects[DrawItem];
            var call__21 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__22 = call__21.FEventLogbook.GetLogs(Aow.Rtl.Rtl.ObjectToInt(call__20));
            var call__23 = call__22.GetMiddleIcon();
            MiddleIcon = call__23;
            var call__24 = EventList.Items.Objects[DrawItem];
            var call__25 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__26 = call__25.FEventLogbook.GetLogs(Aow.Rtl.Rtl.ObjectToInt(call__24));
            var call__27 = call__26.GetRightIcon();
            RightIcon = call__27;
            if (LeftIcon > -1)
            {
                if (MiddleIcon == -1)
                {
                    var call__28 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.EventIcons[LeftIcon];
                    var call__29 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.EventIcons[LeftIcon];
                    call__29.ShowClipped(DrawRect.Left + ((36 - call__28.Width) / 2), DrawRect.Top + 1, SD);
                }
                else
                {
                    var call__30 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.EventIcons[LeftIcon];
                    call__30.ShowClipped(DrawRect.Left, DrawRect.Top + 1, SD);
                }
            }

            if (MiddleIcon > -1)
            {
                var call__31 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.EventIcons[MiddleIcon];
                call__31.ShowClipped(DrawRect.Left + 12, DrawRect.Top + 1, SD);
            }

            if (RightIcon > -1)
            {
                var call__32 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.EventIcons[RightIcon];
                call__32.ShowClipped(DrawRect.Left + 24, DrawRect.Top + 1, SD);
            }
        }

        // AoWPInfoWin.pas:100
        public void PInfoWinResize()
        {
            this.PInfoWin.Alignment.RightOffset = (this.PInfoWin.WinManager.WinWidth - this.PInfoWin.WinLeft) - this.PInfoWin.WinWidth;
            this.PInfoWin.Alignment.BottomOffset = (Aow.Units.AoWWinMan.AoWWinMan.General.Manager.WinHeight - this.PInfoWin.WinTop) - this.PInfoWin.WinHeight;
        }

        // AoWPInfoWin.pas:109
        public void EventListMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if ((EventList.Selected > -1) && (Button == Aow.Rtl.TMouseButton.mbRight))
            {
                var call__34 = EventList.Items.Objects[EventList.Selected];
                var call__35 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__36 = call__35.FEventLogbook.GetLogs(Aow.Rtl.Rtl.ObjectToInt(call__34));
                call__36.ViewLocation();
            }
        }

        // AoWPInfoWin.pas:116
        public void LeaderBtnClick(Aow.Rtl.TObject Sender)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__37 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (call__37.FLeader != null)
            {
                var call__38 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                call__38.GetLeaderLocation(ref X, ref Y, ref L);
                if (X > -1)
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(X, Y, L);
                    Aow.Units.AoWE.AoWE.AoWHSMap.PlayCenterAnimation(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L));
                }
            }
        }

        // AoWPInfoWin.pas:132
        public void RaceBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWPInfoWin.pas:137
        public void FaceFrDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Rtl.TRect R = default;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer > -1)
            {
                var call__39 = Aow.Rtl.Rtl.Rect(FaceFr.WinLeft + 3, FaceFr.WinTop + 3, (FaceFr.WinLeft + FaceFr.WinWidth) - 3, (FaceFr.WinTop + FaceFr.WinHeight) - 3);
                R = call__39;
                var call__40 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__41 = call__40.GetFace();
                if (call__41 != null)
                {
                    var call__42 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    var call__43 = call__42.GetFace();
                    call__43.ShowStretched(R, SD);
                }
            }
        }

        // AoWPInfoWin.pas:149
        public void FaceFrMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            sbyte Xhx = default;
            sbyte Yhx = default;
            sbyte Lhx = default;
            var call__44 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (call__44.FLeader != null)
            {
                var call__45 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                call__45.GetLeaderLocation(ref Xhx, ref Yhx, ref Lhx);
                if (Xhx > -1)
                {
                    var call__46 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit = call__46.FLeader;
                    switch (Button)
                    {
                        case Aow.Rtl.TMouseButton.mbLeft:
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
                            break;
                        case Aow.Rtl.TMouseButton.mbRight:
                            LeaderBtnClick(this);
                            break;
                    }
                }
            }
        }

        // AoWPInfoWin.pas:167
        public void HelpClick()
        {
            Aow.Units.aowHelpWin.aowHelpWin.ShowHelp(502);
        }
    }

    public static partial class AoWPInfoWin
    {
        public static Aow.Units.AoWPInfoWin.TPInfoW PInfoW;
    }
}
