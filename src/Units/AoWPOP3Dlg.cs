// Translated from AoWPOP3Dlg.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWEdit;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWPOP3Dlg;

public partial class TPOP3Dlg : TDataModule
{
    public TPOP3Dlg(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dlg;
    public TAOWLabel TitleLbl;
    public TAoWFrame BCeltL;
    public TAoWFrame TCeltL;
    public TAOWImage TCeltLineL;
    public TAOWImage AOWImage1;
    public TAOWImage AOWImage2;
    public TAOWLabel SearchingEmailGameLbl;
    public TAOWLabel EmailGameFoundLbl;
    public TAOWListBox FoundEmailGames;
    public TAOWPanel ExitPnl;
    public TAOWButton Exit2Btn;
    public TAOWPanel StartExitPnl;
    public TAOWButton StartBtn;
    public TAOWButton ExitBtn;
    public TAOWEdit LocalEmailAddressEdit;
    public TAOWLabel AttachmentDirectoryLbl;
    public TAOWImage TitleIcon;
    public TAOWPanel EditPnl;
    public TAOWPanel ModePnl;
    public TAOWVScrollBar EmailGamesSB;
    public TAOWButton Close;
    public TIvTranslator Translator;
    public bool FSearching;
    public int FCheckDelay;
    // AoWPOP3Dlg.pas:64
    public void ResetSearch()
    {
        FCheckDelay = 30;
        FSearching = false;
        FoundEmailGames.Items.Clear();
        ExitPnl.Visible = true;
        StartExitPnl.Visible = false;
    }

    // AoWPOP3Dlg.pas:73
    public void UpdateSelection()
    {
        if (FoundEmailGames.Items.Count != 0 && FoundEmailGames.Selected != -1)
        {
            StartExitPnl.Visible = true;
            ExitPnl.Visible = false;
        }
        else
        {
            StartExitPnl.Visible = false;
            ExitPnl.Visible = true;
        }
    }

    // AoWPOP3Dlg.pas:87
    public int CheckAttachments()
    {
        /* TODO: Implement Windows.CreateFile in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AoWPOP3Dlg.pas:121
    public void ShowDialog()
    {
        ResetSearch();
        var localEmailAddress = AoWEngine.FRegistry.GetLocalEmailAddress();
        LocalEmailAddressEdit.Text = localEmailAddress;
        var attachmentDirectory = AoWEngine.FRegistry.GetAttachmentDirectory();
        var format = Format(TranslateRStr(AttachmentDirectoryXRStr), [attachmentDirectory]);
        AttachmentDirectoryLbl.Text = "(" + format + ")";
        Dlg.Visible = true;
    }

    // AoWPOP3Dlg.pas:130
    public void CancelBtnClick()
    {
        Dlg.Visible = false;
    }

    // AoWPOP3Dlg.pas:135
    public void DlgUpdate()
    {
        FCheckDelay -= 1;
        if (FCheckDelay < 0)
        {
            FCheckDelay = AoWEngine.FRegistry.GetCheckEmailFrequency() * 30 * 60;
            if (FCheckDelay < 30)
            {
                FCheckDelay = 30;
            }

            // { Original overrides the configured interval. }
            FCheckDelay = 150;
        }

        switch (FCheckDelay)
        {
            case 25:
                CheckAttachments();
                break;
        }
    }

    // AoWPOP3Dlg.pas:147
    public void ExitBtnClick()
    {
        Dlg.Visible = false;
    }

    // AoWPOP3Dlg.pas:152
    public void FoundEmailGamesChange()
    {
        UpdateSelection();
    }

    // AoWPOP3Dlg.pas:157
    public void StartBtnClick()
    {
        if (FoundEmailGames.Selected != -1)
        {
            var item = FoundEmailGames.Items[FoundEmailGames.Selected];
            LoadGame(General, AoWEngine.StartupDirectory + "EmailIn\\" + item);
            StartGame(General);
        }
    }

    // AoWPOP3Dlg.pas:166
    public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }
}

public static partial class AoWPOP3Dlg
{
}
