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
    public void FaceFramePreDraw(LockedSurface Surface)
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

            var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[Settings.FRace]);
            PlayerRace.Text = raceNamesPlural;
            Day = AoWHSMap.Current.FDay;
            if (Day == 0)
            {
                Day = 1;
            }

            var day = AbilityRules.TranslateRStr(GameText.DayRStr);
            Turn.Text = day + " " + IntToStr(Day);
            LShield.CurrentImage = Settings.FRace + 26;
            RShield.CurrentImage = Settings.FRace + 26;
        }
    }
}
