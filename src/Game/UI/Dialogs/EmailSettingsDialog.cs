
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class EmailSettingsDialog : DataModule
{
    public EmailSettingsDialog(Component owner) : base(owner)
    { }

    public Window Window;
    public Edit LocalEmailAddressEdit;
    public Edit SmtpServerEdit;
    public Edit AttachmentDirectoryEdit;

    // AoWEmailSettingsDlg.pas:49
    public void OKBtnClick()
    {
        GameTables.GameEngine.Registry.SetLocalEmailAddress(LocalEmailAddressEdit.Text);
        GameTables.GameEngine.Registry.SetSmtpServer(SmtpServerEdit.Text);
        GameTables.GameEngine.Registry.SetAttachmentDirectory(AttachmentDirectoryEdit.Text);
        Window.Visible = false;
    }

    // AoWEmailSettingsDlg.pas:57
    public void CancelBtnClick()
    {
        Window.Visible = false;
    }

    // AoWEmailSettingsDlg.pas:60
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    internal static void ShowEmailSettings(EmailSettingsDialog dialog)
    {
        var localEmailAddress = GameTables.GameEngine.Registry.GetLocalEmailAddress();
        dialog.LocalEmailAddressEdit.Text = localEmailAddress;
        var smtpServer = GameTables.GameEngine.Registry.GetSmtpServer();
        dialog.SmtpServerEdit.Text = smtpServer;
        var attachmentDirectory = GameTables.GameEngine.Registry.GetAttachmentDirectory();
        dialog.AttachmentDirectoryEdit.Text = attachmentDirectory;
        dialog.Window.Visible = true;
    }

}
