#nullable disable
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
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.CMapCtrl;
using static Aow.Units.CMapCtrl.CMapCtrl;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AOWMWindow;
using static Aow.Units.AOWMWindow.AOWMWindow;
using Aow.Units.AoWTCUnit;
using static Aow.Units.AoWTCUnit.AoWTCUnit;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.aowMapEvents;
using static Aow.Units.aowMapEvents.aowMapEvents;
using Aow.Units.E1Globals;
using static Aow.Units.E1Globals.E1Globals;
using Aow.Units.E1GameFlow;
using static Aow.Units.E1GameFlow.E1GameFlow;
using Aow.Units.E1TacticalSupport;
using static Aow.Units.E1TacticalSupport.E1TacticalSupport;
namespace Aow.Units.aowTCMapEvents
{
    public partial class TTCMapEvents
    {
        public void TCMapActivateMap()
        {
            Aow.Units.Sound.Sound.SongPlayer.Pause(false);
            if (Aow.Units.AoWE.AoWE.AoWEngine.FDemo)
            {
                Aow.Units.Sound.Sound.PlaySongEx((AnsiString)"Songs\\Combat\\Defend.IT", 2, 0, true);
            }
            else
            {
                switch (Aow.Rtl.Rtl.Random(3))
                {
                    case 0:
                        Aow.Units.Sound.Sound.PlaySongEx((AnsiString)"Songs\\Combat\\Defend.IT", 2, 0, true);
                        break;
                    case 1:
                        Aow.Units.Sound.Sound.PlaySongEx((AnsiString)"Songs\\Combat\\Outnum.IT", 2, 0, true);
                        break;
                    case 2:
                        Aow.Units.Sound.Sound.PlaySongEx((AnsiString)"Songs\\Combat\\Races.IT", 2, 0, true);
                        break;
                }
            }

            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.HSMap = Aow.Units.AoWTC.AoWTC.AoWCombatMap;
            Aow.Units.AoWExeForms.AoWExeForms.TCScanner.Scanner.SetMap(Aow.Units.AoWTC.AoWTC.AoWCombatMap);
            {
                var w__1 = Aow.Units.AoWExeForms.AoWExeForms.TCScanner;
                w__1.Scanner.FPreviousWidth = 0;
                w__1.Scanner.FIncremental = false;
                w__1.Scanner.FSized = false;
                w__1.Scanner.FUpdateStep = -1;
            }

            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Visible = true;
            Aow.Units.AoWWinMan.AoWWinMan.General.HideWindows();
            Aow.Units.AoWTC.AoWTC.AoWCombatMap = (Aow.Units.AoWTC.TAoWCombatMap)Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Visible = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldAlignWidth = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.AlignWidth;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldAlignHeight = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.AlignHeight;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldRect.Left = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.LeftOffset;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldRect.Top = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.TopOffset;
            if (Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldAlignWidth == Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
            {
                Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldRect.Right = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.RightOffset;
            }
            else
            {
                Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldRect.Right = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.WinWidth;
            }

            if (Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldAlignHeight == Aow.Units.AoWComp.TAOWAlignHeight.ahBoth)
            {
                Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldRect.Bottom = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.BottomOffset;
            }
            else
            {
                Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldRect.Bottom = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.WinHeight;
            }

            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.LeftOffset = Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Left;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.TopOffset = Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Top;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.RightOffset = Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Right;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.BottomOffset = Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Bottom;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.WinWidth = Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Right;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.WinHeight = Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Bottom;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.AlignWidth = Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapAlignWidth;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.AlignHeight = Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapAlignHeight;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.FAligned = false;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TBWorld.Visible = false;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TBCombat.Visible = true;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.UpdateCombatPlayerIndicator();
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.AutoBtn.Checked = false;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TCChatBtn.Visible = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.ChatBtn.Visible;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TCUnitBtn.Visible = false;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.FDrawn = false;
            Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.Visible = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TCScannerBtn.Visible = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds;
            Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Visible = false;
            if (!Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
            {
                FChatHeight = Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.WinHeight;
                Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth;
            }
            else
            {
                FChatHeight = 0;
            }

            if (Aow.Units.AoWWinMan.AoWWinMan.General.FChatVisible || !Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
            {
                Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Visible = true;
            }
        }
    }
}
