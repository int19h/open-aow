// Translated from AoWSMTPDlg.pas.
using Aow.Rtl;

namespace Aow.Game.Net;

// Declaration-only field type from ims.dpl
public partial class SmtpSender : Component
{
    public SmtpSender(Component owner) : base(owner)
    { }

    public string Host { get; set; } = "";
    public MailMessage Message { get; set; }

    // AoWSMTPDlg.pas:29
    public void SetWinsockLoaded(bool value)
    {
        /* TODO: Implement TmsSMTP.SetWinsockLoaded. */
        throw new NotImplementedException();
    }

    public bool WinsockLoaded { set => SetWinsockLoaded(value); }

    // AoWSMTPDlg.pas:30
    public void Send()
    {
        /* TODO: Implement TmsSMTP.Send. */
        throw new NotImplementedException();
    }
}
