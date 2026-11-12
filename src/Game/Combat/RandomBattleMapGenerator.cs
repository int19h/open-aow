// Translated from AoWTC.pas.
using Aow.Engine.Buffers;

using Aow.Formats;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Saves;

using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// FItems. FRandomTerrain. neighbor weights. terrain weights. terrain-tag weights. terrain
// samples. tag samples. resource lists. FHomePosition
public partial class RandomBattleMapGenerator : IDisposable
{
    public ItemList Items;
    // calls GetItem
    public bool RandomTerrain;
    public Int32Array20 NeighborWeights;
    public Int32Array305 TerrainWeights;
    public Int32Array308 TagWeights;
    public SByteArray7 Terrain;
    public SByteArray7 Tags;
    public MapObjectResourceListArray286 Resources;
    // original allocates one extra list
    public int HomePosition;

    // AoWTC.pas:4149
    public virtual void Dispose()
    {
        for (int I = 0; I <= 285; I++)
        {
            Resources[I]?.Clear();
        }
    }

    // AoWTC.pas:4137
    public RandomBattleMapGenerator()
    {
        for (int I = 0; I <= 285; I++)
        {
            var list = new List<MapObjectResource>();
            Resources[I] = list;
        }

        for (int J = 0, last = AoWHSSet.Current.CombatResources.GetCount() - 1; J <= last; J++)
        {
            if (AoWHSSet.Current.CombatResources.Resources[J] is CombatTerrain.Resource)
            {
                var mapObjectResource = AoWHSSet.Current.CombatResources.Resources[J];
                var mapObjectResource2 = AoWHSSet.Current.CombatResources.Resources[J];
                var mapObjectResource3 = AoWHSSet.Current.CombatResources.Resources[J];
                Resources[(sbyte)((TerrainMapObjectResource)mapObjectResource2).TerrainType * 19 + ((TerrainMapObjectResource)mapObjectResource3).DecorationType].Add(mapObjectResource);
            }
        }

        InitializeTables();
    }

    // AoWTC.pas:4156
    public CombatMap RandomMap(TacticalBattle combat, int X, int Y, int L)
    {
        byte[] terrainBuffer = [];
        int I;
        int J = default;
        int K = default;
        int hexX = default;
        int terrainType = default;
        int tag = default;
        byte[] shapeBuffer = [];
        byte shape = default;
        bool hasUnit = default;
        bool siteCombat;
        bool randomTerrain;
        string fileName;
        Structure structure;
        HexBridge bridge;
        Int32Array20 weights = default;
        bool Blocked(int X, int Y, int first, int last)
        {
            int kind;
            int hexX = default;
            int hexY = default;
            bool result = false;
            for (int I = first; I <= last; I++)
            {
                HexGeometry.CenterHexNumberToHex(X, Y, s_clusterCheckBox[I], ref hexX, ref hexY);
                kind = terrainBuffer[hexY * 65 + hexX];
                result = result || kind == 1 || kind == 2 || kind == 3 || kind == 5 || kind == 6 || kind == 7 || kind == 11 || kind == 14 || kind == 15 || kind == 16 || kind == 17 || kind == 18 || kind == 252 || kind == 253 || kind == 254;
            }

            return result;
        }

        Items = null;
        if (combat == null)
        {
            siteCombat = false;
        }
        else
        {
            siteCombat = combat.ExplorationSite == 1;
        }

        if (combat == null)
        {
            randomTerrain = true;
        }
        else
        {
            randomTerrain = !(siteCombat || combat.ExplorationSite == 2);
        }

        RandomTerrain = randomTerrain;
        CombatPlacement.s_combatXLocation = X;
        CombatPlacement.s_combatYLocation = Y;
        CombatPlacement.s_combatLLocation = L;
        RandSeed = Y * 128 + X + L * 16384 + AoWHSMap.Current.InitialRandomSeed;
        I = 0;
        for (; I <= 6; I += 1)
        {
            HexGeometry.CenterHexNumberToHex(X, Y, I, ref J, ref K);
            var mapField = AoWHSMap.Current.MapContainer.MapLevels[L].Field[J, K];
            Terrain[I] = mapField.TerrainType;
            var combatTerrainTag = ((AoWMapField)AoWHSMap.Current.MapContainer.MapLevels[L].Field[J, K]).GetCombatTerrainTag();
            Tags[I] = (sbyte)combatTerrainTag;
        }

        if (Terrain[0] == 0 || Terrain[0] == 9 || Terrain[0] == 10)
        {
            I = 1;
            for (; I <= 6; I += 1)
            {
                Terrain[I] = Terrain[0];
            }
        }
        else
        {
            I = 1;
            for (; I <= 6; I += 1)
            {
                if (Terrain[I] == 0 || Terrain[I] == 15 || Terrain[I] == 10 || Terrain[I] == 9)
                {
                    Terrain[I] = Terrain[0];
                }
            }
        }

        fileName = "TCMaps\\";
        structure = (Structure)AoWHSMap.Current.MapContainer.MapLevels[L].Field[X, Y].FindNoneTransparentHexSprite(typeof(Structure));
        bridge = null;
        if (structure == null)
        {
            bridge = (HexBridge)AoWHSMap.Current.MapContainer.MapLevels[L].Field[X, Y].FindNoneTransparentHexSprite(typeof(HexBridge));
        }

        if (structure != null)
        {
            // The kind names the map (Structure.Views.cs) and reports what the map takes from the combat.
            var site = new CombatMapSite(siteCombat, randomTerrain, (direction, kind) =>
            {
                HexGeometry.CenterHexNumberToHex(X, Y, direction, ref J, ref K);
                return AoWHSMap.Current.MapContainer.MapLevels[L].Field[J, K].FindNoneTransparentHexSprite(kind) != null;
            });
            fileName = fileName + structure.CombatMapName(site);
            randomTerrain = site.RandomTerrain;
            // Prisoners are an army on the same field. Only an item list is laid out.
            if (site.TakesCombatItems && combat.Items is ItemList items)
            {
                Items = items;
            }

            for (int direction = 1; direction <= 6; direction++)
            {
                if ((site.FixedTags & (1 << direction)) != 0)
                {
                    Tags[direction] = 127;
                }
            }

            Tags[0] = 127;
        }
        else if (bridge != null)
        {
            randomTerrain = false;
            RandomTerrain = false;
            fileName = fileName + "Br";
            switch (Terrain[0])
            {
                case 10:
                    fileName = fileName + "UW";
                    break;
                case 6:
                    fileName = fileName + "Ic";
                    break;
                case 9:
                    fileName = fileName + "La";
                    break;
                case 13:
                    fileName = fileName + "UI";
                    break;
                default:
                    fileName = fileName + "Wa";
                    break;
            }

            switch (bridge.Direction)
            {
                case 1:
                    fileName = fileName + "A000";
                    break;
                case 2:
                    fileName = fileName + "B000";
                    break;
                default:
                    fileName = fileName + "C000";
                    break;
            }

            Tags[0] = 127;
        }

        if (Tags[0] != 127)
        {
            fileName = fileName + "Blank000";
        }

        fileName = fileName + ".HSM";
        if (combat == null)
        {
            GameTables.GameEngine.LoadHsmMap(FileBoundary.ReadName(GameTables.GameEngine.StartupDirectory + fileName), false, null, GameProfile.TacticalMap);
        }
        else
        {
            AoWHSMap.Current.InitializeCombatMap(combat, fileName);
        }

        CombatMap.Current.SetCombat(combat);
        CombatMap.Current.SetExplorationEnabled(siteCombat);
        CombatMap.Current.SetFogEnabled(false);
        if (combat != null)
        {
            PlaceCombatUnits();
        }

        if (randomTerrain)
        {
            CombatMap.Current.LockMapChangedEvent();
            shapeBuffer = new byte[3770];
            terrainBuffer = new byte[3770];
            for (int row = 6; row <= 52; row++)
            {
                for (int column = 6; column <= 58; column++)
                {
                    shape = s_mapShape[row * 65 + column];
                    switch (shape)
                    {
                        case 14:
                            if (Random(100) < 50)
                            {
                                shape = 1;
                            }
                            else
                            {
                                shape = 2;
                            }

                            break;
                        case 15:
                            if (Random(100) < 50)
                            {
                                shape = 1;
                            }
                            else
                            {
                                shape = 3;
                            }

                            break;
                        case 16:
                            if (Random(100) < 50)
                            {
                                shape = 1;
                            }
                            else
                            {
                                shape = 4;
                            }

                            break;
                        case 17:
                            if (Random(100) < 50)
                            {
                                shape = 1;
                            }
                            else
                            {
                                shape = 5;
                            }

                            break;
                        case 18:
                            if (Random(100) < 50)
                            {
                                shape = 1;
                            }
                            else
                            {
                                shape = 6;
                            }

                            break;
                        case 19:
                            if (Random(100) < 50)
                            {
                                shape = 1;
                            }
                            else
                            {
                                shape = 7;
                            }

                            break;
                        case 11:
                            if (Random(100) < 50)
                            {
                                shape = 2;
                            }
                            else
                            {
                                shape = 3;
                            }

                            break;
                        case 9:
                            if (Random(100) < 50)
                            {
                                shape = 3;
                            }
                            else
                            {
                                shape = 4;
                            }

                            break;
                        case 13:
                            if (Random(100) < 50)
                            {
                                shape = 5;
                            }
                            else
                            {
                                shape = 4;
                            }

                            break;
                        case 12:
                            if (Random(100) < 50)
                            {
                                shape = 5;
                            }
                            else
                            {
                                shape = 6;
                            }

                            break;
                        case 8:
                            if (Random(100) < 50)
                            {
                                shape = 7;
                            }
                            else
                            {
                                shape = 6;
                            }

                            break;
                        case 10:
                            if (Random(100) < 50)
                            {
                                shape = 2;
                            }
                            else
                            {
                                shape = 7;
                            }

                            break;
                    }

                    if (CombatMap.Current.MapContainer.MapLevels[0].Field[column, row].GetDecorationType() == 9)
                    {
                        switch (shape)
                        {
                            case 1:
                                J = 0;
                                break;
                            case 2:
                                J = 1;
                                break;
                            case 3:
                                J = 2;
                                break;
                            case 4:
                                J = 3;
                                break;
                            case 5:
                                J = 4;
                                break;
                            case 6:
                                J = 5;
                                break;
                            case 7:
                                J = 6;
                                break;
                            default:
                                J = -1;
                                break;
                        }

                        if (J > -1)
                        {
                            HexGeometry.CenterHexNumberToHex(X, Y, J, ref K, ref hexX);
                            if (AoWHSMap.Current.MapContainer.MapLevels[L].Field[K, hexX].FindOwnedChild(131946) == null)
                            {
                                CombatMap.Current.ClearTerrain(column, row, 0, 0, -1);
                            }
                        }
                    }

                    shapeBuffer[row * 65 + column] = shape;
                }
            }

            if (AoWHSMap.Current.MapContainer.MapLevels[L].Field[X, Y].OverlayType == 4)
            {
                I = 0;
                for (; I <= 6; I += 1)
                {
                    HexGeometry.CenterHexNumberToHex(X, Y, I, ref J, ref K);
                    var item = AoWHSMap.Current.MapContainer.MapLevels[L].Field[J, K];
                    weights[I] = item.OverlayType;
                }

                for (int row = 6; row <= 52; row++)
                {
                    for (int column = 6; column <= 58; column++)
                    {
                        shape = s_mapRoads[row * 65 + column];
                        switch (shape)
                        {
                            case 21:
                                if (Random(100) < 50)
                                {
                                    shape = 0;
                                }
                                else
                                {
                                    shape = 2;
                                }

                                break;
                            case 22:
                                if (Random(100) < 50)
                                {
                                    shape = 0;
                                }
                                else
                                {
                                    shape = 3;
                                }

                                break;
                            case 23:
                                if (Random(100) < 50)
                                {
                                    shape = 0;
                                }
                                else
                                {
                                    shape = 4;
                                }

                                break;
                            case 24:
                                if (Random(100) < 50)
                                {
                                    shape = 0;
                                }
                                else
                                {
                                    shape = 5;
                                }

                                break;
                            case 25:
                                if (Random(100) < 50)
                                {
                                    shape = 0;
                                }
                                else
                                {
                                    shape = 6;
                                }

                                break;
                            case 26:
                                if (Random(100) < 50)
                                {
                                    shape = 0;
                                }
                                else
                                {
                                    shape = 7;
                                }

                                break;
                        }

                        if (Set<byte>.WithBounds(0, 255).WithRange(1, 7).Contains(shape))
                        {
                            if (Tags[shape - 1] != 127 && weights[shape - 1] == 4)
                            {
                                AoWHSSet.Current.CombatRoads.Place(CombatMap.Current.MapContainer, (sbyte)column, (sbyte)row, 0, 1);
                                terrainBuffer[row * 65 + column] = 251;
                            }
                        }
                    }
                }
            }

            for (int row = 6; row <= 52; row++)
            {
                for (int column = 3; column <= 29; column++)
                {
                    I = shapeBuffer[row * 65 + column * 2];
                    if (I > 0)
                    {
                        if (Tags[I - 1] == 127 || terrainBuffer[row * 65 + column * 2] == 251)
                        {
                            I = 0;
                        }
                    }

                    if (I > 0)
                    {
                        terrainType = Terrain[I - 1];
                        tag = Tags[I - 1];
                        I = 0;
                        for (; I <= 18; I += 1)
                        {
                            weights[I] = TerrainWeights[terrainType * 19 + I];
                            if (tag < 14)
                            {
                                weights[I] += TagWeights[tag * 19 + I];
                            }
                        }

                        I = 0;
                        for (; I <= 2; I += 1)
                        {
                            HexGeometry.CenterHexNumberToHex(column * 2, row, s_clusterCheckBox[I], ref J, ref K);
                            tag = terrainBuffer[K * 65 + J];
                            if (tag <= 19)
                            {
                                weights[tag] += NeighborWeights[tag];
                            }

                            switch (tag)
                            {
                                case >= 1 and <= 3:
                                    weights[5] += 10;
                                    weights[10] += 10;
                                    weights[4] += 10;
                                    break;
                                case 14:
                                case 16:
                                    weights[11] += NeighborWeights[11];
                                    break;
                            }
                        }

                        hasUnit = CombatMap.Current.MapContainer.MapLevels[0].Field[column * 2, row].FindChild(2228484) != null;
                        if (hasUnit || Blocked(column * 2, row, 0, 2))
                        {
                            if ((hasUnit || Blocked(column * 2, row, 4, 8)) && (hasUnit || Blocked(column * 2, row, 9, 17)))
                            {
                                weights[1] = 0;
                                weights[2] = 0;
                                weights[3] = 0;
                                weights[5] = 0;
                                weights[6] = 0;
                                weights[7] = 0;
                                weights[11] = 0;
                                weights[14] = 0;
                                weights[15] = 0;
                                weights[16] = 0;
                                weights[17] = 0;
                                weights[18] = 0;
                            }
                        }
                        else
                        {
                            if ((hasUnit || Blocked(column * 2, row, 4, 8)) && (hasUnit || Blocked(column * 2, row, 9, 17)))
                            {
                                weights[14] = 0;
                                weights[15] = 0;
                                weights[16] = 0;
                            }
                        }

                        J = 0;
                        I = 0;
                        for (; I <= 18; I += 1)
                        {
                            J += weights[I];
                        }

                        tag = Random(J) - weights[0];
                        J = 0;
                        while (tag > 0 && J < 18)
                        {
                            J += 1;
                            tag -= weights[J];
                        }

                        if (terrainType == 8)
                        {
                            J = 17;
                        }

                        switch (J)
                        {
                            case 14:
                                HexGeometry.CenterHexNumberToHex(column * 2, row, 6, ref K, ref hexX);
                                terrainBuffer[hexX * 65 + K] = 252;
                                break;
                            case 15:
                                HexGeometry.CenterHexNumberToHex(column * 2, row, 6, ref K, ref hexX);
                                terrainBuffer[hexX * 65 + K] = 253;
                                break;
                            case 16:
                                HexGeometry.CenterHexNumberToHex(column * 2, row, 1, ref K, ref hexX);
                                terrainBuffer[hexX * 65 + K] = 254;
                                HexGeometry.CenterHexNumberToHex(column * 2, row, 2, ref K, ref hexX);
                                terrainBuffer[hexX * 65 + K] = 254;
                                HexGeometry.CenterHexNumberToHex(column * 2, row, 6, ref K, ref hexX);
                                terrainBuffer[hexX * 65 + K] = 254;
                                break;
                        }

                        terrainBuffer[row * 65 + column * 2] = (byte)J;
                    }
                }

                for (int column = 3; column <= 29; column++)
                {
                    I = shapeBuffer[row * 65 + column * 2 + 1];
                    if (I > 0)
                    {
                        if (Tags[I - 1] == 127 || terrainBuffer[row * 65 + column * 2 + 1] == 251)
                        {
                            I = 0;
                        }
                    }

                    if (I > 0)
                    {
                        terrainType = Terrain[I - 1];
                        tag = Tags[I - 1];
                        I = 0;
                        for (; I <= 18; I += 1)
                        {
                            weights[I] = TerrainWeights[terrainType * 19 + I];
                            if (tag < 14)
                            {
                                weights[I] += TagWeights[tag * 19 + I];
                            }
                        }

                        I = 0;
                        for (; I <= 2; I += 1)
                        {
                            HexGeometry.CenterHexNumberToHex(column * 2 + 1, row, s_clusterCheckBox[I], ref J, ref K);
                            tag = terrainBuffer[K * 65 + J];
                            if (tag <= 19)
                            {
                                weights[tag] += NeighborWeights[tag];
                            }

                            switch (tag)
                            {
                                case >= 1 and <= 3:
                                    weights[5] += 10;
                                    weights[10] += 10;
                                    weights[4] += 10;
                                    break;
                                case 14:
                                case 16:
                                    weights[11] += NeighborWeights[11];
                                    break;
                            }
                        }

                        hasUnit = CombatMap.Current.MapContainer.MapLevels[0].Field[column * 2 + 1, row].FindChild(2228484) != null;
                        if (hasUnit || Blocked(column * 2 + 1, row, 0, 2))
                        {
                            if ((hasUnit || Blocked(column * 2 + 1, row, 4, 8)) && (hasUnit || Blocked(column * 2 + 1, row, 9, 17)))
                            {
                                weights[1] = 0;
                                weights[2] = 0;
                                weights[3] = 0;
                                weights[5] = 0;
                                weights[6] = 0;
                                weights[7] = 0;
                                weights[11] = 0;
                                weights[14] = 0;
                                weights[15] = 0;
                                weights[16] = 0;
                                weights[17] = 0;
                                weights[18] = 0;
                            }
                        }
                        else
                        {
                            if ((hasUnit || Blocked(column * 2 + 1, row, 4, 8)) && (hasUnit || Blocked(column * 2 + 1, row, 9, 17)))
                            {
                                weights[14] = 0;
                                weights[15] = 0;
                                weights[16] = 0;
                            }
                        }

                        J = 0;
                        I = 0;
                        for (; I <= 18; I += 1)
                        {
                            J += weights[I];
                        }

                        tag = Random(J) - weights[0];
                        J = 0;
                        while (tag > 0 && J < 18)
                        {
                            J += 1;
                            tag -= weights[J];
                        }

                        if (terrainType == 8)
                        {
                            J = 17;
                        }

                        switch (J)
                        {
                            case 14:
                                HexGeometry.CenterHexNumberToHex(column * 2 + 1, row, 6, ref K, ref hexX);
                                terrainBuffer[hexX * 65 + K] = 252;
                                break;
                            case 15:
                                HexGeometry.CenterHexNumberToHex(column * 2 + 1, row, 6, ref K, ref hexX);
                                terrainBuffer[hexX * 65 + K] = 253;
                                break;
                            case 16:
                                HexGeometry.CenterHexNumberToHex(column * 2 + 1, row, 1, ref K, ref hexX);
                                terrainBuffer[hexX * 65 + K] = 254;
                                HexGeometry.CenterHexNumberToHex(column * 2 + 1, row, 2, ref K, ref hexX);
                                terrainBuffer[hexX * 65 + K] = 254;
                                HexGeometry.CenterHexNumberToHex(column * 2 + 1, row, 6, ref K, ref hexX);
                                terrainBuffer[hexX * 65 + K] = 254;
                                break;
                        }

                        terrainBuffer[row * 65 + column * 2 + 1] = (byte)J;
                    }
                }
            }

            for (int row = 6; row <= 52; row++)
            {
                for (int column = 6; column <= 58; column++)
                {
                    I = shapeBuffer[row * 65 + column];
                    if (I > 0)
                    {
                        J = terrainBuffer[row * 65 + column];
                        if (J == 17)
                        {
                            CombatMap.Current.MapContainer.MapLevels[0].Field[column, row].TerrainType = 7;
                        }
                        else
                        {
                            CombatMap.Current.MapContainer.MapLevels[0].Field[column, row].TerrainType = Terrain[I - 1];
                            if (J > 0 && J < 19)
                            {
                                if (Resources[Terrain[I - 1] * 19 + J].Count > 0)
                                {
                                    var random = Random(Resources[Terrain[I - 1] * 19 + J].Count);
                                    (Resources[Terrain[I - 1] * 19 + J][random]).PlaceResourceUser(CombatMap.Current, (sbyte)column, (sbyte)row, 0);
                                }
                            }
                        }
                    }
                }
            }

            for (int row = 6; row <= 52; row++)
            {
                for (int column = 6; column <= 58; column++)
                {
                    CombatMap.Current.MapContainer.MapLevels[0].Field[column, row].TerrainChanged();
                }
            }

            terrainBuffer = default;
            CombatMap.Current.UnlockMapChangedEvent();
        }

        return CombatMap.Current;
    }

    // AoWTC.pas:4519
    public void PlaceCombatUnits()
    {
        int position;
        int I;
        int J = default;
        int temporary = default;
        DelphiList units;
        TacticalBattleUnit U;
        TacticalBattleUnit other;
        int direction;
        BooleanArray8 used = default;
        TacticalBattleItemHexSprite itemHexSprite;
        direction = 32;
        HomePosition = -1;
        units = new DelphiList();
        position = 0;
        for (; position <= 7; position += 1)
        {
            units.Clear();
            I = 0;
            int last = CombatMap.Current.Combat.CombatData.GetCount() - 1;
            for (; I <= last; I += 1)
            {
                if (CombatMap.Current.Combat.CombatData.GetObjects(I) is CombatUnit)
                {
                    if (CombatMap.Current.Combat.CombatData.GetObjects(I).GetEnabled())
                    {
                        if ((int)(((uint)CombatMap.Current.Combat.CombatData.GetObjects(I).Position) >> 4) == position)
                        {
                            units.Add(CombatMap.Current.Combat.CombatData.GetObjects(I));
                            U = (TacticalBattleUnit)units[units.Count - 1];
                            U.PositionIndex = (byte)(U.Position & 15);
                            U.PositionGroup = (byte)(int)(((uint)U.Position) >> 4);
                            if (U.PositionGroup == 7)
                            {
                                U.PositionGroup = 1;
                            }

                            if (U.PositionGroup == 0)
                            {
                                HomePosition = 0;
                            }

                            if (HomePosition == -1)
                            {
                                if (U.GetSide() == CombatSideIndex.Defender)
                                {
                                    if (AoWHSMap.Current.MapContainer.MapLevels[CombatMap.Current.Combat.CombatData.MapLevel].Field[CombatMap.Current.Combat.CombatData.X, CombatMap.Current.Combat.CombatData.Y].FindNoneTransparentHexSprite(typeof(City)) != null)
                                    {
                                    }
                                }
                            }

                            if (HomePosition > 0 && U.PositionGroup == HomePosition)
                            {
                                U.PositionGroup = 0;
                            }

                            var side = U.GetSide();
                            CombatMap.Current.PositionStatus[U.PositionGroup] = (byte)side;
                        }
                    }
                }
            }

            I = 0;
            int limit = units.Count - 1;
            for (; I <= limit; I += 1)
            {
                U = (TacticalBattleUnit)units[I];
                J = I + 1;
                int last2 = units.Count - 1;
                for (; J <= last2; J += 1)
                {
                    other = (TacticalBattleUnit)units[J];
                    if (U.GetPositionValue() < other.GetPositionValue())
                    {
                        temporary = U.PositionIndex;
                        U.PositionIndex = other.PositionIndex;
                        other.PositionIndex = (byte)temporary;
                    }
                }
            }
        }

        if (CombatMap.Current.PositionStatus[0] != 1)
        {
            if (AoWHSMap.Current.MapContainer.MapLevels[CombatMap.Current.Combat.CombatData.MapLevel].Field[CombatMap.Current.Combat.CombatData.X, CombatMap.Current.Combat.CombatData.Y].FindNoneTransparentHexSprite(typeof(City)) != null && CombatMap.Current.Combat.CombatData.WallType != 0)
            {
                position = 1;
                while (position < 7)
                {
                    if (CombatMap.Current.PositionStatus[position] == 1)
                    {
                        HexGeometry.CenterHexNumberToHex(CombatMap.Current.Combat.CombatData.X, CombatMap.Current.Combat.CombatData.Y, position, ref J, ref temporary);
                        if (AoWHSMap.Current.MapContainer.MapLevels[CombatMap.Current.Combat.CombatData.MapLevel].Field[J, temporary].FindNoneTransparentHexSprite(typeof(City)) != null)
                        {
                            HomePosition = position;
                            CombatMap.Current.PositionStatus[position] = (int)CombatSideIndex.Neutral;
                            CombatMap.Current.PositionStatus[0] = 1;
                            position = 7;
                        }
                    }

                    position += 1;
                }
            }
        }

        position = 0;
        int last3 = CombatMap.Current.Combat.Sides[(int)CombatSideIndex.Attacker].GetCount() - 1;
        for (; position <= last3; position += 1)
        {
            if (CombatMap.Current.Combat.Sides[(int)CombatSideIndex.Attacker].GetObjects(position).GetEnabled())
            {
                SetupUnit(CombatSideIndex.Attacker, position, ref direction);
            }
        }

        position = 0;
        int last4 = CombatMap.Current.Combat.Sides[(int)CombatSideIndex.Defender].GetCount() - 1;
        for (; position <= last4; position += 1)
        {
            if (CombatMap.Current.Combat.Sides[(int)CombatSideIndex.Defender].GetObjects(position) is CombatUnit)
            {
                if (CombatMap.Current.Combat.Sides[(int)CombatSideIndex.Defender].GetObjects(position).GetEnabled())
                {
                    SetupUnit(CombatSideIndex.Defender, position, ref direction);
                }
            }
        }

        units?.Dispose();
        if (Items != null)
        {
            position = 0;
            for (; position <= 7; position += 1)
            {
                used[position] = false;
            }

            position = 0;
            while (position < Items.GetCount())
            {
                I = Random(CombatMap.Current.UnitPositions.Positions.Count - 16);
                J = I - 1;
                if (J < 0)
                {
                    J = CombatMap.Current.UnitPositions.Positions.Count - 17;
                }

                // { Original indexes its eight-slot occupancy array with the position-list index. }
                while (used[I] && I != J)
                {
                    I += 1;
                    if (I > CombatMap.Current.UnitPositions.Positions.Count - 17)
                    {
                        I = 0;
                    }
                }

                used[I] = true;
                J = CombatMap.Current.UnitPositions.GetUnitPosition(2, I).X;
                temporary = CombatMap.Current.UnitPositions.GetUnitPosition(2, I).Y;
                itemHexSprite = new TacticalBattleItemHexSprite(null);
                if (CombatMap.Current.PlaceHex(itemHexSprite, J, temporary, 0))
                {
                    Items.GetItem(position).SetOwner(itemHexSprite.Items);
                }
                else
                {
                    position += 1;
                }
            }
        }
    }

    // AoWTC.pas:4610
    public void SetupUnit(CombatSideIndex side, int index, ref int direction)
    {
        TacticalBattleUnitHexSprite unitHexSprite;
        TacticalBattleUnitHexSprite targetHexSprite;
        int X;
        int Y;
        int position;
        int slot;
        int hexNumber;
        int hexX = default;
        int hexY = default;
        unitHexSprite = new TacticalBattleUnitHexSprite(null);
        unitHexSprite.Initialize((TacticalBattleUnit)CombatMap.Current.Combat.Sides[(int)side].GetObjects(index));
        if (unitHexSprite.Unit.PositionGroup == HomePosition)
        {
            unitHexSprite.Unit.PositionGroup = 0;
        }

        position = unitHexSprite.Unit.PositionGroup;
        slot = unitHexSprite.Unit.PositionIndex;
        X = CombatMap.Current.UnitPositions.GetUnitPosition(position, slot).X;
        Y = CombatMap.Current.UnitPositions.GetUnitPosition(position, slot).Y;
        if (side == CombatSideIndex.Defender && RandomTerrain)
        {
            switch (position)
            {
                case 0:
                    if (direction > 0)
                    {
                        X = s_defensePositionX[(direction - 1) * 8 + slot];
                        Y = s_defensePositionY[(direction - 1) * 8 + slot];
                        unitHexSprite.SetDirection((byte)direction);
                    }
                    else
                    {
                        switch (slot)
                        {
                            case 0:
                                unitHexSprite.SetDirection(1);
                                break;
                            case 1:
                                unitHexSprite.SetDirection(4);
                                break;
                            case 2:
                            case 6:
                                unitHexSprite.SetDirection(5);
                                break;
                            case 3:
                            case 7:
                                unitHexSprite.SetDirection(2);
                                break;
                            case 4:
                                unitHexSprite.SetDirection(3);
                                break;
                            case 5:
                                unitHexSprite.SetDirection(6);
                                break;
                        }
                    }

                    break;
                case 1:
                    Y += 2;
                    break;
                case 2:
                    X -= 2;
                    Y += 1;
                    break;
                case 3:
                    X -= 2;
                    Y -= 1;
                    break;
                case 4:
                    Y -= 2;
                    break;
                case 5:
                    X += 2;
                    Y -= 1;
                    break;
                case 6:
                    X += 2;
                    Y += 1;
                    break;
            }
        }
        else
        {
            if (direction != position)
            {
                if (direction == 32)
                {
                    direction = position;
                }
                else
                {
                    direction = 0;
                }
            }
        }

        if (!RandomTerrain)
        {
            unitHexSprite.SetDirection((byte)CombatMap.Current.UnitPositions.GetUnitPosition(position, slot).Tag);
        }
        else
        {
            switch (position)
            {
                case 1:
                    unitHexSprite.SetDirection(4);
                    break;
                case 2:
                    unitHexSprite.SetDirection(5);
                    break;
                case 3:
                    unitHexSprite.SetDirection(6);
                    break;
                case 4:
                    unitHexSprite.SetDirection(1);
                    break;
                case 5:
                    unitHexSprite.SetDirection(2);
                    break;
                case 6:
                    unitHexSprite.SetDirection(3);
                    break;
            }
        }

        hexNumber = 0;
        do
        {
            HexGeometry.CenterHexNumberToHex(X, Y, hexNumber, ref hexX, ref hexY);
            slot = CombatMap.Current.MapContainer.MapLevels[0].Field[hexX, hexY].OverlayType;
            targetHexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].Field[hexX, hexY].FindOwnedChild(2228484);
            hexNumber += 1;
        }
        while (!(slot != 8 && targetHexSprite == null));
        CombatMap.Current.PlaceHex(unitHexSprite, hexX, hexY, 0);
        if (HexGeometry.DeltaHexToHexNumber(CombatMap.Current.Combat.CombatData.X, CombatMap.Current.Combat.CombatData.Y, CombatMap.Current.Combat.CombatData.AttackerX, CombatMap.Current.Combat.CombatData.AttackerY) == position && CombatMap.Current.Players.GetPlayers(unitHexSprite.Unit.CombatPlayer).LastX == 0)
        {
            var xhx = unitHexSprite.GetHexX();
            CombatMap.Current.Players.GetPlayers(unitHexSprite.Unit.CombatPlayer).LastX = xhx;
            var yhx = unitHexSprite.GetHexY();
            CombatMap.Current.Players.GetPlayers(unitHexSprite.Unit.CombatPlayer).LastY = yhx;
        }
    }

    // AoWTC.pas:4667
    public void InitializeTables()
    {
        NeighborWeights[1] = 500;
        NeighborWeights[2] = 500;
        NeighborWeights[4] = 1500;
        NeighborWeights[5] = 200;
        NeighborWeights[10] = 1500;
        NeighborWeights[11] = 500;
        NeighborWeights[17] = 2000;
        NeighborWeights[18] = 1500;
        TerrainWeights[19] = 2500;
        TerrainWeights[20] = 0;
        TerrainWeights[21] = 35;
        TerrainWeights[22] = 0;
        TerrainWeights[23] = 6;
        TerrainWeights[24] = 2;
        TerrainWeights[25] = 1;
        TerrainWeights[26] = 2;
        TerrainWeights[27] = 5;
        TerrainWeights[29] = 90;
        TerrainWeights[30] = 60;
        TerrainWeights[31] = 2;
        TerrainWeights[32] = 30;
        TerrainWeights[33] = 10;
        TerrainWeights[34] = 0;
        TerrainWeights[35] = 3;
        TerrainWeights[38] = 2500;
        TerrainWeights[39] = 5;
        TerrainWeights[40] = 0;
        TerrainWeights[41] = 0;
        TerrainWeights[42] = 6;
        TerrainWeights[43] = 0;
        TerrainWeights[44] = 5;
        TerrainWeights[45] = 5;
        TerrainWeights[46] = 2;
        TerrainWeights[48] = 35;
        TerrainWeights[49] = 20;
        TerrainWeights[50] = 5;
        TerrainWeights[51] = 30;
        TerrainWeights[52] = 5;
        TerrainWeights[53] = 2;
        TerrainWeights[54] = 1;
        TerrainWeights[57] = 2500;
        TerrainWeights[58] = 8;
        TerrainWeights[59] = 0;
        TerrainWeights[60] = 0;
        TerrainWeights[61] = 6;
        TerrainWeights[62] = 0;
        TerrainWeights[63] = 2;
        TerrainWeights[64] = 2;
        TerrainWeights[65] = 1;
        TerrainWeights[67] = 15;
        TerrainWeights[68] = 20;
        TerrainWeights[69] = 2;
        TerrainWeights[70] = 30;
        TerrainWeights[71] = 5;
        TerrainWeights[72] = 1;
        TerrainWeights[73] = 1;
        TerrainWeights[76] = 2500;
        TerrainWeights[77] = 8;
        TerrainWeights[78] = 0;
        TerrainWeights[79] = 0;
        TerrainWeights[80] = 6;
        TerrainWeights[81] = 0;
        TerrainWeights[82] = 2;
        TerrainWeights[83] = 2;
        TerrainWeights[84] = 1;
        TerrainWeights[86] = 15;
        TerrainWeights[87] = 20;
        TerrainWeights[88] = 2;
        TerrainWeights[89] = 20;
        TerrainWeights[90] = 5;
        TerrainWeights[91] = 1;
        TerrainWeights[92] = 1;
        TerrainWeights[95] = 2500;
        TerrainWeights[96] = 5;
        TerrainWeights[97] = 0;
        TerrainWeights[98] = 1;
        TerrainWeights[99] = 3;
        TerrainWeights[100] = 0;
        TerrainWeights[101] = 5;
        TerrainWeights[102] = 5;
        TerrainWeights[103] = 4;
        TerrainWeights[105] = 10;
        TerrainWeights[106] = 45;
        TerrainWeights[107] = 5;
        TerrainWeights[108] = 12;
        TerrainWeights[109] = 10;
        TerrainWeights[110] = 4;
        TerrainWeights[111] = 3;
        TerrainWeights[114] = 2500;
        TerrainWeights[115] = 0;
        TerrainWeights[116] = 0;
        TerrainWeights[117] = 0;
        TerrainWeights[118] = 0;
        TerrainWeights[119] = 0;
        TerrainWeights[120] = 0;
        TerrainWeights[121] = 0;
        TerrainWeights[122] = 0;
        TerrainWeights[124] = 0;
        TerrainWeights[125] = 0;
        TerrainWeights[126] = 0;
        TerrainWeights[127] = 0;
        TerrainWeights[128] = 0;
        TerrainWeights[129] = 0;
        TerrainWeights[130] = 0;
        TerrainWeights[228] = 2500;
        TerrainWeights[229] = 0;
        TerrainWeights[230] = 0;
        TerrainWeights[231] = 0;
        TerrainWeights[232] = 0;
        TerrainWeights[233] = 10;
        TerrainWeights[234] = 0;
        TerrainWeights[235] = 0;
        TerrainWeights[236] = 0;
        TerrainWeights[238] = 30;
        TerrainWeights[239] = 70;
        TerrainWeights[240] = 4;
        TerrainWeights[241] = 0;
        TerrainWeights[242] = 50;
        TerrainWeights[243] = 4;
        TerrainWeights[244] = 10;
        TerrainWeights[245] = 80;
        TagWeights[20] = 800;
        TagWeights[21] = 100;
        TagWeights[29] = 300;
        TagWeights[25] = 20;
        TagWeights[26] = 40;
        TagWeights[27] = 10;
        TagWeights[40] = 900;
        TagWeights[48] = 400;
        TagWeights[44] = 20;
        TagWeights[45] = 40;
        TagWeights[46] = 10;
        TagWeights[60] = 250;
        TagWeights[61] = 200;
        TagWeights[86] = 300;
        TagWeights[80] = 500;
        TagWeights[100] = 500;
        TagWeights[105] = 200;
        TagWeights[99] = 100;
        TagWeights[120] = 250;
        TagWeights[121] = 250;
        TagWeights[122] = 100;
        TagWeights[144] = 600;
        TagWeights[147] = 250;
        TagWeights[149] = 150;
        TagWeights[164] = 250;
        TagWeights[167] = 100;
        TagWeights[163] = 200;
        TagWeights[162] = 200;
        TagWeights[181] = 500;
        TagWeights[191] = 450;
        TagWeights[200] = 150;
        TagWeights[196] = 40;
        TagWeights[197] = 20;
        TagWeights[198] = 10;
        TagWeights[211] = 450;
        TagWeights[219] = 150;
        TagWeights[215] = 20;
        TagWeights[216] = 20;
        TagWeights[217] = 40;
        TagWeights[241] = 500;
        TagWeights[239] = 200;
        TagWeights[242] = 50;
        TagWeights[244] = 20;
        TagWeights[258] = 600;
        TagWeights[261] = 350;
        TagWeights[263] = 300;
        TagWeights[181] = 600;
    }

    private static Int32Array48 s_defensePositionX = Int32Array48.From([32, 30, 34, 32, 28, 36, 30, 34, 34, 36, 32, 38, 30, 36, 32, 34, 36, 34, 38, 32, 36, 30, 34, 32, 32, 34, 30, 32, 36, 28, 34, 30, 30, 28, 32, 26, 34, 28, 32, 30, 28, 30, 26, 32, 28, 34, 30, 32]);
    private static Int32Array48 s_defensePositionY = Int32Array48.From([26, 27, 27, 28, 28, 28, 29, 29, 27, 28, 26, 29, 27, 30, 28, 29, 30, 31, 29, 32, 28, 31, 29, 30, 32, 31, 31, 30, 30, 30, 29, 29, 31, 30, 32, 29, 31, 28, 30, 29, 28, 27, 29, 26, 30, 27, 29, 28]);

    private static ByteArray3770 s_mapShape = ByteArray3770.From((ReadOnlySpan<byte>)new byte[3770]
    {
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 2, 2, 2, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 2, 2, 2, 2, 2, 2,
        2, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 2, 2, 2,
        2, 2, 2, 2, 2, 2, 2, 2, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
        2, 2, 2, 2, 2, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 2, 2, 2, 2, 2, 2, 2,
        2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
        2, 2, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 10, 2, 2, 2,
        2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
        2, 2, 2, 2, 2, 2, 2, 2, 11, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 7,
        7, 7, 10, 2, 2, 2, 2, 2, 2, 2,
        2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
        2, 2, 11, 3, 3, 3, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 7, 7, 7, 7, 7, 10, 10, 2,
        2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
        2, 2, 2, 2, 2, 2, 11, 11, 3, 3,
        3, 3, 3, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 7, 7, 7, 7, 7,
        7, 7, 7, 10, 2, 2, 2, 2, 2, 2,
        2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
        2, 11, 3, 3, 3, 3, 3, 3, 3, 3,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 7, 7,
        7, 7, 7, 7, 7, 7, 7, 7, 7, 10,
        2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
        2, 2, 2, 2, 2, 11, 3, 3, 3, 3,
        3, 3, 3, 3, 3, 3, 3, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 7, 7, 7, 7, 7, 7, 7, 7, 7,
        7, 7, 7, 7, 10, 10, 2, 2, 2, 2,
        2, 2, 2, 2, 2, 2, 2, 2, 2, 11,
        11, 3, 3, 3, 3, 3, 3, 3, 3, 3,
        3, 3, 3, 3, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 7, 7, 7, 7, 7, 7,
        7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
        10, 2, 2, 2, 2, 2, 2, 2, 2, 2,
        2, 2, 2, 2, 11, 3, 3, 3, 3, 3,
        3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
        3, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 7, 7,
        7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
        7, 7, 7, 7, 7, 7, 10, 2, 2, 2,
        2, 2, 2, 2, 2, 2, 2, 2, 11, 3,
        3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
        3, 3, 3, 3, 3, 3, 3, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 7, 7, 7, 7, 7, 7, 7,
        7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
        7, 10, 10, 2, 14, 2, 14, 2, 14, 2,
        14, 2, 11, 11, 3, 3, 3, 3, 3, 3,
        3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
        3, 3, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 7, 7,
        7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
        7, 7, 7, 7, 7, 7, 7, 19, 14, 1,
        14, 1, 14, 1, 14, 1, 14, 15, 3, 3,
        3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
        3, 3, 3, 3, 3, 3, 3, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 7, 7, 7, 7, 7, 7, 7,
        7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
        19, 19, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 15, 15, 3, 3, 3, 3, 3,
        3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
        3, 3, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 7, 7,
        7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
        7, 7, 7, 19, 19, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        15, 15, 3, 3, 3, 3, 3, 3, 3, 3,
        3, 3, 3, 3, 3, 3, 3, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 7, 7, 7, 7, 7, 7, 7,
        7, 7, 7, 7, 7, 7, 19, 19, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 15, 15, 3,
        3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
        3, 3, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 7, 7,
        7, 7, 7, 7, 7, 7, 7, 7, 7, 19,
        19, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 15, 15, 3, 3, 3, 3,
        3, 3, 3, 3, 3, 3, 3, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 7, 7, 7, 7, 7, 7, 7,
        7, 7, 19, 19, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 15, 15, 3, 3, 3, 3, 3, 3, 3,
        3, 3, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 7, 7,
        7, 7, 7, 7, 7, 19, 19, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 15, 15,
        3, 3, 3, 3, 3, 3, 3, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 7, 7, 7, 7, 7, 7, 19,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 15, 3, 3, 3, 3,
        3, 3, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 7, 7,
        7, 7, 7, 7, 19, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        15, 3, 3, 3, 3, 3, 3, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 8, 8, 8, 8, 8, 8, 19,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 15, 9, 9, 9, 9,
        9, 9, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 6, 6,
        6, 6, 6, 6, 18, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        16, 4, 4, 4, 4, 4, 4, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 6, 6, 6, 6, 6, 6, 18,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 16, 4, 4, 4, 4,
        4, 4, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 6, 6,
        6, 6, 6, 6, 18, 18, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 16,
        16, 4, 4, 4, 4, 4, 4, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 6, 6, 6, 6, 6, 6, 6,
        6, 18, 18, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 16, 16, 4, 4, 4, 4, 4, 4,
        4, 4, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 6, 6,
        6, 6, 6, 6, 6, 6, 6, 6, 18, 18,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 16, 16, 4, 4, 4,
        4, 4, 4, 4, 4, 4, 4, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 6, 6, 6, 6, 6, 6, 6,
        6, 6, 6, 6, 6, 18, 18, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 16, 16,
        4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
        4, 4, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 6, 6,
        6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
        6, 6, 18, 18, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 16, 16, 4, 4, 4, 4, 4, 4, 4,
        4, 4, 4, 4, 4, 4, 4, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 6, 6, 6, 6, 6, 6, 6,
        6, 6, 6, 6, 6, 6, 6, 6, 6, 18,
        18, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 16, 16, 4, 4, 4, 4,
        4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
        4, 4, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 6, 6,
        6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
        6, 6, 6, 6, 6, 6, 18, 17, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 17, 16, 4,
        4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
        4, 4, 4, 4, 4, 4, 4, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 6, 6, 6, 6, 6, 6, 6,
        6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
        6, 12, 5, 17, 17, 17, 17, 17, 17, 17,
        17, 17, 5, 13, 4, 4, 4, 4, 4, 4,
        4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
        4, 4, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 6, 6,
        6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
        6, 6, 6, 6, 6, 12, 12, 5, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 5, 13, 13,
        4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
        4, 4, 4, 4, 4, 4, 4, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 6, 6, 6, 6, 6,
        6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
        12, 5, 5, 5, 5, 5, 5, 5, 5, 5,
        5, 5, 5, 5, 13, 4, 4, 4, 4, 4,
        4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 6, 6, 6, 6, 6, 6, 6, 6,
        6, 6, 6, 6, 12, 5, 5, 5, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
        13, 4, 4, 4, 4, 4, 4, 4, 4, 4,
        4, 4, 4, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 6,
        6, 6, 6, 6, 6, 6, 6, 6, 12, 12,
        5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
        5, 5, 5, 5, 5, 13, 13, 4, 4, 4,
        4, 4, 4, 4, 4, 4, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 6, 6, 6, 6,
        6, 6, 6, 12, 5, 5, 5, 5, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
        5, 13, 4, 4, 4, 4, 4, 4, 4, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 6, 6, 6, 6, 12, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 13, 4, 4,
        4, 4, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        6, 12, 12, 5, 5, 5, 5, 5, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
        5, 5, 13, 13, 4, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 5, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 5, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 5, 5, 5, 5, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
        5, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 5, 5, 5, 5, 5, 5, 5, 5, 5,
        5, 5, 5, 5, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 5, 5, 5, 5, 5,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 5, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    });

    private static ByteArray3770 s_mapRoads = ByteArray3770.From((ReadOnlySpan<byte>)new byte[3770]
    {
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 2, 2, 2, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 21, 2, 2, 2, 21,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        21, 2, 2, 2, 21, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 21, 2, 2, 2, 21,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        21, 2, 2, 2, 21, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 21, 2, 2, 2, 21,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        21, 2, 2, 2, 21, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 21, 2, 2, 2, 21,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        21, 2, 2, 2, 21, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 21, 2, 2, 2, 21,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 26, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        21, 2, 2, 2, 21, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 22, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 7, 7, 7, 26, 26, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 21, 2, 2, 2, 21,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 22, 22, 3, 3,
        3, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 7, 7,
        7, 7, 7, 7, 26, 26, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        21, 2, 2, 2, 21, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 22,
        22, 3, 3, 3, 3, 3, 3, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 26, 26, 7, 7, 7, 7, 7,
        7, 26, 26, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 21, 2, 2, 2, 21,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 22, 22, 3, 3, 3, 3, 3, 3,
        22, 22, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        26, 26, 7, 7, 7, 7, 7, 7, 26, 26,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        21, 2, 2, 2, 21, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 22, 22, 3, 3, 3,
        3, 3, 3, 22, 22, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 26, 26, 7,
        7, 7, 7, 7, 7, 26, 26, 0, 0, 0,
        0, 0, 0, 0, 0, 21, 2, 2, 2, 21,
        0, 0, 0, 0, 0, 0, 0, 0, 22, 22,
        3, 3, 3, 3, 3, 3, 22, 22, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 26, 26, 7, 7, 7, 7,
        7, 7, 26, 26, 0, 0, 0, 0, 0, 0,
        21, 2, 2, 2, 21, 0, 0, 0, 0, 0,
        0, 22, 22, 3, 3, 3, 3, 3, 3, 22,
        22, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 26, 26, 7, 7, 7, 7, 7, 7, 26,
        26, 0, 0, 0, 0, 21, 2, 2, 2, 21,
        0, 0, 0, 0, 22, 22, 3, 3, 3, 3,
        3, 3, 22, 22, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 26, 26,
        7, 7, 7, 7, 7, 7, 26, 26, 0, 0,
        21, 2, 2, 2, 21, 0, 0, 22, 22, 3,
        3, 3, 3, 3, 3, 22, 22, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 26, 26, 7, 7, 7,
        7, 7, 7, 26, 26, 21, 2, 2, 2, 21,
        22, 22, 3, 3, 3, 3, 3, 3, 22, 22,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 26, 26, 7, 7, 7, 7, 7, 7,
        21, 2, 2, 2, 21, 3, 3, 3, 3, 3,
        3, 22, 22, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 26,
        26, 7, 7, 7, 7, 7, 1, 1, 1, 3,
        3, 3, 3, 3, 22, 22, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 26, 26, 7, 7,
        1, 1, 1, 1, 1, 3, 3, 22, 22, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 25, 25, 6, 1, 1, 1, 1, 1,
        4, 23, 23, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 25, 25, 6, 6, 6,
        1, 1, 1, 1, 1, 4, 4, 4, 23, 23,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 25, 25,
        6, 6, 6, 6, 6, 6, 5, 1, 5, 4,
        4, 4, 4, 4, 4, 23, 23, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 25, 25, 6, 6, 6, 6, 6, 6, 25,
        24, 5, 5, 5, 24, 23, 4, 4, 4, 4,
        4, 4, 23, 23, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 25, 25, 6, 6, 6, 6,
        6, 6, 25, 25, 0, 24, 5, 5, 5, 24,
        0, 23, 23, 4, 4, 4, 4, 4, 4, 23,
        23, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 25, 25, 6,
        6, 6, 6, 6, 6, 25, 25, 0, 0, 0,
        24, 5, 5, 5, 24, 0, 0, 0, 23, 23,
        4, 4, 4, 4, 4, 4, 23, 23, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        25, 25, 6, 6, 6, 6, 6, 6, 25, 25,
        0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
        0, 0, 0, 0, 0, 23, 23, 4, 4, 4,
        4, 4, 4, 23, 23, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 25, 25, 6, 6, 6, 6, 6,
        6, 25, 25, 0, 0, 0, 0, 0, 0, 0,
        24, 5, 5, 5, 24, 0, 0, 0, 0, 0,
        0, 0, 23, 23, 4, 4, 4, 4, 4, 4,
        23, 23, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 25, 25, 6, 6,
        6, 6, 6, 6, 25, 25, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 23,
        23, 4, 4, 4, 4, 4, 4, 23, 23, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 25,
        25, 6, 6, 6, 6, 6, 6, 25, 25, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        24, 5, 5, 5, 24, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 23, 23, 4, 4,
        4, 4, 4, 4, 23, 23, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 25, 6, 6, 6, 6, 6, 6,
        25, 25, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 23, 23, 4, 4, 4, 4, 4,
        4, 23, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 6, 6,
        6, 6, 6, 25, 25, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        24, 5, 5, 5, 24, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        23, 23, 4, 4, 4, 4, 4, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 6, 25, 25, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 23, 23, 4,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        24, 5, 5, 5, 24, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        24, 5, 5, 5, 24, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        24, 5, 5, 5, 24, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        24, 5, 5, 5, 24, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        24, 5, 5, 5, 24, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 5, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    });

    private static Int32Array18 s_clusterCheckBox = Int32Array18.From([1, 2, 6, 16, 7, 8, 9, 17, 18, 19, 20, 21, 22, 23, 33, 34, 35, 36]);

}
