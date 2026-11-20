#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Collections;

using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Combat;

// AllUnits. ActiveUnits. Sides. Result. Superiority. Efficiency. Wall. Round.
// NextCpuId. Invalid. Work
public class CombatPredictor : IDisposable
{
    // CombatPredictor.pas:292
    public CombatPredictor(EngineObject creator)
    {
        Creator = creator;
        _allUnits = new CombatPredictorUnitList();
        _activeUnits = new CombatPredictorUnitList();
        CombatPredictorSide combatPredictorSide = new();
        Sides[0] = combatPredictorSide;
        CombatPredictorSide combatPredictorSide2 = new();
        Sides[1] = combatPredictorSide2;
    }

    // The node that created this predictor. It and the nodes it creates reach the engine through it.
    public EngineObject Creator { get; }

    private readonly CombatPredictorUnitList _allUnits;
    private readonly CombatPredictorUnitList _activeUnits;

    public CombatPredictorSide[] Sides { get; } = new CombatPredictorSide[2];

    public byte Result { get; private set; }

    private int _superiority;
    private int _efficiency;
    private CombatPredictorUnit _wall;
    private int _round;
    private int _nextCpuId;

    public byte Invalid { get; set; }
    public int Work { get; set; }

    // CombatPredictor.pas:301
    public virtual void Dispose()
    {
        Reset();
        _allUnits?.Dispose();
        _activeUnits?.Dispose();
        Sides[0]?.Dispose();
        Sides[1]?.Dispose();
    }

    // CombatPredictor.pas:311
    public void Reset()
    {
        for (int i = _allUnits.Count - 1; i >= 0; i--)
        {
            (_allUnits.List[i] as IDisposable)?.Dispose();
        }

        _wall = null;
        _allUnits.Count = 0;
        _activeUnits.Count = 0;
        Sides[0].Count = 0;
        Sides[1].Count = 0;
        _superiority = -1;
        _efficiency = -1;
        _nextCpuId = 0;
    }

    // CombatPredictor.pas:325
    public int CreateUniqueCpuId()
    {
        int result = _nextCpuId;
        _nextCpuId += 1;
        return result;
    }

    // CombatPredictor.pas:328
    public void EnableAllUnits()
    {
        for (int i = _allUnits.Count - 1; i >= 0; i--)
        {
            if (_allUnits.List[i].Enabled == 0)
            {
                _allUnits.List[i].Enabled = 1;
                Invalid = 1;
            }
        }
    }

    // CombatPredictor.pas:339
    public void Validate()
    {
        if (Invalid != 0)
        {
            _ = Execute();
        }
    }

    // CombatPredictor.pas:342
    public void ResetCombat()
    {
        Invalid = 1;
        _superiority = -1;
        _efficiency = -1;
        _round = 0;
        for (int i = _allUnits.Count - 1; i >= 0; i--)
        {
            _allUnits.List[i].ResetCombat();
        }
    }

    // CombatPredictor.pas:352
    public void SetupCombat(byte wallPhase)
    {
        _activeUnits.Clear();
        if (Sides[0].UnitRemovedFlags[0] != 0)
        {
            for (int i = Sides[1].Count - 1; i >= 0; i--)
            {
                Sides[1].List[i].InvalidateTargets();
            }

            Sides[0].UnitRemovedFlags[0] = 0;
        }

        if (Sides[1].UnitRemovedFlags[0] != 0)
        {
            for (int i = Sides[0].Count - 1; i >= 0; i--)
            {
                Sides[0].List[i].InvalidateTargets();
            }

            Sides[1].UnitRemovedFlags[0] = 0;
        }

        for (int i = _allUnits.Count - 1; i >= 0; i--)
        {
            _allUnits.List[i].ResetRoundTargets();
        }

        for (int i = _allUnits.Count - 1; i >= 0; i--)
        {
            _allUnits.List[i].SetupRoundTargets(wallPhase);
        }

        for (int i = _allUnits.Count - 1; i >= 0; i--)
        {
            _allUnits.List[i].UpdateRoundTargetPriority();
        }

        for (int i = _allUnits.Count - 1; i >= 0; i--)
        {
            CombatPredictorUnit u = _allUnits.List[i];
            if (u.Enabled != 0 && u.RoundTargets.Count != 0)
            {
                u.SortRoundTargets();
                _ = _activeUnits.Add(u);
            }
        }
    }

    // CombatPredictor.pas:380
    public bool ExecuteCombatRound()
    {
        CombatPredictorUnit u;
        bool result = false;
        for (int i = _activeUnits.Count - 1; i >= 0; i--)
        {
            u = _activeUnits.List[i];
            if (!u.ExecuteRound())
            {
                _activeUnits.Delete(i);
            }
            else
            {
                result = true;
            }
        }

        for (int i = _activeUnits.Count - 1; i >= 0; i--)
        {
            u = _activeUnits.List[i];
            if (u.HitPoints <= 0)
            {
                _activeUnits.Delete(i);
            }
        }

        if (result)
        {
            _round += 1;
        }

        return result;
    }

    // CombatPredictor.pas:398
    public bool CanPassWall()
    {
        bool result;
        try
        {
            result = false;
            int i = _allUnits.Count;
            while (!result && i != 0)
            {
                i -= 1;
                if (_allUnits.GetUnits(i).Troop is not null)
                {
                    result = _allUnits.GetUnits(i).Troop.GetWallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing);
                }
            }
        }
        catch
        {
            return false;
        }

        return result;
    }

    // CombatPredictor.pas:415
    public void FinishCombat()
    {
        Span<int> surviving = stackalloc int[2];
        Span<int> hits = stackalloc int[2];
        for (int i = _allUnits.Count - 1; i >= 0; i--)
        {
            CombatPredictorUnit u = _allUnits.List[i];
            u.FinishCombat();
            if (!u.Status.Contains(CombatPredictorUnitStatusElement.Destroyed))
            {
                surviving[u.Side] += 1;
            }

            hits[u.Side] += u.HitPoints;
        }

        if (surviving[1] == 0 && surviving[0] == 0)
        {
            Result = hits[0] >= hits[1] ? (byte)3 : (byte)4;
        }
        else
        {
            if (surviving[1] == 0)
            {
                Result = 3;
            }
            else
            {
                CombatPredictorUnit wall = _wall;
                Result = wall is not null && wall.HitPoints > 0
                    ? surviving[1] == 1 && CanPassWall() ? (byte)3 : (byte)8
                    : surviving[0] == 0 ? (byte)4 : (byte)2;
            }
        }

        _activeUnits.Clear();
    }

    // CombatPredictor.pas:440
    public CombatResult Execute()
    {
        if (_wall is not null)
        {
            ResetCombat();
            SetupCombat(1);
            while (_wall.HitPoints > 0 && ExecuteCombatRound())
            {
            }

            if (_wall.HitPoints <= 0)
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

        StrategicMap.Current.AIExecutor.UseProcessTime(Work * 58);
        CombatResult result = (CombatResult)Result;
        Invalid = 0;
        return result;
    }

    // CombatPredictor.pas:466
    public void DeleteUnit(Troop troop)
    {
        int i = _allUnits.IndexOfUnit(troop);
        if (i != -1)
        {
            CombatPredictorUnit u = _allUnits.GetUnits(i);
            int j = Sides[u.Side].IndexOfUnit(troop);
            if (j != -1)
            {
                if (Sides[u.Side].GetUnits(j) == _wall)
                {
                    _wall = null;
                }

                Sides[u.Side].Delete(j);
                Sides[u.Side].UnitRemovedFlags[0] = 1;
                // { Original adds strength when deleting a unit. }
                for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
                {
                    DamageType d = (DamageType)dOrdinal;
                    int strength = troop.GetStrength(d);
                    Sides[u.Side].Strength[(int)d] += strength;
                }
            }

            Invalid = 1;
            _allUnits.Delete(i);
            u?.Dispose();
        }
    }

    // CombatPredictor.pas:489
    public CombatPredictorUnit AddUnit(Troop troop, byte side, byte behindWall)
    {
        CombatPredictorUnit u = new();
        u.Setup(troop, predictor: (CombatPredictor)(object)this, side);
        u.BehindWall = behindWall;
        _ = _allUnits.Add(u);
        _ = Sides[side].Add(u);
        Sides[side].UnitRemovedFlags[0] = 1;
        for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
        {
            DamageType d = (DamageType)dOrdinal;
            int strength = troop.GetStrength(d);
            Sides[side].Strength[(int)d] += strength;
        }

        Invalid = 1;
        return u;
    }

    // CombatPredictor.pas:504
    public void AddUnit(Troop troop) => AddUnit(troop, side: 0, behindWall: 0);

    // CombatPredictor.pas:507
    public void AddUnits(UnitCollection units)
    {
        for (int i = 0, last = units.Count - 1; i <= last; i++)
        {
            AddUnit(units.GetUnit(i));
        }
    }

    // CombatPredictor.pas:514
    public void AddOpponent(Troop troop, byte behindWall) => AddUnit(troop, side: 1, behindWall);

    // CombatPredictor.pas:517
    public void AddOpponents(UnitList units, byte behindWall)
    {
        for (int i = 0, last = units.Count - 1; i <= last; i++)
        {
            AddOpponent(units.GetUnit(i), behindWall);
        }
    }

    // CombatPredictor.pas:524
    public void AddWall(WallType wallType)
    {
        if (wallType == WallType.None)
        {
            // { Preserve the original assertion diagnostic. }
            Assert(false, message: "Use DeleteWall instead!", file: "D:\\AoWDev\\AoWE\\inc\\CombatPredictor.pas", line: 18391);
        }

        CombatPredictorUnit unitObject = _wall;
        if (unitObject is null)
        {
            WallCombatant wall = new(null) { Holder = Creator.CreationHolder() };
            wall.SetWallType(wallType);
            wall.Hits = (sbyte)(wall.Hits + 5);
            wall.HitPoints = (sbyte)(wall.HitPoints + 5);
            _wall = AddUnit(wall, side: 1, behindWall: 0);
            _ = wall.Release();
        }
        else
        {
            // AddWall is the only writer of a non-null _wall, and it always wraps a WallCombatant it created.
            ((WallCombatant)unitObject.Troop).SetWallType(wallType);
        }
    }

    // CombatPredictor.pas:714
    public void DeleteWall()
    {
        CombatPredictorUnit wall = _wall;
        if (wall is not null)
        {
            DeleteUnit(wall.Troop);
            _wall = null;
        }
    }

    // CombatPredictor.pas:725
    public void DeleteUnits()
    {
        for (int i = Sides[0].Count - 1; i >= 0; i--)
        {
            DeleteUnit(Sides[0].GetUnits(i).Troop);
        }
    }

    // CombatPredictor.pas:732
    public void DeleteOpponents()
    {
        for (int i = Sides[1].Count - 1; i >= 0; i--)
        {
            DeleteUnit(Sides[1].GetUnits(i).Troop);
        }
    }

    // CombatPredictor.pas:739
    public void InitializeAdjacentArmies(ArmyHexSprite army, sbyte X, sbyte Y, sbyte L)
    {
        MapFieldMessage message = default;
        AddUnits(army.Army.Units);
        MapLevel level = StrategicMap.Current.MapContainer.GetMapLevel(L);
        Army armyLocal = army.Army;
        byte player = (byte)armyLocal.Player;
        byte relation = (byte)armyLocal.IndependentRelation;

        byte defender = (byte)level.GetField(X, Y).GetPlayer();
        byte defenderRelation = level.GetField(X, Y).GetIndependentRelation();
        if (defender != 255)
        {
            message.MessageId = 4387;
            MapField fieldValue = level.GetField(X, Y);
            MapFieldMessageParameters param = message.Param;
            param[0] = fieldValue;
            message.Param = param;
            message.Result = 0;
            level.GetField(X, Y).SendMapFieldMessage(ref message);
            if ((WallType)message.Result.Int != WallType.None)
            {
                AddWall((WallType)message.Result.Int);
            }

            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
            while (walk.Radius <= 1)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    ArmyHexSprite armyHexSprite = level.GetField(walk.X, walk.Y).GetArmyHexSprite();
                    if (armyHexSprite is not null && armyHexSprite != army)
                    {
                        CombatSideIndex side = CombatAbilityTables.GetCombatSide((sbyte)player, attackerRelation: (sbyte)relation, defender: (sbyte)defender, defenderRelation: (sbyte)defenderRelation, armyHexSprite.Army.Player, relation: armyHexSprite.Army.IndependentRelation);
                        if (side != CombatSideIndex.Neutral)
                        {
                            MapField field = level.GetField(X, Y);
                            message.MessageId = 4387;
                            MapField levelField = level.GetField(walk.X, walk.Y);
                            MapFieldMessageParameters param2 = message.Param;
                            param2[0] = levelField;
                            message.Param = param2;
                            message.Result = 0;
                            field.SendMapFieldMessage(ref message);
                            for (int i = 0, last = armyHexSprite.Army.Units.Count - 1; i <= last; i++)
                            {
                                Troop u = armyHexSprite.Army.Units.GetUnit(i);
                                if (u.ValidTerrain(field.TerrainType, field.OverlayType))
                                {
                                    _ = AddUnit(u, side: (byte)(int)side, behindWall: (byte)((WallType)message.Result.Int != WallType.None ? 1 : 0));
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
        MapFieldMessage message = default;
        MapLevel level = StrategicMap.Current.MapContainer.GetMapLevel(L);
        byte defender = (byte)level.GetField(X, Y).GetPlayer();
        byte defenderRelation = level.GetField(X, Y).GetIndependentRelation();
        if (defender != 255)
        {
            message.MessageId = 4387;
            MapField fieldValue = level.GetField(X, Y);
            MapFieldMessageParameters param = message.Param;
            param[0] = fieldValue;
            message.Param = param;
            message.Result = 0;
            level.GetField(X, Y).SendMapFieldMessage(ref message);
            if ((WallType)message.Result.Int != WallType.None)
            {
                AddWall((WallType)message.Result.Int);
            }

            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
            while (walk.Radius <= 1)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    ArmyHexSprite armyHexSprite = level.GetField(walk.X, walk.Y).GetArmyHexSprite();
                    if (armyHexSprite is not null)
                    {
                        CombatSideIndex side = CombatAbilityTables.GetCombatSide((sbyte)player, attackerRelation: (sbyte)relation, defender: (sbyte)defender, defenderRelation: (sbyte)defenderRelation, armyHexSprite.Army.Player, relation: armyHexSprite.Army.IndependentRelation);
                        if (side == CombatSideIndex.Defender)
                        {
                            MapField field = level.GetField(X, Y);
                            message.MessageId = 4387;
                            MapField levelField = level.GetField(walk.X, walk.Y);
                            MapFieldMessageParameters param2 = message.Param;
                            param2[0] = levelField;
                            message.Param = param2;
                            message.Result = 0;
                            field.SendMapFieldMessage(ref message);
                            for (int i = 0, last = armyHexSprite.Army.Units.Count - 1; i <= last; i++)
                            {
                                Troop u = armyHexSprite.Army.Units.GetUnit(i);
                                if (u.ValidTerrain(field.TerrainType, field.OverlayType))
                                {
                                    _ = AddUnit(u, side: (byte)(int)side, behindWall: (byte)((WallType)message.Result.Int != WallType.None ? 1 : 0));
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
        if (_superiority == -1)
        {
            CombatPredictorUnit u;
            int total;
            int left;
            int value;
            if (Result == 3)
            {
                total = 0;
                left = 0;
                for (int i = Sides[0].Count - 1; i >= 0; i--)
                {
                    u = Sides[0].List[i];
                    value = u.CombatValueTotal;
                    total += value;
                    if (u.HitPoints > 0)
                    {
                        left += u.HitPoints * value / u.InitialHitPoints;
                    }
                }

                _superiority = total == left ? 999 : left > 0 ? total * 100 / (total - left) : 100;
            }
            else
            {
                if (Set<byte>.WithBounds(0, 255).With(2).With(4).With(8).Contains(Result))
                {
                    total = 0;
                    left = 0;
                    for (int i = Sides[1].Count - 1; i >= 0; i--)
                    {
                        u = Sides[1].List[i];
                        value = u.CombatValueTotal;
                        total += value;
                        if (u.HitPoints > 0)
                        {
                            left += u.HitPoints * value / u.InitialHitPoints;
                        }
                    }

                    _superiority = total == 0 ? 0 : left > 0 ? (total - left) * 100 / total : 100;
                }
                else
                {
                    _superiority = Result == 6 ? 200 : 0;
                }
            }
        }

        return _superiority;
    }

    // CombatPredictor.pas:872
    public int GetEfficiency()
    {
        Span<int> lost = stackalloc int[2];
        if (_efficiency == -1)
        {
            for (int sideOrdinal = 0; sideOrdinal <= 1; sideOrdinal++)
            {
                byte side = (byte)sideOrdinal;
                lost[side] = 0;
                for (int i = Sides[side].Count - 1; i >= 0; i--)
                {
                    CombatPredictorUnit u = Sides[side].List[i];
                    if (u.Status.Contains(CombatPredictorUnitStatusElement.Destroyed))
                    {
                        int obtainValue = u.Troop.GetObtainValue();
                        lost[side] += obtainValue;
                    }
                    else
                    {
                        if (u.HitPoints < u.InitialHitPoints)
                        {
                            int obtainValue = u.Troop.GetObtainValue();
                            lost[side] += Round((double)(obtainValue * u.HitPoints) / (u.InitialHitPoints * 2));
                        }
                    }
                }
            }

            _efficiency = lost[0] != 0 ? lost[1] * 100 / lost[0] : lost[1] != 0 || Sides[1].Count == 0 ? 999 : 100;
        }

        return _efficiency;
    }

    // CombatPredictor.pas:899
    public void ListRequiredUnits(UnitUserList required, UnitUserList optional, UnitUserList useless, int superiority)
    {
        EnableAllUnits();
        Validate();
        CombatPredictorUnit u;
        if (GetSuperiority() > superiority && Sides[0].Count != 0)
        {
            DoubleItemList<CombatPredictorUnit> list = new();
            int total = 0;
            for (int i = Sides[0].Count - 1; i >= 0; i--)
            {
                u = Sides[0].List[i];
                int score = u.IncomingCombatValue != 0 ? u.CombatValueTotal * 100 / u.IncomingCombatValue : 9999;

                total += u.CombatValueTotal;
                _ = list.AddAbility(u, score);
            }

            list.Sort(CompareRequiredUnits);
            int minimum = 1;
            int maximum = list.Count;
            int count;
            if (superiority != 0 && GetSuperiority() != 0)
            {
                int budget = Round((double)((double)((double)100 / superiority) - ((double)100 / GetSuperiority())) * (total * 0.65));
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

            int previous = maximum;
            bool done = list.Count <= 1;
            while (!done)
            {
                for (int i = count - 1; i >= 0; i--)
                {
                    list.List[i].First.SetEnabled(1);
                }

                for (int i = list.Count - 1; i >= count; i--)
                {
                    list.List[i].First.SetEnabled(0);
                }

                _ = Execute();
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

            if (required is not null)
            {
                for (int i = 0; i <= minimum - 1; i++)
                {
                    _ = required.AddUnit(list.List[i].First.Troop);
                }
            }

            if (optional is not null)
            {
                for (int i = list.Count - 1; i >= minimum; i--)
                {
                    _ = optional.AddUnit(list.List[i].First.Troop);
                }
            }

            if (useless is not null)
            {
                for (int i = list.Count - 1; i >= minimum; i--)
                {
                    u = list.List[i].First;
                    if (u.CombatValueTotal <= 0)
                    {
                        _ = useless.AddUnit(u.Troop);
                    }
                }
            }

            list?.Dispose();
        }
        else
        {
            for (int i = Sides[0].Count - 1; i >= 0; i--)
            {
                u = Sides[0].List[i];
                _ = u.CombatValueTotal == 0 ? (useless?.AddUnit(u.Troop)) : (required?.AddUnit(u.Troop));
            }
        }
    }

    // CombatPredictor.pas:988
    public void ListRequiredUnits(UnitUserList required, int superiority) => ListRequiredUnits(required, optional: null, useless: null, superiority);

    // CombatPredictor.pas:991
    public void ListOptionalUnits(UnitUserList optional, int superiority) => ListRequiredUnits(null, optional, useless: null, superiority);

    // CombatPredictor.pas:994
    public void ListUselessUnits(UnitList useless)
    {
        for (int i = Sides[0].Count - 1; i >= 0; i--)
        {
            CombatPredictorUnit u = Sides[0].List[i];
            if (u.CombatValueTotal <= 0)
            {
                _ = useless.AddUnit(u.Troop);
            }
        }
    }

    // CombatPredictor.pas:1004
    public int Usefullness(Troop troop)
    {
        EnableAllUnits();
        Validate();
        CombatPredictorUnit u = new();
        u.Setup(troop, predictor: (CombatPredictor)(object)this, side: 0);
        CombatPredictorUnit wall = _wall;
        int result;
        CombatPredictorUnit target;
        int i;
        bool useful;
        if (wall is not null && wall.HitPoints > 0)
        {
            u.UpdateWallTargets();
            result = u.WallCombatValue;
            useful = false;
            i = u.WallTargets.Count;
            while (i != 0 && !useful)
            {
                i -= 1;
                if (u.WallTargets.List[i].Second > 0)
                {
                    target = u.WallTargets.List[i].First;
                    useful = target == _wall && target.HitPoints > 0;
                }
            }

            if (useful)
            {
                result |= 8388608;
            }
        }
        else
        {
            u.UpdateTargets();
            result = u.CombatValue;
            i = Sides[0].Count;
            bool allDead = true;
            while (i != 0 && allDead)
            {
                i -= 1;
                allDead = Sides[0].GetUnits(i).Status.Contains(CombatPredictorUnitStatusElement.Destroyed);
            }

            useful = false;
            i = u.Targets.Count;
            while (i != 0 && !useful)
            {
                i -= 1;
                if (u.Targets.List[i].Second > 0)
                {
                    target = u.Targets.List[i].First;
                    useful = target != _wall && (target.HitPoints > 0 || Result == 3 || allDead);
                }
            }

            if (useful)
            {
                result |= 8388608;
            }
        }

        u?.Dispose();
        return result;
    }

    private static int CompareRequiredUnits(in DoubleItem<CombatPredictorUnit> A, in DoubleItem<CombatPredictorUnit> B) => B.Second - A.Second;
}
