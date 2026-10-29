// Translated from FastCombatWin.pas.
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
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWHScroll;
using static Aow.Units.AoWHScroll.AoWHScroll;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.aowUserDrawBox;
using static Aow.Units.aowUserDrawBox.aowUserDrawBox;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.aowIntGfxMod;
using static Aow.Units.aowIntGfxMod.aowIntGfxMod;
using Aow.Units.E2Globals;
using static Aow.Units.E2Globals.E2Globals;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;

namespace Aow.Units.FastCombatWin
{
    // { TFastCombatWindow (size $154, VMT 00434CD4): $124 phase, $128 phase frame, $12C animation frame, $134/$138 and $13C/$140 shield indices/opacities, $144 combat viewer, $148 completion event, $150 retained event. }
    public partial class TFastCombatWindow : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow FCWin;
        // { +0044 }
        public Aow.Units.AoWPanel.TAOWPanel LNamePnl;
        // { +0048 }
        public Aow.Units.AoWLabel.TAOWLabel LSideLbl;
        // { +004C }
        public Aow.Units.AoWPanel.TAOWPanel RNamePnl;
        // { +0050 }
        public Aow.Units.AoWLabel.TAOWLabel RSideLbl;
        // { +0054 }
        public Aow.Units.AoWPanel.TAOWPanel LCtrlPnl;
        // { +0058 }
        public Aow.Units.AoWPanel.TAOWPanel RCtrlPnl;
        // { +005C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox PlayBtn2;
        // { +0060 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox PauseBtn2;
        // { +0064 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox StartBtn;
        // { +0068 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox EndBtn;
        // { +006C }
        public Aow.Units.AoWButton.TAOWButton CloseBut;
        // { +0070 }
        public Aow.Units.AoWPanel.TAOWPanel Party1Pnl;
        // { +0074 }
        public Aow.Units.AoWImage.TAOWImage Party1Bg;
        // { +0078 }
        public Aow.Units.AoWImage.TAOWImage Part1Dead;
        // { +007C }
        public Aow.Units.AoWPanel.TAOWPanel Party2Pnl;
        // { +0080 }
        public Aow.Units.AoWImage.TAOWImage Party2Bg;
        // { +0084 }
        public Aow.Units.AoWImage.TAOWImage Party2Dead;
        // { +0088 }
        public Aow.Units.AoWPanel.TAOWPanel Party3Pnl;
        // { +008C }
        public Aow.Units.AoWImage.TAOWImage Party3Bg;
        // { +0090 }
        public Aow.Units.AoWImage.TAOWImage Party3Dead;
        // { +0094 }
        public Aow.Units.AoWPanel.TAOWPanel Party4Pnl;
        // { +0098 }
        public Aow.Units.AoWImage.TAOWImage Party4Bg;
        // { +009C }
        public Aow.Units.AoWImage.TAOWImage Party4Dead;
        // { +00A0 }
        public Aow.Units.AoWPanel.TAOWPanel Party5Pnl;
        // { +00A4 }
        public Aow.Units.AoWImage.TAOWImage Party5Bg;
        // { +00A8 }
        public Aow.Units.AoWImage.TAOWImage Party5Dead;
        // { +00AC }
        public Aow.Units.AoWPanel.TAOWPanel Party6Pnl;
        // { +00B0 }
        public Aow.Units.AoWImage.TAOWImage Party6Bg;
        // { +00B4 }
        public Aow.Units.AoWImage.TAOWImage Party6Dead;
        // { +00B8 }
        public Aow.Units.AoWPanel.TAOWPanel Party7Pnl;
        // { +00BC }
        public Aow.Units.AoWImage.TAOWImage Party7Bg;
        // { +00C0 }
        public Aow.Units.AoWImage.TAOWImage Party7Dead;
        // { +00C4 }
        public Aow.Units.AoWPanel.TAOWPanel LSidePnl;
        // { +00C8 }
        public Aow.Units.AoWPanel.TAOWPanel RSidePnl;
        // { +00CC }
        public Aow.Units.AoWPanel.TAOWPanel FightPnl;
        // { +00D0 }
        public Aow.Units.AoWImage.TAOWImage LShield;
        // { +00D4 }
        public Aow.Units.AoWImage.TAOWImage RShield;
        // { +00D8 }
        public Aow.Units.AoWButton.TAOWButton InitialView;
        // { +00DC }
        public Aow.Units.AoWButton.TAOWButton FinalView;
        // { +00E0 }
        public Aow.Units.AoWButton.TAOWButton PlayBtn;
        // { +00E4 }
        public Aow.Units.AoWButton.TAOWButton PauseBtn;
        // { +00E8 }
        public Aow.Units.AoWHScroll.TAOWHScrollBar SequencePositionSld;
        // { +00EC }
        public Aow.Units.AoWHScroll.TAOWHScrollBar SequenceSpeedSld;
        // { +00F0 }
        public Aow.Units.ImageLib.TImageLibrary GlowAnimation;
        // { +00F4 }
        public Aow.Units.AoWImage.TAOWImage GlowImage;
        // { +00F8 }
        public Aow.Units.AoWButton.TAOWButton PrevSeqBtn;
        // { +00FC }
        public Aow.Units.AoWButton.TAOWButton NextSeqBtn;
        // { +0100 }
        public Aow.Units.AoWLabel.TAOWLabel StartLBl;
        // { +0104 }
        public Aow.Units.AoWLabel.TAOWLabel FastLbl;
        // { +0108 }
        public Aow.Units.AoWLabel.TAOWLabel SlowLbl;
        // { +010C }
        public Aow.Units.AoWLabel.TAOWLabel EndLbl;
        // { +0110 }
        public Aow.Units.AoWLabel.TAOWLabel ModeLbl;
        // { +0114 }
        public Aow.Units.aowUserDrawBox.TaowUserDrawBox LShieldUser;
        // { +0118 }
        public Aow.Units.aowUserDrawBox.TaowUserDrawBox RShieldUser;
        // { +011C }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0120 }
        public byte FPhase;
        // { $124 }
        public Aow.Units.FixedArray_byte_3 F_FastCombatWindow_125;
        public int FPhaseFrame;
        // { $128 }
        public int FAnimationFrame;
        // { $12C }
        public Aow.Units.AoWE.TCombatLogbook FLogbook;
        // { $130; 0043534B }
        public int FLeftShield;
        // { $134 }
        public byte FLeftOpacity;
        // { $138 }
        public Aow.Units.FixedArray_byte_3 F_FastCombatWindow_139;
        public int FRightShield;
        // { $13C }
        public byte FRightOpacity;
        // { $140 }
        public Aow.Units.FixedArray_byte_3 F_FastCombatWindow_141;
        public Aow.Units.AoWE.TCombatViewer FViewer;
        // { $144 }
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        // { $148 }
        public Aow.Units.EventLog.TEventLog FEvent;
        // FastCombatWin.pas:231
        // { $150 }
        public void GetPartyPosition(int Index, ref int X, ref int Y)
        {
            switch (Index)
            {
                case 0:
                    X = Party1Pnl.WinLeft;
                    Y = Party1Pnl.WinTop;
                    break;
                case 1:
                case 7:
                    X = Party2Pnl.WinLeft;
                    Y = Party2Pnl.WinTop;
                    break;
                case 2:
                    X = Party3Pnl.WinLeft;
                    Y = Party3Pnl.WinTop;
                    break;
                case 3:
                    X = Party4Pnl.WinLeft;
                    Y = Party4Pnl.WinTop;
                    break;
                case 4:
                    X = Party5Pnl.WinLeft;
                    Y = Party5Pnl.WinTop;
                    break;
                case 5:
                    X = Party6Pnl.WinLeft;
                    Y = Party6Pnl.WinTop;
                    break;
                case 6:
                    X = Party7Pnl.WinLeft;
                    Y = Party7Pnl.WinTop;
                    break;
            }
        }

        // FastCombatWin.pas:244
        public void DrawParties(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            int I = default;
            int PX = default;
            int PY = default;
            int Position = default;
            Aow.Units.AoWE.TCombat Combat = default;
            Aow.Units.AoWE.TCombatObject Obj = default;
            if (FViewer.FStatus.Contains(Aow.Units.AoWE.TCombatViewerStatusElement.vsInitialized))
            {
                Combat = FViewer.FCombat;
                {
                    I = 0;
                    int end__1 = Combat.FCombatData.GetCount() - 1;
                    for (; I <= end__1; I += 1)
                    {
                        var call__2 = Combat.FCombatData.GetObjects(I);
                        Obj = call__2;
                        if (Obj.FPosition != 128)
                        {
                            GetPartyPosition((int)(((uint)Obj.FPosition) >> 4), ref PX, ref PY);
                            PX = (PX + X) - 15;
                            PY = (PY + Y) - 32;
                            Position = Obj.FPosition & 15;
                            PX += (Position % 4) * 40;
                            if (Position >= 4)
                            {
                                PY += 38;
                            }

                            var call__3 = Combat.FCombatData.GetObjects(I);
                            FViewer.ShowFieldObject(call__3, FAnimationFrame, 3, PX, PY, Surface);
                        }
                    }
                }
            }
        }

        // FastCombatWin.pas:270
        public void DrawWalls(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            int Image = default;
            Aow.Units.AoWE.TCombatWall Wall = default;
            bool HasWall(int A, int B)
            {
                bool Result = default;
                Aow.Units.AoWE.TCombatParty PA = default;
                Aow.Units.AoWE.TCombatParty PB = default;
                bool WA = default;
                bool WB = default;
                var call__4 = FViewer.FCombat.FCombatData.FParties.GetParty(unchecked((byte)A));
                PA = call__4;
                var call__5 = FViewer.FCombat.FCombatData.FParties.GetParty(unchecked((byte)B));
                PB = call__5;
                WA = (PA != null) && (PA.FWall != 0);
                WB = (PB != null) && (PB.FWall != 0);
                Result = WA ^ WB;
                return Result;
            }

            Y -= 19;
            if (FViewer.FCombat.FCombatData.FWallType == 2)
            {
                Image = 80;
            }
            else
            {
                Image = 60;
            }

            if (FViewer.FCombat.FCombatData.FWallBreached)
            {
                Image += 10;
            }

            if (HasWall(1, 6))
            {
                var call__6 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatImages[Image];
                call__6.Show(X + 180, (Y + 218) - 50, Surface);
            }

            if (HasWall(1, 2))
            {
                var call__7 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatImages[Image];
                call__7.Show(X + 372, (Y + 218) - 50, Surface);
            }

            if (HasWall(0, 1))
            {
                var call__8 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatImages[Image + 4];
                call__8.Show(X + 180, Y + 218, Surface);
            }

            if (HasWall(0, 6))
            {
                var call__9 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatImages[Image + 5];
                call__9.Show(X + 180, Y + 218, Surface);
            }

            if (HasWall(0, 2))
            {
                var call__10 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatImages[Image + 5];
                call__10.Show(X + 372, Y + 218, Surface);
            }

            if (HasWall(5, 6))
            {
                var call__11 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatImages[Image + 3];
                call__11.Show(X + 0, Y + 275, Surface);
            }

            if (HasWall(2, 3))
            {
                var call__12 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatImages[Image + 2];
                call__12.Show(X + 374, Y + 275, Surface);
            }

            if (HasWall(0, 5))
            {
                var call__13 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatImages[Image + 5];
                call__13.Show(X + 180, Y + 275, Surface);
            }

            if (HasWall(0, 3))
            {
                var call__14 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatImages[Image + 5];
                call__14.Show(X + 372, Y + 275, Surface);
            }

            if (HasWall(0, 4))
            {
                var call__15 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatImages[Image + 4];
                call__15.Show(X + 180, Y + 332, Surface);
            }

            if (HasWall(4, 5))
            {
                var call__16 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatImages[Image + 1];
                call__16.Show(X + 180, Y + 342, Surface);
            }

            if (HasWall(4, 3))
            {
                var call__17 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatImages[Image + 1];
                call__17.Show(X + 372, Y + 342, Surface);
            }

            var combatData = FViewer.FCombat.FCombatData;
            if (combatData.FWallType != 0)
            {
                Wall = combatData.FWall;
                if (Wall != null)
                {
                    if (HasWall(0, 4))
                    {
                        var call__19 = Wall.GetHits();
                        var call__20 = Wall.GetHitPoints();
                        var call__21 = Aow.Units.AoWE.AoWE.HitPointsToHitPcnt(unchecked((sbyte)call__20), unchecked((sbyte)call__19));
                        Aow.Units.AoWE.AoWE.ShowHitPcnt(X + 250, (Y + 332) - 45, Surface, call__21);
                    }
                    else
                    {
                        var call__22 = Wall.GetHits();
                        var call__23 = Wall.GetHitPoints();
                        var call__24 = Aow.Units.AoWE.AoWE.HitPointsToHitPcnt(unchecked((sbyte)call__23), unchecked((sbyte)call__22));
                        Aow.Units.AoWE.AoWE.ShowHitPcnt(X + 250, (Y + 218) - 45, Surface, call__24);
                    }
                }
            }
        }

        // FastCombatWin.pas:316
        public void DrawCombat(int X, int Y, ref Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Units.AoWE.TCombat Combat = default;
            int I = default;
            int PX = default;
            int PY = default;
            int Glow = default;
            Combat = FViewer.FCombat;
            UpdateShields();
            if (ModeLbl.Text != Combat.FCombatData.FHeader)
            {
                ModeLbl.Text = Combat.FCombatData.FHeader;
            }

            DrawParties(X, Y + 20, Surface);
            if (Combat.FCombatData.FWallType != 0)
            {
                DrawWalls(X, Y + 20, Surface);
            }

            FViewer.ShowFightingObjects(FAnimationFrame, X + 234, Y + 15, Surface);
            if ((FPhase == 2) || (FPhase == 3))
            {
                {
                    I = 0;
                    byte end__25 = 7;
                    for (; I <= end__25; I += 1)
                    {
                        var call__26 = Combat.FCombatData.FParties.GetParty(unchecked((byte)I));
                        if (call__26 != null)
                        {
                            var call__27 = Combat.FCombatData.FParties.GetParty(unchecked((byte)I));
                            var call__28 = call__27.FArmy.GetCount();
                            if (call__28 == 0)
                            {
                                if (!TSet<byte>.WithBounds(0, 255).With(2).With(7).With(8).Contains(FViewer.FLogbook.FFinalData.F_CombatData_30))
                                {
                                    Glow = FPhaseFrame / 2;
                                    Glow = System.Math.Abs(((Glow + 7) % 15) - 7);
                                    GetPartyPosition(I, ref PX, ref PY);
                                    var call__29 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatImages[Glow + 100];
                                    call__29.ShowClipped((PX + X) - 10, (PY + Y) - 9, Surface);
                                }
                            }
                        }
                    }
                }
            }
        }

        // FastCombatWin.pas:340
        public void UpdateShields()
        {
            Aow.Units.AoWE.TCombatData Data = default;
            int Left = default;
            int Right = default;
            int LeftOpacity = default;
            int RightOpacity = default;
            Data = FViewer.FCombat.FCombatData;
            var call__30 = Data.FPlayers.GetPlayers(unchecked((sbyte)Data.FAttackerPlayer));
            if (call__30.FPlayer == 0)
            {
                Left = 12;
            }
            else
            {
                var call__31 = Data.FPlayers.GetPlayers(unchecked((sbyte)Data.FAttackerPlayer));
                var call__32 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__31.FPlayer);
                Left = call__32.FPlayerRace;
            }

            bool bool__33 = (Data.FCurrentPlayer == 255);
            if (!bool__33)
            {
                var call__34 = Data.FPlayers.GetPlayers(unchecked((sbyte)Data.FCurrentPlayer));
                bool__33 = (call__34.FSideIndex != 0);
            }

            if (bool__33)
            {
                LeftOpacity = 60;
            }
            else
            {
                LeftOpacity = 100;
            }

            if (FPhase == 3)
            {
                LeftOpacity = 100;
            }

            var call__35 = Data.FPlayers.GetPlayers(unchecked((sbyte)Data.FDefenderPlayer));
            if (call__35.FPlayer == 0)
            {
                Right = 12;
            }
            else
            {
                var call__36 = Data.FPlayers.GetPlayers(unchecked((sbyte)Data.FDefenderPlayer));
                var call__37 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__36.FPlayer);
                Right = call__37.FPlayerRace;
            }

            bool bool__38 = (Data.FCurrentPlayer == 255);
            if (!bool__38)
            {
                var call__39 = Data.FPlayers.GetPlayers(unchecked((sbyte)Data.FCurrentPlayer));
                bool__38 = (call__39.FSideIndex != 1);
            }

            if (bool__38)
            {
                RightOpacity = 60;
            }
            else
            {
                RightOpacity = 100;
            }

            if (FPhase == 3)
            {
                RightOpacity = 100;
            }

            if ((Left != FLeftShield) || (LeftOpacity != FLeftOpacity))
            {
                FLeftShield = Left;
                FLeftOpacity = unchecked((byte)LeftOpacity);
                LShieldUser.FDrawn = false;
            }

            if ((Right != FRightShield) || (RightOpacity != FRightOpacity))
            {
                FRightShield = Right;
                FRightOpacity = unchecked((byte)RightOpacity);
                RShieldUser.FDrawn = false;
            }
        }

        // FastCombatWin.pas:373
        public void FinishDialog()
        {
            Aow.Units.EventLog.TExecuteDoneEvent Done = default;
            Aow.Units.EventLog.TEventLog Log = default;
            FViewer.Finalize();
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
            Done = FDone;
            Log = FEvent;
            FDone = null;
            FEvent = null;
            if (Done != null)
            {
                Done(Log);
            }

            if (Log != null)
            {
                Log.Release();
            }
        }

        // FastCombatWin.pas:549
        // { 00435268 }
        public void SetPhase(byte Phase)
        {
            FPhase = unchecked((byte)Phase);
            FPhaseFrame = 0;
        }

        // FastCombatWin.pas:389
        public void FastCombatWindowCreate()
        {
            var call__40 = new Aow.Units.AoWE.TCombatViewer();
            FViewer = call__40;
        }

        // FastCombatWin.pas:394
        public void FastCombatWindowDestroy()
        {
            FViewer.Free();
        }

        // FastCombatWin.pas:399
        public void CloseButClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FCWin.Visible = false;
        }

        // FastCombatWin.pas:405
        // { draft }
        public void FCWinDrawSurface(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurface Surface, int Left, int Top)
        {
            Aow.Units.GFXE.TSurfaceDesc SD = default;
            try
            {
                if (!FViewer.FStatus.Contains(Aow.Units.AoWE.TCombatViewerStatusElement.vsPaused))
                {
                    FAnimationFrame += 1;
                }

                if (FPhase == 1)
                {
                    FViewer.Update();
                }

                if (Surface.Lock(ref SD) >= 0)
                {
                    if (SequencePositionSld.Position != FViewer.FPosition)
                    {
                        SequencePositionSld.Position = FViewer.FPosition;
                    }

                    SD.ClipRect.Top = FCWin.WinTop + 24;
                    DrawCombat(FCWin.WinLeft + 10, FCWin.WinTop + 10, ref SD);
                    Surface.Unlock();
                }
            }
            catch (System.Exception)
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"Exception during FastCombatWin.DrawSurface");
            }
        }

        // FastCombatWin.pas:424
        public void FCWinUpdate(Aow.Rtl.TObject Sender)
        {
            int Image = default;
            try
            {
                switch (FPhase)
                {
                    case 0:
                        Image = FPhaseFrame / 2;
                        if (Image > 9)
                        {
                            Image = 9;
                        }

                    {
                        if (Image != this.GlowImage.CurrentImage)
                        {
                            this.GlowImage.CurrentImage = Image + 20;
                        }
                    }

                        if (!GlowImage.Visible)
                        {
                            GlowImage.Visible = true;
                        }

                        if (FPhaseFrame > 20)
                        {
                            SetPhase(1);
                        }

                        break;
                    case 1:
                        if (GlowImage.CurrentImage != 29)
                        {
                            GlowImage.CurrentImage = 29;
                        }

                        if (!GlowImage.Visible)
                        {
                            GlowImage.Visible = true;
                        }

                    {
                        var call__43 = this.FViewer.GetEndPosition();
                        if (this.FViewer.FPosition >= call__43)
                        {
                            SetPhase(2);
                        }
                    }

                        break;
                    case 2:
                        Image = 9 - (FPhaseFrame / 2);
                        if (Image < 0)
                        {
                            Image = 0;
                        }

                    {
                        if (Image != this.GlowImage.CurrentImage)
                        {
                            this.GlowImage.CurrentImage = Image + 20;
                        }
                    }

                        if (!GlowImage.Visible)
                        {
                            GlowImage.Visible = true;
                        }

                        if (FPhaseFrame > 20)
                        {
                            SetPhase(3);
                            FPhaseFrame = 21;
                        }

                        break;
                    case 3:
                        if (GlowImage.Visible)
                        {
                            GlowImage.Visible = false;
                            FCWin.FDrawn = false;
                        }

                        break;
                }

                FCWin.FSurfaceValid = false;
                FPhaseFrame += 1;
            }
            catch (System.Exception)
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"Exception during FastCombatWin.Update");
            }
        }

        // FastCombatWin.pas:466
        public void InitialViewClick()
        {
            Aow.Units.AoWE.TCombatViewer Viewer = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Viewer = FViewer;
            if ((Viewer != null) && Viewer.FStatus.Contains(Aow.Units.AoWE.TCombatViewerStatusElement.vsInitialized))
            {
                Viewer.SetPosition(0);
                SetPhase(1);
            }
        }

        // FastCombatWin.pas:479
        // { draft }
        public void FinalViewClick()
        {
            Aow.Units.AoWE.TCombatViewer Viewer = default;
            ;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Viewer = FViewer;
            if ((Viewer != null) && Viewer.FStatus.Contains(Aow.Units.AoWE.TCombatViewerStatusElement.vsInitialized))
            {
                var call__45 = Viewer.GetEndPosition();
                Viewer.SetPosition(call__45 + 1);
                SetPhase(3);
            }
        }

        // FastCombatWin.pas:492
        // { draft }
        public void PauseBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if ((FViewer != null) && FViewer.FStatus.Contains(Aow.Units.AoWE.TCombatViewerStatusElement.vsInitialized))
            {
                FViewer.Pause();
            }
        }

        // FastCombatWin.pas:498
        // { draft }
        public void PlayBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if ((FViewer != null) && FViewer.FStatus.Contains(Aow.Units.AoWE.TCombatViewerStatusElement.vsInitialized))
            {
                FViewer.Play();
            }
        }

        // FastCombatWin.pas:504
        // { draft }
        public void SequencePositionSldChange()
        {
            Aow.Units.AoWE.TCombatViewer Viewer = default;
            Viewer = FViewer;
            if ((Viewer != null) && Viewer.FStatus.Contains(Aow.Units.AoWE.TCombatViewerStatusElement.vsInitialized))
            {
                Viewer.SetPosition(SequencePositionSld.Position);
                SetPhase(1);
            }
        }

        // FastCombatWin.pas:516
        // { draft }
        public void SequenceSpeedSldChange()
        {
            if (FViewer != null)
            {
                FViewer.SetSpeed(unchecked((byte)(20 - SequenceSpeedSld.Position)));
            }
        }

        // FastCombatWin.pas:521
        // { draft }
        public void LShieldUserDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            // { Tried with/local receiver; Sender Pointer identity; derived Sender and base component view; no exact match. }
            if (FLeftOpacity == 100)
            {
                var call__46 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields[FLeftShield];
                call__46.Show(FCWin.WinLeft + Sender.WinLeft, FCWin.WinTop + Sender.WinTop, Surface);
            }
            else
            {
                var call__47 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields[FLeftShield];
                call__47.ShowBlended(FCWin.WinLeft + Sender.WinLeft, FCWin.WinTop + Sender.WinTop, Surface, Aow.Units.GFXE.GFXE.AlphaBlendTable.LookupTables[FLeftOpacity]);
            }
        }

        // FastCombatWin.pas:530
        public void RShieldUserDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            // { Tried with/local receiver; Sender Pointer identity; derived Sender and base component view; no exact match. }
            if (FRightOpacity == 100)
            {
                var call__48 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields[FRightShield];
                call__48.Show(FCWin.WinLeft + Sender.WinLeft, FCWin.WinTop + Sender.WinTop, Surface);
            }
            else
            {
                var call__49 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields[FRightShield];
                call__49.ShowBlended(FCWin.WinLeft + Sender.WinLeft, FCWin.WinTop + Sender.WinTop, Surface, Aow.Units.GFXE.GFXE.AlphaBlendTable.LookupTables[FRightOpacity]);
            }
        }

        // FastCombatWin.pas:539
        public void FCWinHide()
        {
            FinishDialog();
        }

        // FastCombatWin.pas:544
        // { draft }
        public void FCWinKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if ((Key == 27) || (Key == 32))
            {
                FCWin.Visible = false;
            }
        }
    }

    public static partial class FastCombatWin
    {
        // FastCombatWin.pas:124
        public static void ShowCombatEvent(Aow.Units.FastCombatWin.TFastCombatWindow Dialog, Aow.Units.AoWE.TCombatEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Aow.Rtl.Rtl.Assert(Dialog.FEvent == null, (AnsiString)"", "D:\\git\\aow\\src\\delphi3\\AoW\\FastCombatWin.pas", 159);
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 49;
            Dialog.FEvent = Event;
            Dialog.FEvent.AddRef();
            Dialog.FDone = Done;
            Dialog.FLogbook = Event.FLogbook;
            Aow.Units.FastCombatWin.FastCombatWin.InitializeCombatDialog(Dialog, Dialog.FLogbook);
            Dialog.FCWin.Visible = true;
        }

        // FastCombatWin.pas:169
        public static void InitializeCombatDialog(Aow.Units.FastCombatWin.TFastCombatWindow Dialog, Aow.Units.AoWE.TCombatLogbook Logbook)
        {
            int RaceIndex = default;
            int PartyRace = default;
            sbyte PlayerIndex = default;
            Aow.Units.AoWE.TCombat Combat = default;
            Aow.Units.AoWE.TCombatParty Party = default;
            Aow.Units.AoWImage.TAOWImage Image = default;
            int I = default;
            Dialog.FViewer.Initialize(Logbook);
            Combat = Dialog.FViewer.FCombat;
            Dialog.FAnimationFrame = 0;
            {
                I = 0;
                byte end__52 = 7;
                for (; I <= end__52; I += 1)
                {
                    switch (I)
                    {
                        case 0:
                            Image = Dialog.Party1Bg;
                            break;
                        case 1:
                            Image = Dialog.Party2Bg;
                            break;
                        case 2:
                            Image = Dialog.Party3Bg;
                            break;
                        case 3:
                            Image = Dialog.Party4Bg;
                            break;
                        case 4:
                            Image = Dialog.Party5Bg;
                            break;
                        case 5:
                            Image = Dialog.Party6Bg;
                            break;
                        case 6:
                            Image = Dialog.Party7Bg;
                            break;
                        case 7:
                            // { Original reuses the second background for the eighth party. }
                            Image = Dialog.Party2Bg;
                            break;
                        default:
                            Image = null;
                            break;
                    }

                    var call__53 = Combat.FCombatData.FParties.GetParty(unchecked((byte)I));
                    Party = call__53;
                    if (Party != null)
                    {
                        var call__54 = Combat.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)Party.FPlayerIndex));
                        PlayerIndex = unchecked((sbyte)call__54.FPlayer);
                        if (PlayerIndex != 0)
                        {
                            var call__55 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(PlayerIndex);
                            PartyRace = call__55.FPlayerRace;
                        }
                        else
                        {
                            PartyRace = 12;
                        }

                        Image.CurrentImage = PartyRace + 21;
                        Image.Visible = true;
                    }
                    else
                    {
                        if (I != 7)
                        {
                            Image.Visible = false;
                        }
                    }

                    // { Original refreshes both side labels inside the party loop. }
                    var call__56 = Combat.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)Combat.FCombatData.FAttackerPlayer));
                    if (call__56.FPlayer == 0)
                    {
                        var call__57 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.IndependentsRStr);
                        Dialog.LSideLbl.Text = call__57;
                    }
                    else
                    {
                        var call__58 = Combat.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)Combat.FCombatData.FAttackerPlayer));
                        var call__59 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__58.FPlayer);
                        RaceIndex = call__59.FPlayerRace;
                        var call__60 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(RaceIndex);
                        Dialog.LSideLbl.Text = call__60.FRaceName;
                    }

                    var call__61 = Combat.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)Combat.FCombatData.FDefenderPlayer));
                    if (call__61.FPlayer == 0)
                    {
                        var call__62 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.IndependentsRStr);
                        Dialog.RSideLbl.Text = call__62;
                    }
                    else
                    {
                        var call__63 = Combat.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)Combat.FCombatData.FDefenderPlayer));
                        var call__64 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__63.FPlayer);
                        RaceIndex = call__64.FPlayerRace;
                        var call__65 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(RaceIndex);
                        Dialog.RSideLbl.Text = call__65.FRaceName;
                    }
                }
            }

            Dialog.FViewer.Play();
            Dialog.FCWin.FDrawn = false;
            var call__66 = Dialog.FViewer.GetEndPosition();
            Dialog.SequencePositionSld.Max = call__66;
            Dialog.SequenceSpeedSld.Position = 20 - Dialog.FViewer.FDelay;
            var call__67 = Dialog.FViewer.GetEndPosition();
            if (call__67 != -1)
            {
                Dialog.SetPhase(0);
            }
            else
            {
                Dialog.SetPhase(3);
            }
        }
    }
}
