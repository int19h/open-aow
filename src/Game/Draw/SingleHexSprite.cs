#nullable disable warnings
// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Draw;

// a sprite on one hexagon. _status: bit 1 visible, 2 edit mode, 4 selected, 8 activated (a
// Byte: the original sets the bits with or/and, which a set with Include does not give)
public class SingleHexSprite(CustomNode? owner) : HexSprite(owner)
{
    private int _status;

    // HXSPRITE.PAS:646
    public override bool Visible
    {
        get => (_status & 1) != 0;
        // HXSPRITE.PAS:640
        set => _status = value ? _status | 1 : _status & 0b1111_1110;
    }

    // HXSPRITE.PAS:683
    public override bool GetSelected() => (_status & 0b0100) != 0;

    // HXSPRITE.PAS:660
    public override bool EditMode
    {
        get => (_status & 0b0010) != 0;
        // HXSPRITE.PAS:665
        set => _status = value ? _status | 0b0010 : _status & 0b1111_1101;
    }

    // HXSPRITE.PAS:713
    public override int GetShowPriority()
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        return (Level << 26) + ((pixelY + 32) << 13) + pixelX + 16;
    }

    // HXSPRITE.PAS:704
    public override void Connect()
    {
    }

    // HXSPRITE.PAS:708
    public override void Disconnect() => SetOwner(null);

    // HXSPRITE.PAS:749
    public override bool Place(MapContainer? map, int pixelX, int pixelY, int hexLevel)
    {
        HexGeometry.PixelToHex(pixelX, vertical: pixelY, out int hexX, out int hexY);
        return PlaceHex(map, hexX.TruncateToSByte(), hexY.TruncateToSByte(), hexLevel.TruncateToSByte());
    }

    // HXSPRITE.PAS:721
    public override void Remove()
    {
        if (Owner is not null)
        {
            MapField mapField = Owner;
            RemoveFromMap();
            if (Visible)
            {
                if (Level <= 15)
                {
                    _ = mapField.Owner.MapContainer.Map.StaticVisibleHexSprite.RemoveChild(this);
                    mapField.Owner.MapContainer.Map.StaticSceneInvalid = true;
                }
                else
                {
                    _ = mapField.Owner.MapContainer.Map.DynamicVisibleHexSprite.RemoveChild(this);
                }
            }

            Visible = false;
        }
    }

    // HXSPRITE.PAS:688
    public override void Activate() => _status |= 0b1000;

    // HXSPRITE.PAS:693
    public override void Deactivate()
    {
        base.Deactivate();
        _status &= 0b1111_0111;
    }

    // HXSPRITE.PAS:651
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if ((_status & 1) == 0)
        {
            _status |= 1;
            _ = visibilityList.AddSortChild(this, sortValue: GetShowPriority());
        }
    }

    // HXSPRITE.PAS:671
    public override void Select()
    {
        base.Select();
        _status |= 0b0100;
    }

    // HXSPRITE.PAS:677
    public override void Unselect()
    {
        base.Unselect();
        _status &= 0b1111_1011;
    }

    // HXSPRITE.PAS:743
    public virtual bool CanPlace(MapContainer? map, int hexX, int hexY, int hexLevel) => hexX >= 0 && hexY >= 0 && hexLevel >= 0 && hexX < map.Width && hexY < map.Height && hexLevel < map.LevelCount;

    // HXSPRITE.PAS:757
    public virtual bool PlaceHex(MapContainer? map, int hexX, int hexY, int hexLevel)
    {
        if (EditMode || CanPlace(map, hexX, hexY, hexLevel))
        {
            Remove();
            SetOwner(map.GetField(hexX, hexY, hexLevel));
            return PlaceOnMap();
        }
        else
        {
            return false;
        }
    }

    // HXSPRITE.PAS:699
    public bool Activated() => (_status & 0b1000) != 0;
}
