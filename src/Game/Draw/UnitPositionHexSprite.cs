// Translated from AoWTC.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _position), 25);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _index), 26);
        stream.rwByte(ref _direction, 27);
    }

    // AoWTC.pas:5008

    // AoWTC.pas:5021
    public override byte GetLevel() => 27;

    // AoWTC.pas:5074
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int X = default;
        int Y = default;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, ref X, ref Y);
        _images[_imageIndex].ShowClipped(X - pixelXSurface - 8, Y - pixelYSurface - 8, lockedSurface);
    }

    // AoWTC.pas:5031
    public override void Activate()
    {
        MapCoordinate P = default;
        base.Activate();
        if (CombatMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && CombatMap.Current.UnitPositions.ValidatePosition(ref _position, ref _index))
        {
            var hexY = GetHexY();
            var packMapCoordinate = HexGeometry.PackMapCoordinate(GetHexX(), hexY, 0);
            CombatMap.Current.UnitPositions.SetUnitPosition(_position, _index, packMapCoordinate);
            _images = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TCUNITS.ILB");
            UpdateImageIndex();
        }
        else
        {
            P.X = GetHexX();
            P.Y = GetHexY();
            P.MapLevel = 0;
            P.Tag = (sbyte)_direction;
            CombatMap.Current.UnitPositions.SetUnitPosition(_position, _index, P);
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
            bool condition = unitPosition.X == hexX;
            if (condition)
            {
                var fUnitPositionsUnitPosition = CombatMap.Current.UnitPositions.GetUnitPosition(_position, _index);
                var hexY = GetHexY();
                condition = fUnitPositionsUnitPosition.Y == hexY;
            }

            if (condition)
            {
                var packMapCoordinate = HexGeometry.PackMapCoordinate(-1, -1, -1);
                CombatMap.Current.UnitPositions.SetUnitPosition(_position, _index, packMapCoordinate);
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
