// Translated from AoWEmailSettingsDlg.pas.
using Aow.Game.Engine.Buffers;
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

public partial class TEmailSettingsDlg : TDataModule
{
    public TEmailSettingsDlg(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dlg;
    public TAOWLabel TitleLbl;
    public TAoWFrame BCeltL;
    public TAoWFrame TCeltL;
    public TAOWImage TCeltLineL;
    public TAOWImage AOWImage1;
    public TAOWImage AOWImage2;
    public TAOWPanel OKCancelPnl;
    public TAOWButton OKBtn;
    public TAOWButton CancelBtn;
    public TAOWLabel LocalEmailAddressLbl;
    public TAOWEdit LocalEmailAddressEdit;
    public TAOWLabel SMTPServerLbl;
    public TAOWEdit SMTPServerEdit;
    public TAOWLabel AttachmentDirectoryLbl;
    public TAOWEdit AttachmentDirectoryEdit;
    public TAOWButton Close;
    public TAOWImage TitleIcon;
    public TIvTranslator Translator;

    // AoWEmailSettingsDlg.pas:49
    public void OKBtnClick()
    {
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetLocalEmailAddress(LocalEmailAddressEdit.Text);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetSMTPServer(SMTPServerEdit.Text);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetAttachmentDirectory(AttachmentDirectoryEdit.Text);
        Dlg.Visible = false;
    }

    // AoWEmailSettingsDlg.pas:57
    public void CancelBtnClick()
    {
        Dlg.Visible = false;
    }

    // AoWEmailSettingsDlg.pas:60
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowEmailSettings(TEmailSettingsDlg Dialog)
    {
        var localEmailAddress = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetLocalEmailAddress();
        Dialog.LocalEmailAddressEdit.Text = localEmailAddress;
        var smtpServer = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetSMTPServer();
        Dialog.SMTPServerEdit.Text = smtpServer;
        var attachmentDirectory = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetAttachmentDirectory();
        Dialog.AttachmentDirectoryEdit.Text = attachmentDirectory;
        Dialog.Dlg.Visible = true;
    }

}
