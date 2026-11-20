// Translated from HxBridge.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World.Hexes;

// _terrain, Direction (-1 invalid), SideA, SideB
public class HexBridge(CustomNode owner) : AbstractRoad(owner)
{

    // HxBridge.pas:84
    public new const int ClassId = 133242;

    private sbyte _terrain;

    public sbyte Direction { get; private set; } = -1;

    private readonly ByteList _sideA = new();
    private readonly ByteList _sideB = new();

    // HxBridge.pas:77
    public override void Dispose()
    {
        base.Dispose();
        _sideA?.Dispose();
        _sideB?.Dispose();
    }

    // HxBridge.pas:100
    public override sbyte GetTerrain(int index) => index == 0 ? _terrain : (sbyte)-2;

    // HxBridge.pas:89
    public override byte Level => 26;

    // HxBridge.pas:105
    public override int GetShowPriority()
    {
        MapField mapField = Owner;
        int pixelX = (mapField.HexX * 32) + 8;
        int pixelY = ((mapField.HexX & 1) * 16) + (mapField.HexY * 32);

        return 1744830464 + (pixelY << 13) + pixelX + 8;
    }

    // HxBridge.pas:117
    public override void Connect()
    {
        MapLevel level = Owner.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, Owner.HexX, Owner.HexY, 1);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                _ = level.GetField(walk.X, walk.Y).AddHexSprite(this, walk.HexNumber);
                // { Original bug: the walk never advances when a neighbour is out of bounds. }
                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }

        base.Connect();
    }

    // HxBridge.pas:135
    public override void Disconnect()
    {
        MapLevel level = Owner.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, Owner.HexX, Owner.HexY, 1);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                _ = level.GetField(walk.X, walk.Y).RemoveHexSprite(this);
                // { Original bug: the walk never advances when a neighbour is out of bounds. }
                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }

        base.Disconnect();
    }

    // HxBridge.pas:327
    public override void ChangeTerrain(in HexSpriteTerrainInfo message) => base.ChangeTerrain(message);

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
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        base.Resource.ImageLibrary[Direction].Show(pixelX - pixelXSurface - 23, pixelY - pixelYSurface - 20, lockedSurface);
    }

    // HxBridge.pas:160
    public override void MoveExclusive(in HexSpriteSenderInfo info)
    {
        if (info.HexNumber != 0)
        {
            int hexX = default;
            int hexY = default;
            MapField mapField;
            if (_sideA.IndexOf((byte)info.HexNumber) != -1)
            {
                for (int i = 0, last = _sideB.Count - 1; i <= last; i++)
                {
                    byte sideB = _sideB[i];
                    HexGeometry.CenterHexNumberToHex(Owner.HexX, Owner.HexY, sideB, ref hexX, ref hexY);
                    mapField = StrategicMap.Current.MapContainer.GetField(hexX, hexY, Owner.HexLevel);
                    ((MoveControl)info.Sender).ExecuteExternalMove(info.MapField, mapField, ((MoveControl)info.Sender).MovePointTable[mapField.TerrainType][mapField.OverlayType - MovementTables.FirstOverlay]);
                }
            }
            else
            {
                if (_sideB.IndexOf((byte)info.HexNumber) != -1)
                {
                    for (int i = 0, last = _sideA.Count - 1; i <= last; i++)
                    {
                        byte sideA = _sideA[i];
                        HexGeometry.CenterHexNumberToHex(Owner.HexX, Owner.HexY, sideA, ref hexX, ref hexY);
                        mapField = StrategicMap.Current.MapContainer.GetField(hexX, hexY, Owner.HexLevel);
                        ((MoveControl)info.Sender).ExecuteExternalMove(info.MapField, mapField, ((MoveControl)info.Sender).MovePointTable[mapField.TerrainType][mapField.OverlayType - MovementTables.FirstOverlay]);
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
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveExclusiveFlag;
        }
    }

    // HxBridge.pas:94
    public override bool LinkToResource(GameEngine engine)
    {
        bool result = base.LinkToResource(engine);
        _terrain = base.Resource.TerrainList.Terrain[0];
        return result;
    }

    // HxBridge.pas:200
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        MapField mapField = mapContainer.GetField(hexX, hexY, hexLevel);
        return base.CanPlace(mapContainer, hexX, hexY, hexLevel) && (DirectionValid(mapField, 0) || DirectionValid(mapField, 1) || DirectionValid(mapField, 2));
    }

    // HxBridge.pas:209
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        MapField mapField = mapContainer.GetField(hexX, hexY, hexLevel);
        HexSprite road = mapField.FindHexSprite(typeof(AbstractRoad));
        road?.Dispose();

        CustomNode previous = Owner;
        Owner = mapField;
        Status += Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
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
        if (Direction == -1 || !DirectionValid(Owner, Direction))
        {
            if (DirectionValid(Owner, 1))
            {
                Direction = 1;
            }
            else
            {
                if (DirectionValid(Owner, 2))
                {
                    Direction = 2;
                }
                else
                {
                    if (DirectionValid(Owner, 0))
                    {
                        Direction = 0;
                    }
                    else
                    {
                        if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
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
        int hexX = default;
        int hexY = default;
        MapLevel level = mapField.Owner;
        HexGeometry.CenterHexNumberToHex(mapField.HexX, mapField.HexY, direction + 1, ref hexX, ref hexY);
        if (hexX >= 0 && hexY >= 0 && hexX < level.Width && hexY < level.Height)
        {
            if (Land(level.GetField(hexX, hexY).TerrainType))
            {
                HexGeometry.CenterHexNumberToHex(mapField.HexX, mapField.HexY, direction + 4, ref hexX, ref hexY);
                if (hexX >= 0 && hexY >= 0 && hexX < level.Width && hexY < level.Height)
                {
                    return Land(level.GetField(hexX, hexY).TerrainType);
                }
            }
        }

        return false;
    }

    // HxBridge.pas:246
    public void UpdateExclusiveMoves()
    {
        _sideB.Clear();
        _sideA.Clear();
        if (Direction != -1)
        {
            MapLevel level = Owner.Owner;
            int hexX = Owner.HexX;
            int hexY = Owner.HexY;
            // { Original bug: the neighbour lookups here have no bounds checks. }
            int x = default;
            int y = default;
            switch (Direction)
            {
                case 0:
                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 2, ref x, ref y);
                    if (!Land(level.GetField(x, y).TerrainType))
                    {
                        _ = _sideB.Add(2);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 3, ref x, ref y);
                    if (!Land(level.GetField(x, y).TerrainType))
                    {
                        _ = _sideB.Add(3);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 5, ref x, ref y);
                    if (!Land(level.GetField(x, y).TerrainType))
                    {
                        _ = _sideA.Add(5);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 6, ref x, ref y);
                    if (!Land(level.GetField(x, y).TerrainType))
                    {
                        _ = _sideA.Add(6);
                    }

                    break;
                case 1:
                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 3, ref x, ref y);
                    if (!Land(level.GetField(x, y).TerrainType))
                    {
                        _ = _sideB.Add(3);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 4, ref x, ref y);
                    if (!Land(level.GetField(x, y).TerrainType))
                    {
                        _ = _sideB.Add(4);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 6, ref x, ref y);
                    if (!Land(level.GetField(x, y).TerrainType))
                    {
                        _ = _sideA.Add(6);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 1, ref x, ref y);
                    if (!Land(level.GetField(x, y).TerrainType))
                    {
                        _ = _sideA.Add(1);
                    }

                    break;
                case 2:
                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 1, ref x, ref y);
                    if (!Land(level.GetField(x, y).TerrainType))
                    {
                        _ = _sideB.Add(1);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 2, ref x, ref y);
                    if (!Land(level.GetField(x, y).TerrainType))
                    {
                        _ = _sideB.Add(2);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 4, ref x, ref y);
                    if (!Land(level.GetField(x, y).TerrainType))
                    {
                        _ = _sideA.Add(4);
                    }

                    HexGeometry.CenterHexNumberToHex(hexX, hexY, 5, ref x, ref y);
                    if (!Land(level.GetField(x, y).TerrainType))
                    {
                        _ = _sideA.Add(5);
                    }

                    break;
                default:
                    break;
            }
        }
    }

    private static bool Land(sbyte terrain)
    {
        return !Set<byte>.WithBounds(0, 255).With(0).With(6)
            .With(9)
            .With(10)
            .With(13)
            .Contains((byte)terrain);
    }

    public new class Resource : AbstractRoadResource
    {
        public Resource(CustomNode owner) : base(owner) => Level = 10;

        // HxBridge.pas:348
        public new const int ClassId = 133243;

        // HxBridge.pas:338
        public override int ResourceUserClassId() => 133242;

        // HxBridge.pas:343
        public override string ResourceEditName() => HexText.BridgeText;

    }
}
