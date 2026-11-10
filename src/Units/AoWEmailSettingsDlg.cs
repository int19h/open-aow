using Aow.Game.Host;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Game.World.Structures;
using Aow.Rtl;
// Translated from AoWEmailSettingsDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

public partial class EmailSettingsDlg : DataModule
{
    public EmailSettingsDlg(Component owner) : base(owner)
    { }

    public AOWWindow Dlg;
    public AOWLabel TitleLbl;
    public AoWFrame BCeltL;
    public AoWFrame TCeltL;
    public AOWImage TCeltLineL;
    public AOWImage AOWImage1;
    public AOWImage AOWImage2;
    public AOWPanel OKCancelPnl;
    public AOWButton OKBtn;
    public AOWButton CancelBtn;
    public AOWLabel LocalEmailAddressLbl;
    public AOWEdit LocalEmailAddressEdit;
    public AOWLabel SMTPServerLbl;
    public AOWEdit SMTPServerEdit;
    public AOWLabel AttachmentDirectoryLbl;
    public AOWEdit AttachmentDirectoryEdit;
    public AOWButton Close;
    public AOWImage TitleIcon;
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

    internal static void ShowEmailSettings(EmailSettingsDlg Dialog)
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
