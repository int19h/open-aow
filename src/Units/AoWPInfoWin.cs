// Translated from AoWPInfoWin.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Events;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWPInfoWin.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

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
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer > -1)
        {
            var name = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).GetName();
            Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.Player.Text = name;
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer != 0)
            {
                var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
                var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.RaceNamesPluralRStr[player.FPlayerRace]);
                Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.Race.Text = raceNamesPlural;
            }

            var day = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.DayRStr);
            Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.Turn.Text = day + " " + IntToStr(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FDay);
            RaceIndex = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FPlayerRace;
            if (RaceIndex == -1)
            {
                RaceIndex = 12;
            }

            Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.RShield.CurrentImage = RaceIndex + 13;
            Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin.Visible = true;
        }
    }

    // AoWPInfoWin.pas:190
    public void UpdateEvents()
    {
        EventList.Items.Clear();
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer > -1)
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FEventLogbook.ListEvents(EventList.Items);
        }

        var count = EventList.Items.Count;
        EventList.ListOffset = count - 1;
    }

    // AoWPInfoWin.pas:67
    public void MinClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        PInfoWin.Visible = false;
    }

    // AoWPInfoWin.pas:72
    public void EventListDoubleClick()
    {
        var objectValue = EventList.Items.Objects[EventList.Selected];
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue)).Execute(true, TExecuteTrigger.etUser, null);
    }

    // AoWPInfoWin.pas:78
    public void EventListPreDrawItem(in TRect DrawRect, int DrawItem, ref TSurfaceDesc SD)
    {
        if (DrawItem != EventList.Selected)
        {
            var objectValue = EventList.Items.Objects[DrawItem];
            var textColor = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue)).GetTextColor();
            EventList.TextColor = textColor;
        }
    }

    // AoWPInfoWin.pas:85
    public void EventListDrawItem(in TRect DrawRect, int DrawItem, ref TSurfaceDesc SD)
    {
        int LeftIcon;
        int MiddleIcon;
        int RightIcon;
        var objectValue = EventList.Items.Objects[DrawItem];
        LeftIcon = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue)).GetLeftIcon();
        var objectValue2 = EventList.Items.Objects[DrawItem];
        MiddleIcon = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue2)).GetMiddleIcon();
        var objectValue3 = EventList.Items.Objects[DrawItem];
        RightIcon = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue3)).GetRightIcon();
        if (LeftIcon > -1)
        {
            if (MiddleIcon == -1)
            {
                var eventIcon = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.EventIcons[LeftIcon];
                Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.EventIcons[LeftIcon].ShowClipped(DrawRect.Left + (36 - eventIcon.Width) / 2, DrawRect.Top + 1, SD);
            }
            else
            {
                Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.EventIcons[LeftIcon].ShowClipped(DrawRect.Left, DrawRect.Top + 1, SD);
            }
        }

        if (MiddleIcon > -1)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.EventIcons[MiddleIcon].ShowClipped(DrawRect.Left + 12, DrawRect.Top + 1, SD);
        }

        if (RightIcon > -1)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.EventIcons[RightIcon].ShowClipped(DrawRect.Left + 24, DrawRect.Top + 1, SD);
        }
    }

    // AoWPInfoWin.pas:100
    public void PInfoWinResize()
    {
        this.PInfoWin.Alignment.RightOffset = this.PInfoWin.WinManager.WinWidth - this.PInfoWin.WinLeft - this.PInfoWin.WinWidth;
        this.PInfoWin.Alignment.BottomOffset = Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.WinHeight - this.PInfoWin.WinTop - this.PInfoWin.WinHeight;
    }

    // AoWPInfoWin.pas:109
    public void EventListMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (EventList.Selected > -1 && Button == TMouseButton.mbRight)
        {
            var objectValue = EventList.Items.Objects[EventList.Selected];
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue)).ViewLocation();
        }
    }

    // AoWPInfoWin.pas:116
    public void LeaderBtnClick()
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FLeader != null)
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).GetLeaderLocation(ref X, ref Y, ref L);
            if (X > -1)
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.CenterViewTo(X, Y, L);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlayCenterAnimation(X, Y, L);
            }
        }
    }

    // AoWPInfoWin.pas:132
    public void RaceBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
    }

    // AoWPInfoWin.pas:137
    public void FaceFrDraw(TSurfaceDesc SD)
    {
        TRect R;
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer > -1)
        {
            R = Rect(FaceFr.WinLeft + 3, FaceFr.WinTop + 3, FaceFr.WinLeft + FaceFr.WinWidth - 3, FaceFr.WinTop + FaceFr.WinHeight - 3);
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).GetFace() != null)
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).GetFace().ShowStretched(R, SD);
            }
        }
    }

    // AoWPInfoWin.pas:149
    public void FaceFrMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        sbyte Xhx = default;
        sbyte Yhx = default;
        sbyte Lhx = default;
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FLeader != null)
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).GetLeaderLocation(ref Xhx, ref Yhx, ref Lhx);
            if (Xhx > -1)
            {
                var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
                Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit = player.FLeader;
                switch (Button)
                {
                    case TMouseButton.mbLeft:
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ShowUnit();
                        break;
                    case TMouseButton.mbRight:
                        LeaderBtnClick();
                        break;
                }
            }
        }
    }

    // AoWPInfoWin.pas:167
    public void HelpClick()
    {
        Aow.Game.Ui.Dialogs.THelpWindow.ShowHelp(502);
    }
}
