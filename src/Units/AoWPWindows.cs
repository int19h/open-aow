// Translated from AoWPWindows.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AOWMenu;
using static Aow.Units.AOWMenu.AOWMenu;
using Aow.Units.StartMenu;
using static Aow.Units.StartMenu.StartMenu;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;
using Aow.Units.AoWSoundDlg;
using static Aow.Units.AoWSoundDlg.AoWSoundDlg;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.E2Globals;
using static Aow.Units.E2Globals.E2Globals;
using Aow.Units.E2GameFlow;
using static Aow.Units.E2GameFlow.E2GameFlow;
using Aow.Units.E1Globals;
using static Aow.Units.E1Globals.E1Globals;
using Aow.Units.E1GameFlow;
using static Aow.Units.E1GameFlow.E1GameFlow;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.aowHelpWin;
using static Aow.Units.aowHelpWin.aowHelpWin;
using Aow.Units.AoWGamePlayDlg;
using static Aow.Units.AoWGamePlayDlg.AoWGamePlayDlg;
using Aow.Units.AoWFileDialog;
using static Aow.Units.AoWFileDialog.AoWFileDialog;
using Aow.Units.E3EventDialogs;
using static Aow.Units.E3EventDialogs.E3EventDialogs;

namespace Aow.Units.AoWPWindows
{
    // published menu components
    public partial class TPopupWindows : Aow.Rtl.TDataModule
    {
        public TPopupWindows(TComponent owner) : base(owner)
        { }

        public Aow.Units.StartMenu.TStartMenu StartMenu;
        public Aow.Units.AOWMenu.TAOWMenu OverViewsMenu;
        public Aow.Units.AOWMenu.TAOWMenu SettingsMenu;
        public Aow.Units.AOWMenu.TAOWMenu EndMenu;
        public Aow.Units.AOWMenu.TAOWMenu DiskMenu;
        public Aow.Units.StartMenu.TStartMenu TCStartMenu;
        public Aow.Rtl.TIvTranslator Translator;
        public Aow.Units.AOWMenu.TAOWMenu HelpMenu;
        public Aow.Units.AOWMenu.TAOWMenu TCOVMenu;
        public Aow.Units.AOWMenu.TAOWMenu TCDiskMenu;
        public Aow.Units.AOWMenu.TAOWMenu TCHelpMenu;
        public Aow.Units.AOWMenu.TAOWMenu TCSettingsMenu;
        public Aow.Units.AOWMenu.TAOWMenu TCEndMenu;
        // AoWPWindows.pas:61
        public void SettingsMenuSelect(Aow.Units.AOWMenu.TAOWMenu Sender)
        {
            switch (Sender.FSelectedItem)
            {
                case 0:
                    Aow.Units.AoWVideoDlg.AoWVideoDlg.VideoDialog.ShowModes();
                    break;
                case 1:
                    Aow.Units.AoWPWindows.AoWPWindows.ShowSoundSettings(Aow.Units.AoWExeForms.AoWExeForms.SoundDlg);
                    break;
                case 2:
                    Aow.Units.E2Globals.E2Globals.GamePlayDialog.GamePlayDlg.Visible = true;
                    break;
                case 3:
                    Aow.Units.AoWWinMan.AoWWinMan.General.ResetLayout();
                    break;
                case 4:
                    Aow.Units.E3Globals.E3Globals.ShowEmailSettings(Aow.Units.E3Globals.E3Globals.EmailSettingsDlg);
                    break;
                case 5:
                    Aow.Units.E2Globals.E2Globals.DialogWin.DebugWin.Visible = true;
                    break;
            }
        }

        // AoWPWindows.pas:83
        public void DiskMenuSelect(Aow.Units.AOWMenu.TAOWMenu Sender)
        {
            switch (Sender.FSelectedItem)
            {
                case 0:
                    Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FMode = 0;
                    Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FileDialog.Visible = true;
                    break;
                case 1:
                    Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FMode = 1;
                    Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FileDialog.Visible = true;
                    break;
            }
        }

        // AoWPWindows.pas:91
        public void OverViewsMenuSelect(Aow.Units.AOWMenu.TAOWMenu Sender)
        {
            switch (Sender.FSelectedItem)
            {
                case 0:
                    // { Original opens the information book. }
                    Aow.Units.E3EventDialogs.E3EventDialogs.ShowInformationBook(Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap, null, null);
                    break;
                case 1:
                    if (Aow.Units.AoWE.AoWE.AoWEngine.FCampaign != null)
                    {
                        Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap.ShowCampaignHistory();
                    }

                    break;
            }
        }

        // AoWPWindows.pas:99
        public void TCEndMenuSelect(Aow.Units.AOWMenu.TAOWMenu Sender)
        {
            switch (Sender.FSelectedItem)
            {
                case 0:
                    var call__1 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.QuitCombatRStr);
                    var call__2 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.ConfirmRStr);
                    Aow.Units.E2Globals.E2Globals.DialogWin.Confirm(call__2, call__1, 6);
                    break;
            }
        }

        // AoWPWindows.pas:107
        public void HelpMenuSelect(Aow.Units.AOWMenu.TAOWMenu Sender)
        {
            /* TODO: Implement Forms.Handle in the platform or editor service. */
            throw new System.NotImplementedException();
        }
    }

    public static partial class AoWPWindows
    {
        // AoWPWindows.pas:47
        public static void ShowVideoSettings(Aow.Units.AoWVideoDlg.TVideoDialog Dialog)
        {
            Dialog.Dlg.Visible = true;
            Dialog.UpdateModes();
        }

        // AoWPWindows.pas:53
        public static void ShowSoundSettings(Aow.Units.AoWSoundDlg.TSoundDlg Dialog)
        {
            Dialog.AddBtn.Visible = false;
            Dialog.FindFiles();
            Dialog.SoundSetup();
            Dialog.Dlg.Visible = true;
        }
    }
}
