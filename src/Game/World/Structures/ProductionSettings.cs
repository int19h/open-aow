#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Rtl;

namespace Aow.Game.World.Structures;

// costs, turns left, upkeep, turns total, player, group, unit, unit production
public sealed class ProductionSettings(CustomNode? owner) : EngineObject(owner)
{
    public int Gold { get; set; }
    public int Mana { get; set; }
    public int TurnsLeft { get; set; }
    public int GoldUpkeep { get; set; }
    public int ManaUpkeep { get; set; }
    public int TurnsTotal { get; set; }
    public sbyte Player { get; set; }
    public int GroupId { get; set; }
    public Unit? Unit { get; set; }
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
        TurnsLeft = persister.UnsignedByte(name: "TurnsLeft", value: TurnsLeft.TruncateToSByte());
        TurnsTotal = persister.UnsignedByte(name: "TurnsTotal", value: TurnsTotal.TruncateToSByte());
        Player = persister.UnsignedByte(name: "Player", value: Player);
        GroupId = persister.Int32("GroupId", value: GroupId);
        UnitProductionIndex = persister.Int32("UnitProductionIndex", value: UnitProductionIndex);
        Unit = persister.Created("Unit", objectValue: Unit);
        GoldUpkeep = persister.Int32("GoldUpkeep", value: GoldUpkeep);
        ManaUpkeep = persister.Int32("ManaUpkeep", value: ManaUpkeep);
    }

    // Structur.PAS:135
    public override void Clear()
    {
        TurnsLeft = 0;
        Gold = 0;
        Mana = 0;
        TurnsTotal = 0;
        Player = 0;
        GroupId = 0;
        UnitProductionIndex = -1;
        Troop? u = Unit;
        if (u is not null)
        {
            _ = u.Release();
            Unit = null;
        }
    }
}
