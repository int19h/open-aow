#nullable disable
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.World.Map;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

public partial class CombatMapEvents
{
    public void CombatMapActivateMap()
    {
        AudioHost.SongPlayer.Pause(false);
        if (GameTables.GameEngine.Demo)
        {
            AudioPlayback.PlaySong("Songs\\Combat\\Defend.IT", 2, 0, true);
        }
        else
        {
            switch (Random(3))
            {
                case 0:
                    AudioPlayback.PlaySong("Songs\\Combat\\Defend.IT", 2, 0, true);
                    break;
                case 1:
                    AudioPlayback.PlaySong("Songs\\Combat\\Outnum.IT", 2, 0, true);
                    break;
                case 2:
                    AudioPlayback.PlaySong("Songs\\Combat\\Races.IT", 2, 0, true);
                    break;
            }
        }

        UI.MapWindow.MapViewer.HexSpriteMap = CombatMap.Current;
        UI.CombatScanner.Scanner.SetMap(CombatMap.Current);
        UI.CombatScanner.Scanner.PreviousWidth = 0;
        UI.CombatScanner.Scanner.Incremental = false;
        UI.CombatScanner.Scanner.Sized = false;
        UI.CombatScanner.Scanner.UpdateStep = -1;

        UI.MapWindow.MainWindow.Visible = true;
        UI.General.HideWindows();
        CombatMap.Current = StrategicMap.Current.CombatMap;
        UI.MapWindow.MainWindow.Visible = CombatMap.Current.PlaySounds;
        UI.MapWindow.WorldAlignWidth = UI.MapWindow.MainWindow.Alignment.AlignWidth;
        UI.MapWindow.WorldAlignHeight = UI.MapWindow.MainWindow.Alignment.AlignHeight;
        UI.MapWindow.WorldRectangle.Left = UI.MapWindow.MainWindow.Alignment.LeftOffset;
        UI.MapWindow.WorldRectangle.Top = UI.MapWindow.MainWindow.Alignment.TopOffset;
        if (UI.MapWindow.WorldAlignWidth == AlignWidth.Both)
        {
            UI.MapWindow.WorldRectangle.Right = UI.MapWindow.MainWindow.Alignment.RightOffset;
        }
        else
        {
            UI.MapWindow.WorldRectangle.Right = UI.MapWindow.MainWindow.WinWidth;
        }

        if (UI.MapWindow.WorldAlignHeight == AlignHeight.Both)
        {
            UI.MapWindow.WorldRectangle.Bottom = UI.MapWindow.MainWindow.Alignment.BottomOffset;
        }
        else
        {
            UI.MapWindow.WorldRectangle.Bottom = UI.MapWindow.MainWindow.WinHeight;
        }

        UI.MapWindow.MainWindow.Alignment.LeftOffset = UI.MapWindow.MapRectangle.Left;
        UI.MapWindow.MainWindow.Alignment.TopOffset = UI.MapWindow.MapRectangle.Top;
        UI.MapWindow.MainWindow.Alignment.RightOffset = UI.MapWindow.MapRectangle.Right;
        UI.MapWindow.MainWindow.Alignment.BottomOffset = UI.MapWindow.MapRectangle.Bottom;
        UI.MapWindow.MainWindow.WinWidth = UI.MapWindow.MapRectangle.Right;
        UI.MapWindow.MainWindow.WinHeight = UI.MapWindow.MapRectangle.Bottom;
        UI.MapWindow.MainWindow.Alignment.AlignWidth = UI.MapWindow.MapAlignWidth;
        UI.MapWindow.MainWindow.Alignment.AlignHeight = UI.MapWindow.MapAlignHeight;
        UI.MapWindow.MainWindow.Aligned = false;
        UI.TaskBar.WorldPanel.Visible = false;
        UI.TaskBar.CombatPanel.Visible = true;
        UI.TaskBar.UpdateCombatPlayerIndicator();
        UI.TaskBar.AutoCombatCheckBox.Checked = false;
        UI.TaskBar.CombatChatButton.Visible = UI.TaskBar.ChatButton.Visible;
        UI.TaskBar.CombatUnitButton.Visible = false;
        UI.TaskBar.TaskWindow.Drawn = false;
        UI.CombatScanner.CombatScannerWindow.Visible = CombatMap.Current.PlaySounds;
        UI.TaskBar.CombatScannerButton.Visible = CombatMap.Current.PlaySounds;
        UI.CombatUnitInfo.MainWindow.Visible = false;
        if (!CombatMap.Current.PlaySounds)
        {
            _chatHeight = UI.ChatWindow.MainWindow.WinHeight;
            UI.ChatWindow.MainWindow.Alignment.AlignHeight = AlignHeight.Both;
        }
        else
        {
            _chatHeight = 0;
        }

        if (UI.General.ChatVisible || !CombatMap.Current.PlaySounds)
        {
            UI.ChatWindow.MainWindow.Visible = true;
        }
    }
}
