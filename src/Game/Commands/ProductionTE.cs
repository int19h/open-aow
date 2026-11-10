// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Commands;

// location/player, command, item, queue, index
public partial class ProductionTE : TokenEvent
{
    public ProductionTE(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 131736;

    public sbyte FX;
    public sbyte FY;
    public sbyte FL;
    public sbyte FPlayer;
    public int FCommand;
    public ProductionItem FItem;
    public bool FQueue;
    public int FIndex;
    public int FReserved;

    // Structur.PAS:155
    // { Original leaves the upkeep fields unchanged. }
    public override void Dispose()
    {
        if (FItem != null)
        {
            FItem?.Dispose();
            FItem = null;
        }

        base.Dispose();
    }

    // Structur.PAS:168
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FCommand, 20);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FX), 21);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FY), 22);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FL), 23);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 24);
        Stream.rwCreateEObject(ref FItem, null, 25);
        Stream.rwInteger(ref FIndex, 26);
        Stream.rwInteger(ref FReserved, 27);
        Stream.rwBool(ref FQueue, 28);
    }

    // Structur.PAS:182
    public override ExecuteResult Execute(TokenExecuter TE)
    {
        ExecuteResult Result;
        ProductionControl Control;
        Control = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(FX, FY, FL)).GetProductionControl();
        if (Control != null)
        {
            Result = Control.ExecuteProductionTE(this);
        }
        else
        {
            Result = ExecuteResult.DoneHalt;
        }

        return Result;
    }

    // NetworkE.pas:230

}
