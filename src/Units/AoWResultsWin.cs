// Translated from AoWResultsWin.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWComp;
using Aow.Units.AoWE;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVideoDlg;
using Aow.Units.AoWWorldMap;
using Aow.Units.E2Globals;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.aowIntGfxMod.aowIntGfxModUnit;
using static Aow.Units.AOWPyreScrn.AOWPyreScrnUnit;
using static Aow.Units.AoWVideoDlg.AoWVideoDlgUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using static Aow.Units.E1GameFlow.E1GameFlowUnit;
using static Aow.Units.ImageLib.ImageLibUnit;
using static Aow.Units.Sound.SoundUnit;

namespace Aow.Units.AoWResultsWin;

// player indices, checkbox controls
public partial class TResultsWin : TDataModule
{
    public TResultsWin(TComponent owner) : base(owner)
    { }

    public TAOWCheckBox P1CB;
    public TAOWCheckBox P2CB;
    public TAOWCheckBox P3CB;
    public TAOWCheckBox P4CB;
    public TAOWCheckBox P5CB;
    public TAOWCheckBox P6CB;
    public TAOWCheckBox P7CB;
    public TAOWCheckBox P8CB;
    public TAOWCheckBox P9CB;
    public TAOWCheckBox P10CB;
    public TAOWCheckBox P11CB;
    public TAOWCheckBox P12CB;
    public TAOWLabel StrLbl;
    public TAOWLabel TurnLbl;
    public TAOWMemo ResNames;
    public TAOWMemo ResValues;
    public TAOWLabel SideLbl;
    public TAOWImage Axis;
    public TAOWButton ExitBtn;
    public TAOWImage FaceGrid;
    public TAOWButton LoadBtn;
    public TAOWButton ContinueBtn;
    public TAOWPanel ResultsPnl;
    public TAOWCheckBox StrengthCB;
    public TAOWPanel AgendaPnl;
    public TAOWLabel StrengthLbl;
    public TAOWCheckBox GoldCB;
    public TAOWLabel GoldLbl;
    public TAOWCheckBox ArmiesCB;
    public TAOWLabel ArmiesLbl;
    public TAOWCheckBox HeroesCB;
    public TAOWLabel HeroesLbl;
    public TAOWCheckBox StructuresCB;
    public TAOWLabel StructuresLbl;
    public TAOWLabel KeyMax;
    public TAOWLabel KeyMin;
    public TAOWCheckBox ManaCB;
    public TAOWLabel ManaLbl;
    public TIvTranslator Translator;
    public int FMaxTurns;
    public int FMaxGold;
    public int FMaxMana;
    public int FMaxArmies;
    public int FReservedF0;
    public int FMaxHeroes;
    public int FMaxStructures;
    public int FMaxStrength;
    public TByteList FPlayers;
    public TList FControls;

    // AoWResultsWin.pas:93
    public void UpdateSelection()
    {
        TPlayerStatistics Stats;
        FMaxTurns = 1;
        FMaxGold = 1;
        FMaxMana = 1;
        FMaxArmies = 1;
        FReservedF0 = 1;
        FMaxHeroes = 1;
        FMaxStructures = 1;
        FMaxStrength = 1;
        for (int I = 0, last = FPlayers.Count - 1; I <= last; I++)
        {
            var player = FPlayers[I];
            Stats = AoWHSMap.FPlayers.GetPlayers(player).FStatistics;
            var count = Stats.FTurnInfo.GetCount();
            if (FMaxTurns < count)
            {
                FMaxTurns = Stats.FTurnInfo.GetCount();
            }

            for (int J = 0, lastJ = Stats.FTurnInfo.GetCount() - 1; J <= lastJ; J++)
            {
                var turnInfo = Stats.FTurnInfo.GetTurnInfo(J);
                if (FMaxArmies < turnInfo.FArmyStrength)
                {
                    FMaxArmies = Stats.FTurnInfo.GetTurnInfo(J).FArmyStrength;
                }

                var fTurnInfoTurnInfo = Stats.FTurnInfo.GetTurnInfo(J);
                if (FMaxGold < fTurnInfoTurnInfo.FGold)
                {
                    FMaxGold = Stats.FTurnInfo.GetTurnInfo(J).FGold;
                }

                var turnInfo2 = Stats.FTurnInfo.GetTurnInfo(J);
                if (FMaxMana < turnInfo2.FMana)
                {
                    FMaxMana = Stats.FTurnInfo.GetTurnInfo(J).FMana;
                }

                var turnInfo3 = Stats.FTurnInfo.GetTurnInfo(J);
                if (FMaxHeroes < turnInfo3.FHeroes)
                {
                    FMaxHeroes = Stats.FTurnInfo.GetTurnInfo(J).FHeroes;
                }

                var turnInfo4 = Stats.FTurnInfo.GetTurnInfo(J);
                if (FMaxStructures < turnInfo4.FStructures)
                {
                    FMaxStructures = Stats.FTurnInfo.GetTurnInfo(J).FStructures;
                }

                var superiority = Stats.FTurnInfo.GetTurnInfo(J).GetSuperiority();
                if (FMaxStrength < superiority)
                {
                    FMaxStrength = Stats.FTurnInfo.GetTurnInfo(J).GetSuperiority();
                }
            }
        }

        var turn = TranslateRStr(TurnRStr);
        TurnLbl.Text = turn + " " + IntToStr(FMaxTurns);
    }

    // AoWResultsWin.pas:250
    public void DrawGraphs(in TSurfaceDesc Surface)
    {
        TRect Bounds = default;
        Bounds.Left = ResultsPnl.WinLeft + Axis.WinLeft + 5;
        Bounds.Top = ResultsPnl.WinTop + Axis.WinTop + 4;
        Bounds.Right = ResultsPnl.WinLeft + Axis.WinLeft + 338;
        Bounds.Bottom = ResultsPnl.WinTop + Axis.WinTop + 117;
        for (int I = 0, last = FPlayers.Count - 1; I <= last; I++)
        {
            if (GoldCB.Checked)
            {
                DrawGold((sbyte)FPlayers[I], Bounds, Surface);
            }

            if (ManaCB.Checked)
            {
                DrawMana((sbyte)FPlayers[I], Bounds, Surface);
            }

            if (HeroesCB.Checked)
            {
                DrawHeroes((sbyte)FPlayers[I], Bounds, Surface);
            }

            if (StructuresCB.Checked)
            {
                DrawStructures((sbyte)FPlayers[I], Bounds, Surface);
            }

            if (ArmiesCB.Checked)
            {
                DrawArmies((sbyte)FPlayers[I], Bounds, Surface);
            }

            if (StrengthCB.Checked)
            {
                DrawStrength((sbyte)FPlayers[I], Bounds, Surface);
            }
        }
    }

    // AoWResultsWin.pas:123
    public void DrawGraph(ImageLib.TLibraryImage Image, TIntegerList Values, int Turns, int Maximum, in TRect R, in TSurfaceDesc Surface)
    {
        int Width;
        int LastY;
        int Y1;
        int X2;
        float XStep;
        float YStep;
        int NextY;
        int X1;
        int Y2;
        int Height;
        if (Values.Count != 0)
        {
            Width = R.Right - R.Left;
            Height = R.Bottom - R.Top;
            if (Turns >= 2)
            {
                XStep = (float)((double)Width / (Turns - 1));
            }
            else
            {
                XStep = Width;
            }

            YStep = (float)((double)Height / Maximum);
            LastY = Values[0];
            for (int I = 1, last = Values.Count - 1; I <= last; I++)
            {
                NextY = Values[I];
                X1 = Round((double)(I - 1) * XStep);
                Y1 = Round((double)LastY * YStep);
                // { Original advances from the rounded previous x coordinate. }
                X2 = Round((double)X1 + XStep);
                Y2 = Round((double)NextY * YStep);
                DrawImageLine(Image, X1 + R.Left, R.Bottom - Y1, R.Left + X2, R.Bottom - Y2, 1, Surface);
                LastY = NextY;
            }

            if (Values.Count == 1)
            {
                X1 = 0;
                NextY = Round((double)LastY * YStep);
                Y1 = NextY;
                X2 = Width - 1;
                // { Original repeats the rounding and discards its result. }
                Round((double)LastY * YStep);
                Y2 = NextY;
                DrawImageLine(Image, X1 + R.Left, R.Bottom - Y1, R.Left + X2, R.Bottom - Y2, 1, Surface);
            }
        }
    }

    // AoWResultsWin.pas:160
    public void DrawGold(sbyte Player, in TRect R, in TSurfaceDesc Surface)
    {
        TIntegerList Values;
        TTurnInfo Info;
        Values = new TIntegerList();
        Info = AoWHSMap.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).FGold);
        }

        var player = AoWHSMap.FPlayers.GetPlayers(Player);
        DrawGraph(IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxGold, R, Surface);
        Values.Free();
        KeyMax.Text = IntToStr(FMaxGold);
    }

    // AoWResultsWin.pas:175
    public void DrawMana(sbyte Player, in TRect R, in TSurfaceDesc Surface)
    {
        TIntegerList Values;
        TTurnInfo Info;
        Values = new TIntegerList();
        Info = AoWHSMap.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).FMana);
        }

        var player = AoWHSMap.FPlayers.GetPlayers(Player);
        DrawGraph(IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxMana, R, Surface);
        Values.Free();
        KeyMax.Text = IntToStr(FMaxMana);
    }

    // AoWResultsWin.pas:190
    public void DrawHeroes(sbyte Player, in TRect R, in TSurfaceDesc Surface)
    {
        TIntegerList Values;
        TTurnInfo Info;
        Values = new TIntegerList();
        Info = AoWHSMap.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).FHeroes);
        }

        var player = AoWHSMap.FPlayers.GetPlayers(Player);
        DrawGraph(IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxHeroes, R, Surface);
        Values.Free();
        KeyMax.Text = IntToStr(FMaxHeroes);
    }

    // AoWResultsWin.pas:205
    public void DrawArmies(sbyte Player, in TRect R, in TSurfaceDesc Surface)
    {
        TIntegerList Values;
        TTurnInfo Info;
        Values = new TIntegerList();
        Info = AoWHSMap.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).FArmyStrength);
        }

        var player = AoWHSMap.FPlayers.GetPlayers(Player);
        DrawGraph(IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxArmies, R, Surface);
        Values.Free();
        KeyMax.Text = IntToStr(FMaxArmies);
    }

    // AoWResultsWin.pas:220
    public void DrawStructures(sbyte Player, in TRect R, in TSurfaceDesc Surface)
    {
        TIntegerList Values;
        TTurnInfo Info;
        Values = new TIntegerList();
        Info = AoWHSMap.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).FStructures);
        }

        var player = AoWHSMap.FPlayers.GetPlayers(Player);
        DrawGraph(IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxStructures, R, Surface);
        Values.Free();
        KeyMax.Text = IntToStr(FMaxStructures);
    }

    // AoWResultsWin.pas:235
    public void DrawStrength(sbyte Player, in TRect R, in TSurfaceDesc Surface)
    {
        TIntegerList Values;
        TTurnInfo Info;
        Values = new TIntegerList();
        Info = AoWHSMap.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).GetSuperiority());
        }

        var player = AoWHSMap.FPlayers.GetPlayers(Player);
        DrawGraph(IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxStrength, R, Surface);
        Values.Free();
        // { Original strength graph labels the structures maximum. }
        KeyMax.Text = IntToStr(FMaxStructures);
    }

    // AoWResultsWin.pas:270
    public void ResultsWinCreate()
    {
        FPlayers = new TByteList();
        FControls = new TList();
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
    public void ResultsWinDestroy()
    {
        FPlayers.Free();
        FControls.Free();
    }

    // AoWResultsWin.pas:294
    public void ContinueBtnClick()
    {
        FinishPyre(PyreScreen);
        General.PlayButtonSound(0);
        AoWResultsWinUnit.ResultsWin.ResultsPnl.Visible = false;
        FadeMusicVolume(SoundDlg.MusicVolume.Position, 2);
        if (AoWHSMap.FGameStatus == TGameStatus.gsTerminated)
        {
            if (AoWEngine.FCampaign == null)
            {
                General.ReturnToTitle();
            }
            else
            {
                ShowCampaignMap(AoWWorldMapUnit.WorldMap);
            }
        }
        else
        {
            E2GlobalsUnit.TitleWindow.TitleWin.Visible = false;
            SetVideoMode(VideoDialog, TScreenMode.scmGame);
            VideoDialog.ApplyMode();
        }
    }

    // AoWResultsWin.pas:313
    public void ExitBtnClick()
    {
        ResultsPnl.Visible = false;
        General.ReturnToTitle();
    }

    // AoWResultsWin.pas:319
    public void ResultsPnlUpdate()
    {
        PyreScreen.DeadPnlUpdate();
    }

    // AoWResultsWin.pas:324
    public void AxisDraw(TObject Sender, TSurfaceDesc Surface)
    {
        DrawGraphs(Surface);
    }

    // AoWResultsWin.pas:329
    public void AgendaChange()
    {
        ResultsPnl.FDrawn = false;
    }

    // AoWResultsWin.pas:334
    public void ResultsPnlShow()
    {
        UpdateSelection();
    }

    // AoWResultsWin.pas:339
    public void P1CBPreDraw(TObject Sender, TSurfaceDesc Surface)
    {
        TRect R;
        var component = (TAoWComponent)Sender;
        if (component.Tag < FPlayers.Count)
        {
            R = Rect(component.WinLeft + ResultsPnl.WinLeft + 1, component.WinTop + ResultsPnl.WinTop + 1, component.WinLeft + component.WinWidth + ResultsPnl.WinLeft - 1, component.WinTop + component.WinHeight + ResultsPnl.WinTop - 1);
            var player = FPlayers[component.Tag];
            if (AoWHSMap.FPlayers.GetPlayers(player).GetFace() != null)
            {
                var player2 = FPlayers[component.Tag];
                AoWHSMap.FPlayers.GetPlayers(player2).FLeader.ShowFace(R, Surface);
            }

            var player3 = FPlayers[component.Tag];
            var playerPlayerRace = AoWHSMap.FPlayers.GetPlayers(player3);
            IntGfxMod.RelWin[playerPlayerRace.FPlayerRace + 16].ShowClipped(component.WinLeft + ResultsPnl.WinLeft, component.WinTop + ResultsPnl.WinTop, Surface);
        }
    }

    // AoWResultsWin.pas:355
    public void P1CBChange(TObject Sender)
    {
        TAOWCheckBox Control;
        Control = (TAOWCheckBox)Sender;
        if (Control.Checked)
        {
            if (FPlayers.Count > Control.Tag)
            {
                var player = FPlayers[Control.Tag];
                var nameEx = AoWHSMap.FPlayers.GetPlayers(player).GetNameEx();
                SideLbl.Text = nameEx;
            }

            ResValues.Items.Clear();
            ResValues.Items.Add("0%");
            ResValues.Items.Add("0%");
            ResValues.Items.Add("0%");
            ResValues.Items.Add("0%");
        }
    }
}

public static partial class AoWResultsWinUnit
{
    public static TResultsWin ResultsWin;

    // AoWResultsWin.pas:368
    public static void ShowResults(TResultsWin Window)
    {
        if (AoWEngine.FDemo)
        {
            PlaySongEx("Songs\\Special\\end2.s3m", 2, 2, true);
        }
        else
        {
            PlaySongEx("Songs\\Special\\Sunset.it", 2, 2, true);
        }

        SetMusicVolume(0);
        FadeMusicVolume(SoundDlg.MusicVolume.Position, 1);
        Window.FPlayers.Clear();
        for (int I = 1, last = AoWHSMap.FPlayers.Count - 1; I <= last; I++)
        {
            if (AoWHSMap.FPlayers.GetPlayers(I).FPlayerType != TPlayerType.ptIndependent)
            {
                Window.FPlayers.Add((byte)I);
            }
        }

        for (int I = 0; I <= 11; I++)
        {
            ((TAoWComponent)Window.FControls[I]).Visible = I < Window.FPlayers.Count;
            bool condition = I < Window.FPlayers.Count && Window.FPlayers[I] == AoWHSMap.FSeatedPlayer;
            ((TAOWCheckBox)Window.FControls[I]).Checked = condition;
        }

        if (AoWEngine.FCampaign != null && AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FGameOverStatus == TGameOverStatus.goLost)
        {
            var replay = TranslateRStr(ReplayRStr);
            Window.ContinueBtn.Text = replay;
            Window.ExitBtn.Visible = true;
        }
        else
        {
            var continueText = TranslateRStr(ContinueRStr);
            Window.ContinueBtn.Text = continueText;
            Window.ExitBtn.Visible = false;
        }

        ResultsWin.ResultsPnl.Visible = true;
    }
}
