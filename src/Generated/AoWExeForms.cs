// Generated from AoWExeForms.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.AoWScenSetup;
using static Aow.Units.AoWScenSetup.AoWScenSetup;
using Aow.Units.aowIntGfxMod;
using static Aow.Units.aowIntGfxMod.aowIntGfxMod;
using Aow.Units.AoWMessageDlg;
using static Aow.Units.AoWMessageDlg.AoWMessageDlg;
using Aow.Units.AoWInfoBanner;
using static Aow.Units.AoWInfoBanner.AoWInfoBanner;
using Aow.Units.AoWSoundDlg;
using static Aow.Units.AoWSoundDlg.AoWSoundDlg;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;
using Aow.Units.AOWMultiWin;
using static Aow.Units.AOWMultiWin.AOWMultiWin;
using Aow.Units.AoWSWindow;
using static Aow.Units.AoWSWindow.AoWSWindow;
using Aow.Units.AoWPInfoWin;
using static Aow.Units.AoWPInfoWin.AoWPInfoWin;
using Aow.Units.AoWUnitWindow;
using static Aow.Units.AoWUnitWindow.AoWUnitWindow;
using Aow.Units.AoWRealm;
using static Aow.Units.AoWRealm.AoWRealm;
using Aow.Units.AoWMagic;
using static Aow.Units.AoWMagic.AoWMagic;
using Aow.Units.AOWControlWin;
using static Aow.Units.AOWControlWin.AOWControlWin;
using Aow.Units.AoWTBWindow;
using static Aow.Units.AoWTBWindow.AoWTBWindow;
using Aow.Units.AoWInfoWin;
using static Aow.Units.AoWInfoWin.AoWInfoWin;
using Aow.Units.AoWSpellBook;
using static Aow.Units.AoWSpellBook.AoWSpellBook;
using Aow.Units.AOWChatWin;
using static Aow.Units.AOWChatWin.AOWChatWin;
using Aow.Units.AOWMWindow;
using static Aow.Units.AOWMWindow.AOWMWindow;
using Aow.Units.FontMod;
using static Aow.Units.FontMod.FontMod;
using Aow.Units.AoWConstructDlg;
using static Aow.Units.AoWConstructDlg.AoWConstructDlg;
using Aow.Units.AoWDisjunctionDlg;
using static Aow.Units.AoWDisjunctionDlg.AoWDisjunctionDlg;
using Aow.Units.AoWTCUnit;
using static Aow.Units.AoWTCUnit.AoWTCUnit;
using Aow.Units.AoWTCScanner;
using static Aow.Units.AoWTCScanner.AoWTCScanner;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;

namespace Aow.Units.AoWExeForms
{
    public static partial class AoWExeForms
    {
        // { Shared executable form instances used by E2. These are the original global form
//   references; no forms are constructed by the byte-match host. }
        public static Aow.Units.AoWScenSetup.TScenSetup ScenSetup;
        // { $45B068 }
        public static Aow.Units.AoWSoundDlg.TSoundDlg SoundDlg;
        // { $45B07C }
        public static Aow.Units.AOWMultiWin.TMultiWindow MultiWindow;
        // { $45B240 }
        public static Aow.Units.AoWMagic.TMagicWin MagicWin;
        // { $45B054; 00404F58 }
        public static Aow.Units.AoWTBWindow.TTBWindow TBWindow;
        public static Aow.Units.AoWSpellBook.TSpellBook SpellBook;
        public static Aow.Units.AOWChatWin.TChatWindow ChatWindow;
        // { MWindow is declared in AOWMWindow. }
        public static Aow.Units.FontMod.TFontModule FontModule;
        // { $45B1A8 }
        public static Aow.Units.AoWTCScanner.TTCScanner TCScanner;
        public static Aow.Units.AoWConstructDlg.TConstructDlg ConstructDlg;
        public static Aow.Units.AoWDisjunctionDlg.TDisjunctionDlg DisjunctionDlg;
        // AoWExeForms.pas:69
        public static void SetVideoMode(Aow.Units.AoWVideoDlg.TVideoDialog Dialog, Aow.Units.AoWVideoDlg.TScreenMode FullScreen)
        {
            // { conflict: procedure SetVideoMode(Dialog: TVideoDialog; FullScreen: Byte); }
            if (FullScreen != Dialog.FFullScreen)
            {
                Dialog.FFullScreen = FullScreen;
                Dialog.FModeChanged = true;
            }
        }

        // AoWExeForms.pas:79
        // { conflict: procedure SetVideoMode(Dialog: TVideoDialog; FullScreen: Byte); }
        public static void SetVideoSize(Aow.Units.AoWVideoDlg.TVideoDialog Dialog, int Width, int Height)
        {
            Dialog.FDisplayWidth = Width;
            Dialog.FDisplayHeight = Height;
            if (Dialog.FFullScreen == Aow.Units.AoWVideoDlg.TScreenMode.scmGame)
            {
                Dialog.FModeChanged = true;
            }
        }

        // AoWExeForms.pas:51
        // { Shared unnamed routines called by the leader setup window. }
        public static void ResetMessageDialog(Aow.Units.AoWMessageDlg.TMessageDlg Dialog)
        {
            Dialog.YesNoPnl.Visible = false;
            Dialog.YesNoCancelPnl.Visible = false;
            Dialog.OKPnl.Visible = false;
            Dialog.OKCancelPnl.Visible = false;
            Dialog.MessageMemo.Items.Clear();
            Dialog.MemoPnl.Visible = true;
            Dialog.HeaderMemoPnl.Visible = false;
            var call__1 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MessageRStr);
            Dialog.TitleLbl.Text = call__1;
        }

        // AoWExeForms.pas:63
        public static void SetInfoBanner(Aow.Units.AoWInfoBanner.TInfoBanner Banner, Aow.Rtl.AnsiString Caption, Aow.Rtl.TStrings Description)
        {
            Banner.HeaderLbl.Text = Caption;
            var call__2 = Description.Text;
            Banner.InfoMemo.Items.Text = call__2;
        }
    }
}
