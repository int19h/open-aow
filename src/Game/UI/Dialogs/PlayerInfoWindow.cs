#nullable disable warnings
// Translated from AoWPInfoWin.pas.
using System.Globalization;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.UI.Dialogs;

public sealed partial class PlayerInfoWindow(UIContext ui) : UIModule(ui)
{
    public Window MainWindow { get; private set; }
    public ListBox EventList { get; private set; }
    public Label OfTheLabel { get; private set; }
    public Label TurnLabel { get; private set; }
    public Image RaceShield { get; private set; }
    public Panel FaceFrame { get; private set; }
    public Label PlayerLabel { get; private set; }
    public Label RaceLabel { get; private set; }

    // AoWPInfoWin.pas:173
    public void UpdatePlayer()
    {
        if (StrategicMap.Current.SeatedPlayer > -1)
        {
            string name = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetName();
            UI.PlayerInfoWindow.PlayerLabel.Text = name;
            if (StrategicMap.Current.SeatedPlayer != 0)
            {
                Player? player = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
                string raceNamesPlural = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[player.PlayerRace]);
                UI.PlayerInfoWindow.RaceLabel.Text = raceNamesPlural;
            }

            string day = UI.Engine.TranslateResourceString(GameText.DayText);
            UI.PlayerInfoWindow.TurnLabel.Text = $"{day} {StrategicMap.Current.Day.ToString(CultureInfo.InvariantCulture)}";
            int raceIndex = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).PlayerRace;
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

        int count = EventList.Items.Count;
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
        object? objectValue = EventList.Items.Objects[EventList.Selected];
        _ = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).EventLogbook[ObjectToInt(objectValue)].Execute(true, trigger: ExecuteTrigger.User, done: null);
    }

    // AoWPInfoWin.pas:78
    public void EventListPreDrawItem(in Rect drawRectangle, int drawItem, ref readonly LockedSurface lockedSurface)
    {
        if (drawItem != EventList.Selected)
        {
            object? objectValue = EventList.Items.Objects[drawItem];
            int textColor = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).EventLogbook[ObjectToInt(objectValue)].GetTextColor();
            EventList.TextColor = textColor;
        }
    }

    // AoWPInfoWin.pas:85
    public void EventListDrawItem(in Rect drawRectangle, int drawItem, ref readonly LockedSurface lockedSurface)
    {
        object? objectValue = EventList.Items.Objects[drawItem];
        int leftIcon = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).EventLogbook[ObjectToInt(objectValue)].LeftIcon;
        object? objectValue2 = EventList.Items.Objects[drawItem];
        int middleIcon = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).EventLogbook[ObjectToInt(objectValue2)].MiddleIcon;
        object? objectValue3 = EventList.Items.Objects[drawItem];
        int rightIcon = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).EventLogbook[ObjectToInt(objectValue3)].RightIcon;
        if (leftIcon > -1)
        {
            if (middleIcon == -1)
            {
                LibraryImage? eventIcon = UI.Graphics.EventIcons[leftIcon];
                UI.Graphics.EventIcons[leftIcon].ShowClipped(drawRectangle.Left + ((36 - eventIcon.Width) / 2), drawRectangle.Top + 1, lockedSurface);
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
        MainWindow.Alignment.RightOffset = MainWindow.WindowManager.WinWidth - MainWindow.WinLeft - MainWindow.WinWidth;
        MainWindow.Alignment.BottomOffset = UI.General.Manager.WinHeight - MainWindow.WinTop - MainWindow.WinHeight;
    }

    // AoWPInfoWin.pas:109
    public void EventListMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (EventList.Selected > -1 && button == MouseButton.Right)
        {
            object? objectValue = EventList.Items.Objects[EventList.Selected];
            StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).EventLogbook[ObjectToInt(objectValue)].ViewLocation();
        }
    }

    // AoWPInfoWin.pas:116
    public void CenterButtonClick()
    {
        UI.General.PlayButtonSound(0);
        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Leader is not null)
        {
            StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetLeaderLocation(out sbyte x, out sbyte y, out sbyte l);
            if (x > -1)
            {
                StrategicMap.Current.CenterViewTo(x, y, l);
                StrategicMap.Current.PlayCenterAnimation(x, y, l);
            }
        }
    }

    // AoWPInfoWin.pas:132
    public void RaceButtonClick() => UI.General.PlayButtonSound(0);

    // AoWPInfoWin.pas:137
    public void FaceFrDraw(LockedSurface lockedSurface)
    {
        if (StrategicMap.Current.SeatedPlayer > -1)
        {
            Rect r = new(FaceFrame.AbsoluteLeft + 3, FaceFrame.AbsoluteTop + 3, FaceFrame.AbsoluteLeft + FaceFrame.WinWidth - 3, FaceFrame.AbsoluteTop + FaceFrame.WinHeight - 3);
            StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetFace()?.ShowStretched(r, lockedSurface);
        }
    }

    // AoWPInfoWin.pas:149
    public void FaceFrMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Leader is not null)
        {
            StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetLeaderLocation(out sbyte hexX, out _, L: out _);
            if (hexX > -1)
            {
                Player? player = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
                UI.General.SelectedUnit = player.Leader;
                switch (button)
                {
                    case MouseButton.Left:
                        UI.UnitWindow.ShowUnit();
                        break;
                    case MouseButton.Right:
                        CenterButtonClick();
                        break;
                    case MouseButton.Middle:
                    default:
                        break;
                }
            }
        }
    }

    // AoWPInfoWin.pas:167
    public void HelpClick() => UI.HelpWindow.ShowHelp(502);
}
