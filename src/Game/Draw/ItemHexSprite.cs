#nullable disable warnings
// Translated from AoWE.pas.

using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Draw;

// items, generation mode
public partial class ItemHexSprite : SingleHexSprite
{

    public new const int ClassId = 131_697;

    public ItemHSList? Items { get; private set; }

    private ItemHexSpriteGenerateMode _generateMode;

    // Army.PAS:5221
    public override void Dispose()
    {
        base.Dispose();
        Items?.Dispose();
        Items = null;
    }

    // Army.PAS:5278
    public override void PersistFields(IPersister persister)
    {
        using (Items.BeginUpdate())
        {
            base.PersistFields(persister);
            persister.Object("Items", objectValue: Items);
            _generateMode = persister.Enum("GenerateMode", value: _generateMode);
        }
    }

    // Army.PAS:5321
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001985 && StrategicMap.Current.Day == 1)
        {
            GenerateItems();
        }
    }

    // Army.PAS:5235
    public override int Level => 26;

    // Army.PAS:5240
    public override int GetShowPriority()
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        int result = 1_744_830_464 + ((pixelY + 33) << 13) + pixelX + 16;
        HexSprite? hexSprite = Owner.GetMaxLevelNoneTransparentHexSprite();
        if (hexSprite != this && hexSprite is not null)
        {
            if (result <= hexSprite.GetShowPriority())
            {
                return hexSprite.GetShowPriority() + 1;
            }
        }

        return result;
    }

    // Army.PAS:5287
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (!GetSelected() || (StrategicMap.Current.Frame & 0b0110) != 0b0110)
        {
            HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
            GamePack.Current.GeneralImages[0].Show(pixelX - pixelXSurface, pixelY - pixelYSurface, lockedSurface);
        }
    }

    // Army.PAS:5267
    public override void Activate()
    {
        base.Activate();
        Items.RegisterItems();
    }

    // Army.PAS:5273
    public override void Deactivate() => base.Deactivate();

    // Army.PAS:5260
    public override bool CanSelect(int pixelX, int pixelY) => Owner.FogCount != 0 || !StrategicMap.Current.FogEnabled;

    // Army.PAS:5255
    public override string EditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_itemBagText);

    // Army.PAS:5297
    public override bool CanPlace(MapContainer? map, int hexX, int hexY, int hexLevel) => base.CanPlace(map, hexX, hexY, hexLevel);

    // Army.PAS:5228
    public void ItemsChanged(object sender)
    {
        if (Items.Count == 0 && (_generateMode == ItemHexSpriteGenerateMode.UserDefined || StrategicMap.Current.Day > 0))
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
        if (_generateMode != ItemHexSpriteGenerateMode.UserDefined)
        {
            RandSeed = StrategicMap.Current.Random(16777215);
            Items.SetChangedHandler(null);
            if (_generateMode == ItemHexSpriteGenerateMode.Random)
            {
                _generateMode = (ItemHexSpriteGenerateMode)(StrategicMap.Current.Random(3) + 1);
            }

            Items.Clear();
            for (int iOrdinal = 1, last = (int)_generateMode; iOrdinal <= last; iOrdinal++)
            {
                byte i = (byte)iOrdinal;
                Item? item = StrategicMap.Current.ItemControl.GenerateItem(ItemTypes.Head | ItemTypes.Torso | ItemTypes.Attack | ItemTypes.Defense | ItemTypes.Ring | ItemTypes.Use, 0, 65535);
                item?.SetOwner(Items);
            }

            Items.SetChangedHandler(ItemsChanged);
            ItemsChanged(Items);
        }
    }

    private static readonly string s_itemBagText = "Item Bag";
}

public enum ItemHexSpriteGenerateMode : byte
{
    UserDefined = 0, Low = 1, Average = 2, High = 3, Random = 4
}
