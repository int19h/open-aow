// Translated from AoWPOP3Dlg.pas.
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Dialogs;

public partial class POP3Dlg : DataModule
{
    public POP3Dlg(Component owner) : base(owner)
    { }

    public AOWWindow Dlg;
    public AOWLabel TitleLbl;
    public AoWFrame BCeltL;
    public AoWFrame TCeltL;
    public AOWImage TCeltLineL;
    public AOWImage AOWImage1;
    public AOWImage AOWImage2;
    public AOWLabel SearchingEmailGameLbl;
    public AOWLabel EmailGameFoundLbl;
    public AOWListBox FoundEmailGames;
    public AOWPanel ExitPnl;
    public AOWButton Exit2Btn;
    public AOWPanel StartExitPnl;
    public AOWButton StartBtn;
    public AOWButton ExitBtn;
    public AOWEdit LocalEmailAddressEdit;
    public AOWLabel AttachmentDirectoryLbl;
    public AOWImage TitleIcon;
    public AOWPanel EditPnl;
    public AOWPanel ModePnl;
    public AOWVScrollBar EmailGamesSB;
    public AOWButton Close;
    public IvTranslator Translator;
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
        var localEmailAddress = Magic.Abilities.GameTables.AoWEngine.FRegistry.GetLocalEmailAddress();
        LocalEmailAddressEdit.Text = localEmailAddress;
        var attachmentDirectory = Magic.Abilities.GameTables.AoWEngine.FRegistry.GetAttachmentDirectory();
        var format = Magic.Abilities.AbilityRules.Format(Magic.Abilities.AbilityRules.TranslateRStr(AttachmentDirectoryXRStr), [attachmentDirectory]);
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
            FCheckDelay = Magic.Abilities.GameTables.AoWEngine.FRegistry.GetCheckEmailFrequency() * 30 * 60;
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
            General.LoadGame(DialogHosts.General, Magic.Abilities.GameTables.AoWEngine.StartupDirectory + "EmailIn\\" + item);
            General.StartGame(DialogHosts.General);
        }
    }

    // AoWPOP3Dlg.pas:166
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    private static readonly string AttachmentDirectoryXRStr = "Attachment directory: %s";
}
