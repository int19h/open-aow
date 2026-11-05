// Translated from AoWExeForms.pas.
using Aow.Rtl;
using Aow.Units.AoWScenSetup;
using Aow.Units.AoWMessageDlg;
using Aow.Units.AoWInfoBanner;
using Aow.Units.AoWSoundDlg;
using Aow.Units.AoWVideoDlg;
using Aow.Units.AOWMultiWin;
using Aow.Units.AoWMagic;
using Aow.Units.AoWTBWindow;
using Aow.Units.AoWSpellBook;
using Aow.Units.AOWChatWin;
using Aow.Units.FontMod;
using Aow.Units.AoWConstructDlg;
using Aow.Units.AoWDisjunctionDlg;
using Aow.Units.AoWTCScanner;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AoWExeForms;

public static partial class AoWExeForms
{
    // { Shared executable form instances used by E2. These are the original global form
//   references; no forms are constructed by the byte-match host. }
    public static TScenSetup ScenSetup;
    public static TSoundDlg SoundDlg;
    public static TMultiWindow MultiWindow;
    public static TMagicWin MagicWin;
    public static TTBWindow TBWindow;
    public static TSpellBook SpellBook;
    public static TChatWindow ChatWindow;
    // { MWindow is declared in AOWMWindow. }
    public static TFontModule FontModule;
    public static TTCScanner TCScanner;
    public static TConstructDlg ConstructDlg;
    public static TDisjunctionDlg DisjunctionDlg;
    // AoWExeForms.pas:69
    public static void SetVideoMode(TVideoDialog Dialog, TScreenMode FullScreen)
    {
        if (FullScreen != Dialog.FFullScreen)
        {
            Dialog.FFullScreen = FullScreen;
            Dialog.FModeChanged = true;
        }
    }

    // AoWExeForms.pas:79
    public static void SetVideoSize(TVideoDialog Dialog, int Width, int Height)
    {
        Dialog.FDisplayWidth = Width;
        Dialog.FDisplayHeight = Height;
        if (Dialog.FFullScreen == TScreenMode.scmGame)
        {
            Dialog.FModeChanged = true;
        }
    }

    // AoWExeForms.pas:51
    // { Shared unnamed routines called by the leader setup window. }
    public static void ResetMessageDialog(TMessageDlg Dialog)
    {
        Dialog.YesNoPnl.Visible = false;
        Dialog.YesNoCancelPnl.Visible = false;
        Dialog.OKPnl.Visible = false;
        Dialog.OKCancelPnl.Visible = false;
        Dialog.MessageMemo.Items.Clear();
        Dialog.MemoPnl.Visible = true;
        Dialog.HeaderMemoPnl.Visible = false;
        var message = TranslateRStr(MessageRStr);
        Dialog.TitleLbl.Text = message;
    }

    // AoWExeForms.pas:63
    public static void SetInfoBanner(TInfoBanner Banner, string Caption, TStrings Description)
    {
        Banner.HeaderLbl.Text = Caption;
        var text = Description.Text;
        Banner.InfoMemo.Items.Text = text;
    }
}
