#nullable disable
// Translated from GlobalEnchantmentSpells.pas.
using System.Runtime.InteropServices;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// Width, Height, OriginalTerrain
public class FloodControl(CustomNode owner) : EngineObject(owner)
{

    public int Width { get; set; }
    public int Height { get; set; }

    public sbyte[] OriginalTerrain { get; internal set; } = [];

    // GlobalEnchantmentSpells.pas:1072
    public override void Dispose()
    {
        if (OriginalTerrain.Length != 0)
        {
            sbyte[] originalTerrain = OriginalTerrain;
            Array.Resize(ref originalTerrain, 0);
            OriginalTerrain = originalTerrain;
        }

        base.Dispose();
    }

    // GlobalEnchantmentSpells.pas:1078
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Width = persister.Int32("Width", Width);
        Height = persister.Int32("Height", Height);
        if (Width != 0)
        {
            int count = Width * Height * 2;
            // The translate stream's mode holds read, so the original ReadWrite sizes the array there too.
            if (persister.Loading || PersistSupport.Translating(persister))
            {
                sbyte[] originalTerrain = OriginalTerrain;
                Array.Resize(ref originalTerrain, count);
                OriginalTerrain = originalTerrain;
            }
            persister.Bytes("OriginalTerrain", MemoryMarshal.AsBytes(OriginalTerrain.AsSpan(0, count)));
        }
    }

    // GlobalEnchantmentSpells.pas:1090
    public void Setup()
    {
        Width = StrategicMap.Current.MapContainer.Width;
        Height = StrategicMap.Current.MapContainer.Height;
        sbyte[] originalTerrain = OriginalTerrain;
        Array.Resize(ref originalTerrain, Width * Height * 2);
        OriginalTerrain = originalTerrain;
        Array.Clear(OriginalTerrain);
    }

    // GlobalEnchantmentSpells.pas:1098
    public void Flood()
    {
        StrategicMap.Current.LockMapChangedEvent();
        MapLevel mapLevel = StrategicMap.Current.MapContainer.GetMapLevel(0);
        // { Original includes the last row when walking neighbouring hexagons. }
        MapField mapField;
        int index;
        for (int y = 1, last = mapLevel.Height - 1; y <= last; y++)
        {
            for (int x = 1, lastX = mapLevel.Width - 2; x <= lastX; x++)
            {
                if (mapLevel.GetField(x, y).TerrainType == 0)
                {
                    HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, x, y, 1);
                    while (walk.Radius == 1)
                    {
                        mapField = mapLevel.GetField(walk.X, walk.Y);
                        if ((!Set<byte>.WithBounds(0, 255).With(0).With(6).With(15).Contains((byte)mapField.TerrainType) && !Set<byte>.WithBounds(0, 255).With(0).With(2).With(7).Contains((byte)mapField.OverlayType)) || (mapField.TerrainType == 0 && mapField.OverlayType == 5))
                        {
                            index = ((Width * walk.Y) + walk.X) * 2;
                            if (OriginalTerrain[index] == 0 && OriginalTerrain[index + 1] == 0 && mapField.FindNoneTransparentHexSprite(typeof(Structure)) is null)
                            {
                                sbyte[] originalTerrain = OriginalTerrain;
                                originalTerrain[index] = mapField.TerrainType;
                                OriginalTerrain = originalTerrain;
                                sbyte[] originalTerrain2 = OriginalTerrain;
                                originalTerrain2[index + 1] = mapField.OverlayType;
                                OriginalTerrain = originalTerrain2;
                            }

                            if (Set<byte>.WithBounds(0, 255).With(4).With(5).Contains((byte)mapField.OverlayType))
                            {
                                HexSprite roadHexSprite = mapField.FindOwnedHexSprite(typeof(AbstractRoad));
                                roadHexSprite?.Dispose();
                            }
                        }

                        HexNumberWalkData.WalkToNextHexNumber(ref walk);
                    }
                }
            }
        }

        for (int y = 1, last = mapLevel.Height - 1; y <= last; y++)
        {
            index = ((Width * y) + 1) * 2;
            for (int x = 1, lastX = mapLevel.Width - 2; x <= lastX; x++)
            {
                if (OriginalTerrain[index] != 0)
                {
                    mapField = mapLevel.GetField(x, y);
                    sbyte terrainType = mapField.TerrainType;
                    if (terrainType != 0)
                    {
                        mapField.TerrainType = 0;
                        mapField.TerrainChanged();
                    }
                }

                index += 2;
            }
        }

        StrategicMap.Current.UnlockMapChangedEvent();
        StrategicMap.Current.InvalidateMap();
    }

    // GlobalEnchantmentSpells.pas:1160
    public void Restore()
    {
        StrategicMap.Current.LockMapChangedEvent();
        MapLevel mapLevel = StrategicMap.Current.MapContainer.GetMapLevel(0);
        int index;
        MapField mapField;
        for (int y = 1, last = mapLevel.Height - 1; y <= last; y++)
        {
            index = ((Width * y) + 1) * 2;
            for (int x = 1, lastX = mapLevel.Width - 2; x <= lastX; x++)
            {
                if (OriginalTerrain[index] != 0)
                {
                    mapField = mapLevel.GetField(x, y);
                    if (Set<byte>.WithBounds(0, 255).With(0).With(6).Contains((byte)mapField.TerrainType))
                    {
                        mapField.TerrainType = OriginalTerrain[index];
                        mapField.TerrainChanged();
                    }
                }

                index += 2;
            }
        }

        MapObjectResourceList resources = GamePack.Current.MapObjectResourceList;
        DelphiList<MapObjectResource> candidates = new();
        for (int y = 1, last = mapLevel.Height - 1; y <= last; y++)
        {
            index = ((Width * y) + 1) * 2;
            for (int x = 1, lastX = mapLevel.Width - 2; x <= lastX; x++)
            {
                sbyte overlay = OriginalTerrain[index + 1];
                if (overlay > 0)
                {
                    mapField = mapLevel.GetField(x, y);
                    if (Set<byte>.WithBounds(0, 255).With(4).With(5).Contains((byte)overlay))
                    {
                        _ = GamePack.Current.RoadControl.Place(StrategicMap.Current.MapContainer, (sbyte)x, (sbyte)y, 0, mapField.TerrainType);
                    }
                    else
                    {
                        candidates.Clear();
                        for (int i = 0, lastI = resources.Count - 1; i <= lastI; i++)
                        {
                            MapObjectResource resource = resources[i];
                            if (resource.EngineClass.ClassId == 131431 && resource.TerrainList.UsedTerrain() == 1 && resource.TerrainList.Terrain[0] == mapField.TerrainType && overlay == resource.TerrainList.Overlay[0])
                            {
                                _ = candidates.Add(resource);
                            }
                        }

                        if (candidates.Count != 0)
                        {
                            _ = candidates[StrategicMap.Current.Random(candidates.Count)].PlaceResourceUser(StrategicMap.Current, (sbyte)x, (sbyte)y, 0);
                        }
                    }
                }

                index += 2;
            }
        }

        candidates?.Dispose();
        StrategicMap.Current.UnlockMapChangedEvent();
        StrategicMap.Current.InvalidateMap();
    }
}
