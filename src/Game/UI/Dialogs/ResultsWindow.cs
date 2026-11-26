#nullable disable warnings
// Translated from AoWResultsWin.pas.
using System.Globalization;
using Aow.Engine.Collections;
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Events;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.UI.Dialogs;

// player indices, checkbox controls
public sealed partial class ResultsWindow(UIContext ui) : UIModule(ui)
{
    public CheckBox Player1CheckBox { get; private set; }
    public CheckBox Player2CheckBox { get; private set; }
    public CheckBox Player3CheckBox { get; private set; }
    public CheckBox Player4CheckBox { get; private set; }
    public CheckBox Player5CheckBox { get; private set; }
    public CheckBox Player6CheckBox { get; private set; }
    public CheckBox Player7CheckBox { get; private set; }
    public CheckBox Player8CheckBox { get; private set; }
    public CheckBox Player9CheckBox { get; private set; }
    public CheckBox Player10CheckBox { get; private set; }
    public CheckBox Player11CheckBox { get; private set; }
    public CheckBox Player12CheckBox { get; private set; }
    public Label TurnLabel { get; private set; }
    public Memo ResultValuesMemo { get; private set; }
    public Label SideLabel { get; private set; }
    public Image AxisImage { get; private set; }
    public Button ExitButton { get; private set; }
    public Button ContinueButton { get; private set; }
    public Panel ResultsPanel { get; private set; }
    public CheckBox StrengthCheckBox { get; private set; }
    public CheckBox GoldCheckBox { get; private set; }
    public CheckBox ArmiesCheckBox { get; private set; }
    public CheckBox HeroesCheckBox { get; private set; }
    public CheckBox StructuresCheckBox { get; private set; }
    public Label KeyMaximumLabel { get; private set; }
    public CheckBox ManaCheckBox { get; private set; }

    private int _maxTurns;
    private int _maxGold;
    private int _maxMana;
    private int _maxArmies;

    private int _maxHeroes;
    private int _maxStructures;
    private int _maxStrength;

    public ByteList Players { get; private set; }
    public List<CheckBox> Controls { get; private set; }

    // AoWResultsWin.pas:93
    public void UpdateSelection()
    {
        _maxTurns = 1;
        _maxGold = 1;
        _maxMana = 1;
        _maxArmies = 1;
        _maxHeroes = 1;
        _maxStructures = 1;
        _maxStrength = 1;
        for (int i = 0, last = Players.Count - 1; i <= last; i++)
        {
            byte player = Players[i];
            PlayerStatistics stats = UI.Engine.StrategicMap.Players.GetPlayers(player).Statistics;
            int count = stats.TurnInfo.GetCount();
            if (_maxTurns < count)
            {
                _maxTurns = stats.TurnInfo.GetCount();
            }

            for (int j = 0, lastJ = stats.TurnInfo.GetCount() - 1; j <= lastJ; j++)
            {
                TurnInfoItem turnInfo = stats.TurnInfo.GetTurnInfo(j);
                if (_maxArmies < turnInfo.ArmyStrength)
                {
                    _maxArmies = stats.TurnInfo.GetTurnInfo(j).ArmyStrength;
                }

                TurnInfoItem fTurnInfoTurnInfo = stats.TurnInfo.GetTurnInfo(j);
                if (_maxGold < fTurnInfoTurnInfo.Gold)
                {
                    _maxGold = stats.TurnInfo.GetTurnInfo(j).Gold;
                }

                TurnInfoItem turnInfo2 = stats.TurnInfo.GetTurnInfo(j);
                if (_maxMana < turnInfo2.Mana)
                {
                    _maxMana = stats.TurnInfo.GetTurnInfo(j).Mana;
                }

                TurnInfoItem turnInfo3 = stats.TurnInfo.GetTurnInfo(j);
                if (_maxHeroes < turnInfo3.Heroes)
                {
                    _maxHeroes = stats.TurnInfo.GetTurnInfo(j).Heroes;
                }

                TurnInfoItem turnInfo4 = stats.TurnInfo.GetTurnInfo(j);
                if (_maxStructures < turnInfo4.Structures)
                {
                    _maxStructures = stats.TurnInfo.GetTurnInfo(j).Structures;
                }

                int superiority = stats.TurnInfo.GetTurnInfo(j).Superiority;
                if (_maxStrength < superiority)
                {
                    _maxStrength = stats.TurnInfo.GetTurnInfo(j).Superiority;
                }
            }
        }

        string turn = UI.Engine.TranslateResourceString(s_turnText);
        TurnLabel.Text = $"{turn} {_maxTurns.ToString(CultureInfo.InvariantCulture)}";
    }

    // AoWResultsWin.pas:250
    public void DrawGraphs(in LockedSurface surface)
    {
        Rect bounds = default;
        bounds.Left = ResultsPanel.AbsoluteLeft + AxisImage.WinLeft + 5;
        bounds.Top = ResultsPanel.AbsoluteTop + AxisImage.WinTop + 4;
        bounds.Right = ResultsPanel.AbsoluteLeft + AxisImage.WinLeft + 338;
        bounds.Bottom = ResultsPanel.AbsoluteTop + AxisImage.WinTop + 117;
        for (int i = 0, last = Players.Count - 1; i <= last; i++)
        {
            if (GoldCheckBox.Checked)
            {
                DrawGold(PlayerId.FromInt32((sbyte)Players[i]), R: bounds, surface);
            }

            if (ManaCheckBox.Checked)
            {
                DrawMana(PlayerId.FromInt32((sbyte)Players[i]), R: bounds, surface);
            }

            if (HeroesCheckBox.Checked)
            {
                DrawHeroes(PlayerId.FromInt32((sbyte)Players[i]), R: bounds, surface);
            }

            if (StructuresCheckBox.Checked)
            {
                DrawStructures(PlayerId.FromInt32((sbyte)Players[i]), R: bounds, surface);
            }

            if (ArmiesCheckBox.Checked)
            {
                DrawArmies(PlayerId.FromInt32((sbyte)Players[i]), R: bounds, surface);
            }

            if (StrengthCheckBox.Checked)
            {
                DrawStrength(PlayerId.FromInt32((sbyte)Players[i]), R: bounds, surface);
            }
        }
    }

    // AoWResultsWin.pas:123
    public void DrawGraph(LibraryImage? image, Int32List values, int turns, int maximum, in Rect R, in LockedSurface surface)
    {
        if (values.Count != 0)
        {
            int width = R.Right - R.Left;
            int height = R.Bottom - R.Top;
            float xStep = turns >= 2 ? (float)((double)width / (turns - 1)) : width;

            float yStep = (float)((double)height / maximum);
            int lastY = values[0];
            int y1;
            int x2;
            int nextY;
            int x1;
            int y2;
            for (int i = 1, last = values.Count - 1; i <= last; i++)
            {
                nextY = values[i];
                x1 = Round((double)(i - 1) * xStep);
                y1 = Round((double)lastY * yStep);
                // { Original advances from the rounded previous x coordinate. }
                x2 = Round((double)x1 + xStep);
                y2 = Round((double)nextY * yStep);
                DrawImageLine(image, x1 + R.Left, R.Bottom - y1, R.Left + x2, R.Bottom - y2, spacing: 1, lockedSurface: surface);
                lastY = nextY;
            }

            if (values.Count == 1)
            {
                x1 = 0;
                nextY = Round((double)lastY * yStep);
                y1 = nextY;
                x2 = width - 1;
                // { Original repeats the rounding and discards its result. }
                _ = Round((double)lastY * yStep);
                y2 = nextY;
                DrawImageLine(image, x1 + R.Left, R.Bottom - y1, R.Left + x2, R.Bottom - y2, spacing: 1, lockedSurface: surface);
            }
        }
    }

    // AoWResultsWin.pas:160
    public void DrawGold(PlayerId player, in Rect R, in LockedSurface surface)
    {
        Int32List values = [];
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        TurnInfo info = strategicMap.Players.GetPlayers(player).Statistics.TurnInfo;
        for (int i = 0, last = info.GetCount() - 1; i <= last; i++)
        {
            _ = values.Add(info.GetTurnInfo(i).Gold);
        }

        Player? playerLocal = strategicMap.Players.GetPlayers(player);
        DrawGraph(UI.Graphics.EndScene[playerLocal.PlayerRace + 13], values, turns: _maxTurns, maximum: _maxGold, R, surface);
        KeyMaximumLabel.Text = _maxGold.ToString(CultureInfo.InvariantCulture);
    }

    // AoWResultsWin.pas:175
    public void DrawMana(PlayerId player, in Rect R, in LockedSurface surface)
    {
        Int32List values = [];
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        TurnInfo info = strategicMap.Players.GetPlayers(player).Statistics.TurnInfo;
        for (int i = 0, last = info.GetCount() - 1; i <= last; i++)
        {
            _ = values.Add(info.GetTurnInfo(i).Mana);
        }

        Player? playerLocal = strategicMap.Players.GetPlayers(player);
        DrawGraph(UI.Graphics.EndScene[playerLocal.PlayerRace + 13], values, turns: _maxTurns, maximum: _maxMana, R, surface);
        KeyMaximumLabel.Text = _maxMana.ToString(CultureInfo.InvariantCulture);
    }

    // AoWResultsWin.pas:190
    public void DrawHeroes(PlayerId player, in Rect R, in LockedSurface surface)
    {
        Int32List values = [];
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        TurnInfo info = strategicMap.Players.GetPlayers(player).Statistics.TurnInfo;
        for (int i = 0, last = info.GetCount() - 1; i <= last; i++)
        {
            _ = values.Add(info.GetTurnInfo(i).Heroes);
        }

        Player? playerLocal = strategicMap.Players.GetPlayers(player);
        DrawGraph(UI.Graphics.EndScene[playerLocal.PlayerRace + 13], values, turns: _maxTurns, maximum: _maxHeroes, R, surface);
        KeyMaximumLabel.Text = _maxHeroes.ToString(CultureInfo.InvariantCulture);
    }

    // AoWResultsWin.pas:205
    public void DrawArmies(PlayerId player, in Rect R, in LockedSurface surface)
    {
        Int32List values = [];
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        TurnInfo info = strategicMap.Players.GetPlayers(player).Statistics.TurnInfo;
        for (int i = 0, last = info.GetCount() - 1; i <= last; i++)
        {
            _ = values.Add(info.GetTurnInfo(i).ArmyStrength);
        }

        Player? playerLocal = strategicMap.Players.GetPlayers(player);
        DrawGraph(UI.Graphics.EndScene[playerLocal.PlayerRace + 13], values, turns: _maxTurns, maximum: _maxArmies, R, surface);
        KeyMaximumLabel.Text = _maxArmies.ToString(CultureInfo.InvariantCulture);
    }

    // AoWResultsWin.pas:220
    public void DrawStructures(PlayerId player, in Rect R, in LockedSurface surface)
    {
        Int32List values = [];
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        TurnInfo info = strategicMap.Players.GetPlayers(player).Statistics.TurnInfo;
        for (int i = 0, last = info.GetCount() - 1; i <= last; i++)
        {
            _ = values.Add(info.GetTurnInfo(i).Structures);
        }

        Player? playerLocal = strategicMap.Players.GetPlayers(player);
        DrawGraph(UI.Graphics.EndScene[playerLocal.PlayerRace + 13], values, turns: _maxTurns, maximum: _maxStructures, R, surface);
        KeyMaximumLabel.Text = _maxStructures.ToString(CultureInfo.InvariantCulture);
    }

    // AoWResultsWin.pas:235
    public void DrawStrength(PlayerId player, in Rect R, in LockedSurface surface)
    {
        Int32List values = [];
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        TurnInfo info = strategicMap.Players.GetPlayers(player).Statistics.TurnInfo;
        for (int i = 0, last = info.GetCount() - 1; i <= last; i++)
        {
            _ = values.Add(info.GetTurnInfo(i).Superiority);
        }

        Player? playerLocal = strategicMap.Players.GetPlayers(player);
        DrawGraph(UI.Graphics.EndScene[playerLocal.PlayerRace + 13], values, turns: _maxTurns, maximum: _maxStrength, R, surface);
        // { Original strength graph labels the structures maximum. }
        KeyMaximumLabel.Text = _maxStructures.ToString(CultureInfo.InvariantCulture);
    }

    // AoWResultsWin.pas:270
    public void ResultsWinCreate()
    {
        Players = [];
        Controls =
        [
            Player1CheckBox,
            Player2CheckBox,
            Player3CheckBox,
            Player4CheckBox,
            Player5CheckBox,
            Player6CheckBox,
            Player7CheckBox,
            Player8CheckBox,
            Player9CheckBox,
            Player10CheckBox,
            Player11CheckBox,
            Player12CheckBox,
        ];
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
        FinishPyre(UI.PyreScreen);
        UI.General.PlayButtonSound(0);
        UI.ResultsWindow.ResultsPanel.Visible = false;
        AudioPlayback.FadeMusicVolume(UI.SoundDialog.MusicVolumeSlider.Position, speed: 2);
        if (UI.Engine.StrategicMap.GameStatus == StrategicMapGameStatus.Terminated)
        {
            if (UI.Engine.Campaign is null)
            {
                UI.General.ReturnToTitle();
            }
            else
            {
                WorldMap.ShowCampaignMap(UI.WorldMap);
            }
        }
        else
        {
            UI.TitleWindow.MainWindow.Visible = false;
            VideoDialog.SetVideoMode(UI.VideoDialog, fullScreen: VideoDialog.ScreenMode.Game);
            UI.VideoDialog.ApplyMode();
        }
    }

    // AoWResultsWin.pas:313
    public void ExitBtnClick()
    {
        ResultsPanel.Visible = false;
        UI.General.ReturnToTitle();
    }

    // AoWResultsWin.pas:319
    public void ResultsPnlUpdate() => UI.PyreScreen.DeadPnlUpdate();

    // AoWResultsWin.pas:324
    public void AxisDraw(LockedSurface surface) => DrawGraphs(surface);

    // AoWResultsWin.pas:329
    public void AgendaChange() => ResultsPanel.IsDrawn = false;

    // AoWResultsWin.pas:334
    public void ResultsPnlShow() => UpdateSelection();

    // AoWResultsWin.pas:339
    public void P1CBPreDraw(CheckBox sender, LockedSurface surface, int slot)
    {
        CheckBox component = sender;
        if (slot < Players.Count)
        {
            Rect r = new(component.WinLeft + ResultsPanel.AbsoluteLeft + 1, component.WinTop + ResultsPanel.AbsoluteTop + 1, component.WinLeft + component.WinWidth + ResultsPanel.AbsoluteLeft - 1, component.WinTop + component.WinHeight + ResultsPanel.AbsoluteTop - 1);
            byte player = Players[slot];
            if (UI.Engine.StrategicMap.Players.GetPlayers(player).GetFace() is not null)
            {
                byte player2 = Players[slot];
                UI.Engine.StrategicMap.Players.GetPlayers(player2).Leader.ShowFace(r, lockedSurface: surface);
            }

            byte player3 = Players[slot];
            Player? playerPlayerRace = UI.Engine.StrategicMap.Players.GetPlayers(player3);
            UI.Graphics.RelationWindow[playerPlayerRace.PlayerRace + 16].ShowClipped(component.WinLeft + ResultsPanel.AbsoluteLeft, component.WinTop + ResultsPanel.AbsoluteTop, lockedSurface: surface);
        }
    }

    // AoWResultsWin.pas:355
    public void P1CBChange(CheckBox sender, int slot)
    {
        CheckBox control = sender;
        if (control.Checked)
        {
            if (Players.Count > slot)
            {
                byte player = Players[slot];
                string nameEx = UI.Engine.StrategicMap.Players.GetPlayers(player).GetNameWithRace();
                SideLabel.Text = nameEx;
            }

            ResultValuesMemo.Items.Clear();
            _ = ResultValuesMemo.Items.Add("0%");
            _ = ResultValuesMemo.Items.Add("0%");
            _ = ResultValuesMemo.Items.Add("0%");
            _ = ResultValuesMemo.Items.Add("0%");
        }
    }

    private static readonly string s_turnText = "Turn";

    private static void FinishPyre(PyreScreen dialog)
    {
        AudioPlayback.SetSfxVolume(dialog.UI.SoundDialog.EffectsVolumeSlider.Position);
        dialog.ClearFire();
        if (dialog.SavedSongStatus is not null)
        {
            _ = SongStatus.RestoreSongStatus(dialog.SavedSongStatus);
            dialog.SavedSongStatus = null;
        }
        dialog.GameOverEvent.Handled();
        dialog.UI.General.Manager.OverlayPriority = 0;
        ExecuteDoneEvent? fDone = dialog.TakeDoneHandler();
        GameOverEventLog fGameOverEvent = dialog.GameOverEvent;
        dialog.GameOverEvent = null;
        fDone?.Invoke(fGameOverEvent);
        _ = (fGameOverEvent?.Release());
    }

    private static void DrawImageLine(LibraryImage? image, int x1, int y1, int x2, int y2, int spacing, in LockedSurface lockedSurface)
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
