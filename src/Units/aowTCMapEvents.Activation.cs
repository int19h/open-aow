#nullable disable
using Aow.Game.Combat;
using Aow.Game.Ui.Controls;
using static Aow.Rtl.RtlUnit;
namespace Aow.Game.Ui.Dialogs;

public partial class CombatMapEvents
{
    public void TCMapActivateMap()
    {
        Audio.AudioHost.SongPlayer.Pause(false);
        if (Magic.Abilities.GameTables.AoWEngine.FDemo)
        {
            Audio.AudioPlayback.PlaySongEx("Songs\\Combat\\Defend.IT", 2, 0, true);
        }
        else
        {
            switch (Random(3))
            {
                case 0:
                    Audio.AudioPlayback.PlaySongEx("Songs\\Combat\\Defend.IT", 2, 0, true);
                    break;
                case 1:
                    Audio.AudioPlayback.PlaySongEx("Songs\\Combat\\Outnum.IT", 2, 0, true);
                    break;
                case 2:
                    Audio.AudioPlayback.PlaySongEx("Songs\\Combat\\Races.IT", 2, 0, true);
                    break;
            }
        }

        DialogHosts.MWindow.MapViewer.HSMap = AoWCombatMap.Current;
        DialogHosts.TCScanner.Scanner.SetMap(AoWCombatMap.Current);
        DialogHosts.TCScanner.Scanner.FPreviousWidth = 0;
        DialogHosts.TCScanner.Scanner.FIncremental = false;
        DialogHosts.TCScanner.Scanner.FSized = false;
        DialogHosts.TCScanner.Scanner.FUpdateStep = -1;

        DialogHosts.MWindow.MapWindow.Visible = true;
        DialogHosts.General.HideWindows();
        AoWCombatMap.Current = (AoWCombatMap)World.Map.AoWHSMap.Current.FCombatMap;
        DialogHosts.MWindow.MapWindow.Visible = AoWCombatMap.Current.FPlaySounds;
        DialogHosts.MWindow.FWorldAlignWidth = DialogHosts.MWindow.MapWindow.Alignment.AlignWidth;
        DialogHosts.MWindow.FWorldAlignHeight = DialogHosts.MWindow.MapWindow.Alignment.AlignHeight;
        DialogHosts.MWindow.FWorldRect.Left = DialogHosts.MWindow.MapWindow.Alignment.LeftOffset;
        DialogHosts.MWindow.FWorldRect.Top = DialogHosts.MWindow.MapWindow.Alignment.TopOffset;
        if (DialogHosts.MWindow.FWorldAlignWidth == AOWAlignWidth.Both)
        {
            DialogHosts.MWindow.FWorldRect.Right = DialogHosts.MWindow.MapWindow.Alignment.RightOffset;
        }
        else
        {
            DialogHosts.MWindow.FWorldRect.Right = DialogHosts.MWindow.MapWindow.WinWidth;
        }

        if (DialogHosts.MWindow.FWorldAlignHeight == AOWAlignHeight.Both)
        {
            DialogHosts.MWindow.FWorldRect.Bottom = DialogHosts.MWindow.MapWindow.Alignment.BottomOffset;
        }
        else
        {
            DialogHosts.MWindow.FWorldRect.Bottom = DialogHosts.MWindow.MapWindow.WinHeight;
        }

        DialogHosts.MWindow.MapWindow.Alignment.LeftOffset = DialogHosts.MWindow.FMapRect.Left;
        DialogHosts.MWindow.MapWindow.Alignment.TopOffset = DialogHosts.MWindow.FMapRect.Top;
        DialogHosts.MWindow.MapWindow.Alignment.RightOffset = DialogHosts.MWindow.FMapRect.Right;
        DialogHosts.MWindow.MapWindow.Alignment.BottomOffset = DialogHosts.MWindow.FMapRect.Bottom;
        DialogHosts.MWindow.MapWindow.WinWidth = DialogHosts.MWindow.FMapRect.Right;
        DialogHosts.MWindow.MapWindow.WinHeight = DialogHosts.MWindow.FMapRect.Bottom;
        DialogHosts.MWindow.MapWindow.Alignment.AlignWidth = DialogHosts.MWindow.FMapAlignWidth;
        DialogHosts.MWindow.MapWindow.Alignment.AlignHeight = DialogHosts.MWindow.FMapAlignHeight;
        DialogHosts.MWindow.MapWindow.FAligned = false;
        DialogHosts.TBWindow.TBWorld.Visible = false;
        DialogHosts.TBWindow.TBCombat.Visible = true;
        DialogHosts.TBWindow.UpdateCombatPlayerIndicator();
        DialogHosts.TBWindow.AutoBtn.Checked = false;
        DialogHosts.TBWindow.TCChatBtn.Visible = DialogHosts.TBWindow.ChatBtn.Visible;
        DialogHosts.TBWindow.TCUnitBtn.Visible = false;
        DialogHosts.TBWindow.TaskWindow.FDrawn = false;
        DialogHosts.TCScanner.TCScannerWin.Visible = AoWCombatMap.Current.FPlaySounds;
        DialogHosts.TBWindow.TCScannerBtn.Visible = AoWCombatMap.Current.FPlaySounds;
        DialogHosts.TCUnit.TCUnitWin.Visible = false;
        if (!AoWCombatMap.Current.FPlaySounds)
        {
            FChatHeight = DialogHosts.ChatWindow.ChatWin.WinHeight;
            DialogHosts.ChatWindow.ChatWin.Alignment.AlignHeight = AOWAlignHeight.Both;
        }
        else
        {
            FChatHeight = 0;
        }

        if (DialogHosts.General.FChatVisible || !AoWCombatMap.Current.FPlaySounds)
        {
            DialogHosts.ChatWindow.ChatWin.Visible = true;
        }
    }
}
