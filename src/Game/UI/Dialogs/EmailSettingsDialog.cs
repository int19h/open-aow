
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class EmailSettingsDialog : UIModule
{
    public EmailSettingsDialog(UIContext ui) : base(ui)
    { }

    public Window Window;
    public Edit LocalEmailAddressEdit;
    public Edit SmtpServerEdit;
    public Edit AttachmentDirectoryEdit;

    // AoWEmailSettingsDlg.pas:49
    public void OKBtnClick()
    {
        UI.Engine.Registry.SetLocalEmailAddress(LocalEmailAddressEdit.Text);
        UI.Engine.Registry.SetSmtpServer(SmtpServerEdit.Text);
        UI.Engine.Registry.SetAttachmentDirectory(AttachmentDirectoryEdit.Text);
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
        var localEmailAddress = dialog.UI.Engine.Registry.GetLocalEmailAddress();
        dialog.LocalEmailAddressEdit.Text = localEmailAddress;
        var smtpServer = dialog.UI.Engine.Registry.GetSmtpServer();
        dialog.SmtpServerEdit.Text = smtpServer;
        var attachmentDirectory = dialog.UI.Engine.Registry.GetAttachmentDirectory();
        dialog.AttachmentDirectoryEdit.Text = attachmentDirectory;
        dialog.Window.Visible = true;
    }

}
