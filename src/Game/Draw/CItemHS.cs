// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.World.Map;

namespace Aow.Game.Draw;

public partial class CItemHS : ItemHS
{

    public CItemHS(ECustomNode owner) : base(owner)
    { }

    // AoWTC.pas:2586
    public override void Deactivate()
    {
        ItemHS ItemHS;
        ECustomNode Items;
        FItems.FOnChanged = null;
        if (AoWCombatMap.Current.FCombat.FExplorationSite == 1)
        {
            Items = (ECustomNode)AoWCombatMap.Current.FCombat.FItems;
            for (int I = FItems.GetCount() - 1; I >= 0; I--)
            {
                FItems.GetItem(I).SetOwner(Items);
            }
        }
        else
        {
            ItemHS = (ItemHS)AoWHSMap.Current.MapContainer.GetField(AoWCombatMap.Current.FCombat.FCombatData.FX, AoWCombatMap.Current.FCombat.FCombatData.FY, AoWCombatMap.Current.FCombat.FCombatData.FL).FindOwnedChild(131697);
            if (ItemHS == null)
            {
                ItemHS = new ItemHS(null);
                AoWHSMap.Current.PlaceHX(ItemHS, AoWCombatMap.Current.FCombat.FCombatData.FX, AoWCombatMap.Current.FCombat.FCombatData.FY, AoWCombatMap.Current.FCombat.FCombatData.FL);
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
