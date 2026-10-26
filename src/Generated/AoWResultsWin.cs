// Generated from AoWResultsWin.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.E1Resources;
using static Aow.Units.E1Resources.E1Resources;
using Aow.Units.E1Globals;
using static Aow.Units.E1Globals.E1Globals;
using Aow.Units.E1GameFlow;
using static Aow.Units.E1GameFlow.E1GameFlow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AOWPyreScrn;
using static Aow.Units.AOWPyreScrn.AOWPyreScrn;
using Aow.Units.aowIntGfxMod;
using static Aow.Units.aowIntGfxMod.aowIntGfxMod;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;

namespace Aow.Units.AoWResultsWin
{
    // { TResultsWin (size $108, VMT 004279A8): $100 player indices, $104 checkbox controls. }
    public partial class TResultsWin : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWCheckBox.TAOWCheckBox P1CB;
        // { +0044 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P2CB;
        // { +0048 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P3CB;
        // { +004C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P4CB;
        // { +0050 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P5CB;
        // { +0054 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P6CB;
        // { +0058 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P7CB;
        // { +005C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P8CB;
        // { +0060 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P9CB;
        // { +0064 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P10CB;
        // { +0068 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P11CB;
        // { +006C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P12CB;
        // { +0070 }
        public Aow.Units.AoWLabel.TAOWLabel StrLbl;
        // { +0074 }
        public Aow.Units.AoWLabel.TAOWLabel TurnLbl;
        // { +0078 }
        public Aow.Units.AoWMemo.TAOWMemo ResNames;
        // { +007C }
        public Aow.Units.AoWMemo.TAOWMemo ResValues;
        // { +0080 }
        public Aow.Units.AoWLabel.TAOWLabel SideLbl;
        // { +0084 }
        public Aow.Units.AoWImage.TAOWImage Axis;
        // { +0088 }
        public Aow.Units.AoWButton.TAOWButton ExitBtn;
        // { +008C }
        public Aow.Units.AoWImage.TAOWImage FaceGrid;
        // { +0090 }
        public Aow.Units.AoWButton.TAOWButton LoadBtn;
        // { +0094 }
        public Aow.Units.AoWButton.TAOWButton ContinueBtn;
        // { +0098 }
        public Aow.Units.AoWPanel.TAOWPanel ResultsPnl;
        // { +009C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox StrengthCB;
        // { +00A0 }
        public Aow.Units.AoWPanel.TAOWPanel AgendaPnl;
        // { +00A4 }
        public Aow.Units.AoWLabel.TAOWLabel StrengthLbl;
        // { +00A8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox GoldCB;
        // { +00AC }
        public Aow.Units.AoWLabel.TAOWLabel GoldLbl;
        // { +00B0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox ArmiesCB;
        // { +00B4 }
        public Aow.Units.AoWLabel.TAOWLabel ArmiesLbl;
        // { +00B8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox HeroesCB;
        // { +00BC }
        public Aow.Units.AoWLabel.TAOWLabel HeroesLbl;
        // { +00C0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox StructuresCB;
        // { +00C4 }
        public Aow.Units.AoWLabel.TAOWLabel StructuresLbl;
        // { +00C8 }
        public Aow.Units.AoWLabel.TAOWLabel KeyMax;
        // { +00CC }
        public Aow.Units.AoWLabel.TAOWLabel KeyMin;
        // { +00D0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox ManaCB;
        // { +00D4 }
        public Aow.Units.AoWLabel.TAOWLabel ManaLbl;
        // { +00D8 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +00DC }
        public int FMaxTurns;
        public int FMaxGold;
        public int FMaxMana;
        public int FMaxArmies;
        public int FReservedF0;
        public int FMaxHeroes;
        public int FMaxStructures;
        public int FMaxStrength;
        // { $E0..$FC }
        public Aow.Units.Engine.TByteList FPlayers;
        // { $100 }
        public Aow.Rtl.TList FControls;
        // AoWResultsWin.pas:93
        // { $104 }
        public void UpdateSelection()
        {
            int I = default;
            int J = default;
            Aow.Units.AoWE.TPlayerStatistics Stats = default;
            FMaxTurns = 1;
            FMaxGold = 1;
            FMaxMana = 1;
            FMaxArmies = 1;
            FReservedF0 = 1;
            FMaxHeroes = 1;
            FMaxStructures = 1;
            FMaxStrength = 1;
            {
                I = 0;
                int end__1 = FPlayers.Count - 1;
                for (; I <= end__1; I += 1)
                {
                    var call__2 = FPlayers[I];
                    var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__2);
                    Stats = call__3.FStatistics;
                    var call__4 = Stats.FTurnInfo.GetCount();
                    if (FMaxTurns < call__4)
                    {
                        var call__5 = Stats.FTurnInfo.GetCount();
                        FMaxTurns = call__5;
                    }

                    {
                        J = 0;
                        int end__6 = Stats.FTurnInfo.GetCount() - 1;
                        for (; J <= end__6; J += 1)
                        {
                            var call__7 = Stats.FTurnInfo.GetTurnInfo(J);
                            if (FMaxArmies < call__7.FArmyStrength)
                            {
                                var call__8 = Stats.FTurnInfo.GetTurnInfo(J);
                                FMaxArmies = call__8.FArmyStrength;
                            }

                            var call__9 = Stats.FTurnInfo.GetTurnInfo(J);
                            if (FMaxGold < call__9.FGold)
                            {
                                var call__10 = Stats.FTurnInfo.GetTurnInfo(J);
                                FMaxGold = call__10.FGold;
                            }

                            var call__11 = Stats.FTurnInfo.GetTurnInfo(J);
                            if (FMaxMana < call__11.FMana)
                            {
                                var call__12 = Stats.FTurnInfo.GetTurnInfo(J);
                                FMaxMana = call__12.FMana;
                            }

                            var call__13 = Stats.FTurnInfo.GetTurnInfo(J);
                            if (FMaxHeroes < call__13.FHeroes)
                            {
                                var call__14 = Stats.FTurnInfo.GetTurnInfo(J);
                                FMaxHeroes = call__14.FHeroes;
                            }

                            var call__15 = Stats.FTurnInfo.GetTurnInfo(J);
                            if (FMaxStructures < call__15.FStructures)
                            {
                                var call__16 = Stats.FTurnInfo.GetTurnInfo(J);
                                FMaxStructures = call__16.FStructures;
                            }

                            var call__17 = Stats.FTurnInfo.GetTurnInfo(J);
                            var call__18 = call__17.GetSuperiority();
                            if (FMaxStrength < call__18)
                            {
                                var call__19 = Stats.FTurnInfo.GetTurnInfo(J);
                                var call__20 = call__19.GetSuperiority();
                                FMaxStrength = call__20;
                            }
                        }
                    }
                }
            }

            var call__21 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TurnRStr);
            TurnLbl.Text = (call__21 + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(FMaxTurns);
        }

        // AoWResultsWin.pas:250
        public void DrawGraphs(in Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Rtl.TRect Bounds = default;
            int I = default;
            Bounds.Left = (ResultsPnl.WinLeft + Axis.WinLeft) + 5;
            Bounds.Top = (ResultsPnl.WinTop + Axis.WinTop) + 4;
            Bounds.Right = (ResultsPnl.WinLeft + Axis.WinLeft) + 338;
            Bounds.Bottom = (ResultsPnl.WinTop + Axis.WinTop) + 117;
            {
                I = 0;
                int end__22 = FPlayers.Count - 1;
                for (; I <= end__22; I += 1)
                {
                    if (GoldCB.Checked)
                    {
                        var call__23 = FPlayers[I];
                        DrawGold(unchecked((sbyte)call__23), Bounds, Surface);
                    }

                    if (ManaCB.Checked)
                    {
                        var call__24 = FPlayers[I];
                        DrawMana(unchecked((sbyte)call__24), Bounds, Surface);
                    }

                    if (HeroesCB.Checked)
                    {
                        var call__25 = FPlayers[I];
                        DrawHeroes(unchecked((sbyte)call__25), Bounds, Surface);
                    }

                    if (StructuresCB.Checked)
                    {
                        var call__26 = FPlayers[I];
                        DrawStructures(unchecked((sbyte)call__26), Bounds, Surface);
                    }

                    if (ArmiesCB.Checked)
                    {
                        var call__27 = FPlayers[I];
                        DrawArmies(unchecked((sbyte)call__27), Bounds, Surface);
                    }

                    if (StrengthCB.Checked)
                    {
                        var call__28 = FPlayers[I];
                        DrawStrength(unchecked((sbyte)call__28), Bounds, Surface);
                    }
                }
            }
        }

        // AoWResultsWin.pas:123
        public void DrawGraph(Aow.Units.ImageLib.TLibraryImage Image, Aow.Units.Engine.TIntegerList Values, int Turns, int Maximum, in Aow.Rtl.TRect R, in Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            int Width = default;
            int LastY = default;
            int Y1 = default;
            int X2 = default;
            int I = default;
            float XStep = default;
            float YStep = default;
            int NextY = default;
            int X1 = default;
            int Y2 = default;
            int Height = default;
            if (Values.Count != 0)
            {
                Width = R.Right - R.Left;
                Height = R.Bottom - R.Top;
                if (Turns >= 2)
                {
                    XStep = (float)(((double)Width) / (Turns - 1));
                }
                else
                {
                    XStep = Width;
                }

                YStep = (float)(((double)Height) / Maximum);
                var call__29 = Values[0];
                LastY = call__29;
                {
                    I = 1;
                    int end__30 = Values.Count - 1;
                    for (; I <= end__30; I += 1)
                    {
                        var call__31 = Values[I];
                        NextY = call__31;
                        X1 = Aow.Rtl.Rtl.Round((I - 1) * XStep);
                        ;
                        Y1 = Aow.Rtl.Rtl.Round(LastY * YStep);
                        // { Original advances from the rounded previous x coordinate. }
                        X2 = Aow.Rtl.Rtl.Round(X1 + XStep);
                        Y2 = Aow.Rtl.Rtl.Round(NextY * YStep);
                        Aow.Units.ImageLib.ImageLib.DrawImageLine(Image, X1 + R.Left, R.Bottom - Y1, R.Left + X2, R.Bottom - Y2, 1, Surface);
                        LastY = NextY;
                    }
                }

                if (Values.Count == 1)
                {
                    X1 = 0;
                    NextY = Aow.Rtl.Rtl.Round(LastY * YStep);
                    Y1 = NextY;
                    X2 = Width - 1;
                    // { Original repeats the rounding and discards its result. }
                    Y2 = Aow.Rtl.Rtl.Round(LastY * YStep);
                    Y2 = NextY;
                    Aow.Units.ImageLib.ImageLib.DrawImageLine(Image, X1 + R.Left, R.Bottom - Y1, R.Left + X2, R.Bottom - Y2, 1, Surface);
                }
            }
        }

        // AoWResultsWin.pas:160
        public void DrawGold(sbyte Player, in Aow.Rtl.TRect R, in Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Units.Engine.TIntegerList Values = default;
            Aow.Units.AoWE.TTurnInfo Info = default;
            int I = default;
            Values = Aow.Units.Engine.TIntegerList.Create();
            var call__32 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            Info = call__32.FStatistics.FTurnInfo;
            {
                I = 0;
                int end__33 = Info.GetCount() - 1;
                for (; I <= end__33; I += 1)
                {
                    var call__34 = Info.GetTurnInfo(I);
                    Values.Add(call__34.FGold);
                }
            }

            var call__35 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            var call__36 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.EndScene[call__35.FPlayerRace + 13];
            DrawGraph(call__36, Values, FMaxTurns, FMaxGold, R, Surface);
            Values.Free();
            KeyMax.Text = Aow.Rtl.Rtl.IntToStr(FMaxGold);
        }

        // AoWResultsWin.pas:175
        public void DrawMana(sbyte Player, in Aow.Rtl.TRect R, in Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Units.Engine.TIntegerList Values = default;
            Aow.Units.AoWE.TTurnInfo Info = default;
            int I = default;
            Values = Aow.Units.Engine.TIntegerList.Create();
            var call__37 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            Info = call__37.FStatistics.FTurnInfo;
            {
                I = 0;
                int end__38 = Info.GetCount() - 1;
                for (; I <= end__38; I += 1)
                {
                    var call__39 = Info.GetTurnInfo(I);
                    Values.Add(call__39.FMana);
                }
            }

            var call__40 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            var call__41 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.EndScene[call__40.FPlayerRace + 13];
            DrawGraph(call__41, Values, FMaxTurns, FMaxMana, R, Surface);
            Values.Free();
            KeyMax.Text = Aow.Rtl.Rtl.IntToStr(FMaxMana);
        }

        // AoWResultsWin.pas:190
        public void DrawHeroes(sbyte Player, in Aow.Rtl.TRect R, in Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Units.Engine.TIntegerList Values = default;
            Aow.Units.AoWE.TTurnInfo Info = default;
            int I = default;
            Values = Aow.Units.Engine.TIntegerList.Create();
            var call__42 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            Info = call__42.FStatistics.FTurnInfo;
            {
                I = 0;
                int end__43 = Info.GetCount() - 1;
                for (; I <= end__43; I += 1)
                {
                    var call__44 = Info.GetTurnInfo(I);
                    Values.Add(call__44.FHeroes);
                }
            }

            var call__45 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            var call__46 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.EndScene[call__45.FPlayerRace + 13];
            DrawGraph(call__46, Values, FMaxTurns, FMaxHeroes, R, Surface);
            Values.Free();
            KeyMax.Text = Aow.Rtl.Rtl.IntToStr(FMaxHeroes);
        }

        // AoWResultsWin.pas:205
        public void DrawArmies(sbyte Player, in Aow.Rtl.TRect R, in Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Units.Engine.TIntegerList Values = default;
            Aow.Units.AoWE.TTurnInfo Info = default;
            int I = default;
            Values = Aow.Units.Engine.TIntegerList.Create();
            var call__47 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            Info = call__47.FStatistics.FTurnInfo;
            {
                I = 0;
                int end__48 = Info.GetCount() - 1;
                for (; I <= end__48; I += 1)
                {
                    var call__49 = Info.GetTurnInfo(I);
                    Values.Add(call__49.FArmyStrength);
                }
            }

            var call__50 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            var call__51 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.EndScene[call__50.FPlayerRace + 13];
            DrawGraph(call__51, Values, FMaxTurns, FMaxArmies, R, Surface);
            Values.Free();
            KeyMax.Text = Aow.Rtl.Rtl.IntToStr(FMaxArmies);
        }

        // AoWResultsWin.pas:220
        public void DrawStructures(sbyte Player, in Aow.Rtl.TRect R, in Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Units.Engine.TIntegerList Values = default;
            Aow.Units.AoWE.TTurnInfo Info = default;
            int I = default;
            Values = Aow.Units.Engine.TIntegerList.Create();
            var call__52 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            Info = call__52.FStatistics.FTurnInfo;
            {
                I = 0;
                int end__53 = Info.GetCount() - 1;
                for (; I <= end__53; I += 1)
                {
                    var call__54 = Info.GetTurnInfo(I);
                    Values.Add(call__54.FStructures);
                }
            }

            var call__55 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            var call__56 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.EndScene[call__55.FPlayerRace + 13];
            DrawGraph(call__56, Values, FMaxTurns, FMaxStructures, R, Surface);
            Values.Free();
            KeyMax.Text = Aow.Rtl.Rtl.IntToStr(FMaxStructures);
        }

        // AoWResultsWin.pas:235
        public void DrawStrength(sbyte Player, in Aow.Rtl.TRect R, in Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Units.Engine.TIntegerList Values = default;
            Aow.Units.AoWE.TTurnInfo Info = default;
            int I = default;
            Values = Aow.Units.Engine.TIntegerList.Create();
            var call__57 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            Info = call__57.FStatistics.FTurnInfo;
            {
                I = 0;
                int end__58 = Info.GetCount() - 1;
                for (; I <= end__58; I += 1)
                {
                    var call__59 = Info.GetTurnInfo(I);
                    var call__60 = call__59.GetSuperiority();
                    Values.Add(call__60);
                }
            }

            var call__61 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            var call__62 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.EndScene[call__61.FPlayerRace + 13];
            DrawGraph(call__62, Values, FMaxTurns, FMaxStrength, R, Surface);
            Values.Free();
            // { Original strength graph labels the structures maximum. }
            KeyMax.Text = Aow.Rtl.Rtl.IntToStr(FMaxStructures);
        }

        // AoWResultsWin.pas:270
        public void ResultsWinCreate()
        {
            var call__63 = Aow.Units.Engine.TByteList.Create();
            FPlayers = call__63;
            var call__64 = new Aow.Rtl.TList();
            FControls = call__64;
            FControls.Add(P1CB);
            FControls.Add(P2CB);
            FControls.Add(P3CB);
            FControls.Add(P4CB);
            FControls.Add(P5CB);
            FControls.Add(P6CB);
            FControls.Add(P7CB);
            FControls.Add(P8CB);
            FControls.Add(P9CB);
            FControls.Add(P10CB);
            FControls.Add(P11CB);
            FControls.Add(P12CB);
        }

        // AoWResultsWin.pas:288
        // { draft }
        public void ResultsWinDestroy()
        {
            FPlayers.Free();
            FControls.Free();
        }

        // AoWResultsWin.pas:294
        // { draft }
        public void ContinueBtnClick()
        {
            Aow.Units.E1GameFlow.E1GameFlow.FinishPyre(Aow.Units.AOWPyreScrn.AOWPyreScrn.PyreScreen);
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Aow.Units.AoWResultsWin.AoWResultsWin.ResultsWin.ResultsPnl.Visible = false;
            Aow.Units.Sound.Sound.FadeMusicVolume(Aow.Units.AoWExeForms.AoWExeForms.SoundDlg.MusicVolume.Position, 2);
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FGameStatus == Aow.Units.AoWE.TGameStatus.gsTerminated)
            {
                if (Aow.Units.AoWE.AoWE.AoWEngine.FCampaign == null)
                {
                    Aow.Units.AoWWinMan.AoWWinMan.General.ReturnToTitle();
                }
                else
                {
                    Aow.Units.E1GameFlow.E1GameFlow.ShowCampaignMap(Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap);
                }
            }
            else
            {
                Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.Visible = false;
                Aow.Units.AoWExeForms.AoWExeForms.SetVideoMode(Aow.Units.AoWVideoDlg.AoWVideoDlg.VideoDialog, Aow.Units.AoWVideoDlg.TScreenMode.scmGame);
                Aow.Units.AoWVideoDlg.AoWVideoDlg.VideoDialog.ApplyMode();
            }
        }

        // AoWResultsWin.pas:313
        // { draft }
        public void ExitBtnClick()
        {
            ResultsPnl.Visible = false;
            Aow.Units.AoWWinMan.AoWWinMan.General.ReturnToTitle();
        }

        // AoWResultsWin.pas:319
        // { draft }
        public void ResultsPnlUpdate()
        {
            Aow.Units.AOWPyreScrn.AOWPyreScrn.PyreScreen.DeadPnlUpdate();
        }

        // AoWResultsWin.pas:324
        // { draft }
        public void AxisDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            DrawGraphs(Surface);
        }

        // AoWResultsWin.pas:329
        public void AgendaChange()
        {
            ResultsPnl.FDrawn = false;
        }

        // AoWResultsWin.pas:334
        // { draft }
        public void ResultsPnlShow()
        {
            UpdateSelection();
        }

        // AoWResultsWin.pas:339
        // { draft }
        public void P1CBPreDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Rtl.TRect R = default;
            {
                var w__65 = (Aow.Units.AoWComp.TAoWComponent)Sender;
                if (w__65.Tag < FPlayers.Count)
                {
                    var call__66 = Aow.Rtl.Rtl.Rect((w__65.WinLeft + ResultsPnl.WinLeft) + 1, (w__65.WinTop + ResultsPnl.WinTop) + 1, ((w__65.WinLeft + w__65.WinWidth) + ResultsPnl.WinLeft) - 1, ((w__65.WinTop + w__65.WinHeight) + ResultsPnl.WinTop) - 1);
                    R = call__66;
                    var call__67 = FPlayers[w__65.Tag];
                    var call__68 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__67);
                    var call__69 = call__68.GetFace();
                    if (call__69 != null)
                    {
                        var call__70 = FPlayers[w__65.Tag];
                        var call__71 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__70);
                        call__71.FLeader.ShowFace(R, Surface);
                    }

                    var call__72 = FPlayers[w__65.Tag];
                    var call__73 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__72);
                    var call__74 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin[call__73.FPlayerRace + 16];
                    call__74.ShowClipped(w__65.WinLeft + ResultsPnl.WinLeft, w__65.WinTop + ResultsPnl.WinTop, Surface);
                }
            }
        }

        // AoWResultsWin.pas:355
        public void P1CBChange(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWCheckBox.TAOWCheckBox Control = default;
            Control = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
            if (Control.Checked)
            {
                if (FPlayers.Count > Control.Tag)
                {
                    var call__75 = FPlayers[Control.Tag];
                    var call__76 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__75);
                    var call__77 = call__76.GetNameEx();
                    SideLbl.Text = call__77;
                }

                ResValues.Items.Clear();
                ResValues.Items.Add((AnsiString)"0%");
                ResValues.Items.Add((AnsiString)"0%");
                ResValues.Items.Add((AnsiString)"0%");
                ResValues.Items.Add((AnsiString)"0%");
            }
        }
    }

    public static partial class AoWResultsWin
    {
        public static Aow.Units.AoWResultsWin.TResultsWin ResultsWin;
        // AoWResultsWin.pas:368
        public static void ShowResults(Aow.Units.AoWResultsWin.TResultsWin Window)
        {
            int I = default;
            if (Aow.Units.AoWE.AoWE.AoWEngine.FDemo)
            {
                Aow.Units.Sound.Sound.PlaySongEx((AnsiString)"Songs\\Special\\end2.s3m", 2, 2, true);
            }
            else
            {
                Aow.Units.Sound.Sound.PlaySongEx((AnsiString)"Songs\\Special\\Sunset.it", 2, 2, true);
            }

            Aow.Units.Sound.Sound.SetMusicVolume(0);
            Aow.Units.Sound.Sound.FadeMusicVolume(Aow.Units.AoWExeForms.AoWExeForms.SoundDlg.MusicVolume.Position, 1);
            Window.FPlayers.Clear();
            {
                I = 1;
                int end__78 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count - 1;
                for (; I <= end__78; I += 1)
                {
                    var call__79 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                    if (call__79.FPlayerType != Aow.Units.AoWE.TPlayerType.ptIndependent)
                    {
                        Window.FPlayers.Add(unchecked((byte)I));
                    }
                }
            }

            {
                I = 0;
                byte end__80 = 11;
                for (; I <= end__80; I += 1)
                {
                    var call__81 = Window.FControls[I];
                    ((Aow.Units.AoWCheckBox.TAOWCheckBox)call__81).Visible = I < Window.FPlayers.Count;
                    bool bool__82 = (I < Window.FPlayers.Count);
                    if (bool__82)
                    {
                        var call__83 = Window.FPlayers[I];
                        bool__82 = (call__83 == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    }

                    var call__84 = Window.FControls[I];
                    ((Aow.Units.AoWCheckBox.TAOWCheckBox)call__84).Checked = bool__82;
                }
            }

            bool bool__85 = (Aow.Units.AoWE.AoWE.AoWEngine.FCampaign != null);
            if (bool__85)
            {
                var call__86 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                bool__85 = (call__86.FGameOverStatus == Aow.Units.AoWE.TGameOverStatus.goLost);
            }

            if (bool__85)
            {
                var call__87 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ReplayRStr);
                Window.ContinueBtn.Text = call__87;
                Window.ExitBtn.Visible = true;
            }
            else
            {
                var call__88 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ContinueRStr);
                Window.ContinueBtn.Text = call__88;
                Window.ExitBtn.Visible = false;
            }

            Aow.Units.AoWResultsWin.AoWResultsWin.ResultsWin.ResultsPnl.Visible = true;
        }
    }
}
