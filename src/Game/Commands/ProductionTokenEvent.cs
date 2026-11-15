#nullable disable
// Translated from AoWE.pas.
using System.Runtime.CompilerServices;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.Net;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Commands;

// location/player, command, item, queue, index
public partial class ProductionTokenEvent : TokenEvent
{
    public ProductionTokenEvent(CustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131736;

    public sbyte X { get; internal set; }
    public sbyte Y { get; internal set; }
    public sbyte MapLevel { get; internal set; }

    public sbyte Player { get; set; }
    public int Command { get; set; }
    public ProductionItem Item { get; set; }
    public bool Queue { get; set; }
    public int Index { get; set; }

    // An integer of unknown meaning that the original stores at id 27 and never reads. It round
    // trips unchanged.
    private int _unknownValue;

    // Structur.PAS:155
    // { Original leaves the upkeep fields unchanged. }
    public override void Dispose()
    {
        if (Item != null)
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
        X = PersistSupport.UnsignedByte(persister, name: "X", value: X);
        Y = PersistSupport.UnsignedByte(persister, name: "Y", value: Y);
        MapLevel = PersistSupport.UnsignedByte(persister, name: "MapLevel", value: MapLevel);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        Item = persister.Created("Item", objectValue: Item);
        Index = persister.Int32("Index", value: Index);
        _unknownValue = persister.Int32("UnknownValue", value: _unknownValue);
        Queue = persister.Boolean("Queue", value: Queue);
    }

    // Structur.PAS:182
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result;
        ProductionControl control;
        control = StrategicMap.Current.MapContainer.GetField(X, Y, MapLevel).GetProductionControl();
        if (control != null)
        {
            result = control.ExecuteProductionTokenEvent(this);
        }
        else
        {
            result = ExecuteResult.DoneHalt;
        }

        return result;
    }

    // NetworkE.pas:230

}
