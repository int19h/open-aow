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
        FOwner = owner;
    }

    // Hero.PAS:394
    public override void Dispose()
    {
        FOwner = null;
        base.Dispose();
    }

    // Hero.PAS:400
    public override Set<ControlStyleElement> GetControlStyle() => base.GetControlStyle() + Set<ControlStyleElement>.Of(ControlStyleElement.ArrayMode);

    // Hero.PAS:410
    public sbyte GetAttack()
    {
        Item Item;
        sbyte Result = 0;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            Item = (Item)FChildren.List[I];
            if (Item != null)
            {
                Result = (sbyte)(Result + Item.FAttack);
            }
        }

        return Result;
    }

    // Hero.PAS:421
    public sbyte GetDefense()
    {
        Item Item;
        sbyte Result = 0;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            Item = (Item)FChildren.List[I];
            if (Item != null)
            {
                Result = (sbyte)(Result + Item.FDefense);
            }
        }

        return Result;
    }

    // Hero.PAS:432
    public sbyte GetDamage()
    {
        Item Item;
        sbyte Result = 0;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            Item = (Item)FChildren.List[I];
            if (Item != null)
            {
                Result = (sbyte)(Result + Item.FDamage);
            }
        }

        return Result;
    }

    // Hero.PAS:443
    public sbyte GetResistance()
    {
        Item Item;
        sbyte Result = 0;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            Item = (Item)FChildren.List[I];
            if (Item != null)
            {
                Result = (sbyte)(Result + Item.FResistance);
            }
        }

        return Result;
    }

    // Hero.PAS:454
    public Set<MoveType> GetMoveTypes()
    {
        Item Item;
        Set<MoveType> Result = Set<MoveType>.Of();
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            Item = (Item)FChildren.List[I];
            if (Item != null)
            {
                Result = Result + Item.GetAbMoveTypesAll();
            }
        }

        return Result;
    }

    // Hero.PAS:465
    public Set<DamageType> GetImmunityTypes()
    {
        Item Item;
        Set<DamageType> Result = Set<DamageType>.Of();
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            Item = (Item)FChildren.List[I];
            if (Item != null)
            {
                Result = Result + Item.GetAbImmunityTypesAll();
            }
        }

        return Result;
    }

    // Hero.PAS:476
    public Set<DamageType> GetProtectionTypes()
    {
        Item Item;
        Set<DamageType> Result = Set<DamageType>.Of();
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            Item = (Item)FChildren.List[I];
            if (Item != null)
            {
                Result = Result + Item.GetAbProtectionTypesAll();
            }
        }

        return Result;
    }

    // Hero.PAS:487
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

    // Hero.PAS:498
    public AbilityOwner GetSuperlativeEnabledAbOwner(int AbilityID)
    {
        Item Item;
        AbilityOwner Result = null;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            Item = (Item)FChildren.List[I];
            if (Item != null && Item.GetAbEnabled(AbilityID))
            {
                if (Result == null || Item.GetAbLevel(AbilityID) > Result.GetAbLevel(AbilityID))
                {
                    Result = Item;
                }
            }
        }

        return Result;
    }

    // Hero.PAS:511
    public AbilityOwner GetSuperlativeAbOwner(int AbilityID)
    {
        Item Item;
        AbilityOwner Result = null;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            Item = (Item)FChildren.List[I];
            if (Item != null && Item.GetAbSet(AbilityID))
            {
                if (Result == null || Item.GetAbLevel(AbilityID) > Result.GetAbLevel(AbilityID))
                {
                    Result = Item;
                }
            }
        }

        return Result;
    }

    // Hero.PAS:524
    public bool GetAbilityEnabled(int AbilityID)
    {
        int I;
        Item Item;
        bool Result = false;
        I = 0;
        while (!Result && I < Count)
        {
            Item = (Item)FChildren.List[I];
            Result = Item != null && Item.GetAbEnabled(AbilityID);
            I += 1;
        }

        return Result;
    }

    // Hero.PAS:537
    public bool GetAbilitySet(int AbilityID)
    {
        int I;
        Item Item;
        bool Result = false;
        I = 0;
        while (!Result && I < Count)
        {
            Item = (Item)FChildren.List[I];
            Result = Item != null && Item.GetAbSet(AbilityID);
            I += 1;
        }

        return Result;
    }

    // Hero.PAS:550
    public Item GetPositionItem(HeroItemPosition Position)
    {
        Item Result;
        int Index;
        Index = ((int)Position) & 127;
        if (Index < FChildren.Count)
        {
            Result = (Item)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Hero.PAS:558
    public ItemType GetPositionItemType(HeroItemPosition Position) => THeroItems_GetPositionItemType__Types[(int)Position];

    // Hero.PAS:564
    public HeroItemPosition GetItemTypePosition(ItemType ItemType) => THeroItems_GetItemTypePosition__Positions[(int)ItemType];

    // Hero.PAS:571
    public bool CanPlaceItem(Item Item, HeroItemPosition Position) => Item != null && (int)Position < 6 && GetPositionItem(Position) == null && GetPositionItemType(Position) == Item.FItemType;

    // Hero.PAS:577
    public bool PlaceItem(Item Item, HeroItemPosition Position)
    {
        bool Result;
        if (CanPlaceItem(Item, Position))
        {
            Item.SetArrayOwner(this, (int)Position);
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static ItemTypeArray6 THeroItems_GetPositionItemType__Types = ItemTypeArray6.From((ReadOnlySpan<ItemType>)new ItemType[6]
    {
        ItemType.Attack,
        ItemType.Head,
        ItemType.Defense,
        ItemType.Ring,
        ItemType.Torso,
        ItemType.Ring
    });

    private static HeroItemPositionArray7 THeroItems_GetItemTypePosition__Positions = HeroItemPositionArray7.From((ReadOnlySpan<HeroItemPosition>)new HeroItemPosition[7]
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
