// Translated from AoWSyncDlg.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWSetup;
using static Aow.Units.AoWSetup.AoWSetup;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.aowSetupEvents;
using static Aow.Units.aowSetupEvents.aowSetupEvents;

namespace Aow.Units.AoWSyncDlg
{
    // { TSynchronizeDlg (instance size 128, VMT 00413334) }
    public partial class TSynchronizeDlg : Aow.Rtl.TDataModule
    {
        public TSynchronizeDlg(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0044 }
        public Aow.Units.AoWFrame.TAoWFrame TCeltB;
        // { +0048 }
        public Aow.Units.AoWLabel.TAOWLabel BottomMsg;
        // { +004C }
        public Aow.Units.AoWImage.TAOWImage LShield;
        // { +0050 }
        public Aow.Units.AoWLabel.TAOWLabel PlayerName;
        // { +0054 }
        public Aow.Units.AoWLabel.TAOWLabel InfoMsg;
        // { +0058 }
        public Aow.Units.AoWLabel.TAOWLabel PlayerRace;
        // { +005C }
        public Aow.Units.AoWLabel.TAOWLabel Turn;
        // { +0060 }
        public Aow.Units.AoWImage.TAOWImage RShield;
        // { +0064 }
        public Aow.Units.AoWFrame.TAoWFrame BlueFr;
        // { +0068 }
        public Aow.Units.AoWImage.TAOWImage TCeltLineL;
        // { +006C }
        public Aow.Units.AoWImage.TAOWImage TCeltLineR;
        // { +0070 }
        public Aow.Units.AoWPanel.TAOWPanel NewTPnl;
        // { +0074 }
        public Aow.Units.AoWImage.TAOWImage FaceFrame;
        // { +0078 }
        public Aow.Rtl.TIvTranslator Translator;
        // AoWSyncDlg.pas:45
        // { +007C }
        public Aow.Units.AoWSetup.TPlayerSetupSettings GetPlayerSetup()
        {
            Aow.Units.AoWSetup.TPlayerSetupSettings Result = default;
            int Index = default;
            var call__1 = Aow.Units.aowSetupEvents.aowSetupEvents.SetupEvents.SetupControl.FSettings.IndexOfRemoteID(Aow.Units.aowSetupEvents.aowSetupEvents.SetupEvents.SetupControl.FRemotePlayer.FRemoteID);
            Index = call__1;
            if (Index != -1)
            {
                var call__2 = Aow.Units.aowSetupEvents.aowSetupEvents.SetupEvents.SetupControl.FSettings.GetSettings(Index);
                Result = call__2;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // AoWSyncDlg.pas:54
        public void FaceFramePreDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Units.AoWSetup.TPlayerSetupSettings Settings = default;
            Aow.Rtl.TRect R = default;
            Settings = GetPlayerSetup();
            if (Settings != null)
            {
                var call__3 = Aow.Rtl.Rtl.Rect(Dlg.WinLeft + 188, Dlg.WinTop + 40, (Dlg.WinLeft + 188) + 103, (Dlg.WinTop + 40) + 128);
                R = call__3;
                Settings.FLeader.ShowFace(R, Surface);
            }
        }

        // AoWSyncDlg.pas:67
        public void DlgShow()
        {
            Aow.Units.AoWSetup.TPlayerSetupSettings Settings = default;
            int I = default;
            int Day = default;
            Settings = GetPlayerSetup();
            if (Settings != null)
            {
                if (!Aow.Units.aowSetupEvents.aowSetupEvents.SetupEvents.SetupControl.FSettings.FCustomizeLeaders)
                {
                    var call__4 = Aow.Units.aowSetupEvents.aowSetupEvents.SetupEvents.SetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(Aow.Units.aowSetupEvents.aowSetupEvents.SetupEvents.SetupControl.FRemotePlayer.FRemoteID);
                    I = call__4;
                    if (I != -1)
                    {
                        var call__5 = Aow.Units.aowSetupEvents.aowSetupEvents.SetupEvents.SetupControl.FSettings.FRemotePlayers.GetPlayers(I);
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

                var call__6 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[Settings.FRace]);
                PlayerRace.Text = call__6;
                Day = Aow.Units.AoWE.AoWE.AoWHSMap.FDay;
                if (Day == 0)
                {
                    Day = 1;
                }

                var call__7 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DayRStr);
                Turn.Text = (call__7 + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(Day);
                LShield.CurrentImage = Settings.FRace + 26;
                RShield.CurrentImage = Settings.FRace + 26;
            }
        }
    }

    public static partial class AoWSyncDlg
    {
    }
}
