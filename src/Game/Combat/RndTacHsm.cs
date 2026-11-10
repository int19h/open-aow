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
public partial class RndTacHsm : IDisposable
{
    public ItemList FItems;
    // calls GetItem
    public bool FRandomTerrain;
    public FixedArray_int_20 FNeighborWeights;
    public FixedArray_int_305 FTerrainWeights;
    public FixedArray_int_308 FTagWeights;
    public FixedArray_sbyte_7 FTerrain;
    public FixedArray_sbyte_7 FTags;
    public FixedArray_Aow_Rtl_TList_286 FResources;
    // original allocates one extra list
    public int FHomePosition;

    // AoWTC.pas:4149
    public virtual void Dispose()
    {
        for (int I = 0; I <= 285; I++)
        {
            FResources[I]?.Dispose();
        }
    }

    // AoWTC.pas:4137
    public RndTacHsm()
    {
        for (int I = 0; I <= 285; I++)
        {
            var list = new DelphiList();
            FResources[I] = list;
        }

        for (int J = 0, last = AoWHSSet.Current.FCombatResources.GetCount() - 1; J <= last; J++)
        {
            if (AoWHSSet.Current.FCombatResources.MORs[J] is CombatTerrain.Resource)
            {
                var mor = AoWHSSet.Current.FCombatResources.MORs[J];
                var mor2 = AoWHSSet.Current.FCombatResources.MORs[J];
                var mor3 = AoWHSSet.Current.FCombatResources.MORs[J];
                FResources[(sbyte)((TerrainMOR)mor2).TerrainType * 19 + ((TerrainMOR)mor3).DecorationType].Add(mor);
            }
        }

        InitTables();
    }

    // AoWTC.pas:4156
    public AoWCombatMap RandomMap(TacticalBattle Combat, int X, int Y, int L)
    {
        byte[] TerrainBuffer = [];
        int I;
        int J = default;
        int K = default;
        int NX = default;
        int TerrainType = default;
        int Tag = default;
        byte[] ShapeBuffer = [];
        byte Shape = default;
        bool HasUnit = default;
        bool SiteCombat;
        bool RandomTerrain;
        string FileName;
        Structure Structure;
        HexagonBridge Bridge;
        FixedArray_int_20 Weights = default;
        bool Blocked(int X, int Y, int First, int Last)
        {
            int Kind;
            int NX = default;
            int NY = default;
            bool Result = false;
            for (int I = First; I <= Last; I++)
            {
                HexGeometry.CenterHNtoHX(X, Y, clusterChk[I], ref NX, ref NY);
                Kind = TerrainBuffer[NY * 65 + NX];
                Result = Result || Kind == 1 || Kind == 2 || Kind == 3 || Kind == 5 || Kind == 6 || Kind == 7 || Kind == 11 || Kind == 14 || Kind == 15 || Kind == 16 || Kind == 17 || Kind == 18 || Kind == 252 || Kind == 253 || Kind == 254;
            }

            return Result;
        }

        FItems = null;
        if (Combat == null)
        {
            SiteCombat = false;
        }
        else
        {
            SiteCombat = Combat.FExplorationSite == 1;
        }

        if (Combat == null)
        {
            RandomTerrain = true;
        }
        else
        {
            RandomTerrain = !(SiteCombat || Combat.FExplorationSite == 2);
        }

        FRandomTerrain = RandomTerrain;
        CombatPlacement.CombatXLoc = X;
        CombatPlacement.CombatYLoc = Y;
        CombatPlacement.CombatLLoc = L;
        RandSeed = Y * 128 + X + L * 16384 + AoWHSMap.Current.FInitialRandomSeed;
        I = 0;
        for (; I <= 6; I += 1)
        {
            HexGeometry.CenterHNtoHX(X, Y, I, ref J, ref K);
            var mapField = AoWHSMap.Current.MapContainer.MapLevels[L].Field[J, K];
            FTerrain[I] = mapField.TerrainType;
            var combatTerrainTag = ((AoWMapField)AoWHSMap.Current.MapContainer.MapLevels[L].Field[J, K]).GetCombatTerrainTag();
            FTags[I] = (sbyte)combatTerrainTag;
        }

        if (FTerrain[0] == 0 || FTerrain[0] == 9 || FTerrain[0] == 10)
        {
            I = 1;
            for (; I <= 6; I += 1)
            {
                FTerrain[I] = FTerrain[0];
            }
        }
        else
        {
            I = 1;
            for (; I <= 6; I += 1)
            {
                if (FTerrain[I] == 0 || FTerrain[I] == 15 || FTerrain[I] == 10 || FTerrain[I] == 9)
                {
                    FTerrain[I] = FTerrain[0];
                }
            }
        }

        FileName = "TCMaps\\";
        Structure = (Structure)AoWHSMap.Current.MapContainer.MapLevels[L].Field[X, Y].FindNoneTransparentHS(typeof(Structure));
        Bridge = null;
        if (Structure == null)
        {
            Bridge = (HexagonBridge)AoWHSMap.Current.MapContainer.MapLevels[L].Field[X, Y].FindNoneTransparentHS(typeof(HexagonBridge));
        }

        if (Structure != null)
        {
            // The kind names the map (Structure.Views.cs) and reports what the map takes from the combat.
            var site = new CombatMapSite(SiteCombat, RandomTerrain, (Direction, Kind) =>
            {
                HexGeometry.CenterHNtoHX(X, Y, Direction, ref J, ref K);
                return AoWHSMap.Current.MapContainer.MapLevels[L].Field[J, K].FindNoneTransparentHS(Kind) != null;
            });
            FileName = FileName + Structure.CombatMapName(site);
            RandomTerrain = site.RandomTerrain;
            // Prisoners are an army on the same field. Only an item list is laid out.
            if (site.TakesCombatItems && Combat.FItems is ItemList items)
            {
                FItems = items;
            }

            for (int Direction = 1; Direction <= 6; Direction++)
            {
                if ((site.FixedTags & (1 << Direction)) != 0)
                {
                    FTags[Direction] = 127;
                }
            }

            FTags[0] = 127;
        }
        else if (Bridge != null)
        {
            RandomTerrain = false;
            FRandomTerrain = false;
            FileName = FileName + "Br";
            switch (FTerrain[0])
            {
                case 10:
                    FileName = FileName + "UW";
                    break;
                case 6:
                    FileName = FileName + "Ic";
                    break;
                case 9:
                    FileName = FileName + "La";
                    break;
                case 13:
                    FileName = FileName + "UI";
                    break;
                default:
                    FileName = FileName + "Wa";
                    break;
            }

            switch (Bridge.FDirection)
            {
                case 1:
                    FileName = FileName + "A000";
                    break;
                case 2:
                    FileName = FileName + "B000";
                    break;
                default:
                    FileName = FileName + "C000";
                    break;
            }

            FTags[0] = 127;
        }

        if (FTags[0] != 127)
        {
            FileName = FileName + "Blank000";
        }

        FileName = FileName + ".HSM";
        if (Combat == null)
        {
            GameTables.AoWEngine.LoadHSMEx(FileBoundary.ReadName(GameTables.AoWEngine.StartupDirectory + FileName), false, null, GameProfile.TacticalMap);
        }
        else
        {
            AoWHSMap.Current.InitializeCombatMap(Combat, FileName);
        }

        AoWCombatMap.Current.SetCombat(Combat);
        AoWCombatMap.Current.SetExplorationEnabled(SiteCombat);
        AoWCombatMap.Current.SetFogEnabled(false);
        if (Combat != null)
        {
            PlaceCombatUnits();
        }

        if (RandomTerrain)
        {
            AoWCombatMap.Current.LockMapChangedEvent();
            ShapeBuffer = new byte[3770];
            TerrainBuffer = new byte[3770];
            for (int Row = 6; Row <= 52; Row++)
            {
                for (int Col = 6; Col <= 58; Col++)
                {
                    Shape = MapShape[Row * 65 + Col];
                    switch (Shape)
                    {
                        case 14:
                            if (Random(100) < 50)
                            {
                                Shape = 1;
                            }
                            else
                            {
                                Shape = 2;
                            }

                            break;
                        case 15:
                            if (Random(100) < 50)
                            {
                                Shape = 1;
                            }
                            else
                            {
                                Shape = 3;
                            }

                            break;
                        case 16:
                            if (Random(100) < 50)
                            {
                                Shape = 1;
                            }
                            else
                            {
                                Shape = 4;
                            }

                            break;
                        case 17:
                            if (Random(100) < 50)
                            {
                                Shape = 1;
                            }
                            else
                            {
                                Shape = 5;
                            }

                            break;
                        case 18:
                            if (Random(100) < 50)
                            {
                                Shape = 1;
                            }
                            else
                            {
                                Shape = 6;
                            }

                            break;
                        case 19:
                            if (Random(100) < 50)
                            {
                                Shape = 1;
                            }
                            else
                            {
                                Shape = 7;
                            }

                            break;
                        case 11:
                            if (Random(100) < 50)
                            {
                                Shape = 2;
                            }
                            else
                            {
                                Shape = 3;
                            }

                            break;
                        case 9:
                            if (Random(100) < 50)
                            {
                                Shape = 3;
                            }
                            else
                            {
                                Shape = 4;
                            }

                            break;
                        case 13:
                            if (Random(100) < 50)
                            {
                                Shape = 5;
                            }
                            else
                            {
                                Shape = 4;
                            }

                            break;
                        case 12:
                            if (Random(100) < 50)
                            {
                                Shape = 5;
                            }
                            else
                            {
                                Shape = 6;
                            }

                            break;
                        case 8:
                            if (Random(100) < 50)
                            {
                                Shape = 7;
                            }
                            else
                            {
                                Shape = 6;
                            }

                            break;
                        case 10:
                            if (Random(100) < 50)
                            {
                                Shape = 2;
                            }
                            else
                            {
                                Shape = 7;
                            }

                            break;
                    }

                    if (AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Col, Row].GetDecorationType() == 9)
                    {
                        switch (Shape)
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
                            HexGeometry.CenterHNtoHX(X, Y, J, ref K, ref NX);
                            if (AoWHSMap.Current.MapContainer.MapLevels[L].Field[K, NX].FindOwnedChild(131946) == null)
                            {
                                AoWCombatMap.Current.ClearTerrain(Col, Row, 0, 0, -1);
                            }
                        }
                    }

                    ShapeBuffer[Row * 65 + Col] = Shape;
                }
            }

            if (AoWHSMap.Current.MapContainer.MapLevels[L].Field[X, Y].OverlayType == 4)
            {
                I = 0;
                for (; I <= 6; I += 1)
                {
                    HexGeometry.CenterHNtoHX(X, Y, I, ref J, ref K);
                    var item = AoWHSMap.Current.MapContainer.MapLevels[L].Field[J, K];
                    Weights[I] = item.OverlayType;
                }

                for (int Row = 6; Row <= 52; Row++)
                {
                    for (int Col = 6; Col <= 58; Col++)
                    {
                        Shape = MapRoads[Row * 65 + Col];
                        switch (Shape)
                        {
                            case 21:
                                if (Random(100) < 50)
                                {
                                    Shape = 0;
                                }
                                else
                                {
                                    Shape = 2;
                                }

                                break;
                            case 22:
                                if (Random(100) < 50)
                                {
                                    Shape = 0;
                                }
                                else
                                {
                                    Shape = 3;
                                }

                                break;
                            case 23:
                                if (Random(100) < 50)
                                {
                                    Shape = 0;
                                }
                                else
                                {
                                    Shape = 4;
                                }

                                break;
                            case 24:
                                if (Random(100) < 50)
                                {
                                    Shape = 0;
                                }
                                else
                                {
                                    Shape = 5;
                                }

                                break;
                            case 25:
                                if (Random(100) < 50)
                                {
                                    Shape = 0;
                                }
                                else
                                {
                                    Shape = 6;
                                }

                                break;
                            case 26:
                                if (Random(100) < 50)
                                {
                                    Shape = 0;
                                }
                                else
                                {
                                    Shape = 7;
                                }

                                break;
                        }

                        if (Set<byte>.WithBounds(0, 255).WithRange(1, 7).Contains(Shape))
                        {
                            if (FTags[Shape - 1] != 127 && Weights[Shape - 1] == 4)
                            {
                                AoWHSSet.Current.FCombatRoads.Place(AoWCombatMap.Current.MapContainer, (sbyte)Col, (sbyte)Row, 0, 1);
                                TerrainBuffer[Row * 65 + Col] = 251;
                            }
                        }
                    }
                }
            }

            for (int Row = 6; Row <= 52; Row++)
            {
                for (int Col = 3; Col <= 29; Col++)
                {
                    I = ShapeBuffer[Row * 65 + Col * 2];
                    if (I > 0)
                    {
                        if (FTags[I - 1] == 127 || TerrainBuffer[Row * 65 + Col * 2] == 251)
                        {
                            I = 0;
                        }
                    }

                    if (I > 0)
                    {
                        TerrainType = FTerrain[I - 1];
                        Tag = FTags[I - 1];
                        I = 0;
                        for (; I <= 18; I += 1)
                        {
                            Weights[I] = FTerrainWeights[TerrainType * 19 + I];
                            if (Tag < 14)
                            {
                                Weights[I] += FTagWeights[Tag * 19 + I];
                            }
                        }

                        I = 0;
                        for (; I <= 2; I += 1)
                        {
                            HexGeometry.CenterHNtoHX(Col * 2, Row, clusterChk[I], ref J, ref K);
                            Tag = TerrainBuffer[K * 65 + J];
                            if (Tag <= 19)
                            {
                                Weights[Tag] += FNeighborWeights[Tag];
                            }

                            switch (Tag)
                            {
                                case >= 1 and <= 3:
                                    Weights[5] += 10;
                                    Weights[10] += 10;
                                    Weights[4] += 10;
                                    break;
                                case 14:
                                case 16:
                                    Weights[11] += FNeighborWeights[11];
                                    break;
                            }
                        }

                        HasUnit = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Col * 2, Row].FindChild(2228484) != null;
                        if (HasUnit || Blocked(Col * 2, Row, 0, 2))
                        {
                            if ((HasUnit || Blocked(Col * 2, Row, 4, 8)) && (HasUnit || Blocked(Col * 2, Row, 9, 17)))
                            {
                                Weights[1] = 0;
                                Weights[2] = 0;
                                Weights[3] = 0;
                                Weights[5] = 0;
                                Weights[6] = 0;
                                Weights[7] = 0;
                                Weights[11] = 0;
                                Weights[14] = 0;
                                Weights[15] = 0;
                                Weights[16] = 0;
                                Weights[17] = 0;
                                Weights[18] = 0;
                            }
                        }
                        else
                        {
                            if ((HasUnit || Blocked(Col * 2, Row, 4, 8)) && (HasUnit || Blocked(Col * 2, Row, 9, 17)))
                            {
                                Weights[14] = 0;
                                Weights[15] = 0;
                                Weights[16] = 0;
                            }
                        }

                        J = 0;
                        I = 0;
                        for (; I <= 18; I += 1)
                        {
                            J += Weights[I];
                        }

                        Tag = Random(J) - Weights[0];
                        J = 0;
                        while (Tag > 0 && J < 18)
                        {
                            J += 1;
                            Tag -= Weights[J];
                        }

                        if (TerrainType == 8)
                        {
                            J = 17;
                        }

                        switch (J)
                        {
                            case 14:
                                HexGeometry.CenterHNtoHX(Col * 2, Row, 6, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 252;
                                break;
                            case 15:
                                HexGeometry.CenterHNtoHX(Col * 2, Row, 6, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 253;
                                break;
                            case 16:
                                HexGeometry.CenterHNtoHX(Col * 2, Row, 1, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 254;
                                HexGeometry.CenterHNtoHX(Col * 2, Row, 2, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 254;
                                HexGeometry.CenterHNtoHX(Col * 2, Row, 6, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 254;
                                break;
                        }

                        TerrainBuffer[Row * 65 + Col * 2] = (byte)J;
                    }
                }

                for (int Col = 3; Col <= 29; Col++)
                {
                    I = ShapeBuffer[Row * 65 + Col * 2 + 1];
                    if (I > 0)
                    {
                        if (FTags[I - 1] == 127 || TerrainBuffer[Row * 65 + Col * 2 + 1] == 251)
                        {
                            I = 0;
                        }
                    }

                    if (I > 0)
                    {
                        TerrainType = FTerrain[I - 1];
                        Tag = FTags[I - 1];
                        I = 0;
                        for (; I <= 18; I += 1)
                        {
                            Weights[I] = FTerrainWeights[TerrainType * 19 + I];
                            if (Tag < 14)
                            {
                                Weights[I] += FTagWeights[Tag * 19 + I];
                            }
                        }

                        I = 0;
                        for (; I <= 2; I += 1)
                        {
                            HexGeometry.CenterHNtoHX(Col * 2 + 1, Row, clusterChk[I], ref J, ref K);
                            Tag = TerrainBuffer[K * 65 + J];
                            if (Tag <= 19)
                            {
                                Weights[Tag] += FNeighborWeights[Tag];
                            }

                            switch (Tag)
                            {
                                case >= 1 and <= 3:
                                    Weights[5] += 10;
                                    Weights[10] += 10;
                                    Weights[4] += 10;
                                    break;
                                case 14:
                                case 16:
                                    Weights[11] += FNeighborWeights[11];
                                    break;
                            }
                        }

                        HasUnit = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Col * 2 + 1, Row].FindChild(2228484) != null;
                        if (HasUnit || Blocked(Col * 2 + 1, Row, 0, 2))
                        {
                            if ((HasUnit || Blocked(Col * 2 + 1, Row, 4, 8)) && (HasUnit || Blocked(Col * 2 + 1, Row, 9, 17)))
                            {
                                Weights[1] = 0;
                                Weights[2] = 0;
                                Weights[3] = 0;
                                Weights[5] = 0;
                                Weights[6] = 0;
                                Weights[7] = 0;
                                Weights[11] = 0;
                                Weights[14] = 0;
                                Weights[15] = 0;
                                Weights[16] = 0;
                                Weights[17] = 0;
                                Weights[18] = 0;
                            }
                        }
                        else
                        {
                            if ((HasUnit || Blocked(Col * 2 + 1, Row, 4, 8)) && (HasUnit || Blocked(Col * 2 + 1, Row, 9, 17)))
                            {
                                Weights[14] = 0;
                                Weights[15] = 0;
                                Weights[16] = 0;
                            }
                        }

                        J = 0;
                        I = 0;
                        for (; I <= 18; I += 1)
                        {
                            J += Weights[I];
                        }

                        Tag = Random(J) - Weights[0];
                        J = 0;
                        while (Tag > 0 && J < 18)
                        {
                            J += 1;
                            Tag -= Weights[J];
                        }

                        if (TerrainType == 8)
                        {
                            J = 17;
                        }

                        switch (J)
                        {
                            case 14:
                                HexGeometry.CenterHNtoHX(Col * 2 + 1, Row, 6, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 252;
                                break;
                            case 15:
                                HexGeometry.CenterHNtoHX(Col * 2 + 1, Row, 6, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 253;
                                break;
                            case 16:
                                HexGeometry.CenterHNtoHX(Col * 2 + 1, Row, 1, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 254;
                                HexGeometry.CenterHNtoHX(Col * 2 + 1, Row, 2, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 254;
                                HexGeometry.CenterHNtoHX(Col * 2 + 1, Row, 6, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 254;
                                break;
                        }

                        TerrainBuffer[Row * 65 + Col * 2 + 1] = (byte)J;
                    }
                }
            }

            for (int Row = 6; Row <= 52; Row++)
            {
                for (int Col = 6; Col <= 58; Col++)
                {
                    I = ShapeBuffer[Row * 65 + Col];
                    if (I > 0)
                    {
                        J = TerrainBuffer[Row * 65 + Col];
                        if (J == 17)
                        {
                            AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Col, Row].TerrainType = 7;
                        }
                        else
                        {
                            AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Col, Row].TerrainType = FTerrain[I - 1];
                            if (J > 0 && J < 19)
                            {
                                if (FResources[FTerrain[I - 1] * 19 + J].Count > 0)
                                {
                                    var random = Random(FResources[FTerrain[I - 1] * 19 + J].Count);
                                    ((MapObjectResource)FResources[FTerrain[I - 1] * 19 + J][random]).PlaceResourceUser(AoWCombatMap.Current, (sbyte)Col, (sbyte)Row, 0);
                                }
                            }
                        }
                    }
                }
            }

            for (int Row = 6; Row <= 52; Row++)
            {
                for (int Col = 6; Col <= 58; Col++)
                {
                    AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Col, Row].TerrainChanged();
                }
            }

            TerrainBuffer = default;
            AoWCombatMap.Current.UnlockMapChangedEvent();
        }

        return AoWCombatMap.Current;
    }

    // AoWTC.pas:4519
    public void PlaceCombatUnits()
    {
        int Position;
        int I;
        int J = default;
        int Temp = default;
        DelphiList Units;
        TacticalCombatUnit U;
        TacticalCombatUnit Other;
        int Direction;
        FixedArray_bool_8 Used = default;
        CItemHS ItemHS;
        Direction = 32;
        FHomePosition = -1;
        Units = new DelphiList();
        Position = 0;
        for (; Position <= 7; Position += 1)
        {
            Units.Clear();
            I = 0;
            int last = AoWCombatMap.Current.FCombat.FCombatData.GetCount() - 1;
            for (; I <= last; I += 1)
            {
                if (AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit)
                {
                    if (AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled())
                    {
                        if ((int)(((uint)AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).FPosition) >> 4) == Position)
                        {
                            Units.Add(AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I));
                            U = (TacticalCombatUnit)Units[Units.Count - 1];
                            U.FPositionIndex = (byte)(U.FPosition & 15);
                            U.FPositionGroup = (byte)(int)(((uint)U.FPosition) >> 4);
                            if (U.FPositionGroup == 7)
                            {
                                U.FPositionGroup = 1;
                            }

                            if (U.FPositionGroup == 0)
                            {
                                FHomePosition = 0;
                            }

                            if (FHomePosition == -1)
                            {
                                if (U.GetSide() == CombatSideIndex.Defender)
                                {
                                    if (AoWHSMap.Current.MapContainer.MapLevels[AoWCombatMap.Current.FCombat.FCombatData.FL].Field[AoWCombatMap.Current.FCombat.FCombatData.FX, AoWCombatMap.Current.FCombat.FCombatData.FY].FindNoneTransparentHS(typeof(City)) != null)
                                    {
                                    }
                                }
                            }

                            if (FHomePosition > 0 && U.FPositionGroup == FHomePosition)
                            {
                                U.FPositionGroup = 0;
                            }

                            var side = U.GetSide();
                            AoWCombatMap.Current.FPositionStatus[U.FPositionGroup] = (byte)side;
                        }
                    }
                }
            }

            I = 0;
            int limit = Units.Count - 1;
            for (; I <= limit; I += 1)
            {
                U = (TacticalCombatUnit)Units[I];
                J = I + 1;
                int last2 = Units.Count - 1;
                for (; J <= last2; J += 1)
                {
                    Other = (TacticalCombatUnit)Units[J];
                    if (U.GetPosValue() < Other.GetPosValue())
                    {
                        Temp = U.FPositionIndex;
                        U.FPositionIndex = Other.FPositionIndex;
                        Other.FPositionIndex = (byte)Temp;
                    }
                }
            }
        }

        if (AoWCombatMap.Current.FPositionStatus[0] != 1)
        {
            if (AoWHSMap.Current.MapContainer.MapLevels[AoWCombatMap.Current.FCombat.FCombatData.FL].Field[AoWCombatMap.Current.FCombat.FCombatData.FX, AoWCombatMap.Current.FCombat.FCombatData.FY].FindNoneTransparentHS(typeof(City)) != null && AoWCombatMap.Current.FCombat.FCombatData.FWallType != 0)
            {
                Position = 1;
                while (Position < 7)
                {
                    if (AoWCombatMap.Current.FPositionStatus[Position] == 1)
                    {
                        HexGeometry.CenterHNtoHX(AoWCombatMap.Current.FCombat.FCombatData.FX, AoWCombatMap.Current.FCombat.FCombatData.FY, Position, ref J, ref Temp);
                        if (AoWHSMap.Current.MapContainer.MapLevels[AoWCombatMap.Current.FCombat.FCombatData.FL].Field[J, Temp].FindNoneTransparentHS(typeof(City)) != null)
                        {
                            FHomePosition = Position;
                            AoWCombatMap.Current.FPositionStatus[Position] = (int)CombatSideIndex.Neutral;
                            AoWCombatMap.Current.FPositionStatus[0] = 1;
                            Position = 7;
                        }
                    }

                    Position += 1;
                }
            }
        }

        Position = 0;
        int last3 = AoWCombatMap.Current.FCombat.FSides[(int)CombatSideIndex.Attacker].GetCount() - 1;
        for (; Position <= last3; Position += 1)
        {
            if (AoWCombatMap.Current.FCombat.FSides[(int)CombatSideIndex.Attacker].GetObjects(Position).GetEnabled())
            {
                SetupUnit(CombatSideIndex.Attacker, Position, ref Direction);
            }
        }

        Position = 0;
        int last4 = AoWCombatMap.Current.FCombat.FSides[(int)CombatSideIndex.Defender].GetCount() - 1;
        for (; Position <= last4; Position += 1)
        {
            if (AoWCombatMap.Current.FCombat.FSides[(int)CombatSideIndex.Defender].GetObjects(Position) is CombatUnit)
            {
                if (AoWCombatMap.Current.FCombat.FSides[(int)CombatSideIndex.Defender].GetObjects(Position).GetEnabled())
                {
                    SetupUnit(CombatSideIndex.Defender, Position, ref Direction);
                }
            }
        }

        Units?.Dispose();
        if (FItems != null)
        {
            Position = 0;
            for (; Position <= 7; Position += 1)
            {
                Used[Position] = false;
            }

            Position = 0;
            while (Position < FItems.GetCount())
            {
                I = Random(AoWCombatMap.Current.FUnitPositions.FPositions.Count - 16);
                J = I - 1;
                if (J < 0)
                {
                    J = AoWCombatMap.Current.FUnitPositions.FPositions.Count - 17;
                }

                // { Original indexes its eight-slot occupancy array with the position-list index. }
                while (Used[I] && I != J)
                {
                    I += 1;
                    if (I > AoWCombatMap.Current.FUnitPositions.FPositions.Count - 17)
                    {
                        I = 0;
                    }
                }

                Used[I] = true;
                J = AoWCombatMap.Current.FUnitPositions.GetUnitPosition(2, I).X;
                Temp = AoWCombatMap.Current.FUnitPositions.GetUnitPosition(2, I).Y;
                ItemHS = new CItemHS(null);
                if (AoWCombatMap.Current.PlaceHX(ItemHS, J, Temp, 0))
                {
                    FItems.GetItem(Position).SetOwner(ItemHS.FItems);
                }
                else
                {
                    Position += 1;
                }
            }
        }
    }

    // AoWTC.pas:4610
    public void SetupUnit(CombatSideIndex Side, int Index, ref int Direction)
    {
        TacticalCombatUnitHS UnitHS;
        TacticalCombatUnitHS TargetHS;
        int X;
        int Y;
        int Position;
        int Slot;
        int HN;
        int NX = default;
        int NY = default;
        UnitHS = new TacticalCombatUnitHS(null);
        UnitHS.Initialize((TacticalCombatUnit)AoWCombatMap.Current.FCombat.FSides[(int)Side].GetObjects(Index));
        if (UnitHS.FUnit.FPositionGroup == FHomePosition)
        {
            UnitHS.FUnit.FPositionGroup = 0;
        }

        Position = UnitHS.FUnit.FPositionGroup;
        Slot = UnitHS.FUnit.FPositionIndex;
        X = AoWCombatMap.Current.FUnitPositions.GetUnitPosition(Position, Slot).X;
        Y = AoWCombatMap.Current.FUnitPositions.GetUnitPosition(Position, Slot).Y;
        if (Side == CombatSideIndex.Defender && FRandomTerrain)
        {
            switch (Position)
            {
                case 0:
                    if (Direction > 0)
                    {
                        X = DefPosX[(Direction - 1) * 8 + Slot];
                        Y = DefPosY[(Direction - 1) * 8 + Slot];
                        UnitHS.SetDirection((byte)Direction);
                    }
                    else
                    {
                        switch (Slot)
                        {
                            case 0:
                                UnitHS.SetDirection(1);
                                break;
                            case 1:
                                UnitHS.SetDirection(4);
                                break;
                            case 2:
                            case 6:
                                UnitHS.SetDirection(5);
                                break;
                            case 3:
                            case 7:
                                UnitHS.SetDirection(2);
                                break;
                            case 4:
                                UnitHS.SetDirection(3);
                                break;
                            case 5:
                                UnitHS.SetDirection(6);
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
            if (Direction != Position)
            {
                if (Direction == 32)
                {
                    Direction = Position;
                }
                else
                {
                    Direction = 0;
                }
            }
        }

        if (!FRandomTerrain)
        {
            UnitHS.SetDirection((byte)AoWCombatMap.Current.FUnitPositions.GetUnitPosition(Position, Slot).Flag);
        }
        else
        {
            switch (Position)
            {
                case 1:
                    UnitHS.SetDirection(4);
                    break;
                case 2:
                    UnitHS.SetDirection(5);
                    break;
                case 3:
                    UnitHS.SetDirection(6);
                    break;
                case 4:
                    UnitHS.SetDirection(1);
                    break;
                case 5:
                    UnitHS.SetDirection(2);
                    break;
                case 6:
                    UnitHS.SetDirection(3);
                    break;
            }
        }

        HN = 0;
        do
        {
            HexGeometry.CenterHNtoHX(X, Y, HN, ref NX, ref NY);
            Slot = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[NX, NY].OverlayType;
            TargetHS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[NX, NY].FindOwnedChild(2228484);
            HN += 1;
        }
        while (!(Slot != 8 && TargetHS == null));
        AoWCombatMap.Current.PlaceHX(UnitHS, NX, NY, 0);
        if (HexGeometry.dHXtoHN(AoWCombatMap.Current.FCombat.FCombatData.FX, AoWCombatMap.Current.FCombat.FCombatData.FY, AoWCombatMap.Current.FCombat.FCombatData.FAttackerX, AoWCombatMap.Current.FCombat.FCombatData.FAttackerY) == Position && AoWCombatMap.Current.FPlayers.GetPlayers(UnitHS.FUnit.FCombatPlayer).FLastX == 0)
        {
            var xhx = UnitHS.GetXhx();
            AoWCombatMap.Current.FPlayers.GetPlayers(UnitHS.FUnit.FCombatPlayer).FLastX = xhx;
            var yhx = UnitHS.GetYhx();
            AoWCombatMap.Current.FPlayers.GetPlayers(UnitHS.FUnit.FCombatPlayer).FLastY = yhx;
        }
    }

    // AoWTC.pas:4667
    public void InitTables()
    {
        FNeighborWeights[1] = 500;
        FNeighborWeights[2] = 500;
        FNeighborWeights[4] = 1500;
        FNeighborWeights[5] = 200;
        FNeighborWeights[10] = 1500;
        FNeighborWeights[11] = 500;
        FNeighborWeights[17] = 2000;
        FNeighborWeights[18] = 1500;
        FTerrainWeights[19] = 2500;
        FTerrainWeights[20] = 0;
        FTerrainWeights[21] = 35;
        FTerrainWeights[22] = 0;
        FTerrainWeights[23] = 6;
        FTerrainWeights[24] = 2;
        FTerrainWeights[25] = 1;
        FTerrainWeights[26] = 2;
        FTerrainWeights[27] = 5;
        FTerrainWeights[29] = 90;
        FTerrainWeights[30] = 60;
        FTerrainWeights[31] = 2;
        FTerrainWeights[32] = 30;
        FTerrainWeights[33] = 10;
        FTerrainWeights[34] = 0;
        FTerrainWeights[35] = 3;
        FTerrainWeights[38] = 2500;
        FTerrainWeights[39] = 5;
        FTerrainWeights[40] = 0;
        FTerrainWeights[41] = 0;
        FTerrainWeights[42] = 6;
        FTerrainWeights[43] = 0;
        FTerrainWeights[44] = 5;
        FTerrainWeights[45] = 5;
        FTerrainWeights[46] = 2;
        FTerrainWeights[48] = 35;
        FTerrainWeights[49] = 20;
        FTerrainWeights[50] = 5;
        FTerrainWeights[51] = 30;
        FTerrainWeights[52] = 5;
        FTerrainWeights[53] = 2;
        FTerrainWeights[54] = 1;
        FTerrainWeights[57] = 2500;
        FTerrainWeights[58] = 8;
        FTerrainWeights[59] = 0;
        FTerrainWeights[60] = 0;
        FTerrainWeights[61] = 6;
        FTerrainWeights[62] = 0;
        FTerrainWeights[63] = 2;
        FTerrainWeights[64] = 2;
        FTerrainWeights[65] = 1;
        FTerrainWeights[67] = 15;
        FTerrainWeights[68] = 20;
        FTerrainWeights[69] = 2;
        FTerrainWeights[70] = 30;
        FTerrainWeights[71] = 5;
        FTerrainWeights[72] = 1;
        FTerrainWeights[73] = 1;
        FTerrainWeights[76] = 2500;
        FTerrainWeights[77] = 8;
        FTerrainWeights[78] = 0;
        FTerrainWeights[79] = 0;
        FTerrainWeights[80] = 6;
        FTerrainWeights[81] = 0;
        FTerrainWeights[82] = 2;
        FTerrainWeights[83] = 2;
        FTerrainWeights[84] = 1;
        FTerrainWeights[86] = 15;
        FTerrainWeights[87] = 20;
        FTerrainWeights[88] = 2;
        FTerrainWeights[89] = 20;
        FTerrainWeights[90] = 5;
        FTerrainWeights[91] = 1;
        FTerrainWeights[92] = 1;
        FTerrainWeights[95] = 2500;
        FTerrainWeights[96] = 5;
        FTerrainWeights[97] = 0;
        FTerrainWeights[98] = 1;
        FTerrainWeights[99] = 3;
        FTerrainWeights[100] = 0;
        FTerrainWeights[101] = 5;
        FTerrainWeights[102] = 5;
        FTerrainWeights[103] = 4;
        FTerrainWeights[105] = 10;
        FTerrainWeights[106] = 45;
        FTerrainWeights[107] = 5;
        FTerrainWeights[108] = 12;
        FTerrainWeights[109] = 10;
        FTerrainWeights[110] = 4;
        FTerrainWeights[111] = 3;
        FTerrainWeights[114] = 2500;
        FTerrainWeights[115] = 0;
        FTerrainWeights[116] = 0;
        FTerrainWeights[117] = 0;
        FTerrainWeights[118] = 0;
        FTerrainWeights[119] = 0;
        FTerrainWeights[120] = 0;
        FTerrainWeights[121] = 0;
        FTerrainWeights[122] = 0;
        FTerrainWeights[124] = 0;
        FTerrainWeights[125] = 0;
        FTerrainWeights[126] = 0;
        FTerrainWeights[127] = 0;
        FTerrainWeights[128] = 0;
        FTerrainWeights[129] = 0;
        FTerrainWeights[130] = 0;
        FTerrainWeights[228] = 2500;
        FTerrainWeights[229] = 0;
        FTerrainWeights[230] = 0;
        FTerrainWeights[231] = 0;
        FTerrainWeights[232] = 0;
        FTerrainWeights[233] = 10;
        FTerrainWeights[234] = 0;
        FTerrainWeights[235] = 0;
        FTerrainWeights[236] = 0;
        FTerrainWeights[238] = 30;
        FTerrainWeights[239] = 70;
        FTerrainWeights[240] = 4;
        FTerrainWeights[241] = 0;
        FTerrainWeights[242] = 50;
        FTerrainWeights[243] = 4;
        FTerrainWeights[244] = 10;
        FTerrainWeights[245] = 80;
        FTagWeights[20] = 800;
        FTagWeights[21] = 100;
        FTagWeights[29] = 300;
        FTagWeights[25] = 20;
        FTagWeights[26] = 40;
        FTagWeights[27] = 10;
        FTagWeights[40] = 900;
        FTagWeights[48] = 400;
        FTagWeights[44] = 20;
        FTagWeights[45] = 40;
        FTagWeights[46] = 10;
        FTagWeights[60] = 250;
        FTagWeights[61] = 200;
        FTagWeights[86] = 300;
        FTagWeights[80] = 500;
        FTagWeights[100] = 500;
        FTagWeights[105] = 200;
        FTagWeights[99] = 100;
        FTagWeights[120] = 250;
        FTagWeights[121] = 250;
        FTagWeights[122] = 100;
        FTagWeights[144] = 600;
        FTagWeights[147] = 250;
        FTagWeights[149] = 150;
        FTagWeights[164] = 250;
        FTagWeights[167] = 100;
        FTagWeights[163] = 200;
        FTagWeights[162] = 200;
        FTagWeights[181] = 500;
        FTagWeights[191] = 450;
        FTagWeights[200] = 150;
        FTagWeights[196] = 40;
        FTagWeights[197] = 20;
        FTagWeights[198] = 10;
        FTagWeights[211] = 450;
        FTagWeights[219] = 150;
        FTagWeights[215] = 20;
        FTagWeights[216] = 20;
        FTagWeights[217] = 40;
        FTagWeights[241] = 500;
        FTagWeights[239] = 200;
        FTagWeights[242] = 50;
        FTagWeights[244] = 20;
        FTagWeights[258] = 600;
        FTagWeights[261] = 350;
        FTagWeights[263] = 300;
        FTagWeights[181] = 600;
    }

    private static FixedArray_int_48 DefPosX = FixedArray_int_48.From([32, 30, 34, 32, 28, 36, 30, 34, 34, 36, 32, 38, 30, 36, 32, 34, 36, 34, 38, 32, 36, 30, 34, 32, 32, 34, 30, 32, 36, 28, 34, 30, 30, 28, 32, 26, 34, 28, 32, 30, 28, 30, 26, 32, 28, 34, 30, 32]);
    private static FixedArray_int_48 DefPosY = FixedArray_int_48.From([26, 27, 27, 28, 28, 28, 29, 29, 27, 28, 26, 29, 27, 30, 28, 29, 30, 31, 29, 32, 28, 31, 29, 30, 32, 31, 31, 30, 30, 30, 29, 29, 31, 30, 32, 29, 31, 28, 30, 29, 28, 27, 29, 26, 30, 27, 29, 28]);

    private static FixedArray_byte_3770 MapShape = FixedArray_byte_3770.From((ReadOnlySpan<byte>)new byte[3770]
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

    private static FixedArray_byte_3770 MapRoads = FixedArray_byte_3770.From((ReadOnlySpan<byte>)new byte[3770]
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

    private static FixedArray_int_18 clusterChk = FixedArray_int_18.From([1, 2, 6, 16, 7, 8, 9, 17, 18, 19, 20, 21, 22, 23, 33, 34, 35, 36]);

}
