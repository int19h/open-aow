#nullable disable warnings
// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Draw;

public sealed class TacticalBattleItemHexSprite(CustomNode? owner) : ItemHexSprite(owner)
{

    // AoWTC.pas:2586
    public override void Deactivate()
    {
        Items.SetChangedHandler(null);
        if (CombatMap.Current.Combat.ExplorationSite == 1)
        {
            // An item site's combat returns the items to the site's list. A dungeon's combat holds
            // its prisoners there, and the items go to the prisoners' own item list.
            // An item site stores an item list. A dungeon stores a prisoners army.
            // Another object fails. Null stays null.
            ItemList? items = CombatMap.Current.Combat.Items switch
            {
                null => null,
                PrisonersArmy prisoners => prisoners.Items,
                var other => (ItemList)other,
            };
            for (int i = Items.GetCount() - 1; i >= 0; i--)
            {
                Items.GetItem(i).SetOwner(items);
            }
        }
        else
        {
            ItemHexSprite itemHexSprite = StrategicMap.Current.MapContainer.GetField(CombatMap.Current.Combat.CombatData.Location.X, CombatMap.Current.Combat.CombatData.Location.Y, CombatMap.Current.Combat.CombatData.Location.MapLevel).FindOwnedChild<ItemHexSprite>(ClassId);
            if (itemHexSprite is null)
            {
                itemHexSprite = new ItemHexSprite(null) { Holder = CreationHolder() };
                _ = StrategicMap.Current.PlaceHex(itemHexSprite, CombatMap.Current.Combat.CombatData.Location.X, CombatMap.Current.Combat.CombatData.Location.Y, L: CombatMap.Current.Combat.CombatData.Location.MapLevel);
            }

            for (int i = Items.GetCount() - 1; i >= 0; i--)
            {
                Items.GetItem(i).SetOwner(itemHexSprite.Items);
            }
        }

        base.Deactivate();
    }
}
