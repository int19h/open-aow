// Translated from ItemExpS.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Events;

// ItemIds, Messages, Title, Location
public partial class ItemExplorationSiteEventLog : EventLog
{
    // ItemExpS.pas:187
    public ItemExplorationSiteEventLog(ECustomNode owner) : base(owner)
    {
        ItemIds = new Int32List();
        Messages = new StringList();
    }

    // ItemExpS.pas:201
    public new const int ClassId = 132085;

    public Int32List ItemIds;
    public StringList Messages;
    public string Title = "";
    public MapCoordinate Location;

    // ItemExpS.pas:194
    public override void Dispose()
    {
        ItemIds?.Dispose();
        Messages?.Dispose();
        base.Dispose();
    }

    // ItemExpS.pas:204
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwStreamProc(20, ItemIds.LoadFromStream, ItemIds.SaveToStream);
        stream.rwStrings(Messages, 21);
        stream.rwString(ref Title, 22);
    }

}
