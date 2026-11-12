// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Events;

// message
public partial class SmtpEventLog : EventLog
{
    // Diplomacy.pas:2441
    public SmtpEventLog(ECustomNode owner) : base(owner)
    {
        Message = new StringList();
    }

    public string ToAddress { get; } = "";
    public string FileName { get; } = "";
    public string Subject { get; } = "";
    public StringList Message { get; }
    public bool Success { get; set; }
    public string FromAddress { get; } = "";
    public string Error { get; set; } = "";

    // Diplomacy.pas:2447
    public override void Dispose()
    {
        Message?.Dispose();
        base.Dispose();
    }

}
