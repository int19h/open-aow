// Translated from AoWSyncDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWSyncDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

public partial class TSynchronizeDlg : TDataModule
{
    public TSynchronizeDlg(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dlg;
    public TAoWFrame TCeltB;
    public TAOWLabel BottomMsg;
    public TAOWImage LShield;
    public TAOWLabel PlayerName;
    public TAOWLabel InfoMsg;
    public TAOWLabel PlayerRace;
    public TAOWLabel Turn;
    public TAOWImage RShield;
    public TAoWFrame BlueFr;
    public TAOWImage TCeltLineL;
    public TAOWImage TCeltLineR;
    public TAOWPanel NewTPnl;
    public TAOWImage FaceFrame;
    public TIvTranslator Translator;

    // AoWSyncDlg.pas:45
    public TPlayerSetupSettings GetPlayerSetup()
    {
        TPlayerSetupSettings Result;
        int Index;
        Index = Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.SetupControl.FSettings.IndexOfRemoteID(Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.SetupControl.FRemotePlayer.FRemoteID);
        if (Index != -1)
        {
            Result = Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.SetupControl.FSettings.GetSettings(Index);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // AoWSyncDlg.pas:54
    public void FaceFramePreDraw(TSurfaceDesc Surface)
    {
        TPlayerSetupSettings Settings;
        TRect R;
        Settings = GetPlayerSetup();
        if (Settings != null)
        {
            R = Rect(Dlg.WinLeft + 188, Dlg.WinTop + 40, Dlg.WinLeft + 188 + 103, Dlg.WinTop + 40 + 128);
            Settings.FLeader.ShowFace(R, Surface);
        }
    }

    // AoWSyncDlg.pas:67
    public void DlgShow()
    {
        TPlayerSetupSettings Settings;
        int I;
        int Day;
        Settings = GetPlayerSetup();
        if (Settings != null)
        {
            if (!Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.SetupControl.FSettings.FCustomizeLeaders)
            {
                I = Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.SetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.SetupControl.FRemotePlayer.FRemoteID);
                if (I != -1)
                {
                    var player = Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.SetupControl.FSettings.FRemotePlayers.GetPlayers(I);
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

            var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.RaceNamesPluralRStr[Settings.FRace]);
            PlayerRace.Text = raceNamesPlural;
            Day = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FDay;
            if (Day == 0)
            {
                Day = 1;
            }

            var day = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.DayRStr);
            Turn.Text = day + " " + IntToStr(Day);
            LShield.CurrentImage = Settings.FRace + 26;
            RShield.CurrentImage = Settings.FRace + 26;
        }
    }
}
