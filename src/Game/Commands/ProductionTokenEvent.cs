#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Net;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Commands;

// location/player, command, item, queue, index
public partial class ProductionTokenEvent(CustomNode owner) : TokenEvent(owner)
{
    public new const int ClassId = 131736;

    public MapCoordinate Location { get; internal set; }

    public sbyte Player { get; set; }
    public int Command { get; set; }
    public ProductionItem Item { get; set; }
    public bool Queue { get; set; }
    public int Index { get; set; }

    // Structur.PAS:155
    // { Original leaves the upkeep fields unchanged. }
    public override void Dispose()
    {
        if (Item is not null)
        {
            Item?.Dispose();
            Item = null;
        }

        base.Dispose();
    }

    // Structur.PAS:168
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Command = persister.Int32("Command", value: Command);
        Location = PersistSupport.Coordinate(persister, name: "Location", value: Location, width: CoordinateWidth.Byte);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        Item = persister.Created("Item", objectValue: Item);
        Index = persister.Int32("Index", value: Index);
        // Delphi TProductionTE.FReserved (id 27) is always 0 (omitted) and never read.
        persister.Skip("UnknownValue");
        Queue = persister.Boolean("Queue", value: Queue);
    }

    // Structur.PAS:182
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ProductionControl control = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).GetProductionControl();
        ExecuteResult result = control is not null ? control.ExecuteProductionTokenEvent(this) : ExecuteResult.DoneHalt;

        return result;
    }

    // NetworkE.pas:230

}
