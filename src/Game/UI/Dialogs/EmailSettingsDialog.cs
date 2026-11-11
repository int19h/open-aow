
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

    public Window Dlg;
    public Label TitleLbl;
    public Frame BCeltL;
    public Frame TCeltL;
    public Image TCeltLineL;
    public Image AOWImage1;
    public Image AOWImage2;
    public Panel OKCancelPnl;
    public Button OKBtn;
    public Button CancelBtn;
    public Label LocalEmailAddressLbl;
    public Edit LocalEmailAddressEdit;
    public Label SMTPServerLbl;
    public Edit SMTPServerEdit;
    public Label AttachmentDirectoryLbl;
    public Edit AttachmentDirectoryEdit;
    public Button Close;
    public Image TitleIcon;
    public IvTranslator Translator;

    // AoWEmailSettingsDlg.pas:49
    public void OKBtnClick()
    {
        GameTables.AoWEngine.FRegistry.SetLocalEmailAddress(LocalEmailAddressEdit.Text);
        GameTables.AoWEngine.FRegistry.SetSMTPServer(SMTPServerEdit.Text);
        GameTables.AoWEngine.FRegistry.SetAttachmentDirectory(AttachmentDirectoryEdit.Text);
        Dlg.Visible = false;
    }

    // AoWEmailSettingsDlg.pas:57
    public void CancelBtnClick()
    {
        Dlg.Visible = false;
    }

    // AoWEmailSettingsDlg.pas:60
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowEmailSettings(EmailSettingsDialog Dialog)
    {
        var localEmailAddress = GameTables.AoWEngine.FRegistry.GetLocalEmailAddress();
        Dialog.LocalEmailAddressEdit.Text = localEmailAddress;
        var smtpServer = GameTables.AoWEngine.FRegistry.GetSMTPServer();
        Dialog.SMTPServerEdit.Text = smtpServer;
        var attachmentDirectory = GameTables.AoWEngine.FRegistry.GetAttachmentDirectory();
        Dialog.AttachmentDirectoryEdit.Text = attachmentDirectory;
        Dialog.Dlg.Visible = true;
    }

}
