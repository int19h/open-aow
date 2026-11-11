// Translated from ItemExpS.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Events;

// FItemIDs, FMessages, FTitle, FLocation
public partial class ItemExplorationSiteEventLog : EventLog
{
    // ItemExpS.pas:187
    public ItemExplorationSiteEventLog(ECustomNode owner) : base(owner)
    {
        FItemIDs = new Int32List();
        FMessages = new StringList();
    }

    // ItemExpS.pas:201
    public new const int ClassID = 132085;

    public Int32List FItemIDs;
    public StringList FMessages;
    public string FTitle = "";
    public MapCoordinate FLocation;

    // ItemExpS.pas:194
    public override void Dispose()
    {
        FItemIDs?.Dispose();
        FMessages?.Dispose();
        base.Dispose();
    }

    // ItemExpS.pas:204
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStreamProc(20, FItemIDs.LoadFromStream, FItemIDs.SaveToStream);
        Stream.rwStrings(FMessages, 21);
        Stream.rwString(ref FTitle, 22);
    }

}
