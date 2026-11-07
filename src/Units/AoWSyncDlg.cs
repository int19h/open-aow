// Translated from AoWSyncDlg.pas.
using Aow.Rtl;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWPanel;
using Aow.Units.AoWSetup;
using Aow.Units.AoWWindow;
using Aow.Units.GFXE;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.aowSetupEvents.aowSetupEventsUnit;

namespace Aow.Units.AoWSyncDlg;

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
        Index = SetupEvents.SetupControl.FSettings.IndexOfRemoteID(SetupEvents.SetupControl.FRemotePlayer.FRemoteID);
        if (Index != -1)
        {
            Result = SetupEvents.SetupControl.FSettings.GetSettings(Index);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // AoWSyncDlg.pas:54
    public void FaceFramePreDraw(TObject Sender, TSurfaceDesc Surface)
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
            if (!SetupEvents.SetupControl.FSettings.FCustomizeLeaders)
            {
                I = SetupEvents.SetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(SetupEvents.SetupControl.FRemotePlayer.FRemoteID);
                if (I != -1)
                {
                    var player = SetupEvents.SetupControl.FSettings.FRemotePlayers.GetPlayers(I);
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

            var raceNamesPlural = TranslateRStr(RaceNamesPluralRStr[Settings.FRace]);
            PlayerRace.Text = raceNamesPlural;
            Day = AoWHSMap.FDay;
            if (Day == 0)
            {
                Day = 1;
            }

            var day = TranslateRStr(DayRStr);
            Turn.Text = day + " " + IntToStr(Day);
            LShield.CurrentImage = Settings.FRace + 26;
            RShield.CurrentImage = Settings.FRace + 26;
        }
    }
}

public static partial class AoWSyncDlgUnit
{
}
