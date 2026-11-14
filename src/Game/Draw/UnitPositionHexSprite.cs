// Translated from AoWTC.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Draw;

// Position. Index. ImageIndex. Images. Direction
public partial class UnitPositionHexSprite : SingleHexSprite
{
    public UnitPositionHexSprite(ECustomNode owner) : base(owner)
    {
        _position = 0;
        _index = 0;
        _direction = 1;
    }

    // AoWTC.pas:5016
    public new const int ClassId = 2228483;

    private sbyte _position;
    private sbyte _index;
    private int _imageIndex;
    private CustomImageLibrary _images;
    private byte _direction;

    // AoWTC.pas:5060
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _position = PersistSupport.UnsignedByte(persister, name: "Position", value: _position);
        _index = PersistSupport.UnsignedByte(persister, name: "Index", value: _index);
        _direction = persister.Byte("Direction", value: _direction);
    }

    // AoWTC.pas:5008

    // AoWTC.pas:5021
    public override byte GetLevel() => 27;

    // AoWTC.pas:5074
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int X = default;
        int Y = default;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out X, out Y);
        _images[_imageIndex].ShowClipped(X - pixelXSurface - 8, Y - pixelYSurface - 8, lockedSurface);
    }

    // AoWTC.pas:5031
    public override void Activate()
    {
        base.Activate();
        if (CombatMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && CombatMap.Current.UnitPositions.ValidatePosition(ref _position, ref _index))
        {
            var hexY = GetHexY();
            var packMapCoordinate = HexGeometry.PackMapCoordinate(GetHexX(), hexY, 0);
            CombatMap.Current.UnitPositions.SetUnitPosition(_position, _index, new UnitPosition(Location: packMapCoordinate, Facing: 0));
            _images = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TCUNITS.ILB");
            UpdateImageIndex();
        }
        else
        {
            var location = new MapCoordinate(X: GetHexX(), Y: GetHexY(), MapLevel: 0);
            CombatMap.Current.UnitPositions.SetUnitPosition(_position, _index, new UnitPosition(Location: location, Facing: _direction));
            Dispose();
        }
    }

    // AoWTC.pas:5049
    public override void Deactivate()
    {
        if (Activated() && Owner.Owner.Map.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TCUNITS.ILB");
            var unitPosition = CombatMap.Current.UnitPositions.GetUnitPosition(_position, _index);
            var hexX = GetHexX();
            bool condition = unitPosition.Location.X == hexX;
            if (condition)
            {
                var fUnitPositionsUnitPosition = CombatMap.Current.UnitPositions.GetUnitPosition(_position, _index);
                var hexY = GetHexY();
                condition = fUnitPositionsUnitPosition.Location.Y == hexY;
            }

            if (condition)
            {
                CombatMap.Current.UnitPositions.SetUnitPosition(_position, _index, UnitPosition.None);
            }
        }

        base.Deactivate();
    }

    // AoWTC.pas:5068
    public override bool CanPlace(MapContainer map, sbyte hexX, sbyte hexY, sbyte hexLevel) => map.GetField(hexX, hexY, hexLevel).FindOwnedChild(2228483) == null && CombatMap.Current.UnitPositions.ValidatePosition(ref _position, ref _index);

    // AoWTC.pas:5026
    public void UpdateImageIndex()
    {
        _imageIndex = _position * 8 + _index;
    }

    // AoWTC.pas:223

}
