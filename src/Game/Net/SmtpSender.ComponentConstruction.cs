// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;

namespace Aow.Game.Net;

public partial class SmtpSender
{
    public int DefaultPort { get; set; }

    // Nothing in the port raises OnError. The bound handler is kept.
    private SmtpError<SmtpSender> _onError;

    public event SmtpError<SmtpSender> OnError { add => _onError += value; remove => _onError -= value; }

    // Nothing in the port raises OnProgress. The bound handler is kept.
    private SmtpProgress<SmtpSender> _onProgress;

    public event SmtpProgress<SmtpSender> OnProgress { add => _onProgress += value; remove => _onProgress -= value; }
}
