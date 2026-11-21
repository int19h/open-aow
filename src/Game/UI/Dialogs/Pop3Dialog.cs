#nullable disable
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public sealed partial class Pop3Dialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public ListBox FoundEmailGamesList { get; private set; }
    public Panel ExitPanel { get; private set; }
    public Panel StartExitPanel { get; private set; }
    public Edit LocalEmailAddressEdit { get; private set; }
    public Label AttachmentDirectoryLabel { get; private set; }

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
        /* TODO: Implement Windows.CreateFile in the platform or editor service. */
        => throw new NotImplementedException();

    // AoWPOP3Dlg.pas:121
    public void ShowDialog()
    {
        ResetSearch();
        string localEmailAddress = UI.Engine.Registry.GetLocalEmailAddress();
        LocalEmailAddressEdit.Text = localEmailAddress;
        string attachmentDirectory = UI.Engine.Registry.GetAttachmentDirectory();
        string format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_attachmentDirectoryFormat), arguments: [attachmentDirectory]);
        AttachmentDirectoryLabel.Text = $"({format})";
        Window.Visible = true;
    }

    // AoWPOP3Dlg.pas:130
    public void CancelButtonClick() => Window.Visible = false;

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
                _ = CheckAttachments();
                break;
            default:
                break;
        }
    }

    // AoWPOP3Dlg.pas:147
    public void ExitBtnClick() => Window.Visible = false;

    // AoWPOP3Dlg.pas:152
    public void FoundEmailGamesChange() => UpdateSelection();

    // AoWPOP3Dlg.pas:157
    public void StartBtnClick()
    {
        if (FoundEmailGamesList.Selected != -1)
        {
            string item = FoundEmailGamesList.Items[FoundEmailGamesList.Selected];
            General.LoadGame(UI.General, name: $"{UI.Engine.StartupDirectory}EmailIn\\{item}");
            General.StartGame(UI.General);
        }
    }

    // AoWPOP3Dlg.pas:166
    public void DlgKeyDown(in ushort key, ShiftStates shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    private static readonly string s_attachmentDirectoryFormat = "Attachment directory: %s";
}
