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

// AllUnits. ActiveUnits. Sides. Result. Superiority. Efficiency. Wall. Round.
// NextCpuId. Invalid. Work
public partial class CombatPredictor : IDisposable
{
    // CombatPredictor.pas:292
    public CombatPredictor()
    {
        AllUnits = new CombatPredictorUnitList();
        ActiveUnits = new CombatPredictorUnitList();
        var combatPredictorSide = new CombatPredictorSide();
        Sides[0] = combatPredictorSide;
        var combatPredictorSide2 = new CombatPredictorSide();
        Sides[1] = combatPredictorSide2;
    }

    public CombatPredictorUnitList AllUnits;
    public CombatPredictorUnitList ActiveUnits;
    public CombatPredictorSideArray2 Sides;
    public byte Result;
    public int Superiority;
    public int Efficiency;
    public CombatPredictorUnit Wall;
    public int Round;
    public int NextCpuId;
    public byte Invalid;
    public int Work;

    // CombatPredictor.pas:301
    public virtual void Dispose()
    {
        Reset();
        AllUnits?.Dispose();
        ActiveUnits?.Dispose();
        Sides[0]?.Dispose();
        Sides[1]?.Dispose();
    }

    // CombatPredictor.pas:311
    public void Reset()
    {
        for (int I = AllUnits.Count - 1; I >= 0; I--)
        {
            (AllUnits.List[I] as IDisposable)?.Dispose();
        }

        Wall = null;
        AllUnits.Count = 0;
        ActiveUnits.Count = 0;
        Sides[0].Count = 0;
        Sides[1].Count = 0;
        Superiority = -1;
        Efficiency = -1;
        NextCpuId = 0;
    }

    // CombatPredictor.pas:325
    public int CreateUniqueCpuId()
    {
        int result = NextCpuId;
        NextCpuId += 1;
        return result;
    }

    // CombatPredictor.pas:328
    public void EnableAllUnits()
    {
        for (int I = AllUnits.Count - 1; I >= 0; I--)
        {
            if ((AllUnits.List[I]).Enabled == 0)
            {
                (AllUnits.List[I]).Enabled = 1;
                Invalid = 1;
            }
        }
    }

    // CombatPredictor.pas:339
    public void Validate()
    {
        if (Invalid != 0)
        {
            Execute();
        }
    }

    // CombatPredictor.pas:342
    public void ResetCombat()
    {
        Invalid = 1;
        Superiority = -1;
        Efficiency = -1;
        Round = 0;
        for (int I = AllUnits.Count - 1; I >= 0; I--)
        {
            (AllUnits.List[I]).ResetCombat();
        }
    }

    // CombatPredictor.pas:352
    public void SetupCombat(byte wallPhase)
    {
        CombatPredictorUnit U;
        ActiveUnits.Clear();
        if (Sides[0].UnitRemovedFlags[0] != 0)
        {
            for (int I = Sides[1].Count - 1; I >= 0; I--)
            {
                (Sides[1].List[I]).InvalidateTargets();
            }

            Sides[0].UnitRemovedFlags[0] = 0;
        }

        if (Sides[1].UnitRemovedFlags[0] != 0)
        {
            for (int I = Sides[0].Count - 1; I >= 0; I--)
            {
                (Sides[0].List[I]).InvalidateTargets();
            }

            Sides[1].UnitRemovedFlags[0] = 0;
        }

        for (int I = AllUnits.Count - 1; I >= 0; I--)
        {
            (AllUnits.List[I]).ResetRoundTargets();
        }

        for (int I = AllUnits.Count - 1; I >= 0; I--)
        {
            (AllUnits.List[I]).SetupRoundTargets(wallPhase);
        }

        for (int I = AllUnits.Count - 1; I >= 0; I--)
        {
            (AllUnits.List[I]).UpdateRoundTargetPriority();
        }

        for (int I = AllUnits.Count - 1; I >= 0; I--)
        {
            U = AllUnits.List[I];
            if (U.Enabled != 0 && U.RoundTargets.Count != 0)
            {
                U.SortRoundTargets();
                ActiveUnits.Add(U);
            }
        }
    }

    // CombatPredictor.pas:380
    public bool ExecuteCombatRound()
    {
        CombatPredictorUnit U;
        bool result = false;
        for (int I = ActiveUnits.Count - 1; I >= 0; I--)
        {
            U = ActiveUnits.List[I];
            if (!U.ExecuteRound())
            {
                ActiveUnits.Delete(I);
            }
            else
            {
                result = true;
            }
        }

        for (int I = ActiveUnits.Count - 1; I >= 0; I--)
        {
            U = ActiveUnits.List[I];
            if (U.HitPoints <= 0)
            {
                ActiveUnits.Delete(I);
            }
        }

        if (result)
        {
            Round += 1;
        }

        return result;
    }

    // CombatPredictor.pas:398
    public bool CanPassWall()
    {
        bool result;
        int I;
        try
        {
            result = false;
            I = AllUnits.Count;
            while (!result && I != 0)
            {
                I -= 1;
                if (AllUnits.GetUnits(I).Troop != null)
                {
                    result = AllUnits.GetUnits(I).Troop.GetWallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing);
                }
            }
        }
        catch
        {
            result = false;
        }

        return result;
    }

    // CombatPredictor.pas:415
    public void FinishCombat()
    {
        CombatPredictorUnit wall;
        Int32Array2 surviving;
        Int32Array2 hits;
        CombatPredictorUnit U;
        surviving = default;
        hits = default;
        for (int I = AllUnits.Count - 1; I >= 0; I--)
        {
            U = AllUnits.List[I];
            U.FinishCombat();
            if (!U.Status.Contains(CombatPredictorUnitStatusElement.Destroyed))
            {
                surviving[U.Side] += 1;
            }

            hits[U.Side] += U.HitPoints;
        }

        if (surviving[1] == 0 && surviving[0] == 0)
        {
            if (hits[0] >= hits[1])
            {
                Result = 3;
            }
            else
            {
                Result = 4;
            }
        }
        else
        {
            if (surviving[1] == 0)
            {
                Result = 3;
            }
            else
            {
                wall = Wall;
                if (wall != null && wall.HitPoints > 0)
                {
                    if (surviving[1] == 1 && CanPassWall())
                    {
                        Result = 3;
                    }
                    else
                    {
                        Result = 8;
                    }
                }
                else
                {
                    if (surviving[0] == 0)
                    {
                        Result = 4;
                    }
                    else
                    {
                        Result = 2;
                    }
                }
            }
        }

        ActiveUnits.Clear();
    }

    // CombatPredictor.pas:440
    public CombatResult Execute()
    {
        if (Wall != null)
        {
            ResetCombat();
            SetupCombat(1);
            while (Wall.HitPoints > 0 && ExecuteCombatRound())
            {
            }

            if (Wall.HitPoints <= 0)
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

        AoWHSMap.Current.AIExecutor.UseProcessTime(Work * 58);
        CombatResult result = (CombatResult)Result;
        Invalid = 0;
        return result;
    }

    // CombatPredictor.pas:466
    public void DeleteUnit(Troop troop)
    {
        int I;
        int J;
        CombatPredictorUnit U;
        DamageType D;
        I = AllUnits.IndexOfUnit(troop);
        if (I != -1)
        {
            U = AllUnits.GetUnits(I);
            J = Sides[U.Side].IndexOfUnit(troop);
            if (J != -1)
            {
                if (Sides[U.Side].GetUnits(J) == Wall)
                {
                    Wall = null;
                }

                Sides[U.Side].Delete(J);
                Sides[U.Side].UnitRemovedFlags[0] = 1;
                // { Original adds strength when deleting a unit. }
                for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
                {
                    D = (DamageType)dOrdinal;
                    var strength = troop.GetStrength(D);
                    Sides[U.Side].Strength[(int)D] += strength;
                }
            }

            Invalid = 1;
            AllUnits.Delete(I);
            U?.Dispose();
        }
    }

    // CombatPredictor.pas:489
    public CombatPredictorUnit AddUnit(Troop troop, byte side, byte behindWall)
    {
        CombatPredictorUnit U;
        DamageType D;
        U = new CombatPredictorUnit();
        U.Setup(troop, (CombatPredictor)(object)this, side);
        U.BehindWall = behindWall;
        AllUnits.Add(U);
        Sides[side].Add(U);
        Sides[side].UnitRemovedFlags[0] = 1;
        for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
        {
            D = (DamageType)dOrdinal;
            var strength = troop.GetStrength(D);
            Sides[side].Strength[(int)D] += strength;
        }

        Invalid = 1;
        return U;
    }

    // CombatPredictor.pas:504
    public void AddUnit(Troop troop)
    {
        AddUnit(troop, 0, 0);
    }

    // CombatPredictor.pas:507
    public void AddUnits(UnitList units)
    {
        for (int I = 0, last = units.Count - 1; I <= last; I++)
        {
            AddUnit(units.GetUnit(I));
        }
    }

    // CombatPredictor.pas:514
    public void AddOpponent(Troop troop, byte behindWall)
    {
        AddUnit(troop, 1, behindWall);
    }

    // CombatPredictor.pas:517
    public void AddOpponents(UnitList units, byte behindWall)
    {
        for (int I = 0, last = units.Count - 1; I <= last; I++)
        {
            AddOpponent(units.GetUnit(I), behindWall);
        }
    }

    // CombatPredictor.pas:524
    public void AddWall(WallType wallType)
    {
        WallCombatant wall;
        CombatPredictorUnit unitObject;
        if (wallType == WallType.None)
        {
            // { Preserve the original assertion diagnostic. }
            Assert(false, "Use DeleteWall instead!", "D:\\AoWDev\\AoWE\\inc\\CombatPredictor.pas", 18391);
        }

        unitObject = Wall;
        if (unitObject == null)
        {
            wall = new WallCombatant(null);
            wall.SetWallType(wallType);
            wall.Hits = (sbyte)(wall.Hits + 5);
            wall.HitPoints = (sbyte)(wall.HitPoints + 5);
            Wall = AddUnit(wall, 1, 0);
            wall.Release();
        }
        else
        {
            ((WallCombatant)unitObject.Troop).SetWallType(wallType);
        }
    }

    // CombatPredictor.pas:714
    public void DeleteWall()
    {
        CombatPredictorUnit wall;
        wall = Wall;
        if (wall != null)
        {
            DeleteUnit(wall.Troop);
            Wall = null;
        }
    }

    // CombatPredictor.pas:725
    public void DeleteUnits()
    {
        for (int I = Sides[0].Count - 1; I >= 0; I--)
        {
            DeleteUnit(Sides[0].GetUnits(I).Troop);
        }
    }

    // CombatPredictor.pas:732
    public void DeleteOpponents()
    {
        for (int I = Sides[1].Count - 1; I >= 0; I--)
        {
            DeleteUnit(Sides[1].GetUnits(I).Troop);
        }
    }

    // CombatPredictor.pas:739
    public void InitializeAdjacentArmies(ArmyHexSprite army, sbyte X, sbyte Y, sbyte L)
    {
        MapLevel level;
        ArmyHexSprite armyHexSprite;
        Troop U;
        byte defender;
        byte player;
        byte relation;
        byte defenderRelation;
        CombatSideIndex side;
        MapField field;
        MapFieldMessage message = default;
        HexNumberWalkData walk = default;
        AddUnits(army.Army);
        level = AoWHSMap.Current.MapContainer.GetMapLevel(L);
        var armyLocal = army.Army;
        player = (byte)armyLocal.Player;
        relation = (byte)armyLocal.IndependentRelation;

        defender = (byte)((AoWMapField)level.GetField(X, Y)).GetPlayer();
        defenderRelation = ((AoWMapField)level.GetField(X, Y)).GetIndependentRelation();
        if (defender != 255)
        {
            message.MessageId = 4387;
            var fieldValue = level.GetField(X, Y);
            message.Param[0] = fieldValue;
            message.Result = 0;
            level.GetField(X, Y).SendMapFieldMessage(ref message);
            if ((WallType)message.Result.Int != WallType.None)
            {
                AddWall((WallType)message.Result.Int);
            }

            HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
            while (walk.Radius <= 1)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    armyHexSprite = (ArmyHexSprite)level.GetField(walk.X, walk.Y).FindOwnedChild(131607);
                    if (armyHexSprite != null && armyHexSprite != army)
                    {
                        side = CombatAbilityTables.GetCombatSide((sbyte)player, (sbyte)relation, (sbyte)defender, (sbyte)defenderRelation, armyHexSprite.Army.Player, armyHexSprite.Army.IndependentRelation);
                        if (side != CombatSideIndex.Neutral)
                        {
                            field = level.GetField(X, Y);
                            message.MessageId = 4387;
                            var levelField = level.GetField(walk.X, walk.Y);
                            message.Param[0] = levelField;
                            message.Result = 0;
                            field.SendMapFieldMessage(ref message);
                            for (int I = 0, last = armyHexSprite.Army.Count - 1; I <= last; I++)
                            {
                                U = armyHexSprite.Army.GetUnit(I);
                                if (U.ValidTerrain(field.TerrainType, field.OverlayType))
                                {
                                    AddUnit(U, (byte)(int)side, (byte)((WallType)message.Result.Int != WallType.None ? 1 : 0));
                                }
                            }
                        }
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
    }

    // CombatPredictor.pas:790
    public void InitializeAdjacentOpponents(byte player, byte relation, sbyte X, sbyte Y, sbyte L)
    {
        MapLevel level;
        ArmyHexSprite armyHexSprite;
        Troop U;
        byte defenderRelation;
        byte defender;
        CombatSideIndex side;
        MapField field;
        MapFieldMessage message = default;
        HexNumberWalkData walk = default;
        level = AoWHSMap.Current.MapContainer.GetMapLevel(L);
        defender = (byte)((AoWMapField)level.GetField(X, Y)).GetPlayer();
        defenderRelation = ((AoWMapField)level.GetField(X, Y)).GetIndependentRelation();
        if (defender != 255)
        {
            message.MessageId = 4387;
            var fieldValue = level.GetField(X, Y);
            message.Param[0] = fieldValue;
            message.Result = 0;
            level.GetField(X, Y).SendMapFieldMessage(ref message);
            if ((WallType)message.Result.Int != WallType.None)
            {
                AddWall((WallType)message.Result.Int);
            }

            HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
            while (walk.Radius <= 1)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    armyHexSprite = (ArmyHexSprite)level.GetField(walk.X, walk.Y).FindOwnedChild(131607);
                    if (armyHexSprite != null)
                    {
                        side = CombatAbilityTables.GetCombatSide((sbyte)player, (sbyte)relation, (sbyte)defender, (sbyte)defenderRelation, armyHexSprite.Army.Player, armyHexSprite.Army.IndependentRelation);
                        if (side == CombatSideIndex.Defender)
                        {
                            field = level.GetField(X, Y);
                            message.MessageId = 4387;
                            var levelField = level.GetField(walk.X, walk.Y);
                            message.Param[0] = levelField;
                            message.Result = 0;
                            field.SendMapFieldMessage(ref message);
                            for (int I = 0, last = armyHexSprite.Army.Count - 1; I <= last; I++)
                            {
                                U = armyHexSprite.Army.GetUnit(I);
                                if (U.ValidTerrain(field.TerrainType, field.OverlayType))
                                {
                                    AddUnit(U, (byte)(int)side, (byte)((WallType)message.Result.Int != WallType.None ? 1 : 0));
                                }
                            }
                        }
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
    }

    // CombatPredictor.pas:835
    public int GetSuperiority()
    {
        CombatPredictorUnit U;
        int total;
        int left;
        int value;
        if (Superiority == -1)
        {
            if (Result == 3)
            {
                total = 0;
                left = 0;
                for (int I = Sides[0].Count - 1; I >= 0; I--)
                {
                    U = Sides[0].List[I];
                    value = U.CombatValueTotal;
                    total += value;
                    if (U.HitPoints > 0)
                    {
                        left += U.HitPoints * value / U.InitialHitPoints;
                    }
                }

                if (total == left)
                {
                    Superiority = 999;
                }
                else
                {
                    if (left > 0)
                    {
                        Superiority = total * 100 / (total - left);
                    }
                    else
                    {
                        Superiority = 100;
                    }
                }
            }
            else
            {
                if (Set<byte>.WithBounds(0, 255).With(2).With(4).With(8).Contains(Result))
                {
                    total = 0;
                    left = 0;
                    for (int I = Sides[1].Count - 1; I >= 0; I--)
                    {
                        U = Sides[1].List[I];
                        value = U.CombatValueTotal;
                        total += value;
                        if (U.HitPoints > 0)
                        {
                            left += U.HitPoints * value / U.InitialHitPoints;
                        }
                    }

                    if (total == 0)
                    {
                        Superiority = 0;
                    }
                    else
                    {
                        if (left > 0)
                        {
                            Superiority = (total - left) * 100 / total;
                        }
                        else
                        {
                            Superiority = 100;
                        }
                    }
                }
                else
                {
                    if (Result == 6)
                    {
                        Superiority = 200;
                    }
                    else
                    {
                        Superiority = 0;
                    }
                }
            }
        }

        return Superiority;
    }

    // CombatPredictor.pas:872
    public int GetEfficiency()
    {
        Int32Array2 lost = default;
        byte side;
        CombatPredictorUnit U;
        if (Efficiency == -1)
        {
            for (int sideOrdinal = 0; sideOrdinal <= 1; sideOrdinal++)
            {
                side = (byte)sideOrdinal;
                lost[side] = 0;
                for (int I = Sides[side].Count - 1; I >= 0; I--)
                {
                    U = Sides[side].List[I];
                    if (U.Status.Contains(CombatPredictorUnitStatusElement.Destroyed))
                    {
                        var obtainValue = U.Troop.GetObtainValue();
                        lost[side] += obtainValue;
                    }
                    else
                    {
                        if (U.HitPoints < U.InitialHitPoints)
                        {
                            var obtainValue = U.Troop.GetObtainValue();
                            lost[side] += Round((double)(obtainValue * U.HitPoints) / (U.InitialHitPoints * 2));
                        }
                    }
                }
            }

            if (lost[0] != 0)
            {
                Efficiency = lost[1] * 100 / lost[0];
            }
            else
            {
                if (lost[1] != 0 || Sides[1].Count == 0)
                {
                    Efficiency = 999;
                }
                else
                {
                    Efficiency = 100;
                }
            }
        }

        return Efficiency;
    }

    // CombatPredictor.pas:899
    public void ListRequiredUnits(UnitList required, UnitList optional, UnitList useless, int superiority)
    {
        DoubleItemList<CombatPredictorUnit> list = default;
        int total = default;
        int score = default;
        int budget = default;
        CombatPredictorUnit U = default;
        int maximum = default;
        int minimum = default;
        int count = default;
        int previous = default;
        bool done = default;
        EnableAllUnits();
        Validate();
        if (GetSuperiority() > superiority && Sides[0].Count != 0)
        {
            list = new DoubleItemList<CombatPredictorUnit>();
            total = 0;
            for (int I = Sides[0].Count - 1; I >= 0; I--)
            {
                U = Sides[0].List[I];
                if (U.IncomingCombatValue != 0)
                {
                    score = U.CombatValueTotal * 100 / U.IncomingCombatValue;
                }
                else
                {
                    score = 9999;
                }

                total += U.CombatValueTotal;
                list.AddAbility(U, score);
            }

            list.Sort(CompareRequiredUnits);
            minimum = 1;
            maximum = list.Count;
            if (superiority != 0 && GetSuperiority() != 0)
            {
                budget = Round((double)((double)((double)100 / superiority) - (double)100 / GetSuperiority()) * ((double)total * 0.65));
                count = list.Count;
                while (count > 1 && budget > 0)
                {
                    count -= 1;
                    budget -= list[count].First.CombatValueTotal;
                }

                if (count < minimum)
                {
                    count = minimum;
                }

                if (count >= maximum)
                {
                    count = maximum - 1;
                }
            }
            else
            {
                count = maximum - 1;
            }

            previous = maximum;
            done = list.Count <= 1;
            while (!done)
            {
                for (int I = count - 1; I >= 0; I--)
                {
                    list.List[I].First.SetEnabled(1);
                }

                for (int I = list.Count - 1; I >= count; I--)
                {
                    list.List[I].First.SetEnabled(0);
                }

                Execute();
                if (GetSuperiority() >= superiority)
                {
                    previous = count;
                    done = minimum == previous;
                    if (!done)
                    {
                        count -= 1;
                        if (count > minimum && GetSuperiority() - superiority > 300 / list.Count)
                        {
                            count -= 1;
                        }

                        if (count > minimum && GetSuperiority() - superiority > 600 / list.Count)
                        {
                            count -= 1;
                        }
                    }
                }
                else
                {
                    if (GetSuperiority() < superiority)
                    {
                        minimum = count + 1;
                        done = previous == minimum;
                        if (!done)
                        {
                            count += 1;
                            if (count + 1 != previous && superiority - GetSuperiority() > 300 / list.Count)
                            {
                                count += 1;
                            }

                            if (count + 1 != previous && superiority - GetSuperiority() > 600 / list.Count)
                            {
                                count += 1;
                            }
                        }
                    }
                }
            }

            if (required != null)
            {
                for (int I = 0; I <= minimum - 1; I++)
                {
                    required.AddUnit(list.List[I].First.Troop);
                }
            }

            if (optional != null)
            {
                for (int I = list.Count - 1; I >= minimum; I--)
                {
                    optional.AddUnit(list.List[I].First.Troop);
                }
            }

            if (useless != null)
            {
                for (int I = list.Count - 1; I >= minimum; I--)
                {
                    U = list.List[I].First;
                    if (U.CombatValueTotal <= 0)
                    {
                        useless.AddUnit(U.Troop);
                    }
                }
            }

            list?.Dispose();
        }
        else
        {
            for (int I = Sides[0].Count - 1; I >= 0; I--)
            {
                U = Sides[0].List[I];
                if (U.CombatValueTotal == 0)
                {
                    if (useless != null)
                    {
                        useless.AddUnit(U.Troop);
                    }
                }
                else
                {
                    if (required != null)
                    {
                        required.AddUnit(U.Troop);
                    }
                }
            }
        }
    }

    // CombatPredictor.pas:988
    public void ListRequiredUnits(UnitList required, int superiority)
    {
        ListRequiredUnits(required, null, null, superiority);
    }

    // CombatPredictor.pas:991
    public void ListOptionalUnits(UnitList optional, int superiority)
    {
        ListRequiredUnits(null, optional, null, superiority);
    }

    // CombatPredictor.pas:994
    public void ListUselessUnits(UnitList useless)
    {
        CombatPredictorUnit U;
        for (int I = Sides[0].Count - 1; I >= 0; I--)
        {
            U = Sides[0].List[I];
            if (U.CombatValueTotal <= 0)
            {
                useless.AddUnit(U.Troop);
            }
        }
    }

    // CombatPredictor.pas:1004
    public int Usefullness(Troop troop)
    {
        int result;
        CombatPredictorUnit wall;
        CombatPredictorUnit U;
        CombatPredictorUnit target;
        int I;
        bool allDead;
        bool useful;
        EnableAllUnits();
        Validate();
        U = new CombatPredictorUnit();
        U.Setup(troop, (CombatPredictor)(object)this, 0);
        wall = Wall;
        if (wall != null && wall.HitPoints > 0)
        {
            U.UpdateWallTargets();
            result = U.WallCombatValue;
            useful = false;
            I = U.WallTargets.Count;
            while (I != 0 && !useful)
            {
                I -= 1;
                if (U.WallTargets.List[I].Second > 0)
                {
                    target = U.WallTargets.List[I].First;
                    useful = target == Wall && target.HitPoints > 0;
                }
            }

            if (useful)
            {
                result = result | 8388608;
            }
        }
        else
        {
            U.UpdateTargets();
            result = U.CombatValue;
            I = Sides[0].Count;
            allDead = true;
            while (I != 0 && allDead)
            {
                I -= 1;
                allDead = Sides[0].GetUnits(I).Status.Contains(CombatPredictorUnitStatusElement.Destroyed);
            }

            useful = false;
            I = U.Targets.Count;
            while (I != 0 && !useful)
            {
                I -= 1;
                if (U.Targets.List[I].Second > 0)
                {
                    target = U.Targets.List[I].First;
                    useful = target != Wall && (target.HitPoints > 0 || Result == 3 || allDead);
                }
            }

            if (useful)
            {
                result = result | 8388608;
            }
        }

        U?.Dispose();
        return result;
    }

    private static int CompareRequiredUnits(in DoubleItem<CombatPredictorUnit> A, in DoubleItem<CombatPredictorUnit> B)
    {
        return B.Second - A.Second;
    }

    // intf_P3_01.inc:92

    // intf_P3_01.inc:92

}
