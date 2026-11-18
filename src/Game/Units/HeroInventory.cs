#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Units;

// The original's THeroInventory derives from TItemList. The port's inventory owns its items as
// its node children and reads them through Items. FOwner is the hero.
public partial class HeroInventory : ObservableNode, IChildList<Item>, IItemHolder
{
    /// <summary>The items in this inventory, in their stored order.</summary>
    public ItemCollection Items => new(this);

    Item IOwner<Item>.this[int index] => Items[index];

    // An inventory takes items only.
    protected sealed override bool AcceptsChild(EngineObject child) => child is Item;

    int IChildSink<Item>.AddChild(Item child) => AddChild(child);

    public event NotifyEvent<object> OnChanged;

    // Hero.PAS:274
    public HeroInventory(CustomNode owner) : base(null) => SetOwnerWithoutNotify(owner);

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
            for (int i = 0, last = Count - 1; i <= last; i++)
            {
                if (GetChild(i) != null)
                {
                    _ = Items[i].Release();
                }
            }
        }
    }

    // Army.PAS:4546
    public override void Clear()
    {
        EngineObject child;
        for (int i = Count - 1; i >= 0; i--)
        {
            if (GetChild(i) != null)
            {
                child = Items[i];
                if (child != null)
                {
                    if (child.Owner != this)
                    {
                        _ = child.Release();
                        DeleteChildAt(i);
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
            _ = child.Release();
        }

        return result;
    }

    // Army.PAS:4525
    public override int AddChild(EngineObject child)
    {
        int result = base.AddChild(child);
        if (result != -1)
        {
            _ = child.AddRef();
        }

        return result;
    }

    // Army.PAS:4564
    protected override void OnChange()
    {
        base.OnChange();
        OnChanged?.Invoke(this);
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
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            item = Items[i];
            if (item != null && result < item.GetOwnAbilityCount())
            {
                result = item.GetOwnAbilityCount();
            }
        }

        return result;
    }

    // Hero.PAS:318
    public AbilityOwner GetSuperlativeEnabledAbilityOwner(int abilityId)
    {
        Item item;
        AbilityOwner result = null;
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            item = Items[i];
            if (item != null && item.GetOwnAbilityEnabled(abilityId) && item.ItemType == ItemType.Use)
            {
                if (result == null || item.GetOwnAbilityLevel(abilityId) > result.GetOwnAbilityLevel(abilityId))
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
        int i;
        Item item;
        bool result = false;
        i = 0;
        while (!result && i < Items.Count)
        {
            item = Items[i];
            result = item != null && item.ItemType == ItemType.Use && item.GetOwnAbilityEnabled(abilityId);
            i += 1;
        }

        return result;
    }

    // Hero.PAS:344
    public bool GetAbilitySet(int abilityId)
    {
        int i;
        Item item;
        bool result = false;
        i = 0;
        while (!result && i < Items.Count)
        {
            item = Items[i];
            result = item != null && item.ItemType == ItemType.Use && item.GetAbilitySetAt(abilityId);
            i += 1;
        }

        return result;
    }

    // Hero.PAS:357
    public AbilityOwner GetSuperlativeAbilityOwner(int abilityId)
    {
        Item item;
        AbilityOwner result = null;
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            item = Items[i];
            if (item != null && item.GetAbilitySetAt(abilityId) && item.ItemType == ItemType.Use)
            {
                if (result == null || item.GetOwnAbilityLevel(abilityId) > result.GetOwnAbilityLevel(abilityId))
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
        int i;
        switch (position)
        {
            case HeroItemPosition.InventorySlot1:
            case HeroItemPosition.InventorySlot2:
            case HeroItemPosition.InventorySlot3:
            case HeroItemPosition.InventorySlot4:
            case HeroItemPosition.InventorySlot5:
            case HeroItemPosition.InventorySlot6:
            case HeroItemPosition.InventorySlot7:
            case HeroItemPosition.InventorySlot8:
                i = (int)position - (int)HeroItemPosition.InventorySlot1;
                if (i < Items.Count && i >= 0)
                {
                    result = Items.GetItem(i);
                }
                else
                {
                    return null;
                }

                break;
            case HeroItemPosition.AttackSlot:
            case HeroItemPosition.HeadSlot:
            case HeroItemPosition.DefenseSlot:
            case HeroItemPosition.LeftRingSlot:
            case HeroItemPosition.TorsoSlot:
            case HeroItemPosition.RightRingSlot:
            case HeroItemPosition.Ground:
            case HeroItemPosition.None:
            default:
                result = null;
                break;
        }

        return result;
    }

    // intf_P2_06.inc:8

}
