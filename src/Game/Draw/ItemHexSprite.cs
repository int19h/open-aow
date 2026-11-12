// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// items, generation mode
public partial class ItemHexSprite : SingleHexSprite
{

    public new const int ClassId = 131697;

    public ItemHSList Items { get; private set; }

    private ItemHexSpriteGenerateMode _generateMode;

    // Army.PAS:5221
    public override void Dispose()
    {
        base.Dispose();
        Items?.Dispose();
        Items = null;
    }

    // Army.PAS:5278
    public override void ReadWrite(EStorageStream stream)
    {
        Items.BeginUpdate();
        base.ReadWrite(stream);
        stream.rwEObject(Items, 4);
        stream.rwData(Bytes.Of(ref _generateMode, 1), 1, 5);
        Items.EndUpdate();
    }

    // Army.PAS:5321
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001985 && AoWHSMap.Current.Day == 1)
        {
            GenerateItems();
        }
    }

    // Army.PAS:5235
    public override byte GetLevel() => 26;

    // Army.PAS:5240
    public override int GetShowPriority()
    {
        int pixelX = default;
        int pixelY = default;
        HexSprite hexSprite;
        HexGeometry.HexToPixel(((MapField)Owner).HexX, ((MapField)Owner).HexY, ref pixelX, ref pixelY);
        int result = 1744830464 + ((pixelY + 33) << 13) + (pixelX + 16);
        hexSprite = ((MapField)Owner).GetMaxLevelNoneTransparentHexSprite();
        if (hexSprite != this && hexSprite != null)
        {
            if (result <= hexSprite.GetShowPriority())
            {
                result = hexSprite.GetShowPriority() + 1;
            }
        }

        return result;
    }

    // Army.PAS:5287
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        if (!GetSelected() || (AoWHSMap.Current.Frame & 6) != 6)
        {
            HexGeometry.HexToPixel(((MapField)Owner).HexX, ((MapField)Owner).HexY, ref pixelX, ref pixelY);
            AoWHSSet.Current.GeneralImages[0].Show(pixelX - pixelXSurface, pixelY - pixelYSurface, lockedSurface);
        }
    }

    // Army.PAS:5267
    public override void Activate()
    {
        base.Activate();
        Items.RegisterItems();
    }

    // Army.PAS:5273
    public override void Deactivate()
    {
        base.Deactivate();
    }

    // Army.PAS:5260
    public override bool CanSelect(int pixelX, int pixelY)
    {
        bool result;
        if (Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((AoWMapField)Owner).FieldFlags).Hi).Lo == 0 && AoWHSMap.Current.FogEnabled)
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Army.PAS:5255
    public override string EditName() => AbilityRules.TranslateResourceString(s_itemBagText);

    // Army.PAS:5297
    public override bool CanPlace(MapContainer map, sbyte hexX, sbyte hexY, sbyte hexLevel) => base.CanPlace(map, hexX, hexY, hexLevel);

    // Army.PAS:5228
    public void ItemsChanged(object sender)
    {
        if (Items.Count == 0 && (_generateMode == ItemHexSpriteGenerateMode.UserDefined || AoWHSMap.Current.Day > 0))
        {
            Dispose();
        }
        else
        {
            Changed();
        }
    }

    // Army.PAS:5302
    public void GenerateItems()
    {
        byte I = default;
        Item item = default;
        if (_generateMode != ItemHexSpriteGenerateMode.UserDefined)
        {
            RandSeed = AoWHSMap.Current.Random(16777215);
            Items.OnChanged = null;
            if (_generateMode == ItemHexSpriteGenerateMode.Random)
            {
                _generateMode = (ItemHexSpriteGenerateMode)(AoWHSMap.Current.Random(3) + 1);
            }

            Items.Clear();
            for (int iOrdinal = 1, last = (int)_generateMode; iOrdinal <= last; iOrdinal++)
            {
                I = (byte)iOrdinal;
                item = AoWHSMap.Current.ItemControl.GenerateItem(Set<ItemType>.Of(ItemType.Head, ItemType.Torso, ItemType.Attack, ItemType.Defense, ItemType.Ring, ItemType.Use), 0, 65535);
                if (item != null)
                {
                    item.SetOwner(Items);
                }
            }

            Items.OnChanged = ItemsChanged;
            ItemsChanged(Items);
        }
    }

    private static readonly string s_itemBagText = "Item Bag";

    // intf_P2_04.inc:216

    // intf_P2_04.inc:216

}

public enum ItemHexSpriteGenerateMode : byte
{
    UserDefined = 0, Low = 1, Average = 2, High = 3, Random = 4
}
