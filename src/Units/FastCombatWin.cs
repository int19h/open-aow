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

namespace Aow.Units.FastCombatWin;

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
        int PX = default;
        int PY = default;
        int Position;
        TCombat Combat;
        TCombatObject Obj;
        if (FViewer.FStatus.Contains(TCombatViewerStatusElement.vsInitialized))
        {
            Combat = FViewer.FCombat;
            for (int I = 0, last = Combat.FCombatData.GetCount() - 1; I <= last; I++)
            {
                Obj = Combat.FCombatData.GetObjects(I);
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

                    var combatObject = Combat.FCombatData.GetObjects(I);
                    FViewer.ShowFieldObject(combatObject, FAnimationFrame, 3, PX, PY, Surface);
                }
            }
        }
    }

    // FastCombatWin.pas:270
    public void DrawWalls(int X, int Y, in TSurfaceDesc Surface)
    {
        int Image;
        TCombatWall Wall;
        bool HasWall(int A, int B)
        {
            TCombatParty PA;
            TCombatParty PB;
            bool WA;
            bool WB;
            PA = FViewer.FCombat.FCombatData.FParties.GetParty((byte)A);
            PB = FViewer.FCombat.FCombatData.FParties.GetParty((byte)B);
            WA = PA != null && PA.FWall != 0;
            WB = PB != null && PB.FWall != 0;
            return WA ^ WB;
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
            AoWHSSet.FCombatImages[Image].Show(X + 180, Y + 218 - 50, Surface);
        }

        if (HasWall(1, 2))
        {
            AoWHSSet.FCombatImages[Image].Show(X + 372, Y + 218 - 50, Surface);
        }

        if (HasWall(0, 1))
        {
            AoWHSSet.FCombatImages[Image + 4].Show(X + 180, Y + 218, Surface);
        }

        if (HasWall(0, 6))
        {
            AoWHSSet.FCombatImages[Image + 5].Show(X + 180, Y + 218, Surface);
        }

        if (HasWall(0, 2))
        {
            AoWHSSet.FCombatImages[Image + 5].Show(X + 372, Y + 218, Surface);
        }

        if (HasWall(5, 6))
        {
            AoWHSSet.FCombatImages[Image + 3].Show(X + 0, Y + 275, Surface);
        }

        if (HasWall(2, 3))
        {
            AoWHSSet.FCombatImages[Image + 2].Show(X + 374, Y + 275, Surface);
        }

        if (HasWall(0, 5))
        {
            AoWHSSet.FCombatImages[Image + 5].Show(X + 180, Y + 275, Surface);
        }

        if (HasWall(0, 3))
        {
            AoWHSSet.FCombatImages[Image + 5].Show(X + 372, Y + 275, Surface);
        }

        if (HasWall(0, 4))
        {
            AoWHSSet.FCombatImages[Image + 4].Show(X + 180, Y + 332, Surface);
        }

        if (HasWall(4, 5))
        {
            AoWHSSet.FCombatImages[Image + 1].Show(X + 180, Y + 342, Surface);
        }

        if (HasWall(4, 3))
        {
            AoWHSSet.FCombatImages[Image + 1].Show(X + 372, Y + 342, Surface);
        }

        var combatData = FViewer.FCombat.FCombatData;
        if (combatData.FWallType != 0)
        {
            Wall = combatData.FWall;
            if (Wall != null)
            {
                if (HasWall(0, 4))
                {
                    var hits = Wall.GetHits();
                    var hitPointsToHitPcnt = HitPointsToHitPcnt(Wall.GetHitPoints(), hits);
                    ShowHitPcnt(X + 250, Y + 332 - 45, Surface, hitPointsToHitPcnt);
                }
                else
                {
                    var hits = Wall.GetHits();
                    var hitPointsToHitPcnt = HitPointsToHitPcnt(Wall.GetHitPoints(), hits);
                    ShowHitPcnt(X + 250, Y + 218 - 45, Surface, hitPointsToHitPcnt);
                }
            }
        }
    }

    // FastCombatWin.pas:316
    public void DrawCombat(int X, int Y, ref TSurfaceDesc Surface)
    {
        TCombat Combat;
        int PX = default;
        int PY = default;
        int Glow;
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
            for (int I = 0; I <= 7; I++)
            {
                if (Combat.FCombatData.FParties.GetParty((byte)I) != null)
                {
                    if (Combat.FCombatData.FParties.GetParty((byte)I).FArmy.GetCount() == 0)
                    {
                        if (!TSet<byte>.WithBounds(0, 255).With(2).With(7).With(8).Contains(FViewer.FLogbook.FFinalData.F_CombatData_30))
                        {
                            Glow = FPhaseFrame / 2;
                            Glow = Math.Abs((Glow + 7) % 15 - 7);
                            GetPartyPosition(I, ref PX, ref PY);
                            AoWHSSet.FCombatImages[Glow + 100].ShowClipped(PX + X - 10, PY + Y - 9, Surface);
                        }
                    }
                }
            }
        }
    }

    // FastCombatWin.pas:340
    public void UpdateShields()
    {
        TCombatData Data;
        int Left;
        int Right;
        int LeftOpacity;
        int RightOpacity;
        Data = FViewer.FCombat.FCombatData;
        if (Data.FPlayers.GetPlayers(Data.FAttackerPlayer).FPlayer == 0)
        {
            Left = 12;
        }
        else
        {
            var player = Data.FPlayers.GetPlayers(Data.FAttackerPlayer);
            Left = AoWHSMap.FPlayers.GetPlayers(player.FPlayer).FPlayerRace;
        }

        if (Data.FCurrentPlayer == 255 || Data.FPlayers.GetPlayers((sbyte)Data.FCurrentPlayer).FSideIndex != 0)
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

        if (Data.FPlayers.GetPlayers(Data.FDefenderPlayer).FPlayer == 0)
        {
            Right = 12;
        }
        else
        {
            var player = Data.FPlayers.GetPlayers(Data.FDefenderPlayer);
            Right = AoWHSMap.FPlayers.GetPlayers(player.FPlayer).FPlayerRace;
        }

        if (Data.FCurrentPlayer == 255 || Data.FPlayers.GetPlayers((sbyte)Data.FCurrentPlayer).FSideIndex != 1)
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
        TExecuteDoneEvent Done;
        TEventLog Log;
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
        FViewer = new TCombatViewer();
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
        catch
        {
            throw Exception.Create("Exception during FastCombatWin.DrawSurface");
        }
    }

    // FastCombatWin.pas:424
    public void FCWinUpdate(TObject Sender)
    {
        int Image;
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
                    var endPosition = this.FViewer.GetEndPosition();
                    if (this.FViewer.FPosition >= endPosition)
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
        catch
        {
            throw Exception.Create("Exception during FastCombatWin.Update");
        }
    }

    // FastCombatWin.pas:466
    public void InitialViewClick()
    {
        TCombatViewer Viewer;
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
        TCombatViewer Viewer;
        General.PlayButtonSound(0);
        Viewer = FViewer;
        if (Viewer != null && Viewer.FStatus.Contains(TCombatViewerStatusElement.vsInitialized))
        {
            Viewer.SetPosition(Viewer.GetEndPosition() + 1);
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
        TCombatViewer Viewer;
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
            IntGfxMod.Shields[FLeftShield].Show(FCWin.WinLeft + Sender.WinLeft, FCWin.WinTop + Sender.WinTop, Surface);
        }
        else
        {
            IntGfxMod.Shields[FLeftShield].ShowBlended(FCWin.WinLeft + Sender.WinLeft, FCWin.WinTop + Sender.WinTop, Surface, AlphaBlendTable.LookupTables[FLeftOpacity]);
        }
    }

    // FastCombatWin.pas:530
    public void RShieldUserDraw(TAoWComponent Sender, TSurfaceDesc Surface)
    {
        if (FRightOpacity == 100)
        {
            IntGfxMod.Shields[FRightShield].Show(FCWin.WinLeft + Sender.WinLeft, FCWin.WinTop + Sender.WinTop, Surface);
        }
        else
        {
            IntGfxMod.Shields[FRightShield].ShowBlended(FCWin.WinLeft + Sender.WinLeft, FCWin.WinTop + Sender.WinTop, Surface, AlphaBlendTable.LookupTables[FRightOpacity]);
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
        Assert(Dialog.FEvent == null, "", "FastCombatWin.pas", 159);
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
        int RaceIndex;
        int PartyRace;
        sbyte PlayerIndex;
        TCombat Combat;
        TCombatParty Party;
        TAOWImage Image;
        Dialog.FViewer.Initialize(Logbook);
        Combat = Dialog.FViewer.FCombat;
        Dialog.FAnimationFrame = 0;
        for (int I = 0; I <= 7; I++)
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

            Party = Combat.FCombatData.FParties.GetParty((byte)I);
            if (Party != null)
            {
                PlayerIndex = Combat.FCombatData.FPlayers.GetPlayers((sbyte)Party.FPlayerIndex).FPlayer;
                if (PlayerIndex != 0)
                {
                    PartyRace = AoWHSMap.FPlayers.GetPlayers(PlayerIndex).FPlayerRace;
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
            if (Combat.FCombatData.FPlayers.GetPlayers(Combat.FCombatData.FAttackerPlayer).FPlayer == 0)
            {
                var independents = TranslateRStr(E3Globals.E3Globals.IndependentsRStr);
                Dialog.LSideLbl.Text = independents;
            }
            else
            {
                var player = Combat.FCombatData.FPlayers.GetPlayers(Combat.FCombatData.FAttackerPlayer);
                RaceIndex = AoWHSMap.FPlayers.GetPlayers(player.FPlayer).FPlayerRace;
                var raceResource = AoWHSSet.FRaceResources.GetRaceResource(RaceIndex);
                Dialog.LSideLbl.Text = raceResource.FRaceName;
            }

            if (Combat.FCombatData.FPlayers.GetPlayers(Combat.FCombatData.FDefenderPlayer).FPlayer == 0)
            {
                var independents = TranslateRStr(E3Globals.E3Globals.IndependentsRStr);
                Dialog.RSideLbl.Text = independents;
            }
            else
            {
                var player = Combat.FCombatData.FPlayers.GetPlayers(Combat.FCombatData.FDefenderPlayer);
                RaceIndex = AoWHSMap.FPlayers.GetPlayers(player.FPlayer).FPlayerRace;
                var raceResource = AoWHSSet.FRaceResources.GetRaceResource(RaceIndex);
                Dialog.RSideLbl.Text = raceResource.FRaceName;
            }
        }

        Dialog.FViewer.Play();
        Dialog.FCWin.FDrawn = false;
        var endPosition = Dialog.FViewer.GetEndPosition();
        Dialog.SequencePositionSld.Max = endPosition;
        Dialog.SequenceSpeedSld.Position = 20 - Dialog.FViewer.FDelay;
        if (Dialog.FViewer.GetEndPosition() != -1)
        {
            Dialog.SetPhase(0);
        }
        else
        {
            Dialog.SetPhase(3);
        }
    }
}
