// Translated from AoWE.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Collections;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// FAllUnits. FActiveUnits. FSides. FResult. FSuperiority. FEfficiency. FWall. FRound.
// FNextCPUID. FInvalid. FWork
public partial class CombatPredictor : IDisposable
{
    // CombatPredictor.pas:292
    public CombatPredictor()
    {
        FAllUnits = new CombatPredictorUnitList();
        FActiveUnits = new CombatPredictorUnitList();
        var combatPredictorSide = new CombatPredictorSide();
        FSides[0] = combatPredictorSide;
        var combatPredictorSide2 = new CombatPredictorSide();
        FSides[1] = combatPredictorSide2;
    }

    public CombatPredictorUnitList FAllUnits;
    public CombatPredictorUnitList FActiveUnits;
    public FixedArray_Aow_Units_AoWE_TCombatPredictorSide_2 FSides;
    public byte FResult;
    public int FSuperiority;
    public int FEfficiency;
    public CombatPredictorUnit FWall;
    public int FRound;
    public int FNextCPUID;
    public byte FInvalid;
    public int FWork;

    // CombatPredictor.pas:301
    public virtual void Dispose()
    {
        Reset();
        FAllUnits?.Dispose();
        FActiveUnits?.Dispose();
        FSides[0]?.Dispose();
        FSides[1]?.Dispose();
    }

    // CombatPredictor.pas:311
    public void Reset()
    {
        for (int I = FAllUnits.Count - 1; I >= 0; I--)
        {
            (FAllUnits.List[I] as IDisposable)?.Dispose();
        }

        FWall = null;
        FAllUnits.Count = 0;
        FActiveUnits.Count = 0;
        FSides[0].Count = 0;
        FSides[1].Count = 0;
        FSuperiority = -1;
        FEfficiency = -1;
        FNextCPUID = 0;
    }

    // CombatPredictor.pas:325
    public int CreateUniqueCPUID()
    {
        int Result = FNextCPUID;
        FNextCPUID += 1;
        return Result;
    }

    // CombatPredictor.pas:328
    public void EnableAllUnits()
    {
        for (int I = FAllUnits.Count - 1; I >= 0; I--)
        {
            if (((CombatPredictorUnit)FAllUnits.List[I]).FEnabled == 0)
            {
                ((CombatPredictorUnit)FAllUnits.List[I]).FEnabled = 1;
                FInvalid = 1;
            }
        }
    }

    // CombatPredictor.pas:339
    public void Validate()
    {
        if (FInvalid != 0)
        {
            Execute();
        }
    }

    // CombatPredictor.pas:342
    public void ResetCombat()
    {
        FInvalid = 1;
        FSuperiority = -1;
        FEfficiency = -1;
        FRound = 0;
        for (int I = FAllUnits.Count - 1; I >= 0; I--)
        {
            ((CombatPredictorUnit)FAllUnits.List[I]).ResetCombat();
        }
    }

    // CombatPredictor.pas:352
    public void SetupCombat(byte WallPhase)
    {
        CombatPredictorUnit U;
        FActiveUnits.Clear();
        if (FSides[0].F_CombatPredictorSide_10[0] != 0)
        {
            for (int I = FSides[1].Count - 1; I >= 0; I--)
            {
                ((CombatPredictorUnit)FSides[1].List[I]).InvalidateTargets();
            }

            FSides[0].F_CombatPredictorSide_10[0] = 0;
        }

        if (FSides[1].F_CombatPredictorSide_10[0] != 0)
        {
            for (int I = FSides[0].Count - 1; I >= 0; I--)
            {
                ((CombatPredictorUnit)FSides[0].List[I]).InvalidateTargets();
            }

            FSides[1].F_CombatPredictorSide_10[0] = 0;
        }

        for (int I = FAllUnits.Count - 1; I >= 0; I--)
        {
            ((CombatPredictorUnit)FAllUnits.List[I]).ResetRoundTargets();
        }

        for (int I = FAllUnits.Count - 1; I >= 0; I--)
        {
            ((CombatPredictorUnit)FAllUnits.List[I]).SetupRoundTargets(WallPhase);
        }

        for (int I = FAllUnits.Count - 1; I >= 0; I--)
        {
            ((CombatPredictorUnit)FAllUnits.List[I]).UpdateRoundTargetPriority();
        }

        for (int I = FAllUnits.Count - 1; I >= 0; I--)
        {
            U = (CombatPredictorUnit)FAllUnits.List[I];
            if (U.FEnabled != 0 && U.FRoundTargets.Count != 0)
            {
                U.SortRoundTargets();
                FActiveUnits.Add(U);
            }
        }
    }

    // CombatPredictor.pas:380
    public bool ExecuteCombatRound()
    {
        CombatPredictorUnit U;
        bool Result = false;
        for (int I = FActiveUnits.Count - 1; I >= 0; I--)
        {
            U = (CombatPredictorUnit)FActiveUnits.List[I];
            if (!U.ExecuteRound())
            {
                FActiveUnits.Delete(I);
            }
            else
            {
                Result = true;
            }
        }

        for (int I = FActiveUnits.Count - 1; I >= 0; I--)
        {
            U = (CombatPredictorUnit)FActiveUnits.List[I];
            if (U.FHitPoints <= 0)
            {
                FActiveUnits.Delete(I);
            }
        }

        if (Result)
        {
            FRound += 1;
        }

        return Result;
    }

    // CombatPredictor.pas:398
    public bool CanPassWall()
    {
        bool Result;
        int I;
        try
        {
            Result = false;
            I = FAllUnits.Count;
            while (!Result && I != 0)
            {
                I -= 1;
                if (FAllUnits.GetUnits(I).FUnit != null)
                {
                    Result = FAllUnits.GetUnits(I).FUnit.GetWallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing);
                }
            }
        }
        catch
        {
            Result = false;
        }

        return Result;
    }

    // CombatPredictor.pas:415
    public void FinishCombat()
    {
        CombatPredictorUnit Wall;
        FixedArray_int_2 Surviving;
        FixedArray_int_2 Hits;
        CombatPredictorUnit U;
        Surviving = default;
        Hits = default;
        for (int I = FAllUnits.Count - 1; I >= 0; I--)
        {
            U = (CombatPredictorUnit)FAllUnits.List[I];
            U.FinishCombat();
            if (!U.FStatus.Contains(CombatPredictorUnitStatusElement.Destroyed))
            {
                Surviving[U.FSide] += 1;
            }

            Hits[U.FSide] += U.FHitPoints;
        }

        if (Surviving[1] == 0 && Surviving[0] == 0)
        {
            if (Hits[0] >= Hits[1])
            {
                FResult = 3;
            }
            else
            {
                FResult = 4;
            }
        }
        else
        {
            if (Surviving[1] == 0)
            {
                FResult = 3;
            }
            else
            {
                Wall = FWall;
                if (Wall != null && Wall.FHitPoints > 0)
                {
                    if (Surviving[1] == 1 && CanPassWall())
                    {
                        FResult = 3;
                    }
                    else
                    {
                        FResult = 8;
                    }
                }
                else
                {
                    if (Surviving[0] == 0)
                    {
                        FResult = 4;
                    }
                    else
                    {
                        FResult = 2;
                    }
                }
            }
        }

        FActiveUnits.Clear();
    }

    // CombatPredictor.pas:440
    public CombatResult Execute()
    {
        if (FWall != null)
        {
            ResetCombat();
            SetupCombat(1);
            while (FWall.FHitPoints > 0 && ExecuteCombatRound())
            {
            }

            if (FWall.FHitPoints <= 0)
            {
                SetupCombat(0);
                while (ExecuteCombatRound())
                {
                }
            }

            FinishCombat();
        }
        else
        {
            ResetCombat();
            SetupCombat(0);
            while (ExecuteCombatRound())
            {
            }

            FinishCombat();
        }

        AoWHSMap.Current.FAIExecuter.UseProcessTime(FWork * 58);
        CombatResult Result = (CombatResult)FResult;
        FInvalid = 0;
        return Result;
    }

    // CombatPredictor.pas:466
    public void DeleteUnit(AbstractUnit AUnit)
    {
        int I;
        int J;
        CombatPredictorUnit U;
        DamageType D;
        I = FAllUnits.IndexOfUnit(AUnit);
        if (I != -1)
        {
            U = FAllUnits.GetUnits(I);
            J = FSides[U.FSide].IndexOfUnit(AUnit);
            if (J != -1)
            {
                if (FSides[U.FSide].GetUnits(J) == FWall)
                {
                    FWall = null;
                }

                FSides[U.FSide].Delete(J);
                FSides[U.FSide].F_CombatPredictorSide_10[0] = 1;
                // { Original adds strength when deleting a unit. }
                for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
                {
                    D = (DamageType)dOrdinal;
                    var strength = AUnit.GetStrength(D);
                    FSides[U.FSide].FStrength[(int)D] += strength;
                }
            }

            FInvalid = 1;
            FAllUnits.Delete(I);
            U?.Dispose();
        }
    }

    // CombatPredictor.pas:489
    public CombatPredictorUnit AddUnitEx(AbstractUnit AUnit, byte Side, byte BehindWall)
    {
        CombatPredictorUnit U;
        DamageType D;
        U = new CombatPredictorUnit();
        U.Setup(AUnit, (CombatPredictor)(object)this, Side);
        U.FBehindWall = BehindWall;
        FAllUnits.Add(U);
        FSides[Side].Add(U);
        FSides[Side].F_CombatPredictorSide_10[0] = 1;
        for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
        {
            D = (DamageType)dOrdinal;
            var strength = AUnit.GetStrength(D);
            FSides[Side].FStrength[(int)D] += strength;
        }

        FInvalid = 1;
        return U;
    }

    // CombatPredictor.pas:504
    public void AddUnit(AbstractUnit AUnit)
    {
        AddUnitEx(AUnit, 0, 0);
    }

    // CombatPredictor.pas:507
    public void AddUnits(UnitList Units)
    {
        for (int I = 0, last = Units.Count - 1; I <= last; I++)
        {
            AddUnit(Units.GetUnit(I));
        }
    }

    // CombatPredictor.pas:514
    public void AddOpponent(AbstractUnit AUnit, byte BehindWall)
    {
        AddUnitEx(AUnit, 1, BehindWall);
    }

    // CombatPredictor.pas:517
    public void AddOpponents(UnitList Units, byte BehindWall)
    {
        for (int I = 0, last = Units.Count - 1; I <= last; I++)
        {
            AddOpponent(Units.GetUnit(I), BehindWall);
        }
    }

    // CombatPredictor.pas:524
    public void AddWall(WallType WallType)
    {
        WallUnit Wall;
        CombatPredictorUnit UnitObj;
        if (WallType == WallType.None)
        {
            // { Preserve the original assertion diagnostic. }
            Assert(false, "Use DeleteWall instead!", "D:\\AoWDev\\AoWE\\inc\\CombatPredictor.pas", 18391);
        }

        UnitObj = FWall;
        if (UnitObj == null)
        {
            Wall = new WallUnit(null);
            Wall.SetWallType(WallType);
            Wall.FHits = (sbyte)(Wall.FHits + 5);
            Wall.FHitPoints = (sbyte)(Wall.FHitPoints + 5);
            FWall = AddUnitEx(Wall, 1, 0);
            Wall.Release();
        }
        else
        {
            ((WallUnit)UnitObj.FUnit).SetWallType(WallType);
        }
    }

    // CombatPredictor.pas:714
    public void DeleteWall()
    {
        CombatPredictorUnit Wall;
        Wall = FWall;
        if (Wall != null)
        {
            DeleteUnit(Wall.FUnit);
            FWall = null;
        }
    }

    // CombatPredictor.pas:725
    public void DeleteUnits()
    {
        for (int I = FSides[0].Count - 1; I >= 0; I--)
        {
            DeleteUnit(FSides[0].GetUnits(I).FUnit);
        }
    }

    // CombatPredictor.pas:732
    public void DeleteOpponents()
    {
        for (int I = FSides[1].Count - 1; I >= 0; I--)
        {
            DeleteUnit(FSides[1].GetUnits(I).FUnit);
        }
    }

    // CombatPredictor.pas:739
    public void InitAdjacentArmies(ArmyHS Army, sbyte X, sbyte Y, sbyte L)
    {
        MapLevel Level;
        ArmyHS ArmyHS;
        AbstractUnit U;
        byte Defender;
        byte Player;
        byte Relation;
        byte DefenderRelation;
        CombatSideIndex Side;
        MapField Field;
        MapFieldMsg Msg = default;
        HNWalkData Walk = default;
        AddUnits(Army.FArmy);
        Level = AoWHSMap.Current.MapContainer.GetMapLevel(L);
        var army = Army.FArmy;
        Player = (byte)army.FPlayer;
        Relation = (byte)army.FIndependentRelation;

        Defender = (byte)((AoWMapField)Level.GetField(X, Y)).GetPlayer();
        DefenderRelation = ((AoWMapField)Level.GetField(X, Y)).GetIndependentRelation();
        if (Defender != 255)
        {
            Msg.MsgID = 4387;
            var fieldValue = Level.GetField(X, Y);
            Msg.Param[0] = fieldValue;
            Msg.Result = 0;
            Level.GetField(X, Y).SendMapFieldMsg(ref Msg);
            if ((WallType)Msg.Result.Int != WallType.None)
            {
                AddWall((WallType)Msg.Result.Int);
            }

            HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.Radius <= 1)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    ArmyHS = (ArmyHS)Level.GetField(Walk.X, Walk.Y).FindOwnedChild(131607);
                    if (ArmyHS != null && ArmyHS != Army)
                    {
                        Side = CombatAbilityTables.GetCombatSide((sbyte)Player, (sbyte)Relation, (sbyte)Defender, (sbyte)DefenderRelation, ArmyHS.FArmy.FPlayer, ArmyHS.FArmy.FIndependentRelation);
                        if (Side != CombatSideIndex.Neutral)
                        {
                            Field = Level.GetField(X, Y);
                            Msg.MsgID = 4387;
                            var levelField = Level.GetField(Walk.X, Walk.Y);
                            Msg.Param[0] = levelField;
                            Msg.Result = 0;
                            Field.SendMapFieldMsg(ref Msg);
                            for (int I = 0, last = ArmyHS.FArmy.Count - 1; I <= last; I++)
                            {
                                U = ArmyHS.FArmy.GetUnit(I);
                                if (U.ValidTerrain(Field.TerrainType, Field.OverlayType))
                                {
                                    AddUnitEx(U, (byte)(int)Side, (byte)((WallType)Msg.Result.Int != WallType.None ? 1 : 0));
                                }
                            }
                        }
                    }
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }
        }
    }

    // CombatPredictor.pas:790
    public void InitAdjacentOpponents(byte Player, byte Relation, sbyte X, sbyte Y, sbyte L)
    {
        MapLevel Level;
        ArmyHS ArmyHS;
        AbstractUnit U;
        byte DefenderRelation;
        byte Defender;
        CombatSideIndex Side;
        MapField Field;
        MapFieldMsg Msg = default;
        HNWalkData Walk = default;
        Level = AoWHSMap.Current.MapContainer.GetMapLevel(L);
        Defender = (byte)((AoWMapField)Level.GetField(X, Y)).GetPlayer();
        DefenderRelation = ((AoWMapField)Level.GetField(X, Y)).GetIndependentRelation();
        if (Defender != 255)
        {
            Msg.MsgID = 4387;
            var fieldValue = Level.GetField(X, Y);
            Msg.Param[0] = fieldValue;
            Msg.Result = 0;
            Level.GetField(X, Y).SendMapFieldMsg(ref Msg);
            if ((WallType)Msg.Result.Int != WallType.None)
            {
                AddWall((WallType)Msg.Result.Int);
            }

            HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.Radius <= 1)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    ArmyHS = (ArmyHS)Level.GetField(Walk.X, Walk.Y).FindOwnedChild(131607);
                    if (ArmyHS != null)
                    {
                        Side = CombatAbilityTables.GetCombatSide((sbyte)Player, (sbyte)Relation, (sbyte)Defender, (sbyte)DefenderRelation, ArmyHS.FArmy.FPlayer, ArmyHS.FArmy.FIndependentRelation);
                        if (Side == CombatSideIndex.Defender)
                        {
                            Field = Level.GetField(X, Y);
                            Msg.MsgID = 4387;
                            var levelField = Level.GetField(Walk.X, Walk.Y);
                            Msg.Param[0] = levelField;
                            Msg.Result = 0;
                            Field.SendMapFieldMsg(ref Msg);
                            for (int I = 0, last = ArmyHS.FArmy.Count - 1; I <= last; I++)
                            {
                                U = ArmyHS.FArmy.GetUnit(I);
                                if (U.ValidTerrain(Field.TerrainType, Field.OverlayType))
                                {
                                    AddUnitEx(U, (byte)(int)Side, (byte)((WallType)Msg.Result.Int != WallType.None ? 1 : 0));
                                }
                            }
                        }
                    }
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }
        }
    }

    // CombatPredictor.pas:835
    public int GetSuperiority()
    {
        CombatPredictorUnit U;
        int Total;
        int Left;
        int Value;
        if (FSuperiority == -1)
        {
            if (FResult == 3)
            {
                Total = 0;
                Left = 0;
                for (int I = FSides[0].Count - 1; I >= 0; I--)
                {
                    U = (CombatPredictorUnit)FSides[0].List[I];
                    Value = U.FCombatValueTotal;
                    Total += Value;
                    if (U.FHitPoints > 0)
                    {
                        Left += U.FHitPoints * Value / U.FInitialHitPoints;
                    }
                }

                if (Total == Left)
                {
                    FSuperiority = 999;
                }
                else
                {
                    if (Left > 0)
                    {
                        FSuperiority = Total * 100 / (Total - Left);
                    }
                    else
                    {
                        FSuperiority = 100;
                    }
                }
            }
            else
            {
                if (Set<byte>.WithBounds(0, 255).With(2).With(4).With(8).Contains(FResult))
                {
                    Total = 0;
                    Left = 0;
                    for (int I = FSides[1].Count - 1; I >= 0; I--)
                    {
                        U = (CombatPredictorUnit)FSides[1].List[I];
                        Value = U.FCombatValueTotal;
                        Total += Value;
                        if (U.FHitPoints > 0)
                        {
                            Left += U.FHitPoints * Value / U.FInitialHitPoints;
                        }
                    }

                    if (Total == 0)
                    {
                        FSuperiority = 0;
                    }
                    else
                    {
                        if (Left > 0)
                        {
                            FSuperiority = (Total - Left) * 100 / Total;
                        }
                        else
                        {
                            FSuperiority = 100;
                        }
                    }
                }
                else
                {
                    if (FResult == 6)
                    {
                        FSuperiority = 200;
                    }
                    else
                    {
                        FSuperiority = 0;
                    }
                }
            }
        }

        return FSuperiority;
    }

    // CombatPredictor.pas:872
    public int GetEfficiency()
    {
        FixedArray_int_2 Lost = default;
        byte Side;
        CombatPredictorUnit U;
        if (FEfficiency == -1)
        {
            for (int sideOrdinal = 0; sideOrdinal <= 1; sideOrdinal++)
            {
                Side = (byte)sideOrdinal;
                Lost[Side] = 0;
                for (int I = FSides[Side].Count - 1; I >= 0; I--)
                {
                    U = (CombatPredictorUnit)FSides[Side].List[I];
                    if (U.FStatus.Contains(CombatPredictorUnitStatusElement.Destroyed))
                    {
                        var obtainValue = U.FUnit.GetObtainValue();
                        Lost[Side] += obtainValue;
                    }
                    else
                    {
                        if (U.FHitPoints < U.FInitialHitPoints)
                        {
                            var obtainValue = U.FUnit.GetObtainValue();
                            Lost[Side] += Round((double)(obtainValue * U.FHitPoints) / (U.FInitialHitPoints * 2));
                        }
                    }
                }
            }

            if (Lost[0] != 0)
            {
                FEfficiency = Lost[1] * 100 / Lost[0];
            }
            else
            {
                if (Lost[1] != 0 || FSides[1].Count == 0)
                {
                    FEfficiency = 999;
                }
                else
                {
                    FEfficiency = 100;
                }
            }
        }

        return FEfficiency;
    }

    // CombatPredictor.pas:899
    public void ListRequiredUnitsEx(UnitList Required, UnitList Optional, UnitList Useless, int Superiority)
    {
        DoubleItemList<CombatPredictorUnit> List = default;
        int Total = default;
        int Score = default;
        int Budget = default;
        CombatPredictorUnit U = default;
        int Maximum = default;
        int Minimum = default;
        int Count = default;
        int Previous = default;
        bool Done = default;
        EnableAllUnits();
        Validate();
        if (GetSuperiority() > Superiority && FSides[0].Count != 0)
        {
            List = new DoubleItemList<CombatPredictorUnit>();
            Total = 0;
            for (int I = FSides[0].Count - 1; I >= 0; I--)
            {
                U = (CombatPredictorUnit)FSides[0].List[I];
                if (U.FIncomingCombatValue != 0)
                {
                    Score = U.FCombatValueTotal * 100 / U.FIncomingCombatValue;
                }
                else
                {
                    Score = 9999;
                }

                Total += U.FCombatValueTotal;
                List.AddAB(U, Score);
            }

            List.Sort(P3CompareRequiredUnits);
            Minimum = 1;
            Maximum = List.Count;
            if (Superiority != 0 && GetSuperiority() != 0)
            {
                Budget = Round((double)((double)((double)100 / Superiority) - (double)100 / GetSuperiority()) * ((double)Total * 0.65));
                Count = List.Count;
                while (Count > 1 && Budget > 0)
                {
                    Count -= 1;
                    Budget -= List[Count].A.FCombatValueTotal;
                }

                if (Count < Minimum)
                {
                    Count = Minimum;
                }

                if (Count >= Maximum)
                {
                    Count = Maximum - 1;
                }
            }
            else
            {
                Count = Maximum - 1;
            }

            Previous = Maximum;
            Done = List.Count <= 1;
            while (!Done)
            {
                for (int I = Count - 1; I >= 0; I--)
                {
                    List.List[I].A.SetEnabled(1);
                }

                for (int I = List.Count - 1; I >= Count; I--)
                {
                    List.List[I].A.SetEnabled(0);
                }

                Execute();
                if (GetSuperiority() >= Superiority)
                {
                    Previous = Count;
                    Done = Minimum == Previous;
                    if (!Done)
                    {
                        Count -= 1;
                        if (Count > Minimum && GetSuperiority() - Superiority > 300 / List.Count)
                        {
                            Count -= 1;
                        }

                        if (Count > Minimum && GetSuperiority() - Superiority > 600 / List.Count)
                        {
                            Count -= 1;
                        }
                    }
                }
                else
                {
                    if (GetSuperiority() < Superiority)
                    {
                        Minimum = Count + 1;
                        Done = Previous == Minimum;
                        if (!Done)
                        {
                            Count += 1;
                            if (Count + 1 != Previous && Superiority - GetSuperiority() > 300 / List.Count)
                            {
                                Count += 1;
                            }

                            if (Count + 1 != Previous && Superiority - GetSuperiority() > 600 / List.Count)
                            {
                                Count += 1;
                            }
                        }
                    }
                }
            }

            if (Required != null)
            {
                for (int I = 0; I <= Minimum - 1; I++)
                {
                    Required.AddUnit(List.List[I].A.FUnit);
                }
            }

            if (Optional != null)
            {
                for (int I = List.Count - 1; I >= Minimum; I--)
                {
                    Optional.AddUnit(List.List[I].A.FUnit);
                }
            }

            if (Useless != null)
            {
                for (int I = List.Count - 1; I >= Minimum; I--)
                {
                    U = List.List[I].A;
                    if (U.FCombatValueTotal <= 0)
                    {
                        Useless.AddUnit(U.FUnit);
                    }
                }
            }

            List?.Dispose();
        }
        else
        {
            for (int I = FSides[0].Count - 1; I >= 0; I--)
            {
                U = (CombatPredictorUnit)FSides[0].List[I];
                if (U.FCombatValueTotal == 0)
                {
                    if (Useless != null)
                    {
                        Useless.AddUnit(U.FUnit);
                    }
                }
                else
                {
                    if (Required != null)
                    {
                        Required.AddUnit(U.FUnit);
                    }
                }
            }
        }
    }

    // CombatPredictor.pas:988
    public void ListRequiredUnits(UnitList Required, int Superiority)
    {
        ListRequiredUnitsEx(Required, null, null, Superiority);
    }

    // CombatPredictor.pas:991
    public void ListOptionalUnits(UnitList Optional, int Superiority)
    {
        ListRequiredUnitsEx(null, Optional, null, Superiority);
    }

    // CombatPredictor.pas:994
    public void ListUselessUnits(UnitList Useless)
    {
        CombatPredictorUnit U;
        for (int I = FSides[0].Count - 1; I >= 0; I--)
        {
            U = (CombatPredictorUnit)FSides[0].List[I];
            if (U.FCombatValueTotal <= 0)
            {
                Useless.AddUnit(U.FUnit);
            }
        }
    }

    // CombatPredictor.pas:1004
    public int Usefullness(AbstractUnit AUnit)
    {
        int Result;
        CombatPredictorUnit Wall;
        CombatPredictorUnit U;
        CombatPredictorUnit Target;
        int I;
        bool AllDead;
        bool Useful;
        EnableAllUnits();
        Validate();
        U = new CombatPredictorUnit();
        U.Setup(AUnit, (CombatPredictor)(object)this, 0);
        Wall = FWall;
        if (Wall != null && Wall.FHitPoints > 0)
        {
            U.UpdateWallTargets();
            Result = U.FWallCombatValue;
            Useful = false;
            I = U.FWallTargets.Count;
            while (I != 0 && !Useful)
            {
                I -= 1;
                if (U.FWallTargets.List[I].B > 0)
                {
                    Target = U.FWallTargets.List[I].A;
                    Useful = Target == FWall && Target.FHitPoints > 0;
                }
            }

            if (Useful)
            {
                Result = Result | 8388608;
            }
        }
        else
        {
            U.UpdateTargets();
            Result = U.FCombatValue;
            I = FSides[0].Count;
            AllDead = true;
            while (I != 0 && AllDead)
            {
                I -= 1;
                AllDead = FSides[0].GetUnits(I).FStatus.Contains(CombatPredictorUnitStatusElement.Destroyed);
            }

            Useful = false;
            I = U.FTargets.Count;
            while (I != 0 && !Useful)
            {
                I -= 1;
                if (U.FTargets.List[I].B > 0)
                {
                    Target = U.FTargets.List[I].A;
                    Useful = Target != FWall && (Target.FHitPoints > 0 || FResult == 3 || AllDead);
                }
            }

            if (Useful)
            {
                Result = Result | 8388608;
            }
        }

        U?.Dispose();
        return Result;
    }

    private static int P3CompareRequiredUnits(in DoubleItem<CombatPredictorUnit> A, in DoubleItem<CombatPredictorUnit> B)
    {
        return B.B - A.B;
    }

    // intf_P3_01.inc:92

    // intf_P3_01.inc:92

}
