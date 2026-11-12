// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Draw;

// a sprite on one hexagon. _status: bit 1 visible, 2 edit mode, 4 selected, 8 activated (a
// Byte: the original sets the bits with or/and, which a set with Include does not give)
public partial class SingleHexSprite : HexSprite
{
    public SingleHexSprite(ECustomNode owner) : base(owner)
    { }

    private byte _status;

    // HXSPRITE.PAS:640
    public override void SetVisible(bool value)
    {
        if (value)
        {
            _status = (byte)(_status | 1);
        }
        else
        {
            _status = (byte)(_status & 254);
        }
    }

    // HXSPRITE.PAS:646
    public override bool GetVisible() => (_status & 1) != 0;

    // HXSPRITE.PAS:683
    public override bool GetSelected() => (_status & 4) != 0;

    // HXSPRITE.PAS:660
    public override bool GetEditMode() => (_status & 2) != 0;

    // HXSPRITE.PAS:665
    public override void SetEditMode(bool value)
    {
        if (value)
        {
            _status = (byte)(_status | 2);
        }
        else
        {
            _status = (byte)(_status & 253);
        }
    }

    // HXSPRITE.PAS:713
    public override int GetShowPriority()
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out pixelX, out pixelY);
        return (GetLevel() << 26) + ((pixelY + 32) << 13) + (pixelX + 16);
    }

    // HXSPRITE.PAS:704
    public override void Connect()
    {
    }

    // HXSPRITE.PAS:708
    public override void Disconnect()
    {
        SetOwner(null);
    }

    // HXSPRITE.PAS:749
    public override bool Place(MapContainer map, int pixelX, int pixelY, int hexLevel)
    {
        int hexX = default;
        int hexY = default;
        HexGeometry.PixelToHex(pixelX, pixelY, out hexX, out hexY);
        return PlaceHex(map, (sbyte)hexX, (sbyte)hexY, (sbyte)hexLevel);
    }

    // HXSPRITE.PAS:721
    public override void Remove()
    {
        MapField mapField;
        if (Owner != null)
        {
            mapField = Owner;
            RemoveFromMap();
            if (GetVisible())
            {
                if (GetLevel() <= 15)
                {
                    mapField.Owner.MapContainer.Map.StaticVisibleHexSprite.RemoveChild(this);
                    mapField.Owner.MapContainer.Map.StaticSceneInvalid = true;
                }
                else
                {
                    mapField.Owner.MapContainer.Map.DynamicVisibleHexSprite.RemoveChild(this);
                }
            }

            SetVisible(false);
        }
    }

    // HXSPRITE.PAS:688
    public override void Activate()
    {
        _status = (byte)(_status | 8);
    }

    // HXSPRITE.PAS:693
    public override void Deactivate()
    {
        base.Deactivate();
        _status = (byte)(_status & 247);
    }

    // HXSPRITE.PAS:651
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if ((_status & 1) == 0)
        {
            _status = (byte)(_status | 1);
            visibilityList.AddSortChild(this, GetShowPriority());
        }
    }

    // HXSPRITE.PAS:671
    public override void Select()
    {
        base.Select();
        _status = (byte)(_status | 4);
    }

    // HXSPRITE.PAS:677
    public override void Unselect()
    {
        base.Unselect();
        _status = (byte)(_status & 251);
    }

    // HXSPRITE.PAS:743
    public virtual bool CanPlace(MapContainer map, sbyte hexX, sbyte hexY, sbyte hexLevel) => hexX >= 0 && hexY >= 0 && hexLevel >= 0 && hexX < map.Width && hexY < map.Height && hexLevel < map.LevelCount;

    // HXSPRITE.PAS:757
    public virtual bool PlaceHex(MapContainer map, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool result;
        if (GetEditMode() || CanPlace(map, hexX, hexY, hexLevel))
        {
            Remove();
            SetOwner(map.GetField(hexX, hexY, hexLevel));
            result = PlaceOnMap();
        }
        else
        {
            result = false;
        }

        return result;
    }

    // HXSPRITE.PAS:699
    public bool Activated() => (_status & 8) != 0;
}
