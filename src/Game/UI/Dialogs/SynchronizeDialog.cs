// Translated from AoWSyncDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

public partial class SynchronizeDialog : DataModule
{
    public SynchronizeDialog(Component owner) : base(owner)
    { }

    public Window Dlg;
    public Frame TCeltB;
    public Label BottomMsg;
    public Image LShield;
    public Label PlayerName;
    public Label InfoMsg;
    public Label PlayerRace;
    public Label Turn;
    public Image RShield;
    public Frame BlueFr;
    public Image TCeltLineL;
    public Image TCeltLineR;
    public Panel NewTPnl;
    public Image FaceFrame;
    public IvTranslator Translator;

    // AoWSyncDlg.pas:45
    public PlayerSetupSettings GetPlayerSetup()
    {
        PlayerSetupSettings result;
        int index;
        index = DialogHosts.SetupEvents.SetupControl.Settings.IndexOfRemoteId(DialogHosts.SetupEvents.SetupControl.RemotePlayer.RemoteId);
        if (index != -1)
        {
            result = DialogHosts.SetupEvents.SetupControl.Settings.GetSettings(index);
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
            R = new Rect(Dlg.WinLeft + 188, Dlg.WinTop + 40, Dlg.WinLeft + 188 + 103, Dlg.WinTop + 40 + 128);
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
            if (!DialogHosts.SetupEvents.SetupControl.Settings.CustomizeLeaders)
            {
                I = DialogHosts.SetupEvents.SetupControl.Settings.RemotePlayers.IndexOfRemoteId(DialogHosts.SetupEvents.SetupControl.RemotePlayer.RemoteId);
                if (I != -1)
                {
                    var player = DialogHosts.SetupEvents.SetupControl.Settings.RemotePlayers.GetPlayers(I);
                    PlayerName.Text = player.Name;
                }
                else
                {
                    PlayerName.Text = settings.Leader.HeroName;
                }
            }
            else
            {
                PlayerName.Text = settings.Leader.HeroName;
            }

            var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[settings.Race]);
            PlayerRace.Text = raceNamesPlural;
            Day = AoWHSMap.Current.Day;
            if (Day == 0)
            {
                Day = 1;
            }

            var day = AbilityRules.TranslateResourceString(GameText.DayText);
            Turn.Text = day + " " + IntToStr(Day);
            LShield.CurrentImage = settings.Race + 26;
            RShield.CurrentImage = settings.Race + 26;
        }
    }
}
