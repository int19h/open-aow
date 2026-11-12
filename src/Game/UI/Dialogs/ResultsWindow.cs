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
    public Label TurnLbl;
    public Memo ResValues;
    public Label SideLbl;
    public Image Axis;
    public Button ExitBtn;
    public Button ContinueBtn;
    public Panel ResultsPnl;
    public CheckBox StrengthCB;
    public CheckBox GoldCB;
    public CheckBox ArmiesCB;
    public CheckBox HeroesCB;
    public CheckBox StructuresCB;
    public Label KeyMax;
    public CheckBox ManaCB;
    private int _maxTurns;
    private int _maxGold;
    private int _maxMana;
    private int _maxArmies;

    public int UnusedMaximum { get; private set; }

    private int _maxHeroes;
    private int _maxStructures;
    private int _maxStrength;

    public ByteList Players { get; private set; }
    public List<CheckBox> Controls { get; private set; }

    // AoWResultsWin.pas:93
    public void UpdateSelection()
    {
        PlayerStatistics stats;
        _maxTurns = 1;
        _maxGold = 1;
        _maxMana = 1;
        _maxArmies = 1;
        UnusedMaximum = 1;
        _maxHeroes = 1;
        _maxStructures = 1;
        _maxStrength = 1;
        for (int I = 0, last = Players.Count - 1; I <= last; I++)
        {
            var player = Players[I];
            stats = AoWHSMap.Current.Players.GetPlayers(player).Statistics;
            var count = stats.TurnInfo.GetCount();
            if (_maxTurns < count)
            {
                _maxTurns = stats.TurnInfo.GetCount();
            }

            for (int J = 0, lastJ = stats.TurnInfo.GetCount() - 1; J <= lastJ; J++)
            {
                var turnInfo = stats.TurnInfo.GetTurnInfo(J);
                if (_maxArmies < turnInfo.ArmyStrength)
                {
                    _maxArmies = stats.TurnInfo.GetTurnInfo(J).ArmyStrength;
                }

                var fTurnInfoTurnInfo = stats.TurnInfo.GetTurnInfo(J);
                if (_maxGold < fTurnInfoTurnInfo.Gold)
                {
                    _maxGold = stats.TurnInfo.GetTurnInfo(J).Gold;
                }

                var turnInfo2 = stats.TurnInfo.GetTurnInfo(J);
                if (_maxMana < turnInfo2.Mana)
                {
                    _maxMana = stats.TurnInfo.GetTurnInfo(J).Mana;
                }

                var turnInfo3 = stats.TurnInfo.GetTurnInfo(J);
                if (_maxHeroes < turnInfo3.Heroes)
                {
                    _maxHeroes = stats.TurnInfo.GetTurnInfo(J).Heroes;
                }

                var turnInfo4 = stats.TurnInfo.GetTurnInfo(J);
                if (_maxStructures < turnInfo4.Structures)
                {
                    _maxStructures = stats.TurnInfo.GetTurnInfo(J).Structures;
                }

                var superiority = stats.TurnInfo.GetTurnInfo(J).GetSuperiority();
                if (_maxStrength < superiority)
                {
                    _maxStrength = stats.TurnInfo.GetTurnInfo(J).GetSuperiority();
                }
            }
        }

        var turn = AbilityRules.TranslateResourceString(s_turnText);
        TurnLbl.Text = turn + " " + IntToStr(_maxTurns);
    }

    // AoWResultsWin.pas:250
    public void DrawGraphs(in LockedSurface surface)
    {
        Rect bounds = default;
        bounds.Left = ResultsPnl.WinLeft + Axis.WinLeft + 5;
        bounds.Top = ResultsPnl.WinTop + Axis.WinTop + 4;
        bounds.Right = ResultsPnl.WinLeft + Axis.WinLeft + 338;
        bounds.Bottom = ResultsPnl.WinTop + Axis.WinTop + 117;
        for (int I = 0, last = Players.Count - 1; I <= last; I++)
        {
            if (GoldCB.Checked)
            {
                DrawGold((sbyte)Players[I], bounds, surface);
            }

            if (ManaCB.Checked)
            {
                DrawMana((sbyte)Players[I], bounds, surface);
            }

            if (HeroesCB.Checked)
            {
                DrawHeroes((sbyte)Players[I], bounds, surface);
            }

            if (StructuresCB.Checked)
            {
                DrawStructures((sbyte)Players[I], bounds, surface);
            }

            if (ArmiesCB.Checked)
            {
                DrawArmies((sbyte)Players[I], bounds, surface);
            }

            if (StrengthCB.Checked)
            {
                DrawStrength((sbyte)Players[I], bounds, surface);
            }
        }
    }

    // AoWResultsWin.pas:123
    public void DrawGraph(LibraryImage image, Int32List values, int turns, int maximum, in Rect R, in LockedSurface surface)
    {
        int width;
        int lastY;
        int y1;
        int x2;
        float xStep;
        float yStep;
        int nextY;
        int x1;
        int y2;
        int height;
        if (values.Count != 0)
        {
            width = R.Right - R.Left;
            height = R.Bottom - R.Top;
            if (turns >= 2)
            {
                xStep = (float)((double)width / (turns - 1));
            }
            else
            {
                xStep = width;
            }

            yStep = (float)((double)height / maximum);
            lastY = values[0];
            for (int I = 1, last = values.Count - 1; I <= last; I++)
            {
                nextY = values[I];
                x1 = Round((double)(I - 1) * xStep);
                y1 = Round((double)lastY * yStep);
                // { Original advances from the rounded previous x coordinate. }
                x2 = Round((double)x1 + xStep);
                y2 = Round((double)nextY * yStep);
                DrawImageLine(image, x1 + R.Left, R.Bottom - y1, R.Left + x2, R.Bottom - y2, 1, surface);
                lastY = nextY;
            }

            if (values.Count == 1)
            {
                x1 = 0;
                nextY = Round((double)lastY * yStep);
                y1 = nextY;
                x2 = width - 1;
                // { Original repeats the rounding and discards its result. }
                Round((double)lastY * yStep);
                y2 = nextY;
                DrawImageLine(image, x1 + R.Left, R.Bottom - y1, R.Left + x2, R.Bottom - y2, 1, surface);
            }
        }
    }

    // AoWResultsWin.pas:160
    public void DrawGold(sbyte player, in Rect R, in LockedSurface surface)
    {
        Int32List values;
        TurnInfo info;
        values = new Int32List();
        info = AoWHSMap.Current.Players.GetPlayers(player).Statistics.TurnInfo;
        for (int I = 0, last = info.GetCount() - 1; I <= last; I++)
        {
            values.Add(info.GetTurnInfo(I).Gold);
        }

        var playerLocal = AoWHSMap.Current.Players.GetPlayers(player);
        DrawGraph(DialogHosts.UIGraphics.EndScene[playerLocal.PlayerRace + 13], values, _maxTurns, _maxGold, R, surface);
        values?.Dispose();
        KeyMax.Text = IntToStr(_maxGold);
    }

    // AoWResultsWin.pas:175
    public void DrawMana(sbyte player, in Rect R, in LockedSurface surface)
    {
        Int32List values;
        TurnInfo info;
        values = new Int32List();
        info = AoWHSMap.Current.Players.GetPlayers(player).Statistics.TurnInfo;
        for (int I = 0, last = info.GetCount() - 1; I <= last; I++)
        {
            values.Add(info.GetTurnInfo(I).Mana);
        }

        var playerLocal = AoWHSMap.Current.Players.GetPlayers(player);
        DrawGraph(DialogHosts.UIGraphics.EndScene[playerLocal.PlayerRace + 13], values, _maxTurns, _maxMana, R, surface);
        values?.Dispose();
        KeyMax.Text = IntToStr(_maxMana);
    }

    // AoWResultsWin.pas:190
    public void DrawHeroes(sbyte player, in Rect R, in LockedSurface surface)
    {
        Int32List values;
        TurnInfo info;
        values = new Int32List();
        info = AoWHSMap.Current.Players.GetPlayers(player).Statistics.TurnInfo;
        for (int I = 0, last = info.GetCount() - 1; I <= last; I++)
        {
            values.Add(info.GetTurnInfo(I).Heroes);
        }

        var playerLocal = AoWHSMap.Current.Players.GetPlayers(player);
        DrawGraph(DialogHosts.UIGraphics.EndScene[playerLocal.PlayerRace + 13], values, _maxTurns, _maxHeroes, R, surface);
        values?.Dispose();
        KeyMax.Text = IntToStr(_maxHeroes);
    }

    // AoWResultsWin.pas:205
    public void DrawArmies(sbyte player, in Rect R, in LockedSurface surface)
    {
        Int32List values;
        TurnInfo info;
        values = new Int32List();
        info = AoWHSMap.Current.Players.GetPlayers(player).Statistics.TurnInfo;
        for (int I = 0, last = info.GetCount() - 1; I <= last; I++)
        {
            values.Add(info.GetTurnInfo(I).ArmyStrength);
        }

        var playerLocal = AoWHSMap.Current.Players.GetPlayers(player);
        DrawGraph(DialogHosts.UIGraphics.EndScene[playerLocal.PlayerRace + 13], values, _maxTurns, _maxArmies, R, surface);
        values?.Dispose();
        KeyMax.Text = IntToStr(_maxArmies);
    }

    // AoWResultsWin.pas:220
    public void DrawStructures(sbyte player, in Rect R, in LockedSurface surface)
    {
        Int32List values;
        TurnInfo info;
        values = new Int32List();
        info = AoWHSMap.Current.Players.GetPlayers(player).Statistics.TurnInfo;
        for (int I = 0, last = info.GetCount() - 1; I <= last; I++)
        {
            values.Add(info.GetTurnInfo(I).Structures);
        }

        var playerLocal = AoWHSMap.Current.Players.GetPlayers(player);
        DrawGraph(DialogHosts.UIGraphics.EndScene[playerLocal.PlayerRace + 13], values, _maxTurns, _maxStructures, R, surface);
        values?.Dispose();
        KeyMax.Text = IntToStr(_maxStructures);
    }

    // AoWResultsWin.pas:235
    public void DrawStrength(sbyte player, in Rect R, in LockedSurface surface)
    {
        Int32List values;
        TurnInfo info;
        values = new Int32List();
        info = AoWHSMap.Current.Players.GetPlayers(player).Statistics.TurnInfo;
        for (int I = 0, last = info.GetCount() - 1; I <= last; I++)
        {
            values.Add(info.GetTurnInfo(I).GetSuperiority());
        }

        var playerLocal = AoWHSMap.Current.Players.GetPlayers(player);
        DrawGraph(DialogHosts.UIGraphics.EndScene[playerLocal.PlayerRace + 13], values, _maxTurns, _maxStrength, R, surface);
        values?.Dispose();
        // { Original strength graph labels the structures maximum. }
        KeyMax.Text = IntToStr(_maxStructures);
    }

    // AoWResultsWin.pas:270
    public void ResultsWinCreate()
    {
        Players = new ByteList();
        Controls = new List<CheckBox>();
        Controls.Add(P1CB);
        Controls.Add(P2CB);
        Controls.Add(P3CB);
        Controls.Add(P4CB);
        Controls.Add(P5CB);
        Controls.Add(P6CB);
        Controls.Add(P7CB);
        Controls.Add(P8CB);
        Controls.Add(P9CB);
        Controls.Add(P10CB);
        Controls.Add(P11CB);
        Controls.Add(P12CB);
    }

    // AoWResultsWin.pas:288
    public void ResultsWinDestroy()
    {
        Players?.Dispose();
        Controls?.Clear();
    }

    // AoWResultsWin.pas:294
    public void ContinueBtnClick()
    {
        FinishPyre(DialogHosts.PyreScreen);
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.ResultsWindow.ResultsPnl.Visible = false;
        AudioPlayback.FadeMusicVolume(DialogHosts.SoundDialog.MusicVolume.Position, 2);
        if (AoWHSMap.Current.GameStatus == AoWHSMapGameStatus.Terminated)
        {
            if (GameTables.GameEngine.Campaign == null)
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
    public void AxisDraw(LockedSurface surface)
    {
        DrawGraphs(surface);
    }

    // AoWResultsWin.pas:329
    public void AgendaChange()
    {
        ResultsPnl.Drawn = false;
    }

    // AoWResultsWin.pas:334
    public void ResultsPnlShow()
    {
        UpdateSelection();
    }

    // AoWResultsWin.pas:339
    public void P1CBPreDraw(CheckBox sender, LockedSurface surface, int slot)
    {
        Rect R;
        var component = sender;
        if (slot < Players.Count)
        {
            R = new Rect(component.WinLeft + ResultsPnl.WinLeft + 1, component.WinTop + ResultsPnl.WinTop + 1, component.WinLeft + component.WinWidth + ResultsPnl.WinLeft - 1, component.WinTop + component.WinHeight + ResultsPnl.WinTop - 1);
            var player = Players[slot];
            if (AoWHSMap.Current.Players.GetPlayers(player).GetFace() != null)
            {
                var player2 = Players[slot];
                AoWHSMap.Current.Players.GetPlayers(player2).Leader.ShowFace(R, surface);
            }

            var player3 = Players[slot];
            var playerPlayerRace = AoWHSMap.Current.Players.GetPlayers(player3);
            DialogHosts.UIGraphics.RelWin[playerPlayerRace.PlayerRace + 16].ShowClipped(component.WinLeft + ResultsPnl.WinLeft, component.WinTop + ResultsPnl.WinTop, surface);
        }
    }

    // AoWResultsWin.pas:355
    public void P1CBChange(CheckBox sender, int slot)
    {
        CheckBox control;
        control = sender;
        if (control.Checked)
        {
            if (Players.Count > slot)
            {
                var player = Players[slot];
                var nameEx = AoWHSMap.Current.Players.GetPlayers(player).GetNameWithRace();
                SideLbl.Text = nameEx;
            }

            ResValues.Items.Clear();
            ResValues.Items.Add("0%");
            ResValues.Items.Add("0%");
            ResValues.Items.Add("0%");
            ResValues.Items.Add("0%");
        }
    }

    private static readonly string s_turnText = "Turn";

    private static void FinishPyre(PyreScreen dialog)
    {
        AudioPlayback.SetSfxVolume(DialogHosts.SoundDialog.SFXVolume.Position);
        dialog.ClearFire();
        if (dialog.SavedSongStatus != null)
        {
            SongStatus.RestoreSongStatus(dialog.SavedSongStatus);
            dialog.SavedSongStatus = null;
        }
        dialog.GameOverEvent.Handled();
        DialogHosts.General.Manager.OverlayPriority = 0;
        ExecuteDoneEvent fDone = dialog.Done;
        GameOverEventLog fGameOverEvent = dialog.GameOverEvent;
        dialog.Done = null;
        dialog.GameOverEvent = null;
        fDone?.Invoke(fGameOverEvent);
        fGameOverEvent?.Release();
    }

    private static void DrawImageLine(LibraryImage image, int x1, int y1, int x2, int y2, int spacing, in LockedSurface lockedSurface)
    {
        int number = 0;
        int i;
        int j;
        if (x1 > x2)
        {
            i = x2;
            x2 = x1;
            x1 = i;
            j = y2;
            y2 = y1;
            y1 = j;
        }
        else
        {
            i = x1;
            j = y1;
        }
        int number2;
        int number3;
        int number4;
        int number7;
        int number6;
        if (y1 <= y2)
        {
            number2 = x2 - x1;
            number3 = y2 - y1;
            if (number3 >= number2)
            {
                number4 = (number2 - number3) * 2;
                int number5 = number2 * 2;
                number6 = number5 - number3;
                for (; j != y2; j++)
                {
                    if (number == 0)
                    {
                        image.Show(i, j, in lockedSurface);
                        number = spacing;
                    }
                    else
                    {
                        number--;
                    }
                    if (number6 >= 0)
                    {
                        number6 += number4;
                        i++;
                    }
                    else
                    {
                        number6 += number5;
                    }
                }
                if (number == 0)
                {
                    image.Show(i, j, in lockedSurface);
                }
                return;
            }
            number4 = (number3 - number2) * 2;
            number7 = number3 * 2;
            number6 = number7 - number2;
            for (; i != x2; i++)
            {
                if (number == 0)
                {
                    image.Show(i, j, in lockedSurface);
                    number = spacing;
                }
                else
                {
                    number--;
                }
                if (number6 >= 0)
                {
                    number6 += number4;
                    j++;
                }
                else
                {
                    number6 += number7;
                }
            }
            if (number == 0)
            {
                image.Show(i, j, in lockedSurface);
            }
            return;
        }
        number2 = x2 - x1;
        number3 = y1 - y2;
        if (number3 >= number2)
        {
            number4 = (number2 - number3) * 2;
            int number5 = number2 * 2;
            number6 = number5 - number3;
            while (j != y2)
            {
                if (number == 0)
                {
                    image.Show(i, j, in lockedSurface);
                    number = spacing;
                }
                else
                {
                    number--;
                }
                if (number6 >= 0)
                {
                    number6 += number4;
                    i++;
                }
                else
                {
                    number6 += number5;
                }
                j--;
            }
            if (number == 0)
            {
                image.Show(i, j, in lockedSurface);
            }
            return;
        }
        number4 = (number3 - number2) * 2;
        number7 = number3 * 2;
        number6 = number7 - number2;
        for (; i != x2; i++)
        {
            if (number == 0)
            {
                image.Show(i, j, in lockedSurface);
                number = spacing;
            }
            else
            {
                number--;
            }
            if (number6 >= 0)
            {
                number6 += number4;
                j--;
            }
            else
            {
                number6 += number7;
            }
        }
        if (number == 0)
        {
            image.Show(i, j, in lockedSurface);
        }
    }
}
