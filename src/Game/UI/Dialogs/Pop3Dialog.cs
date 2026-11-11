using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class Pop3Dialog : DataModule
{
    public Pop3Dialog(Component owner) : base(owner)
    { }

    public Window Dlg;
    public Label TitleLbl;
    public Frame BCeltL;
    public Frame TCeltL;
    public Image TCeltLineL;
    public Image AOWImage1;
    public Image AOWImage2;
    public Label SearchingEmailGameLbl;
    public Label EmailGameFoundLbl;
    public ListBox FoundEmailGames;
    public Panel ExitPnl;
    public Button Exit2Btn;
    public Panel StartExitPnl;
    public Button StartBtn;
    public Button ExitBtn;
    public Edit LocalEmailAddressEdit;
    public Label AttachmentDirectoryLbl;
    public Image TitleIcon;
    public Panel EditPnl;
    public Panel ModePnl;
    public VerticalScrollBar EmailGamesSB;
    public Button Close;
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
        var localEmailAddress = GameTables.AoWEngine.FRegistry.GetLocalEmailAddress();
        LocalEmailAddressEdit.Text = localEmailAddress;
        var attachmentDirectory = GameTables.AoWEngine.FRegistry.GetAttachmentDirectory();
        var format = AbilityRules.Format(AbilityRules.TranslateRStr(AttachmentDirectoryXRStr), [attachmentDirectory]);
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
            FCheckDelay = GameTables.AoWEngine.FRegistry.GetCheckEmailFrequency() * 30 * 60;
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
            General.LoadGame(DialogHosts.General, GameTables.AoWEngine.StartupDirectory + "EmailIn\\" + item);
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
