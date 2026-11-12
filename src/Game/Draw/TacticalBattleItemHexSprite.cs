// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

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
        ECustomNode items;
        Items.OnChanged = null;
        if (CombatMap.Current.Combat.ExplorationSite == 1)
        {
            items = (ECustomNode)CombatMap.Current.Combat.Items;
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
