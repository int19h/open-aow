#nullable disable warnings
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
public sealed partial class HeroItems : ObservableNode, IChildList<Item>, IItemHolder
{
    /// <summary>The items this hero wears, in their stored order.</summary>
    public ItemCollection Items => new(this);

    Item IOwner<Item>.this[int index] => Items[index];

    // Worn items take items only.
    protected sealed override bool AcceptsChild(EngineObject child) => child is Item;

    int IChildSink<Item>.AddChild(Item child) => AddChild(child);

    // Hero.PAS:388
    public HeroItems(CustomNode? owner) : base(null) => SetOwnerWithoutNotify(owner);

    // Hero.PAS:394
    public override void Dispose()
    {
        SetOwnerWithoutNotify(null);
        base.Dispose();
    }

    // Hero.PAS:400
    public override ControlStyles GetControlStyle() => base.GetControlStyle() | ControlStyles.ArrayMode;

    // Army.PAS:4516, the item list's release of each loaded child.
    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        if (persister.Loading)
        {
            for (int i = 0, last = Count - 1; i <= last; i++)
            {
                if (GetChild(i) is not null)
                {
                    _ = Items[i].Release();
                }
            }
        }
    }

    // Army.PAS:4546
    public override void Clear()
    {
        for (int i = Count - 1; i >= 0; i--)
        {
            if (GetChild(i) is not null)
            {
                EngineObject child = Items[i];
                if (child is not null)
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
    public override int RemoveChild(EngineObject? child)
    {
        int result = base.RemoveChild(child);
        if (result != -1)
        {
            _ = child.Release();
        }

        return result;
    }

    // Army.PAS:4525
    public override int AddChild(EngineObject? child)
    {
        int result = base.AddChild(child);
        if (result != -1)
        {
            _ = child.AddRef();
        }

        return result;
    }

    // Army.PAS:4564
    protected override void OnChange() => base.OnChange();

    // Hero.PAS:410
    public int GetAttack()
    {
        int result = 0;
        foreach (Item item in Items)
        {
            if (item is not null)
            {
                result = (result + item.Attack).TruncateToSByte();
            }
        }

        return result;
    }

    // Hero.PAS:421
    public int GetDefense()
    {
        int result = 0;
        foreach (Item item in Items)
        {
            if (item is not null)
            {
                result = (result + item.Defense).TruncateToSByte();
            }
        }

        return result;
    }

    // Hero.PAS:432
    public int GetDamage()
    {
        int result = 0;
        foreach (Item item in Items)
        {
            if (item is not null)
            {
                result = (result + item.Damage).TruncateToSByte();
            }
        }

        return result;
    }

    // Hero.PAS:443
    public int GetResistance()
    {
        int result = 0;
        foreach (Item item in Items)
        {
            if (item is not null)
            {
                result = (result + item.Resistance).TruncateToSByte();
            }
        }

        return result;
    }

    // Hero.PAS:454
    public MoveTypes GetMoveTypes()
    {
        MoveTypes result = MoveTypes.None;
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            Item item = Items[i];
            if (item is not null)
            {
                result |= item.GetAbilityMoveTypesAll();
            }
        }

        return result;
    }

    // Hero.PAS:465
    public DamageTypes GetImmunityTypes()
    {
        DamageTypes result = DamageTypes.None;
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            Item item = Items[i];
            if (item is not null)
            {
                result |= item.GetAbilityImmunityTypesAll();
            }
        }

        return result;
    }

    // Hero.PAS:476
    public DamageTypes GetProtectionTypes()
    {
        DamageTypes result = DamageTypes.None;
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            Item item = Items[i];
            if (item is not null)
            {
                result |= item.GetAbilityProtectionTypesAll();
            }
        }

        return result;
    }

    // Hero.PAS:487
    public int GetAbilityCount()
    {
        int result = 0;
        foreach (Item item in Items)
        {
            if (item is not null && result < item.OwnAbilityCount)
            {
                result = item.OwnAbilityCount;
            }
        }

        return result;
    }

    // Hero.PAS:498
    public AbilityOwner? GetSuperlativeEnabledAbilityOwner(int abilityId)
    {
        AbilityOwner? result = null;
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            Item item = Items[i];
            if (item is not null && item.GetOwnAbilityEnabled(abilityId))
            {
                if (result is null || item.GetOwnAbilityLevel(abilityId) > result.GetOwnAbilityLevel(abilityId))
                {
                    result = item;
                }
            }
        }

        return result;
    }

    // Hero.PAS:511
    public AbilityOwner? GetSuperlativeAbilityOwner(int abilityId)
    {
        AbilityOwner? result = null;
        for (int i = 0, last = Items.Count - 1; i <= last; i++)
        {
            Item item = Items[i];
            if (item is not null && item.GetAbilitySetAt(abilityId))
            {
                if (result is null || item.GetOwnAbilityLevel(abilityId) > result.GetOwnAbilityLevel(abilityId))
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
        bool result = false;
        int i = 0;
        while (!result && i < Items.Count)
        {
            Item item = Items[i];
            result = item is not null && item.GetOwnAbilityEnabled(abilityId);
            i += 1;
        }

        return result;
    }

    // Hero.PAS:537
    public bool GetAbilitySet(int abilityId)
    {
        bool result = false;
        int i = 0;
        while (!result && i < Items.Count)
        {
            Item item = Items[i];
            result = item is not null && item.GetAbilitySetAt(abilityId);
            i += 1;
        }

        return result;
    }

    // Hero.PAS:550
    public Item? GetPositionItem(HeroItemPosition position)
    {
        int index = ((int)position) & 0b0111_1111;
        return index < Items.Count ? Items[index] : null;
    }

    // Hero.PAS:558
    public ItemType GetPositionItemType(HeroItemPosition position) => PositionItemTypes[(int)position];

    // Hero.PAS:564
    public HeroItemPosition GetItemTypePosition(ItemType itemType) => ItemTypePositions[(int)itemType];

    // Hero.PAS:571
    public bool CanPlaceItem(Item? item, HeroItemPosition position) => item is not null && (int)position < 6 && GetPositionItem(position) is null && GetPositionItemType(position) == item.ItemType;

    // Hero.PAS:577
    public bool PlaceItem(Item? item, HeroItemPosition position)
    {
        if (CanPlaceItem(item, position))
        {
            item.SetArrayOwner(this, index: (int)position);
            return true;
        }
        else
        {
            return false;
        }
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

    Item System.Collections.Generic.IReadOnlyList<Item>.this[int index] => Items[index];
}
