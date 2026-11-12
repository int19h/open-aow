// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Units;

// { Interface declaration run owned by P2. }
// inherited FOwner is the hero. FChildren holds inventory slots
public partial class HeroInventory : ItemList
{
    // Hero.PAS:274
    public HeroInventory(ECustomNode owner) : base(null)
    {
        SetOwnerWithoutNotify(owner);
    }

    // Hero.PAS:280
    public override void Dispose()
    {
        SetOwnerWithoutNotify(null);
        base.Dispose();
    }

    // Hero.PAS:297
    public override Set<ControlStyleElement> GetControlStyle() => base.GetControlStyle() + Set<ControlStyleElement>.Of(ControlStyleElement.ArrayMode);

    // Hero.PAS:286
    public int GetFreeSlot()
    {
        int result = 0;
        while (result < Count && result != 8)
        {
            if (GetChild(result) == null)
            {
                return result;
            }

            result += 1;
        }

        return -1;
    }

    // Hero.PAS:307
    public int GetAbilityCount()
    {
        Item item;
        int result = 0;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            item = this[I];
            if (item != null && result < item.GetAbCount())
            {
                result = item.GetAbCount();
            }
        }

        return result;
    }

    // Hero.PAS:318
    public AbilityOwner GetSuperlativeEnabledAbilityOwner(int abilityId)
    {
        Item item;
        AbilityOwner result = null;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            item = this[I];
            if (item != null && item.GetAbEnabled(abilityId) && item.ItemType == ItemType.Use)
            {
                if (result == null || item.GetAbLevel(abilityId) > result.GetAbLevel(abilityId))
                {
                    result = item;
                }
            }
        }

        return result;
    }

    // Hero.PAS:331
    public bool GetAbilityEnabled(int abilityId)
    {
        int I;
        Item item;
        bool result = false;
        I = 0;
        while (!result && I < Count)
        {
            item = this[I];
            result = item != null && item.ItemType == ItemType.Use && item.GetAbEnabled(abilityId);
            I += 1;
        }

        return result;
    }

    // Hero.PAS:344
    public bool GetAbilitySet(int abilityId)
    {
        int I;
        Item item;
        bool result = false;
        I = 0;
        while (!result && I < Count)
        {
            item = this[I];
            result = item != null && item.ItemType == ItemType.Use && item.GetAbilitySetAt(abilityId);
            I += 1;
        }

        return result;
    }

    // Hero.PAS:357
    public AbilityOwner GetSuperlativeAbilityOwner(int abilityId)
    {
        Item item;
        AbilityOwner result = null;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            item = this[I];
            if (item != null && item.GetAbilitySetAt(abilityId) && item.ItemType == ItemType.Use)
            {
                if (result == null || item.GetAbLevel(abilityId) > result.GetAbLevel(abilityId))
                {
                    result = item;
                }
            }
        }

        return result;
    }

    // Hero.PAS:370
    public Item GetSlot(HeroItemPosition position)
    {
        Item result;
        int I;
        switch (position)
        {
            case >= HeroItemPosition.InventorySlot1 and <= HeroItemPosition.InventorySlot8:
                I = (int)position - (int)HeroItemPosition.InventorySlot1;
                if (I < Count && I >= 0)
                {
                    result = GetItem(I);
                }
                else
                {
                    return null;
                }

                break;
            default:
                result = null;
                break;
        }

        return result;
    }

    // intf_P2_06.inc:8

}
