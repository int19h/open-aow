// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Units;

// The original's THeroInventory derives from TItemList. The port's inventory owns its items as
// its node children and reads them through Items. FOwner is the hero.
public partial class HeroInventory : EChangeNotifyNode, IChildList<Item>, IItemHolder
{
    /// <summary>The items in this inventory, in their stored order.</summary>
    public ItemCollection Items => new(this);

    Item IOwner<Item>.this[int index] => Items[index];

    // An inventory takes items only.
    protected sealed override bool AcceptsChild(EngineObject child) => child is Item;

    int IChildSink<Item>.AddChild(Item child) => AddChild(child);

    public event NotifyEvent<object> OnChanged;

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

    // Army.PAS:4516, the item list's release of each loaded child.
    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        if (persister.Loading)
        {
            for (int I = 0, last = Count - 1; I <= last; I++)
            {
                if (GetChild(I) != null)
                {
                    Items[I].Release();
                }
            }
        }
    }

    // Army.PAS:4546
    public override void Clear()
    {
        EngineObject child;
        for (int I = Count - 1; I >= 0; I--)
        {
            if (GetChild(I) != null)
            {
                child = Items[I];
                if (child != null)
                {
                    if (child.Owner != this)
                    {
                        child.Release();
                        DeleteChildAt(I);
                    }
                    else
                    {
                        child.SetOwner(null);
                    }
                }
            }
        }

        base.Clear();
    }

    // Army.PAS:4531
    public override void DestroyAllChildren()
    {
        Clear();
        base.DestroyAllChildren();
    }

    // Army.PAS:4519
    public override int RemoveChild(EngineObject child)
    {
        int result = base.RemoveChild(child);
        if (result != -1)
        {
            child.Release();
        }

        return result;
    }

    // Army.PAS:4525
    public override int AddChild(EngineObject child)
    {
        int result = base.AddChild(child);
        if (result != -1)
        {
            child.AddRef();
        }

        return result;
    }

    // Army.PAS:4564
    public override void Changed()
    {
        base.Changed();
        if (OnChanged != null)
        {
            OnChanged(this);
        }
    }

    // Hero.PAS:286
    public int GetFreeSlot()
    {
        int result = 0;
        while (result < Items.Count && result != 8)
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
        for (int I = 0, last = Items.Count - 1; I <= last; I++)
        {
            item = Items[I];
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
        for (int I = 0, last = Items.Count - 1; I <= last; I++)
        {
            item = Items[I];
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
        while (!result && I < Items.Count)
        {
            item = Items[I];
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
        while (!result && I < Items.Count)
        {
            item = Items[I];
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
        for (int I = 0, last = Items.Count - 1; I <= last; I++)
        {
            item = Items[I];
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
                if (I < Items.Count && I >= 0)
                {
                    result = Items.GetItem(I);
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
