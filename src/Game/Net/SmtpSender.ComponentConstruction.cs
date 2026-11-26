// Layout translated from recovered Delphi forms.
#nullable disable warnings
using System.Diagnostics.CodeAnalysis;

namespace Aow.Game.Net;

public sealed partial class SmtpSender
{
    public int DefaultPort { get; set; }

    // Nothing in the port raises OnError. The bound handler is kept.
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event OnError keeps this backing field until the port raises it.")]
    private EventHandler<SmtpErrorEventArgs> _onError;

    public event EventHandler<SmtpErrorEventArgs> Failed { add => _onError += value; remove => _onError -= value; }

    // Nothing in the port raises OnProgress. The bound handler is kept.
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event OnProgress keeps this backing field until the port raises it.")]
    private EventHandler<SmtpProgressEventArgs> _onProgress;

    public event EventHandler<SmtpProgressEventArgs> ProgressChanged { add => _onProgress += value; remove => _onProgress -= value; }
}
