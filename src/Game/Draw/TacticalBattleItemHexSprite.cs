// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Draw;

public partial class TacticalBattleItemHexSprite : ItemHexSprite
{

    public TacticalBattleItemHexSprite(ECustomNode owner) : base(owner)
    { }

    // AoWTC.pas:2586
    public override void Deactivate()
    {
        ItemHexSprite itemHexSprite;
        ItemList items;
        Items.OnChanged = null;
        if (CombatMap.Current.Combat.ExplorationSite == 1)
        {
            // An item site's combat returns the items to the site's list. A dungeon's combat holds
            // its prisoners there, and the items go to the prisoners' own item list.
            items = CombatMap.Current.Combat.Items switch
            {
                PrisonersArmy prisoners => prisoners.Items,
                var other => (ItemList)other,
            };
            for (int I = Items.GetCount() - 1; I >= 0; I--)
            {
                Items.GetItem(I).SetOwner(items);
            }
        }
        else
        {
            itemHexSprite = (ItemHexSprite)AoWHSMap.Current.MapContainer.GetField(CombatMap.Current.Combat.CombatData.X, CombatMap.Current.Combat.CombatData.Y, CombatMap.Current.Combat.CombatData.MapLevel).FindOwnedChild(131697);
            if (itemHexSprite == null)
            {
                itemHexSprite = new ItemHexSprite(null);
                AoWHSMap.Current.PlaceHex(itemHexSprite, CombatMap.Current.Combat.CombatData.X, CombatMap.Current.Combat.CombatData.Y, CombatMap.Current.Combat.CombatData.MapLevel);
            }

            for (int I = Items.GetCount() - 1; I >= 0; I--)
            {
                Items.GetItem(I).SetOwner(itemHexSprite.Items);
            }
        }

        base.Deactivate();
    }

    // intf_P2_04.inc:216

}
