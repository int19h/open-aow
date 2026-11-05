#nullable disable
using static Aow.Units.AOWMWindow.AOWMWindowUnit;
using static Aow.Units.AoWSWindow.AoWSWindowUnit;
using static Aow.Units.AoWRealm.AoWRealmUnit;
using static Aow.Units.AoWPInfoWin.AoWPInfoWinUnit;
using static Aow.Units.AoWVideoDlg.AoWVideoDlgUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.Sound.SoundUnit;
using static Aow.Units.E1Resources.E1ResourcesUnit;
using Aow.Units.AoWE;
using Aow.Units.HSEngine;
using Aow.Rtl;
using Aow.Units.E1Globals;
using Aow.Units.E2Globals;
using Aow.Units.aowSetupEvents;
using Aow.Units.aowMapEvents;

namespace Aow.Units.AoWWinMan;

public static partial class AoWWinManUnit
{
    // AoWWinMan.pas:559. The offline transport supplies Connected = false.
    public static void StartGame(TGeneral controller)
    {
        controller.FWindowsHidden = false;
        SongPlayer.Songs.Clear();
        if (AoWHSMap.FGameSongs.Songs.Count != 0) SongPlayer.Songs.AddStrings(AoWHSMap.FGameSongs.Songs);
        else if (AoWHSMap.FStartupSongs.Songs.Count != 0)
        {
            for (int i = 0; i < AoWHSMap.FStartupSongs.Songs.Count; i++)
                if (RtlUnit.CompareText(AoWHSMap.FStartupSongs.SongNames[i], TranslateRStr(E1FileNotFoundRStr)) != 0)
                    SongPlayer.Songs.Add(AoWHSMap.FStartupSongs.Songs[i]);
        }
        else SongPlayer.Songs.Add("Songs\\Heartwoo.it");
        SongPlayer.Play(0);
        SetMusicVolume(SoundDlg.MusicVolume.Position);
        var menu = E1GlobalsUnit.PopupWindows.OverViewsMenu.Items;
        menu.Clear();
        menu.Add(TranslateRStr(E1BackgroundRStr));
        if (AoWEngine.FCampaign != null) menu.Add(TranslateRStr(E1CampaignRStr));
        E2GlobalsUnit.TitleScene.Window.Visible = false;
        E2GlobalsUnit.TitleWindow.TitleWin.Visible = false;
        TBWindow.TaskWindow.Visible = true;
        MWindow.MapWindow.Visible = true;
        SWindow.ScannerWindow.Visible = true;
        MWindow.MapViewer.HSMap = AoWHSMap;
        var scanner = SWindow.Scanner;
        scanner.SetMap(AoWHSMap);
        scanner.SetLevel(AoWHSMap.MapContainer.GetMapLevel(0));
        TBWindow.TurnButton.FButtonChecked = false;
        TBWindow.ChatBtn.Visible = aowSetupEventsUnit.SetupEvents.DPlay.Connected;
        General.Manager.FAligned = false;
        scanner.FPreviousWidth = 0;
        scanner.FIncremental = false;
        scanner.FSized = false;
        var tabs = SWindow.ScannerTab;
        tabs.Items.Clear();
        tabs.Items.Add(TranslateRStr(E1SurfaceRStr));
        if (AoWHSMap.MapContainer.LevelCount > 1) tabs.Items.Add(TranslateRStr(E1CavernsRStr));
        if (AoWHSMap.MapContainer.LevelCount > 2) tabs.Items.Add(TranslateRStr(E1DepthsRStr));
        tabs.Selected = 0;
        tabs.FSized = false;
        scanner.SetTWidth(0);
        scanner.SetTHeight(0);
        scanner.SetTLeft(-1);
        scanner.SetTTop(-1);
        scanner.FUpdateStep = -1;
        scanner.SetMap(AoWHSMap);
        scanner.SetLevel(AoWHSMap.MapContainer.GetMapLevel(0));
        MWindow.MapViewer.HSMap = AoWHSMap;
        MWindow.MapViewer.SceneWidth = MWindow.MapWindow.WinWidth - 24;
        MWindow.MapViewer.SceneHeight = MWindow.MapWindow.WinHeight - 24;
        AoWHSMap.Activate();
        RealmWin.Dlg.Visible = false;
        aowMapEventsUnit.MapEvents.TheMapArmyDeselected(null);
        aowMapEventsUnit.MapEvents.TheMapStructureDeselected(null);
        // The original call at targets the indicator sizing routine
        TBWindow.UpdatePlayerIndicator();
        PInfoW.UpdatePlayer();
        if (AoWHSMap.Status.Contains(THSMapStatusElement.msActivated))
        {
            var turn = TBWindow.TurnButton;
            if (AoWHSMap.FSeatedPlayer != -1)
            {
                turn.FLastPlayer = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FPlayerRace;
                if (turn.FLastPlayer == -1) turn.FLastPlayer = 12;
                turn.FCurPlayer = turn.FLastPlayer;
                turn.FAnimationFrame = turn.FCurPlayer * 7;
                TBWindow.CurP.Visible = AoWHSMap.FPlayMode == TPlayMode.pmTurn;
            }
            else
            {
                turn.FCurPlayer = 12;
                turn.FLastPlayer = turn.FCurPlayer;
                turn.FAnimationFrame = turn.FCurPlayer * 7;
                TBWindow.CurP.Visible = false;
            }
        }
        TBWindow.FWorldClockDelay = 0;
        TBWindow.ChatBtn.ILIndexUp = 14;
        TBWindow.FChatFlash = 0;
        VideoDialog.SetSceneSize(VideoDialog.FDisplayWidth, VideoDialog.FDisplayHeight);
    }
}
