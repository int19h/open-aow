// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Audio;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

// ID, owner name, status, references, library ID, library name, value, name, type, spell ID,
// description, graphics index, hidden, availability, attack, defense, damage, resistance
public partial class Item : AbilityOwner, IOwned<ItemList>, IOwnedSink<ItemList>
{
    public new const int ClassId = 131688;

    // The item list the item lies in. AcceptsOwner admits only an item list, so the cast never
    // fails.
    public new ItemList Owner { get => (ItemList)base.Owner; set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(ECustomNode owner) => owner is ItemList;

    void IOwnedSink<ItemList>.SetOwner(ItemList owner) => SetOwner(owner);

    // Army.PAS:4211
    public Item(ECustomNode owner) : base(owner)
    {
        _itemGraphicsIndex = -1;
        _description = new StringList();
        LibraryId = -1;
        RefCount = 1;
    }

    public int ItemId;
    public string OwnerName = "";

    public Set<ItemStatusElement> Status { get; set; }
    public int RefCount { get; private set; }

    public int LibraryId;
    public string LibraryName = "";
    private int _obtainValue;
    public string Name = "";
    public ItemType ItemType;
    private int _spellId;
    private StringList _description;
    private int _itemGraphicsIndex;
    private bool _hidden;
    public ItemAvailability Availability;
    public sbyte Attack;
    public sbyte Defense;
    public sbyte Damage;
    public sbyte Resistance;

    // Army.PAS:4220
    public override void Dispose()
    {
        AddRef();
        if (Status.Contains(ItemStatusElement.Activated))
        {
            Deactivate();
        }

        if (Status.Contains(ItemStatusElement.Registered))
        {
            AoWHSMap.Current.ItemControl.UnregisterItem(this);
        }

        _description?.Dispose();
        base.Dispose();
    }

    // Army.PAS:4241
    public override void SetOwner(ECustomNode owner)
    {
        if (owner != Owner)
        {
            AddRef();
            if (Owner != null)
            {
                Owner.RemoveChild(this);
            }

            SetOwnerWithoutNotify(owner);
            if (Owner != null)
            {
                if (Owner.AddChild(this) == -1)
                {
                    SetOwnerWithoutNotify(null);
                }
            }

            Changed();
            Release();
        }
    }

    // Army.PAS:4448
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        if (!stream.StorageMode().Contains(EngineServices.Text) || !GetLibraryItem())
        {
            stream.rwInteger(ref _itemGraphicsIndex, 7);
        }

        if (!stream.StorageMode().Contains(EngineServices.Text) || Owner is ItemLibrary || !GetLibraryItem())
        {
            stream.rwString(ref OwnerName, 8);
        }

        if (!stream.StorageMode().Contains(EngineServices.Text) || !GetLibraryItem())
        {
            stream.rwString(ref Name, 9);
            stream.rwData(Bytes.Of(ref Availability, 1), 1, 10);
            stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Attack), 11);
            stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Defense), 12);
            stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Damage), 13);
            stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Resistance), 14);
            stream.rwData(Bytes.Of(ref ItemType, 1), 1, 15);
            stream.rwBool(ref _hidden, 16);
            stream.rwInteger(ref ItemId, 17);
            stream.rwStrings(_description, 18);
            stream.rwInteger(ref _obtainValue, 19);
            stream.rwInteger(ref _spellId, 20);
            stream.rwIntegerEx(ref LibraryId, -1, 21);
            if (!stream.StorageMode().Contains(EngineServices.Text))
            {
                stream.rwString(ref LibraryName, 22);
            }
        }
    }

    // Army.PAS:4229
    public override int AddRef()
    {
        RefCount += 1;
        return RefCount;
    }

    // Army.PAS:4232
    public override int Release()
    {
        int count;
        RefCount -= 1;
        count = RefCount;
        int result = count;
        if (count == 0)
        {
            Dispose();
        }

        return result;
    }

    // Army.PAS:4510
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 65541)
        {
            /* TODO: Implement the managed record view field mapping. */
            throw new NotImplementedException();
        }
    }

    // Army.PAS:4311
    public override void SetArrayOwner(ECustomNode owner, int index)
    {
        AddRef();
        base.SetArrayOwner(owner, index);
        if (Owner != null)
        {
            AddRef();
        }

        Changed();
        Release();
    }

    // Army.PAS:4399
    public override string GetOwnerName() => OwnerName;

    // Army.PAS:4402
    public override Set<AbilitySelectionType> GetAbilitySelectionTypes()
    {
        Set<AbilitySelectionType> result;
        switch (ItemType)
        {
            case ItemType.Head:
                result = Set<AbilitySelectionType>.Of(AbilitySelectionType.HeadItem);
                break;
            case ItemType.Torso:
                result = Set<AbilitySelectionType>.Of(AbilitySelectionType.TorsoItem);
                break;
            case ItemType.Attack:
                result = Set<AbilitySelectionType>.Of(AbilitySelectionType.AttackItem);
                break;
            case ItemType.Defense:
                result = Set<AbilitySelectionType>.Of(AbilitySelectionType.DefenseItem);
                break;
            case ItemType.Ring:
                result = Set<AbilitySelectionType>.Of(AbilitySelectionType.RingItem);
                break;
            case ItemType.Use:
                result = Set<AbilitySelectionType>.Of(AbilitySelectionType.UseItem);
                break;
            default:
                result = Set<AbilitySelectionType>.Of();
                break;
        }

        return result;
    }

    // Army.PAS:4345
    public override void Changed()
    {
        if (Status.Contains(ItemStatusElement.Registered))
        {
            AoWHSMap.Current.ItemControl.ItemChanged(this);
        }
    }

    // Army.PAS:4276
    public virtual bool CanUse(Hero hero, ref string reason)
    {
        bool result;
        if (ItemType == ItemType.Scroll && _spellId != 0)
        {
            if (AoWHSMap.Current.Players.GetPlayers(hero.Player).MagicControl.Spells.IndexOf(_spellId) == -1)
            {
                result = true;
            }
            else
            {
                result = false;
                var spell = AoWHSSet.Current.SpellControl.GetSpell(_spellId);
                reason = AbilityRules.Format(AbilityRules.TranslateResourceString(s_itemSpellAlreadyResearchedFormat), [spell.SpellName]);
            }
        }
        else
        {
            result = false;
            reason = AbilityRules.TranslateResourceString(s_itemNotUsableText);
        }

        return result;
    }

    // Army.PAS:4296
    public virtual bool Use(Hero hero)
    {
        bool result;
        string reason = "";
        ItemTokenEvent eventValue;
        if (CanUse(hero, ref reason) && !AoWHSMap.Current.Players.GetPlayers(hero.Player).GetBusy())
        {
            eventValue = new ItemTokenEvent(null);
            eventValue.ItemId = ItemId;
            eventValue.HeroId = hero.UnitId;
            AoWHSMap.Current.TokenManager.GetTokenControl(hero.Player).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Army.PAS:4255
    public virtual bool ExecuteUse(Hero hero)
    {
        string reason = "";
        // { Original reports success even when CanUse rejects the item. }
        bool result = true;
        if (CanUse(hero, ref reason))
        {
            switch (ItemType)
            {
                case ItemType.Scroll:
                    AoWHSMap.Current.Players.GetPlayers(hero.Player).MagicControl.ExecuteSpellResearched(_spellId);
                    if (AoWHSMap.Current.SeatedPlayer == hero.Player)
                    {
                        AudioPlayback.PlaySample("SFX\\ATTACK\\MAGICB01.WAV");
                        var spell = AoWHSSet.Current.SpellControl.GetSpell(_spellId);
                        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_itemSpellResearchedFormat), [spell.SpellName]);
                        AoWHSMap.Current.ShowMessage(format);
                    }

                    Dispose();
                    hero.Changed();
                    break;
            }
        }

        return result;
    }

    // Army.PAS:4418
    public virtual void Show(int X, int Y, in LockedSurface surface)
    {
        ItemGraphics graphics;
        graphics = AoWHSSet.Current.ItemGraphicsResources.GetItemGraphics(_itemGraphicsIndex);
        if (graphics != null)
        {
            graphics.AddRef();
            graphics.GetItemImage().Show(X, Y, surface);
            graphics.Release();
        }
    }

    // Army.PAS:4331
    public virtual void Activate()
    {
        if (!Status.Contains(ItemStatusElement.Activated))
        {
            if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) || AoWHSMap.Current.Day == 0)
            {
                UpdateFromLibrary();
            }

            Status = Status + Set<ItemStatusElement>.Of(ItemStatusElement.Activated);
        }
    }

    // Army.PAS:4340
    public virtual void Deactivate()
    {
        if (Status.Contains(ItemStatusElement.Activated))
        {
            Status = Status - Set<ItemStatusElement>.Of(ItemStatusElement.Activated);
        }
    }

    // Army.PAS:4320
    public void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        if (Owner != null)
        {
            Owner.GetLocation(ref X, ref Y, ref L);
        }
        else
        {
            X = -1;
            Y = -1;
            L = -1;
        }
    }

    // Army.PAS:4353
    public int GetCampaignTransferPoints() => GetObtainValue();

    // Army.PAS:4356
    public LibraryImage GetImage()
    {
        LibraryImage result;
        ItemGraphics graphics;
        graphics = AoWHSSet.Current.ItemGraphicsResources.GetItemGraphics(_itemGraphicsIndex);
        if (graphics != null)
        {
            graphics.AddRef();
            result = graphics.GetItemImage();
            graphics.Release();
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Army.PAS:4369
    public bool GetLibraryItem() => LibraryId != -1;

    // Army.PAS:4372
    public string GetLibraryName()
    {
        string result = "";
        if (LibraryId != -1)
        {
            result = LibraryName;
            if (IsEmpty(result))
            {
                result = AbilityRules.TranslateResourceString(s_itemSystemLibraryText);
            }
        }

        return result;
    }

    // Army.PAS:4381
    public void UpdateFromLibrary()
    {
        int id;
        Item item;
        if (LibraryId != -1)
        {
            item = AoWHSSet.Current.ItemLibraries.FindItem(LibraryName, LibraryId);
            if (item != null)
            {
                id = ItemId;
                Assign(item, GameTables.GameEngine);
                ItemId = id;
                Changed();
            }
            else
            {
                if (Owner == null)
                {
                    this.Dispose();
                }
                else
                {
                    LibraryId = -1;
                }
            }
        }
    }

    // Army.PAS:4415
    public void SetItemGraphicsIndex(int value)
    {
        _itemGraphicsIndex = value;
    }

    // Army.PAS:4430
    public void SetItemType(ItemType value)
    {
        ItemType itemType;
        Attack = 0;
        Defense = 0;
        Resistance = 0;
        Damage = 0;
        itemType = value;
        ItemType = itemType;
        _itemGraphicsIndex = AoWHSSet.Current.ItemGraphicsResources.FindItemTypeGraphics(itemType);
        base.Clear();
    }

    // Army.PAS:4443
    public int GetObtainValue()
    {
        int result;
        if (_obtainValue > 10)
        {
            result = _obtainValue;
        }
        else
        {
            result = 10;
        }

        return result;
    }

    // Army.PAS:4474
    public bool PlaceOnMap(GameMap map, sbyte X, sbyte Y, sbyte L)
    {
        bool result;
        ItemHexSprite hexSprite;
        if (X >= 0 && Y >= 0 && L >= 0 && X < map.MapContainer.Width && Y < map.MapContainer.Height && L < map.MapContainer.LevelCount && map.MapContainer.GetField(X, Y, L).TerrainType != 15)
        {
            hexSprite = (ItemHexSprite)map.MapContainer.GetField(X, Y, L).FindOwnedChild(131697);
            if (hexSprite == null)
            {
                hexSprite = new ItemHexSprite(null);
                if (!map.PlaceHex(hexSprite, X, Y, L))
                {
                    hexSprite?.Dispose();
                    hexSprite = null;
                }
            }

            if (hexSprite != null)
            {
                SetOwner(hexSprite.Items);
                result = true;
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly string s_itemSpellResearchedFormat = "%s researched";
    private static readonly string s_itemSpellAlreadyResearchedFormat = "%s already researched";
    private static readonly string s_itemNotUsableText = "This is not an usable item";
    private static readonly string s_itemSystemLibraryText = "System";

    // intf_P2_04.inc:84

    // intf_P2_04.inc:84

}

public enum ItemAvailability : byte
{
    Exceptional = 0, Rare = 1, Common = 2, Frequent = 3
}
