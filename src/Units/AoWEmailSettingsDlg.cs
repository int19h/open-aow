// Translated from AoWEmailSettingsDlg.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWEdit;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.AoWEmailSettingsDlg;

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
    public void OKBtnClick(TObject Sender)
    {
        AoWEngine.FRegistry.SetLocalEmailAddress(LocalEmailAddressEdit.Text);
        AoWEngine.FRegistry.SetSMTPServer(SMTPServerEdit.Text);
        AoWEngine.FRegistry.SetAttachmentDirectory(AttachmentDirectoryEdit.Text);
        Dlg.Visible = false;
    }

    // AoWEmailSettingsDlg.pas:57
    public void CancelBtnClick(TObject Sender)
    {
        Dlg.Visible = false;
    }

    // AoWEmailSettingsDlg.pas:60
    public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
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
