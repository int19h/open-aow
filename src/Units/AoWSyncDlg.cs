// Translated from AoWSyncDlg.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWSetup;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.aowSetupEvents.aowSetupEvents;

namespace Aow.Units.AoWSyncDlg
{
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
            TPlayerSetupSettings Result = default;
            int Index = default;
            var call__1 = SetupEvents.SetupControl.FSettings.IndexOfRemoteID(SetupEvents.SetupControl.FRemotePlayer.FRemoteID);
            Index = call__1;
            if (Index != -1)
            {
                var call__2 = SetupEvents.SetupControl.FSettings.GetSettings(Index);
                Result = call__2;
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
            TPlayerSetupSettings Settings = default;
            TRect R = default;
            Settings = GetPlayerSetup();
            if (Settings != null)
            {
                var call__3 = Rect(Dlg.WinLeft + 188, Dlg.WinTop + 40, Dlg.WinLeft + 188 + 103, Dlg.WinTop + 40 + 128);
                R = call__3;
                Settings.FLeader.ShowFace(R, Surface);
            }
        }

        // AoWSyncDlg.pas:67
        public void DlgShow()
        {
            TPlayerSetupSettings Settings = default;
            int I = default;
            int Day = default;
            Settings = GetPlayerSetup();
            if (Settings != null)
            {
                if (!SetupEvents.SetupControl.FSettings.FCustomizeLeaders)
                {
                    var call__4 = SetupEvents.SetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(SetupEvents.SetupControl.FRemotePlayer.FRemoteID);
                    I = call__4;
                    if (I != -1)
                    {
                        var call__5 = SetupEvents.SetupControl.FSettings.FRemotePlayers.GetPlayers(I);
                        PlayerName.Text = call__5.FName;
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

                var call__6 = TranslateRStr(RaceNamesPluralRStr[Settings.FRace]);
                PlayerRace.Text = call__6;
                Day = AoWHSMap.FDay;
                if (Day == 0)
                {
                    Day = 1;
                }

                var call__7 = TranslateRStr(DayRStr);
                Turn.Text = call__7 + AnsiString.FromBytes(32) + IntToStr(Day);
                LShield.CurrentImage = Settings.FRace + 26;
                RShield.CurrentImage = Settings.FRace + 26;
            }
        }
    }

    public static partial class AoWSyncDlg
    {
    }
}
