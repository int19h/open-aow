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
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWTabPanel;
using static Aow.Units.AoWTabPanel.AoWTabPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.E2Globals;
using static Aow.Units.E2Globals.E2Globals;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWInfoBanner;
using static Aow.Units.AoWInfoBanner.AoWInfoBanner;
namespace Aow.Units.AoWUnitWindow
{
    public partial class TUnitWindow
    {
        public void ShowUnit()
        {
            Aow.Units.AoWE.THero H = default;
            Aow.Units.AoWE.TAbstractUnit U = default;
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit != null)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.BuildAbilities();
                U = Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit;
                if ((U != null) && (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == U.FPlayer))
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(64, true, true);
                }

                if (U is Aow.Units.AoWE.THero)
                {
                    H = (Aow.Units.AoWE.THero)U;
                }
                else
                {
                    H = null;
                }

                bool bool__1 = (Aow.Units.AoWTC.AoWTC.AoWCombatMap == null) && (Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList != null);
                if (bool__1)
                {
                    var call__2 = Aow.Units.AoWWinMan.AoWWinMan.General.IndexOfUnit(U);
                    bool__1 = (call__2 > -1);
                }

                if (bool__1 && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[1] > -1))
                {
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.PPrev.Visible = true;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.PNext.Visible = true;
                }
                else
                {
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.PPrev.Visible = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.PNext.Visible = false;
                }

                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Mana.Visible = false;
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ManaIcon.Visible = false;
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Items.Clear();
                var call__3 = Aow.Units.AoWUnitWindow.AoWUnitWindow.AbilitiesRStr;
                var call__4 = Aow.Units.AoWE.AoWE.TranslateRStr(call__3);
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Items.Add(call__4);
                var call__5 = Aow.Units.AoWUnitWindow.AoWUnitWindow.InfoRStr;
                var call__6 = Aow.Units.AoWE.AoWE.TranslateRStr(call__5);
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Items.Add(call__6);
                if (U.GetRace() > -1)
                {
                    var call__7 = U.GetRace();
                    var call__8 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesRStr[call__7]);
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text = call__8;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RelationIcn.Visible = true;
                    var call__9 = U.GetRace();
                    var call__10 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(call__9);
                    var call__11 = call__10.GetPlayerRelation(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RelationIcn.CurrentImage = ((int)call__11) + 11;
                    var call__12 = U.GetRace();
                    var call__13 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(call__12);
                    var call__14 = call__13.GetPlayerRelation(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    var call__15 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TStatusRStr[(int)call__14]);
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RelationV.Text = call__15;
                }
                else
                {
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text = (AnsiString)"";
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RelationIcn.Visible = false;
                    var call__16 = Aow.Units.AoWUnitWindow.AoWUnitWindow.NoneRStr;
                    var call__17 = Aow.Units.AoWE.AoWE.TranslateRStr(call__16);
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RelationV.Text = call__17;
                }

                if (((Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == U.FPlayer) && !(U is Aow.Units.AoWE.TLeader)) && (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null))
                {
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.DisbandBtn.Visible = true;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BCloseBtn.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
                }
                else
                {
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.DisbandBtn.Visible = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BCloseBtn.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter;
                }

                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BCloseBtn.FAligned = false;
                switch (U.GetUnitType())
                {
                    case Aow.Units.AoWE.TUnitType.utCreature:
                        var call__18 = Aow.Units.AoWUnitWindow.AoWUnitWindow.CreatureRStr;
                        var call__19 = Aow.Units.AoWE.AoWE.TranslateRStr(call__18);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text = (Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32)) + call__19;
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeIcn.CurrentImage = 21;
                        break;
                    case Aow.Units.AoWE.TUnitType.utMachine:
                        var call__20 = Aow.Units.AoWUnitWindow.AoWUnitWindow.MachineRStr;
                        var call__21 = Aow.Units.AoWE.AoWE.TranslateRStr(call__20);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text = (Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32)) + call__21;
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeIcn.CurrentImage = 22;
                        break;
                    default:
                        if (H == null)
                        {
                            var call__22 = Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitRStr;
                            var call__23 = Aow.Units.AoWE.AoWE.TranslateRStr(call__22);
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text = (Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32)) + call__23;
                        }
                        else
                        {
                            if (H is Aow.Units.AoWE.TLeader)
                            {
                                var call__24 = Aow.Units.AoWUnitWindow.AoWUnitWindow.LeaderRStr;
                                var call__25 = Aow.Units.AoWE.AoWE.TranslateRStr(call__24);
                                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text = (Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32)) + call__25;
                            }
                            else
                            {
                                var call__26 = Aow.Units.AoWUnitWindow.AoWUnitWindow.HeroRStr;
                                var call__27 = Aow.Units.AoWE.AoWE.TranslateRStr(call__26);
                                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text = (Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32)) + call__27;
                            }
                        }

                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeIcn.CurrentImage = 23;
                        break;
                }

                if (H == null)
                {
                    var call__28 = Aow.Units.AoWUnitWindow.AoWUnitWindow.LevelRStr;
                    var call__29 = Aow.Units.AoWE.AoWE.TranslateRStr(call__28);
                    var call__30 = U.GetUnitLevel();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Level.Text = (call__29 + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(call__30);
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T2Leader.Visible = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T2Text.Visible = true;
                    var call__31 = U.GetDescription();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T2Memo.Items.Assign(call__31);
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T2SBar.Position = 0;
                    var call__32 = U.GetName();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitName.Text = call__32;
                }
                else
                {
                    if (H.FPlayer == 0)
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.SelPnl.Visible = false;
                        var call__33 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWUnitWindow.AoWUnitWindow.NoLeaderRStr);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.SphereLbl.Text = call__33;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.SelPnl.Visible = true;
                        var call__34 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__35 = call__34.FLeader.GetSphereCount();
                        if (call__35 > 0)
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel1.Visible = true;
                            var call__36 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__37 = call__36.FLeader.GetSpheres(0);
                            var call__38 = Aow.Units.AoWWinMan.AoWWinMan.General.SphereToImage(call__37);
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel1.ILIndexUp = call__38;
                        }
                        else
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel1.Visible = false;
                        }

                        var call__39 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__40 = call__39.FLeader.GetSphereCount();
                        if (call__40 > 1)
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel2.Visible = true;
                            var call__41 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__42 = call__41.FLeader.GetSpheres(1);
                            var call__43 = Aow.Units.AoWWinMan.AoWWinMan.General.SphereToImage(call__42);
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel2.ILIndexUp = call__43;
                        }
                        else
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel2.Visible = false;
                        }

                        var call__44 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__45 = call__44.FLeader.GetSphereCount();
                        if (call__45 > 2)
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel3.Visible = true;
                            var call__46 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__47 = call__46.FLeader.GetSpheres(2);
                            var call__48 = Aow.Units.AoWWinMan.AoWWinMan.General.SphereToImage(call__47);
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel3.ILIndexUp = call__48;
                        }
                        else
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel3.Visible = false;
                        }

                        var call__49 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__50 = call__49.FLeader.GetSphereCount();
                        if (call__50 > 3)
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel4.Visible = true;
                            var call__51 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__52 = call__51.FLeader.GetSpheres(3);
                            var call__53 = Aow.Units.AoWWinMan.AoWWinMan.General.SphereToImage(call__52);
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel4.ILIndexUp = call__53;
                        }
                        else
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel4.Visible = false;
                        }

                        var call__54 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__55 = call__54.FLeader.GetSphereCount();
                        if (call__55 > 4)
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel5.Visible = true;
                            var call__56 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__57 = call__56.FLeader.GetSpheres(4);
                            var call__58 = Aow.Units.AoWWinMan.AoWWinMan.General.SphereToImage(call__57);
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel5.ILIndexUp = call__58;
                        }
                        else
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel5.Visible = false;
                        }

                        var call__59 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__60 = call__59.FLeader.GetSphereCount();
                        if (call__60 > 5)
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel6.Visible = true;
                            var call__61 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__62 = call__61.FLeader.GetSpheres(5);
                            var call__63 = Aow.Units.AoWWinMan.AoWWinMan.General.SphereToImage(call__62);
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel6.ILIndexUp = call__63;
                        }
                        else
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel6.Visible = false;
                        }

                        var call__64 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__65 = call__64.FLeader.GetSphereCount();
                        if (call__65 > 6)
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel7.Visible = true;
                            var call__66 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__67 = call__66.FLeader.GetSpheres(6);
                            var call__68 = Aow.Units.AoWWinMan.AoWWinMan.General.SphereToImage(call__67);
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel7.ILIndexUp = call__68;
                        }
                        else
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel7.Visible = false;
                        }

                        var call__69 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWUnitWindow.AoWUnitWindow.LeaderSpheresRStr);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.SphereLbl.Text = call__69;
                    }

                    bool bool__70 = ((Aow.Units.AoWE.AoWE.AoWHSMap.FCombat == null) && (H.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    if (!bool__70)
                    {
                        bool bool__71 = (Aow.Units.AoWE.AoWE.AoWHSMap.FCombat != null);
                        if (bool__71)
                        {
                            var call__72 = Aow.Units.AoWE.AoWE.AoWHSMap.FCombat.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FCombat.FCombatData.FCurrentPlayer));
                            bool__71 = (call__72.FPlayer == H.FPlayer);
                        }

                        bool__70 = bool__71;
                    }

                    if (bool__70)
                    {
                        var call__73 = Aow.Units.AoWUnitWindow.AoWUnitWindow.ItemsRStr;
                        var call__74 = Aow.Units.AoWE.AoWE.TranslateRStr(call__73);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Items.Add(call__74);
                    }

                    if (H.GetCastingPointsMax() > 0)
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Mana.Visible = true;
                        var call__75 = H.GetCastingPoints();
                        var call__76 = H.GetCastingPointsMax();
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Mana.Text = (Aow.Rtl.Rtl.IntToStr(call__75) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__76);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ManaIcon.Visible = true;
                    }

                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ItemUseBtn.Visible = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.FGroundOffset = 0;
                    var call__77 = Aow.Units.AoWUnitWindow.AoWUnitWindow.LevelRStr;
                    var call__78 = Aow.Units.AoWE.AoWE.TranslateRStr(call__77);
                    var call__79 = H.GetLevel();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Level.Text = (call__78 + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(call__79);
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T2Leader.Visible = true;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T2Text.Visible = false;
                    var call__80 = H.GetNickname();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitName.Text = (H.FHeroName + AnsiString.FromBytes(32)) + call__80;
                }

                var call__81 = Aow.Units.AoWUnitWindow.AoWUnitWindow.ExpRStr;
                var call__82 = Aow.Units.AoWE.AoWE.TranslateRStr(call__81);
                var call__83 = U.GetExperience();
                var call__84 = U.GetNextLevelExperience();
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.XP.Text = ((call__82 + ((AnsiString)" (")) + Aow.Rtl.Rtl.IntToStr(call__83)) + AnsiString.FromBytes(47) + Aow.Rtl.Rtl.IntToStr(call__84) + AnsiString.FromBytes(41);
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Cursor = 0;
                var call__85 = Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Items.Count;
                if ((call__85 <= Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Selected) || (Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Selected < 0))
                {
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Selected = 0;
                }

                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = true;
                bool bool__86 = (H != null);
                if (bool__86)
                {
                    bool bool__87 = ((Aow.Units.AoWE.AoWE.AoWHSMap.FCombat == null) && (H.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    if (!bool__87)
                    {
                        bool bool__88 = (Aow.Units.AoWE.AoWE.AoWHSMap.FCombat != null);
                        if (bool__88)
                        {
                            var call__89 = Aow.Units.AoWE.AoWE.AoWHSMap.FCombat.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FCombat.FCombatData.FCurrentPlayer));
                            bool__88 = (call__89.FPlayer == H.FPlayer);
                        }

                        bool__87 = bool__88;
                    }

                    bool__86 = bool__87;
                }

                if (bool__86)
                {
                    var call__90 = H.FInventory.GetItem(0);
                    if (call__90 != null)
                    {
                        var call__91 = H.FInventory.GetItem(0);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP1.Tag = call__91.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP1.Tag = -1;
                    }

                    var call__92 = H.FInventory.GetItem(1);
                    if (call__92 != null)
                    {
                        var call__93 = H.FInventory.GetItem(1);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP2.Tag = call__93.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP2.Tag = -1;
                    }

                    var call__94 = H.FInventory.GetItem(2);
                    if (call__94 != null)
                    {
                        var call__95 = H.FInventory.GetItem(2);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP3.Tag = call__95.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP3.Tag = -1;
                    }

                    var call__96 = H.FInventory.GetItem(3);
                    if (call__96 != null)
                    {
                        var call__97 = H.FInventory.GetItem(3);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP4.Tag = call__97.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP4.Tag = -1;
                    }

                    var call__98 = H.FInventory.GetItem(4);
                    if (call__98 != null)
                    {
                        var call__99 = H.FInventory.GetItem(4);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP5.Tag = call__99.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP5.Tag = -1;
                    }

                    var call__100 = H.FInventory.GetItem(5);
                    if (call__100 != null)
                    {
                        var call__101 = H.FInventory.GetItem(5);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP6.Tag = call__101.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP6.Tag = -1;
                    }

                    var call__102 = H.FInventory.GetItem(6);
                    if (call__102 != null)
                    {
                        var call__103 = H.FInventory.GetItem(6);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP7.Tag = call__103.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP7.Tag = -1;
                    }

                    var call__104 = H.FInventory.GetItem(7);
                    if (call__104 != null)
                    {
                        var call__105 = H.FInventory.GetItem(7);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP8.Tag = call__105.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP8.Tag = -1;
                    }

                    var call__106 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipAttackSlot);
                    if (call__106 != null)
                    {
                        var call__107 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipAttackSlot);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Attack.Tag = call__107.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Attack.Tag = -1;
                    }

                    var call__108 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipDefenseSlot);
                    if (call__108 != null)
                    {
                        var call__109 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipDefenseSlot);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Defend.Tag = call__109.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Defend.Tag = -1;
                    }

                    var call__110 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipHeadSlot);
                    if (call__110 != null)
                    {
                        var call__111 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipHeadSlot);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Head.Tag = call__111.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Head.Tag = -1;
                    }

                    var call__112 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipTorsoSlot);
                    if (call__112 != null)
                    {
                        var call__113 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipTorsoSlot);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Torso.Tag = call__113.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Torso.Tag = -1;
                    }

                    var call__114 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipLeftRingSlot);
                    if (call__114 != null)
                    {
                        var call__115 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipLeftRingSlot);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RingL.Tag = call__115.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RingL.Tag = -1;
                    }

                    var call__116 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipRightRingSlot);
                    if (call__116 != null)
                    {
                        var call__117 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipRightRingSlot);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RingR.Tag = call__117.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RingR.Tag = -1;
                    }

                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UpdateGround(0);
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem = null;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP1.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP2.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP3.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP4.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP5.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP6.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP7.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP8.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Attack.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Defend.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Head.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Torso.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RingL.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RingR.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ItemUseBtn.Visible = false;
                }

                if (U != null)
                {
                    var call__118 = U.GetHitPoints();
                    var call__119 = U.GetHits();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UHits.Text = (Aow.Rtl.Rtl.IntToStr(call__118) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__119);
                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null)
                    {
                        var call__120 = U.GetMovePoints();
                        var call__121 = U.GetMoves();
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UMoves.Text = (Aow.Rtl.Rtl.IntToStr(call__120) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__121);
                    }
                    else
                    {
                        if (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit != null)
                        {
                            var call__122 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.GetMoves();
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UMoves.Text = (Aow.Rtl.Rtl.IntToStr(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FMovePoints) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__122);
                        }
                    }

                    var call__123 = U.GetAttack();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UAttack.Text = Aow.Rtl.Rtl.IntToStr(call__123);
                    var call__124 = U.GetDamage();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UDamage.Text = Aow.Rtl.Rtl.IntToStr(call__124);
                    var call__125 = U.GetDefense();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UDefense.Text = Aow.Rtl.Rtl.IntToStr(call__125);
                    var call__126 = U.GetResistance();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UResistance.Text = Aow.Rtl.Rtl.IntToStr(call__126);
                    var call__127 = U.GetMoveTypes();
                    var call__128 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__127);
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UMovesIcon.CurrentImage = call__128;
                    if (U.GetManaUpkeep() > 0)
                    {
                        var call__129 = U.GetManaUpkeep();
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UpKeep.Text = Aow.Rtl.Rtl.IntToStr(call__129);
                        UpkeepIcon.CurrentImage = 52;
                    }
                    else
                    {
                        var call__130 = U.GetUpkeep();
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UpKeep.Text = Aow.Rtl.Rtl.IntToStr(call__130);
                        UpkeepIcon.CurrentImage = 51;
                    }

                    var call__131 = U.GetAlignment();
                    AAlignment.Visible = call__131 != Aow.Units.AoWE.TAlignment.alNone;
                    switch (U.GetAlignment())
                    {
                        case Aow.Units.AoWE.TAlignment.alPureGood:
                            AAlignment.CurrentImage = 31;
                            break;
                        case Aow.Units.AoWE.TAlignment.alGood:
                            AAlignment.CurrentImage = 32;
                            break;
                        case Aow.Units.AoWE.TAlignment.alPureNeutral:
                        case Aow.Units.AoWE.TAlignment.alNeutral:
                            AAlignment.CurrentImage = 33;
                            break;
                        case Aow.Units.AoWE.TAlignment.alEvil:
                            AAlignment.CurrentImage = 34;
                            break;
                        case Aow.Units.AoWE.TAlignment.alPureEvil:
                            AAlignment.CurrentImage = 35;
                            break;
                    }

                    var call__132 = U.GetAlignment();
                    var call__133 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TAlignmentRStr[(int)call__132]);
                    AAlignmentLbl.Text = call__133;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T1AbilityLB.Items.Clear();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T1TypeLB.Items.Clear();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T1SrcLB.Items.Clear();
                    var call__134 = U.GetMorale();
                    var call__135 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TMoraleRStr[(int)call__134]);
                    var call__136 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MoraleXRStr);
                    var call__137 = Aow.Units.AoWE.AoWE.Format(call__136, [call__135]);
                    MoraleLbl.Text = call__137;
                    var call__138 = U.GetUnitType();
                    MoraleLbl.Visible = call__138 != Aow.Units.AoWE.TUnitType.utMachine;
                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null)
                    {
                        U.ListAbilityInfo(T1AbilityLB.Items, T1TypeLB.Items, T1SrcLB.Items);
                        Aow.Units.Engine.Engine.SortStringsEx(T1AbilityLB.Items, Aow.Units.AoWUnitWindow.AoWUnitWindow.CompareUnitAbilities, [T1TypeLB.Items, T1SrcLB.Items]);
                    }
                    else
                    {
                        if (((Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit != null) && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit != null)) && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit != null))
                        {
                            Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit.ListAbilityInfo(T1AbilityLB.Items, T1TypeLB.Items, T1SrcLB.Items);
                            Aow.Units.Engine.Engine.SortStringsEx(T1AbilityLB.Items, Aow.Units.AoWUnitWindow.AoWUnitWindow.CompareUnitAbilities, [T1TypeLB.Items, T1SrcLB.Items]);
                        }
                    }

                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T1AbilityLB.Selected = -1;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T1AbilityLBChange(T1AbilityLB);
                    UnitWin.FDrawn = false;
                }
            }
        }
    }
}
