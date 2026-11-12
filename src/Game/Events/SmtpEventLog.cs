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

    public string ToAddress = "";
    public string FileName = "";
    public string Subject = "";
    public StringList Message;
    public bool Success;
    public string FromAddress = "";
    public string Error = "";

    // Diplomacy.pas:2447
    public override void Dispose()
    {
        Message?.Dispose();
        base.Dispose();
    }

}
