#nullable disable warnings
// Translated from AoWTC.pas.
using System.Globalization;
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.PCharStrings;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.Combat;

// Side. Units. Allies. Enemies. Moves. Walls. CurrentMove. UnitIndex. UnitLimit. MinMoves.
// HeroMoves. NormalUnits. IgnoreWalls. Flee. Status
public sealed class BattleAI(EngineObject creator) : IDisposable
{
    private CombatSideIndex _side;

    // The collect step admits only tactical battle units, so the element casts never fail.
    private readonly List<TacticalBattleUnit> _units = [];
    private readonly List<TacticalBattleUnit> _allies = [];
    private readonly List<TacticalBattleUnit> _enemies = [];
    private readonly List<BattleAIMove> _moves = [];
    private readonly List<CityWall> _walls = [];
    private int _currentMove;
    private int _unitIndex;
    private int _unitLimit;
    private int _minMoves;
    private int _heroMoves;
    private int _normalUnits;
    private bool _ignoreWalls;
    private bool _flee;

    public AIStatus Status { get; set; }

    // AoWTC.pas:762
    public void Dispose()
    {
        _units?.Clear();
        _allies?.Clear();
        _enemies?.Clear();
        _walls?.Clear();
        for (int i = 0, last = _moves.Count - 1; i <= last; i++)
        {
            (_moves[i] as IDisposable)?.Dispose();
        }

        _moves?.Clear();
    }

    // The node that created this battle AI. It and the nodes it creates reach the engine through it.
    public EngineObject Creator { get; } = creator;

    // AoWTC.pas:774
    public void Reset()
    {
        Status = AIStatus.Idle;
        _currentMove = 0;
    }

    // AoWTC.pas:780
    public void BeginTurn() => EvalBattle();

    // AoWTC.pas:785
    public void ContinueTurn() => EvalBattle();

    // AoWTC.pas:790
    public void CreateMove(BattleAIMoveType moveType, int x, int y, int value, int ability, int spell, TacticalBattleUnit unit, CombatObject? target)
    {
        BattleAIMove m = new()
        {
            UnitHexSprite = unit.UnitHexSprite,
            Target = target,
            MoveType = moveType,
            Ability = (short)ability,
            Spell = (short)spell,
            X = x.TruncateToUInt16(),
            Y = y.TruncateToUInt16(),
            Value = value
        };
        _moves.Add(m);
    }

    // AoWTC.pas:805
    public int TacticalBattleDamageValueToDamageEvaluation(int damageValue1, int damageValue2, CombatObject source, CombatObject? target)
    {
        int strength = target.GetTargetStrength();
        if (target is TacticalBattleUnit targetUnit)
        {
            if (targetUnit.Troop is Hero)
            {
                strength *= 2;
            }

            if (targetUnit.Troop is Leader)
            {
                strength *= 2;
            }
        }

        int result = strength * damageValue1;
        strength = source.GetTargetStrength();
        if (source is TacticalBattleUnit sourceUnit)
        {
            if (sourceUnit.Troop is Hero)
            {
                strength *= 2;
            }

            if (sourceUnit.Troop is Leader)
            {
                strength *= 2;
            }
        }

        result -= strength * damageValue2;
        return result / 100;
    }

    // AoWTC.pas:823
    public bool ChargeFromPosition(int position)
    {
        bool result;
        if (position == 0)
        {
            result = false;
            if (_side == CombatSideIndex.Defender && CombatMap.Current.Combat.CombatData.WallType != 0 && !CombatMap.Current.Combat.CombatData.WallBreached)
            {
                return true;
            }
            else
            {
                int empty = 0;
                for (int i = 1; i <= 6; i++)
                {
                    if (CombatMap.Current.PositionStatus[i] == 1)
                    {
                        int j = i - 1;
                        if (j == 0)
                        {
                            j = 6;
                        }

                        if (CombatMap.Current.PositionStatus[j] == 0)
                        {
                            result = true;
                        }

                        j = i + 1;
                        if (j == 7)
                        {
                            j = 1;
                        }

                        if (CombatMap.Current.PositionStatus[j] == 0)
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        if (CombatMap.Current.PositionStatus[i] == 0)
                        {
                            empty += 1;
                        }
                    }
                }

                if (empty > 1)
                {
                    return true;
                }
            }
        }
        else
        {
            return true;
        }

        return result;
    }

    // AoWTC.pas:849
    public int UnitSafe(TacticalBattleUnit unit, int x, int y)
    {
        int result;
        if (unit.Troop is not Hero || (_units.Count == 1 && _side == CombatSideIndex.Defender))
        {
            return 0;
        }
        else
        {
            result = unit.GetTargetStrength() * 20 * unit.HitPoints / unit.GetHits();
            if (unit.Troop is Leader)
            {
                result /= 2;
            }

            int distance;
            for (int i = 0, last = _enemies.Count - 1; i <= last; i++)
            {
                int hexY = _enemies[i].UnitHexSprite.HexY;
                distance = HexGeometry.DeltaHexToRadius(x, y, _enemies[i].UnitHexSprite.HexX, hexY);
                if (distance < 15 && _enemies[i].GetTargetStrength() > 0)
                {
                    result -= _enemies[i].GetTargetStrength() * (15 - distance);
                }
            }

            for (int i = 0, last = _units.Count - 1; i <= last; i++)
            {
                if (_units[i] != unit)
                {
                    int hexY = _units[i].UnitHexSprite.HexY;
                    distance = HexGeometry.DeltaHexToRadius(x, y, _units[i].UnitHexSprite.HexX, hexY);
                    if (distance < 15 && _units[i].GetTargetStrength() > 0)
                    {
                        result += _units[i].GetTargetStrength() * (15 - distance);
                    }
                }
            }

            for (int i = 0, last = _allies.Count - 1; i <= last; i++)
            {
                int hexY = _allies[i].UnitHexSprite.HexY;
                distance = HexGeometry.DeltaHexToRadius(x, y, _allies[i].UnitHexSprite.HexX, hexY);
                if (distance < 15 && _allies[i].GetTargetStrength() > 0)
                {
                    result += _allies[i].GetTargetStrength() * (15 - distance);
                }
            }
        }

        return result;
    }

    // AoWTC.pas:880
    // The tactical AI passes the wall as Ignore. The original casts that object to TTacticalCombatUnit
    // without a check and compares the reference with the sprite's unit. Ignore is a combat object so the
    // wall reaches the same comparison.
    public int MakeTerrainPath(CombatMoveControlSettings moveControlSettings, MovePath path, ref int value, TacticalBattleUnit unit, CombatObject? ignore, bool freeStrikes, bool findWalls, int cost)
    {
        int result = path.Count < 2 ? -1 : 0;

        try
        {
            value = 0;
            if (result != -1)
            {
                int x = default;
                int y = default;
                for (int i = 0, last = path.Count - 1; i <= last; i++)
                {
                    TacticalBattleUnitHexSprite hexSprite = CombatMap.Current.MapContainer.MapLevels[0].Field[path.List[i].Location.X, path.List[i].Location.Y].FindOwnedChild<TacticalBattleUnitHexSprite>(TacticalBattleUnitHexSprite.ClassId);
                    int j;
                    if (freeStrikes && (hexSprite is null || (hexSprite == unit.UnitHexSprite && i - 1 >= 0 && CombatMap.Current.MapContainer.MapLevels[0].Field[path.List[i - 1].Location.X, path.List[i - 1].Location.Y].FindOwnedChild(TacticalBattleUnitHexSprite.ClassId) is null)))
                    {
                        j = 1;
                        for (; j <= 6; j += 1)
                        {
                            HexGeometry.CenterHexNumberToHex(path.List[i].Location.X, path.List[i].Location.Y, hexNumber: j, ref x, ref y);
                            hexSprite = CombatMap.Current.MapContainer.MapLevels[0].Field[x, y].FindOwnedChild<TacticalBattleUnitHexSprite>(TacticalBattleUnitHexSprite.ClassId);
                            if (hexSprite is not null && hexSprite.Unit != ignore && hexSprite.Unit.Side != unit.Side && CombatObject.MeleeDamageTypes(hexSprite.Unit) != DamageTypes.None && !hexSprite.Unit.GetLocked() && hexSprite.Unit.GetAbilityEnabled(1) == unit.GetAbilityEnabled(1))
                            {
                                int tacticalBattleDamageValueToDamageEvaluation2 = TacticalBattleDamageValueToDamageEvaluation(CombatObject.StrikeDamageValue(hexSprite.Unit, target: unit), 0, source: hexSprite.Unit, target: unit);
                                value -= tacticalBattleDamageValueToDamageEvaluation2 * 10;
                            }
                        }
                    }

                    if (i < path.Count - 1)
                    {
                        CityWall? wall;
                        if (CombatMap.Current.Combat.CombatData.WallType != 0)
                        {
                            PathStep pathY = path[i];
                            PathStep pathX = path[i];
                            wall = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y].FindOwnedChild<CityWall>(CityWall.ClassId);
                            if (wall is null)
                            {
                                PathStep pathLocal = path[i];
                                PathStep path2 = path[i];
                                wall = CombatMap.Current.MapContainer.MapLevels[0].Field[path2.Location.X, pathLocal.Location.Y].FindOwnedChild(2_229_510) as CityWall;
                            }
                        }
                        else
                        {
                            wall = null;
                        }

                        PathStep path3 = path[i];
                        PathStep path4 = path[i];
                        MapField mapField = CombatMap.Current.MapContainer.MapLevels[0].Field[path4.Location.X, path3.Location.Y];
                        PathStep path5 = path[i];
                        PathStep path6 = path[i];
                        MapField fieldItem = CombatMap.Current.MapContainer.MapLevels[0].Field[path6.Location.X, path5.Location.Y];
                        path.List[i] = path.List[i] with { MoveCost = moveControlSettings.MovePointTable[fieldItem.TerrainType][mapField.OverlayType - MovementTables.FirstOverlay] };
                        if (wall is not null)
                        {
                            // { The wall lookup uses the current path point and the no-overlay column. }
                            if (_side == CombatSideIndex.Attacker && (unit.GetAbilityEnabled(65) || unit.GetAbilityEnabled(117)) && i == 0)
                            {
                                PathStep pathY = path[i];
                                PathStep pathX = path[i];
                                MapField item = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y];
                                path.List[i] = path.List[i] with { MoveCost = moveControlSettings.MovePointTable[item.TerrainType][-1 - MovementTables.FirstOverlay] };
                            }
                            else
                            {
                                if ((_side == CombatSideIndex.Attacker || CombatMap.Current.Combat.CombatData.WallBreached || _ignoreWalls) && (wall.HitPoints == 0 || (_side == CombatSideIndex.Defender && wall is CityWallDoor) || CombatMap.Current.MoveControl.UnitHexSprite.Unit.GetAbilityEnabled(1) || CombatMap.Current.MoveControl.UnitHexSprite.Unit.GetAbilityEnabled(66) || CombatMap.Current.MoveControl.UnitHexSprite.Unit.GetAbilityEnabled(36)))
                                {
                                    PathStep pathY = path[i];
                                    PathStep pathX = path[i];
                                    MapField item2 = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y];
                                    path.List[i] = path.List[i] with { MoveCost = moveControlSettings.MovePointTable[item2.TerrainType][-1 - MovementTables.FirstOverlay] };
                                }
                                else
                                {
                                    path.List[i] = path.List[i] with { MoveCost = 100 };
                                }
                            }

                            if (findWalls && wall.HitPoints > 0)
                            {
                                bool newWall = true;
                                j = 0;
                                while (newWall && j < _walls.Count)
                                {
                                    if (_walls[j] == wall)
                                    {
                                        newWall = false;
                                    }
                                    else
                                    {
                                        j += 1;
                                    }
                                }

                                if (newWall)
                                {
                                    _walls.Add(wall);
                                }
                            }
                        }

                        PathStep path7 = path[i];
                        PathStep path8 = path[i];
                        if (CombatMap.Current.MapContainer.MapLevels[0].Field[path8.Location.X, path7.Location.Y].FindOwnedChild(TacticalBattleUnitHexSprite.ClassId) is not null)
                        {
                            PathStep pathY = path[i];
                            PathStep pathX = path[i];
                            MapField item2 = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y];
                            path.List[i] = path.List[i] with { MoveCost = moveControlSettings.MovePointTable[item2.TerrainType][-1 - MovementTables.FirstOverlay] };
                        }
                    }

                    result += path.List[i].MoveCost;
                }
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Error during Eval Movepath", caption: NullTerminatedText($"Error - TCAI {unit.Troop.GetName()}"), flags: 0);
        }

        return result;
    }

    // AoWTC.pas:952
    public int ScaleDamageEvaluation(int value, int multiplier, int divider)
    {
        int result = value;
        if (value >= 0)
        {
            try
            {
                if (multiplier != 1)
                {
                    result *= multiplier;
                }

                if (divider is not 1 and not 0)
                {
                    result /= divider;
                }
            }
            catch
            {
                try
                {
                    if (divider is not 1 and not 0)
                    {
                        result /= divider;
                    }

                    if (multiplier != 1)
                    {
                        result *= multiplier;
                    }
                }
                catch
                {
                    result = 1_000_000;
                }
            }
        }
        else
        {
            try
            {
                if (divider != 1)
                {
                    result *= divider;
                }

                if (multiplier is not 1 and not 0)
                {
                    result /= multiplier;
                }
            }
            catch
            {
                try
                {
                    if (multiplier is not 1 and not 0)
                    {
                        result /= multiplier;
                    }

                    if (divider != 1)
                    {
                        result *= divider;
                    }
                }
                catch
                {
                    result = -1_000_000;
                }
            }
        }

        if (result > 1_000_000)
        {
            result = 1_000_000;
        }

        return result < -1_000_000 ? -1_000_000 : result;
    }

    // AoWTC.pas:980
    public void EvalAll(ref int value, List<TacticalBattleUnit> list, CombatObject ignore, int spell, TacticalBattleUnit U)
    {
        AbilityDamageValues damageValue = default;
        for (int i = 0, last = list.Count - 1; i <= last; i++)
        {
            if (list[i] != ignore)
            {
                CombatObject target = list[i];
                GamePack.Current.SpellControl[spell].GetCombatDamageValues(U, target, values: ref damageValue);
                if (damageValue.SourceDamage > 0)
                {
                    switch (spell)
                    {
                        case 102:
                            if (target.Side != _side)
                            {
                                int tacticalBattleDamageValueToDamageEvaluation2 = TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, source: U, target);
                                value -= tacticalBattleDamageValueToDamageEvaluation2;
                            }
                            else
                            {
                                int tacticalBattleDamageValueToDamageEvaluation2 = TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, source: U, target);
                                value += tacticalBattleDamageValueToDamageEvaluation2;
                            }

                            break;
                        default:
                            if (target.Side != _side)
                            {
                                int hitPoints = target.HitPoints;
                                int hits = target.GetHits();
                                int multDamageEvaluation = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, source: U, target), multiplier: hits, divider: hitPoints);
                                value += multDamageEvaluation;
                            }
                            else
                            {
                                int hitPoints = target.HitPoints;
                                int hits = target.GetHits();
                                int multDamageEvaluation = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, source: U, target), multiplier: hits, divider: hitPoints);
                                value -= multDamageEvaluation;
                            }

                            break;
                    }
                }
            }
        }
    }

    // AoWTC.pas:1007
    public void EvalHex(CombatObject? ignore, int ability, int spell, int X, int Y, ref int value, bool hitWalls, bool friendly, TacticalBattleUnit? U, object? ignoreWall)
    {
        AbilityDamageValues damageValue = default;
        TacticalBattleUnitHexSprite hexSprite = CombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild<TacticalBattleUnitHexSprite>(TacticalBattleUnitHexSprite.ClassId);
        if (hexSprite is not null && hexSprite.Unit != ignore && (friendly || hexSprite.Unit.Side != U.Side))
        {
            if (ability == 52)
            {
                GamePack.Current.SpellControl[spell].GetCombatDamageValues(U, target: hexSprite.Unit, values: ref damageValue);
            }
            else
            {
                if (TacticalTables.AbilityTypes[ability] == BattleEffectType.Touch)
                {
                    GamePack.Current.AbilityControl[ability].TacticalBattleGetDamageValues(U, defender: hexSprite.Unit, damageValues: ref damageValue);
                }
                else
                {
                    GamePack.Current.AbilityControl[ability].TacticalBattleGetDamageValues(U, defender: hexSprite.Unit, damageValues: ref damageValue);
                }
            }

            if (ability == 65)
            {
                damageValue.TargetDamage = 0;
            }

            if (damageValue.SourceDamage > 0)
            {
                switch (ability)
                {
                    case 52:
                        switch (spell)
                        {
                            case 106:
                                if (hexSprite.Unit.Side != U.Side)
                                {
                                    int tacticalBattleDamageValueToDamageEvaluation2 = TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, source: U, target: hexSprite.Unit);
                                    value += tacticalBattleDamageValueToDamageEvaluation2;
                                }
                                else
                                {
                                    int tacticalBattleDamageValueToDamageEvaluation2 = TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, source: U, target: hexSprite.Unit);
                                    value -= tacticalBattleDamageValueToDamageEvaluation2;
                                }

                                break;
                            default:
                                if (hexSprite.Unit.Side != U.Side)
                                {
                                    int hitPoints = hexSprite.Unit.HitPoints;
                                    int hits = hexSprite.Unit.GetHits();
                                    int multDamageEvaluation = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, source: U, target: hexSprite.Unit), multiplier: hits, divider: hitPoints);
                                    value += multDamageEvaluation;
                                }
                                else
                                {
                                    int hitPoints = hexSprite.Unit.HitPoints;
                                    int hits = hexSprite.Unit.GetHits();
                                    int multDamageEvaluation = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, source: U, target: hexSprite.Unit), multiplier: hits, divider: hitPoints);
                                    value -= multDamageEvaluation;
                                }

                                break;
                        }

                        break;
                    default:
                        if (hexSprite.Unit.Side != U.Side)
                        {
                            int hitPoints = hexSprite.Unit.HitPoints;
                            int hits = hexSprite.Unit.GetHits();
                            int multDamageEvaluation = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, source: U, target: hexSprite.Unit), multiplier: hits, divider: hitPoints);
                            value += multDamageEvaluation;
                        }
                        else
                        {
                            int hitPoints = hexSprite.Unit.HitPoints;
                            int hits = hexSprite.Unit.GetHits();
                            int multDamageEvaluation = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, source: U, target: hexSprite.Unit), multiplier: hits, divider: hitPoints);
                            value -= multDamageEvaluation;
                        }

                        break;
                }
            }
        }

        if (hitWalls && CombatMap.Current.Combat.CombatData.WallType != 0)
        {
            CityWall? wall = CombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild<CityWall>(CityWall.ClassId) ?? CombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2_229_510) as CityWall;
            if (!CombatMap.Current.Combat.CombatData.WallBreached && ignoreWall is not null && wall is not null && (ignore is null or not CombatWall || (ignore is CombatWall && ignoreWall != wall)))
            {
                if (ability == 52)
                {
                    GamePack.Current.SpellControl[spell].GetCombatDamageValues(U, target: CombatMap.Current.Combat.CombatData.Wall, values: ref damageValue);
                }
                else
                {
                    if (TacticalTables.AbilityTypes[ability] == BattleEffectType.Touch)
                    {
                        GamePack.Current.AbilityControl[ability].TacticalBattleGetDamageValues(U, defender: CombatMap.Current.Combat.CombatData.Wall, damageValues: ref damageValue);
                    }
                    else
                    {
                        GamePack.Current.AbilityControl[ability].TacticalBattleGetDamageValues(U, defender: CombatMap.Current.Combat.CombatData.Wall, damageValues: ref damageValue);
                    }
                }

                if (ability == 65)
                {
                    damageValue.TargetDamage = 0;
                }

                if (damageValue.SourceDamage > 0)
                {
                    if (U.Side == CombatSideIndex.Attacker)
                    {
                        int multDamageEvaluation = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, source: U, target: CombatMap.Current.Combat.CombatData.Wall), multiplier: TacticalTables.WallMaxHitPoints[wall.WallType - 1], divider: wall.HitPoints);
                        value += multDamageEvaluation;
                    }
                    else
                    {
                        int multDamageEvaluation = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, source: U, target: CombatMap.Current.Combat.CombatData.Wall), multiplier: TacticalTables.WallMaxHitPoints[wall.WallType - 1], divider: wall.HitPoints);
                        value -= multDamageEvaluation;
                    }
                }
            }
        }
    }

    // AoWTC.pas:1072
    public void EvalArea(ref int value, CombatObject ignore, int ability, int spell, int x, int y, bool hitWalls, bool includeCenter, bool friendly, TacticalBattleUnit unit, object? ignoreWall)
    {
        int x1 = default;
        int y1 = default;
        for (int i = 0; i <= 6; i++)
        {
            HexGeometry.CenterHexNumberToHex(x, y, hexNumber: i, ref x1, ref y1);
            if (includeCenter || unit.UnitHexSprite.HexX != x1 || unit.UnitHexSprite.HexY != y1)
            {
                EvalHex(ignore, ability, spell, x1, y1, ref value, hitWalls, friendly, U: unit, ignoreWall);
            }
        }
    }

    // AoWTC.pas:1084
    public int EvalPath(ref int value, CombatObject ignore, int ability, int spell, int x1, int y1, int x2, int y2, bool hitWalls, TacticalBattleUnit U, object? ignoreWall)
    {
        BattleEffectType effect = ability == 52 ? TacticalTables.SpellTypes[spell] : TacticalTables.AbilityTypes[ability];
        switch (effect)
        {
            case BattleEffectType.Arrow:
            case BattleEffectType.Rock:
            case BattleEffectType.Comet:
            case BattleEffectType.Breath:
            case BattleEffectType.CometBall:
            case BattleEffectType.Ray:
            case BattleEffectType.AltArrow:
                Point3DList point3DList = new();
                DelphiList<RangedObstacle> obstacles = [];
                if (effect == BattleEffectType.Breath)
                {
                    TacticalTables.MakeRangedPath(U.Side, speed: 50, arc: 20, unusedHeight: 50, x1.TruncateToInt16(), y1.TruncateToInt16(), x2.TruncateToInt16(), y2.TruncateToInt16(), 0, 0, includeUnits: false, path: point3DList, obstacles);
                }
                else
                {
                    TacticalTables.MakeAbilityPath(U.Side, ability: (short)ability, spell: (short)spell, x1.TruncateToInt16(), y1.TruncateToInt16(), x2.TruncateToInt16(), y2.TruncateToInt16(), path: point3DList, obstacles);
                }

                int total = 0;
                int original = value;
                int chance = 100;
                {
                    for (int i = 0, last = obstacles.Count - 1; i <= last; i++)
                    {
                        RangedObstacle? obstacle = obstacles[i];
                        Point3D? path = point3DList[obstacle.PathIndex];
                        RangedObstacle? obstacle2 = obstacles[i];
                        HexGeometry.PixelToHex(point3DList[obstacle2.PathIndex].X, vertical: path.Y, out int x, out int y);
                        int v = 0;
                        EvalHex(ignore, ability, spell, x, y, value: ref v, hitWalls, friendly: true, U, ignoreWall);
                        try
                        {
                            RangedObstacle? obstacle3 = obstacles[i];
                            total += v * chance / 100 * obstacle3.Chance / 100;
                        }
                        catch
                        {
                            total += obstacles[i].Chance * (v / 100 * chance / 100);
                        }

                        chance = (100 - obstacles[i].Chance) * chance / 100;
                    }
                }

                try
                {
                    value = value * chance / 100;
                }
                catch
                {
                    value = value / 100 * chance;
                }

                value += total;
                point3DList?.Dispose();
                {
                    for (int i = 0, last = obstacles.Count - 1; i <= last; i++)
                    {
                        (obstacles[i] as IDisposable)?.Dispose();
                    }
                }

                obstacles?.Dispose();
                return original - value;
            case BattleEffectType.None:
            case BattleEffectType.Touch:
            case BattleEffectType.Effect:
            case BattleEffectType.FBExplosion:
            case BattleEffectType.ScreenNESW:
            case BattleEffectType.ScreenNS:
            case BattleEffectType.Chain:
            case BattleEffectType.Swarm:
            case BattleEffectType.ScreenSwarm:
            case BattleEffectType.SFOverlay:
            case BattleEffectType.DamLightning:
            case BattleEffectType.DamFire:
            case BattleEffectType.DamPoison:
            case BattleEffectType.DamMagic:
            case BattleEffectType.DamHoly:
            case BattleEffectType.DamDeath:
            case BattleEffectType.DamCold:
            case BattleEffectType.SacredWrath:
            case BattleEffectType.SWComet:
            case BattleEffectType.Dispel:
            case BattleEffectType.WallSmoke:
            case BattleEffectType.Blood:
            case BattleEffectType.Tremors:
            case BattleEffectType.Geyser:
            case BattleEffectType.GeyserWater:
            case BattleEffectType.HailImpact:
            case BattleEffectType.Cloud:
            case BattleEffectType.CloudPuff:
            case BattleEffectType.HitGround:
            case BattleEffectType.HitWater:
            case BattleEffectType.HighPrayer:
            case BattleEffectType.PrayerGlow:
            case BattleEffectType.Resurrect:
            case BattleEffectType.FrostBeam:
            case BattleEffectType.Frost:
            case BattleEffectType.MindDecay:
            case BattleEffectType.DecayLeg:
            case BattleEffectType.ShockWave:
            case BattleEffectType.DecayComet:
            case BattleEffectType.SacFlame:
            case BattleEffectType.Winds:
            case BattleEffectType.SacFire:
            case BattleEffectType.BreathDam:
            default:
                return 0;
        }
    }

    // AoWTC.pas:1130
    public int EvalBreath(ref int value, CombatObject ignore, int ability, int spell, int x1, int y1, int x2, int y2, TacticalBattleUnit U)
    {
        int x = default;
        int y = default;
        int hexX = U.UnitHexSprite.HexX;
        int hexY = U.UnitHexSprite.HexY;
        int result = EvalPath(ref value, ignore, ability, spell, hexX, hexY, x2, y2, hitWalls: false, U, ignoreWall: null);
        for (int i = 0; i <= 11; i++)
        {
            int v = 0;
            int deltaHexToHexNumber2 = HexGeometry.DeltaHexToHexNumber(x1, y1, x2, y2);
            HexGeometry.CenterHexNumberToHex(x1, y1, hexNumber: TacticalTables.BreathHexNumber[(TacticalTables.BreathDirection[deltaHexToHexNumber2 - 1] * 12) + i - 12], ref x, ref y);
            EvalHex(ignore, ability, spell, x, y, value: ref v, hitWalls: true, friendly: true, U, ignoreWall: null);
            if (v != 0)
            {
                int fUnitHexSpriteHexX = U.UnitHexSprite.HexX;
                int fUnitHexSpriteHexY = U.UnitHexSprite.HexY;
                _ = EvalPath(ref v, ignore, ability, spell, fUnitHexSpriteHexX, fUnitHexSpriteHexY, x, y, hitWalls: false, U, ignoreWall: null);
            }

            value += v;
            result -= v;
        }

        return result;
    }

    // AoWTC.pas:1149
    public void CheckUnit(int x2, int y2, int hits, int hitPoints, CombatMoveControlSettings settings, TacticalBattleUnit unit, CombatObject? target, DelphiList<CombatAbilityRef> abilities, object? wall)
    {
        AbilityDamageValues values = default;
        MovePath pathStepList = new();
        MovePath basePath = new();
        MovePath detourPathPathStepList = new();
        int terrainValue = 0;
        int originalX = x2;
        int originalY = y2;
        int yhx = unit.UnitHexSprite.HexY;
        int distance = HexGeometry.DeltaHexToRadius(unit.UnitHexSprite.HexX, yhx, x2, y2);
        int i;
        int pathCost;
        if (unit.MovePoints > 0)
        {
            CombatMap.Current.MoveControl.CalculatePath(settings, path: basePath, x2.TruncateToSByte(), y2.TruncateToSByte(), L: 0);
            pathCost = MakeTerrainPath(settings, path: basePath, value: ref terrainValue, unit, ignore: target, freeStrikes: true, findWalls: target is TacticalBattleUnit && _side == CombatSideIndex.Attacker, cost: 1);
            i = 0;
            int last = basePath.Count - 1;
            for (; i <= last; i += 1)
            {
                _ = pathStepList.Add(basePath[i]);
            }
        }
        else
        {
            pathCost = distance * 4;
        }

        if (pathCost != -1)
        {
            int safeValue;
            int v;
            int cost = default;
            int bestIndex;
            int ability;
            int spell;
            int value;
            bool adjacent;
            try
            {
                if (target is CombatUnit && target.Side != unit.Side && CombatObject.MeleeDamageTypes(unit) != DamageTypes.None && (unit.GetAbilityEnabled(1) || !target.GetAbilityEnabled(1)))
                {
                    ability = 0;
                    spell = 0;
                    MeleeRound round = new(Creator);
                    round.Calculate(unit, defender: target);
                    if (round.GetAttackerDamageValue() > 0)
                    {
                        adjacent = pathStepList.MovePointsToPosition(unit.MovePoints) == 0;
                        unit.CanFlee = false;
                        if (target is CombatWall)
                        {
                            _flee = false;
                        }
                        value = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue2: round.GetDefenderDamageValue(), damageValue1: round.GetAttackerDamageValue(), source: unit, target: target), multiplier: hitPoints, divider: hits) + terrainValue;
                        if (target.GetTargetStrength() <= 0 && _enemies.Count > 1)
                        {
                            value -= 10_000;
                        }

                        cost = (pathCost - unit.MovePoints) * 10;
                        if (cost < 1)
                        {
                            cost = 1;
                        }

                        if (unit.Troop is not Hero)
                        {
                            safeValue = 0;
                        }
                        else
                        {
                            i = -1;
                            bestIndex = pathStepList.MovePointsToPosition(unit.MovePoints);
                            PathStep pathY = pathStepList[bestIndex];
                            safeValue = UnitSafe(unit, pathStepList[bestIndex].Location.X, pathY.Location.Y);
                            while (bestIndex < pathStepList.Count - 1 && safeValue < 0)
                            {
                                PathStep path = pathStepList[bestIndex];
                                safeValue = UnitSafe(unit, pathStepList[bestIndex].Location.X, path.Location.Y);
                                if (safeValue >= 0)
                                {
                                    i = bestIndex;
                                }

                                bestIndex += 1;
                            }

                            if (i > -1)
                            {
                                v = 0;
                                for (; v <= i - 1; v += 1)
                                {
                                    pathStepList.Delete(0);
                                }
                            }
                        }

                        if (pathStepList.Count > 1)
                        {
                            if ((safeValue >= 0 || _normalUnits == 0) && adjacent)
                            {
                                if (value > 0 || _normalUnits == 0 || unit.Troop is not Hero || _units.Count <= 1)
                                {
                                    _heroMoves += 1;
                                    i = unit.Troop is Hero ? 50 : 100;

                                    CreateMove(BattleAIMoveType.Melee, x2, y2, value: ScaleDamageEvaluation(value, multiplier: i, divider: 1), ability, spell, unit, target);
                                }
                            }
                            else
                            {
                                if (_ignoreWalls || ChargeFromPosition(unit.Position >>> 4))
                                {
                                    if (!((safeValue < 0 && _normalUnits != 0) || unit.MovePoints <= 0 || (pathStepList.List[pathStepList.Count - 2].MoveCost > unit.MovePoints && pathStepList.List[pathStepList.Count - 2].MoveCost <= 15)))
                                    {
                                        _heroMoves += 1;
                                        int multDamageEvaluation = ScaleDamageEvaluation(multiplier: unit.Moves, value: value, divider: cost);
                                        PathStep path = pathStepList[0];
                                        CreateMove(BattleAIMoveType.Move, pathStepList[0].Location.X, path.Location.Y, value: multDamageEvaluation, ability, spell, unit, target);
                                    }
                                    else
                                    {
                                        CreateMove(BattleAIMoveType.Fake, x2, y2, value: ScaleDamageEvaluation(multiplier: unit.Moves, value: value, divider: cost), ability, spell, unit, target);
                                    }
                                }
                            }
                        }
                    }

                    round?.Dispose();
                }
            }
            catch
            {
                _ = MessageBox(0, text: "Error during Melee Check", caption: "Error - TCAI ", flags: 0);
            }

            int hexX = default;
            int hexY = default;
            int x = default;
            int y = default;
            AbilityCombatInfo abilityInfo = default;
            SpellCombatInfo spellInfo = default;
            for (int abilityIndex = 0, last = abilities.Count - 1; abilityIndex <= last; abilityIndex++)
            {
                pathStepList.Clear();
                i = 0;
                int limit = basePath.Count - 1;
                for (; i <= limit; i += 1)
                {
                    _ = pathStepList.Add(basePath[i]);
                }

                ability = abilities[abilityIndex].Ability;
                spell = 0;
                x2 = originalX;
                y2 = originalY;
                GamePack.Current.AbilityControl[ability].GetCombatInfo(unit.Troop, info: ref abilityInfo);
                int range;
                int bestValue;
                int d;
                int pathValue;
                CityWall? cityWall;
                switch (ability)
                {
                    case 52:
                        try
                        {
                            if (target is not CombatWall || !CombatMap.Current.Combat.CombatData.WallBreached)
                            {
                                spell = abilities[abilityIndex].Spell;
                                switch (TacticalTables.SpellTypes[spell])
                                {
                                    case BattleEffectType.Arrow:
                                    case BattleEffectType.Rock:
                                    case BattleEffectType.Comet:
                                    case BattleEffectType.Breath:
                                    case BattleEffectType.CometBall:
                                    case BattleEffectType.Effect:
                                    case BattleEffectType.ScreenNESW:
                                    case BattleEffectType.ScreenNS:
                                    case BattleEffectType.Ray:
                                    case BattleEffectType.Chain:
                                    case BattleEffectType.Swarm:
                                    case BattleEffectType.ScreenSwarm:
                                    case BattleEffectType.SacredWrath:
                                    case BattleEffectType.SWComet:
                                    case BattleEffectType.Dispel:
                                    case BattleEffectType.Blood:
                                    case BattleEffectType.AltArrow:
                                    case BattleEffectType.Tremors:
                                    case BattleEffectType.HailImpact:
                                    case BattleEffectType.HighPrayer:
                                    case BattleEffectType.MindDecay:
                                    case BattleEffectType.ShockWave:
                                    case BattleEffectType.SacFlame:
                                    case BattleEffectType.Winds:
                                        GamePack.Current.SpellControl[spell].GetCombatInfo(unit.Troop, info: ref spellInfo);
                                        GamePack.Current.SpellControl[spell].GetCombatDamageValues(unit, target, ref values);
                                        if (values.SourceDamage > 0)
                                        {
                                            unit.CanFlee = false;
                                            if (target is CombatWall)
                                            {
                                                _flee = false;
                                            }

                                            pathValue = 0;
                                            range = spellInfo.Range;
                                            if (spell is 108 or 114)
                                            {
                                                range = 4;
                                            }

                                            if (range == -1)
                                            {
                                                range = 100;
                                            }

                                            value = spell switch
                                            {
                                                8 or (>= 15 and <= 17) or 32 or 33 or 36 or 37 or 40 or 42 or 43 or 49 or 56 or 77 or 102 or 106 or 110 or 111 => TacticalBattleDamageValueToDamageEvaluation(values.SourceDamage, 0, source: unit, target),
                                                _ => ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(values.SourceDamage, 0, source: unit, target), multiplier: hitPoints, divider: hits),
                                            };
                                            if (target.GetTargetStrength() <= 0 && _enemies.Count > 1)
                                            {
                                                value -= 10_000;
                                            }

                                            if (unit.MovePoints > 0)
                                            {
                                                i = pathStepList.MovePointsToPosition(unit.MovePoints);
                                                PathStep pathY = pathStepList[i];
                                                d = HexGeometry.DeltaHexToRadius(pathStepList[i].Location.X, pathY.Location.Y, x2, y2);
                                                if (d <= range || distance <= range)
                                                {
                                                    cost = value;
                                                    if (distance <= range)
                                                    {
                                                        switch (spell)
                                                        {
                                                            case 117:
                                                                int xhx = unit.UnitHexSprite.HexX;
                                                                int fUnitHexSpriteHexY = unit.UnitHexSprite.HexY;
                                                                pathValue = EvalPath(ref value, ignore: target, ability, spell, xhx, fUnitHexSpriteHexY, x2, y2, hitWalls: true, U: unit, ignoreWall: wall);
                                                                EvalArea(ref value, ignore: target, ability, spell, x2, y2, hitWalls: true, includeCenter: true, friendly: true, unit, ignoreWall: wall);
                                                                bestIndex = x2;
                                                                bestValue = y2;
                                                                {
                                                                    i = 1;
                                                                    for (; i <= 6; i += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(x2, y2, hexNumber: i, ref hexX, ref hexY);
                                                                        int hexY2 = unit.UnitHexSprite.HexY;
                                                                        if (HexGeometry.DeltaHexToRadius(unit.UnitHexSprite.HexX, hexY2, hexX, hexY) <= range)
                                                                        {
                                                                            d = cost;
                                                                            int fUnitHexSpriteHexX = unit.UnitHexSprite.HexX;
                                                                            int hexY3 = unit.UnitHexSprite.HexY;
                                                                            v = EvalPath(ref d, ignore: target, ability, spell, fUnitHexSpriteHexX, hexY3, hexX, hexY, hitWalls: true, U: unit, ignoreWall: wall);
                                                                            EvalArea(ref d, ignore: target, ability, spell, hexX, hexY, hitWalls: true, includeCenter: true, friendly: true, unit, ignoreWall: wall);
                                                                            if (d > value)
                                                                            {
                                                                                value = d;
                                                                                pathValue = v;
                                                                                bestIndex = hexX;
                                                                                bestValue = hexY;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                x2 = bestIndex;
                                                                y2 = bestValue;
                                                                break;
                                                            case 127:
                                                                value = 0;
                                                                cost = 0;
                                                                {
                                                                    i = 1;
                                                                    for (; i <= 36; i += 1)
                                                                    {
                                                                        int hexY2 = unit.UnitHexSprite.HexY;
                                                                        HexGeometry.CenterHexNumberToHex(unit.UnitHexSprite.HexX, hexY2, hexNumber: i, ref hexX, ref hexY);
                                                                        EvalHex(null, ability, spell, hexX, hexY, ref value, hitWalls: true, friendly: true, U: unit, ignoreWall: wall);
                                                                    }
                                                                }

                                                                break;
                                                            case 128:
                                                                EvalHex(target, ability, spell, unit.UnitHexSprite.HexX, unit.UnitHexSprite.HexY, ref value, hitWalls: true, friendly: true, U: unit, ignoreWall: wall);
                                                                {
                                                                    i = 0;
                                                                    for (; i <= 18; i += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(x2, y2, hexNumber: i, ref hexX, ref hexY);
                                                                        EvalHex(target, ability, spell, hexX, hexY, ref value, hitWalls: true, friendly: true, U: unit, ignoreWall: wall);
                                                                    }
                                                                }

                                                                bestIndex = x2;
                                                                bestValue = y2;
                                                                {
                                                                    i = 1;
                                                                    for (; i <= 18; i += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(x2, y2, hexNumber: i, ref hexX, ref hexY);
                                                                        int hexY2 = unit.UnitHexSprite.HexY;
                                                                        if (HexGeometry.DeltaHexToRadius(unit.UnitHexSprite.HexX, hexY2, hexX, hexY) <= range)
                                                                        {
                                                                            d = cost;
                                                                            EvalHex(target, ability, spell, unit.UnitHexSprite.HexX, unit.UnitHexSprite.HexY, value: ref d, hitWalls: true, friendly: true, U: unit, ignoreWall: wall);
                                                                            v = 0;
                                                                            for (; v <= 18; v += 1)
                                                                            {
                                                                                HexGeometry.CenterHexNumberToHex(hexX, hexY, hexNumber: v, ref x, ref y);
                                                                                EvalHex(target, ability, spell, x, y, value: ref d, hitWalls: true, friendly: true, U: unit, ignoreWall: wall);
                                                                            }

                                                                            if (d > value)
                                                                            {
                                                                                value = d;
                                                                                bestIndex = hexX;
                                                                                bestValue = hexY;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                x2 = bestIndex;
                                                                y2 = bestValue;
                                                                break;
                                                            case 100:
                                                            case 103:
                                                            case 119:
                                                            case 122:
                                                            case 123:
                                                                int hexX2 = unit.UnitHexSprite.HexX;
                                                                int hexY4 = unit.UnitHexSprite.HexY;
                                                                pathValue = EvalPath(ref value, ignore: target, ability, spell, hexX2, hexY4, x2, y2, hitWalls: false, U: unit, ignoreWall: wall);
                                                                break;
                                                            case 116:
                                                                EvalArea(ref value, ignore: target, ability, spell, x2, y2, hitWalls: true, includeCenter: true, friendly: true, unit, ignoreWall: wall);
                                                                bestIndex = x2;
                                                                bestValue = y2;
                                                                {
                                                                    i = 1;
                                                                    for (; i <= 6; i += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(x2, y2, hexNumber: i, ref hexX, ref hexY);
                                                                        int hexY2 = unit.UnitHexSprite.HexY;
                                                                        if (HexGeometry.DeltaHexToRadius(unit.UnitHexSprite.HexX, hexY2, hexX, hexY) <= range)
                                                                        {
                                                                            d = cost;
                                                                            EvalArea(ref d, ignore: target, ability, spell, hexX, hexY, hitWalls: true, includeCenter: true, friendly: true, unit, ignoreWall: wall);
                                                                            if (d > value)
                                                                            {
                                                                                value = d;
                                                                                bestIndex = hexX;
                                                                                bestValue = hexY;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                x2 = bestIndex;
                                                                y2 = bestValue;
                                                                break;
                                                            case 104:
                                                            case 120:
                                                                EvalArea(ref value, ignore: target, ability, spell, x2, y2, hitWalls: false, includeCenter: true, friendly: true, unit, ignoreWall: wall);
                                                                bestIndex = x2;
                                                                bestValue = y2;
                                                                {
                                                                    i = 1;
                                                                    for (; i <= 6; i += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(x2, y2, hexNumber: i, ref hexX, ref hexY);
                                                                        int hexY2 = unit.UnitHexSprite.HexY;
                                                                        if (HexGeometry.DeltaHexToRadius(unit.UnitHexSprite.HexX, hexY2, hexX, hexY) <= range)
                                                                        {
                                                                            d = cost;
                                                                            EvalArea(ref d, ignore: target, ability, spell, hexX, hexY, hitWalls: false, includeCenter: true, friendly: true, unit, ignoreWall: wall);
                                                                            if (d > value)
                                                                            {
                                                                                value = d;
                                                                                bestIndex = hexX;
                                                                                bestValue = hexY;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                x2 = bestIndex;
                                                                y2 = bestValue;
                                                                break;
                                                            case 106:
                                                                EvalArea(ref value, ignore: target, ability, spell, x2, y2, hitWalls: false, includeCenter: false, friendly: false, unit, ignoreWall: wall);
                                                                bestIndex = x2;
                                                                bestValue = y2;
                                                                {
                                                                    i = 1;
                                                                    for (; i <= 6; i += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(x2, y2, hexNumber: i, ref hexX, ref hexY);
                                                                        int hexY2 = unit.UnitHexSprite.HexY;
                                                                        if (HexGeometry.DeltaHexToRadius(unit.UnitHexSprite.HexX, hexY2, hexX, hexY) <= range)
                                                                        {
                                                                            d = cost;
                                                                            EvalArea(ref d, ignore: target, ability, spell, hexX, hexY, hitWalls: false, includeCenter: false, friendly: false, unit, ignoreWall: wall);
                                                                            if (d > value)
                                                                            {
                                                                                value = d;
                                                                                bestIndex = hexX;
                                                                                bestValue = hexY;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                x2 = bestIndex;
                                                                y2 = bestValue;
                                                                break;
                                                            case 112:
                                                                if (_side == CombatSideIndex.Defender && CombatMap.Current.Combat.CombatData.WallType != 0)
                                                                {
                                                                    value = -9_999;
                                                                }
                                                                else
                                                                {
                                                                    EvalAll(ref value, list: _enemies, ignore: target, spell, U: unit);
                                                                    EvalAll(ref value, list: _allies, ignore: target, spell, U: unit);
                                                                    EvalAll(ref value, list: _units, ignore: target, spell, U: unit);
                                                                }

                                                                break;
                                                            case 125:
                                                                EvalAll(ref value, list: _enemies, ignore: target, spell, U: unit);
                                                                EvalAll(ref value, list: _allies, ignore: target, spell, U: unit);
                                                                EvalAll(ref value, list: _units, ignore: target, spell, U: unit);
                                                                break;
                                                            case 102:
                                                                EvalAll(ref value, list: _units, ignore: target, spell, U: unit);
                                                                break;
                                                            case 105:
                                                                EvalAll(ref value, list: _enemies, ignore: target, spell, U: unit);
                                                                break;
                                                            case 108:
                                                            case 114:
                                                                int hexX3 = unit.UnitHexSprite.HexX;
                                                                int hexY5 = unit.UnitHexSprite.HexY;
                                                                pathValue = EvalBreath(ref value, ignore: target, ability, spell, hexX3, hexY5, x2, y2, U: unit);
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                    }
                                                }

                                                if (d <= range && (pathValue > 0 || value > 0))
                                                {
                                                    if (pathValue != 0 && value < cost)
                                                    {
                                                        bestIndex = 0;
                                                        bestValue = value;
                                                        d = cost;
                                                        i = pathStepList.Count - 2;
                                                        while (i >= 1)
                                                        {
                                                            PathStep path = pathStepList[i];
                                                            PathStep pathX = pathStepList[i];
                                                            cityWall = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, path.Location.Y].FindOwnedChild<CityWall>(CityWall.ClassId);
                                                            if (cityWall is null && _side == CombatSideIndex.Attacker)
                                                            {
                                                                PathStep path2 = pathStepList[i];
                                                                PathStep path3 = pathStepList[i];
                                                                cityWall = CombatMap.Current.MapContainer.MapLevels[0].Field[path3.Location.X, path2.Location.Y].FindOwnedChild(2_229_510) as CityWall;
                                                            }

                                                            if (!(cityWall is null || cityWall.HitPoints <= 0 || unit.GetAbilityEnabled(1) || unit.GetAbilityEnabled(36) || unit.GetAbilityEnabled(66)))
                                                            {
                                                                i = 1;
                                                            }
                                                            else
                                                            {
                                                                PathStep path2 = pathStepList[i];
                                                                PathStep path3 = pathStepList[i];
                                                                PathStep path4 = pathStepList[0];
                                                                if (HexGeometry.DeltaHexToRadius(pathStepList[0].Location.X, path4.Location.Y, path3.Location.X, path2.Location.Y) <= range)
                                                                {
                                                                    PathStep path5 = pathStepList[i];
                                                                    PathStep path6 = pathStepList[i];
                                                                    v = EvalPath(ref cost, ignore: target, ability, spell, path5.Location.X, path6.Location.Y, x2, y2, hitWalls: spell == 117, U: unit, ignoreWall: wall);
                                                                    cost = d;
                                                                    if (v < pathValue)
                                                                    {
                                                                        bestValue = bestValue + pathValue - v;
                                                                        pathValue = v;
                                                                        bestIndex = i;
                                                                    }
                                                                }
                                                            }

                                                            i -= 1;
                                                        }

                                                        if (bestIndex > 0)
                                                        {
                                                            detourPathPathStepList.Clear();
                                                            i = bestIndex;
                                                            int last2 = pathStepList.Count - 1;
                                                            for (; i <= last2; i += 1)
                                                            {
                                                                _ = detourPathPathStepList.Add(pathStepList[i]);
                                                            }

                                                            terrainValue = 0;
                                                            pathCost = MakeTerrainPath(settings, path: detourPathPathStepList, value: ref terrainValue, unit, ignore: target, freeStrikes: true, findWalls: false, cost: 1);
                                                            pathValue += bestValue;
                                                            pathCost = pathStepList.PositionToMovePoints(0);
                                                        }
                                                    }

                                                    bestIndex = 0;
                                                    i = 1;
                                                    int last3 = pathStepList.Count - 2;
                                                    for (; i <= last3; i += 1)
                                                    {
                                                        PathStep path = pathStepList[i];
                                                        if (HexGeometry.DeltaHexToRadius(x2, y2, pathStepList[i].Location.X, path.Location.Y) <= range && pathStepList.PositionToMovePoints(i) <= unit.MovePoints)
                                                        {
                                                            cost = 1_000;
                                                            PathStep pathX = pathStepList[i];
                                                            PathStep path2 = pathStepList[i];
                                                            v = EvalPath(ref cost, ignore: target, ability, spell, pathX.Location.X, path2.Location.Y, x2, y2, hitWalls: spell == 117, U: unit, ignoreWall: wall);
                                                            if (v == 0 && cost == 1_000)
                                                            {
                                                                bestIndex = i;
                                                            }
                                                        }
                                                    }

                                                    if (bestIndex > 0)
                                                    {
                                                        i = 0;
                                                        for (; i <= bestIndex - 1; i += 1)
                                                        {
                                                            pathStepList.Delete(0);
                                                        }
                                                    }

                                                    pathCost = pathStepList.PositionToMovePoints(0);
                                                }

                                                // { Keep move selection inside the positive movement guard. }
                                                cost = (pathCost - unit.MovePoints - (range * 4)) * 10;
                                                if (cost < 1)
                                                {
                                                    cost = 1;
                                                }

                                                if (unit.Troop is not Hero)
                                                {
                                                    safeValue = 0;
                                                }
                                                else
                                                {
                                                    i = -1;
                                                    bestIndex = pathStepList.MovePointsToPosition(unit.MovePoints);
                                                    PathStep path = pathStepList[bestIndex];
                                                    safeValue = UnitSafe(unit, pathStepList[bestIndex].Location.X, path.Location.Y);
                                                    while (bestIndex < pathStepList.Count - 1 && safeValue < 0)
                                                    {
                                                        PathStep path2 = pathStepList[bestIndex];
                                                        safeValue = UnitSafe(unit, pathStepList[bestIndex].Location.X, path2.Location.Y);
                                                        if (safeValue >= 0)
                                                        {
                                                            i = bestIndex;
                                                        }

                                                        bestIndex += 1;
                                                    }

                                                    if (i > -1)
                                                    {
                                                        v = 0;
                                                        for (; v <= i - 1; v += 1)
                                                        {
                                                            pathStepList.Delete(0);
                                                        }
                                                    }
                                                }

                                                if ((pathValue == 0 || value != 0) && distance <= range && unit.Moves == unit.MovePoints)
                                                {
                                                    _heroMoves += 1;
                                                    i = Math.Abs(value + pathValue);
                                                    if (i < 1)
                                                    {
                                                        i = 1;
                                                    }

                                                    CreateMove(BattleAIMoveType.Ranged, x2, y2, value: ScaleDamageEvaluation(value, multiplier: value * 100 / i, divider: 1), ability, spell, unit, target);
                                                    if (pathValue > 0 && unit.MovePoints > 0 && detourPathPathStepList.Count > 1)
                                                    {
                                                        if (!(detourPathPathStepList.List[detourPathPathStepList.Count - 2].MoveCost > unit.MovePoints && detourPathPathStepList.List[detourPathPathStepList.Count - 2].MoveCost <= 15))
                                                        {
                                                            _heroMoves += 1;
                                                            int multDamageEvaluation = ScaleDamageEvaluation(pathValue + terrainValue, multiplier: 100 * pathValue / i, divider: 1);
                                                            PathStep detourPath = detourPathPathStepList[0];
                                                            CreateMove(BattleAIMoveType.MoveRanged, detourPathPathStepList[0].Location.X, detourPath.Location.Y, value: multDamageEvaluation, ability, spell, unit, target);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (pathStepList.Count > 1 && (_ignoreWalls || ChargeFromPosition(unit.Position >>> 4)))
                                                    {
                                                        if (!((safeValue < 0 && _normalUnits != 0) || unit.MovePoints <= 0 || pathStepList.Count <= 1 || (pathStepList.List[pathStepList.Count - 2].MoveCost > unit.MovePoints && pathStepList.List[pathStepList.Count - 2].MoveCost <= 15)))
                                                        {
                                                            int multDamageEvaluation = ScaleDamageEvaluation(multiplier: unit.Moves, value: pathValue + terrainValue + value, divider: cost);
                                                            PathStep path = pathStepList[0];
                                                            CreateMove(BattleAIMoveType.MoveRanged, pathStepList[0].Location.X, path.Location.Y, value: multDamageEvaluation, ability, spell, unit, target);
                                                        }
                                                        else
                                                        {
                                                            CreateMove(BattleAIMoveType.Fake, x2, y2, value: ScaleDamageEvaluation(multiplier: unit.Moves, value: ScaleDamageEvaluation(pathValue, multiplier: 100, divider: 1) + terrainValue + value, divider: cost), ability, spell, unit, target);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        break;
                                    case BattleEffectType.None:
                                    case BattleEffectType.Touch:
                                    case BattleEffectType.FBExplosion:
                                    case BattleEffectType.SFOverlay:
                                    case BattleEffectType.DamLightning:
                                    case BattleEffectType.DamFire:
                                    case BattleEffectType.DamPoison:
                                    case BattleEffectType.DamMagic:
                                    case BattleEffectType.DamHoly:
                                    case BattleEffectType.DamDeath:
                                    case BattleEffectType.DamCold:
                                    case BattleEffectType.WallSmoke:
                                    case BattleEffectType.Geyser:
                                    case BattleEffectType.GeyserWater:
                                    case BattleEffectType.Cloud:
                                    case BattleEffectType.CloudPuff:
                                    case BattleEffectType.HitGround:
                                    case BattleEffectType.HitWater:
                                    case BattleEffectType.PrayerGlow:
                                    case BattleEffectType.Resurrect:
                                    case BattleEffectType.FrostBeam:
                                    case BattleEffectType.Frost:
                                    case BattleEffectType.DecayLeg:
                                    case BattleEffectType.DecayComet:
                                    case BattleEffectType.SacFire:
                                    case BattleEffectType.BreathDam:
                                    default:
                                        break;
                                }
                            }
                        }
                        catch
                        {
                            _ = MessageBox(0, text: "Error during Spell Check", caption: NullTerminatedText($"Error - TCAI {ability.ToString(CultureInfo.InvariantCulture)} {spell.ToString(CultureInfo.InvariantCulture)}"), flags: 0);
                        }

                        break;
                    default:
                        switch (TacticalTables.AbilityTypes[ability])
                        {
                            case BattleEffectType.Touch:
                                if (target is CombatWall || (target is CombatUnit && GamePack.Current.AbilityControl.GetAbility<TouchAbility>(ability).CanTouch(unit, target)))
                                {
                                    try
                                    {
                                        GamePack.Current.AbilityControl[ability].TacticalBattleGetDamageValues(unit, defender: target, damageValues: ref values);
                                        if (values.SourceDamage > 0)
                                        {
                                            adjacent = pathStepList.MovePointsToPosition(unit.MovePoints) == 0;
                                            unit.CanFlee = false;
                                            if (target is CombatWall)
                                            {
                                                _flee = false;
                                            }

                                            if (ability == 65)
                                            {
                                                values.TargetDamage = 0;
                                            }

                                            value = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(values.SourceDamage, values.TargetDamage, source: unit, target), multiplier: hitPoints, divider: hits) + terrainValue;
                                            value = ability switch
                                            {
                                                (>= 27 and <= 29) or 40 or 60 or 106 or 107 or 148 => TacticalBattleDamageValueToDamageEvaluation(values.SourceDamage, 0, source: unit, target),
                                                _ => ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(values.SourceDamage, 0, source: unit, target), multiplier: hitPoints, divider: hits),
                                            };
                                            if (ability == 117 && CombatObject.MeleeDamageTypes(unit) == DamageTypes.None)
                                            {
                                                value = ScaleDamageEvaluation(value, multiplier: 100, divider: 1);
                                            }

                                            if (target.GetTargetStrength() <= 0 && _enemies.Count > 1)
                                            {
                                                value -= 10_000;
                                            }

                                            if (unit.MovePoints > 0 && pathStepList.MovePointsToPosition(unit.MovePoints) == 0)
                                            {
                                                switch (ability)
                                                {
                                                    case 65:
                                                        EvalArea(ref value, ignore: target, ability, spell, pathStepList.List[1].Location.X, pathStepList.List[1].Location.Y, hitWalls: true, includeCenter: false, friendly: true, unit, ignoreWall: wall);
                                                        break;
                                                    case 114:
                                                        EvalArea(ref value, ignore: target, ability, spell, pathStepList.List[1].Location.X, pathStepList.List[1].Location.Y, hitWalls: false, includeCenter: false, friendly: true, unit, ignoreWall: wall);
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }

                                            cost = (pathCost - unit.MovePoints) * 10;
                                            if (cost < 1)
                                            {
                                                cost = 1;
                                            }

                                            if (unit.Troop is not Hero)
                                            {
                                                safeValue = 0;
                                            }
                                            else
                                            {
                                                i = -1;
                                                bestIndex = pathStepList.MovePointsToPosition(unit.MovePoints);
                                                PathStep pathY = pathStepList[bestIndex];
                                                safeValue = UnitSafe(unit, pathStepList[bestIndex].Location.X, pathY.Location.Y);
                                                while (bestIndex < pathStepList.Count - 1 && safeValue < 0)
                                                {
                                                    PathStep path = pathStepList[bestIndex];
                                                    safeValue = UnitSafe(unit, pathStepList[bestIndex].Location.X, path.Location.Y);
                                                    if (safeValue >= 0)
                                                    {
                                                        i = bestIndex;
                                                    }

                                                    bestIndex += 1;
                                                }

                                                if (i > -1)
                                                {
                                                    v = 0;
                                                    for (; v <= i - 1; v += 1)
                                                    {
                                                        pathStepList.Delete(0);
                                                    }
                                                }
                                            }

                                            if (pathStepList.Count > 1)
                                            {
                                                if ((safeValue >= 0 || _normalUnits == 0) && adjacent)
                                                {
                                                    _heroMoves += 1;
                                                    i = unit.Troop is Hero
                                                        ? 1
: target is CombatWall && CombatMap.Current.Combat.CombatData.WallBreached && CombatMap.Current.Round > 1
                                                            ? 10
                                                            : 100;

                                                    CreateMove(BattleAIMoveType.Touch, x2, y2, value: ScaleDamageEvaluation(value, multiplier: i, divider: 1), ability, spell, unit, target);
                                                }
                                                else
                                                {
                                                    if (_ignoreWalls || ChargeFromPosition(unit.Position >>> 4))
                                                    {
                                                        if (!((safeValue < 0 && _normalUnits != 0) || unit.MovePoints <= 0 || (pathStepList.List[pathStepList.Count - 2].MoveCost > unit.MovePoints && pathStepList.List[pathStepList.Count - 2].MoveCost <= 15)))
                                                        {
                                                            _heroMoves += 1;
                                                            int multDamageEvaluation = ScaleDamageEvaluation(multiplier: unit.Moves, value: value, divider: cost);
                                                            PathStep path = pathStepList[0];
                                                            CreateMove(BattleAIMoveType.Move, pathStepList[0].Location.X, path.Location.Y, value: multDamageEvaluation, ability, spell, unit, target);
                                                        }
                                                        else
                                                        {
                                                            CreateMove(BattleAIMoveType.Fake, x2, y2, value: ScaleDamageEvaluation(multiplier: unit.Moves, value: value, divider: cost), ability, spell, unit, target);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        _ = MessageBox(0, text: "Error during Touch Check", caption: NullTerminatedText($"Error - TCAI {ability.ToString(CultureInfo.InvariantCulture)} {spell.ToString(CultureInfo.InvariantCulture)}"), flags: 0);
                                    }
                                }

                                break;
                            case BattleEffectType.Arrow:
                            case BattleEffectType.Rock:
                            case BattleEffectType.Comet:
                            case BattleEffectType.Breath:
                            case BattleEffectType.CometBall:
                            case BattleEffectType.Effect:
                            case BattleEffectType.ScreenNESW:
                            case BattleEffectType.ScreenNS:
                            case BattleEffectType.Ray:
                            case BattleEffectType.Chain:
                            case BattleEffectType.Swarm:
                            case BattleEffectType.ScreenSwarm:
                            case BattleEffectType.SacredWrath:
                            case BattleEffectType.SWComet:
                            case BattleEffectType.Dispel:
                            case BattleEffectType.Blood:
                            case BattleEffectType.AltArrow:
                            case BattleEffectType.Tremors:
                            case BattleEffectType.HailImpact:
                            case BattleEffectType.HighPrayer:
                                try
                                {
                                    cost = GamePack.Current.AbilityControl[ability].TacticalBattleGetDamageValue(unit, target);
                                    if (cost > 0)
                                    {
                                        unit.CanFlee = false;
                                        if (target is CombatWall)
                                        {
                                            _flee = false;
                                        }

                                        pathValue = 0;
                                        range = GamePack.Current.AbilityControl[ability].GetCombatMode(unit.Troop) == CombatMode.LongRange
                                            ? 12
: GamePack.Current.AbilityControl[ability].GetCombatMode(unit.Troop) == CombatMode.MediumRange
                                                ? 8
                                                : 4;

                                        value = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(cost, 0, source: unit, target), multiplier: hitPoints, divider: hits);
                                        if (target.GetTargetStrength() <= 0 && _enemies.Count > 1)
                                        {
                                            value -= 10_000;
                                        }

                                        if (unit.MovePoints > 0)
                                        {
                                            i = pathStepList.MovePointsToPosition(unit.MovePoints);
                                            PathStep pathY = pathStepList[i];
                                            d = HexGeometry.DeltaHexToRadius(pathStepList[i].Location.X, pathY.Location.Y, x2, y2);
                                            if (d <= range || distance <= range)
                                            {
                                                cost = value;
                                                if (distance <= range)
                                                {
                                                    switch (ability)
                                                    {
                                                        case 22:
                                                        case 25:
                                                        case 45:
                                                        case 55:
                                                        case 57:
                                                        case 58:
                                                        case 61:
                                                        case 62:
                                                        case >= 120 and <= 124:
                                                            int xhx = unit.UnitHexSprite.HexX;
                                                            int fUnitHexSpriteHexY = unit.UnitHexSprite.HexY;
                                                            pathValue = EvalPath(ref value, ignore: target, ability, spell, xhx, fUnitHexSpriteHexY, x2, y2, hitWalls: false, U: unit, ignoreWall: wall);
                                                            break;
                                                        case 24:
                                                        case 44:
                                                            int fUnitHexSpriteHexX = unit.UnitHexSprite.HexX;
                                                            int hexY2 = unit.UnitHexSprite.HexY;
                                                            pathValue = EvalPath(ref value, ignore: target, ability, spell, fUnitHexSpriteHexX, hexY2, x2, y2, hitWalls: true, U: unit, ignoreWall: wall);
                                                            break;
                                                        case 30:
                                                        case >= 86 and <= 90:
                                                            int hexX2 = unit.UnitHexSprite.HexX;
                                                            int hexY3 = unit.UnitHexSprite.HexY;
                                                            pathValue = EvalBreath(ref value, ignore: target, ability, spell, hexX2, hexY3, x2, y2, U: unit);
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                            }

                                            if (d <= range && (pathValue > 0 || value > 0))
                                            {
                                                if (pathValue != 0 && value < cost)
                                                {
                                                    bestIndex = 0;
                                                    bestValue = value;
                                                    d = cost;
                                                    i = pathStepList.Count - 2;
                                                    while (i >= 1)
                                                    {
                                                        PathStep path = pathStepList[i];
                                                        PathStep pathX = pathStepList[i];
                                                        cityWall = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, path.Location.Y].FindOwnedChild<CityWall>(CityWall.ClassId);
                                                        if (cityWall is null && _side == CombatSideIndex.Attacker)
                                                        {
                                                            PathStep path2 = pathStepList[i];
                                                            PathStep path3 = pathStepList[i];
                                                            cityWall = CombatMap.Current.MapContainer.MapLevels[0].Field[path3.Location.X, path2.Location.Y].FindOwnedChild(2_229_510) as CityWall;
                                                        }

                                                        if (!(cityWall is null || cityWall.HitPoints <= 0 || unit.GetAbilityEnabled(1) || unit.GetAbilityEnabled(36) || unit.GetAbilityEnabled(66)))
                                                        {
                                                            i = 1;
                                                        }
                                                        else
                                                        {
                                                            PathStep path2 = pathStepList[i];
                                                            PathStep path3 = pathStepList[i];
                                                            PathStep path4 = pathStepList[0];
                                                            if (HexGeometry.DeltaHexToRadius(pathStepList[0].Location.X, path4.Location.Y, path3.Location.X, path2.Location.Y) <= range)
                                                            {
                                                                PathStep path5 = pathStepList[i];
                                                                PathStep path6 = pathStepList[i];
                                                                v = EvalPath(ref cost, ignore: target, ability, spell, path5.Location.X, path6.Location.Y, x2, y2, hitWalls: ability is 24 or 44, U: unit, ignoreWall: wall);
                                                                cost = d;
                                                                if (v < pathValue)
                                                                {
                                                                    bestValue = bestValue + pathValue - v;
                                                                    pathValue = v;
                                                                    bestIndex = i;
                                                                }
                                                            }
                                                        }

                                                        i -= 1;
                                                    }

                                                    if (bestIndex > 0)
                                                    {
                                                        detourPathPathStepList.Clear();
                                                        i = bestIndex;
                                                        int last2 = pathStepList.Count - 1;
                                                        for (; i <= last2; i += 1)
                                                        {
                                                            _ = detourPathPathStepList.Add(pathStepList[i]);
                                                        }

                                                        terrainValue = 0;
                                                        pathCost = MakeTerrainPath(settings, path: detourPathPathStepList, value: ref terrainValue, unit, ignore: target, freeStrikes: true, findWalls: false, cost: 1);
                                                        pathValue += bestValue;
                                                        pathCost = pathStepList.PositionToMovePoints(0);
                                                    }
                                                }

                                                bestIndex = 0;
                                                i = 1;
                                                int last3 = pathStepList.Count - 2;
                                                for (; i <= last3; i += 1)
                                                {
                                                    PathStep path = pathStepList[i];
                                                    if (HexGeometry.DeltaHexToRadius(x2, y2, pathStepList[i].Location.X, path.Location.Y) <= range && pathStepList.PositionToMovePoints(i) <= unit.MovePoints)
                                                    {
                                                        cost = 1_000;
                                                        PathStep pathX = pathStepList[i];
                                                        PathStep path2 = pathStepList[i];
                                                        v = EvalPath(ref cost, ignore: target, ability, spell, pathX.Location.X, path2.Location.Y, x2, y2, hitWalls: spell == 117, U: unit, ignoreWall: wall);
                                                        if (v == 0 && cost == 1_000)
                                                        {
                                                            bestIndex = i;
                                                        }
                                                    }
                                                }

                                                if (bestIndex > 0)
                                                {
                                                    i = 0;
                                                    for (; i <= bestIndex - 1; i += 1)
                                                    {
                                                        pathStepList.Delete(0);
                                                    }
                                                }

                                                pathCost = pathStepList.PositionToMovePoints(0);
                                            }

                                            // { Keep move selection inside the positive movement guard. }
                                            cost = (pathCost - unit.MovePoints - (range * 4)) * 10;
                                            if (cost < 1)
                                            {
                                                cost = 1;
                                            }

                                            if (unit.Troop is not Hero)
                                            {
                                                safeValue = 0;
                                            }
                                            else
                                            {
                                                i = -1;
                                                bestIndex = pathStepList.MovePointsToPosition(unit.MovePoints);
                                                PathStep path = pathStepList[bestIndex];
                                                safeValue = UnitSafe(unit, pathStepList[bestIndex].Location.X, path.Location.Y);
                                                while (bestIndex < pathStepList.Count - 1 && safeValue < 0)
                                                {
                                                    PathStep path2 = pathStepList[bestIndex];
                                                    safeValue = UnitSafe(unit, pathStepList[bestIndex].Location.X, path2.Location.Y);
                                                    if (safeValue >= 0)
                                                    {
                                                        i = bestIndex;
                                                    }

                                                    bestIndex += 1;
                                                }

                                                if (i > -1)
                                                {
                                                    v = 0;
                                                    for (; v <= i - 1; v += 1)
                                                    {
                                                        pathStepList.Delete(0);
                                                    }
                                                }
                                            }

                                            int hexY4 = unit.UnitHexSprite.HexY;
                                            if (!((UnitSafe(unit, unit.UnitHexSprite.HexX, hexY4) < 0 && _normalUnits != 0) || (pathValue != 0 && value == 0) || distance > range || unit.Moves != unit.MovePoints))
                                            {
                                                _heroMoves += 1;
                                                i = target is CombatWall && CombatMap.Current.Combat.CombatData.WallBreached && CombatMap.Current.Round > 1
                                                    ? 10
                                                    : 100;

                                                v = Math.Abs(value + pathValue);
                                                if (v < 1)
                                                {
                                                    v = 1;
                                                }

                                                CreateMove(BattleAIMoveType.Ranged, x2, y2, value: ScaleDamageEvaluation(value, multiplier: i * value / v, divider: 1), ability, spell, unit, target);
                                                if (pathValue > 0 && unit.MovePoints > 0 && detourPathPathStepList.Count > 1)
                                                {
                                                    if (!(detourPathPathStepList.List[detourPathPathStepList.Count - 2].MoveCost > unit.MovePoints && detourPathPathStepList.List[detourPathPathStepList.Count - 2].MoveCost <= 15))
                                                    {
                                                        _heroMoves += 1;
                                                        int multDamageEvaluation = ScaleDamageEvaluation(pathValue + terrainValue, multiplier: i * pathValue / v, divider: 1);
                                                        PathStep detourPath = detourPathPathStepList[0];
                                                        CreateMove(BattleAIMoveType.MoveRanged, detourPathPathStepList[0].Location.X, detourPath.Location.Y, value: multDamageEvaluation, ability, spell, unit, target);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (pathStepList.Count > 1 && (_ignoreWalls || ChargeFromPosition(unit.Position >>> 4)))
                                                {
                                                    if (!((safeValue < 0 && _normalUnits != 0) || unit.MovePoints <= 0 || pathStepList.Count <= 1 || (pathStepList.List[pathStepList.Count - 2].MoveCost > unit.MovePoints && pathStepList.List[pathStepList.Count - 2].MoveCost <= 15)))
                                                    {
                                                        int multDamageEvaluation = ScaleDamageEvaluation(multiplier: unit.Moves, value: pathValue + terrainValue + value, divider: cost);
                                                        PathStep path = pathStepList[0];
                                                        CreateMove(BattleAIMoveType.MoveRanged, pathStepList[0].Location.X, path.Location.Y, value: multDamageEvaluation, ability, spell, unit, target);
                                                    }
                                                    else
                                                    {
                                                        CreateMove(BattleAIMoveType.Fake, x2, y2, value: ScaleDamageEvaluation(multiplier: unit.Moves, value: ScaleDamageEvaluation(pathValue, multiplier: 100, divider: 1) + terrainValue + value, divider: cost), ability, spell, unit, target);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    _ = MessageBox(0, text: "Error during Ranged Check", caption: NullTerminatedText($"Error - TCAI {ability.ToString(CultureInfo.InvariantCulture)} {spell.ToString(CultureInfo.InvariantCulture)}"), flags: 0);
                                }

                                break;
                            case BattleEffectType.None:
                            case BattleEffectType.FBExplosion:
                            case BattleEffectType.SFOverlay:
                            case BattleEffectType.DamLightning:
                            case BattleEffectType.DamFire:
                            case BattleEffectType.DamPoison:
                            case BattleEffectType.DamMagic:
                            case BattleEffectType.DamHoly:
                            case BattleEffectType.DamDeath:
                            case BattleEffectType.DamCold:
                            case BattleEffectType.WallSmoke:
                            case BattleEffectType.Geyser:
                            case BattleEffectType.GeyserWater:
                            case BattleEffectType.Cloud:
                            case BattleEffectType.CloudPuff:
                            case BattleEffectType.HitGround:
                            case BattleEffectType.HitWater:
                            case BattleEffectType.PrayerGlow:
                            case BattleEffectType.Resurrect:
                            case BattleEffectType.FrostBeam:
                            case BattleEffectType.Frost:
                            case BattleEffectType.MindDecay:
                            case BattleEffectType.DecayLeg:
                            case BattleEffectType.ShockWave:
                            case BattleEffectType.DecayComet:
                            case BattleEffectType.SacFlame:
                            case BattleEffectType.Winds:
                            case BattleEffectType.SacFire:
                            case BattleEffectType.BreathDam:
                            default:
                                break;
                        }

                        break;
                }
            }
        }

        pathStepList?.Dispose();
        basePath?.Dispose();
        detourPathPathStepList?.Dispose();
    }

    // AoWTC.pas:1628
    public void EvalBattle()
    {
        Status = AIStatus.Busy;
        if (StrategicMap.Current.TokenManager.PlayerReady(CombatMap.Current.CurrentPlayer))
        {
            int i;
            int j;
            int k = default;
            int l = default;
            int n;
            int value;
            BattleAIMove move;
            BattleAIMove otherMove;
            TacticalBattleUnit u;
            TacticalBattleUnit target;
            CombatMoveControlSettings settings;
            DelphiList<CombatAbilityRef> abilities;
            string reason = "";
            AbilityCombatInfo aiInfo = default;
            SpellCombatInfo spellInfo = default;
            CityWall? cityWall;
            switch (_currentMove)
            {
                case 0:
                    try
                    {
                        _unitIndex = 0;
                        _heroMoves = 0;
                        _units.Clear();
                        _allies.Clear();
                        _enemies.Clear();
                        i = 0;
                        int last = _moves.Count - 1;
                        for (; i <= last; i += 1)
                        {
                            (_moves[i] as IDisposable)?.Dispose();
                        }

                        _moves.Clear();
                        _walls.Clear();
                        TacticalBattle? tacticalBattle = CombatMap.Current.Combat;
                        _side = (CombatSideIndex)tacticalBattle.CombatData.Players.GetPlayers((sbyte)tacticalBattle.CombatData.CurrentPlayer).SideIndex;
                        _normalUnits = 0;
                        i = 0;
                        int limit = tacticalBattle.CombatData.GetCount() - 1;
                        for (; i <= limit; i += 1)
                        {
                            if (tacticalBattle.CombatData.GetObjects(i) is not null && tacticalBattle.CombatData.GetObjects(i) is CombatUnit && tacticalBattle.CombatData.GetObjects(i).GetEnabled())
                            {
                                bool visible = false;
                                j = 0;
                                while (!visible && j < 19)
                                {
                                    int hexY = tacticalBattle.CombatData.GetObjects<TacticalBattleUnit>(i).UnitHexSprite.HexY;
                                    HexGeometry.CenterHexNumberToHex(tacticalBattle.CombatData.GetObjects<TacticalBattleUnit>(i).UnitHexSprite.HexX, hexY, hexNumber: j, ref k, ref l);
                                    if (!CombatMap.Current.ExplorationEnabled || CombatMap.Current.MapContainer.GetField(k, l, 0).GetExplored(CombatMap.Current.SeatedPlayer))
                                    {
                                        visible = true;
                                    }
                                    else
                                    {
                                        j += 1;
                                    }
                                }

                                if (visible)
                                {
                                    if (tacticalBattle.CombatData.GetObjects(i).Side == _side)
                                    {
                                        if ((byte)tacticalBattle.CombatData.GetObjects(i).CombatPlayer == tacticalBattle.CombatData.CurrentPlayer)
                                        {
                                            if (!tacticalBattle.CombatData.GetObjects(i).GetLocked())
                                            {
                                                if (tacticalBattle.CombatData.GetObjects<CombatUnit>(i).Troop is Hero)
                                                {
                                                    _units.Add(tacticalBattle.CombatData.GetObjects<TacticalBattleUnit>(i));
                                                }
                                                else
                                                {
                                                    _normalUnits += 1;
                                                    _units.Insert(0, item: tacticalBattle.CombatData.GetObjects<TacticalBattleUnit>(i));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            _allies.Add(tacticalBattle.CombatData.GetObjects<TacticalBattleUnit>(i));
                                        }
                                    }
                                    else
                                    {
                                        _enemies.Add(tacticalBattle.CombatData.GetObjects<TacticalBattleUnit>(i));
                                    }
                                }
                            }
                        }

                        Creator.GetEngine().OutputDebugString($"AI found {_units.Count.ToString(CultureInfo.InvariantCulture)} Own units.");
                        Creator.GetEngine().OutputDebugString($"AI found {_allies.Count.ToString(CultureInfo.InvariantCulture)} Ally units.");
                        Creator.GetEngine().OutputDebugString($"AI found {_enemies.Count.ToString(CultureInfo.InvariantCulture)} Enemy units.");
                        _ignoreWalls = _side == CombatSideIndex.Attacker;
                        _flee = _side == CombatSideIndex.Attacker && CombatMap.Current.Combat.CombatData.WallType != 0 && !CombatMap.Current.Combat.CombatData.WallBreached;
                        _minMoves = 100;
                        i = 0;
                        int last2 = _units.Count - 1;
                        for (; i <= last2; i += 1)
                        {
                            u = _units[i];
                            u.CanFlee = _side == CombatSideIndex.Attacker;
                            if (u.Moves < _minMoves)
                            {
                                _minMoves = u.Moves;
                            }

                            j = 0;
                            int last3 = _enemies.Count - 1;
                            for (; j <= last3; j += 1)
                            {
                                target = _enemies[j];
                                if (!_ignoreWalls && target.Side != u.Side)
                                {
                                    int hexY = target.UnitHexSprite.HexY;
                                    int hexX = target.UnitHexSprite.HexX;
                                    int fUnitHexSpriteHexY = u.UnitHexSprite.HexY;
                                    k = HexGeometry.DeltaHexToRadius(u.UnitHexSprite.HexX, fUnitHexSpriteHexY, hexX, hexY);
                                    l = 22;
                                    for (; l <= 169; l += 1)
                                    {
                                        if (target.GetAbilityEnabled(l))
                                        {
                                            if (l == 52 && target.Troop is Hero)
                                            {
                                                n = 0;
                                                for (; n <= 130; n += 1)
                                                {
                                                    bool condition = target.Troop.Player > 0 && TacticalTables.SpellTypes[n] != BattleEffectType.None && StrategicMap.Current.Players.GetPlayers(target.Troop.Player).MagicControl.Spells.IndexOf(n) != -1;
                                                    if (condition)
                                                    {
                                                        Spell? spell = GamePack.Current.SpellControl[n];
                                                        condition = target.Troop.CanCastSpellInstantly(n, mana: spell.BaseManaCost, ref reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        GamePack.Current.SpellControl[n].GetCombatInfo(target.Troop, info: ref spellInfo);
                                                        if (spellInfo.TargetTypes.Contains(CombatTargetTypes.Enemy) && (k <= spellInfo.Range || spellInfo.Range == -1))
                                                        {
                                                            value = GamePack.Current.SpellControl[n].GetCombatDamageValue(target, target: u);
                                                            if (value > 0)
                                                            {
                                                                if (TacticalTables.SpellTypes[n] is (>= BattleEffectType.Arrow and <= BattleEffectType.Comet) or BattleEffectType.Breath or BattleEffectType.CometBall or BattleEffectType.Ray)
                                                                {
                                                                    int fUnitHexSpriteHexX = target.UnitHexSprite.HexX;
                                                                    int hexY2 = target.UnitHexSprite.HexY;
                                                                    int hexX2 = u.UnitHexSprite.HexX;
                                                                    int hexY3 = u.UnitHexSprite.HexY;
                                                                    _ = EvalPath(ref value, ignore: u, ability: l, spell: n, fUnitHexSpriteHexX, hexY2, hexX2, hexY3, hitWalls: false, U: target, ignoreWall: null);
                                                                    if (value > 0)
                                                                    {
                                                                        _ignoreWalls = true;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    _ignoreWalls = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (TacticalTables.AbilityTypes[l] is not BattleEffectType.None and not BattleEffectType.Touch)
                                                {
                                                    GamePack.Current.AbilityControl[l].GetCombatInfo(target.Troop, info: ref aiInfo);
                                                    if ((((byte)aiInfo.TargetTypes) & 2) != 0)
                                                    {
                                                        n = GamePack.Current.AbilityControl[l].GetCombatMode(target.Troop) == CombatMode.LongRange
                                                            ? 12
: GamePack.Current.AbilityControl[l].GetCombatMode(target.Troop) == CombatMode.MediumRange
                                                                ? 8
                                                                : 4;

                                                        if (k <= n)
                                                        {
                                                            value = GamePack.Current.AbilityControl[l].TacticalBattleGetDamageValue(target, target: u);
                                                            if (value > 0)
                                                            {
                                                                if (TacticalTables.AbilityTypes[l] is (>= BattleEffectType.Arrow and <= BattleEffectType.Comet) or BattleEffectType.Breath or BattleEffectType.CometBall or BattleEffectType.Ray)
                                                                {
                                                                    int fUnitHexSpriteHexX = target.UnitHexSprite.HexX;
                                                                    int hexY2 = target.UnitHexSprite.HexY;
                                                                    int hexX2 = u.UnitHexSprite.HexX;
                                                                    int hexY3 = u.UnitHexSprite.HexY;
                                                                    _ = EvalPath(ref value, ignore: u, ability: l, spell: 0, fUnitHexSpriteHexX, hexY2, hexX2, hexY3, hitWalls: false, U: target, ignoreWall: null);
                                                                    if (value > 0)
                                                                    {
                                                                        _ignoreWalls = true;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    _ignoreWalls = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        _currentMove += 1;
                        _unitIndex = 0;
                        _unitLimit = 0;
                    }
                    catch
                    {
                        _ = MessageBox(0, text: "Error - TCAI", caption: "Error during Create Unit List", flags: 0);
                    }

                    break;
                case 1:
                    try
                    {
                        while (_unitIndex < _units.Count && _unitLimit < 1)
                        {
                            u = _units[_unitIndex];
                            if (!u.GetLocked())
                            {
                                abilities = [];
                                try
                                {
                                    j = 0;
                                    for (; j <= 169; j += 1)
                                    {
                                        if (u.GetAbilityEnabled(j))
                                        {
                                            if (j is 1 or 36 or 66 or 117)
                                            {
                                                _flee = false;
                                            }

                                            if (j == 52 && u.Troop is Hero)
                                            {
                                                k = 0;
                                                for (; k <= 130; k += 1)
                                                {
                                                    bool condition = u.Troop.Player > 0 && TacticalTables.SpellTypes[k] != BattleEffectType.None && StrategicMap.Current.Players.GetPlayers(u.Troop.Player).MagicControl.Spells.IndexOf(k) != -1;
                                                    if (condition)
                                                    {
                                                        Spell? spell = GamePack.Current.SpellControl[k];
                                                        condition = u.Troop.CanCastSpellInstantly(k, mana: spell.BaseManaCost, ref reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        GamePack.Current.SpellControl[k].GetCombatInfo(u.Troop, info: ref spellInfo);
                                                        if (spellInfo.TargetTypes.Contains(CombatTargetTypes.Enemy))
                                                        {
                                                            _ = abilities.Add(new CombatAbilityRef());
                                                            abilities[^1].Ability = j;
                                                            abilities[^1].Spell = k;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (TacticalTables.AbilityTypes[j] != BattleEffectType.None)
                                                {
                                                    GamePack.Current.AbilityControl[j].GetCombatInfo(u.Troop, info: ref aiInfo);
                                                    if ((((byte)aiInfo.TargetTypes) & 2) != 0)
                                                    {
                                                        _ = abilities.Add(new CombatAbilityRef());
                                                        abilities[^1].Ability = j;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    _ = MessageBox(0, text: "Error during Eval Enemies", caption: NullTerminatedText($"Error - TCAI {u.Troop.GetName()}"), flags: 0);
                                }

                                settings = new CombatMoveControlSettings(null) { Holder = Creator.CreationHolder() };
                                if (u.MovePoints > 0)
                                {
                                    int hexY = u.UnitHexSprite.HexY;
                                    settings.SetSourcePosition(u.UnitHexSprite.HexX, hexY, 0);
                                    settings.ArmyHexSprite = u.UnitHexSprite;
                                    settings.MoveFlags = CombatMoveModes.Melee | CombatMoveModes.Move | CombatMoveModes.Wall;
                                    MovePointTable movePointTable = settings.MovePointTable;
                                    u.Troop.CreateMovePointTable(out movePointTable);
                                    settings.MovePointTable = movePointTable;
                                    settings.Player = u.GetPlayer();
                                    settings.MovePointsMax = 200;
                                    CombatMap.Current.MoveControl.ProcessMove(settings);
                                }

                                if (abilities.Count > 0 || CombatObject.MeleeDamageTypes(u) != DamageTypes.None)
                                {
                                    j = 0;
                                    int last = _enemies.Count - 1;
                                    for (; j <= last; j += 1)
                                    {
                                        target = _enemies[j];
                                        if (u.Troop is not Hero || _heroMoves == 0)
                                        {
                                            int hits = target.GetHits();
                                            int hitPoints = target.HitPoints;
                                            int hexY = target.UnitHexSprite.HexY;
                                            CheckUnit(target.UnitHexSprite.HexX, hexY, hits: hitPoints, hitPoints: hits, settings, unit: u, target, abilities, wall: null);
                                        }
                                    }
                                }

                                i = 0;
                                int limit = abilities.Count - 1;
                                for (; i <= limit; i += 1)
                                {
                                    (abilities[i] as IDisposable)?.Dispose();
                                }

                                abilities.Clear();
                                try
                                {
                                    j = 22;
                                    for (; j <= 169; j += 1)
                                    {
                                        if (u.GetAbilityEnabled(j))
                                        {
                                            if (j == 52 && u.Troop is Hero)
                                            {
                                                k = 0;
                                                for (; k <= 130; k += 1)
                                                {
                                                    bool condition = TacticalTables.SpellTypes[k] != BattleEffectType.None && StrategicMap.Current.Players.GetPlayers(u.Troop.Player).MagicControl.Spells.IndexOf(k) != -1;
                                                    if (condition)
                                                    {
                                                        Spell? spell = GamePack.Current.SpellControl[k];
                                                        condition = u.Troop.CanCastSpellInstantly(k, mana: spell.BaseManaCost, ref reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        GamePack.Current.SpellControl[k].GetCombatInfo(u.Troop, info: ref spellInfo);
                                                        if (spellInfo.TargetTypes.Contains(CombatTargetTypes.Friend))
                                                        {
                                                            _ = abilities.Add(new CombatAbilityRef());
                                                            abilities[^1].Ability = j;
                                                            abilities[^1].Spell = k;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (TacticalTables.AbilityTypes[j] != BattleEffectType.None)
                                                {
                                                    GamePack.Current.AbilityControl[j].GetCombatInfo(u.Troop, info: ref aiInfo);
                                                    if ((((byte)aiInfo.TargetTypes) & 1) != 0)
                                                    {
                                                        _ = abilities.Add(new CombatAbilityRef());
                                                        abilities[^1].Ability = j;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    _ = MessageBox(0, text: "Error during Eval Friends", caption: NullTerminatedText($"Error - TCAI {u.Troop.GetName()}"), flags: 0);
                                }

                                if (abilities.Count > 0)
                                {
                                    j = 0;
                                    int last = _units.Count - 1;
                                    for (; j <= last; j += 1)
                                    {
                                        target = _units[j];
                                        if (u.Troop is not Hero || _heroMoves == 0)
                                        {
                                            int hits = target.GetHits();
                                            int hitPoints = target.HitPoints;
                                            int hexY = target.UnitHexSprite.HexY;
                                            CheckUnit(target.UnitHexSprite.HexX, hexY, hits: hitPoints, hitPoints: hits, settings, unit: u, target, abilities, wall: null);
                                        }
                                    }
                                }

                                settings?.Dispose();
                                i = 0;
                                int last2 = abilities.Count - 1;
                                for (; i <= last2; i += 1)
                                {
                                    (abilities[i] as IDisposable)?.Dispose();
                                }

                                abilities?.Dispose();
                            }

                            _unitIndex += 1;
                            _unitLimit += 1;
                        }

                        if (_unitIndex >= _units.Count)
                        {
                            _currentMove += 1;
                            try
                            {
                                if (CombatMap.Current.Combat.CombatData.WallType != 0 && CombatMap.Current.Combat.CombatData.Wall is not null && _side == CombatSideIndex.Attacker)
                                {
                                    j = 0;
                                    int last = CombatMap.Current.Walls.Count - 1;
                                    for (; j <= last; j += 1)
                                    {
                                        bool condition = CombatMap.Current.Walls[j].HitPoints > 0;
                                        if (condition)
                                        {
                                            CityWall wall = CombatMap.Current.Walls[j];
                                            CityWall wall2 = CombatMap.Current.Walls[j];
                                            condition = wall.HitPoints < TacticalTables.WallMaxHitPoints[wall2.WallType - 1];
                                        }

                                        if (condition)
                                        {
                                            bool found = true;
                                            k = 0;
                                            while (found && k < _walls.Count)
                                            {
                                                if (_walls[k] == CombatMap.Current.Walls[j])
                                                {
                                                    found = false;
                                                }
                                                else
                                                {
                                                    k += 1;
                                                }
                                            }

                                            if (found)
                                            {
                                                CityWall wall = CombatMap.Current.Walls[j];
                                                _walls.Add(wall);
                                            }
                                        }
                                    }
                                }

                                if (CombatMap.Current.Combat.CombatData.WallType == 0 || CombatMap.Current.Combat.CombatData.Wall is null || _side == CombatSideIndex.Defender)
                                {
                                    _currentMove += 1;
                                }

                                _unitIndex = 0;
                                Creator.GetEngine().OutputDebugString($"AI calculated {(_units.Count * _enemies.Count).ToString(CultureInfo.InvariantCulture)} Attack paths.");
                                Creator.GetEngine().OutputDebugString($"AI calculated {_moves.Count.ToString(CultureInfo.InvariantCulture)} Moves.");
                            }
                            catch
                            {
                                _ = MessageBox(0, text: "Error - TCAI ", caption: "Error during Eval Ending", flags: 0);
                            }
                        }

                        _unitLimit = 0;
                    }
                    catch
                    {
                        _ = MessageBox(0, text: "Error - TCAI", caption: "Error during Create Move List", flags: 0);
                    }

                    break;
                case 2:
                    try
                    {
                        while (_unitIndex < _units.Count && _unitLimit < 1)
                        {
                            u = _units[_unitIndex];
                            if (!u.GetLocked())
                            {
                                abilities = [];
                                j = 0;
                                for (; j <= 169; j += 1)
                                {
                                    if (u.GetAbilityEnabled(j))
                                    {
                                        if (j is 1 or 36 or 66 or 117)
                                        {
                                            _flee = false;
                                        }

                                        if (j == 52 && u.Troop is Hero)
                                        {
                                            if (u.Troop.Player > 0)
                                            {
                                                k = 0;
                                                for (; k <= 130; k += 1)
                                                {
                                                    bool condition = TacticalTables.SpellTypes[k] != BattleEffectType.None && StrategicMap.Current.Players.GetPlayers(u.Troop.Player).MagicControl.Spells.IndexOf(k) != -1;
                                                    if (condition)
                                                    {
                                                        Spell? spell = GamePack.Current.SpellControl[k];
                                                        condition = u.Troop.CanCastSpellInstantly(k, mana: spell.BaseManaCost, ref reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        GamePack.Current.SpellControl[k].GetCombatInfo(u.Troop, info: ref spellInfo);
                                                        if (spellInfo.DamageTypes.Contains(DamageTypes.Wall))
                                                        {
                                                            _flee = false;
                                                            _ = abilities.Add(new CombatAbilityRef());
                                                            abilities[^1].Ability = j;
                                                            abilities[^1].Spell = k;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (TacticalTables.AbilityTypes[j] != BattleEffectType.None)
                                            {
                                                GamePack.Current.AbilityControl[j].GetCombatInfo(u.Troop, info: ref aiInfo);
                                                if (aiInfo.DamageTypes.Contains(DamageTypes.Wall))
                                                {
                                                    _flee = false;
                                                    _ = abilities.Add(new CombatAbilityRef());
                                                    abilities[^1].Ability = j;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (abilities.Count > 0)
                                {
                                    settings = new CombatMoveControlSettings(null) { Holder = Creator.CreationHolder() };
                                    if (u.MovePoints > 0)
                                    {
                                        int hexY = u.UnitHexSprite.HexY;
                                        settings.SetSourcePosition(u.UnitHexSprite.HexX, hexY, 0);
                                        settings.ArmyHexSprite = u.UnitHexSprite;
                                        settings.MoveFlags = CombatMoveModes.Melee | CombatMoveModes.Move | CombatMoveModes.Wall;
                                        if (u.GetAbilityEnabled(65) || u.GetAbilityEnabled(117))
                                        {
                                            settings.MoveFlags |= CombatMoveModes.WallCrushing;
                                        }

                                        MovePointTable movePointTable2 = settings.MovePointTable;
                                        u.Troop.CreateMovePointTable(out movePointTable2);
                                        settings.MovePointTable = movePointTable2;
                                        settings.Player = u.GetPlayer();
                                        settings.MovePointsMax = 200;
                                        CombatMap.Current.MoveControl.ProcessMove(settings);
                                    }

                                    j = 0;
                                    int last = _walls.Count - 1;
                                    for (; j <= last; j += 1)
                                    {
                                        cityWall = _walls[j];
                                        if (u.Troop is not Hero || _heroMoves == 0)
                                        {
                                            int hexY = cityWall.HexY;
                                            CheckUnit(cityWall.HexX, hexY, hits: cityWall.HitPoints, hitPoints: TacticalTables.WallMaxHitPoints[cityWall.WallType - 1] * 2, settings, unit: u, target: CombatMap.Current.Combat.CombatData.Wall, abilities, wall: cityWall);
                                        }
                                    }

                                    if (_walls.Count == 0)
                                    {
                                        l = 100;
                                        cityWall = null;
                                        foreach (CityWall item in CombatMap.Current.Walls)
                                        {
                                            int hexY = u.UnitHexSprite.HexY;
                                            int hexY2 = item.HexY;
                                            n = HexGeometry.DeltaHexToRadius(item.HexX, hexY2, u.UnitHexSprite.HexX, hexY);
                                            if (item.HitPoints > 0 && n < l)
                                            {
                                                cityWall = item;
                                                l = n;
                                            }
                                        }

                                        if (cityWall is not null)
                                        {
                                            int hexY = cityWall.HexY;
                                            CheckUnit(cityWall.HexX, hexY, hits: cityWall.HitPoints * 2, hitPoints: TacticalTables.WallMaxHitPoints[cityWall.WallType - 1], settings, unit: u, target: CombatMap.Current.Combat.CombatData.Wall, abilities, wall: cityWall);
                                        }
                                    }

                                    settings?.Dispose();
                                    i = 0;
                                    int last2 = abilities.Count - 1;
                                    for (; i <= last2; i += 1)
                                    {
                                        (abilities[i] as IDisposable)?.Dispose();
                                    }
                                }

                                abilities?.Dispose();
                            }

                            _unitIndex += 1;
                            _unitLimit += 1;
                        }

                        if (_unitIndex >= _units.Count)
                        {
                            _currentMove += 1;
                            _walls.Clear();
                            _unitIndex = 0;
                            Creator.GetEngine().OutputDebugString($"AI calculated {(_units.Count * _enemies.Count).ToString(CultureInfo.InvariantCulture)} Attack paths.");
                            Creator.GetEngine().OutputDebugString($"AI calculated {_moves.Count.ToString(CultureInfo.InvariantCulture)} Moves.");
                        }

                        _unitLimit = 0;
                    }
                    catch
                    {
                        _ = MessageBox(0, text: "Error - TCAI", caption: "Error during Create WallCrush List", flags: 0);
                    }

                    break;
                case 3:
                    try
                    {
                        List<int> bonuses = [];
                        i = 0;
                        int last = _units.Count - 1;
                        for (; i <= last; i += 1)
                        {
                            bonuses.Add(0);
                        }

                        while (_unitIndex < _moves.Count && _unitLimit < 50)
                        {
                            move = _moves[_unitIndex];
                            if (move.MoveType != BattleAIMoveType.Fake)
                            {
                                j = 0;
                                int limit = _units.Count - 1;
                                for (; j <= limit; j += 1)
                                {
                                    bonuses[j] = 0;
                                }

                                foreach (BattleAIMove aiMove in _moves)
                                {
                                    otherMove = aiMove;
                                    if (move.UnitHexSprite.Unit != otherMove.UnitHexSprite.Unit)
                                    {
                                        k = (HexGeometry.DeltaHexToRadius(move.X, move.Y, otherMove.X, otherMove.Y) * 4) - otherMove.UnitHexSprite.Unit.MovePoints;
                                        if (k < 4)
                                        {
                                            k = 4;
                                        }

                                        l = otherMove.UnitHexSprite.Unit.Moves * (otherMove.Value / (k * 10));
                                        k = _units.IndexOf(otherMove.UnitHexSprite.Unit);
                                        if (bonuses[k] < l)
                                        {
                                            bonuses[k] = l;
                                        }
                                    }
                                }

                                j = 0;
                                int last3 = _units.Count - 1;
                                for (; j <= last3; j += 1)
                                {
                                    if (move.GroupValue is > -1_000_000 and < 1_000_000)
                                    {
                                        int bonus = bonuses[j];
                                        move.GroupValue += bonus;
                                    }
                                }
                            }

                            _unitIndex += 1;
                            _unitLimit += 1;
                        }

                        if (_unitIndex >= _moves.Count)
                        {
                            _currentMove += 1;
                        }

                        _unitLimit = 0;
                    }
                    catch
                    {
                        _ = MessageBox(0, text: "Error - TCAI", caption: "Error during Build Group Bonuses", flags: 0);
                    }

                    break;
                case 4:
                    try
                    {
                        i = 0;
                        int last = _units.Count - 1;
                        for (; i <= last; i += 1)
                        {
                            u = _units[i];
                            if (u.Troop is Hero && _heroMoves == 0 && u.MovePoints > 0)
                            {
                                int hexY = u.UnitHexSprite.HexY;
                                k = UnitSafe(u, u.UnitHexSprite.HexX, hexY);
                                value = k;
                                n = -1;
                                j = 0;
                                int limit = _units.Count - 1;
                                for (; j <= limit; j += 1)
                                {
                                    target = _units[j];
                                    if (u != target)
                                    {
                                        int fUnitHexSpriteHexY = target.UnitHexSprite.HexY;
                                        l = UnitSafe(u, target.UnitHexSprite.HexX, fUnitHexSpriteHexY);
                                        if (l > k)
                                        {
                                            n = j;
                                            k = l;
                                        }
                                    }
                                }

                                if (k > value)
                                {
                                    settings = new CombatMoveControlSettings(null) { Holder = Creator.CreationHolder() };
                                    // { Original uses X for both coordinates. }
                                    int hexX = u.UnitHexSprite.HexX;
                                    settings.SetSourcePosition(u.UnitHexSprite.HexX, hexX, 0);
                                    int fUnitHexSpriteHexY = _units[n].UnitHexSprite.HexY;
                                    settings.SetDestinationPosition(_units[n].UnitHexSprite.HexX, fUnitHexSpriteHexY, 0);
                                    settings.ArmyHexSprite = u.UnitHexSprite;
                                    settings.MoveFlags = CombatMoveModes.Melee | CombatMoveModes.Move | CombatMoveModes.Wall;
                                    MovePointTable movePointTable3 = settings.MovePointTable;
                                    u.Troop.CreateMovePointTable(out movePointTable3);
                                    settings.MovePointTable = movePointTable3;
                                    settings.Player = u.GetPlayer();
                                    CombatMap.Current.MoveControl.ProcessMove(settings);
                                    if (settings.Path.Count > 2)
                                    {
                                        settings.Path.Delete(0);
                                        PathStep path = settings.Path[0];
                                        CreateMove(BattleAIMoveType.Move, settings.Path[0].Location.X, path.Location.Y, value: k, ability: 0, spell: 0, unit: u, target: null);
                                    }

                                    settings?.Dispose();
                                }
                                else
                                {
                                    if (_side == CombatSideIndex.Attacker)
                                    {
                                        u.CanFlee = true;
                                    }
                                }
                            }
                        }

                        if (_side == CombatSideIndex.Attacker)
                        {
                            i = 0;
                            int limit = _units.Count - 1;
                            for (; i <= limit; i += 1)
                            {
                                u = _units[i];
                                if ((u.Troop is not Hero || _heroMoves == 0 || (u.Troop is Hero && _units.Count == 1)) && u.GetEnabled() && !u.GetLocked() && u.MovePoints > 0 && (_flee || u.CanFlee))
                                {
                                    foreach (World.FleeLocation location in CombatMap.Current.FleeLocations)
                                    {
                                        int hexY = u.UnitHexSprite.HexY;
                                        int hexY2 = location.HexY;
                                        int deltaHexToRadius2 = HexGeometry.DeltaHexToRadius(location.HexX, hexY2, u.UnitHexSprite.HexX, hexY);
                                        int hexY3 = location.HexY;
                                        CreateMove(BattleAIMoveType.Move, location.HexX, hexY3, value: 110_000_000 - (deltaHexToRadius2 * 10), ability: 0, spell: 0, unit: u, target: null);
                                    }
                                }
                            }
                        }

                        i = 0;
                        while (i < _moves.Count)
                        {
                            move = _moves[i];
                            switch (move.MoveType)
                            {
                                case BattleAIMoveType.Fake:
                                    _moves.RemoveAt(i);
                                    break;
                                case BattleAIMoveType.Move:
                                case BattleAIMoveType.Touch:
                                case BattleAIMoveType.Melee:
                                case BattleAIMoveType.Ranged:
                                case BattleAIMoveType.MoveRanged:
                                case BattleAIMoveType.WallTouch:
                                case BattleAIMoveType.WallRanged:
                                default:
                                    {
                                        if (move.Value is > -100_000_000 and < 100_000_000)
                                        {
                                            move.Value += move.GroupValue / 4;
                                        }

                                        i += 1;
                                    }

                                    break;
                            }
                        }

                        i = 1;
                        int last3 = _moves.Count - 1;
                        for (; i <= last3; i += 1)
                        {
                            move = _moves[i];
                            j = 0;
                            k = -1;
                            while (j < i)
                            {
                                otherMove = _moves[j];
                                if (move.Value > otherMove.Value)
                                {
                                    k = j;
                                    j = i;
                                }
                                else
                                {
                                    j += 1;
                                }
                            }

                            if (k > -1)
                            {
                                if (i != k)
                                {
                                    BattleAIMove relocated = _moves[i];
                                    _moves.RemoveAt(i);
                                    _moves.Insert(k, item: relocated);
                                }
                            }
                        }

                        _currentMove += 1;
                    }
                    catch
                    {
                        _ = MessageBox(0, text: "Error - TCAI", caption: "Error during Add Group Bonuses", flags: 0);
                    }

                    break;
                case 5:
                    try
                    {
                        bool moved = false;
                        int terrainCost = default;
                        while (_moves.Count > 0 && !moved)
                        {
                            move = _moves[0];
                            int pathCost;
                            MovePath pathStepList;
                            switch (move.MoveType)
                            {
                                case BattleAIMoveType.Move:
                                case BattleAIMoveType.MoveRanged:
                                    {
                                        TacticalBattleUnitHexSprite? tacticalBattleUnitHexSprite = move.UnitHexSprite;
                                        if ((move.UnitHexSprite.Unit.Troop is Hero || (_side == CombatSideIndex.Attacker && CombatMap.Current.Round == 1)) && move.UnitHexSprite.Unit.MovePoints > _minMoves)
                                        {
                                            move.UnitHexSprite.Unit.MovePoints = _minMoves;
                                        }

                                        settings = new CombatMoveControlSettings(null) { Holder = Creator.CreationHolder() };
                                        int hexLevel = tacticalBattleUnitHexSprite.HexLevel;
                                        int hexY = tacticalBattleUnitHexSprite.HexY;
                                        settings.SetSourcePosition(tacticalBattleUnitHexSprite.HexX, hexY, hexLevel);
                                        settings.SetDestinationPosition(move.X, move.Y, 0);
                                        settings.ArmyHexSprite = move.UnitHexSprite;
                                        settings.MoveFlags = move.MoveType == BattleAIMoveType.MoveRanged || CombatObject.MeleeDamageTypes(move.UnitHexSprite.Unit) == DamageTypes.None
                                            ? CombatMoveModes.MoveRanged
                                            : CombatMoveModes.Melee;

                                        if (CombatMap.Current.Combat.CombatData.WallType != 0)
                                        {
                                            if (!(CombatMap.Current.Combat.CombatData.WallBreached && CombatMap.Current.MapContainer.MapLevels[0].Field[move.X, move.Y].FindOwnedChild(CityWall.ClassId) is null && CombatMap.Current.MapContainer.MapLevels[0].Field[move.X, move.Y].FindOwnedChild(2_229_510) is null))
                                            {
                                                if (!_ignoreWalls || !ChargeFromPosition(move.UnitHexSprite.Unit.Position >>> 4) || _side == CombatSideIndex.Attacker)
                                                {
                                                    settings.MoveFlags |= CombatMoveModes.Wall;
                                                }
                                            }
                                        }

                                        MovePointTable movePointTable4 = settings.MovePointTable;
                                        move.UnitHexSprite.Unit.Troop.CreateMovePointTable(out movePointTable4);
                                        settings.MovePointTable = movePointTable4;
                                        settings.Player = move.UnitHexSprite.Unit.GetPlayer();
                                        CombatMap.Current.MoveControl.ProcessMove(settings);
                                        if (settings.Path.Count < 2 && !settings.MoveFlags.Contains(CombatMoveModes.Wall))
                                        {
                                            settings.MoveFlags |= CombatMoveModes.Wall;
                                            CombatMap.Current.MoveControl.ProcessMove(settings);
                                        }

                                        cityWall = CombatMap.Current.MapContainer.MapLevels[0].Field[move.X, move.Y].FindOwnedChild<CityWall>(CityWall.ClassId) ?? CombatMap.Current.MapContainer.MapLevels[0].Field[move.X, move.Y].FindOwnedChild(2_229_510) as CityWall;
                                        if (settings.Path.Count == 2 && move.MoveType == BattleAIMoveType.MoveRanged && cityWall is not null)
                                        {
                                            move.UnitHexSprite.Unit.MovePoints = 0;
                                        }
                                        else
                                        {
                                            if (settings.Path.Count > 1)
                                            {
                                                pathStepList = settings.Path;
                                                pathCost = MakeTerrainPath(settings, path: pathStepList, value: ref terrainCost, move.UnitHexSprite.Unit, ignore: null, freeStrikes: false, findWalls: false, cost: 0);
                                                if (pathStepList.Count > 1 && pathCost > -1)
                                                {
                                                    if (pathStepList[^2].MoveCost > 0 && pathStepList[^2].MoveCost <= move.UnitHexSprite.Unit.MovePoints)
                                                    {
                                                        if (move.MoveType == BattleAIMoveType.MoveRanged)
                                                        {
                                                            value = pathStepList.PositionToMovePoints(0);
                                                            if (value < move.UnitHexSprite.Unit.MovePoints)
                                                            {
                                                                move.UnitHexSprite.Unit.MovePoints = value;
                                                            }
                                                        }

                                                        value = pathStepList.MovePointsToPosition(move.UnitHexSprite.Unit.MovePoints);
                                                        value = pathStepList.PositionToMovePoints(value);
                                                        if (value > 0 && value < move.UnitHexSprite.Unit.MovePoints)
                                                        {
                                                            move.UnitHexSprite.Unit.MovePoints = value;
                                                        }

                                                        // { Original repeats X. }
                                                        Creator.GetEngine().OutputDebugString($"AI moves to X: {move.X.ToString(CultureInfo.InvariantCulture)} Y: {move.X.ToString(CultureInfo.InvariantCulture)}");
                                                        moved = TacticalBattleUnit.MoveCombatUnit(move.UnitHexSprite.Unit, path: pathStepList);
                                                        if (move.UnitHexSprite.Unit.CombatPlayer == CombatMap.Current.SeatedPlayer)
                                                        {
                                                            int unitHexSpriteHexY = move.UnitHexSprite.HexY;
                                                            int hexX = move.UnitHexSprite.HexX;
                                                            CombatMap.Current.CenterViewTo(hexX, unitHexSpriteHexY, 0);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        move.UnitHexSprite.Unit.MovePoints = 0;
                                                    }
                                                }
                                            }
                                        }

                                        settings?.Dispose();
                                    }

                                    break;
                                case BattleAIMoveType.Melee:
                                    settings = new CombatMoveControlSettings(null) { Holder = Creator.CreationHolder() };
                                    int hexY2 = move.UnitHexSprite.HexY;
                                    settings.SetSourcePosition(move.UnitHexSprite.HexX, hexY2, 0);
                                    settings.SetDestinationPosition(move.X, move.Y, 0);
                                    settings.ArmyHexSprite = move.UnitHexSprite;
                                    settings.MoveFlags = CombatMoveModes.Melee;
                                    MovePointTable movePointTable5 = settings.MovePointTable;
                                    move.UnitHexSprite.Unit.Troop.CreateMovePointTable(out movePointTable5);
                                    settings.MovePointTable = movePointTable5;
                                    settings.Player = move.UnitHexSprite.Unit.GetPlayer();
                                    CombatMap.Current.MoveControl.ProcessMove(settings);
                                    if (settings.Path.Count > 1)
                                    {
                                        pathStepList = settings.Path;
                                        pathCost = MakeTerrainPath(settings, path: pathStepList, value: ref terrainCost, move.UnitHexSprite.Unit, ignore: null, freeStrikes: false, findWalls: false, cost: 1);
                                        if (pathStepList.Count > 1 && pathCost > -1)
                                        {
                                            if (pathStepList[^2].MoveCost <= move.UnitHexSprite.Unit.MovePoints)
                                            {
                                                // { Original repeats X in the debug text. }
                                                Creator.GetEngine().OutputDebugString($"AI touches at X: {move.X.ToString(CultureInfo.InvariantCulture)} Y: {move.X.ToString(CultureInfo.InvariantCulture)}");
                                                moved = move.UnitHexSprite.MeleeMoveTacticalBattle(pathStepList);
                                                if (move.UnitHexSprite.Unit.CombatPlayer == CombatMap.Current.SeatedPlayer)
                                                {
                                                    int hexY = move.UnitHexSprite.HexY;
                                                    int hexX = move.UnitHexSprite.HexX;
                                                    CombatMap.Current.CenterViewTo(hexX, hexY, 0);
                                                }
                                            }
                                        }
                                    }

                                    settings?.Dispose();
                                    break;
                                case BattleAIMoveType.Touch:
                                    GamePack.Current.AbilityControl[move.Ability].GetCombatInfo(move.UnitHexSprite.Unit.Troop, info: ref aiInfo);
                                    settings = new CombatMoveControlSettings(null) { Holder = Creator.CreationHolder() };
                                    int hexY3 = move.UnitHexSprite.HexY;
                                    settings.SetSourcePosition(move.UnitHexSprite.HexX, hexY3, 0);
                                    settings.SetDestinationPosition(move.X, move.Y, 0);
                                    settings.ArmyHexSprite = move.UnitHexSprite;
                                    settings.MoveFlags = CombatMoveModes.Melee;
                                    if (aiInfo.DamageTypes.Contains(DamageTypes.Wall))
                                    {
                                        settings.MoveFlags |= CombatMoveModes.WallCrushing;
                                    }

                                    MovePointTable movePointTable6 = settings.MovePointTable;
                                    move.UnitHexSprite.Unit.Troop.CreateMovePointTable(out movePointTable6);
                                    settings.MovePointTable = movePointTable6;
                                    settings.Player = move.UnitHexSprite.Unit.GetPlayer();
                                    move.UnitHexSprite.SelectedAbility = move.Ability;
                                    CombatMap.Current.MoveControl.ProcessMove(settings);
                                    if (settings.Path.Count > 1)
                                    {
                                        pathStepList = settings.Path;
                                        pathCost = MakeTerrainPath(settings, path: pathStepList, value: ref terrainCost, move.UnitHexSprite.Unit, ignore: null, freeStrikes: false, findWalls: false, cost: 1);
                                        if (pathStepList.Count > 1 && pathCost > -1)
                                        {
                                            if (pathStepList[^2].MoveCost <= move.UnitHexSprite.Unit.MovePoints)
                                            {
                                                move.UnitHexSprite.SelectedAbility = move.Ability;
                                                // { Original repeats X in the debug text. }
                                                Creator.GetEngine().OutputDebugString($"AI melees at X: {move.X.ToString(CultureInfo.InvariantCulture)} Y: {move.X.ToString(CultureInfo.InvariantCulture)}");
                                                moved = move.UnitHexSprite.AbilityTouchMoveTacticalBattle(pathStepList);
                                                if (move.UnitHexSprite.Unit.CombatPlayer == CombatMap.Current.SeatedPlayer)
                                                {
                                                    int hexY = move.UnitHexSprite.HexY;
                                                    int hexX = move.UnitHexSprite.HexX;
                                                    CombatMap.Current.CenterViewTo(hexX, hexY, 0);
                                                }
                                            }
                                        }
                                    }

                                    settings?.Dispose();
                                    break;
                                case BattleAIMoveType.Ranged:
                                    move.UnitHexSprite.SelectedAbility = move.Ability;
                                    move.UnitHexSprite.SelectedSpell = move.Spell;
                                    // { Original repeats X in the debug text. }
                                    Creator.GetEngine().OutputDebugString($"AI ranged at X: {move.X.ToString(CultureInfo.InvariantCulture)} Y: {move.X.ToString(CultureInfo.InvariantCulture)}");
                                    moved = move.UnitHexSprite.AbilityRangedTacticalBattle(move.X, move.Y);
                                    if (move.UnitHexSprite.Unit.CombatPlayer == CombatMap.Current.SeatedPlayer)
                                    {
                                        int hexY = move.UnitHexSprite.HexY;
                                        int hexX = move.UnitHexSprite.HexX;
                                        CombatMap.Current.CenterViewTo(hexX, hexY, 0);
                                    }

                                    break;
                                case BattleAIMoveType.Fake:
                                case BattleAIMoveType.WallTouch:
                                case BattleAIMoveType.WallRanged:
                                default:
                                    break;
                            }

                            (
                            _moves[0] as IDisposable)?.Dispose();
                            _moves.RemoveAt(0);
                        }

                        if (!moved)
                        {
                            Status = AIStatus.Done;
                        }

                        i = 0;
                        int last = _moves.Count - 1;
                        for (; i <= last; i += 1)
                        {
                            (_moves[i] as IDisposable)?.Dispose();
                        }

                        _moves.Clear();
                        _currentMove = 0;
                    }
                    catch
                    {
                        _ = MessageBox(0, text: "Error - TCAI", caption: "Error during Execute Moves", flags: 0);
                    }

                    break;
                default:
                    break;
            }
        }
    }

    public enum AIStatus : byte
    {
        Idle = 0, Busy = 1, Done = 2
    }
}
