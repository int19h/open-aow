#nullable disable warnings
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
public sealed class FloodControl(CustomNode? owner) : EngineObject(owner)
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
            Array.Resize(ref originalTerrain, newSize: 0);
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
            if (persister.Loading || persister.Translating())
            {
                sbyte[] originalTerrain = OriginalTerrain;
                Array.Resize(ref originalTerrain, newSize: count);
                OriginalTerrain = originalTerrain;
            }
            persister.Bytes("OriginalTerrain", data: MemoryMarshal.AsBytes(OriginalTerrain.AsSpan(0, length: count)));
        }
    }

    // GlobalEnchantmentSpells.pas:1090
    public void Setup()
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        Width = strategicMap.MapContainer.Width;
        Height = strategicMap.MapContainer.Height;
        sbyte[] originalTerrain = OriginalTerrain;
        Array.Resize(ref originalTerrain, newSize: Width * Height * 2);
        OriginalTerrain = originalTerrain;
        Array.Clear(OriginalTerrain);
    }

    // GlobalEnchantmentSpells.pas:1098
    public void Flood()
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.LockMapChangedEvent();
        MapLevel mapLevel = strategicMap.MapContainer.GetMapLevel(0);
        // { Original includes the last row when walking neighbouring hexagons. }
        MapField mapField;
        int index;
        for (int y = 1, last = mapLevel.Height - 1; y <= last; y++)
        {
            for (int x = 1, lastX = mapLevel.Width - 2; x <= lastX; x++)
            {
                if (mapLevel.GetField(x, y).Terrain == TerrainType.Water)
                {
                    HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, x, y, radius: 1);
                    while (walk.Radius == 1)
                    {
                        mapField = mapLevel.GetField(walk.X, walk.Y);
                        if ((mapField.Terrain is not (TerrainType.Water or TerrainType.Ice or TerrainType.Border) && mapField.OverlayType is not (OverlayType.Mountain or OverlayType.Hill or OverlayType.Structure)) || (mapField.Terrain == TerrainType.Water && mapField.OverlayType == OverlayType.Bridge))
                        {
                            index = ((Width * walk.Y) + walk.X) * 2;
                            if (OriginalTerrain[index] == 0 && OriginalTerrain[index + 1] == 0 && mapField.FindNoneTransparentHexSprite(typeof(Structure)) is null)
                            {
                                sbyte[] originalTerrain = OriginalTerrain;
                                originalTerrain[index] = mapField.TerrainNumber;
                                OriginalTerrain = originalTerrain;
                                sbyte[] originalTerrain2 = OriginalTerrain;
                                originalTerrain2[index + 1] = mapField.OverlayType.ToOriginal();
                                OriginalTerrain = originalTerrain2;
                            }

                            if (mapField.OverlayType is OverlayType.Road or OverlayType.Bridge)
                            {
                                HexSprite? roadHexSprite = mapField.FindOwnedHexSprite(typeof(AbstractRoad));
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
                    TerrainType terrainType = mapField.Terrain;
                    if (terrainType != TerrainType.Water)
                    {
                        mapField.SetTerrain(TerrainType.Water);
                        mapField.TerrainChanged();
                    }
                }

                index += 2;
            }
        }

        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
        engineStrategicMap.UnlockMapChangedEvent();
        engineStrategicMap.InvalidateMap();
    }

    // GlobalEnchantmentSpells.pas:1160
    public void Restore()
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.LockMapChangedEvent();
        MapLevel mapLevel = strategicMap.MapContainer.GetMapLevel(0);
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
                    if (mapField.Terrain is TerrainType.Water or TerrainType.Ice)
                    {
                        mapField.SetTerrain(TerrainType.FromOriginal(OriginalTerrain[index]));
                        mapField.TerrainChanged();
                    }
                }

                index += 2;
            }
        }

        MapObjectResourceList resources = GetEngine().GamePack.MapObjectResourceList;
        DelphiList<MapObjectResource> candidates = [];
        for (int y = 1, last = mapLevel.Height - 1; y <= last; y++)
        {
            index = ((Width * y) + 1) * 2;
            for (int x = 1, lastX = mapLevel.Width - 2; x <= lastX; x++)
            {
                int overlay = OriginalTerrain[index + 1];
                if (overlay > 0)
                {
                    mapField = mapLevel.GetField(x, y);
                    if ((byte)overlay is 4 or 5)
                    {
                        _ = GetEngine().GamePack.RoadControl.Place(GetEngine().StrategicMap.MapContainer, x.TruncateToSByte(), y.TruncateToSByte(), L: 0, terrainType: mapField.TerrainNumber);
                    }
                    else
                    {
                        candidates.Clear();
                        for (int i = 0, lastI = resources.Count - 1; i <= lastI; i++)
                        {
                            MapObjectResource resource = resources[i];
                            if (resource.EngineClass.ClassId == World.Hexes.TerrainMapObjectResource.ClassId && resource.TerrainList.UsedTerrain() == 1 && resource.TerrainList.Terrain[0] is { IsCovered: true } terrainCell && terrainCell.Value == mapField.Terrain && overlay == resource.TerrainList.Overlay[0].ToOriginal())
                            {
                                _ = candidates.Add(resource);
                            }
                        }

                        if (candidates.Count != 0)
                        {
                            _ = candidates[GetEngine().StrategicMap.Random(candidates.Count)].PlaceResourceUser(GetEngine().StrategicMap, x.TruncateToSByte(), y.TruncateToSByte(), 0);
                        }
                    }
                }

                index += 2;
            }
        }
        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
        engineStrategicMap.UnlockMapChangedEvent();
        engineStrategicMap.InvalidateMap();
    }
}
