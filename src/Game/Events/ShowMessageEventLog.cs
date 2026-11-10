// Translated from EventLog.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.Events;

// message
public partial class ShowMessageEventLog : EventLog
{
    // EventLog.pas:759
    public ShowMessageEventLog(ECustomNode owner) : base(owner)
    {
        FMessage = new StringList();
    }

    // EventLog.pas:765
    public new const int ClassID = 131830;

    public StringList FMessage;

    // EventLog.pas:762
    public override void Dispose()
    {
        FMessage?.Dispose();
        base.Dispose();
    }

    // EventLog.pas:768
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStrings(FMessage, 20);
    }

}
