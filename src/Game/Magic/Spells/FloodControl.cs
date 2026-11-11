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

// FWidth, FHeight, FOriginalTerrain
public partial class FloodControl : EngineObject
{
    public int FWidth;
    public int FHeight;
    public sbyte[] FOriginalTerrain = [];

    // GlobalEnchantmentSpells.pas:1072
    public override void Dispose()
    {
        if (FOriginalTerrain.Length != 0)
        {
            Array.Resize(ref FOriginalTerrain, 0);
        }

        base.Dispose();
    }

    // GlobalEnchantmentSpells.pas:1078
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FWidth, 20);
        Stream.rwInteger(ref FHeight, 21);
        if (FWidth != 0)
        {
            if (Stream.StorageMode().Contains(StorageModeElement.Read))
            {
                Array.Resize(ref FOriginalTerrain, FWidth * FHeight * 2);
            }

            Stream.rwData(MemoryMarshal.AsBytes(FOriginalTerrain.AsSpan(0, FWidth * FHeight * 2)), FWidth * FHeight * 2, 22);
        }
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int("Width", ref FWidth);
        persister.Int("Height", ref FHeight);
        if (FWidth != 0)
        {
            int count = FWidth * FHeight * 2;
            if (persister.Loading) Array.Resize(ref FOriginalTerrain, count);
            persister.Bytes("OriginalTerrain", MemoryMarshal.AsBytes(FOriginalTerrain.AsSpan(0, count)));
        }
    }

    // GlobalEnchantmentSpells.pas:1066
    public FloodControl(ECustomNode owner) : base(owner)
    {
        FOriginalTerrain = [];
    }

    // GlobalEnchantmentSpells.pas:1090
    public void Setup()
    {
        FWidth = AoWHSMap.Current.MapContainer.Width;
        FHeight = AoWHSMap.Current.MapContainer.Height;
        Array.Resize(ref FOriginalTerrain, FWidth * FHeight * 2);
        Array.Clear(FOriginalTerrain);
    }

    // GlobalEnchantmentSpells.pas:1098
    public void Flood()
    {
        MapLevel MapLevel;
        HexNumberWalkData Walk = default;
        MapField MapF;
        int Index;
        HexSprite RoadHS;
        sbyte TerrainType;
        AoWHSMap.Current.LockMapChangedEvent();
        MapLevel = AoWHSMap.Current.MapContainer.GetMapLevel(0);
        // { Original includes the last row when walking neighbouring hexagons. }
        for (int Y = 1, last = MapLevel.Height - 1; Y <= last; Y++)
        {
            for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
            {
                if (MapLevel.GetField(X, Y).TerrainType == 0)
                {
                    HexNumberWalkData.InitHNWalk(ref Walk, X, Y, 1);
                    while (Walk.Radius == 1)
                    {
                        MapF = MapLevel.GetField(Walk.X, Walk.Y);
                        if ((!Set<byte>.WithBounds(0, 255).With(0).With(6).With(15).Contains((byte)MapF.TerrainType) && !Set<byte>.WithBounds(0, 255).With(0).With(2).With(7).Contains((byte)MapF.OverlayType)) || (MapF.TerrainType == 0 && MapF.OverlayType == 5))
                        {
                            Index = (FWidth * Walk.Y + Walk.X) * 2;
                            if (FOriginalTerrain[Index] == 0 && FOriginalTerrain[Index + 1] == 0 && MapF.FindNoneTransparentHS(typeof(Structure)) == null)
                            {
                                FOriginalTerrain[Index] = MapF.TerrainType;
                                FOriginalTerrain[Index + 1] = MapF.OverlayType;
                            }

                            if (Set<byte>.WithBounds(0, 255).With(4).With(5).Contains((byte)MapF.OverlayType))
                            {
                                RoadHS = MapF.FindOwnedHS(typeof(AbstractRoad));
                                if (RoadHS != null)
                                {
                                    RoadHS.Dispose();
                                }
                            }
                        }

                        HexNumberWalkData.WalkToNextHN(ref Walk);
                    }
                }
            }
        }

        for (int Y = 1, last = MapLevel.Height - 1; Y <= last; Y++)
        {
            Index = (FWidth * Y + 1) * 2;
            for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
            {
                if (FOriginalTerrain[Index] != 0)
                {
                    MapF = MapLevel.GetField(X, Y);
                    TerrainType = MapF.TerrainType;
                    if (TerrainType != 0)
                    {
                        MapF.TerrainType = 0;
                        MapF.TerrainChanged();
                    }
                }

                Index += 2;
            }
        }

        AoWHSMap.Current.UnlockMapChangedEvent();
        AoWHSMap.Current.InvalidateMap();
    }

    // GlobalEnchantmentSpells.pas:1160
    public void Restore()
    {
        EngineObjectList Resources;
        DelphiList Candidates;
        int Index;
        MapField MapF;
        MapLevel MapLevel;
        sbyte Overlay;
        MapObjectResource Resource;
        AoWHSMap.Current.LockMapChangedEvent();
        MapLevel = AoWHSMap.Current.MapContainer.GetMapLevel(0);
        for (int Y = 1, last = MapLevel.Height - 1; Y <= last; Y++)
        {
            Index = (FWidth * Y + 1) * 2;
            for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
            {
                if (FOriginalTerrain[Index] != 0)
                {
                    MapF = MapLevel.GetField(X, Y);
                    if (Set<byte>.WithBounds(0, 255).With(0).With(6).Contains((byte)MapF.TerrainType))
                    {
                        MapF.TerrainType = FOriginalTerrain[Index];
                        MapF.TerrainChanged();
                    }
                }

                Index += 2;
            }
        }

        Resources = AoWHSSet.Current.MapObjectResourceList.FChildren;
        Candidates = new DelphiList();
        for (int Y = 1, last = MapLevel.Height - 1; Y <= last; Y++)
        {
            Index = (FWidth * Y + 1) * 2;
            for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
            {
                Overlay = FOriginalTerrain[Index + 1];
                if (Overlay > 0)
                {
                    MapF = MapLevel.GetField(X, Y);
                    if (Set<byte>.WithBounds(0, 255).With(4).With(5).Contains((byte)Overlay))
                    {
                        AoWHSSet.Current.RoadControl.Place(AoWHSMap.Current.MapContainer, (sbyte)X, (sbyte)Y, 0, MapF.TerrainType);
                    }
                    else
                    {
                        Candidates.Clear();
                        for (int I = 0, lastI = Resources.Count - 1; I <= lastI; I++)
                        {
                            Resource = (MapObjectResource)Resources.List[I];
                            if (Resource.EClass.ClassID == 131431 && Resource.TerrainList.UsedTerrain() == 1 && Resource.TerrainList.Terrain[0] == MapF.TerrainType && Overlay == Resource.TerrainList.Overlay[0])
                            {
                                Candidates.Add(Resource);
                            }
                        }

                        if (Candidates.Count != 0)
                        {
                            ((MapObjectResource)Candidates[AoWHSMap.Current.Random(Candidates.Count)]).PlaceResourceUser(AoWHSMap.Current, (sbyte)X, (sbyte)Y, 0);
                        }
                    }
                }

                Index += 2;
            }
        }

        Candidates?.Dispose();
        AoWHSMap.Current.UnlockMapChangedEvent();
        AoWHSMap.Current.InvalidateMap();
    }

}
