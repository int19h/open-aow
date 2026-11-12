// Translated from HxBridge.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World.Hexes;

// _terrain, Direction (-1 invalid), SideA, SideB
public partial class HexBridge : AbstractRoad
{
    public HexBridge(ECustomNode owner) : base(owner)
    {
        Direction = -1;
        SideA = new ByteList();
        SideB = new ByteList();
    }

    // HxBridge.pas:84
    public new const int ClassId = 133242;

    public sbyte _terrain;
    public sbyte Direction;
    public ByteList SideA;
    public ByteList SideB;

    // HxBridge.pas:77
    public override void Dispose()
    {
        base.Dispose();
        SideA?.Dispose();
        SideB?.Dispose();
    }

    // HxBridge.pas:69

    // HxBridge.pas:100
    public override sbyte GetTerrain(int index)
    {
        sbyte result;
        if (index == 0)
        {
            result = _terrain;
        }
        else
        {
            result = -2;
        }

        return result;
    }

    // HxBridge.pas:89
    public override byte GetLevel() => 26;

    // HxBridge.pas:105
    public override int GetShowPriority()
    {
        int pixelX;
        int pixelY;
        var mapField = (MapField)Owner;
        pixelX = mapField.HexX * 32 + 8;
        pixelY = (mapField.HexX & 1) * 16 + mapField.HexY * 32;

        return 1744830464 + (pixelY << 13) + (pixelX + 8);
    }

    // HxBridge.pas:117
    public override void Connect()
    {
        HexNumberWalkData walk = default;
        MapLevel level;
        level = (MapLevel)Owner.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, ((MapField)Owner).HexX, ((MapField)Owner).HexY, 1);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                level.GetField(walk.X, walk.Y).AddHexSprite(this, walk.HexNumber);
                // { Original bug: the walk never advances when a neighbour is out of bounds. }
                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }

        base.Connect();
    }

    // HxBridge.pas:135
    public override void Disconnect()
    {
        HexNumberWalkData walk = default;
        MapLevel level;
        level = (MapLevel)Owner.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, ((MapField)Owner).HexX, ((MapField)Owner).HexY, 1);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                level.GetField(walk.X, walk.Y).RemoveHexSprite(this);
                // { Original bug: the walk never advances when a neighbour is out of bounds. }
                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }

        base.Disconnect();
    }

    // HxBridge.pas:327
    public override void ChangeTerrain(in HexSpriteTerrainInfo message)
    {
        base.ChangeTerrain(message);
    }

    // HxBridge.pas:321
    public override void NeighbourTerrainChanged(int hexNumber, in HexSpriteTerrainInfo message)
    {
        UpdateTransitions();
        if (Direction == -1)
        {
            Dispose();
        }
    }

    // HxBridge.pas:186
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(((MapField)Owner).HexX, ((MapField)Owner).HexY, ref pixelX, ref pixelY);
        _resource.ImageLibrary[Direction].Show(pixelX - pixelXSurface - 23, pixelY - pixelYSurface - 20, lockedSurface);
    }

    // HxBridge.pas:160
    public override void MoveExclusive(in HexSpriteSenderInfo info)
    {
        int hexX = default;
        int hexY = default;
        MapField mapField;
        if (info.HexNumber != 0)
        {
            if (SideA.IndexOf((byte)info.HexNumber) != -1)
            {
                for (int I = 0, last = SideB.Count - 1; I <= last; I++)
                {
                    var sideB = SideB[I];
                    HexGeometry.CenterHexNumberToHex(((MapField)Owner).HexX, ((MapField)Owner).HexY, sideB, ref hexX, ref hexY);
                    mapField = AoWHSMap.Current.MapContainer.GetField(hexX, hexY, ((MapField)Owner).HexLevel);
                    ((MoveControl)info.Sender).ExecuteExternalMove((AoWMapField)info.MapField, (AoWMapField)mapField, ((MoveControl)info.Sender).MovePointTable[mapField.TerrainType][mapField.OverlayType - -1]);
                }
            }
            else
            {
                if (SideB.IndexOf((byte)info.HexNumber) != -1)
                {
                    for (int I = 0, last = SideA.Count - 1; I <= last; I++)
                    {
                        var sideA = SideA[I];
                        HexGeometry.CenterHexNumberToHex(((MapField)Owner).HexX, ((MapField)Owner).HexY, sideA, ref hexX, ref hexY);
                        mapField = AoWHSMap.Current.MapContainer.GetField(hexX, hexY, ((MapField)Owner).HexLevel);
                        ((MoveControl)info.Sender).ExecuteExternalMove((AoWMapField)info.MapField, (AoWMapField)mapField, ((MoveControl)info.Sender).MovePointTable[mapField.TerrainType][mapField.OverlayType - -1]);
                    }
                }
            }
        }
    }

    // HxBridge.pas:194
    public override void Activate()
    {
        base.Activate();
        UpdateExclusiveMoves();
    }

    // HxBridge.pas:153
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (info.HexNumber != 0)
        {
            ((AoWMapField)info.MapField).FieldFlags = ((AoWMapField)info.MapField).FieldFlags | 32768;
        }
    }

    // HxBridge.pas:94
    public override bool LinkToResource(GameEngine engine)
    {
        bool result = base.LinkToResource(engine);
        _terrain = _resource.TerrainList.Terrain[0];
        return result;
    }

    // HxBridge.pas:200
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        MapField mapField;
        mapField = mapContainer.GetField(hexX, hexY, hexLevel);
        return base.CanPlace(mapContainer, hexX, hexY, hexLevel) && (DirectionValid(mapField, 0) || DirectionValid(mapField, 1) || DirectionValid(mapField, 2));
    }

    // HxBridge.pas:209
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        HexSprite road;
        MapField mapField;
        mapField = mapContainer.GetField(hexX, hexY, hexLevel);
        road = mapField.FindHexSprite(typeof(AbstractRoad));
        if (road != null)
        {
            road.Dispose();
        }

        ECustomNode previous = Owner;
        Owner = mapField;
        _status = _status + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
        _terrain = mapField.TerrainType;
        // { Original bug: refresh the old owner's owner as a map field. }
        // FOwner lives on TEObject, so the old owner has it. A nil owner or a non-field owner is undefined in the original, so skip it.
        if (previous?.Owner is MapField field)
        {
            field.UpdateOverlayType();
        }

        mapField.UpdateOverlayType();
        mapContainer.Map.InvalidateStaticScene(hexX, hexY, hexLevel);
        UpdateTransitions();
        return Direction != -1;
    }

    // HxBridge.pas:309
    public override void UpdateTransitions()
    {
        if (Direction == -1 || !DirectionValid((MapField)Owner, Direction))
        {
            if (DirectionValid((MapField)Owner, 1))
            {
                Direction = 1;
            }
            else
            {
                if (DirectionValid((MapField)Owner, 2))
                {
                    Direction = 2;
                }
                else
                {
                    if (DirectionValid((MapField)Owner, 0))
                    {
                        Direction = 0;
                    }
                    else
                    {
                        if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
                        {
                            Direction = -1;
                        }
                    }
                }
            }

            UpdateExclusiveMoves();
        }
    }

    // HxBridge.pas:229
    public bool DirectionValid(MapField mapField, sbyte direction)
    {
        MapLevel level;
        int hexX = default;
        int hexY = default;
        bool result = false;
        level = (MapLevel)mapField.Owner;
        HexGeometry.CenterHexNumberToHex(mapField.HexX, mapField.HexY, direction + 1, ref hexX, ref hexY);
        if (hexX >= 0 && hexY >= 0 && hexX < level.Width && hexY < level.Height)
        {
            if (Land(level.GetField(hexX, hexY).TerrainType))
            {
                HexGeometry.CenterHexNumberToHex(mapField.HexX, mapField.HexY, direction + 4, ref hexX, ref hexY);
                if (hexX >= 0 && hexY >= 0 && hexX < level.Width && hexY < level.Height)
                {
                    result = Land(level.GetField(hexX, hexY).TerrainType);
                }
            }
        }

        return result;
    }

    // HxBridge.pas:246
    public void UpdateExclusiveMoves()
    {
        int hexX;
        int hexY;
        int X = default;
        int Y = default;
        MapLevel level;
        SideB.Clear();
        SideA.Clear();
        if (Direction != -1)
        {
            level = (MapLevel)Owner.Owner;
            hexX = ((MapField)Owner).HexX;
            hexY = ((MapField)Owner).HexY;
            // { Original bug: the neighbour lookups here have no bounds checks. }
            switch (Direction)
            {
                case 0:
                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 2, ref X, ref Y);
                    if (!Land(level.GetField(X, Y).TerrainType))
                    {
                        SideB.Add(2);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 3, ref X, ref Y);
                    if (!Land(level.GetField(X, Y).TerrainType))
                    {
                        SideB.Add(3);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 5, ref X, ref Y);
                    if (!Land(level.GetField(X, Y).TerrainType))
                    {
                        SideA.Add(5);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 6, ref X, ref Y);
                    if (!Land(level.GetField(X, Y).TerrainType))
                    {
                        SideA.Add(6);
                    }

                    break;
                case 1:
                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 3, ref X, ref Y);
                    if (!Land(level.GetField(X, Y).TerrainType))
                    {
                        SideB.Add(3);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 4, ref X, ref Y);
                    if (!Land(level.GetField(X, Y).TerrainType))
                    {
                        SideB.Add(4);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 6, ref X, ref Y);
                    if (!Land(level.GetField(X, Y).TerrainType))
                    {
                        SideA.Add(6);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 1, ref X, ref Y);
                    if (!Land(level.GetField(X, Y).TerrainType))
                    {
                        SideA.Add(1);
                    }

                    break;
                case 2:
                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 1, ref X, ref Y);
                    if (!Land(level.GetField(X, Y).TerrainType))
                    {
                        SideB.Add(1);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 2, ref X, ref Y);
                    if (!Land(level.GetField(X, Y).TerrainType))
                    {
                        SideB.Add(2);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 4, ref X, ref Y);
                    if (!Land(level.GetField(X, Y).TerrainType))
                    {
                        SideA.Add(4);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 5, ref X, ref Y);
                    if (!Land(level.GetField(X, Y).TerrainType))
                    {
                        SideA.Add(5);
                    }

                    break;
            }
        }
    }

    private static bool Land(sbyte terrain)
    {
        return !Set<byte>.WithBounds(0, 255).With((byte)0).With((byte)6)
            .With((byte)9)
            .With((byte)10)
            .With((byte)13)
            .Contains((byte)terrain);
    }

    public new partial class Resource : AbstractRoadResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            Level = 10;
        }

        // HxBridge.pas:348
        public new const int ClassId = 133243;

        // HxBridge.pas:332

        // HxBridge.pas:338
        public override int ResourceUserClassId() => 133242;

        // HxBridge.pas:49

        // HxBridge.pas:343
        public override string ResourceEditName() => HexText.s_bridgeText;

    }

    // HxBridge.pas:25

    // HxBridge.pas:25

}
