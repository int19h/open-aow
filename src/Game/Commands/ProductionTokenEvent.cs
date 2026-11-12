// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Commands;

// location/player, command, item, queue, index
public partial class ProductionTokenEvent : TokenEvent
{
    public ProductionTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131736;

    public sbyte X;
    public sbyte Y;
    public sbyte MapLevel;
    public sbyte Player;
    public int Command;
    public ProductionItem Item;
    public bool Queue;
    public int Index;
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
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref Command, 20);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref X), 21);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Y), 22);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref MapLevel), 23);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Player), 24);
        stream.Created(ref Item, null, 25);
        stream.Int32(ref Index, 26);
        stream.Int32(ref _unknownValue, 27);
        stream.Boolean(ref Queue, 28);
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
