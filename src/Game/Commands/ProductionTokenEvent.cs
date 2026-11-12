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
    private int _reserved;

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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref Command, 20);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref X), 21);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Y), 22);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref MapLevel), 23);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Player), 24);
        stream.rwCreateEObject(ref Item, null, 25);
        stream.rwInteger(ref Index, 26);
        stream.rwInteger(ref _reserved, 27);
        stream.rwBool(ref Queue, 28);
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
