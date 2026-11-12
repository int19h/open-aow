// Translated from AoWSMTPDlg.pas.
using Aow.Rtl;

namespace Aow.Game.Net;

// declarations of AoWSMTPDlg, from AoW.exe
// Declaration-only field type from ims.dpl
public partial class MailMessage : Component
{
    public MailMessage(Component owner) : base(owner)
    { }

    public MailHeader Header;
    public string Subject = "";
    public bool IncludeHeader;
    public bool Encode;
    public MailAttachmentList Attachments;
    public MailAddressList Recipients;

    // AoWSMTPDlg.pas:20
    public void SetBody(StringCollection value)
    {
        /* TODO: Implement TmsMessage.SetBody. */
        throw new NotImplementedException();
    }
}
