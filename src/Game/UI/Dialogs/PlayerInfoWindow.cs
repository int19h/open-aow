// Translated from AoWPInfoWin.pas.
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

public partial class PlayerInfoWindow : UIModule
{
    public PlayerInfoWindow(UIContext ui) : base(ui)
    { }

    public Window MainWindow;
    public ListBox EventList;
    public Label OfTheLabel;
    public Label TurnLabel;
    public Image RaceShield;
    public Panel FaceFrame;
    public Label PlayerLabel;
    public Label RaceLabel;

    // AoWPInfoWin.pas:173
    public void UpdatePlayer()
    {
        int raceIndex;
        if (StrategicMap.Current.SeatedPlayer > -1)
        {
            var name = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetName();
            UI.PlayerInfoWindow.PlayerLabel.Text = name;
            if (StrategicMap.Current.SeatedPlayer != 0)
            {
                var player = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
                var raceNamesPlural = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[player.PlayerRace]);
                UI.PlayerInfoWindow.RaceLabel.Text = raceNamesPlural;
            }

            var day = UI.Engine.TranslateResourceString(GameText.DayText);
            UI.PlayerInfoWindow.TurnLabel.Text = day + " " + IntToStr(StrategicMap.Current.Day);
            raceIndex = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).PlayerRace;
            if (raceIndex == -1)
            {
                raceIndex = 12;
            }

            UI.PlayerInfoWindow.RaceShield.CurrentImage = raceIndex + 13;
            UI.PlayerInfoWindow.MainWindow.Visible = true;
        }
    }

    // AoWPInfoWin.pas:190
    public void UpdateEvents()
    {
        EventList.Items.Clear();
        if (StrategicMap.Current.SeatedPlayer > -1)
        {
            StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).EventLogbook.ListEvents(EventList.Items);
        }

        var count = EventList.Items.Count;
        EventList.ListOffset = count - 1;
    }

    // AoWPInfoWin.pas:67
    public void MinClick()
    {
        UI.General.PlayButtonSound(0);
        MainWindow.Visible = false;
    }

    // AoWPInfoWin.pas:72
    public void EventListDoubleClick()
    {
        var objectValue = EventList.Items.Objects[EventList.Selected];
        StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).EventLogbook.GetLogs(ObjectToInt(objectValue)).Execute(true, ExecuteTrigger.User, null);
    }

    // AoWPInfoWin.pas:78
    public void EventListPreDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        if (drawItem != EventList.Selected)
        {
            var objectValue = EventList.Items.Objects[drawItem];
            var textColor = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).EventLogbook.GetLogs(ObjectToInt(objectValue)).GetTextColor();
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
        leftIcon = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).EventLogbook.GetLogs(ObjectToInt(objectValue)).GetLeftIcon();
        var objectValue2 = EventList.Items.Objects[drawItem];
        middleIcon = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).EventLogbook.GetLogs(ObjectToInt(objectValue2)).GetMiddleIcon();
        var objectValue3 = EventList.Items.Objects[drawItem];
        rightIcon = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).EventLogbook.GetLogs(ObjectToInt(objectValue3)).GetRightIcon();
        if (leftIcon > -1)
        {
            if (middleIcon == -1)
            {
                var eventIcon = UI.Graphics.EventIcons[leftIcon];
                UI.Graphics.EventIcons[leftIcon].ShowClipped(drawRectangle.Left + (36 - eventIcon.Width) / 2, drawRectangle.Top + 1, lockedSurface);
            }
            else
            {
                UI.Graphics.EventIcons[leftIcon].ShowClipped(drawRectangle.Left, drawRectangle.Top + 1, lockedSurface);
            }
        }

        if (middleIcon > -1)
        {
            UI.Graphics.EventIcons[middleIcon].ShowClipped(drawRectangle.Left + 12, drawRectangle.Top + 1, lockedSurface);
        }

        if (rightIcon > -1)
        {
            UI.Graphics.EventIcons[rightIcon].ShowClipped(drawRectangle.Left + 24, drawRectangle.Top + 1, lockedSurface);
        }
    }

    // AoWPInfoWin.pas:100
    public void PInfoWinResize()
    {
        this.MainWindow.Alignment.RightOffset = this.MainWindow.WindowManager.WinWidth - this.MainWindow.WinLeft - this.MainWindow.WinWidth;
        this.MainWindow.Alignment.BottomOffset = UI.General.Manager.WinHeight - this.MainWindow.WinTop - this.MainWindow.WinHeight;
    }

    // AoWPInfoWin.pas:109
    public void EventListMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (EventList.Selected > -1 && button == MouseButton.Right)
        {
            var objectValue = EventList.Items.Objects[EventList.Selected];
            StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).EventLogbook.GetLogs(ObjectToInt(objectValue)).ViewLocation();
        }
    }

    // AoWPInfoWin.pas:116
    public void CenterButtonClick()
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        UI.General.PlayButtonSound(0);
        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Leader != null)
        {
            StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetLeaderLocation(out X, out Y, out L);
            if (X > -1)
            {
                StrategicMap.Current.CenterViewTo(X, Y, L);
                StrategicMap.Current.PlayCenterAnimation(X, Y, L);
            }
        }
    }

    // AoWPInfoWin.pas:132
    public void RaceButtonClick()
    {
        UI.General.PlayButtonSound(0);
    }

    // AoWPInfoWin.pas:137
    public void FaceFrDraw(LockedSurface lockedSurface)
    {
        Rect R;
        if (StrategicMap.Current.SeatedPlayer > -1)
        {
            R = new Rect(FaceFrame.WinLeft + 3, FaceFrame.WinTop + 3, FaceFrame.WinLeft + FaceFrame.WinWidth - 3, FaceFrame.WinTop + FaceFrame.WinHeight - 3);
            if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetFace() != null)
            {
                StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetFace().ShowStretched(R, lockedSurface);
            }
        }
    }

    // AoWPInfoWin.pas:149
    public void FaceFrMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        sbyte hexX = default;
        sbyte hexY = default;
        sbyte hexLevel = default;
        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Leader != null)
        {
            StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetLeaderLocation(out hexX, out hexY, out hexLevel);
            if (hexX > -1)
            {
                var player = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
                UI.General.SelectedUnit = player.Leader;
                switch (button)
                {
                    case MouseButton.Left:
                        UI.UnitWindow.ShowUnit();
                        break;
                    case MouseButton.Right:
                        CenterButtonClick();
                        break;
                }
            }
        }
    }

    // AoWPInfoWin.pas:167
    public void HelpClick()
    {
        UI.HelpWindow.ShowHelp(502);
    }
}
