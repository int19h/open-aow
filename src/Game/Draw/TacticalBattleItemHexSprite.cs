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
        ItemHexSprite ItemHS;
        ECustomNode Items;
        FItems.FOnChanged = null;
        if (CombatMap.Current.FCombat.FExplorationSite == 1)
        {
            Items = (ECustomNode)CombatMap.Current.FCombat.FItems;
            for (int I = FItems.GetCount() - 1; I >= 0; I--)
            {
                FItems.GetItem(I).SetOwner(Items);
            }
        }
        else
        {
            ItemHS = (ItemHexSprite)AoWHSMap.Current.MapContainer.GetField(CombatMap.Current.FCombat.FCombatData.FX, CombatMap.Current.FCombat.FCombatData.FY, CombatMap.Current.FCombat.FCombatData.FL).FindOwnedChild(131697);
            if (ItemHS == null)
            {
                ItemHS = new ItemHexSprite(null);
                AoWHSMap.Current.PlaceHX(ItemHS, CombatMap.Current.FCombat.FCombatData.FX, CombatMap.Current.FCombat.FCombatData.FY, CombatMap.Current.FCombat.FCombatData.FL);
            }

            for (int I = FItems.GetCount() - 1; I >= 0; I--)
            {
                FItems.GetItem(I).SetOwner(ItemHS.FItems);
            }
        }

        base.Deactivate();
    }

    // intf_P2_04.inc:216

}
