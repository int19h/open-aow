// Translated from Dungeon.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

// { ot=1 } The prisoners of a dungeon, and the items its combat leaves on the battlefield.
public partial class PrisonersArmy : Army
{
    public PrisonersArmy(ECustomNode owner) : base(owner)
    {
        Items = new ItemList(null);
    }

    // The items left on the battlefield when a dungeon combat ends. The original hands them to
    // the prisoners' own unit list, where an item has no place (the port threw at the troop cast
    // in UnitList.AddChild). They lie here instead, so the unit list holds troops only. The list
    // is not stored: no stored prisoners army holds an item.
    public ItemList Items { get; }

    // Dungeon.pas:102 frees the prisoners. The items go with them.
    public override void Dispose()
    {
        base.Dispose();
        Items?.Dispose();
    }

    // Dungeon.pas:89
    public override bool CanAddUnit(Troop troop)
    {
        bool result = base.CanAddUnit(troop);
        return result && !troop.GetTransporter() && troop.ValidTerrain(1, 4);
    }

    // intf_P2_07.inc:445

}
