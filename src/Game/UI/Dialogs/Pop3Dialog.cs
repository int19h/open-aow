using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class Pop3Dialog : UIModule
{
    public Pop3Dialog(UIContext ui) : base(ui)
    { }

    public Window Window;
    public ListBox FoundEmailGamesList;
    public Panel ExitPanel;
    public Panel StartExitPanel;
    public Edit LocalEmailAddressEdit;
    public Label AttachmentDirectoryLabel;

    private int _checkDelay;

    // AoWPOP3Dlg.pas:64
    public void ResetSearch()
    {
        _checkDelay = 30;
        FoundEmailGamesList.Items.Clear();
        ExitPanel.Visible = true;
        StartExitPanel.Visible = false;
    }

    // AoWPOP3Dlg.pas:73
    public void UpdateSelection()
    {
        if (FoundEmailGamesList.Items.Count != 0 && FoundEmailGamesList.Selected != -1)
        {
            StartExitPanel.Visible = true;
            ExitPanel.Visible = false;
        }
        else
        {
            StartExitPanel.Visible = false;
            ExitPanel.Visible = true;
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
        var localEmailAddress = UI.Engine.Registry.GetLocalEmailAddress();
        LocalEmailAddressEdit.Text = localEmailAddress;
        var attachmentDirectory = UI.Engine.Registry.GetAttachmentDirectory();
        var format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_attachmentDirectoryFormat), [attachmentDirectory]);
        AttachmentDirectoryLabel.Text = "(" + format + ")";
        Window.Visible = true;
    }

    // AoWPOP3Dlg.pas:130
    public void CancelButtonClick()
    {
        Window.Visible = false;
    }

    // AoWPOP3Dlg.pas:135
    public void DlgUpdate()
    {
        _checkDelay -= 1;
        if (_checkDelay < 0)
        {
            _checkDelay = UI.Engine.Registry.GetCheckEmailFrequency() * 30 * 60;
            if (_checkDelay < 30)
            {
                _checkDelay = 30;
            }

            // { Original overrides the configured interval. }
            _checkDelay = 150;
        }

        switch (_checkDelay)
        {
            case 25:
                CheckAttachments();
                break;
        }
    }

    // AoWPOP3Dlg.pas:147
    public void ExitBtnClick()
    {
        Window.Visible = false;
    }

    // AoWPOP3Dlg.pas:152
    public void FoundEmailGamesChange()
    {
        UpdateSelection();
    }

    // AoWPOP3Dlg.pas:157
    public void StartBtnClick()
    {
        if (FoundEmailGamesList.Selected != -1)
        {
            var item = FoundEmailGamesList.Items[FoundEmailGamesList.Selected];
            General.LoadGame(UI.General, UI.Engine.StartupDirectory + "EmailIn\\" + item);
            General.StartGame(UI.General);
        }
    }

    // AoWPOP3Dlg.pas:166
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    private static readonly string s_attachmentDirectoryFormat = "Attachment directory: %s";
}
