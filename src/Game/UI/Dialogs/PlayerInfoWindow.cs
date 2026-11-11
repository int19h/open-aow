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
    public Button Min;
    public Label IWTitle;
    public Image SIcon;
    public ListBox EventList;
    public Label LeadL;
    public Label Turn;
    public Image RShield;
    public Panel EventFr;
    public VerticalScrollBar EventSB;
    public Panel PInfoPnl;
    public CheckBox LeaderCB;
    public Panel FaceFr;
    public Label EventLbl;
    public Panel ShieldFr;
    public Panel TurnFr;
    public Panel InfoFr;
    public Label Player;
    public Label Race;
    public CheckBox RaceCB;
    public Button Center;
    public Button Help;
    public IvTranslator Translator;

    // AoWPInfoWin.pas:173
    public void UpdatePlayer()
    {
        int RaceIndex;
        if (AoWHSMap.Current.FSeatedPlayer > -1)
        {
            var name = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetName();
            DialogHosts.PInfoW.Player.Text = name;
            if (AoWHSMap.Current.FSeatedPlayer != 0)
            {
                var player = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer);
                var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[player.FPlayerRace]);
                DialogHosts.PInfoW.Race.Text = raceNamesPlural;
            }

            var day = AbilityRules.TranslateRStr(GameText.DayRStr);
            DialogHosts.PInfoW.Turn.Text = day + " " + IntToStr(AoWHSMap.Current.FDay);
            RaceIndex = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FPlayerRace;
            if (RaceIndex == -1)
            {
                RaceIndex = 12;
            }

            DialogHosts.PInfoW.RShield.CurrentImage = RaceIndex + 13;
            DialogHosts.PInfoW.PInfoWin.Visible = true;
        }
    }

    // AoWPInfoWin.pas:190
    public void UpdateEvents()
    {
        EventList.Items.Clear();
        if (AoWHSMap.Current.FSeatedPlayer > -1)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FEventLogbook.ListEvents(EventList.Items);
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
        AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue)).Execute(true, ExecuteTrigger.User, null);
    }

    // AoWPInfoWin.pas:78
    public void EventListPreDrawItem(in Rect DrawRect, int DrawItem, ref LockedSurface SD)
    {
        if (DrawItem != EventList.Selected)
        {
            var objectValue = EventList.Items.Objects[DrawItem];
            var textColor = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue)).GetTextColor();
            EventList.TextColor = textColor;
        }
    }

    // AoWPInfoWin.pas:85
    public void EventListDrawItem(in Rect DrawRect, int DrawItem, ref LockedSurface SD)
    {
        int LeftIcon;
        int MiddleIcon;
        int RightIcon;
        var objectValue = EventList.Items.Objects[DrawItem];
        LeftIcon = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue)).GetLeftIcon();
        var objectValue2 = EventList.Items.Objects[DrawItem];
        MiddleIcon = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue2)).GetMiddleIcon();
        var objectValue3 = EventList.Items.Objects[DrawItem];
        RightIcon = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue3)).GetRightIcon();
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
            AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FEventLogbook.GetLogs(ObjectToInt(objectValue)).ViewLocation();
        }
    }

    // AoWPInfoWin.pas:116
    public void LeaderBtnClick()
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        DialogHosts.General.PlayButtonSound(0);
        if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FLeader != null)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetLeaderLocation(ref X, ref Y, ref L);
            if (X > -1)
            {
                AoWHSMap.Current.CenterViewTo(X, Y, L);
                AoWHSMap.Current.PlayCenterAnimation(X, Y, L);
            }
        }
    }

    // AoWPInfoWin.pas:132
    public void RaceBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWPInfoWin.pas:137
    public void FaceFrDraw(LockedSurface SD)
    {
        Rect R;
        if (AoWHSMap.Current.FSeatedPlayer > -1)
        {
            R = new Rect(FaceFr.WinLeft + 3, FaceFr.WinTop + 3, FaceFr.WinLeft + FaceFr.WinWidth - 3, FaceFr.WinTop + FaceFr.WinHeight - 3);
            if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetFace() != null)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetFace().ShowStretched(R, SD);
            }
        }
    }

    // AoWPInfoWin.pas:149
    public void FaceFrMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        sbyte Xhx = default;
        sbyte Yhx = default;
        sbyte Lhx = default;
        if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FLeader != null)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetLeaderLocation(ref Xhx, ref Yhx, ref Lhx);
            if (Xhx > -1)
            {
                var player = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer);
                DialogHosts.General.FSelectedUnit = player.FLeader;
                switch (Button)
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
