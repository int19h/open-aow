// Translated from ItemExpS.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Events;

// ItemIds, Messages, Title, Location
public partial class ItemExplorationSiteEventLog(CustomNode owner) : EventLog(owner)
{

    // ItemExpS.pas:201
    public new const int ClassId = 132085;

    public Int32List ItemIds { get; } = new Int32List();
    public DelphiStringList Messages { get; } = new DelphiStringList();

    public string Title { get; set; } = "";

    public MapCoordinate Location { get; internal init; }

    // ItemExpS.pas:194
    public override void Dispose()
    {
        ItemIds?.Dispose();
        Messages?.Dispose();
        base.Dispose();
    }

    // ItemExpS.pas:204
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.Int32List(persister, name: "ItemIds", list: ItemIds);
        PersistSupport.Lines(persister, name: "Messages", lines: Messages);
        Title = persister.String("Title", value: Title);
    }

}
