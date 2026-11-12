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
        Position = 0;
        Index = 0;
        Direction = 1;
    }

    // AoWTC.pas:5016
    public new const int ClassId = 2228483;

    public sbyte Position;
    public sbyte Index;
    public int ImageIndex;
    public CustomImageLibrary Images;
    public byte Direction;

    // AoWTC.pas:5060
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Position), 25);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Index), 26);
        stream.rwByte(ref Direction, 27);
    }

    // AoWTC.pas:5008

    // AoWTC.pas:5021
    public override byte GetLevel() => 27;

    // AoWTC.pas:5074
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int X = default;
        int Y = default;
        HexGeometry.HexToPixel(((MapField)Owner).HexX, ((MapField)Owner).HexY, ref X, ref Y);
        Images[ImageIndex].ShowClipped(X - pixelXSurface - 8, Y - pixelYSurface - 8, lockedSurface);
    }

    // AoWTC.pas:5031
    public override void Activate()
    {
        MapCoordinate P = default;
        base.Activate();
        if (CombatMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && CombatMap.Current.UnitPositions.ValidatePosition(ref Position, ref Index))
        {
            var hexY = GetHexY();
            var packMapCoordinate = HexGeometry.PackMapCoordinate(GetHexX(), hexY, 0);
            CombatMap.Current.UnitPositions.SetUnitPosition(Position, Index, packMapCoordinate);
            Images = AoWHSSet.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TCUNITS.ILB");
            UpdateImageIndex();
        }
        else
        {
            P.X = GetHexX();
            P.Y = GetHexY();
            P.MapLevel = 0;
            P.Tag = (sbyte)Direction;
            CombatMap.Current.UnitPositions.SetUnitPosition(Position, Index, P);
            Dispose();
        }
    }

    // AoWTC.pas:5049
    public override void Deactivate()
    {
        if (Activated() && ((MapLevel)Owner.Owner).Map.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            AoWHSSet.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TCUNITS.ILB");
            var unitPosition = CombatMap.Current.UnitPositions.GetUnitPosition(Position, Index);
            var hexX = GetHexX();
            bool condition = unitPosition.X == hexX;
            if (condition)
            {
                var fUnitPositionsUnitPosition = CombatMap.Current.UnitPositions.GetUnitPosition(Position, Index);
                var hexY = GetHexY();
                condition = fUnitPositionsUnitPosition.Y == hexY;
            }

            if (condition)
            {
                var packMapCoordinate = HexGeometry.PackMapCoordinate(-1, -1, -1);
                CombatMap.Current.UnitPositions.SetUnitPosition(Position, Index, packMapCoordinate);
            }
        }

        base.Deactivate();
    }

    // AoWTC.pas:5068
    public override bool CanPlace(MapContainer map, sbyte hexX, sbyte hexY, sbyte hexLevel) => map.GetField(hexX, hexY, hexLevel).FindOwnedChild(2228483) == null && CombatMap.Current.UnitPositions.ValidatePosition(ref Position, ref Index);

    // AoWTC.pas:5026
    public void UpdateImageIndex()
    {
        ImageIndex = Position * 8 + Index;
    }

    // AoWTC.pas:223

}
