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
        FOwner = owner;
    }

    // Hero.PAS:280
    public override void Dispose()
    {
        FOwner = null;
        base.Dispose();
    }

    // Hero.PAS:297
    public override Set<ControlStyleElement> GetControlStyle() => base.GetControlStyle() + Set<ControlStyleElement>.Of(ControlStyleElement.ArrayMode);

    // Hero.PAS:286
    public int GetFreeSlot()
    {
        int Result = 0;
        while (Result < FChildren.Count && Result != 8)
        {
            if (FChildren.List[Result] == null)
            {
                return Result;
            }

            Result += 1;
        }

        return -1;
    }

    // Hero.PAS:307
    public int GetAbilityCount()
    {
        Item Item;
        int Result = 0;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            Item = (Item)FChildren.List[I];
            if (Item != null && Result < Item.GetAbCount())
            {
                Result = Item.GetAbCount();
            }
        }

        return Result;
    }

    // Hero.PAS:318
    public AbilityOwner GetSuperlativeEnabledAbOwner(int AbilityID)
    {
        Item Item;
        AbilityOwner Result = null;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            Item = (Item)FChildren.List[I];
            if (Item != null && Item.GetAbEnabled(AbilityID) && Item.FItemType == ItemType.Use)
            {
                if (Result == null || Item.GetAbLevel(AbilityID) > Result.GetAbLevel(AbilityID))
                {
                    Result = Item;
                }
            }
        }

        return Result;
    }

    // Hero.PAS:331
    public bool GetAbilityEnabled(int AbilityID)
    {
        int I;
        Item Item;
        bool Result = false;
        I = 0;
        while (!Result && I < Count)
        {
            Item = (Item)FChildren.List[I];
            Result = Item != null && Item.FItemType == ItemType.Use && Item.GetAbEnabled(AbilityID);
            I += 1;
        }

        return Result;
    }

    // Hero.PAS:344
    public bool GetAbilitySet(int AbilityID)
    {
        int I;
        Item Item;
        bool Result = false;
        I = 0;
        while (!Result && I < Count)
        {
            Item = (Item)FChildren.List[I];
            Result = Item != null && Item.FItemType == ItemType.Use && Item.GetAbSet(AbilityID);
            I += 1;
        }

        return Result;
    }

    // Hero.PAS:357
    public AbilityOwner GetSuperlativeAbOwner(int AbilityID)
    {
        Item Item;
        AbilityOwner Result = null;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            Item = (Item)FChildren.List[I];
            if (Item != null && Item.GetAbSet(AbilityID) && Item.FItemType == ItemType.Use)
            {
                if (Result == null || Item.GetAbLevel(AbilityID) > Result.GetAbLevel(AbilityID))
                {
                    Result = Item;
                }
            }
        }

        return Result;
    }

    // Hero.PAS:370
    public Item GetSlot(HeroItemPosition Position)
    {
        Item Result;
        int I;
        switch (Position)
        {
            case >= HeroItemPosition.InventorySlot1 and <= HeroItemPosition.InventorySlot8:
                I = (int)Position - (int)HeroItemPosition.InventorySlot1;
                if (I < Count && I >= 0)
                {
                    Result = GetItem(I);
                }
                else
                {
                    return null;
                }

                break;
            default:
                Result = null;
                break;
        }

        return Result;
    }

    // intf_P2_06.inc:8

}
