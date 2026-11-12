// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

namespace Aow.Game.World.Structures;

// costs, turns left, upkeep, turns total, player, group, unit, unit production
public partial class ProductionSettings : EngineObject
{
    public ProductionSettings(ECustomNode owner) : base(owner)
    { }

    public int Gold;
    public int Mana;
    public sbyte TurnsLeft;
    public int GoldUpkeep;
    public int ManaUpkeep;
    public sbyte TurnsTotal;
    public sbyte Player;
    public int GroupId;
    public Unit Unit;
    public int UnitProductionIndex;

    // Structur.PAS:114
    public override void Dispose()
    {
        Clear();
        base.Dispose();
    }

    // Structur.PAS:120
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref Gold, 10);
        stream.rwInteger(ref Mana, 11);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref TurnsLeft), 12);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref TurnsTotal), 13);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Player), 14);
        stream.rwInteger(ref GroupId, 15);
        stream.rwInteger(ref UnitProductionIndex, 16);
        stream.rwCreateEObject(ref Unit, null, 17);
        stream.rwInteger(ref GoldUpkeep, 18);
        stream.rwInteger(ref ManaUpkeep, 19);
    }

    // Structur.PAS:135
    public override void Clear()
    {
        Troop U;
        TurnsLeft = 0;
        Gold = 0;
        Mana = 0;
        TurnsTotal = 0;
        Player = 0;
        GroupId = 0;
        UnitProductionIndex = -1;
        U = Unit;
        if (U != null)
        {
            U.Release();
            Unit = null;
        }
    }

    // Engine.pas:524

}
