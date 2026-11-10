// Translated from AoWEmailSettingsDlg.pas.
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWEmailSettingsDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Game.Magic.Abilities.AoWEUnit;

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
        AoWEngine.FRegistry.SetLocalEmailAddress(LocalEmailAddressEdit.Text);
        AoWEngine.FRegistry.SetSMTPServer(SMTPServerEdit.Text);
        AoWEngine.FRegistry.SetAttachmentDirectory(AttachmentDirectoryEdit.Text);
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
}

public static partial class AoWEmailSettingsDlgUnit
{
}
