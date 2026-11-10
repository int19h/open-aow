// Translated from FastCombatWin.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
// Translated from FastCombatWin.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// phase, phase frame, animation frame, and shield indices/opacities, combat viewer, completion
// event, retained event
public partial class FastCombatWindow : DataModule
{
    public FastCombatWindow(Component owner) : base(owner)
    { }

    public AOWWindow FCWin;
    public AOWPanel LNamePnl;
    public AOWLabel LSideLbl;
    public AOWPanel RNamePnl;
    public AOWLabel RSideLbl;
    public AOWPanel LCtrlPnl;
    public AOWPanel RCtrlPnl;
    public AOWCheckBox PlayBtn2;
    public AOWCheckBox PauseBtn2;
    public AOWCheckBox StartBtn;
    public AOWCheckBox EndBtn;
    public AOWButton CloseBut;
    public AOWPanel Party1Pnl;
    public AOWImage Party1Bg;
    public AOWImage Part1Dead;
    public AOWPanel Party2Pnl;
    public AOWImage Party2Bg;
    public AOWImage Party2Dead;
    public AOWPanel Party3Pnl;
    public AOWImage Party3Bg;
    public AOWImage Party3Dead;
    public AOWPanel Party4Pnl;
    public AOWImage Party4Bg;
    public AOWImage Party4Dead;
    public AOWPanel Party5Pnl;
    public AOWImage Party5Bg;
    public AOWImage Party5Dead;
    public AOWPanel Party6Pnl;
    public AOWImage Party6Bg;
    public AOWImage Party6Dead;
    public AOWPanel Party7Pnl;
    public AOWImage Party7Bg;
    public AOWImage Party7Dead;
    public AOWPanel LSidePnl;
    public AOWPanel RSidePnl;
    public AOWPanel FightPnl;
    public AOWImage LShield;
    public AOWImage RShield;
    public AOWButton InitialView;
    public AOWButton FinalView;
    public AOWButton PlayBtn;
    public AOWButton PauseBtn;
    public AOWHScrollBar SequencePositionSld;
    public AOWHScrollBar SequenceSpeedSld;
    public ImageLibrary GlowAnimation;
    public AOWImage GlowImage;
    public AOWButton PrevSeqBtn;
    public AOWButton NextSeqBtn;
    public AOWLabel StartLBl;
    public AOWLabel FastLbl;
    public AOWLabel SlowLbl;
    public AOWLabel EndLbl;
    public AOWLabel ModeLbl;
    public TaowUserDrawBox LShieldUser;
    public TaowUserDrawBox RShieldUser;
    public IvTranslator Translator;
    public byte FPhase;
    public int FPhaseFrame;
    public int FAnimationFrame;
    public CombatLogbook FLogbook;
    public int FLeftShield;
    public byte FLeftOpacity;
    public int FRightShield;
    public byte FRightOpacity;
    public CombatViewer FViewer;
    public ExecuteDoneEvent FDone;
    public EventLog FEvent;

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
    public void DrawParties(int X, int Y, in SurfaceDesc Surface)
    {
        int PX = default;
        int PY = default;
        int Position;
        Combat.Battle Combat;
        CombatObject Obj;
        if (FViewer.FStatus.Contains(CombatViewerStatusElement.Initialized))
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
    public void DrawWalls(int X, int Y, in SurfaceDesc Surface)
    {
        int Image;
        CombatWall Wall;
        bool HasWall(int A, int B)
        {
            CombatParty PA;
            CombatParty PB;
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
            AoWHSSet.Current.FCombatImages[Image].Show(X + 180, Y + 218 - 50, Surface);
        }

        if (HasWall(1, 2))
        {
            AoWHSSet.Current.FCombatImages[Image].Show(X + 372, Y + 218 - 50, Surface);
        }

        if (HasWall(0, 1))
        {
            AoWHSSet.Current.FCombatImages[Image + 4].Show(X + 180, Y + 218, Surface);
        }

        if (HasWall(0, 6))
        {
            AoWHSSet.Current.FCombatImages[Image + 5].Show(X + 180, Y + 218, Surface);
        }

        if (HasWall(0, 2))
        {
            AoWHSSet.Current.FCombatImages[Image + 5].Show(X + 372, Y + 218, Surface);
        }

        if (HasWall(5, 6))
        {
            AoWHSSet.Current.FCombatImages[Image + 3].Show(X + 0, Y + 275, Surface);
        }

        if (HasWall(2, 3))
        {
            AoWHSSet.Current.FCombatImages[Image + 2].Show(X + 374, Y + 275, Surface);
        }

        if (HasWall(0, 5))
        {
            AoWHSSet.Current.FCombatImages[Image + 5].Show(X + 180, Y + 275, Surface);
        }

        if (HasWall(0, 3))
        {
            AoWHSSet.Current.FCombatImages[Image + 5].Show(X + 372, Y + 275, Surface);
        }

        if (HasWall(0, 4))
        {
            AoWHSSet.Current.FCombatImages[Image + 4].Show(X + 180, Y + 332, Surface);
        }

        if (HasWall(4, 5))
        {
            AoWHSSet.Current.FCombatImages[Image + 1].Show(X + 180, Y + 342, Surface);
        }

        if (HasWall(4, 3))
        {
            AoWHSSet.Current.FCombatImages[Image + 1].Show(X + 372, Y + 342, Surface);
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
                    var hitPointsToHitPcnt = HitChance.HitPointsToHitPcnt(Wall.GetHitPoints(), hits);
                    HitChance.ShowHitPcnt(X + 250, Y + 332 - 45, Surface, hitPointsToHitPcnt);
                }
                else
                {
                    var hits = Wall.GetHits();
                    var hitPointsToHitPcnt = HitChance.HitPointsToHitPcnt(Wall.GetHitPoints(), hits);
                    HitChance.ShowHitPcnt(X + 250, Y + 218 - 45, Surface, hitPointsToHitPcnt);
                }
            }
        }
    }

    // FastCombatWin.pas:316
    public void DrawCombat(int X, int Y, ref SurfaceDesc Surface)
    {
        Combat.Battle Combat;
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
                        if (!Set<byte>.WithBounds(0, 255).With(2).With(7).With(8).Contains(FViewer.FLogbook.FFinalData.F_CombatData_30))
                        {
                            Glow = FPhaseFrame / 2;
                            Glow = Math.Abs((Glow + 7) % 15 - 7);
                            GetPartyPosition(I, ref PX, ref PY);
                            AoWHSSet.Current.FCombatImages[Glow + 100].ShowClipped(PX + X - 10, PY + Y - 9, Surface);
                        }
                    }
                }
            }
        }
    }

    // FastCombatWin.pas:340
    public void UpdateShields()
    {
        CombatData Data;
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
            Left = AoWHSMap.Current.FPlayers.GetPlayers(player.FPlayer).FPlayerRace;
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
            Right = AoWHSMap.Current.FPlayers.GetPlayers(player.FPlayer).FPlayerRace;
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
        ExecuteDoneEvent Done;
        EventLog Log;
        FViewer.Finalize();
        DialogHosts.General.Manager.OverlayPriority = 0;
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
        FViewer = new CombatViewer();
    }

    // FastCombatWin.pas:394
    public void FastCombatWindowDestroy()
    {
        FViewer?.Dispose();
    }

    // FastCombatWin.pas:399
    public void CloseButClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FCWin.Visible = false;
    }

    // FastCombatWin.pas:405
    public void FCWinDrawSurface(Surface Surface, int Left, int Top)
    {
        SurfaceDesc SD = default;
        try
        {
            if (!FViewer.FStatus.Contains(CombatViewerStatusElement.Paused))
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
            throw new InvalidOperationException("Exception during FastCombatWin.DrawSurface");
        }
    }

    // FastCombatWin.pas:424
    public void FCWinUpdate()
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
            throw new InvalidOperationException("Exception during FastCombatWin.Update");
        }
    }

    // FastCombatWin.pas:466
    public void InitialViewClick()
    {
        CombatViewer Viewer;
        DialogHosts.General.PlayButtonSound(0);
        Viewer = FViewer;
        if (Viewer != null && Viewer.FStatus.Contains(CombatViewerStatusElement.Initialized))
        {
            Viewer.SetPosition(0);
            SetPhase(1);
        }
    }

    // FastCombatWin.pas:479
    public void FinalViewClick()
    {
        CombatViewer Viewer;
        DialogHosts.General.PlayButtonSound(0);
        Viewer = FViewer;
        if (Viewer != null && Viewer.FStatus.Contains(CombatViewerStatusElement.Initialized))
        {
            Viewer.SetPosition(Viewer.GetEndPosition() + 1);
            SetPhase(3);
        }
    }

    // FastCombatWin.pas:492
    public void PauseBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (FViewer != null && FViewer.FStatus.Contains(CombatViewerStatusElement.Initialized))
        {
            FViewer.Pause();
        }
    }

    // FastCombatWin.pas:498
    public void PlayBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (FViewer != null && FViewer.FStatus.Contains(CombatViewerStatusElement.Initialized))
        {
            FViewer.Play();
        }
    }

    // FastCombatWin.pas:504
    public void SequencePositionSldChange()
    {
        CombatViewer Viewer;
        Viewer = FViewer;
        if (Viewer != null && Viewer.FStatus.Contains(CombatViewerStatusElement.Initialized))
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
    public void LShieldUserDraw(AoWComponent Sender, SurfaceDesc Surface)
    {
        if (FLeftOpacity == 100)
        {
            DialogHosts.IntGfxMod.Shields[FLeftShield].Show(FCWin.WinLeft + Sender.WinLeft, FCWin.WinTop + Sender.WinTop, Surface);
        }
        else
        {
            DialogHosts.IntGfxMod.Shields[FLeftShield].ShowBlended(FCWin.WinLeft + Sender.WinLeft, FCWin.WinTop + Sender.WinTop, Surface, GfxState.AlphaBlendTable.LookupTables[FLeftOpacity]);
        }
    }

    // FastCombatWin.pas:530
    public void RShieldUserDraw(AoWComponent Sender, SurfaceDesc Surface)
    {
        if (FRightOpacity == 100)
        {
            DialogHosts.IntGfxMod.Shields[FRightShield].Show(FCWin.WinLeft + Sender.WinLeft, FCWin.WinTop + Sender.WinTop, Surface);
        }
        else
        {
            DialogHosts.IntGfxMod.Shields[FRightShield].ShowBlended(FCWin.WinLeft + Sender.WinLeft, FCWin.WinTop + Sender.WinTop, Surface, GfxState.AlphaBlendTable.LookupTables[FRightOpacity]);
        }
    }

    // FastCombatWin.pas:539
    public void FCWinHide()
    {
        FinishDialog();
    }

    // FastCombatWin.pas:544
    public void FCWinKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27 || Key == 32)
        {
            FCWin.Visible = false;
        }
    }

    internal static void ShowCombatEvent(FastCombatWindow Dialog, CombatEventLog Event, ExecuteDoneEvent Done)
    {
        Assert(Dialog.FEvent == null, "", "FastCombatWin.pas", 159);
        DialogHosts.General.Manager.OverlayPriority = 49;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Dialog.FDone = Done;
        Dialog.FLogbook = Event.FLogbook;
        InitializeCombatDialog(Dialog, Dialog.FLogbook);
        Dialog.FCWin.Visible = true;
    }

    internal static void InitializeCombatDialog(FastCombatWindow Dialog, CombatLogbook Logbook)
    {
        int RaceIndex;
        int PartyRace;
        sbyte PlayerIndex;
        Combat.Battle Combat;
        CombatParty Party;
        AOWImage Image;
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
                    PartyRace = AoWHSMap.Current.FPlayers.GetPlayers(PlayerIndex).FPlayerRace;
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
                var independents = AbilityRules.TranslateRStr(ScenarioText.IndependentsRStr);
                Dialog.LSideLbl.Text = independents;
            }
            else
            {
                var player = Combat.FCombatData.FPlayers.GetPlayers(Combat.FCombatData.FAttackerPlayer);
                RaceIndex = AoWHSMap.Current.FPlayers.GetPlayers(player.FPlayer).FPlayerRace;
                var raceResource = AoWHSSet.Current.FRaceResources.GetRaceResource(RaceIndex);
                Dialog.LSideLbl.Text = raceResource.FRaceName;
            }

            if (Combat.FCombatData.FPlayers.GetPlayers(Combat.FCombatData.FDefenderPlayer).FPlayer == 0)
            {
                var independents = AbilityRules.TranslateRStr(ScenarioText.IndependentsRStr);
                Dialog.RSideLbl.Text = independents;
            }
            else
            {
                var player = Combat.FCombatData.FPlayers.GetPlayers(Combat.FCombatData.FDefenderPlayer);
                RaceIndex = AoWHSMap.Current.FPlayers.GetPlayers(player.FPlayer).FPlayerRace;
                var raceResource = AoWHSSet.Current.FRaceResources.GetRaceResource(RaceIndex);
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
