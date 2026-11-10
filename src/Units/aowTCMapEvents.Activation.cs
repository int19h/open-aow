#nullable disable
using Aow.Game.Combat;
using Aow.Game.Ui.Controls;
using static Aow.Rtl.RtlUnit;
namespace Aow.Game.Ui.Dialogs;

public partial class TTCMapEvents
{
    public void TCMapActivateMap()
    {
        Aow.Game.Audio.AudioHost.SongPlayer.Pause(false);
        if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDemo)
        {
            Aow.Game.Audio.AudioPlayback.PlaySongEx("Songs\\Combat\\Defend.IT", 2, 0, true);
        }
        else
        {
            switch (Random(3))
            {
                case 0:
                    Aow.Game.Audio.AudioPlayback.PlaySongEx("Songs\\Combat\\Defend.IT", 2, 0, true);
                    break;
                case 1:
                    Aow.Game.Audio.AudioPlayback.PlaySongEx("Songs\\Combat\\Outnum.IT", 2, 0, true);
                    break;
                case 2:
                    Aow.Game.Audio.AudioPlayback.PlaySongEx("Songs\\Combat\\Races.IT", 2, 0, true);
                    break;
            }
        }

        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.HSMap = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap;
        Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.Scanner.SetMap(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap);
        Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.Scanner.FPreviousWidth = 0;
        Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.Scanner.FIncremental = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.Scanner.FSized = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.Scanner.FUpdateStep = -1;

        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.General.HideWindows();
        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap = (TAoWCombatMap)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombatMap;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Visible = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlaySounds;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldAlignWidth = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.AlignWidth;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldAlignHeight = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.AlignHeight;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldRect.Left = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.LeftOffset;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldRect.Top = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.TopOffset;
        if (Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldAlignWidth == TAOWAlignWidth.awBoth)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldRect.Right = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.RightOffset;
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldRect.Right = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinWidth;
        }

        if (Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldAlignHeight == TAOWAlignHeight.ahBoth)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldRect.Bottom = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.BottomOffset;
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldRect.Bottom = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinHeight;
        }

        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.LeftOffset = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Left;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.TopOffset = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Top;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.RightOffset = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Right;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.BottomOffset = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Bottom;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinWidth = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Right;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinHeight = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Bottom;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.AlignWidth = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignWidth;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.AlignHeight = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignHeight;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.FAligned = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TBWorld.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TBCombat.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.UpdateCombatPlayerIndicator();
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.AutoBtn.Checked = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TCChatBtn.Visible = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.ChatBtn.Visible;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TCUnitBtn.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.FDrawn = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin.Visible = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlaySounds;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TCScannerBtn.Visible = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlaySounds;
        Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin.Visible = false;
        if (!Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlaySounds)
        {
            FChatHeight = Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.WinHeight;
            Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Alignment.AlignHeight = TAOWAlignHeight.ahBoth;
        }
        else
        {
            FChatHeight = 0;
        }

        if (Aow.Game.Ui.Dialogs.DialogHosts.General.FChatVisible || !Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlaySounds)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Visible = true;
        }
    }
}
