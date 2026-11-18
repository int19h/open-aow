// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Events;

// message
public partial class SmtpEventLog(CustomNode owner) : EventLog(owner)
{

    public string ToAddress { get; } = "";
    public string FileName { get; } = "";
    public string Subject { get; } = "";
    public DelphiStringList Message { get; } = new DelphiStringList();
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
