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

        DialogHosts.MapWindow.MapViewer.HexSpriteMap = CombatMap.Current;
        DialogHosts.CombatScanner.Scanner.SetMap(CombatMap.Current);
        DialogHosts.CombatScanner.Scanner.PreviousWidth = 0;
        DialogHosts.CombatScanner.Scanner.Incremental = false;
        DialogHosts.CombatScanner.Scanner.Sized = false;
        DialogHosts.CombatScanner.Scanner.UpdateStep = -1;

        DialogHosts.MapWindow.MainWindow.Visible = true;
        DialogHosts.General.HideWindows();
        CombatMap.Current = (CombatMap)AoWHSMap.Current.CombatMap;
        DialogHosts.MapWindow.MainWindow.Visible = CombatMap.Current.PlaySounds;
        DialogHosts.MapWindow.WorldAlignWidth = DialogHosts.MapWindow.MainWindow.Alignment.AlignWidth;
        DialogHosts.MapWindow.WorldAlignHeight = DialogHosts.MapWindow.MainWindow.Alignment.AlignHeight;
        DialogHosts.MapWindow.WorldRectangle.Left = DialogHosts.MapWindow.MainWindow.Alignment.LeftOffset;
        DialogHosts.MapWindow.WorldRectangle.Top = DialogHosts.MapWindow.MainWindow.Alignment.TopOffset;
        if (DialogHosts.MapWindow.WorldAlignWidth == AlignWidth.Both)
        {
            DialogHosts.MapWindow.WorldRectangle.Right = DialogHosts.MapWindow.MainWindow.Alignment.RightOffset;
        }
        else
        {
            DialogHosts.MapWindow.WorldRectangle.Right = DialogHosts.MapWindow.MainWindow.WinWidth;
        }

        if (DialogHosts.MapWindow.WorldAlignHeight == AlignHeight.Both)
        {
            DialogHosts.MapWindow.WorldRectangle.Bottom = DialogHosts.MapWindow.MainWindow.Alignment.BottomOffset;
        }
        else
        {
            DialogHosts.MapWindow.WorldRectangle.Bottom = DialogHosts.MapWindow.MainWindow.WinHeight;
        }

        DialogHosts.MapWindow.MainWindow.Alignment.LeftOffset = DialogHosts.MapWindow.MapRectangle.Left;
        DialogHosts.MapWindow.MainWindow.Alignment.TopOffset = DialogHosts.MapWindow.MapRectangle.Top;
        DialogHosts.MapWindow.MainWindow.Alignment.RightOffset = DialogHosts.MapWindow.MapRectangle.Right;
        DialogHosts.MapWindow.MainWindow.Alignment.BottomOffset = DialogHosts.MapWindow.MapRectangle.Bottom;
        DialogHosts.MapWindow.MainWindow.WinWidth = DialogHosts.MapWindow.MapRectangle.Right;
        DialogHosts.MapWindow.MainWindow.WinHeight = DialogHosts.MapWindow.MapRectangle.Bottom;
        DialogHosts.MapWindow.MainWindow.Alignment.AlignWidth = DialogHosts.MapWindow.MapAlignWidth;
        DialogHosts.MapWindow.MainWindow.Alignment.AlignHeight = DialogHosts.MapWindow.MapAlignHeight;
        DialogHosts.MapWindow.MainWindow.Aligned = false;
        DialogHosts.TaskBar.WorldPanel.Visible = false;
        DialogHosts.TaskBar.CombatPanel.Visible = true;
        DialogHosts.TaskBar.UpdateCombatPlayerIndicator();
        DialogHosts.TaskBar.AutoCombatCheckBox.Checked = false;
        DialogHosts.TaskBar.CombatChatButton.Visible = DialogHosts.TaskBar.ChatButton.Visible;
        DialogHosts.TaskBar.CombatUnitButton.Visible = false;
        DialogHosts.TaskBar.TaskWindow.Drawn = false;
        DialogHosts.CombatScanner.CombatScannerWindow.Visible = CombatMap.Current.PlaySounds;
        DialogHosts.TaskBar.CombatScannerButton.Visible = CombatMap.Current.PlaySounds;
        DialogHosts.CombatUnitInfo.MainWindow.Visible = false;
        if (!CombatMap.Current.PlaySounds)
        {
            _chatHeight = DialogHosts.ChatWindow.MainWindow.WinHeight;
            DialogHosts.ChatWindow.MainWindow.Alignment.AlignHeight = AlignHeight.Both;
        }
        else
        {
            _chatHeight = 0;
        }

        if (DialogHosts.General.ChatVisible || !CombatMap.Current.PlaySounds)
        {
            DialogHosts.ChatWindow.MainWindow.Visible = true;
        }
    }
}
