#nullable disable warnings
// Translated from Dungeon.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

// { ot=1 } The prisoners of a dungeon, and the items its combat leaves on the battlefield.
public sealed class PrisonersArmy : Army
{
    public PrisonersArmy(CustomNode? owner) : base(owner) => Items = new ItemList(null) { Holder = CreationHolder() };

    // The items left on the battlefield when a dungeon combat ends. The original hands them to
    // the prisoners' own unit list, where an item has no place (the port threw at the troop cast
    // in UnitList.AddChild). They lie here instead, so the unit list holds troops only. An empty
    // list is absent from a save. A list that holds an item is port-only state at property id
    // 0x40000000. The own document keeps it and the original image refuses it.
    public ItemList Items { get; }

    // Dungeon.pas:102 frees the prisoners. The items go with them.
    public override void Dispose()
    {
        base.Dispose();
        Items?.Dispose();
    }

    // Dungeon.pas:89
    public override bool CanAddUnit(Troop? troop)
    {
        bool result = base.CanAddUnit(troop);
        return result && !troop.GetTransporter() && troop.ValidTerrain(1, overlayType: 4);
    }

    // An empty list is not state. A load still reads the member, and absence leaves the list empty.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        if (persister.Loading || Items.Count > 0)
        {
            persister.Object("Items", objectValue: Items);
        }
    }
}
