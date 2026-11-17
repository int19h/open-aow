#nullable disable

using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class EmailSettingsDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Edit LocalEmailAddressEdit { get; private set; }
    public Edit SmtpServerEdit { get; private set; }
    public Edit AttachmentDirectoryEdit { get; private set; }

    // AoWEmailSettingsDlg.pas:49
    public void OKBtnClick()
    {
        UI.Engine.Registry.SetLocalEmailAddress(LocalEmailAddressEdit.Text);
        UI.Engine.Registry.SetSmtpServer(SmtpServerEdit.Text);
        UI.Engine.Registry.SetAttachmentDirectory(AttachmentDirectoryEdit.Text);
        Window.Visible = false;
    }

    // AoWEmailSettingsDlg.pas:57
    public void CancelBtnClick() => Window.Visible = false;

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
        string localEmailAddress = dialog.UI.Engine.Registry.GetLocalEmailAddress();
        dialog.LocalEmailAddressEdit.Text = localEmailAddress;
        string smtpServer = dialog.UI.Engine.Registry.GetSmtpServer();
        dialog.SmtpServerEdit.Text = smtpServer;
        string attachmentDirectory = dialog.UI.Engine.Registry.GetAttachmentDirectory();
        dialog.AttachmentDirectoryEdit.Text = attachmentDirectory;
        dialog.Window.Visible = true;
    }

}
