// Translated from AoWSyncDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

public partial class SynchronizeDlg : DataModule
{
    public SynchronizeDlg(Component owner) : base(owner)
    { }

    public AOWWindow Dlg;
    public AoWFrame TCeltB;
    public AOWLabel BottomMsg;
    public AOWImage LShield;
    public AOWLabel PlayerName;
    public AOWLabel InfoMsg;
    public AOWLabel PlayerRace;
    public AOWLabel Turn;
    public AOWImage RShield;
    public AoWFrame BlueFr;
    public AOWImage TCeltLineL;
    public AOWImage TCeltLineR;
    public AOWPanel NewTPnl;
    public AOWImage FaceFrame;
    public IvTranslator Translator;

    // AoWSyncDlg.pas:45
    public PlayerSetupSettings GetPlayerSetup()
    {
        PlayerSetupSettings Result;
        int Index;
        Index = DialogHosts.SetupEvents.SetupControl.FSettings.IndexOfRemoteID(DialogHosts.SetupEvents.SetupControl.FRemotePlayer.FRemoteID);
        if (Index != -1)
        {
            Result = DialogHosts.SetupEvents.SetupControl.FSettings.GetSettings(Index);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // AoWSyncDlg.pas:54
    public void FaceFramePreDraw(SurfaceDesc Surface)
    {
        PlayerSetupSettings Settings;
        Rect R;
        Settings = GetPlayerSetup();
        if (Settings != null)
        {
            R = new Rect(Dlg.WinLeft + 188, Dlg.WinTop + 40, Dlg.WinLeft + 188 + 103, Dlg.WinTop + 40 + 128);
            Settings.FLeader.ShowFace(R, Surface);
        }
    }

    // AoWSyncDlg.pas:67
    public void DlgShow()
    {
        PlayerSetupSettings Settings;
        int I;
        int Day;
        Settings = GetPlayerSetup();
        if (Settings != null)
        {
            if (!DialogHosts.SetupEvents.SetupControl.FSettings.FCustomizeLeaders)
            {
                I = DialogHosts.SetupEvents.SetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(DialogHosts.SetupEvents.SetupControl.FRemotePlayer.FRemoteID);
                if (I != -1)
                {
                    var player = DialogHosts.SetupEvents.SetupControl.FSettings.FRemotePlayers.GetPlayers(I);
                    PlayerName.Text = player.FName;
                }
                else
                {
                    PlayerName.Text = Settings.FLeader.FHeroName;
                }
            }
            else
            {
                PlayerName.Text = Settings.FLeader.FHeroName;
            }

            var raceNamesPlural = Magic.Abilities.AbilityRules.TranslateRStr(Magic.Abilities.GameTables.RaceNamesPluralRStr[Settings.FRace]);
            PlayerRace.Text = raceNamesPlural;
            Day = World.Map.AoWHSMap.Current.FDay;
            if (Day == 0)
            {
                Day = 1;
            }

            var day = Magic.Abilities.AbilityRules.TranslateRStr(Magic.Abilities.GameText.DayRStr);
            Turn.Text = day + " " + IntToStr(Day);
            LShield.CurrentImage = Settings.FRace + 26;
            RShield.CurrentImage = Settings.FRace + 26;
        }
    }
}
