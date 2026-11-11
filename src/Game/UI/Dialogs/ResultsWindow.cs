// Translated from AoWResultsWin.pas.
using Aow.Engine.Collections;
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// player indices, checkbox controls
public partial class ResultsWindow : DataModule
{
    public ResultsWindow(Component owner) : base(owner)
    { }

    public CheckBox P1CB;
    public CheckBox P2CB;
    public CheckBox P3CB;
    public CheckBox P4CB;
    public CheckBox P5CB;
    public CheckBox P6CB;
    public CheckBox P7CB;
    public CheckBox P8CB;
    public CheckBox P9CB;
    public CheckBox P10CB;
    public CheckBox P11CB;
    public CheckBox P12CB;
    public Label StrLbl;
    public Label TurnLbl;
    public Memo ResNames;
    public Memo ResValues;
    public Label SideLbl;
    public Image Axis;
    public Button ExitBtn;
    public Image FaceGrid;
    public Button LoadBtn;
    public Button ContinueBtn;
    public Panel ResultsPnl;
    public CheckBox StrengthCB;
    public Panel AgendaPnl;
    public Label StrengthLbl;
    public CheckBox GoldCB;
    public Label GoldLbl;
    public CheckBox ArmiesCB;
    public Label ArmiesLbl;
    public CheckBox HeroesCB;
    public Label HeroesLbl;
    public CheckBox StructuresCB;
    public Label StructuresLbl;
    public Label KeyMax;
    public Label KeyMin;
    public CheckBox ManaCB;
    public Label ManaLbl;
    public IvTranslator Translator;
    public int FMaxTurns;
    public int FMaxGold;
    public int FMaxMana;
    public int FMaxArmies;
    public int FReservedF0;
    public int FMaxHeroes;
    public int FMaxStructures;
    public int FMaxStrength;
    public ByteList FPlayers;
    public List<CheckBox> FControls;

    // AoWResultsWin.pas:93
    public void UpdateSelection()
    {
        PlayerStatistics Stats;
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
            Stats = AoWHSMap.Current.FPlayers.GetPlayers(player).FStatistics;
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

        var turn = AbilityRules.TranslateRStr(TurnRStr);
        TurnLbl.Text = turn + " " + IntToStr(FMaxTurns);
    }

    // AoWResultsWin.pas:250
    public void DrawGraphs(in LockedSurface Surface)
    {
        Rect Bounds = default;
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
    public void DrawGraph(LibraryImage Image, Int32List Values, int Turns, int Maximum, in Rect R, in LockedSurface Surface)
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
    public void DrawGold(sbyte Player, in Rect R, in LockedSurface Surface)
    {
        Int32List Values;
        TurnInfo Info;
        Values = new Int32List();
        Info = AoWHSMap.Current.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).FGold);
        }

        var player = AoWHSMap.Current.FPlayers.GetPlayers(Player);
        DrawGraph(DialogHosts.IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxGold, R, Surface);
        Values?.Dispose();
        KeyMax.Text = IntToStr(FMaxGold);
    }

    // AoWResultsWin.pas:175
    public void DrawMana(sbyte Player, in Rect R, in LockedSurface Surface)
    {
        Int32List Values;
        TurnInfo Info;
        Values = new Int32List();
        Info = AoWHSMap.Current.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).FMana);
        }

        var player = AoWHSMap.Current.FPlayers.GetPlayers(Player);
        DrawGraph(DialogHosts.IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxMana, R, Surface);
        Values?.Dispose();
        KeyMax.Text = IntToStr(FMaxMana);
    }

    // AoWResultsWin.pas:190
    public void DrawHeroes(sbyte Player, in Rect R, in LockedSurface Surface)
    {
        Int32List Values;
        TurnInfo Info;
        Values = new Int32List();
        Info = AoWHSMap.Current.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).FHeroes);
        }

        var player = AoWHSMap.Current.FPlayers.GetPlayers(Player);
        DrawGraph(DialogHosts.IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxHeroes, R, Surface);
        Values?.Dispose();
        KeyMax.Text = IntToStr(FMaxHeroes);
    }

    // AoWResultsWin.pas:205
    public void DrawArmies(sbyte Player, in Rect R, in LockedSurface Surface)
    {
        Int32List Values;
        TurnInfo Info;
        Values = new Int32List();
        Info = AoWHSMap.Current.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).FArmyStrength);
        }

        var player = AoWHSMap.Current.FPlayers.GetPlayers(Player);
        DrawGraph(DialogHosts.IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxArmies, R, Surface);
        Values?.Dispose();
        KeyMax.Text = IntToStr(FMaxArmies);
    }

    // AoWResultsWin.pas:220
    public void DrawStructures(sbyte Player, in Rect R, in LockedSurface Surface)
    {
        Int32List Values;
        TurnInfo Info;
        Values = new Int32List();
        Info = AoWHSMap.Current.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).FStructures);
        }

        var player = AoWHSMap.Current.FPlayers.GetPlayers(Player);
        DrawGraph(DialogHosts.IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxStructures, R, Surface);
        Values?.Dispose();
        KeyMax.Text = IntToStr(FMaxStructures);
    }

    // AoWResultsWin.pas:235
    public void DrawStrength(sbyte Player, in Rect R, in LockedSurface Surface)
    {
        Int32List Values;
        TurnInfo Info;
        Values = new Int32List();
        Info = AoWHSMap.Current.FPlayers.GetPlayers(Player).FStatistics.FTurnInfo;
        for (int I = 0, last = Info.GetCount() - 1; I <= last; I++)
        {
            Values.Add(Info.GetTurnInfo(I).GetSuperiority());
        }

        var player = AoWHSMap.Current.FPlayers.GetPlayers(Player);
        DrawGraph(DialogHosts.IntGfxMod.EndScene[player.FPlayerRace + 13], Values, FMaxTurns, FMaxStrength, R, Surface);
        Values?.Dispose();
        // { Original strength graph labels the structures maximum. }
        KeyMax.Text = IntToStr(FMaxStructures);
    }

    // AoWResultsWin.pas:270
    public void ResultsWinCreate()
    {
        FPlayers = new ByteList();
        FControls = new List<CheckBox>();
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
        FControls?.Clear();
    }

    // AoWResultsWin.pas:294
    public void ContinueBtnClick()
    {
        FinishPyre(DialogHosts.PyreScreen);
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.ResultsWin.ResultsPnl.Visible = false;
        AudioPlayback.FadeMusicVolume(DialogHosts.SoundDlg.MusicVolume.Position, 2);
        if (AoWHSMap.Current.FGameStatus == AoWHSMap.GameStatus.Terminated)
        {
            if (GameTables.AoWEngine.FCampaign == null)
            {
                DialogHosts.General.ReturnToTitle();
            }
            else
            {
                WorldMap.ShowCampaignMap(DialogHosts.WorldMap);
            }
        }
        else
        {
            DialogHosts.TitleWindow.TitleWin.Visible = false;
            VideoDialog.SetVideoMode(DialogHosts.VideoDialog, VideoDialog.ScreenMode.Game);
            DialogHosts.VideoDialog.ApplyMode();
        }
    }

    // AoWResultsWin.pas:313
    public void ExitBtnClick()
    {
        ResultsPnl.Visible = false;
        DialogHosts.General.ReturnToTitle();
    }

    // AoWResultsWin.pas:319
    public void ResultsPnlUpdate()
    {
        DialogHosts.PyreScreen.DeadPnlUpdate();
    }

    // AoWResultsWin.pas:324
    public void AxisDraw(LockedSurface Surface)
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
    public void P1CBPreDraw(CheckBox Sender, LockedSurface Surface, int Slot)
    {
        Rect R;
        var component = Sender;
        if (Slot < FPlayers.Count)
        {
            R = new Rect(component.WinLeft + ResultsPnl.WinLeft + 1, component.WinTop + ResultsPnl.WinTop + 1, component.WinLeft + component.WinWidth + ResultsPnl.WinLeft - 1, component.WinTop + component.WinHeight + ResultsPnl.WinTop - 1);
            var player = FPlayers[Slot];
            if (AoWHSMap.Current.FPlayers.GetPlayers(player).GetFace() != null)
            {
                var player2 = FPlayers[Slot];
                AoWHSMap.Current.FPlayers.GetPlayers(player2).FLeader.ShowFace(R, Surface);
            }

            var player3 = FPlayers[Slot];
            var playerPlayerRace = AoWHSMap.Current.FPlayers.GetPlayers(player3);
            DialogHosts.IntGfxMod.RelWin[playerPlayerRace.FPlayerRace + 16].ShowClipped(component.WinLeft + ResultsPnl.WinLeft, component.WinTop + ResultsPnl.WinTop, Surface);
        }
    }

    // AoWResultsWin.pas:355
    public void P1CBChange(CheckBox Sender, int Slot)
    {
        CheckBox Control;
        Control = Sender;
        if (Control.Checked)
        {
            if (FPlayers.Count > Slot)
            {
                var player = FPlayers[Slot];
                var nameEx = AoWHSMap.Current.FPlayers.GetPlayers(player).GetNameEx();
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

    private static void FinishPyre(PyreScreen Dialog)
    {
        AudioPlayback.SetSFXVolume(DialogHosts.SoundDlg.SFXVolume.Position);
        Dialog.ClearFire();
        if (Dialog.FSavedSongStatus != null)
        {
            SongStatus.RestoreSongStatus(Dialog.FSavedSongStatus);
            Dialog.FSavedSongStatus = null;
        }
        Dialog.FGameOverEvent.Handled();
        DialogHosts.General.Manager.OverlayPriority = 0;
        ExecuteDoneEvent fDone = Dialog.FDone;
        GameOverEventLog fGameOverEvent = Dialog.FGameOverEvent;
        Dialog.FDone = null;
        Dialog.FGameOverEvent = null;
        fDone?.Invoke(fGameOverEvent);
        fGameOverEvent?.Release();
    }

    private static void DrawImageLine(LibraryImage Image, int X1, int Y1, int X2, int Y2, int Spacing, in LockedSurface SD)
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
