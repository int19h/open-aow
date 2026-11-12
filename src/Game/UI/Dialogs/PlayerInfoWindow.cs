// Translated from AoWPInfoWin.pas.
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

public partial class PlayerInfoWindow : DataModule
{
    public PlayerInfoWindow(Component owner) : base(owner)
    { }

    public Window PInfoWin;
    public ListBox EventList;
    public Label LeadL;
    public Label Turn;
    public Image RShield;
    public Panel FaceFr;
    public Label Player;
    public Label Race;

    // AoWPInfoWin.pas:173
    public void UpdatePlayer()
    {
        int raceIndex;
        if (AoWHSMap.Current.SeatedPlayer > -1)
        {
            var name = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetName();
            DialogHosts.PlayerInfoWindow.Player.Text = name;
            if (AoWHSMap.Current.SeatedPlayer != 0)
            {
                var player = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
                var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[player.PlayerRace]);
                DialogHosts.PlayerInfoWindow.Race.Text = raceNamesPlural;
            }

            var day = AbilityRules.TranslateResourceString(GameText.DayText);
            DialogHosts.PlayerInfoWindow.Turn.Text = day + " " + IntToStr(AoWHSMap.Current.Day);
            raceIndex = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).PlayerRace;
            if (raceIndex == -1)
            {
                raceIndex = 12;
            }

            DialogHosts.PlayerInfoWindow.RShield.CurrentImage = raceIndex + 13;
            DialogHosts.PlayerInfoWindow.PInfoWin.Visible = true;
        }
    }

    // AoWPInfoWin.pas:190
    public void UpdateEvents()
    {
        EventList.Items.Clear();
        if (AoWHSMap.Current.SeatedPlayer > -1)
        {
            AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).EventLogbook.ListEvents(EventList.Items);
        }

        var count = EventList.Items.Count;
        EventList.ListOffset = count - 1;
    }

    // AoWPInfoWin.pas:67
    public void MinClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        PInfoWin.Visible = false;
    }

    // AoWPInfoWin.pas:72
    public void EventListDoubleClick()
    {
        var objectValue = EventList.Items.Objects[EventList.Selected];
        AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).EventLogbook.GetLogs(ObjectToInt(objectValue)).Execute(true, ExecuteTrigger.User, null);
    }

    // AoWPInfoWin.pas:78
    public void EventListPreDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        if (drawItem != EventList.Selected)
        {
            var objectValue = EventList.Items.Objects[drawItem];
            var textColor = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).EventLogbook.GetLogs(ObjectToInt(objectValue)).GetTextColor();
            EventList.TextColor = textColor;
        }
    }

    // AoWPInfoWin.pas:85
    public void EventListDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        int leftIcon;
        int middleIcon;
        int rightIcon;
        var objectValue = EventList.Items.Objects[drawItem];
        leftIcon = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).EventLogbook.GetLogs(ObjectToInt(objectValue)).GetLeftIcon();
        var objectValue2 = EventList.Items.Objects[drawItem];
        middleIcon = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).EventLogbook.GetLogs(ObjectToInt(objectValue2)).GetMiddleIcon();
        var objectValue3 = EventList.Items.Objects[drawItem];
        rightIcon = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).EventLogbook.GetLogs(ObjectToInt(objectValue3)).GetRightIcon();
        if (leftIcon > -1)
        {
            if (middleIcon == -1)
            {
                var eventIcon = DialogHosts.UIGraphics.EventIcons[leftIcon];
                DialogHosts.UIGraphics.EventIcons[leftIcon].ShowClipped(drawRectangle.Left + (36 - eventIcon.Width) / 2, drawRectangle.Top + 1, lockedSurface);
            }
            else
            {
                DialogHosts.UIGraphics.EventIcons[leftIcon].ShowClipped(drawRectangle.Left, drawRectangle.Top + 1, lockedSurface);
            }
        }

        if (middleIcon > -1)
        {
            DialogHosts.UIGraphics.EventIcons[middleIcon].ShowClipped(drawRectangle.Left + 12, drawRectangle.Top + 1, lockedSurface);
        }

        if (rightIcon > -1)
        {
            DialogHosts.UIGraphics.EventIcons[rightIcon].ShowClipped(drawRectangle.Left + 24, drawRectangle.Top + 1, lockedSurface);
        }
    }

    // AoWPInfoWin.pas:100
    public void PInfoWinResize()
    {
        this.PInfoWin.Alignment.RightOffset = this.PInfoWin.WindowManager.WinWidth - this.PInfoWin.WinLeft - this.PInfoWin.WinWidth;
        this.PInfoWin.Alignment.BottomOffset = DialogHosts.General.Manager.WinHeight - this.PInfoWin.WinTop - this.PInfoWin.WinHeight;
    }

    // AoWPInfoWin.pas:109
    public void EventListMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (EventList.Selected > -1 && button == MouseButton.Right)
        {
            var objectValue = EventList.Items.Objects[EventList.Selected];
            AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).EventLogbook.GetLogs(ObjectToInt(objectValue)).ViewLocation();
        }
    }

    // AoWPInfoWin.pas:116
    public void LeaderBtnClick()
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        DialogHosts.General.PlayButtonSound(0);
        if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Leader != null)
        {
            AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetLeaderLocation(ref X, ref Y, ref L);
            if (X > -1)
            {
                AoWHSMap.Current.CenterViewTo(X, Y, L);
                AoWHSMap.Current.PlayCenterAnimation(X, Y, L);
            }
        }
    }

    // AoWPInfoWin.pas:132
    public void RaceButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWPInfoWin.pas:137
    public void FaceFrDraw(LockedSurface lockedSurface)
    {
        Rect R;
        if (AoWHSMap.Current.SeatedPlayer > -1)
        {
            R = new Rect(FaceFr.WinLeft + 3, FaceFr.WinTop + 3, FaceFr.WinLeft + FaceFr.WinWidth - 3, FaceFr.WinTop + FaceFr.WinHeight - 3);
            if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetFace() != null)
            {
                AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetFace().ShowStretched(R, lockedSurface);
            }
        }
    }

    // AoWPInfoWin.pas:149
    public void FaceFrMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        sbyte hexX = default;
        sbyte hexY = default;
        sbyte hexLevel = default;
        if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Leader != null)
        {
            AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetLeaderLocation(ref hexX, ref hexY, ref hexLevel);
            if (hexX > -1)
            {
                var player = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
                DialogHosts.General.SelectedUnit = player.Leader;
                switch (button)
                {
                    case MouseButton.Left:
                        DialogHosts.UnitWindow.ShowUnit();
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
        HelpWindow.ShowHelp(502);
    }
}
