// Translated from FastCombatWin.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.EventLog;
using Aow.Units.AoWComp;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWHScroll;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWPanel;
using Aow.Units.AoWWindow;
using Aow.Units.aowUserDrawBox;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.aowIntGfxMod.aowIntGfxMod;

namespace Aow.Units.FastCombatWin
{
    // phase, phase frame, animation frame, and shield indices/opacities, combat viewer, completion
    // event, retained event
    public partial class TFastCombatWindow : TDataModule
    {
        public TFastCombatWindow(TComponent owner) : base(owner)
        { }

        public TAOWWindow FCWin;
        public TAOWPanel LNamePnl;
        public TAOWLabel LSideLbl;
        public TAOWPanel RNamePnl;
        public TAOWLabel RSideLbl;
        public TAOWPanel LCtrlPnl;
        public TAOWPanel RCtrlPnl;
        public TAOWCheckBox PlayBtn2;
        public TAOWCheckBox PauseBtn2;
        public TAOWCheckBox StartBtn;
        public TAOWCheckBox EndBtn;
        public TAOWButton CloseBut;
        public TAOWPanel Party1Pnl;
        public TAOWImage Party1Bg;
        public TAOWImage Part1Dead;
        public TAOWPanel Party2Pnl;
        public TAOWImage Party2Bg;
        public TAOWImage Party2Dead;
        public TAOWPanel Party3Pnl;
        public TAOWImage Party3Bg;
        public TAOWImage Party3Dead;
        public TAOWPanel Party4Pnl;
        public TAOWImage Party4Bg;
        public TAOWImage Party4Dead;
        public TAOWPanel Party5Pnl;
        public TAOWImage Party5Bg;
        public TAOWImage Party5Dead;
        public TAOWPanel Party6Pnl;
        public TAOWImage Party6Bg;
        public TAOWImage Party6Dead;
        public TAOWPanel Party7Pnl;
        public TAOWImage Party7Bg;
        public TAOWImage Party7Dead;
        public TAOWPanel LSidePnl;
        public TAOWPanel RSidePnl;
        public TAOWPanel FightPnl;
        public TAOWImage LShield;
        public TAOWImage RShield;
        public TAOWButton InitialView;
        public TAOWButton FinalView;
        public TAOWButton PlayBtn;
        public TAOWButton PauseBtn;
        public TAOWHScrollBar SequencePositionSld;
        public TAOWHScrollBar SequenceSpeedSld;
        public TImageLibrary GlowAnimation;
        public TAOWImage GlowImage;
        public TAOWButton PrevSeqBtn;
        public TAOWButton NextSeqBtn;
        public TAOWLabel StartLBl;
        public TAOWLabel FastLbl;
        public TAOWLabel SlowLbl;
        public TAOWLabel EndLbl;
        public TAOWLabel ModeLbl;
        public TaowUserDrawBox LShieldUser;
        public TaowUserDrawBox RShieldUser;
        public TIvTranslator Translator;
        public byte FPhase;
        public int FPhaseFrame;
        public int FAnimationFrame;
        public TCombatLogbook FLogbook;
        public int FLeftShield;
        public byte FLeftOpacity;
        public int FRightShield;
        public byte FRightOpacity;
        public TCombatViewer FViewer;
        public TExecuteDoneEvent FDone;
        public TEventLog FEvent;
        // FastCombatWin.pas:231
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
        public void DrawParties(int X, int Y, in TSurfaceDesc Surface)
        {
            int I = default;
            int PX = default;
            int PY = default;
            int Position = default;
            TCombat Combat = default;
            TCombatObject Obj = default;
            if (FViewer.FStatus.Contains(TCombatViewerStatusElement.vsInitialized))
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
                            PX = PX + X - 15;
                            PY = PY + Y - 32;
                            Position = Obj.FPosition & 15;
                            PX += Position % 4 * 40;
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
        public void DrawWalls(int X, int Y, in TSurfaceDesc Surface)
        {
            int Image = default;
            TCombatWall Wall = default;
            bool HasWall(int A, int B)
            {
                bool Result = default;
                TCombatParty PA = default;
                TCombatParty PB = default;
                bool WA = default;
                bool WB = default;
                var call__4 = FViewer.FCombat.FCombatData.FParties.GetParty((byte)A);
                PA = call__4;
                var call__5 = FViewer.FCombat.FCombatData.FParties.GetParty((byte)B);
                PB = call__5;
                WA = PA != null && PA.FWall != 0;
                WB = PB != null && PB.FWall != 0;
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
                var call__6 = AoWHSSet.FCombatImages[Image];
                call__6.Show(X + 180, Y + 218 - 50, Surface);
            }

            if (HasWall(1, 2))
            {
                var call__7 = AoWHSSet.FCombatImages[Image];
                call__7.Show(X + 372, Y + 218 - 50, Surface);
            }

            if (HasWall(0, 1))
            {
                var call__8 = AoWHSSet.FCombatImages[Image + 4];
                call__8.Show(X + 180, Y + 218, Surface);
            }

            if (HasWall(0, 6))
            {
                var call__9 = AoWHSSet.FCombatImages[Image + 5];
                call__9.Show(X + 180, Y + 218, Surface);
            }

            if (HasWall(0, 2))
            {
                var call__10 = AoWHSSet.FCombatImages[Image + 5];
                call__10.Show(X + 372, Y + 218, Surface);
            }

            if (HasWall(5, 6))
            {
                var call__11 = AoWHSSet.FCombatImages[Image + 3];
                call__11.Show(X + 0, Y + 275, Surface);
            }

            if (HasWall(2, 3))
            {
                var call__12 = AoWHSSet.FCombatImages[Image + 2];
                call__12.Show(X + 374, Y + 275, Surface);
            }

            if (HasWall(0, 5))
            {
                var call__13 = AoWHSSet.FCombatImages[Image + 5];
                call__13.Show(X + 180, Y + 275, Surface);
            }

            if (HasWall(0, 3))
            {
                var call__14 = AoWHSSet.FCombatImages[Image + 5];
                call__14.Show(X + 372, Y + 275, Surface);
            }

            if (HasWall(0, 4))
            {
                var call__15 = AoWHSSet.FCombatImages[Image + 4];
                call__15.Show(X + 180, Y + 332, Surface);
            }

            if (HasWall(4, 5))
            {
                var call__16 = AoWHSSet.FCombatImages[Image + 1];
                call__16.Show(X + 180, Y + 342, Surface);
            }

            if (HasWall(4, 3))
            {
                var call__17 = AoWHSSet.FCombatImages[Image + 1];
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
                        var call__21 = HitPointsToHitPcnt(call__20, call__19);
                        ShowHitPcnt(X + 250, Y + 332 - 45, Surface, call__21);
                    }
                    else
                    {
                        var call__22 = Wall.GetHits();
                        var call__23 = Wall.GetHitPoints();
                        var call__24 = HitPointsToHitPcnt(call__23, call__22);
                        ShowHitPcnt(X + 250, Y + 218 - 45, Surface, call__24);
                    }
                }
            }
        }

        // FastCombatWin.pas:316
        public void DrawCombat(int X, int Y, ref TSurfaceDesc Surface)
        {
            TCombat Combat = default;
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
            if (FPhase == 2 || FPhase == 3)
            {
                {
                    I = 0;
                    byte end__25 = 7;
                    for (; I <= end__25; I += 1)
                    {
                        var call__26 = Combat.FCombatData.FParties.GetParty((byte)I);
                        if (call__26 != null)
                        {
                            var call__27 = Combat.FCombatData.FParties.GetParty((byte)I);
                            var call__28 = call__27.FArmy.GetCount();
                            if (call__28 == 0)
                            {
                                if (!TSet<byte>.WithBounds(0, 255).With(2).With(7).With(8).Contains(FViewer.FLogbook.FFinalData.F_CombatData_30))
                                {
                                    Glow = FPhaseFrame / 2;
                                    Glow = Math.Abs((Glow + 7) % 15 - 7);
                                    GetPartyPosition(I, ref PX, ref PY);
                                    var call__29 = AoWHSSet.FCombatImages[Glow + 100];
                                    call__29.ShowClipped(PX + X - 10, PY + Y - 9, Surface);
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
            TCombatData Data = default;
            int Left = default;
            int Right = default;
            int LeftOpacity = default;
            int RightOpacity = default;
            Data = FViewer.FCombat.FCombatData;
            var call__30 = Data.FPlayers.GetPlayers(Data.FAttackerPlayer);
            if (call__30.FPlayer == 0)
            {
                Left = 12;
            }
            else
            {
                var call__31 = Data.FPlayers.GetPlayers(Data.FAttackerPlayer);
                var call__32 = AoWHSMap.FPlayers.GetPlayers(call__31.FPlayer);
                Left = call__32.FPlayerRace;
            }

            bool bool__33 = Data.FCurrentPlayer == 255;
            if (!bool__33)
            {
                var call__34 = Data.FPlayers.GetPlayers((sbyte)Data.FCurrentPlayer);
                bool__33 = call__34.FSideIndex != 0;
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

            var call__35 = Data.FPlayers.GetPlayers(Data.FDefenderPlayer);
            if (call__35.FPlayer == 0)
            {
                Right = 12;
            }
            else
            {
                var call__36 = Data.FPlayers.GetPlayers(Data.FDefenderPlayer);
                var call__37 = AoWHSMap.FPlayers.GetPlayers(call__36.FPlayer);
                Right = call__37.FPlayerRace;
            }

            bool bool__38 = Data.FCurrentPlayer == 255;
            if (!bool__38)
            {
                var call__39 = Data.FPlayers.GetPlayers((sbyte)Data.FCurrentPlayer);
                bool__38 = call__39.FSideIndex != 1;
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

            if (Left != FLeftShield || LeftOpacity != FLeftOpacity)
            {
                FLeftShield = Left;
                FLeftOpacity = (byte)LeftOpacity;
                LShieldUser.FDrawn = false;
            }

            if (Right != FRightShield || RightOpacity != FRightOpacity)
            {
                FRightShield = Right;
                FRightOpacity = (byte)RightOpacity;
                RShieldUser.FDrawn = false;
            }
        }

        // FastCombatWin.pas:373
        public void FinishDialog()
        {
            TExecuteDoneEvent Done = default;
            TEventLog Log = default;
            FViewer.Finalize();
            General.Manager.OverlayPriority = 0;
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
        public void SetPhase(byte Phase)
        {
            FPhase = Phase;
            FPhaseFrame = 0;
        }

        // FastCombatWin.pas:389
        public void FastCombatWindowCreate()
        {
            var call__40 = new TCombatViewer();
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
            General.PlayButtonSound(0);
            FCWin.Visible = false;
        }

        // FastCombatWin.pas:405
        public void FCWinDrawSurface(TObject Sender, TSurface Surface, int Left, int Top)
        {
            TSurfaceDesc SD = default;
            try
            {
                if (!FViewer.FStatus.Contains(TCombatViewerStatusElement.vsPaused))
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
                throw Exception.Create((AnsiString)"Exception during FastCombatWin.DrawSurface");
            }
        }

        // FastCombatWin.pas:424
        public void FCWinUpdate(TObject Sender)
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
                        Image = 9 - FPhaseFrame / 2;
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
                throw Exception.Create((AnsiString)"Exception during FastCombatWin.Update");
            }
        }

        // FastCombatWin.pas:466
        public void InitialViewClick()
        {
            TCombatViewer Viewer = default;
            General.PlayButtonSound(0);
            Viewer = FViewer;
            if (Viewer != null && Viewer.FStatus.Contains(TCombatViewerStatusElement.vsInitialized))
            {
                Viewer.SetPosition(0);
                SetPhase(1);
            }
        }

        // FastCombatWin.pas:479
        public void FinalViewClick()
        {
            TCombatViewer Viewer = default;
            ;
            General.PlayButtonSound(0);
            Viewer = FViewer;
            if (Viewer != null && Viewer.FStatus.Contains(TCombatViewerStatusElement.vsInitialized))
            {
                var call__45 = Viewer.GetEndPosition();
                Viewer.SetPosition(call__45 + 1);
                SetPhase(3);
            }
        }

        // FastCombatWin.pas:492
        public void PauseBtnClick()
        {
            General.PlayButtonSound(0);
            if (FViewer != null && FViewer.FStatus.Contains(TCombatViewerStatusElement.vsInitialized))
            {
                FViewer.Pause();
            }
        }

        // FastCombatWin.pas:498
        public void PlayBtnClick()
        {
            General.PlayButtonSound(0);
            if (FViewer != null && FViewer.FStatus.Contains(TCombatViewerStatusElement.vsInitialized))
            {
                FViewer.Play();
            }
        }

        // FastCombatWin.pas:504
        public void SequencePositionSldChange()
        {
            TCombatViewer Viewer = default;
            Viewer = FViewer;
            if (Viewer != null && Viewer.FStatus.Contains(TCombatViewerStatusElement.vsInitialized))
            {
                Viewer.SetPosition(SequencePositionSld.Position);
                SetPhase(1);
            }
        }

        // FastCombatWin.pas:516
        public void SequenceSpeedSldChange()
        {
            if (FViewer != null)
            {
                FViewer.SetSpeed((byte)(20 - SequenceSpeedSld.Position));
            }
        }

        // FastCombatWin.pas:521
        public void LShieldUserDraw(TAoWComponent Sender, TSurfaceDesc Surface)
        {
            if (FLeftOpacity == 100)
            {
                var call__46 = IntGfxMod.Shields[FLeftShield];
                call__46.Show(FCWin.WinLeft + Sender.WinLeft, FCWin.WinTop + Sender.WinTop, Surface);
            }
            else
            {
                var call__47 = IntGfxMod.Shields[FLeftShield];
                call__47.ShowBlended(FCWin.WinLeft + Sender.WinLeft, FCWin.WinTop + Sender.WinTop, Surface, AlphaBlendTable.LookupTables[FLeftOpacity]);
            }
        }

        // FastCombatWin.pas:530
        public void RShieldUserDraw(TAoWComponent Sender, TSurfaceDesc Surface)
        {
            if (FRightOpacity == 100)
            {
                var call__48 = IntGfxMod.Shields[FRightShield];
                call__48.Show(FCWin.WinLeft + Sender.WinLeft, FCWin.WinTop + Sender.WinTop, Surface);
            }
            else
            {
                var call__49 = IntGfxMod.Shields[FRightShield];
                call__49.ShowBlended(FCWin.WinLeft + Sender.WinLeft, FCWin.WinTop + Sender.WinTop, Surface, AlphaBlendTable.LookupTables[FRightOpacity]);
            }
        }

        // FastCombatWin.pas:539
        public void FCWinHide()
        {
            FinishDialog();
        }

        // FastCombatWin.pas:544
        public void FCWinKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
        {
            if (Key == 27 || Key == 32)
            {
                FCWin.Visible = false;
            }
        }
    }

    public static partial class FastCombatWin
    {
        // FastCombatWin.pas:124
        public static void ShowCombatEvent(TFastCombatWindow Dialog, TCombatEventLog Event, TExecuteDoneEvent Done)
        {
            Assert(Dialog.FEvent == null, (AnsiString)"", "FastCombatWin.pas", 159);
            General.Manager.OverlayPriority = 49;
            Dialog.FEvent = Event;
            Dialog.FEvent.AddRef();
            Dialog.FDone = Done;
            Dialog.FLogbook = Event.FLogbook;
            InitializeCombatDialog(Dialog, Dialog.FLogbook);
            Dialog.FCWin.Visible = true;
        }

        // FastCombatWin.pas:169
        public static void InitializeCombatDialog(TFastCombatWindow Dialog, TCombatLogbook Logbook)
        {
            int RaceIndex = default;
            int PartyRace = default;
            sbyte PlayerIndex = default;
            TCombat Combat = default;
            TCombatParty Party = default;
            TAOWImage Image = default;
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

                    var call__53 = Combat.FCombatData.FParties.GetParty((byte)I);
                    Party = call__53;
                    if (Party != null)
                    {
                        var call__54 = Combat.FCombatData.FPlayers.GetPlayers((sbyte)Party.FPlayerIndex);
                        PlayerIndex = call__54.FPlayer;
                        if (PlayerIndex != 0)
                        {
                            var call__55 = AoWHSMap.FPlayers.GetPlayers(PlayerIndex);
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
                    var call__56 = Combat.FCombatData.FPlayers.GetPlayers(Combat.FCombatData.FAttackerPlayer);
                    if (call__56.FPlayer == 0)
                    {
                        var call__57 = TranslateRStr(E3Globals.E3Globals.IndependentsRStr);
                        Dialog.LSideLbl.Text = call__57;
                    }
                    else
                    {
                        var call__58 = Combat.FCombatData.FPlayers.GetPlayers(Combat.FCombatData.FAttackerPlayer);
                        var call__59 = AoWHSMap.FPlayers.GetPlayers(call__58.FPlayer);
                        RaceIndex = call__59.FPlayerRace;
                        var call__60 = AoWHSSet.FRaceResources.GetRaceResource(RaceIndex);
                        Dialog.LSideLbl.Text = call__60.FRaceName;
                    }

                    var call__61 = Combat.FCombatData.FPlayers.GetPlayers(Combat.FCombatData.FDefenderPlayer);
                    if (call__61.FPlayer == 0)
                    {
                        var call__62 = TranslateRStr(E3Globals.E3Globals.IndependentsRStr);
                        Dialog.RSideLbl.Text = call__62;
                    }
                    else
                    {
                        var call__63 = Combat.FCombatData.FPlayers.GetPlayers(Combat.FCombatData.FDefenderPlayer);
                        var call__64 = AoWHSMap.FPlayers.GetPlayers(call__63.FPlayer);
                        RaceIndex = call__64.FPlayerRace;
                        var call__65 = AoWHSSet.FRaceResources.GetRaceResource(RaceIndex);
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
