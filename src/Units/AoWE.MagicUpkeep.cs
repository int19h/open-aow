using Aow.Rtl;
using Aow.Units.Engine;

namespace Aow.Units.AoWE;

public partial class TPlayerMagicControl
{
    // Spells.PAS:2400. List entries retain object identities instead of native addresses.
    public int AIOverrideUpkeep(int priority, int amount)
    {
        var list = new TQuadItemList();
        for (int i = 0; i < FEnchantments.GetCount(); i++)
        {
            var enchantment = FEnchantments.GetEnchantments(i);
            int order = enchantment.GetAIUpkeepPriority();
            if (order <= priority) list.AddABCD(order, enchantment.FUpkeep, Rtl.Rtl.ObjectToInt(enchantment), 0);
        }
        var global = AoWE.AoWHSMap.FGlobalMagicControl;
        for (int i = global.FEnchantments.GetCount() - 1; i >= 0; i--)
        {
            var enchantment = AoWE.AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(i);
            if (enchantment.FPlayerIndex != FPlayer.FPlayerIndex) continue;
            int order = enchantment.GetAIUpkeepPriority();
            if (order <= priority) list.AddABCD(order, enchantment.GetUpkeep(), 0, Rtl.Rtl.ObjectToInt(enchantment));
        }
        if (list.Count != 0)
        {
            int total = 0;
            list.SortAscendingItemA();
            while (total < amount && list.Count != 0 && FPlayer.FGameOverStatus != TGameOverStatus.goLost)
            {
                var item = list[0];
                total += item.B;
                if (item.C != 0)
                    ExecuteDispelEnchantment(FEnchantments.FChildren.IndexOf(Rtl.Rtl.ReferenceFromId<TEnchantment>(item.C)));
                else
                {
                    var enchantment = Rtl.Rtl.ReferenceFromId<TGlobalEnchantment>(item.D)!;
                    enchantment.AddRef();
                    enchantment.Remove(unchecked((byte)FPlayer.FPlayerIndex));
                    enchantment.Release();
                }
                list.Delete(0);
            }
        }
        list.Free();
        return 0;
    }
}
