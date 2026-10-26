// Generated from aowTCMapEvents.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
    // { TTCMapEvents (instance size 76, VMT 00458004) }
    public partial class TTCMapEvents : Aow.Rtl.TDataModule
    {
        public Aow.Units.CMapCtrl.TAoWCombatMapControl TCMap;
        // { +0044 }
        public int FChatHeight;
        // aowTCMapEvents.pas:43
        // { draft }
        // { $48; 004584BE }
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
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TCCurPUpdate();
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

        // aowTCMapEvents.pas:105
        // { draft }
        public void TCMapDeActivateMap()
        {
            // { Tried Self identity; no exact match. }
            Aow.Units.Sound.Sound.StopSong();
            Aow.Units.Sound.Sound.SongPlayer.Resume();
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.HSMap = Aow.Units.AoWE.AoWE.AoWHSMap;
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetMap(Aow.Units.AoWE.AoWE.AoWHSMap);
            {
                var w__2 = Aow.Units.AoWSWindow.AoWSWindow.SWindow;
                w__2.Scanner.FPreviousWidth = 0;
                w__2.Scanner.FIncremental = false;
                w__2.Scanner.FSized = false;
                w__2.Scanner.FUpdateStep = -1;
            }

            if (FChatHeight > 0)
            {
                Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.WinHeight = FChatHeight;
            }

            {
                var w__3 = Aow.Units.AoWExeForms.AoWExeForms.ChatWindow;
                w__3.ChatWin.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom;
                w__3.ChatWin.Visible = false;
            }

            Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.FScrollPending = true;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapAlignWidth = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.AlignWidth;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapAlignHeight = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.AlignHeight;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Left = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.LeftOffset;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Top = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.TopOffset;
            if (Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapAlignWidth == Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
            {
                Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Right = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.RightOffset;
            }
            else
            {
                Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Right = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.WinWidth;
            }

            if (Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapAlignHeight == Aow.Units.AoWComp.TAOWAlignHeight.ahBoth)
            {
                Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Bottom = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.BottomOffset;
            }
            else
            {
                Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Bottom = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.WinHeight;
            }

            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.LeftOffset = Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldRect.Left;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.TopOffset = Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldRect.Top;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.RightOffset = Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldRect.Right;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.BottomOffset = Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldRect.Bottom;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.WinWidth = Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldRect.Right;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.WinHeight = Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldRect.Bottom;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.AlignWidth = Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldAlignWidth;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.AlignHeight = Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldAlignHeight;
            Aow.Units.AoWWinMan.AoWWinMan.General.ShowWindows();
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.FAligned = false;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TBWorld.Visible = true;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TBCombat.Visible = false;
            var call__4 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TurnButton.FButtonChecked = call__4.FSingleProcessCycle;
            Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.Visible = false;
            Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Visible = false;
            Aow.Units.AoWExeForms.AoWExeForms.TCScanner.Scanner.SetMap(null);
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
            Aow.Units.aowMapEvents.aowMapEvents.MapEvents.TheMapGemsChanged(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Aow.Units.aowMapEvents.aowMapEvents.MapEvents.TheMapIncomeChanged(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Aow.Units.aowMapEvents.aowMapEvents.MapEvents.TheMapPlayerMagicChanged(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
        }

        // aowTCMapEvents.pas:157
        // { draft }
        public void TCMapDeselected(Aow.Units.HSEngine.THexagonSprite HS)
        {
            if (HS is Aow.Units.AoWTC.TTacticalCombatUnitHS)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit = null;
                Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit = null;
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = false;
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TCUnitBtn.Visible = false;
                Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Visible = false;
            }
        }

        // aowTCMapEvents.pas:169
        public void TCMapMapChanged(in Aow.Units.AoWE.TMapChangedInfo R)
        {
            Aow.Units.AoWExeForms.AoWExeForms.TCScanner.Scanner.DrawMap(R.Bounds.Left - 8, R.Bounds.Top - 6, R.Bounds.Right - R.Bounds.Left, R.Bounds.Bottom - R.Bounds.Top);
        }

        // aowTCMapEvents.pas:174
        public void TCMapMouseOverTarget(Aow.Units.HSEngine.THexagonSprite HS)
        {
            Aow.Units.AoWE.TAbstractUnit U = default;
            if (Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Visible)
            {
                Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TargetSelPnl.Visible = HS != null;
                Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.NoTargetPnl.Visible = HS == null;
                if (HS != null)
                {
                    if (HS is Aow.Units.AoWTC.TTacticalCombatUnitHS)
                    {
                        U = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)HS).FUnit.FUnit;
                        var call__5 = U.GetName();
                        Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TargetULbl.Text = call__5;
                        var call__6 = U.GetHitPoints();
                        var call__7 = U.GetHits();
                        Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.AHits.Text = (Aow.Rtl.Rtl.IntToStr(call__6) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__7);
                        var call__8 = U.GetMovePoints();
                        var call__9 = U.GetMoves();
                        Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.AMoves.Text = (Aow.Rtl.Rtl.IntToStr(call__8) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__9);
                        var call__10 = U.GetAttack();
                        Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.AAttack.Text = Aow.Rtl.Rtl.IntToStr(call__10);
                        var call__11 = U.GetDamage();
                        Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.ADamage.Text = Aow.Rtl.Rtl.IntToStr(call__11);
                        var call__12 = U.GetDefense();
                        Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.ADefense.Text = Aow.Rtl.Rtl.IntToStr(call__12);
                        var call__13 = U.GetResistance();
                        Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.AResistance.Text = Aow.Rtl.Rtl.IntToStr(call__13);
                        var call__14 = U.GetMoveTypes();
                        var call__15 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__14);
                        Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.AMovesIcon.SetCurrent(call__15);
                        U.ListAbilities(Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.USelAbilLb.Items);
                    }
                }
            }
        }

        // aowTCMapEvents.pas:199
        public void TCMapSeatedPlayerChanged(sbyte Player)
        {
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Visible = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds;
            Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.Visible = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds;
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer == -1)
            {
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.AutoBtn.Checked = false;
            }
            else
            {
                var call__16 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer));
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.AutoBtn.Checked = call__16.FAuto;
            }

            Aow.Units.aowMapEvents.aowMapEvents.MapEvents.TheMapGemsChanged(unchecked((sbyte)Player));
            Aow.Units.aowMapEvents.aowMapEvents.MapEvents.TheMapIncomeChanged(unchecked((sbyte)Player));
            Aow.Units.aowMapEvents.aowMapEvents.MapEvents.TheMapPlayerMagicChanged(unchecked((sbyte)Player));
        }

        // aowTCMapEvents.pas:210
        public void TCMapSelected(Aow.Units.HSEngine.THexagonSprite HS)
        {
            if (HS is Aow.Units.AoWTC.TTacticalCombatUnitHS)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit = (Aow.Units.AoWTC.TTacticalCombatUnitHS)HS;
                Aow.Units.AoWTCUnit.AoWTCUnit.RefreshCombatUnit(Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit);
            }
        }

        // aowTCMapEvents.pas:219
        public void TCMapShowMessage(in Aow.Units.AoWE.TShowMessageInfo Info)
        {
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TCMsg.TextColor = Info.Color;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TCMsg.Text = Info.Text;
        }

        // aowTCMapEvents.pas:225
        public void TCMapUnitChanged(Aow.Units.AoWTC.TTacticalCombatUnitHS HS)
        {
            if ((HS == Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit) && (HS != null))
            {
                Aow.Units.AoWTCUnit.AoWTCUnit.RefreshCombatUnit(Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit);
            }
        }

        // aowTCMapEvents.pas:230
        public void TCMapCastSpell(Aow.Units.AoWE.TCombatSpellCaster Caster)
        {
            var call__17 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.SelectSpell(Caster.FCombatCasterSpellID);
            Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.UAbilLB.Selected = call__17;
        }

        // aowTCMapEvents.pas:235
        public void TCMapActivateAbility(Aow.Units.AoWE.TActivateAbilityInfo Info)
        {
            if (Info.FAbilityID == 52)
            {
                Aow.Units.AoWExeForms.AoWExeForms.SpellBook.ShowCombatCasting(Info);
            }
        }

        // aowTCMapEvents.pas:240
        public void TCMapTurnChanged()
        {
            {
                var w__18 = Aow.Units.AoWExeForms.AoWExeForms.TBWindow;
                var call__19 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentPlayer);
                w__18.TCTurnButton.FCurPlayer = call__19.FPlayerRace;
                if (w__18.TCTurnButton.FCurPlayer == -1)
                {
                    w__18.TCTurnButton.FCurPlayer = 12;
                }

                w__18.TCTurnButton.FLastPlayer = -1;
            }

            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 49;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TCPInd.FDrawn = false;
        }
    }

    public static partial class aowTCMapEvents
    {
    }
}
