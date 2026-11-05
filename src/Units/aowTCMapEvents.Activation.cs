#nullable disable
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.Sound.SoundUnit;
using static Aow.Units.AoWE.AoWEUnit;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTCUnit;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.AOWMWindow.AOWMWindowUnit;
using static Aow.Units.AoWTCUnit.AoWTCUnitUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using Aow.Units.AoWComp;
namespace Aow.Units.aowTCMapEvents;

public partial class TTCMapEvents
{
    public void TCMapActivateMap()
    {
        SongPlayer.Pause(false);
        if (AoWEngine.FDemo)
        {
            PlaySongEx("Songs\\Combat\\Defend.IT", 2, 0, true);
        }
        else
        {
            switch (Random(3))
            {
                case 0:
                    PlaySongEx("Songs\\Combat\\Defend.IT", 2, 0, true);
                    break;
                case 1:
                    PlaySongEx("Songs\\Combat\\Outnum.IT", 2, 0, true);
                    break;
                case 2:
                    PlaySongEx("Songs\\Combat\\Races.IT", 2, 0, true);
                    break;
            }
        }

        MWindow.MapViewer.HSMap = AoWCombatMap;
        TCScanner.Scanner.SetMap(AoWCombatMap);
        TCScanner.Scanner.FPreviousWidth = 0;
        TCScanner.Scanner.FIncremental = false;
        TCScanner.Scanner.FSized = false;
        TCScanner.Scanner.FUpdateStep = -1;

        MWindow.MapWindow.Visible = true;
        General.HideWindows();
        AoWCombatMap = (TAoWCombatMap)AoWHSMap.FCombatMap;
        MWindow.MapWindow.Visible = AoWCombatMap.FPlaySounds;
        MWindow.FWorldAlignWidth = MWindow.MapWindow.Alignment.AlignWidth;
        MWindow.FWorldAlignHeight = MWindow.MapWindow.Alignment.AlignHeight;
        MWindow.FWorldRect.Left = MWindow.MapWindow.Alignment.LeftOffset;
        MWindow.FWorldRect.Top = MWindow.MapWindow.Alignment.TopOffset;
        if (MWindow.FWorldAlignWidth == TAOWAlignWidth.awBoth)
        {
            MWindow.FWorldRect.Right = MWindow.MapWindow.Alignment.RightOffset;
        }
        else
        {
            MWindow.FWorldRect.Right = MWindow.MapWindow.WinWidth;
        }

        if (MWindow.FWorldAlignHeight == TAOWAlignHeight.ahBoth)
        {
            MWindow.FWorldRect.Bottom = MWindow.MapWindow.Alignment.BottomOffset;
        }
        else
        {
            MWindow.FWorldRect.Bottom = MWindow.MapWindow.WinHeight;
        }

        MWindow.MapWindow.Alignment.LeftOffset = MWindow.FMapRect.Left;
        MWindow.MapWindow.Alignment.TopOffset = MWindow.FMapRect.Top;
        MWindow.MapWindow.Alignment.RightOffset = MWindow.FMapRect.Right;
        MWindow.MapWindow.Alignment.BottomOffset = MWindow.FMapRect.Bottom;
        MWindow.MapWindow.WinWidth = MWindow.FMapRect.Right;
        MWindow.MapWindow.WinHeight = MWindow.FMapRect.Bottom;
        MWindow.MapWindow.Alignment.AlignWidth = MWindow.FMapAlignWidth;
        MWindow.MapWindow.Alignment.AlignHeight = MWindow.FMapAlignHeight;
        MWindow.MapWindow.FAligned = false;
        TBWindow.TBWorld.Visible = false;
        TBWindow.TBCombat.Visible = true;
        TBWindow.UpdateCombatPlayerIndicator();
        TBWindow.AutoBtn.Checked = false;
        TBWindow.TCChatBtn.Visible = TBWindow.ChatBtn.Visible;
        TBWindow.TCUnitBtn.Visible = false;
        TBWindow.TaskWindow.FDrawn = false;
        TCScanner.TCScannerWin.Visible = AoWCombatMap.FPlaySounds;
        TBWindow.TCScannerBtn.Visible = AoWCombatMap.FPlaySounds;
        TCUnit.TCUnitWin.Visible = false;
        if (!AoWCombatMap.FPlaySounds)
        {
            FChatHeight = ChatWindow.ChatWin.WinHeight;
            ChatWindow.ChatWin.Alignment.AlignHeight = TAOWAlignHeight.ahBoth;
        }
        else
        {
            FChatHeight = 0;
        }

        if (General.FChatVisible || !AoWCombatMap.FPlaySounds)
        {
            ChatWindow.ChatWin.Visible = true;
        }
    }
}
