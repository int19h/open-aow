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
        FMessage = new StringList();
    }

    public string FToAddress = "";
    public string FFileName = "";
    public string FSubject = "";
    public StringList FMessage;
    public bool FSuccess;
    public string FFromAddress = "";
    public string FError = "";

    // Diplomacy.pas:2447
    public override void Dispose()
    {
        FMessage?.Dispose();
        base.Dispose();
    }

}
