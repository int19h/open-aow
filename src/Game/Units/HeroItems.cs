#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Units;

// The original's THeroItems derives from TItemList. The items a hero wears own the equipped
// items as node children and read them through Items. FOwner is the hero.
public partial class HeroItems : ObservableNode, IChildList<Item>, IItemHolder
{
    /// <summary>The items this hero wears, in their stored order.</summary>
    public ItemCollection Items => new(this);

    Item IOwner<Item>.this[int index] => Items[index];

    // Worn items take items only.
    protected sealed override bool AcceptsChild(EngineObject child) => child is Item;

    int IChildSink<Item>.AddChild(Item child) => AddChild(child);

    public event NotifyEvent<object> OnChanged;

    // Hero.PAS:388
    public HeroItems(CustomNode owner) : base(null) => SetOwnerWithoutNotify(owner);

    // Hero.PAS:394
    public override void Dispose()
    {
        SetOwnerWithoutNotify(null);
        base.Dispose();
    }

    // Hero.PAS:400
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
                    Items[i].Release();
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
                        child.Release();
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
    protected override void OnChange()
    {
        base.OnChange();
        OnChanged?.Invoke(this);
    }

    // Hero.PAS:410
    public sbyte GetAttack()
    {
        Item item;
        sbyte result = 0;
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            item = Items[i];
            if (item != null)
            {
                result = (sbyte)(result + item.Attack);
            }
        }

        return result;
    }

    // Hero.PAS:421
    public sbyte GetDefense()
    {
        Item item;
        sbyte result = 0;
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            item = Items[i];
            if (item != null)
            {
                result = (sbyte)(result + item.Defense);
            }
        }

        return result;
    }

    // Hero.PAS:432
    public sbyte GetDamage()
    {
        Item item;
        sbyte result = 0;
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            item = Items[i];
            if (item != null)
            {
                result = (sbyte)(result + item.Damage);
            }
        }

        return result;
    }

    // Hero.PAS:443
    public sbyte GetResistance()
    {
        Item item;
        sbyte result = 0;
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            item = Items[i];
            if (item != null)
            {
                result = (sbyte)(result + item.Resistance);
            }
        }

        return result;
    }

    // Hero.PAS:454
    public Set<MoveType> GetMoveTypes()
    {
        Item item;
        Set<MoveType> result = Set<MoveType>.Of();
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            item = Items[i];
            if (item != null)
            {
                result = result + item.GetAbilityMoveTypesAll();
            }
        }

        return result;
    }

    // Hero.PAS:465
    public Set<DamageType> GetImmunityTypes()
    {
        Item item;
        Set<DamageType> result = Set<DamageType>.Of();
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            item = Items[i];
            if (item != null)
            {
                result = result + item.GetAbilityImmunityTypesAll();
            }
        }

        return result;
    }

    // Hero.PAS:476
    public Set<DamageType> GetProtectionTypes()
    {
        Item item;
        Set<DamageType> result = Set<DamageType>.Of();
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            item = Items[i];
            if (item != null)
            {
                result = result + item.GetAbilityProtectionTypesAll();
            }
        }

        return result;
    }

    // Hero.PAS:487
    public int GetAbilityCount()
    {
        Item item;
        int result = 0;
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            item = Items[i];
            if (item != null && result < item.GetAbCount())
            {
                result = item.GetAbCount();
            }
        }

        return result;
    }

    // Hero.PAS:498
    public AbilityOwner GetSuperlativeEnabledAbilityOwner(int abilityId)
    {
        Item item;
        AbilityOwner result = null;
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            item = Items[i];
            if (item != null && item.GetAbEnabled(abilityId))
            {
                if (result == null || item.GetAbLevel(abilityId) > result.GetAbLevel(abilityId))
                {
                    result = item;
                }
            }
        }

        return result;
    }

    // Hero.PAS:511
    public AbilityOwner GetSuperlativeAbilityOwner(int abilityId)
    {
        Item item;
        AbilityOwner result = null;
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            item = Items[i];
            if (item != null && item.GetAbilitySetAt(abilityId))
            {
                if (result == null || item.GetAbLevel(abilityId) > result.GetAbLevel(abilityId))
                {
                    result = item;
                }
            }
        }

        return result;
    }

    // Hero.PAS:524
    public bool GetAbilityEnabled(int abilityId)
    {
        int i;
        Item item;
        bool result = false;
        i = 0;
        while (!result && i < Items.Count)
        {
            item = Items[i];
            result = item != null && item.GetAbEnabled(abilityId);
            i += 1;
        }

        return result;
    }

    // Hero.PAS:537
    public bool GetAbilitySet(int abilityId)
    {
        int i;
        Item item;
        bool result = false;
        i = 0;
        while (!result && i < Items.Count)
        {
            item = Items[i];
            result = item != null && item.GetAbilitySetAt(abilityId);
            i += 1;
        }

        return result;
    }

    // Hero.PAS:550
    public Item GetPositionItem(HeroItemPosition position)
    {
        Item result;
        int index;
        index = ((int)position) & 127;
        result = index < Items.Count ? Items[index] : null;

        return result;
    }

    // Hero.PAS:558
    public ItemType GetPositionItemType(HeroItemPosition position) => PositionItemTypes[(int)position];

    // Hero.PAS:564
    public HeroItemPosition GetItemTypePosition(ItemType itemType) => ItemTypePositions[(int)itemType];

    // Hero.PAS:571
    public bool CanPlaceItem(Item item, HeroItemPosition position) => item != null && (int)position < 6 && GetPositionItem(position) == null && GetPositionItemType(position) == item.ItemType;

    // Hero.PAS:577
    public bool PlaceItem(Item item, HeroItemPosition position)
    {
        bool result;
        if (CanPlaceItem(item, position))
        {
            item.SetArrayOwner(this, (int)position);
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static ReadOnlySpan<ItemType> PositionItemTypes => [
        ItemType.Attack,
        ItemType.Head,
        ItemType.Defense,
        ItemType.Ring,
        ItemType.Torso,
        ItemType.Ring
    ];

    private static ReadOnlySpan<HeroItemPosition> ItemTypePositions => [
        HeroItemPosition.HeadSlot,
        HeroItemPosition.TorsoSlot,
        HeroItemPosition.AttackSlot,
        HeroItemPosition.DefenseSlot,
        HeroItemPosition.LeftRingSlot,
        HeroItemPosition.None,
        HeroItemPosition.None
    ];

    // intf_P2_06.inc:25

    // intf_P2_06.inc:25

}
