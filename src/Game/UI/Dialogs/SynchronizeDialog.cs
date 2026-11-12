// Translated from AoWSyncDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

public partial class SynchronizeDialog : UIModule
{
    public SynchronizeDialog(UIContext ui) : base(ui)
    { }

    public Window Window;
    public Image LeftShield;
    public Label PlayerNameLabel;
    public Label PlayerRaceLabel;
    public Label TurnLabel;
    public Image RightShield;

    // AoWSyncDlg.pas:45
    public PlayerSetupSettings GetPlayerSetup()
    {
        PlayerSetupSettings result;
        int index;
        index = UI.SetupEvents.SetupControl.Settings.IndexOfRemoteId(UI.SetupEvents.SetupControl.RemotePlayer.RemoteId);
        if (index != -1)
        {
            result = UI.SetupEvents.SetupControl.Settings.GetSettings(index);
        }
        else
        {
            result = null;
        }

        return result;
    }

    // AoWSyncDlg.pas:54
    public void FaceFramePreDraw(LockedSurface surface)
    {
        PlayerSetupSettings settings;
        Rect R;
        settings = GetPlayerSetup();
        if (settings != null)
        {
            R = new Rect(Window.WinLeft + 188, Window.WinTop + 40, Window.WinLeft + 188 + 103, Window.WinTop + 40 + 128);
            settings.Leader.ShowFace(R, surface);
        }
    }

    // AoWSyncDlg.pas:67
    public void DlgShow()
    {
        PlayerSetupSettings settings;
        int I;
        int Day;
        settings = GetPlayerSetup();
        if (settings != null)
        {
            if (!UI.SetupEvents.SetupControl.Settings.CustomizeLeaders)
            {
                I = UI.SetupEvents.SetupControl.Settings.RemotePlayers.IndexOfRemoteId(UI.SetupEvents.SetupControl.RemotePlayer.RemoteId);
                if (I != -1)
                {
                    var player = UI.SetupEvents.SetupControl.Settings.RemotePlayers.GetPlayers(I);
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

            var raceNamesPlural = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[settings.Race]);
            PlayerRaceLabel.Text = raceNamesPlural;
            Day = StrategicMap.Current.Day;
            if (Day == 0)
            {
                Day = 1;
            }

            var day = UI.Engine.TranslateResourceString(GameText.DayText);
            TurnLabel.Text = day + " " + IntToStr(Day);
            LeftShield.CurrentImage = settings.Race + 26;
            RightShield.CurrentImage = settings.Race + 26;
        }
    }
}
