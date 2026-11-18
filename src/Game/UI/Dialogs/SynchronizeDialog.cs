#nullable disable
// Translated from AoWSyncDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.UI.Dialogs;

public partial class SynchronizeDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Image LeftShield { get; private set; }
    public Label PlayerNameLabel { get; private set; }
    public Label PlayerRaceLabel { get; private set; }
    public Label TurnLabel { get; private set; }
    public Image RightShield { get; private set; }

    // AoWSyncDlg.pas:45
    public PlayerSetupSettings GetPlayerSetup()
    {
        PlayerSetupSettings result;
        int index;
        index = UI.SetupEvents.SetupControl.Settings.IndexOfRemoteId(UI.SetupEvents.SetupControl.RemotePlayer.RemoteId);
        result = index != -1 ? UI.SetupEvents.SetupControl.Settings.GetSettings(index) : null;

        return result;
    }

    // AoWSyncDlg.pas:54
    public void FaceFramePreDraw(LockedSurface surface)
    {
        PlayerSetupSettings settings;
        Rect r;
        settings = GetPlayerSetup();
        if (settings != null)
        {
            r = new Rect(Window.WinLeft + 188, Window.WinTop + 40, Window.WinLeft + 188 + 103, Window.WinTop + 40 + 128);
            settings.Leader.ShowFace(r, surface);
        }
    }

    // AoWSyncDlg.pas:67
    public void DlgShow()
    {
        PlayerSetupSettings settings;
        int i;
        int day2;
        settings = GetPlayerSetup();
        if (settings != null)
        {
            if (!UI.SetupEvents.SetupControl.Settings.CustomizeLeaders)
            {
                i = UI.SetupEvents.SetupControl.Settings.RemotePlayers.IndexOfRemoteId(UI.SetupEvents.SetupControl.RemotePlayer.RemoteId);
                if (i != -1)
                {
                    RemotePlayer player = UI.SetupEvents.SetupControl.Settings.RemotePlayers.GetPlayers(i);
                    PlayerNameLabel.Text = player.Name;
                }
                else
                {
                    PlayerNameLabel.Text = settings.Leader.HeroName;
                }
            }
            else
            {
                PlayerNameLabel.Text = settings.Leader.HeroName;
            }

            string raceNamesPlural = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[settings.Race]);
            PlayerRaceLabel.Text = raceNamesPlural;
            day2 = StrategicMap.Current.Day;
            if (day2 == 0)
            {
                day2 = 1;
            }

            string day = UI.Engine.TranslateResourceString(GameText.DayText);
            TurnLabel.Text = day + " " + IntToStr(day2);
            LeftShield.CurrentImage = settings.Race + 26;
            RightShield.CurrentImage = settings.Race + 26;
        }
    }
}
