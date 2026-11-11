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
    public void TCMapActivateMap()
    {
        AudioHost.SongPlayer.Pause(false);
        if (GameTables.AoWEngine.FDemo)
        {
            AudioPlayback.PlaySongEx("Songs\\Combat\\Defend.IT", 2, 0, true);
        }
        else
        {
            switch (Random(3))
            {
                case 0:
                    AudioPlayback.PlaySongEx("Songs\\Combat\\Defend.IT", 2, 0, true);
                    break;
                case 1:
                    AudioPlayback.PlaySongEx("Songs\\Combat\\Outnum.IT", 2, 0, true);
                    break;
                case 2:
                    AudioPlayback.PlaySongEx("Songs\\Combat\\Races.IT", 2, 0, true);
                    break;
            }
        }

        DialogHosts.MWindow.MapViewer.HSMap = CombatMap.Current;
        DialogHosts.TCScanner.Scanner.SetMap(CombatMap.Current);
        DialogHosts.TCScanner.Scanner.FPreviousWidth = 0;
        DialogHosts.TCScanner.Scanner.FIncremental = false;
        DialogHosts.TCScanner.Scanner.FSized = false;
        DialogHosts.TCScanner.Scanner.FUpdateStep = -1;

        DialogHosts.MWindow.MainWindow.Visible = true;
        DialogHosts.General.HideWindows();
        CombatMap.Current = (CombatMap)AoWHSMap.Current.FCombatMap;
        DialogHosts.MWindow.MainWindow.Visible = CombatMap.Current.FPlaySounds;
        DialogHosts.MWindow.FWorldAlignWidth = DialogHosts.MWindow.MainWindow.Alignment.AlignWidth;
        DialogHosts.MWindow.FWorldAlignHeight = DialogHosts.MWindow.MainWindow.Alignment.AlignHeight;
        DialogHosts.MWindow.FWorldRect.Left = DialogHosts.MWindow.MainWindow.Alignment.LeftOffset;
        DialogHosts.MWindow.FWorldRect.Top = DialogHosts.MWindow.MainWindow.Alignment.TopOffset;
        if (DialogHosts.MWindow.FWorldAlignWidth == AlignWidth.Both)
        {
            DialogHosts.MWindow.FWorldRect.Right = DialogHosts.MWindow.MainWindow.Alignment.RightOffset;
        }
        else
        {
            DialogHosts.MWindow.FWorldRect.Right = DialogHosts.MWindow.MainWindow.WinWidth;
        }

        if (DialogHosts.MWindow.FWorldAlignHeight == AlignHeight.Both)
        {
            DialogHosts.MWindow.FWorldRect.Bottom = DialogHosts.MWindow.MainWindow.Alignment.BottomOffset;
        }
        else
        {
            DialogHosts.MWindow.FWorldRect.Bottom = DialogHosts.MWindow.MainWindow.WinHeight;
        }

        DialogHosts.MWindow.MainWindow.Alignment.LeftOffset = DialogHosts.MWindow.FMapRect.Left;
        DialogHosts.MWindow.MainWindow.Alignment.TopOffset = DialogHosts.MWindow.FMapRect.Top;
        DialogHosts.MWindow.MainWindow.Alignment.RightOffset = DialogHosts.MWindow.FMapRect.Right;
        DialogHosts.MWindow.MainWindow.Alignment.BottomOffset = DialogHosts.MWindow.FMapRect.Bottom;
        DialogHosts.MWindow.MainWindow.WinWidth = DialogHosts.MWindow.FMapRect.Right;
        DialogHosts.MWindow.MainWindow.WinHeight = DialogHosts.MWindow.FMapRect.Bottom;
        DialogHosts.MWindow.MainWindow.Alignment.AlignWidth = DialogHosts.MWindow.FMapAlignWidth;
        DialogHosts.MWindow.MainWindow.Alignment.AlignHeight = DialogHosts.MWindow.FMapAlignHeight;
        DialogHosts.MWindow.MainWindow.FAligned = false;
        DialogHosts.TBWindow.TBWorld.Visible = false;
        DialogHosts.TBWindow.TBCombat.Visible = true;
        DialogHosts.TBWindow.UpdateCombatPlayerIndicator();
        DialogHosts.TBWindow.AutoBtn.Checked = false;
        DialogHosts.TBWindow.TCChatBtn.Visible = DialogHosts.TBWindow.ChatBtn.Visible;
        DialogHosts.TBWindow.TCUnitBtn.Visible = false;
        DialogHosts.TBWindow.TaskWindow.FDrawn = false;
        DialogHosts.TCScanner.TCScannerWin.Visible = CombatMap.Current.FPlaySounds;
        DialogHosts.TBWindow.TCScannerBtn.Visible = CombatMap.Current.FPlaySounds;
        DialogHosts.TCUnit.TCUnitWin.Visible = false;
        if (!CombatMap.Current.FPlaySounds)
        {
            FChatHeight = DialogHosts.ChatWindow.ChatWin.WinHeight;
            DialogHosts.ChatWindow.ChatWin.Alignment.AlignHeight = AlignHeight.Both;
        }
        else
        {
            FChatHeight = 0;
        }

        if (DialogHosts.General.FChatVisible || !CombatMap.Current.FPlaySounds)
        {
            DialogHosts.ChatWindow.ChatWin.Visible = true;
        }
    }
}
