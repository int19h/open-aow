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
using Aow.Graphics;
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
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        if (PlayerIdExtensions.ToOriginal(strategicMap.SeatedPlayer) > -1)
        {
            string name = strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer).GetName();
            UI.PlayerInfoWindow.PlayerLabel.Text = name;
            StrategicMap engineStrategicMap = UI.Engine.StrategicMap;
            if (engineStrategicMap.SeatedPlayer != PlayerId.Independent)
            {
                Player? player = engineStrategicMap.Players.GetPlayers(engineStrategicMap.SeatedPlayer);
                string raceNamesPlural = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[player.PlayerRace]);
                UI.PlayerInfoWindow.RaceLabel.Text = raceNamesPlural;
            }

            string day = UI.Engine.TranslateResourceString(GameText.DayText);
            StrategicMap toStringStrategicMap = UI.Engine.StrategicMap;
            UI.PlayerInfoWindow.TurnLabel.Text = $"{day} {toStringStrategicMap.Day.ToString(CultureInfo.InvariantCulture)}";
            int raceIndex = toStringStrategicMap.Players.GetPlayers(toStringStrategicMap.SeatedPlayer).PlayerRace;
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
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        if (PlayerIdExtensions.ToOriginal(strategicMap.SeatedPlayer) > -1)
        {
            strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer).EventLogbook.ListEvents(EventList.Items);
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
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        _ = strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer).EventLogbook[ObjectToInt(objectValue)].Execute(true, trigger: ExecuteTrigger.User, done: null);
    }

    // AoWPInfoWin.pas:78
    public void EventListPreDrawItem(in Rect drawRectangle, int drawItem, ref readonly LockedSurface lockedSurface)
    {
        if (drawItem != EventList.Selected)
        {
            object? objectValue = EventList.Items.Objects[drawItem];
            StrategicMap strategicMap = UI.Engine.StrategicMap;
            Color textColor = strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer).EventLogbook[ObjectToInt(objectValue)].GetTextColor();
            EventList.TextColor = textColor;
        }
    }

    // AoWPInfoWin.pas:85
    public void EventListDrawItem(in Rect drawRectangle, int drawItem, ref readonly LockedSurface lockedSurface)
    {
        object? objectValue = EventList.Items.Objects[drawItem];
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        int leftIcon = strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer).EventLogbook[ObjectToInt(objectValue)].LeftIcon;
        object? objectValue2 = EventList.Items.Objects[drawItem];
        StrategicMap engineStrategicMap = UI.Engine.StrategicMap;
        int middleIcon = engineStrategicMap.Players.GetPlayers(engineStrategicMap.SeatedPlayer).EventLogbook[ObjectToInt(objectValue2)].MiddleIcon;
        object? objectValue3 = EventList.Items.Objects[drawItem];
        StrategicMap getPlayersStrategicMap = UI.Engine.StrategicMap;
        int rightIcon = getPlayersStrategicMap.Players.GetPlayers(getPlayersStrategicMap.SeatedPlayer).EventLogbook[ObjectToInt(objectValue3)].RightIcon;
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
            StrategicMap strategicMap = UI.Engine.StrategicMap;
            strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer).EventLogbook[ObjectToInt(objectValue)].ViewLocation();
        }
    }

    // AoWPInfoWin.pas:116
    public void CenterButtonClick()
    {
        UI.General.PlayButtonSound(0);
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        if (strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer).Leader is not null)
        {
            strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer).GetLeaderLocation(out int x, out int y, out int l);
            if (x > -1)
            {
                strategicMap.CenterViewTo(x, y, l);
                UI.Engine.StrategicMap.PlayCenterAnimation(x, y, l);
            }
        }
    }

    // AoWPInfoWin.pas:132
    public void RaceButtonClick() => UI.General.PlayButtonSound(0);

    // AoWPInfoWin.pas:137
    public void FaceFrDraw(LockedSurface lockedSurface)
    {
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        if (PlayerIdExtensions.ToOriginal(strategicMap.SeatedPlayer) > -1)
        {
            Rect r = new(FaceFrame.AbsoluteLeft + 3, FaceFrame.AbsoluteTop + 3, FaceFrame.AbsoluteLeft + FaceFrame.WinWidth - 3, FaceFrame.AbsoluteTop + FaceFrame.WinHeight - 3);
            strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer).GetFace()?.ShowStretched(r, lockedSurface);
        }
    }

    // AoWPInfoWin.pas:149
    public void FaceFrMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        if (strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer).Leader is not null)
        {
            strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer).GetLeaderLocation(out int hexX, out _, L: out _);
            if (hexX > -1)
            {
                Player? player = strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer);
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
