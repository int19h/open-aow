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

    public int FGold;
    public int FMana;
    public sbyte FTurnsLeft;
    public int FGoldUpkeep;
    public int FManaUpkeep;
    public sbyte FTurnsTotal;
    public sbyte FPlayer;
    public int FGroupID;
    public Unit FUnit;
    public int FUnitProductionIndex;

    // Structur.PAS:114
    public override void Dispose()
    {
        Clear();
        base.Dispose();
    }

    // Structur.PAS:120
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FGold, 10);
        Stream.rwInteger(ref FMana, 11);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FTurnsLeft), 12);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FTurnsTotal), 13);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 14);
        Stream.rwInteger(ref FGroupID, 15);
        Stream.rwInteger(ref FUnitProductionIndex, 16);
        Stream.rwCreateEObject(ref FUnit, null, 17);
        Stream.rwInteger(ref FGoldUpkeep, 18);
        Stream.rwInteger(ref FManaUpkeep, 19);
    }

    // Structur.PAS:135
    public override void Clear()
    {
        Troop U;
        FTurnsLeft = 0;
        FGold = 0;
        FMana = 0;
        FTurnsTotal = 0;
        FPlayer = 0;
        FGroupID = 0;
        FUnitProductionIndex = -1;
        U = FUnit;
        if (U != null)
        {
            U.Release();
            FUnit = null;
        }
    }

    // Engine.pas:524

}
