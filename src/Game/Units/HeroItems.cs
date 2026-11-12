// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Units;

// inherited FOwner is the hero. FChildren holds six equipment slots
public partial class HeroItems : ItemList
{
    // Hero.PAS:388
    public HeroItems(ECustomNode owner) : base(null)
    {
        SetOwnerWithoutNotify(owner);
    }

    // Hero.PAS:394
    public override void Dispose()
    {
        SetOwnerWithoutNotify(null);
        base.Dispose();
    }

    // Hero.PAS:400
    public override Set<ControlStyleElement> GetControlStyle() => base.GetControlStyle() + Set<ControlStyleElement>.Of(ControlStyleElement.ArrayMode);

    // Hero.PAS:410
    public sbyte GetAttack()
    {
        Item item;
        sbyte result = 0;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            item = this[I];
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
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            item = this[I];
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
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            item = this[I];
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
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            item = this[I];
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
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            item = this[I];
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
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            item = this[I];
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
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            item = this[I];
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

    // Hero.PAS:498
    public AbilityOwner GetSuperlativeEnabledAbilityOwner(int abilityId)
    {
        Item item;
        AbilityOwner result = null;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            item = this[I];
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
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            item = this[I];
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
        int I;
        Item item;
        bool result = false;
        I = 0;
        while (!result && I < Count)
        {
            item = this[I];
            result = item != null && item.GetAbEnabled(abilityId);
            I += 1;
        }

        return result;
    }

    // Hero.PAS:537
    public bool GetAbilitySet(int abilityId)
    {
        int I;
        Item item;
        bool result = false;
        I = 0;
        while (!result && I < Count)
        {
            item = this[I];
            result = item != null && item.GetAbilitySetAt(abilityId);
            I += 1;
        }

        return result;
    }

    // Hero.PAS:550
    public Item GetPositionItem(HeroItemPosition position)
    {
        Item result;
        int index;
        index = ((int)position) & 127;
        if (index < _children.Count)
        {
            result = this[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Hero.PAS:558
    public ItemType GetPositionItemType(HeroItemPosition position) => s_positionItemTypes[(int)position];

    // Hero.PAS:564
    public HeroItemPosition GetItemTypePosition(ItemType itemType) => s_itemTypePositions[(int)itemType];

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

    private static ItemTypeArray6 s_positionItemTypes = ItemTypeArray6.From((ReadOnlySpan<ItemType>)new ItemType[6]
    {
        ItemType.Attack,
        ItemType.Head,
        ItemType.Defense,
        ItemType.Ring,
        ItemType.Torso,
        ItemType.Ring
    });

    private static HeroItemPositionArray7 s_itemTypePositions = HeroItemPositionArray7.From((ReadOnlySpan<HeroItemPosition>)new HeroItemPosition[7]
    {
        HeroItemPosition.HeadSlot,
        HeroItemPosition.TorsoSlot,
        HeroItemPosition.AttackSlot,
        HeroItemPosition.DefenseSlot,
        HeroItemPosition.LeftRingSlot,
        HeroItemPosition.None,
        HeroItemPosition.None
    });

    // intf_P2_06.inc:25

    // intf_P2_06.inc:25

}
