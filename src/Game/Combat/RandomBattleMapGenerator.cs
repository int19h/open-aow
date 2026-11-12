// Translated from AoWTC.pas.

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
    public ItemList Items { get; private set; }

    // calls GetItem
    private bool _randomTerrain;
    private readonly int[] _neighborWeights = new int[20];
    private readonly int[] _terrainWeights = new int[305];
    private readonly int[] _tagWeights = new int[308];
    private readonly sbyte[] _terrain = new sbyte[7];
    private readonly sbyte[] _tags = new sbyte[7];
    private readonly List<MapObjectResource>[] _resources = new List<MapObjectResource>[286];
    // original allocates one extra list
    private int _homePosition;

    // AoWTC.pas:4149
    public virtual void Dispose()
    {
        for (int I = 0; I <= 285; I++)
        {
            _resources[I]?.Clear();
        }
    }

    // AoWTC.pas:4137
    public RandomBattleMapGenerator()
    {
        for (int I = 0; I <= 285; I++)
        {
            var list = new List<MapObjectResource>();
            _resources[I] = list;
        }

        for (int J = 0, last = GamePack.Current.CombatResources.GetCount() - 1; J <= last; J++)
        {
            if (GamePack.Current.CombatResources.Resources[J] is CombatTerrain.Resource)
            {
                var mapObjectResource = GamePack.Current.CombatResources.Resources[J];
                var mapObjectResource2 = GamePack.Current.CombatResources.Resources[J];
                var mapObjectResource3 = GamePack.Current.CombatResources.Resources[J];
                _resources[(sbyte)((TerrainMapObjectResource)mapObjectResource2).TerrainType * 19 + ((TerrainMapObjectResource)mapObjectResource3).DecorationType].Add(mapObjectResource);
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
        Span<int> weights = stackalloc int[20];
        bool Blocked(int X, int Y, int first, int last)
        {
            int kind;
            int hexX = default;
            int hexY = default;
            bool result = false;
            for (int I = first; I <= last; I++)
            {
                HexGeometry.CenterHexNumberToHex(X, Y, ClusterCheckBox[I], ref hexX, ref hexY);
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

        _randomTerrain = randomTerrain;
        CombatPlacement.CombatXLocation = X;
        CombatPlacement.CombatYLocation = Y;
        CombatPlacement.CombatLLocation = L;
        RandSeed = Y * 128 + X + L * 16384 + StrategicMap.Current.InitialRandomSeed;
        I = 0;
        for (; I <= 6; I += 1)
        {
            HexGeometry.CenterHexNumberToHex(X, Y, I, ref J, ref K);
            var mapField = StrategicMap.Current.MapContainer.MapLevels[L].Field[J, K];
            _terrain[I] = mapField.TerrainType;
            var combatTerrainTag = StrategicMap.Current.MapContainer.MapLevels[L].Field[J, K].GetCombatTerrainTag();
            _tags[I] = (sbyte)combatTerrainTag;
        }

        if (_terrain[0] == 0 || _terrain[0] == 9 || _terrain[0] == 10)
        {
            I = 1;
            for (; I <= 6; I += 1)
            {
                _terrain[I] = _terrain[0];
            }
        }
        else
        {
            I = 1;
            for (; I <= 6; I += 1)
            {
                if (_terrain[I] == 0 || _terrain[I] == 15 || _terrain[I] == 10 || _terrain[I] == 9)
                {
                    _terrain[I] = _terrain[0];
                }
            }
        }

        fileName = "TCMaps\\";
        structure = (Structure)StrategicMap.Current.MapContainer.MapLevels[L].Field[X, Y].FindNoneTransparentHexSprite(typeof(Structure));
        bridge = null;
        if (structure == null)
        {
            bridge = (HexBridge)StrategicMap.Current.MapContainer.MapLevels[L].Field[X, Y].FindNoneTransparentHexSprite(typeof(HexBridge));
        }

        if (structure != null)
        {
            // The kind names the map (Structure.Views.cs) and reports what the map takes from the combat.
            var site = new CombatMapSite(siteCombat, randomTerrain, (direction, kind) =>
            {
                HexGeometry.CenterHexNumberToHex(X, Y, direction, ref J, ref K);
                return StrategicMap.Current.MapContainer.MapLevels[L].Field[J, K].FindNoneTransparentHexSprite(kind) != null;
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
                    _tags[direction] = 127;
                }
            }

            _tags[0] = 127;
        }
        else if (bridge != null)
        {
            randomTerrain = false;
            _randomTerrain = false;
            fileName = fileName + "Br";
            switch (_terrain[0])
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

            _tags[0] = 127;
        }

        if (_tags[0] != 127)
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
            StrategicMap.Current.InitializeCombatMap(combat, fileName);
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
                    shape = MapShape[row * 65 + column];
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
                            if (StrategicMap.Current.MapContainer.MapLevels[L].Field[K, hexX].FindOwnedChild(131946) == null)
                            {
                                CombatMap.Current.ClearTerrain(column, row, 0, 0, -1);
                            }
                        }
                    }

                    shapeBuffer[row * 65 + column] = shape;
                }
            }

            if (StrategicMap.Current.MapContainer.MapLevels[L].Field[X, Y].OverlayType == 4)
            {
                I = 0;
                for (; I <= 6; I += 1)
                {
                    HexGeometry.CenterHexNumberToHex(X, Y, I, ref J, ref K);
                    var item = StrategicMap.Current.MapContainer.MapLevels[L].Field[J, K];
                    weights[I] = item.OverlayType;
                }

                for (int row = 6; row <= 52; row++)
                {
                    for (int column = 6; column <= 58; column++)
                    {
                        shape = MapRoads[row * 65 + column];
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
                            if (_tags[shape - 1] != 127 && weights[shape - 1] == 4)
                            {
                                GamePack.Current.CombatRoads.Place(CombatMap.Current.MapContainer, (sbyte)column, (sbyte)row, 0, 1);
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
                        if (_tags[I - 1] == 127 || terrainBuffer[row * 65 + column * 2] == 251)
                        {
                            I = 0;
                        }
                    }

                    if (I > 0)
                    {
                        terrainType = _terrain[I - 1];
                        tag = _tags[I - 1];
                        I = 0;
                        for (; I <= 18; I += 1)
                        {
                            weights[I] = _terrainWeights[terrainType * 19 + I];
                            if (tag < 14)
                            {
                                weights[I] += _tagWeights[tag * 19 + I];
                            }
                        }

                        I = 0;
                        for (; I <= 2; I += 1)
                        {
                            HexGeometry.CenterHexNumberToHex(column * 2, row, ClusterCheckBox[I], ref J, ref K);
                            tag = terrainBuffer[K * 65 + J];
                            if (tag <= 19)
                            {
                                weights[tag] += _neighborWeights[tag];
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
                                    weights[11] += _neighborWeights[11];
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
                        if (_tags[I - 1] == 127 || terrainBuffer[row * 65 + column * 2 + 1] == 251)
                        {
                            I = 0;
                        }
                    }

                    if (I > 0)
                    {
                        terrainType = _terrain[I - 1];
                        tag = _tags[I - 1];
                        I = 0;
                        for (; I <= 18; I += 1)
                        {
                            weights[I] = _terrainWeights[terrainType * 19 + I];
                            if (tag < 14)
                            {
                                weights[I] += _tagWeights[tag * 19 + I];
                            }
                        }

                        I = 0;
                        for (; I <= 2; I += 1)
                        {
                            HexGeometry.CenterHexNumberToHex(column * 2 + 1, row, ClusterCheckBox[I], ref J, ref K);
                            tag = terrainBuffer[K * 65 + J];
                            if (tag <= 19)
                            {
                                weights[tag] += _neighborWeights[tag];
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
                                    weights[11] += _neighborWeights[11];
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
                            CombatMap.Current.MapContainer.MapLevels[0].Field[column, row].TerrainType = _terrain[I - 1];
                            if (J > 0 && J < 19)
                            {
                                if (_resources[_terrain[I - 1] * 19 + J].Count > 0)
                                {
                                    var random = Random(_resources[_terrain[I - 1] * 19 + J].Count);
                                    (_resources[_terrain[I - 1] * 19 + J][random]).PlaceResourceUser(CombatMap.Current, (sbyte)column, (sbyte)row, 0);
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
        DelphiList<CombatObject> units;
        TacticalBattleUnit U;
        TacticalBattleUnit other;
        int direction;
        Span<bool> used = stackalloc bool[8];
        TacticalBattleItemHexSprite itemHexSprite;
        direction = 32;
        _homePosition = -1;
        units = new DelphiList<CombatObject>();
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
                                _homePosition = 0;
                            }

                            if (_homePosition == -1)
                            {
                                if (U.GetSide() == CombatSideIndex.Defender)
                                {
                                    if (StrategicMap.Current.MapContainer.MapLevels[CombatMap.Current.Combat.CombatData.MapLevel].Field[CombatMap.Current.Combat.CombatData.X, CombatMap.Current.Combat.CombatData.Y].FindNoneTransparentHexSprite(typeof(City)) != null)
                                    {
                                    }
                                }
                            }

                            if (_homePosition > 0 && U.PositionGroup == _homePosition)
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
            if (StrategicMap.Current.MapContainer.MapLevels[CombatMap.Current.Combat.CombatData.MapLevel].Field[CombatMap.Current.Combat.CombatData.X, CombatMap.Current.Combat.CombatData.Y].FindNoneTransparentHexSprite(typeof(City)) != null && CombatMap.Current.Combat.CombatData.WallType != 0)
            {
                position = 1;
                while (position < 7)
                {
                    if (CombatMap.Current.PositionStatus[position] == 1)
                    {
                        HexGeometry.CenterHexNumberToHex(CombatMap.Current.Combat.CombatData.X, CombatMap.Current.Combat.CombatData.Y, position, ref J, ref temporary);
                        if (StrategicMap.Current.MapContainer.MapLevels[CombatMap.Current.Combat.CombatData.MapLevel].Field[J, temporary].FindNoneTransparentHexSprite(typeof(City)) != null)
                        {
                            _homePosition = position;
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
        if (unitHexSprite.Unit.PositionGroup == _homePosition)
        {
            unitHexSprite.Unit.PositionGroup = 0;
        }

        position = unitHexSprite.Unit.PositionGroup;
        slot = unitHexSprite.Unit.PositionIndex;
        X = CombatMap.Current.UnitPositions.GetUnitPosition(position, slot).X;
        Y = CombatMap.Current.UnitPositions.GetUnitPosition(position, slot).Y;
        if (side == CombatSideIndex.Defender && _randomTerrain)
        {
            switch (position)
            {
                case 0:
                    if (direction > 0)
                    {
                        X = DefensePositionX[(direction - 1) * 8 + slot];
                        Y = DefensePositionY[(direction - 1) * 8 + slot];
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

        if (!_randomTerrain)
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
        _neighborWeights[1] = 500;
        _neighborWeights[2] = 500;
        _neighborWeights[4] = 1500;
        _neighborWeights[5] = 200;
        _neighborWeights[10] = 1500;
        _neighborWeights[11] = 500;
        _neighborWeights[17] = 2000;
        _neighborWeights[18] = 1500;
        _terrainWeights[19] = 2500;
        _terrainWeights[20] = 0;
        _terrainWeights[21] = 35;
        _terrainWeights[22] = 0;
        _terrainWeights[23] = 6;
        _terrainWeights[24] = 2;
        _terrainWeights[25] = 1;
        _terrainWeights[26] = 2;
        _terrainWeights[27] = 5;
        _terrainWeights[29] = 90;
        _terrainWeights[30] = 60;
        _terrainWeights[31] = 2;
        _terrainWeights[32] = 30;
        _terrainWeights[33] = 10;
        _terrainWeights[34] = 0;
        _terrainWeights[35] = 3;
        _terrainWeights[38] = 2500;
        _terrainWeights[39] = 5;
        _terrainWeights[40] = 0;
        _terrainWeights[41] = 0;
        _terrainWeights[42] = 6;
        _terrainWeights[43] = 0;
        _terrainWeights[44] = 5;
        _terrainWeights[45] = 5;
        _terrainWeights[46] = 2;
        _terrainWeights[48] = 35;
        _terrainWeights[49] = 20;
        _terrainWeights[50] = 5;
        _terrainWeights[51] = 30;
        _terrainWeights[52] = 5;
        _terrainWeights[53] = 2;
        _terrainWeights[54] = 1;
        _terrainWeights[57] = 2500;
        _terrainWeights[58] = 8;
        _terrainWeights[59] = 0;
        _terrainWeights[60] = 0;
        _terrainWeights[61] = 6;
        _terrainWeights[62] = 0;
        _terrainWeights[63] = 2;
        _terrainWeights[64] = 2;
        _terrainWeights[65] = 1;
        _terrainWeights[67] = 15;
        _terrainWeights[68] = 20;
        _terrainWeights[69] = 2;
        _terrainWeights[70] = 30;
        _terrainWeights[71] = 5;
        _terrainWeights[72] = 1;
        _terrainWeights[73] = 1;
        _terrainWeights[76] = 2500;
        _terrainWeights[77] = 8;
        _terrainWeights[78] = 0;
        _terrainWeights[79] = 0;
        _terrainWeights[80] = 6;
        _terrainWeights[81] = 0;
        _terrainWeights[82] = 2;
        _terrainWeights[83] = 2;
        _terrainWeights[84] = 1;
        _terrainWeights[86] = 15;
        _terrainWeights[87] = 20;
        _terrainWeights[88] = 2;
        _terrainWeights[89] = 20;
        _terrainWeights[90] = 5;
        _terrainWeights[91] = 1;
        _terrainWeights[92] = 1;
        _terrainWeights[95] = 2500;
        _terrainWeights[96] = 5;
        _terrainWeights[97] = 0;
        _terrainWeights[98] = 1;
        _terrainWeights[99] = 3;
        _terrainWeights[100] = 0;
        _terrainWeights[101] = 5;
        _terrainWeights[102] = 5;
        _terrainWeights[103] = 4;
        _terrainWeights[105] = 10;
        _terrainWeights[106] = 45;
        _terrainWeights[107] = 5;
        _terrainWeights[108] = 12;
        _terrainWeights[109] = 10;
        _terrainWeights[110] = 4;
        _terrainWeights[111] = 3;
        _terrainWeights[114] = 2500;
        _terrainWeights[115] = 0;
        _terrainWeights[116] = 0;
        _terrainWeights[117] = 0;
        _terrainWeights[118] = 0;
        _terrainWeights[119] = 0;
        _terrainWeights[120] = 0;
        _terrainWeights[121] = 0;
        _terrainWeights[122] = 0;
        _terrainWeights[124] = 0;
        _terrainWeights[125] = 0;
        _terrainWeights[126] = 0;
        _terrainWeights[127] = 0;
        _terrainWeights[128] = 0;
        _terrainWeights[129] = 0;
        _terrainWeights[130] = 0;
        _terrainWeights[228] = 2500;
        _terrainWeights[229] = 0;
        _terrainWeights[230] = 0;
        _terrainWeights[231] = 0;
        _terrainWeights[232] = 0;
        _terrainWeights[233] = 10;
        _terrainWeights[234] = 0;
        _terrainWeights[235] = 0;
        _terrainWeights[236] = 0;
        _terrainWeights[238] = 30;
        _terrainWeights[239] = 70;
        _terrainWeights[240] = 4;
        _terrainWeights[241] = 0;
        _terrainWeights[242] = 50;
        _terrainWeights[243] = 4;
        _terrainWeights[244] = 10;
        _terrainWeights[245] = 80;
        _tagWeights[20] = 800;
        _tagWeights[21] = 100;
        _tagWeights[29] = 300;
        _tagWeights[25] = 20;
        _tagWeights[26] = 40;
        _tagWeights[27] = 10;
        _tagWeights[40] = 900;
        _tagWeights[48] = 400;
        _tagWeights[44] = 20;
        _tagWeights[45] = 40;
        _tagWeights[46] = 10;
        _tagWeights[60] = 250;
        _tagWeights[61] = 200;
        _tagWeights[86] = 300;
        _tagWeights[80] = 500;
        _tagWeights[100] = 500;
        _tagWeights[105] = 200;
        _tagWeights[99] = 100;
        _tagWeights[120] = 250;
        _tagWeights[121] = 250;
        _tagWeights[122] = 100;
        _tagWeights[144] = 600;
        _tagWeights[147] = 250;
        _tagWeights[149] = 150;
        _tagWeights[164] = 250;
        _tagWeights[167] = 100;
        _tagWeights[163] = 200;
        _tagWeights[162] = 200;
        _tagWeights[181] = 500;
        _tagWeights[191] = 450;
        _tagWeights[200] = 150;
        _tagWeights[196] = 40;
        _tagWeights[197] = 20;
        _tagWeights[198] = 10;
        _tagWeights[211] = 450;
        _tagWeights[219] = 150;
        _tagWeights[215] = 20;
        _tagWeights[216] = 20;
        _tagWeights[217] = 40;
        _tagWeights[241] = 500;
        _tagWeights[239] = 200;
        _tagWeights[242] = 50;
        _tagWeights[244] = 20;
        _tagWeights[258] = 600;
        _tagWeights[261] = 350;
        _tagWeights[263] = 300;
        _tagWeights[181] = 600;
    }

    private static ReadOnlySpan<int> DefensePositionX => [32, 30, 34, 32, 28, 36, 30, 34, 34, 36, 32, 38, 30, 36, 32, 34, 36, 34, 38, 32, 36, 30, 34, 32, 32, 34, 30, 32, 36, 28, 34, 30, 30, 28, 32, 26, 34, 28, 32, 30, 28, 30, 26, 32, 28, 34, 30, 32];
    private static ReadOnlySpan<int> DefensePositionY => [26, 27, 27, 28, 28, 28, 29, 29, 27, 28, 26, 29, 27, 30, 28, 29, 30, 31, 29, 32, 28, 31, 29, 30, 32, 31, 31, 30, 30, 30, 29, 29, 31, 30, 32, 29, 31, 28, 30, 29, 28, 27, 29, 26, 30, 27, 29, 28];

    private static ReadOnlySpan<byte> MapShape => [
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
    ];

    private static ReadOnlySpan<byte> MapRoads => [
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
    ];

    private static ReadOnlySpan<int> ClusterCheckBox => [1, 2, 6, 16, 7, 8, 9, 17, 18, 19, 20, 21, 22, 23, 33, 34, 35, 36];

}
