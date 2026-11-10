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

public partial class PInfoW : DataModule
{
    public PInfoW(Component owner) : base(owner)
    { }

    public AOWWindow PInfoWin;
    public AOWButton Min;
    public AOWLabel IWTitle;
    public AOWImage SIcon;
    public AOWListBox EventList;
    public AOWLabel LeadL;
    public AOWLabel Turn;
    public AOWImage RShield;
    public AOWPanel EventFr;
    public AOWVScrollBar EventSB;
    public AOWPanel PInfoPnl;
    public AOWCheckBox LeaderCB;
    public AOWPanel FaceFr;
    public AOWLabel EventLbl;
    public AOWPanel ShieldFr;
    public AOWPanel TurnFr;
    public AOWPanel InfoFr;
    public AOWLabel Player;
    public AOWLabel Race;
    public AOWCheckBox RaceCB;
    public AOWButton Center;
    public AOWButton Help;
    public IvTranslator Translator;

    // AoWPInfoWin.pas:173
    public void UpdatePlayer()
    {
        int RaceIndex;
        if (World.Map.AoWHSMap.Current.FSeatedPlayer > -1)
        {
            var name = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).GetName();
            Player.Text = name;
            if (World.Map.AoWHSMap.Current.FSeatedPlayer != 0)
            {
                var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer);
                var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Magic.Abilities.GameTables.RaceNamesPluralRStr[player.FPlayerRace]);
                Race.Text = raceNamesPlural;
            }

            var day = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Magic.Abilities.GameText.DayRStr);
            Turn.Text = day + " " + IntToStr(World.Map.AoWHSMap.Current.FDay);
            RaceIndex = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FPlayerRace;
            if (RaceIndex == -1)
            {
                RaceIndex = 12;
            }

            RShield.CurrentImage = RaceIndex + 13;
            PInfoWin.Visible = true;
        }
    }

    // AoWPInfoWin.pas:190
    public void UpdateEvents()
    {
        EventList.Items.Clear();
        if (World.Map.AoWHSMap.Current.FSeatedPlayer > -1)
        {
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FEventLogbook.ListEvents(EventList.Items);
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
        Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue)).Execute(true, ExecuteTrigger.User, null);
    }

    // AoWPInfoWin.pas:78
    public void EventListPreDrawItem(in Rect DrawRect, int DrawItem, ref SurfaceDesc SD)
    {
        if (DrawItem != EventList.Selected)
        {
            var objectValue = EventList.Items.Objects[DrawItem];
            var textColor = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue)).GetTextColor();
            EventList.TextColor = textColor;
        }
    }

    // AoWPInfoWin.pas:85
    public void EventListDrawItem(in Rect DrawRect, int DrawItem, ref SurfaceDesc SD)
    {
        int LeftIcon;
        int MiddleIcon;
        int RightIcon;
        var objectValue = EventList.Items.Objects[DrawItem];
        LeftIcon = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue)).GetLeftIcon();
        var objectValue2 = EventList.Items.Objects[DrawItem];
        MiddleIcon = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue2)).GetMiddleIcon();
        var objectValue3 = EventList.Items.Objects[DrawItem];
        RightIcon = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue3)).GetRightIcon();
        if (LeftIcon > -1)
        {
            if (MiddleIcon == -1)
            {
                var eventIcon = DialogHosts.IntGfxMod.EventIcons[LeftIcon];
                DialogHosts.IntGfxMod.EventIcons[LeftIcon].ShowClipped(DrawRect.Left + (36 - eventIcon.Width) / 2, DrawRect.Top + 1, SD);
            }
            else
            {
                DialogHosts.IntGfxMod.EventIcons[LeftIcon].ShowClipped(DrawRect.Left, DrawRect.Top + 1, SD);
            }
        }

        if (MiddleIcon > -1)
        {
            DialogHosts.IntGfxMod.EventIcons[MiddleIcon].ShowClipped(DrawRect.Left + 12, DrawRect.Top + 1, SD);
        }

        if (RightIcon > -1)
        {
            DialogHosts.IntGfxMod.EventIcons[RightIcon].ShowClipped(DrawRect.Left + 24, DrawRect.Top + 1, SD);
        }
    }

    // AoWPInfoWin.pas:100
    public void PInfoWinResize()
    {
        this.PInfoWin.Alignment.RightOffset = this.PInfoWin.WinManager.WinWidth - this.PInfoWin.WinLeft - this.PInfoWin.WinWidth;
        this.PInfoWin.Alignment.BottomOffset = DialogHosts.General.Manager.WinHeight - this.PInfoWin.WinTop - this.PInfoWin.WinHeight;
    }

    // AoWPInfoWin.pas:109
    public void EventListMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (EventList.Selected > -1 && Button == MouseButton.Right)
        {
            var objectValue = EventList.Items.Objects[EventList.Selected];
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue)).ViewLocation();
        }
    }

    // AoWPInfoWin.pas:116
    public void LeaderBtnClick()
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FLeader != null)
        {
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).GetLeaderLocation(ref X, ref Y, ref L);
            if (X > -1)
            {
                Aow.Game.World.Map.AoWHSMap.Current.CenterViewTo(X, Y, L);
                Aow.Game.World.Map.AoWHSMap.Current.PlayCenterAnimation(X, Y, L);
            }
        }
    }

    // AoWPInfoWin.pas:132
    public void RaceBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
    }

    // AoWPInfoWin.pas:137
    public void FaceFrDraw(SurfaceDesc SD)
    {
        Rect R;
        if (World.Map.AoWHSMap.Current.FSeatedPlayer > -1)
        {
            R = new Rect(FaceFr.WinLeft + 3, FaceFr.WinTop + 3, FaceFr.WinLeft + FaceFr.WinWidth - 3, FaceFr.WinTop + FaceFr.WinHeight - 3);
            if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).GetFace() != null)
            {
                Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).GetFace().ShowStretched(R, SD);
            }
        }
    }

    // AoWPInfoWin.pas:149
    public void FaceFrMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        sbyte Xhx = default;
        sbyte Yhx = default;
        sbyte Lhx = default;
        if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FLeader != null)
        {
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).GetLeaderLocation(ref Xhx, ref Yhx, ref Lhx);
            if (Xhx > -1)
            {
                var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer);
                DialogHosts.General.FSelectedUnit = player.FLeader;
                switch (Button)
                {
                    case MouseButton.Left:
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ShowUnit();
                        break;
                    case MouseButton.Right:
                        LeaderBtnClick();
                        break;
                }
            }
        }
    }

    // AoWPInfoWin.pas:167
    public void HelpClick()
    {
        Aow.Game.Ui.Dialogs.HelpWindow.ShowHelp(502);
    }
}
