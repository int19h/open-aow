// Translated from AoWSMTPDlg.pas.
using Aow.Rtl;

namespace Aow.Game.Net;

// declarations of AoWSMTPDlg, from AoW.exe
// Declaration-only field type from ims.dpl
public partial class MailMessage : Component
{
    public MailMessage(Component owner) : base(owner)
    { }

    public MailHeader FHeader;
    public string FSubject = "";
    public bool FIncludeHeader;
    public bool FEncode;
    public MailAttachmentList FAttachments;
    public MailAddressList FRecipients;

    // AoWSMTPDlg.pas:20
    public void SetBody(StringCollection Value)
    {
        /* TODO: Implement TmsMessage.SetBody. */
        throw new NotImplementedException();
    }
}
