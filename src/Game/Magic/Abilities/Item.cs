#nullable disable warnings
// Translated from AoWE.pas.
using System.Diagnostics.CodeAnalysis;
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Audio;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Draw.Images;
using Aow.Game.Magic.Spells;
using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.Magic.Abilities;

// ID, owner name, status, references, library ID, library name, value, name, type, spell ID,
// description, graphics index, hidden, availability, attack, defense, damage, resistance
public sealed class Item : AbilityOwner, IOwned<ItemOwner>, IOwnedSink<ItemOwner>
{
    [SuppressMessage("Style", "IDE0290", Justification = "Item.SetOwner calls AddRef and Release around AddChild. The count starts at 0, so Release reaches 0 and Dispose runs, and the constructor then stores 1. ItemLibrary.cs:51 reads LibraryId during that AddChild and stamps an id when the id is not positive, and the constructor then stores -1.")]
    public Item(CustomNode? owner) : base(owner)
    {
        _itemGraphicsIndex = -1;
        _description = [];
        LibraryId = -1;
        RefCount = 1;
    }

    public new const int ClassId = 131_688;

    // An item lies in an item list, a hero's inventory, the items a hero wears or an item
    // library. AcceptsOwner admits only those.
    protected sealed override bool AcceptsOwner(CustomNode owner)
        => owner is ItemList or HeroInventory or HeroItems or ItemLibrary;

    ItemOwner IOwned<ItemOwner>.Owner => ItemOwner.Of(Owner);

    void IOwnedSink<ItemOwner>.SetOwner(ItemOwner owner) => SetOwner(owner.Node);

    public int ItemId { get; set; }
    public string OwnerName { get; set; } = "";

    public ItemStatuses Status { get; set; }
    public int RefCount { get; private set; }

    public int LibraryId { get; set; }
    public string LibraryName { get; set; } = "";

    private int _obtainValue;

    public string Name { get; set; } = "";
    public ItemType ItemType { get; set; }

    private int _spellId;
    private readonly DelphiStringList _description;
    private int _itemGraphicsIndex;
    private bool _hidden;

    public ItemAvailability Availability { get; private set; }
    public int Attack { get; set; }
    public int Defense { get; private set; }
    public int Damage { get; private set; }
    public int Resistance { get; private set; }

    // Army.PAS:4220
    public override void Dispose()
    {
        _ = AddRef();
        if (Status.Contains(ItemStatuses.Activated))
        {
            Deactivate();
        }

        if (Status.Contains(ItemStatuses.Registered))
        {
            StrategicMap.Current.ItemControl.UnregisterItem(this);
        }

        _description?.Dispose();
        base.Dispose();
    }

    // Army.PAS:4241
    public override void SetOwner(CustomNode? owner)
    {
        if (owner != Owner)
        {
            _ = AddRef();
            _ = (Owner?.RemoveChild(this));

            SetOwnerWithoutNotify(owner);
            if (Owner is not null)
            {
                if (Owner.AddChild(this) == -1)
                {
                    SetOwnerWithoutNotify(null);
                }
            }

            Changed();
            _ = Release();
        }
    }

    // Army.PAS:4448
    // The text-only branches of the original ReadWrite served the text and translate streams. The tree has no such mode.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        // Under the text-only mode of the translate stream the original visits an item of a library
        // only for its owner name, and that only inside an item library, and never visits the
        // library name.
        bool translating = persister.Translating();
        if (!translating || !LibraryItem)
        {
            _itemGraphicsIndex = persister.Int32("ItemGraphicsIndex", value: _itemGraphicsIndex);
        }

        if (!translating || Owner is ItemLibrary || !LibraryItem)
        {
            OwnerName = persister.String("OwnerName", value: OwnerName);
        }

        if (translating && LibraryItem)
        {
            return;
        }

        Name = persister.String("Name", value: Name);
        Availability = persister.Enum("Availability", value: Availability);
        Attack = persister.UnsignedByte(name: "Attack", value: Attack.TruncateToSByte());
        Defense = persister.UnsignedByte(name: "Defense", value: Defense.TruncateToSByte());
        Damage = persister.UnsignedByte(name: "Damage", value: Damage.TruncateToSByte());
        Resistance = persister.UnsignedByte(name: "Resistance", value: Resistance.TruncateToSByte());
        ItemType = persister.Enum("ItemType", value: ItemType);
        _hidden = persister.Boolean("Hidden", value: _hidden);
        ItemId = persister.Int32("ItemId", value: ItemId);
        persister.Lines(name: "Description", lines: _description);
        _obtainValue = persister.Int32("ObtainValue", value: _obtainValue);
        _spellId = persister.Int32("SpellId", value: _spellId);
        LibraryId = persister.Int32("LibraryId", value: LibraryId, fallback: -1);
        if (!translating)
        {
            LibraryName = persister.String("LibraryName", value: LibraryName);
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
        RefCount -= 1;
        int count = RefCount;
        int result = count;
        if (count == 0)
        {
            Dispose();
        }

        return result;
    }

    // Army.PAS:4510
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 65541)
        {
            /* TODO: Implement the managed record view field mapping. */
            throw new NotImplementedException();
        }
    }

    // Army.PAS:4311
    public override void SetArrayOwner(CustomNode owner, int index)
    {
        _ = AddRef();
        base.SetArrayOwner(owner, index);
        if (Owner is not null)
        {
            _ = AddRef();
        }

        Changed();
        _ = Release();
    }

    // Army.PAS:4399
    public override string GetOwnerName() => OwnerName;

    // Army.PAS:4402
    public override AbilitySelectionTypes GetAbilitySelectionTypes()
    {
        return ItemType switch
        {
            ItemType.Head => AbilitySelectionTypes.HeadItem,
            ItemType.Torso => AbilitySelectionTypes.TorsoItem,
            ItemType.Attack => AbilitySelectionTypes.AttackItem,
            ItemType.Defense => AbilitySelectionTypes.DefenseItem,
            ItemType.Ring => AbilitySelectionTypes.RingItem,
            ItemType.Use => AbilitySelectionTypes.UseItem,
            ItemType.Scroll or _ => AbilitySelectionTypes.None,
        };
    }

    // Army.PAS:4345
    public override void Changed()
    {
        if (Status.Contains(ItemStatuses.Registered))
        {
            StrategicMap.Current.ItemControl.NotifyItemChanged(this);
        }
    }

    // Army.PAS:4276
    public bool CanUse(Hero hero, ref string reason)
    {
        bool result;
        if (ItemType == ItemType.Scroll && _spellId != 0)
        {
            if (StrategicMap.Current.Players.GetPlayers(hero.Player).MagicControl.Spells.IndexOf(_spellId) == -1)
            {
                return true;
            }
            else
            {
                result = false;
                Spell? spell = GamePack.Current.SpellControl[_spellId];
                reason = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_itemSpellAlreadyResearchedFormat), arguments: [spell.SpellName]);
            }
        }
        else
        {
            result = false;
            reason = AbilityRules.TranslateResourceString(GetEngine(), S: s_itemNotUsableText);
        }

        return result;
    }

    // Army.PAS:4296
    public bool Use(Hero hero)
    {
        string reason = "";
        if (CanUse(hero, ref reason) && !StrategicMap.Current.Players.GetPlayers(hero.Player).GetBusy())
        {
            ItemTokenEvent eventValue = new(null)
            {
                Holder = CreationHolder(),
                ItemId = ItemId,
                HeroId = hero.UnitId
            };
            StrategicMap.Current.TokenManager.GetTokenControl(hero.Player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Army.PAS:4255
    public bool ExecuteUse(Hero hero)
    {
        string reason = "";
        // { Original reports success even when CanUse rejects the item. }
        if (CanUse(hero, ref reason))
        {
            switch (ItemType)
            {
                case ItemType.Scroll:
                    _ = StrategicMap.Current.Players.GetPlayers(hero.Player).MagicControl.ExecuteSpellResearched(_spellId);
                    if (StrategicMap.Current.SeatedPlayer == hero.Player)
                    {
                        _ = AudioPlayback.PlaySample("SFX\\ATTACK\\MAGICB01.WAV");
                        Spell? spell = GamePack.Current.SpellControl[_spellId];
                        string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_itemSpellResearchedFormat), arguments: [spell.SpellName]);
                        StrategicMap.Current.ShowMessage(format);
                    }

                    Dispose();
                    hero.Changed();
                    break;
                case ItemType.Head:
                case ItemType.Torso:
                case ItemType.Attack:
                case ItemType.Defense:
                case ItemType.Ring:
                case ItemType.Use:
                default:
                    break;
            }
        }

        return true;
    }

    // Army.PAS:4418
    public void Show(int X, int Y, in LockedSurface surface)
    {
        ItemGraphics? graphics = GamePack.Current.ItemGraphicsResources.GetItemGraphics(_itemGraphicsIndex);
        if (graphics is not null)
        {
            _ = graphics.AddRef();
            graphics.ItemImage.Show(X, Y, lockedSurface: surface);
            _ = graphics.Release();
        }
    }

    // Army.PAS:4331
    public void Activate()
    {
        if (!Status.Contains(ItemStatuses.Activated))
        {
            if (StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting) || StrategicMap.Current.Day == 0)
            {
                UpdateFromLibrary();
            }

            Status |= ItemStatuses.Activated;
        }
    }

    // Army.PAS:4340
    public void Deactivate()
    {
        if (Status.Contains(ItemStatuses.Activated))
        {
            Status &= ~ItemStatuses.Activated;
        }
    }

    // Army.PAS:4320
    public MapCoordinate GetLocation() => Owner is IItemHolder holder ? holder.GetLocation() : MapCoordinate.None;

    // Army.PAS:4353
    public int CampaignTransferPoints => ObtainValue;

    // Army.PAS:4356
    public LibraryImage? GetImage()
    {
        ItemGraphics? graphics = GamePack.Current.ItemGraphicsResources.GetItemGraphics(_itemGraphicsIndex);
        LibraryImage? result;
        if (graphics is not null)
        {
            _ = graphics.AddRef();
            result = graphics.ItemImage;
            _ = graphics.Release();
        }
        else
        {
            return null;
        }

        return result;
    }

    // Army.PAS:4369
    public bool LibraryItem => LibraryId != -1;

    // Army.PAS:4372
    public string GetLibraryName()
    {
        string result = "";
        if (LibraryId != -1)
        {
            result = LibraryName;
            if (IsEmpty(result))
            {
                return AbilityRules.TranslateResourceString(GetEngine(), S: s_itemSystemLibraryText);
            }
        }

        return result;
    }

    // Army.PAS:4381
    public void UpdateFromLibrary()
    {
        if (LibraryId != -1)
        {
            Item? item = GamePack.Current.ItemLibraries.FindItem(LibraryName, LibraryId);
            if (item is not null)
            {
                int id = ItemId;
                Assign(item, engine: GetEngine());
                ItemId = id;
                Changed();
            }
            else
            {
                if (Owner is null)
                {
                    Dispose();
                }
                else
                {
                    LibraryId = -1;
                }
            }
        }
    }

    // Army.PAS:4415
    public void SetItemGraphicsIndex(int value) => _itemGraphicsIndex = value;

    // Army.PAS:4430
    public void SetItemType(ItemType value)
    {
        Attack = 0;
        Defense = 0;
        Resistance = 0;
        Damage = 0;
        ItemType itemType = value;
        ItemType = itemType;
        _itemGraphicsIndex = GamePack.Current.ItemGraphicsResources.FindItemTypeGraphics(itemType);
        Clear();
    }

    // Army.PAS:4443
    public int ObtainValue => _obtainValue > 10 ? _obtainValue : 10;

    // Army.PAS:4474
    public bool PlaceOnMap(GameMap? map, sbyte x, sbyte y, sbyte level)
    {
        if (x >= 0 && y >= 0 && level >= 0 && x < map.MapContainer.Width && y < map.MapContainer.Height && level < map.MapContainer.LevelCount && map.MapContainer.GetField(x, y, level).TerrainType != 15)
        {
            ItemHexSprite? hexSprite = map.MapContainer.GetField(x, y, level).FindOwnedChild<ItemHexSprite>(ItemHexSprite.ClassId);
            if (hexSprite is null)
            {
                hexSprite = new ItemHexSprite(null) { Holder = CreationHolder() };
                if (!map.PlaceHex(hexSprite, x, y, L: level))
                {
                    hexSprite?.Dispose();
                    hexSprite = null;
                }
            }

            if (hexSprite is not null)
            {
                SetOwner(hexSprite.Items);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private static readonly string s_itemSpellResearchedFormat = "%s researched";
    private static readonly string s_itemSpellAlreadyResearchedFormat = "%s already researched";
    private static readonly string s_itemNotUsableText = "This is not an usable item";
    private static readonly string s_itemSystemLibraryText = "System";
}

public enum ItemAvailability : byte
{
    Exceptional = 0, Rare = 1, Common = 2, Frequent = 3
}
