// Translated from AoWResultsWin.pas.
using Aow.Game.Draw;
using Aow.Game.Engine.Collections;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
// Translated from AoWResultsWin.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

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
            Stats = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(player).FStatistics;
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

        var turn = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TResultsWin.TurnRStr);
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
    public void DrawGraph(Aow.Game.Draw.Images.TLibraryImage Image, TIntegerList Values, int Turns, int Maximum, in TRect R, in TSurfaceDesc Surface)
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
                Aow.Game.Ui.Dialogs.TResultsWin.DrawImageLine(Image, X1 + R.Left, R.Bottom - Y1, R.Left + X2, R.Bottom - Y2, 1, Surface);
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
                Aow.Game.Ui.Dialogs.TResultsWin.DrawImageLine(Image, X1 + R.Left, R.Bottom - Y1, R.Left + X2, R.Bottom - Y2, 1, Surface);
            }
        }
    }

    // AoWResultsWin.pas:160
    public void DrawGold(sbyte Player, in TRect R, in TSurfaceDesc Surface)
    {
        TIntegerList Values;
        TTurnInfo Info;
        Values = new TIntegerList();
        Info = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).FGold);
        }

        var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player);
        DrawGraph(Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxGold, R, Surface);
        Values?.Dispose();
        KeyMax.Text = IntToStr(FMaxGold);
    }

    // AoWResultsWin.pas:175
    public void DrawMana(sbyte Player, in TRect R, in TSurfaceDesc Surface)
    {
        TIntegerList Values;
        TTurnInfo Info;
        Values = new TIntegerList();
        Info = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).FMana);
        }

        var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player);
        DrawGraph(Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxMana, R, Surface);
        Values?.Dispose();
        KeyMax.Text = IntToStr(FMaxMana);
    }

    // AoWResultsWin.pas:190
    public void DrawHeroes(sbyte Player, in TRect R, in TSurfaceDesc Surface)
    {
        TIntegerList Values;
        TTurnInfo Info;
        Values = new TIntegerList();
        Info = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).FHeroes);
        }

        var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player);
        DrawGraph(Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxHeroes, R, Surface);
        Values?.Dispose();
        KeyMax.Text = IntToStr(FMaxHeroes);
    }

    // AoWResultsWin.pas:205
    public void DrawArmies(sbyte Player, in TRect R, in TSurfaceDesc Surface)
    {
        TIntegerList Values;
        TTurnInfo Info;
        Values = new TIntegerList();
        Info = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).FArmyStrength);
        }

        var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player);
        DrawGraph(Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxArmies, R, Surface);
        Values?.Dispose();
        KeyMax.Text = IntToStr(FMaxArmies);
    }

    // AoWResultsWin.pas:220
    public void DrawStructures(sbyte Player, in TRect R, in TSurfaceDesc Surface)
    {
        TIntegerList Values;
        TTurnInfo Info;
        Values = new TIntegerList();
        Info = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).FStructures);
        }

        var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player);
        DrawGraph(Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxStructures, R, Surface);
        Values?.Dispose();
        KeyMax.Text = IntToStr(FMaxStructures);
    }

    // AoWResultsWin.pas:235
    public void DrawStrength(sbyte Player, in TRect R, in TSurfaceDesc Surface)
    {
        TIntegerList Values;
        TTurnInfo Info;
        Values = new TIntegerList();
        Info = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).GetSuperiority());
        }

        var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player);
        DrawGraph(Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxStrength, R, Surface);
        Values?.Dispose();
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
        FPlayers?.Dispose();
        FControls?.Dispose();
    }

    // AoWResultsWin.pas:294
    public void ContinueBtnClick()
    {
        Aow.Game.Ui.Dialogs.TResultsWin.FinishPyre(Aow.Game.Ui.Dialogs.DialogHosts.PyreScreen);
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.Ui.Dialogs.DialogHosts.ResultsWin.ResultsPnl.Visible = false;
        Aow.Game.Audio.AudioPlayback.FadeMusicVolume(Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.MusicVolume.Position, 2);
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FGameStatus == TGameStatus.gsTerminated)
        {
            if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign == null)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.General.ReturnToTitle();
            }
            else
            {
                Aow.Game.Ui.Dialogs.TWorldMap.ShowCampaignMap(Aow.Game.Ui.Dialogs.DialogHosts.WorldMap);
            }
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.Visible = false;
            Aow.Game.Ui.Dialogs.TVideoDialog.SetVideoMode(Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog, TScreenMode.scmGame);
            Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.ApplyMode();
        }
    }

    // AoWResultsWin.pas:313
    public void ExitBtnClick()
    {
        ResultsPnl.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.General.ReturnToTitle();
    }

    // AoWResultsWin.pas:319
    public void ResultsPnlUpdate()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.PyreScreen.DeadPnlUpdate();
    }

    // AoWResultsWin.pas:324
    public void AxisDraw(TSurfaceDesc Surface)
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
    public void P1CBPreDraw(TAOWCheckBox Sender, TSurfaceDesc Surface, int Slot)
    {
        TRect R;
        var component = Sender;
        if (Slot < FPlayers.Count)
        {
            R = Rect(component.WinLeft + ResultsPnl.WinLeft + 1, component.WinTop + ResultsPnl.WinTop + 1, component.WinLeft + component.WinWidth + ResultsPnl.WinLeft - 1, component.WinTop + component.WinHeight + ResultsPnl.WinTop - 1);
            var player = FPlayers[Slot];
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(player).GetFace() != null)
            {
                var player2 = FPlayers[Slot];
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(player2).FLeader.ShowFace(R, Surface);
            }

            var player3 = FPlayers[Slot];
            var playerPlayerRace = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(player3);
            Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.RelWin[playerPlayerRace.FPlayerRace + 16].ShowClipped(component.WinLeft + ResultsPnl.WinLeft, component.WinTop + ResultsPnl.WinTop, Surface);
        }
    }

    // AoWResultsWin.pas:355
    public void P1CBChange(TAOWCheckBox Sender, int Slot)
    {
        TAOWCheckBox Control;
        Control = Sender;
        if (Control.Checked)
        {
            if (FPlayers.Count > Slot)
            {
                var player = FPlayers[Slot];
                var nameEx = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(player).GetNameEx();
                SideLbl.Text = nameEx;
            }

            ResValues.Items.Clear();
            ResValues.Items.Add("0%");
            ResValues.Items.Add("0%");
            ResValues.Items.Add("0%");
            ResValues.Items.Add("0%");
        }
    }

    private static readonly string TurnRStr = "Turn";

    private static void FinishPyre(global::Aow.Game.Ui.Dialogs.TPyreScreen Dialog)
    {
        global::Aow.Game.Audio.AudioPlayback.SetSFXVolume(global::Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.SFXVolume.Position);
        Dialog.ClearFire();
        if (Dialog.FSavedSongStatus != null)
        {
            global::Aow.Game.Audio.TSongStatus.RestoreSongStatus(Dialog.FSavedSongStatus);
            Dialog.FSavedSongStatus = null;
        }
        Dialog.FGameOverEvent.Handled();
        global::Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 0;
        global::Aow.Game.Events.TExecuteDoneEvent fDone = Dialog.FDone;
        global::Aow.Game.Events.TGameOverEventLog fGameOverEvent = Dialog.FGameOverEvent;
        Dialog.FDone = null;
        Dialog.FGameOverEvent = null;
        fDone?.Invoke(fGameOverEvent);
        fGameOverEvent?.Release();
    }

    private static void DrawImageLine(global::Aow.Game.Draw.Images.TLibraryImage Image, int X1, int Y1, int X2, int Y2, int Spacing, in global::Aow.Game.Draw.TSurfaceDesc SD)
    {
        int num = 0;
        int i;
        int j;
        if (X1 > X2)
        {
            i = X2;
            X2 = X1;
            X1 = i;
            j = Y2;
            Y2 = Y1;
            Y1 = j;
        }
        else
        {
            i = X1;
            j = Y1;
        }
        int num2;
        int num3;
        int num4;
        int num7;
        int num6;
        if (Y1 <= Y2)
        {
            num2 = X2 - X1;
            num3 = Y2 - Y1;
            if (num3 >= num2)
            {
                num4 = (num2 - num3) * 2;
                int num5 = num2 * 2;
                num6 = num5 - num3;
                for (; j != Y2; j++)
                {
                    if (num == 0)
                    {
                        Image.Show(i, j, in SD);
                        num = Spacing;
                    }
                    else
                    {
                        num--;
                    }
                    if (num6 >= 0)
                    {
                        num6 += num4;
                        i++;
                    }
                    else
                    {
                        num6 += num5;
                    }
                }
                if (num == 0)
                {
                    Image.Show(i, j, in SD);
                }
                return;
            }
            num4 = (num3 - num2) * 2;
            num7 = num3 * 2;
            num6 = num7 - num2;
            for (; i != X2; i++)
            {
                if (num == 0)
                {
                    Image.Show(i, j, in SD);
                    num = Spacing;
                }
                else
                {
                    num--;
                }
                if (num6 >= 0)
                {
                    num6 += num4;
                    j++;
                }
                else
                {
                    num6 += num7;
                }
            }
            if (num == 0)
            {
                Image.Show(i, j, in SD);
            }
            return;
        }
        num2 = X2 - X1;
        num3 = Y1 - Y2;
        if (num3 >= num2)
        {
            num4 = (num2 - num3) * 2;
            int num5 = num2 * 2;
            num6 = num5 - num3;
            while (j != Y2)
            {
                if (num == 0)
                {
                    Image.Show(i, j, in SD);
                    num = Spacing;
                }
                else
                {
                    num--;
                }
                if (num6 >= 0)
                {
                    num6 += num4;
                    i++;
                }
                else
                {
                    num6 += num5;
                }
                j--;
            }
            if (num == 0)
            {
                Image.Show(i, j, in SD);
            }
            return;
        }
        num4 = (num3 - num2) * 2;
        num7 = num3 * 2;
        num6 = num7 - num2;
        for (; i != X2; i++)
        {
            if (num == 0)
            {
                Image.Show(i, j, in SD);
                num = Spacing;
            }
            else
            {
                num--;
            }
            if (num6 >= 0)
            {
                num6 += num4;
                j--;
            }
            else
            {
                num6 += num7;
            }
        }
        if (num == 0)
        {
            Image.Show(i, j, in SD);
        }
    }
}
