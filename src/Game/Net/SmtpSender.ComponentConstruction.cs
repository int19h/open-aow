// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;

namespace Aow.Game.Net;

public partial class SmtpSender
{
    public int DefaultPort { get; set; }
    public SmtpError<SmtpSender> OnError { get; set; }
    public SmtpProgress<SmtpSender> OnProgress { get; set; }
}
