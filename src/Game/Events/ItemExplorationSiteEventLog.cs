// Translated from ItemExpS.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

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

    public Int32List ItemIds { get; }
    public StringList Messages { get; }

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
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Payload(20, ItemIds.LoadFromStream, ItemIds.SaveToStream);
        stream.Lines(Messages, 21);
        stream.String(ref Title, 22);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.Int32List(persister, name: "ItemIds", list: ItemIds);
        PersistSupport.Lines(persister, name: "Messages", lines: Messages);
        Title = persister.String("Title", value: Title);
    }

}
