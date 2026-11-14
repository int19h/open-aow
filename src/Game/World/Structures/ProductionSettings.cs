// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

namespace Aow.Game.World.Structures;

// costs, turns left, upkeep, turns total, player, group, unit, unit production
public partial class ProductionSettings : EngineObject
{
    public ProductionSettings(ECustomNode owner) : base(owner)
    { }

    public int Gold { get; set; }
    public int Mana { get; set; }
    public sbyte TurnsLeft { get; set; }
    public int GoldUpkeep { get; set; }
    public int ManaUpkeep { get; set; }
    public sbyte TurnsTotal { get; set; }
    public sbyte Player { get; set; }
    public int GroupId { get; set; }
    public Unit Unit { get; set; }
    public int UnitProductionIndex { get; set; }

    // Structur.PAS:114
    public override void Dispose()
    {
        Clear();
        base.Dispose();
    }

    // Structur.PAS:120
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Gold = persister.Int32("Gold", value: Gold);
        Mana = persister.Int32("Mana", value: Mana);
        TurnsLeft = PersistSupport.UnsignedByte(persister, name: "TurnsLeft", value: TurnsLeft);
        TurnsTotal = PersistSupport.UnsignedByte(persister, name: "TurnsTotal", value: TurnsTotal);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        GroupId = persister.Int32("GroupId", value: GroupId);
        UnitProductionIndex = persister.Int32("UnitProductionIndex", value: UnitProductionIndex);
        Unit = persister.Created("Unit", objectValue: Unit);
        GoldUpkeep = persister.Int32("GoldUpkeep", value: GoldUpkeep);
        ManaUpkeep = persister.Int32("ManaUpkeep", value: ManaUpkeep);
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
