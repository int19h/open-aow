// Generated from AOWControlWin.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AOWGauge;
using static Aow.Units.AOWGauge.AOWGauge;
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
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.aowIntGfxMod;
using static Aow.Units.aowIntGfxMod.aowIntGfxMod;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.aowHelpWin;
using static Aow.Units.aowHelpWin.aowHelpWin;

namespace Aow.Units.AOWControlWin
{
    // { TControlWin (instance size $17C, VMT 00404910).
//     $44..174 published controls; $178 inverted army selection mask. }
    public partial class TControlWin : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow ControlWindow;
        // { +0044 }
        public Aow.Units.AoWLabel.TAOWLabel CWTitle;
        // { +0048 }
        public Aow.Units.AoWButton.TAOWButton CWMinIcon;
        // { +004C }
        public Aow.Units.AoWFrame.TAoWFrame Unit1Frame;
        // { +0050 }
        public Aow.Units.AoWFrame.TAoWFrame Unit2Frame;
        // { +0054 }
        public Aow.Units.AoWFrame.TAoWFrame Unit3Frame;
        // { +0058 }
        public Aow.Units.AoWFrame.TAoWFrame Unit4Frame;
        // { +005C }
        public Aow.Units.AoWFrame.TAoWFrame Unit5Frame;
        // { +0060 }
        public Aow.Units.AoWFrame.TAoWFrame Unit6Frame;
        // { +0064 }
        public Aow.Units.AoWFrame.TAoWFrame Unit7Frame;
        // { +0068 }
        public Aow.Units.AoWFrame.TAoWFrame Unit8Frame;
        // { +006C }
        public Aow.Units.AoWImage.TAOWImage CTitleIcon;
        // { +0070 }
        public Aow.Units.AoWLabel.TAOWLabel MovesLbl;
        // { +0074 }
        public Aow.Units.AoWLabel.TAOWLabel PartyStatusLbl;
        // { +0078 }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +007C }
        public Aow.Units.AoWImage.TAOWImage U1MIcon;
        // { +0080 }
        public Aow.Units.AoWImage.TAOWImage U2MIcon;
        // { +0084 }
        public Aow.Units.AoWImage.TAOWImage U3MIcon;
        // { +0088 }
        public Aow.Units.AoWImage.TAOWImage U4MIcon;
        // { +008C }
        public Aow.Units.AoWImage.TAOWImage U5MIcon;
        // { +0090 }
        public Aow.Units.AoWImage.TAOWImage U6MIcon;
        // { +0094 }
        public Aow.Units.AoWImage.TAOWImage U7MIcon;
        // { +0098 }
        public Aow.Units.AoWImage.TAOWImage U8MIcon;
        // { +009C }
        public Aow.Units.AoWImage.TAOWImage MoraleMask;
        // { +00A0 }
        public Aow.Units.AoWImage.TAOWImage MoveIcon;
        // { +00A4 }
        public Aow.Units.AoWPanel.TAOWPanel PartyT1Pnl;
        // { +00A8 }
        public Aow.Units.AoWPanel.TAOWPanel PartyBPnl;
        // { +00AC }
        public Aow.Units.AoWPanel.TAOWPanel PartyT2Pnl;
        // { +00B0 }
        public Aow.Units.AoWPanel.TAOWPanel PartyT3Pnl;
        // { +00B4 }
        public Aow.Units.AoWPanel.TAOWPanel PartyT4Pnl;
        // { +00B8 }
        public Aow.Units.AOWGauge.TAoWGauge Unit1Gge;
        // { +00BC }
        public Aow.Units.AOWGauge.TAoWGauge Unit2Gge;
        // { +00C0 }
        public Aow.Units.AOWGauge.TAoWGauge Unit3Gge;
        // { +00C4 }
        public Aow.Units.AOWGauge.TAoWGauge Unit4Gge;
        // { +00C8 }
        public Aow.Units.AOWGauge.TAoWGauge Unit5Gge;
        // { +00CC }
        public Aow.Units.AOWGauge.TAoWGauge Unit6Gge;
        // { +00D0 }
        public Aow.Units.AOWGauge.TAoWGauge Unit7Gge;
        // { +00D4 }
        public Aow.Units.AOWGauge.TAoWGauge Unit8Gge;
        // { +00D8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Unit1Chk;
        // { +00DC }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Unit2Chk;
        // { +00E0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Unit3Chk;
        // { +00E4 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Unit5Chk;
        // { +00E8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Unit6Chk;
        // { +00EC }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Unit7Chk;
        // { +00F0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Unit4Chk;
        // { +00F4 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Unit8Chk;
        // { +00F8 }
        public Aow.Units.AoWFrame.TAoWFrame Emp1Fr;
        // { +00FC }
        public Aow.Units.AoWFrame.TAoWFrame Emp2Fr;
        // { +0100 }
        public Aow.Units.AoWFrame.TAoWFrame Emp3Fr;
        // { +0104 }
        public Aow.Units.AoWFrame.TAoWFrame Emp4Fr;
        // { +0108 }
        public Aow.Units.AoWFrame.TAoWFrame Emp5Fr;
        // { +010C }
        public Aow.Units.AoWFrame.TAoWFrame Emp6Fr;
        // { +0110 }
        public Aow.Units.AoWFrame.TAoWFrame Emp7Fr;
        // { +0114 }
        public Aow.Units.AoWFrame.TAoWFrame Emp8Fr;
        // { +0118 }
        public Aow.Units.AoWFrame.TAoWFrame Emp1Chk;
        // { +011C }
        public Aow.Units.AoWFrame.TAoWFrame Emp2Chk;
        // { +0120 }
        public Aow.Units.AoWFrame.TAoWFrame Emp3Chk;
        // { +0124 }
        public Aow.Units.AoWFrame.TAoWFrame Emp4Chk;
        // { +0128 }
        public Aow.Units.AoWFrame.TAoWFrame Emp5Chk;
        // { +012C }
        public Aow.Units.AoWFrame.TAoWFrame Emp6Chk;
        // { +0130 }
        public Aow.Units.AoWFrame.TAoWFrame Emp7Chk;
        // { +0134 }
        public Aow.Units.AoWFrame.TAoWFrame Emp8Chk;
        // { +0138 }
        public Aow.Units.AoWPanel.TAOWPanel PartyIPnl;
        // { +013C }
        public Aow.Units.AoWLabel.TAOWLabel PartyILbl;
        // { +0140 }
        public Aow.Units.AoWButton.TAOWButton Center;
        // { +0144 }
        public Aow.Units.AoWButton.TAOWButton Help;
        // { +0148 }
        public Aow.Units.AoWButton.TAOWButton PPrev;
        // { +014C }
        public Aow.Units.AoWButton.TAOWButton PMove;
        // { +0150 }
        public Aow.Units.AoWButton.TAOWButton PNext;
        // { +0154 }
        public Aow.Units.AoWPanel.TAOWPanel PControlPnl;
        // { +0158 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox PGuard;
        // { +015C }
        public Aow.Units.AoWFrame.TAoWFrame AoWFrame2;
        // { +0160 }
        public Aow.Units.AoWPanel.TAOWPanel PartyB2Pbl;
        // { +0164 }
        public Aow.Units.AoWFrame.TAoWFrame AoWFrame1;
        // { +0168 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +016C }
        public Aow.Units.AoWLabel.TAOWLabel PartyActionLbl1;
        // { +0170 }
        public Aow.Units.AoWLabel.TAOWLabel PartyActionLbl2;
        // { +0174 }
        public byte FSelection;
        // { $178 }
        public Aow.Units.FixedArray_byte_3 F_ControlWin_179;
        // AOWControlWin.pas:129
        // { $45B054 }
        public void UpdateArmy()
        {
            bool ReadOnly = default;
            Unit1Chk.Checked = false;
            Unit2Chk.Checked = false;
            Unit3Chk.Checked = false;
            Unit4Chk.Checked = false;
            Unit5Chk.Checked = false;
            Unit6Chk.Checked = false;
            Unit7Chk.Checked = false;
            Unit8Chk.Checked = false;
            Emp1Fr.Visible = true;
            Emp1Chk.Visible = true;
            Emp2Fr.Visible = true;
            Emp2Chk.Visible = true;
            Emp3Fr.Visible = true;
            Emp3Chk.Visible = true;
            Emp4Fr.Visible = true;
            Emp4Chk.Visible = true;
            Emp5Fr.Visible = true;
            Emp5Chk.Visible = true;
            Emp6Fr.Visible = true;
            Emp6Chk.Visible = true;
            Emp7Fr.Visible = true;
            Emp7Chk.Visible = true;
            Emp8Fr.Visible = true;
            Emp8Chk.Visible = true;
            Unit1Frame.ILIndexFrame = 12;
            Unit2Frame.ILIndexFrame = 12;
            Unit3Frame.ILIndexFrame = 12;
            Unit4Frame.ILIndexFrame = 12;
            Unit5Frame.ILIndexFrame = 12;
            Unit6Frame.ILIndexFrame = 12;
            Unit7Frame.ILIndexFrame = 12;
            Unit8Frame.ILIndexFrame = 12;
            PartyActionLbl1.Visible = false;
            PartyActionLbl2.Visible = false;
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.FPlayer == 0)
            {
                PartyBPnl.Visible = false;
                PartyIPnl.Visible = true;
                PartyILbl.Text = (AnsiString)"";
                ReadOnly = true;
                PartyActionLbl1.Visible = true;
                PartyActionLbl2.Visible = true;
                var call__1 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.PartyActionRStr);
                PartyActionLbl1.Text = call__1;
                var call__2 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetBehavior(255);
                switch (unchecked((int)call__2) & 127)
                {
                    case 1:
                        var call__3 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.PatrollingRStr);
                        PartyActionLbl2.Text = call__3;
                        break;
                    case 2:
                    case 3:
                        var call__4 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.GuardingRStr);
                        PartyActionLbl2.Text = call__4;
                        break;
                    case 4:
                        var call__5 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.ScoutingRStr);
                        PartyActionLbl2.Text = call__5;
                        break;
                    case 5:
                        var call__6 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.SeekingRefugeRStr);
                        PartyActionLbl2.Text = call__6;
                        break;
                    case 6:
                        var call__7 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.RaidingRStr);
                        PartyActionLbl2.Text = call__7;
                        break;
                    case 7:
                        var call__8 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.HuntingRStr);
                        PartyActionLbl2.Text = call__8;
                        break;
                    case 8:
                        var call__9 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.ProtectingRStr);
                        PartyActionLbl2.Text = call__9;
                        break;
                    case 9:
                        var call__10 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.FortifiedRStr);
                        PartyActionLbl2.Text = call__10;
                        break;
                    case 10:
                        var call__11 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.BerserkRStr);
                        PartyActionLbl2.Text = call__11;
                        break;
                    default:
                        var call__12 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.WanderingRStr);
                        PartyActionLbl2.Text = call__12;
                        break;
                }
            }
            else
            {
                if (Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.FPlayer != Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
                {
                    PartyBPnl.Visible = false;
                    PartyIPnl.Visible = true;
                    ReadOnly = true;
                    var call__13 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.FPlayer);
                    var call__14 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[call__13.FPlayerRace]);
                    var call__15 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PartyOwnerXRStr);
                    var call__16 = Aow.Units.AoWE.AoWE.Format(call__15, [call__14]);
                    PartyILbl.Text = call__16;
                    PControlPnl.Visible = false;
                }
                else
                {
                    PartyBPnl.Visible = true;
                    PartyIPnl.Visible = false;
                    ReadOnly = false;
                    MoveIcon.Visible = true;
                    var call__17 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetPartyMoves(255);
                    var call__18 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__17);
                    MoveIcon.CurrentImage = call__18;
                    var call__19 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MovePoints(255);
                    MovesLbl.Text = Aow.Rtl.Rtl.IntToStr(call__19);
                    Aow.Units.AOWControlWin.AOWControlWin.ControlWin.CheckEnd();
                    PControlPnl.Visible = true;
                    PGuard.Checked = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.FUserMode.Contains(Aow.Units.AoWE.TArmyUserModeElement.aumGuard);
                }
            }

            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[0] != -1)
            {
                var call__20 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MoveTypes(unchecked((byte)(1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[0])));
                var call__21 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__20);
                U1MIcon.CurrentImage = call__21;
                var call__22 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[0]);
                var call__23 = call__22.GetHits();
                Unit1Gge.Max = call__23;
                var call__24 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[0]);
                var call__25 = call__24.GetHitPoints();
                Unit1Gge.Position = call__25;
                var call__26 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MovePoints(unchecked((byte)(1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[0])));
                Unit1Chk.Text = AnsiString.FromBytes(32) + Aow.Rtl.Rtl.IntToStr(call__26);
                Emp1Fr.Visible = false;
                Unit1Chk.Visible = !ReadOnly;
                Emp1Chk.Visible = ReadOnly;
            }

            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[1] != -1)
            {
                var call__27 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MoveTypes(unchecked((byte)(1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[1])));
                var call__28 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__27);
                U2MIcon.CurrentImage = call__28;
                var call__29 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[1]);
                var call__30 = call__29.GetHits();
                Unit2Gge.Max = call__30;
                var call__31 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[1]);
                var call__32 = call__31.GetHitPoints();
                Unit2Gge.Position = call__32;
                var call__33 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MovePoints(unchecked((byte)(1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[1])));
                Unit2Chk.Text = AnsiString.FromBytes(32) + Aow.Rtl.Rtl.IntToStr(call__33);
                Emp2Fr.Visible = false;
                Unit2Chk.Visible = !ReadOnly;
                Emp2Chk.Visible = ReadOnly;
            }

            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[2] != -1)
            {
                var call__34 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MoveTypes(unchecked((byte)(1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[2])));
                var call__35 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__34);
                U3MIcon.CurrentImage = call__35;
                var call__36 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[2]);
                var call__37 = call__36.GetHits();
                Unit3Gge.Max = call__37;
                var call__38 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[2]);
                var call__39 = call__38.GetHitPoints();
                Unit3Gge.Position = call__39;
                var call__40 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MovePoints(unchecked((byte)(1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[2])));
                Unit3Chk.Text = AnsiString.FromBytes(32) + Aow.Rtl.Rtl.IntToStr(call__40);
                Emp3Fr.Visible = false;
                Unit3Chk.Visible = !ReadOnly;
                Emp3Chk.Visible = ReadOnly;
            }

            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[3] != -1)
            {
                var call__41 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MoveTypes(unchecked((byte)(1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[3])));
                var call__42 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__41);
                U4MIcon.CurrentImage = call__42;
                var call__43 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[3]);
                var call__44 = call__43.GetHits();
                Unit4Gge.Max = call__44;
                var call__45 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[3]);
                var call__46 = call__45.GetHitPoints();
                Unit4Gge.Position = call__46;
                var call__47 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MovePoints(unchecked((byte)(1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[3])));
                Unit4Chk.Text = AnsiString.FromBytes(32) + Aow.Rtl.Rtl.IntToStr(call__47);
                Emp4Fr.Visible = false;
                Unit4Chk.Visible = !ReadOnly;
                Emp4Chk.Visible = ReadOnly;
            }

            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[4] != -1)
            {
                var call__48 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MoveTypes(unchecked((byte)(1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[4])));
                var call__49 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__48);
                U5MIcon.CurrentImage = call__49;
                var call__50 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[4]);
                var call__51 = call__50.GetHits();
                Unit5Gge.Max = call__51;
                var call__52 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[4]);
                var call__53 = call__52.GetHitPoints();
                Unit5Gge.Position = call__53;
                var call__54 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MovePoints(unchecked((byte)(1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[4])));
                Unit5Chk.Text = AnsiString.FromBytes(32) + Aow.Rtl.Rtl.IntToStr(call__54);
                Emp5Fr.Visible = false;
                Unit5Chk.Visible = !ReadOnly;
                Emp5Chk.Visible = ReadOnly;
            }

            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[5] != -1)
            {
                var call__55 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MoveTypes(unchecked((byte)(1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[5])));
                var call__56 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__55);
                U6MIcon.CurrentImage = call__56;
                var call__57 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[5]);
                var call__58 = call__57.GetHits();
                Unit6Gge.Max = call__58;
                var call__59 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[5]);
                var call__60 = call__59.GetHitPoints();
                Unit6Gge.Position = call__60;
                var call__61 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MovePoints(unchecked((byte)(1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[5])));
                Unit6Chk.Text = AnsiString.FromBytes(32) + Aow.Rtl.Rtl.IntToStr(call__61);
                Emp6Fr.Visible = false;
                Unit6Chk.Visible = !ReadOnly;
                Emp6Chk.Visible = ReadOnly;
            }

            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[6] != -1)
            {
                var call__62 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MoveTypes(unchecked((byte)(1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[6])));
                var call__63 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__62);
                U7MIcon.CurrentImage = call__63;
                var call__64 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[6]);
                var call__65 = call__64.GetHits();
                Unit7Gge.Max = call__65;
                var call__66 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[6]);
                var call__67 = call__66.GetHitPoints();
                Unit7Gge.Position = call__67;
                var call__68 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MovePoints(unchecked((byte)(1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[6])));
                Unit7Chk.Text = AnsiString.FromBytes(32) + Aow.Rtl.Rtl.IntToStr(call__68);
                Emp7Fr.Visible = false;
                Unit7Chk.Visible = !ReadOnly;
                Emp7Chk.Visible = ReadOnly;
            }

            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[7] != -1)
            {
                var call__69 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MoveTypes(unchecked((byte)(1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[7])));
                var call__70 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__69);
                U8MIcon.CurrentImage = call__70;
                var call__71 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[7]);
                var call__72 = call__71.GetHits();
                Unit8Gge.Max = call__72;
                var call__73 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[7]);
                var call__74 = call__73.GetHitPoints();
                Unit8Gge.Position = call__74;
                var call__75 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MovePoints(unchecked((byte)(1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[7])));
                Unit8Chk.Text = AnsiString.FromBytes(32) + Aow.Rtl.Rtl.IntToStr(call__75);
                Emp8Fr.Visible = false;
                Unit8Chk.Visible = !ReadOnly;
                Emp8Chk.Visible = ReadOnly;
            }

            if (!Aow.Units.AoWExeForms.AoWExeForms.TBWindow.ConButton.Visible)
            {
                ControlWindow.Visible = true;
            }

            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.ConButton.Visible = true;
        }

        // AOWControlWin.pas:294
        public void DrawUnit(int Index, in Aow.Rtl.TRect R, ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Index] != -1)
            {
                var call__76 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Index]);
                var call__77 = call__76.GetPreviewImage();
                if (call__77 != null)
                {
                    SD.ClipRect = R;
                    // { Original tests the unsorted index when checking the background library. }
                    var call__78 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Index);
                    var call__79 = call__78.GetAlignment();
                    if (((int)call__79) < Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs.Count)
                    {
                        var call__80 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Index]);
                        var call__81 = call__80.GetAlignment();
                        var call__82 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[(int)call__81];
                        var call__83 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Index]);
                        var call__84 = call__83.GetAlignment();
                        var call__85 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[(int)call__84];
                        var call__86 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Index]);
                        var call__87 = call__86.GetAlignment();
                        var call__88 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[(int)call__87];
                        call__88.ShowClipped(R.Left + (((R.Right - R.Left) - call__85.Width) / 2), R.Top + (((R.Bottom - R.Top) - call__82.Height) / 2), SD);
                    }

                    var call__89 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Index]);
                    var call__90 = call__89.GetPreviewImage();
                    var call__91 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Index]);
                    var call__92 = call__91.GetPreviewImage();
                    var call__93 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Index]);
                    call__93.ShowEx(R.Left + (((R.Right - R.Left) - call__92.Width) / 2), R.Top + (((R.Bottom - R.Top) - call__90.Height) / 2), SD, Aow.Units.AoWWinMan.AoWWinMan.General.FUnitShowInfo);
                }
            }
        }

        // AOWControlWin.pas:312
        // { 00405D98 }
        public void CWMinIconClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            ControlWindow.Visible = false;
        }

        // AOWControlWin.pas:318
        // { draft }
        public void CloseClick(Aow.Rtl.TObject Sender)
        {
            if (Sender != null)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            }

            ControlWindow.Visible = false;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.ConButton.Visible = false;
        }

        // AOWControlWin.pas:324
        public void ControlWindowResize()
        {
            {
                var w__94 = ControlWindow;
                w__94.Alignment.RightOffset = (w__94.WinManager.WinWidth - w__94.WinLeft) - w__94.WinWidth;
                w__94.Alignment.TopOffset = w__94.WinTop;
            }
        }

        // AOWControlWin.pas:333
        // { draft }
        public void Unit1FrameMouseDown(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.AoWE.TAbstractUnit U = default;
            bool bool__95 = (Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList != null) && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Sender.Tag] > -1);
            if (bool__95)
            {
                var call__96 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Sender.Tag]);
                bool__95 = (call__96 != null);
            }

            if (bool__95)
            {
                {
                    var call__97 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Sender.Tag]);
                    Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit = call__97;
                    if (Button == Aow.Rtl.TMouseButton.mbLeft)
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
                    }
                }
            }
        }

        // AOWControlWin.pas:348
        public void Unit1ChkChange(Aow.Rtl.TObject Sender)
        {
            CheckStart();
            Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.SetSelection(unchecked((byte)FSelection));
            CheckEnd();
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AOWControlWin.pas:354
        public void CheckStart()
        {
            FSelection = 0;
            if (!Unit1Chk.Checked && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[0] != -1))
            {
                FSelection = unchecked((byte)(FSelection | (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[0])));
            }

            if (!Unit2Chk.Checked && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[1] != -1))
            {
                FSelection = unchecked((byte)(FSelection | (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[1])));
            }

            if (!Unit3Chk.Checked && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[2] != -1))
            {
                FSelection = unchecked((byte)(FSelection | (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[2])));
            }

            if (!Unit4Chk.Checked && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[3] != -1))
            {
                FSelection = unchecked((byte)(FSelection | (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[3])));
            }

            if (!Unit5Chk.Checked && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[4] != -1))
            {
                FSelection = unchecked((byte)(FSelection | (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[4])));
            }

            if (!Unit6Chk.Checked && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[5] != -1))
            {
                FSelection = unchecked((byte)(FSelection | (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[5])));
            }

            if (!Unit7Chk.Checked && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[6] != -1))
            {
                FSelection = unchecked((byte)(FSelection | (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[6])));
            }

            if (!Unit8Chk.Checked && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[7] != -1))
            {
                FSelection = unchecked((byte)(FSelection | (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[7])));
            }
        }

        // AOWControlWin.pas:375
        // { draft }
        public void CheckEnd()
        {
            var call__98 = Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.GetSelection();
            FSelection = unchecked((byte)call__98);
            Unit1Chk.Checked = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[0] != -1) && ((FSelection & (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[0])) == 0);
            Unit2Chk.Checked = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[1] != -1) && ((FSelection & (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[1])) == 0);
            Unit3Chk.Checked = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[2] != -1) && ((FSelection & (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[2])) == 0);
            Unit4Chk.Checked = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[3] != -1) && ((FSelection & (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[3])) == 0);
            Unit5Chk.Checked = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[4] != -1) && ((FSelection & (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[4])) == 0);
            Unit6Chk.Checked = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[5] != -1) && ((FSelection & (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[5])) == 0);
            Unit7Chk.Checked = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[6] != -1) && ((FSelection & (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[6])) == 0);
            Unit8Chk.Checked = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[7] != -1) && ((FSelection & (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[7])) == 0);
            if (Unit1Chk.Checked)
            {
                Unit1Frame.ILIndexFrame = 26;
            }
            else
            {
                Unit1Frame.ILIndexFrame = 12;
            }

            if (Unit2Chk.Checked)
            {
                Unit2Frame.ILIndexFrame = 26;
            }
            else
            {
                Unit2Frame.ILIndexFrame = 12;
            }

            if (Unit3Chk.Checked)
            {
                Unit3Frame.ILIndexFrame = 26;
            }
            else
            {
                Unit3Frame.ILIndexFrame = 12;
            }

            if (Unit4Chk.Checked)
            {
                Unit4Frame.ILIndexFrame = 26;
            }
            else
            {
                Unit4Frame.ILIndexFrame = 12;
            }

            if (Unit5Chk.Checked)
            {
                Unit5Frame.ILIndexFrame = 26;
            }
            else
            {
                Unit5Frame.ILIndexFrame = 12;
            }

            if (Unit6Chk.Checked)
            {
                Unit6Frame.ILIndexFrame = 26;
            }
            else
            {
                Unit6Frame.ILIndexFrame = 12;
            }

            if (Unit7Chk.Checked)
            {
                Unit7Frame.ILIndexFrame = 26;
            }
            else
            {
                Unit7Frame.ILIndexFrame = 12;
            }

            if (Unit8Chk.Checked)
            {
                Unit8Frame.ILIndexFrame = 26;
            }
            else
            {
                Unit8Frame.ILIndexFrame = 12;
            }

            var call__99 = Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.GetSelection();
            var call__100 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetPartyMoves(unchecked((byte)call__99));
            var call__101 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__100);
            MoveIcon.CurrentImage = call__101;
            var call__102 = Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.GetSelection();
            var call__103 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MovePoints(unchecked((byte)call__102));
            MovesLbl.Text = Aow.Rtl.Rtl.IntToStr(call__103);
            var call__104 = Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.GetSelection();
            var call__105 = Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.GetArmyHS();
            var call__106 = call__105.FArmy.GetPartyStatus(unchecked((byte)call__104));
            var call__107 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPartyStatusRStr[(int)call__106]);
            PartyStatusLbl.Text = call__107;
        }

        // AOWControlWin.pas:407
        // { draft }
        public void Unit1ChkMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (Button == Aow.Rtl.TMouseButton.mbRight)
            {
                CheckStart();
                Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.SetSelection(unchecked((byte)~FSelection));
                CheckEnd();
            }
        }

        // AOWControlWin.pas:415
        public void CenterClick()
        {
            // { Tried direct coordinate paths; no exact match. }
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            {
                var w__108 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList;
                Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(w__108.FX, w__108.FY, w__108.FL);
            }

            {
                var w__109 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList;
                Aow.Units.AoWE.AoWE.AoWHSMap.PlayCenterAnimation(unchecked((sbyte)w__109.FX), unchecked((sbyte)w__109.FY), unchecked((sbyte)w__109.FL));
            }
        }

        // AOWControlWin.pas:422
        // { draft }
        public void Unit1FramePreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Aow.Rtl.TRect SaveRect = default;
            Aow.Units.AoWComp.TAoWComponent Component = default;
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList != null)
            {
                Component = Sender;
                SaveRect = SurfDesc.ClipRect;
                var call__110 = Aow.Rtl.Rtl.Rect((Component.AOWOwner.WinLeft + Component.WinLeft) + 4, (Component.AOWOwner.WinTop + Component.WinTop) + 3, ((Component.AOWOwner.WinLeft + Component.WinLeft) + Component.WinWidth) - 3, ((Component.AOWOwner.WinTop + Component.WinTop) + Component.WinHeight) - 7);
                DrawUnit(Component.Tag, call__110, ref SurfDesc);
                SurfDesc.ClipRect = SaveRect;
            }
        }

        // AOWControlWin.pas:437
        public void HelpClick()
        {
            Aow.Units.aowHelpWin.aowHelpWin.ShowHelp(503);
        }

        // AOWControlWin.pas:442
        // { draft }
        public void PPrevClick()
        {
            Aow.Units.AoWE.AoWE.AoWHSMap.SelectPrevArmy();
        }

        // AOWControlWin.pas:447
        // { draft }
        public void PNextClick()
        {
            Aow.Units.AoWE.AoWE.AoWHSMap.SelectNextArmy();
        }

        // AOWControlWin.pas:452
        // { draft }
        public void PMoveClick()
        {
            Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.ExecuteMove();
        }

        // AOWControlWin.pas:457
        // { draft }
        public void PGuardChange()
        {
            if (PGuard.Checked)
            {
                if (!Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.FUserMode.Contains(Aow.Units.AoWE.TArmyUserModeElement.aumGuard))
                {
                    Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.FUserMode = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.FUserMode + TSet<Aow.Units.AoWE.TArmyUserModeElement>.Of(Aow.Units.AoWE.TArmyUserModeElement.aumGuard);
                }
            }
            else
            {
                if (Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.FUserMode.Contains(Aow.Units.AoWE.TArmyUserModeElement.aumGuard))
                {
                    Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.FUserMode = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.FUserMode - TSet<Aow.Units.AoWE.TArmyUserModeElement>.Of(Aow.Units.AoWE.TArmyUserModeElement.aumGuard);
                }
            }
        }
    }

    public static partial class AOWControlWin
    {
        // { draft }
        public static Aow.Units.AOWControlWin.TControlWin ControlWin;
    }
}
