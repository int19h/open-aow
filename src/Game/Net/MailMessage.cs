// Translated from AoWSMTPDlg.pas.
using Aow.Rtl;

namespace Aow.Game.Net;

// declarations of AoWSMTPDlg, from AoW.exe
// Declaration-only field type from ims.dpl
public partial class MailMessage : Component
{
    public MailMessage(Component owner) : base(owner)
    { }

    public MailHeader Header { get; }
    public string Subject { get; set; } = "";
    public bool IncludeHeader { get; set; }
    public bool Encode { get; set; }
    public MailAttachmentList Attachments { get; }
    public MailAddressList Recipients { get; }

    // AoWSMTPDlg.pas:20
    public void SetBody(StringCollection value)
    {
        /* TODO: Implement TmsMessage.SetBody. */
        throw new NotImplementedException();
    }
}
