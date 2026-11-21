#nullable disable warnings
// Translated from AoWTC.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

namespace Aow.Game.Draw;

// Position. Index. ImageIndex. Images. Direction
public sealed class UnitPositionHexSprite(CustomNode? owner) : SingleHexSprite(owner)
{

    // AoWTC.pas:5016
    public new const int ClassId = 2_228_483;

    private sbyte _position;
    private sbyte _index;
    private int _imageIndex;
    private CustomImageLibrary? _images;
    private byte _direction = 1;

    // AoWTC.pas:5060
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _position = persister.UnsignedByte(name: "Position", value: _position);
        _index = persister.UnsignedByte(name: "Index", value: _index);
        _direction = persister.Byte("Direction", value: _direction);
    }

    // AoWTC.pas:5021
    public override byte Level => 27;

    // AoWTC.pas:5074
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int x, out int y);
        _images[_imageIndex].ShowClipped(x - pixelXSurface - 8, y - pixelYSurface - 8, lockedSurface);
    }

    // AoWTC.pas:5031
    public override void Activate()
    {
        base.Activate();
        if (CombatMap.Current.Status.Contains(HexSpriteMapStatuses.Editting) && CombatMap.Current.UnitPositions.ValidatePosition(ref _position, ref _index))
        {
            MapCoordinate packMapCoordinate = HexGeometry.PackMapCoordinate(y: HexY, x: HexX, mapLevel: 0);
            CombatMap.Current.UnitPositions.SetUnitPosition(_position, _index, value: new UnitPosition(Location: packMapCoordinate, Facing: 0));
            _images = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(ImageLibraryManagerCategories.Disk, fileName: "TCUNITS.ILB");
            UpdateImageIndex();
        }
        else
        {
            MapCoordinate location = new(X: HexX, Y: HexY, MapLevel: 0);
            CombatMap.Current.UnitPositions.SetUnitPosition(_position, _index, value: new UnitPosition(Location: location, Facing: _direction));
            Dispose();
        }
    }

    // AoWTC.pas:5049
    public override void Deactivate()
    {
        if (Activated() && Owner.Owner.Map.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(ImageLibraryManagerCategories.Disk, fileName: "TCUNITS.ILB");
            UnitPosition unitPosition = CombatMap.Current.UnitPositions.GetUnitPosition(_position, _index);
            sbyte hexX = HexX;
            bool condition = unitPosition.Location.X == hexX;
            if (condition)
            {
                UnitPosition fUnitPositionsUnitPosition = CombatMap.Current.UnitPositions.GetUnitPosition(_position, _index);
                sbyte hexY = HexY;
                condition = fUnitPositionsUnitPosition.Location.Y == hexY;
            }

            if (condition)
            {
                CombatMap.Current.UnitPositions.SetUnitPosition(_position, _index, value: UnitPosition.None);
            }
        }

        base.Deactivate();
    }

    // AoWTC.pas:5068
    public override bool CanPlace(MapContainer? map, sbyte hexX, sbyte hexY, sbyte hexLevel) => map.GetField(hexX, hexY, hexLevel).FindOwnedChild(ClassId) is null && CombatMap.Current.UnitPositions.ValidatePosition(ref _position, ref _index);

    // AoWTC.pas:5026
    public void UpdateImageIndex() => _imageIndex = (_position * 8) + _index;
}
