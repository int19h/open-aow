// Translated from GlobalEnchantmentSpells.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// Width, Height, OriginalTerrain
public partial class FloodControl : EngineObject
{
    public int Width;
    public int Height;
    public sbyte[] OriginalTerrain = [];

    // GlobalEnchantmentSpells.pas:1072
    public override void Dispose()
    {
        if (OriginalTerrain.Length != 0)
        {
            Array.Resize(ref OriginalTerrain, 0);
        }

        base.Dispose();
    }

    // GlobalEnchantmentSpells.pas:1078
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref Width, 20);
        stream.rwInteger(ref Height, 21);
        if (Width != 0)
        {
            if (stream.StorageMode().Contains(StorageModeElement.Read))
            {
                Array.Resize(ref OriginalTerrain, Width * Height * 2);
            }

            stream.rwData(MemoryMarshal.AsBytes(OriginalTerrain.AsSpan(0, Width * Height * 2)), Width * Height * 2, 22);
        }
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int32("Width", ref Width);
        persister.Int32("Height", ref Height);
        if (Width != 0)
        {
            int count = Width * Height * 2;
            if (persister.Loading) Array.Resize(ref OriginalTerrain, count);
            persister.Bytes("OriginalTerrain", MemoryMarshal.AsBytes(OriginalTerrain.AsSpan(0, count)));
        }
    }

    // GlobalEnchantmentSpells.pas:1066
    public FloodControl(ECustomNode owner) : base(owner)
    {
        OriginalTerrain = [];
    }

    // GlobalEnchantmentSpells.pas:1090
    public void Setup()
    {
        Width = AoWHSMap.Current.MapContainer.Width;
        Height = AoWHSMap.Current.MapContainer.Height;
        Array.Resize(ref OriginalTerrain, Width * Height * 2);
        Array.Clear(OriginalTerrain);
    }

    // GlobalEnchantmentSpells.pas:1098
    public void Flood()
    {
        MapLevel mapLevel;
        HexNumberWalkData walk = default;
        MapField mapField;
        int index;
        HexSprite roadHexSprite;
        sbyte terrainType;
        AoWHSMap.Current.LockMapChangedEvent();
        mapLevel = AoWHSMap.Current.MapContainer.GetMapLevel(0);
        // { Original includes the last row when walking neighbouring hexagons. }
        for (int Y = 1, last = mapLevel.Height - 1; Y <= last; Y++)
        {
            for (int X = 1, lastX = mapLevel.Width - 2; X <= lastX; X++)
            {
                if (mapLevel.GetField(X, Y).TerrainType == 0)
                {
                    HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 1);
                    while (walk.Radius == 1)
                    {
                        mapField = mapLevel.GetField(walk.X, walk.Y);
                        if ((!Set<byte>.WithBounds(0, 255).With(0).With(6).With(15).Contains((byte)mapField.TerrainType) && !Set<byte>.WithBounds(0, 255).With(0).With(2).With(7).Contains((byte)mapField.OverlayType)) || (mapField.TerrainType == 0 && mapField.OverlayType == 5))
                        {
                            index = (Width * walk.Y + walk.X) * 2;
                            if (OriginalTerrain[index] == 0 && OriginalTerrain[index + 1] == 0 && mapField.FindNoneTransparentHexSprite(typeof(Structure)) == null)
                            {
                                OriginalTerrain[index] = mapField.TerrainType;
                                OriginalTerrain[index + 1] = mapField.OverlayType;
                            }

                            if (Set<byte>.WithBounds(0, 255).With(4).With(5).Contains((byte)mapField.OverlayType))
                            {
                                roadHexSprite = mapField.FindOwnedHexSprite(typeof(AbstractRoad));
                                if (roadHexSprite != null)
                                {
                                    roadHexSprite.Dispose();
                                }
                            }
                        }

                        HexNumberWalkData.WalkToNextHexNumber(ref walk);
                    }
                }
            }
        }

        for (int Y = 1, last = mapLevel.Height - 1; Y <= last; Y++)
        {
            index = (Width * Y + 1) * 2;
            for (int X = 1, lastX = mapLevel.Width - 2; X <= lastX; X++)
            {
                if (OriginalTerrain[index] != 0)
                {
                    mapField = mapLevel.GetField(X, Y);
                    terrainType = mapField.TerrainType;
                    if (terrainType != 0)
                    {
                        mapField.TerrainType = 0;
                        mapField.TerrainChanged();
                    }
                }

                index += 2;
            }
        }

        AoWHSMap.Current.UnlockMapChangedEvent();
        AoWHSMap.Current.InvalidateMap();
    }

    // GlobalEnchantmentSpells.pas:1160
    public void Restore()
    {
        MapObjectResourceList resources;
        DelphiList candidates;
        int index;
        MapField mapField;
        MapLevel mapLevel;
        sbyte overlay;
        MapObjectResource resource;
        AoWHSMap.Current.LockMapChangedEvent();
        mapLevel = AoWHSMap.Current.MapContainer.GetMapLevel(0);
        for (int Y = 1, last = mapLevel.Height - 1; Y <= last; Y++)
        {
            index = (Width * Y + 1) * 2;
            for (int X = 1, lastX = mapLevel.Width - 2; X <= lastX; X++)
            {
                if (OriginalTerrain[index] != 0)
                {
                    mapField = mapLevel.GetField(X, Y);
                    if (Set<byte>.WithBounds(0, 255).With(0).With(6).Contains((byte)mapField.TerrainType))
                    {
                        mapField.TerrainType = OriginalTerrain[index];
                        mapField.TerrainChanged();
                    }
                }

                index += 2;
            }
        }

        resources = AoWHSSet.Current.MapObjectResourceList;
        candidates = new DelphiList();
        for (int Y = 1, last = mapLevel.Height - 1; Y <= last; Y++)
        {
            index = (Width * Y + 1) * 2;
            for (int X = 1, lastX = mapLevel.Width - 2; X <= lastX; X++)
            {
                overlay = OriginalTerrain[index + 1];
                if (overlay > 0)
                {
                    mapField = mapLevel.GetField(X, Y);
                    if (Set<byte>.WithBounds(0, 255).With(4).With(5).Contains((byte)overlay))
                    {
                        AoWHSSet.Current.RoadControl.Place(AoWHSMap.Current.MapContainer, (sbyte)X, (sbyte)Y, 0, mapField.TerrainType);
                    }
                    else
                    {
                        candidates.Clear();
                        for (int I = 0, lastI = resources.Count - 1; I <= lastI; I++)
                        {
                            resource = resources[I];
                            if (resource.EngineClass.ClassId == 131431 && resource.TerrainList.UsedTerrain() == 1 && resource.TerrainList.Terrain[0] == mapField.TerrainType && overlay == resource.TerrainList.Overlay[0])
                            {
                                candidates.Add(resource);
                            }
                        }

                        if (candidates.Count != 0)
                        {
                            ((MapObjectResource)candidates[AoWHSMap.Current.Random(candidates.Count)]).PlaceResourceUser(AoWHSMap.Current, (sbyte)X, (sbyte)Y, 0);
                        }
                    }
                }

                index += 2;
            }
        }

        candidates?.Dispose();
        AoWHSMap.Current.UnlockMapChangedEvent();
        AoWHSMap.Current.InvalidateMap();
    }

}
