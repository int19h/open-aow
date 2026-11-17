#nullable disable
// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Draw;

public partial class TacticalBattleItemHexSprite : ItemHexSprite
{

    public TacticalBattleItemHexSprite(CustomNode owner) : base(owner)
    { }

    // AoWTC.pas:2586
    public override void Deactivate()
    {
        ItemHexSprite itemHexSprite;
        ItemList items;
        Items.ClearChangedHandler();
        if (CombatMap.Current.Combat.ExplorationSite == 1)
        {
            // An item site's combat returns the items to the site's list. A dungeon's combat holds
            // its prisoners there, and the items go to the prisoners' own item list.
            // An item site stores an item list. A dungeon stores a prisoners army.
            // Another object fails. Null stays null.
            items = CombatMap.Current.Combat.Items switch
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
            itemHexSprite = StrategicMap.Current.MapContainer.GetField(CombatMap.Current.Combat.CombatData.Location.X, CombatMap.Current.Combat.CombatData.Location.Y, CombatMap.Current.Combat.CombatData.Location.MapLevel).FindOwnedChild<ItemHexSprite>(131697);
            if (itemHexSprite == null)
            {
                itemHexSprite = new ItemHexSprite(null) { Holder = CreationHolder() };
                _ = StrategicMap.Current.PlaceHex(itemHexSprite, CombatMap.Current.Combat.CombatData.Location.X, CombatMap.Current.Combat.CombatData.Location.Y, CombatMap.Current.Combat.CombatData.Location.MapLevel);
            }

            for (int i = Items.GetCount() - 1; i >= 0; i--)
            {
                Items.GetItem(i).SetOwner(itemHexSprite.Items);
            }
        }

        base.Deactivate();
    }

    // intf_P2_04.inc:216

}
