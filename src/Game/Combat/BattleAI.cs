// Translated from AoWTC.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// Side. Units. Allies. Enemies. Moves. Walls. CurrentMove. UnitIndex. UnitLimit. MinMoves.
// HeroMoves. NormalUnits. IgnoreWalls. Flee. Status
public partial class BattleAI : IDisposable
{
    private CombatSideIndex _side;
    private readonly List<CombatObject> _units;
    private readonly List<CombatObject> _allies;
    private readonly List<CombatObject> _enemies;
    private readonly List<BattleAIMove> _moves;
    private readonly List<CityWall> _walls;
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
    public virtual void Dispose()
    {
        _units?.Clear();
        _allies?.Clear();
        _enemies?.Clear();
        _walls?.Clear();
        for (int I = 0, last = _moves.Count - 1; I <= last; I++)
        {
            (_moves[I] as IDisposable)?.Dispose();
        }

        _moves?.Clear();
    }

    // AoWTC.pas:752
    public BattleAI(EngineObject creator)
    {
        Creator = creator;
        _units = new List<CombatObject>();
        _allies = new List<CombatObject>();
        _enemies = new List<CombatObject>();
        _moves = new List<BattleAIMove>();
        _walls = new List<CityWall>();
    }

    // The node that created this battle AI. It and the nodes it creates reach the engine through it.
    public EngineObject Creator { get; }

    // AoWTC.pas:774
    public void Reset()
    {
        Status = AIStatus.Idle;
        _currentMove = 0;
    }

    // AoWTC.pas:780
    public void BeginTurn()
    {
        EvalBattle();
    }

    // AoWTC.pas:785
    public void ContinueTurn()
    {
        EvalBattle();
    }

    // AoWTC.pas:790
    public void CreateMove(BattleAIMoveType moveType, int X, int Y, int value, int ability, int spell, TacticalBattleUnit U, CombatObject target)
    {
        BattleAIMove M;
        M = new BattleAIMove();
        M.UnitHexSprite = U.UnitHexSprite;
        M.Target = target;
        M.MoveType = moveType;
        M.Ability = (short)ability;
        M.Spell = (short)spell;
        M.X = (ushort)X;
        M.Y = (ushort)Y;
        M.Value = value;
        _moves.Add(M);
    }

    // AoWTC.pas:805
    public int TacticalBattleDamageValueToDamageEvaluation(int damageValue1, int damageValue2, CombatObject source, CombatObject target)
    {
        int strength;
        strength = target.GetTargetStrength();
        if (target is TacticalBattleUnit)
        {
            if (((CombatUnit)target).Troop is Hero)
            {
                strength = strength * 2;
            }

            if (((CombatUnit)target).Troop is Leader)
            {
                strength = strength * 2;
            }
        }

        int result = strength * damageValue1;
        strength = source.GetTargetStrength();
        if (source is TacticalBattleUnit)
        {
            if (((CombatUnit)source).Troop is Hero)
            {
                strength = strength * 2;
            }

            if (((CombatUnit)source).Troop is Leader)
            {
                strength = strength * 2;
            }
        }

        result = result - strength * damageValue2;
        return result / 100;
    }

    // AoWTC.pas:823
    public bool ChargeFromPosition(int position)
    {
        bool result;
        int J;
        int empty;
        if (position == 0)
        {
            result = false;
            empty = 0;
            if (_side == CombatSideIndex.Defender && CombatMap.Current.Combat.CombatData.WallType != 0 && !CombatMap.Current.Combat.CombatData.WallBreached)
            {
                result = true;
            }
            else
            {
                for (int I = 1; I <= 6; I++)
                {
                    if (CombatMap.Current.PositionStatus[I] == 1)
                    {
                        J = I - 1;
                        if (J == 0)
                        {
                            J = 6;
                        }

                        if (CombatMap.Current.PositionStatus[J] == 0)
                        {
                            result = true;
                        }

                        J = I + 1;
                        if (J == 7)
                        {
                            J = 1;
                        }

                        if (CombatMap.Current.PositionStatus[J] == 0)
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        if (CombatMap.Current.PositionStatus[I] == 0)
                        {
                            empty += 1;
                        }
                    }
                }

                if (empty > 1)
                {
                    result = true;
                }
            }
        }
        else
        {
            result = true;
        }

        return result;
    }

    // AoWTC.pas:849
    public int UnitSafe(TacticalBattleUnit U, int X, int Y)
    {
        int result;
        int distance;
        if (!(U.Troop is Hero) || (_units.Count == 1 && _side == CombatSideIndex.Defender))
        {
            result = 0;
        }
        else
        {
            result = U.GetTargetStrength() * 20 * U.GetHitPoints() / U.GetHits();
            if (U.Troop is Leader)
            {
                result = result / 2;
            }

            for (int I = 0, last = _enemies.Count - 1; I <= last; I++)
            {
                var hexY = ((TacticalBattleUnit)_enemies[I]).UnitHexSprite.GetHexY();
                distance = HexGeometry.DeltaHexToRadius(X, Y, ((TacticalBattleUnit)_enemies[I]).UnitHexSprite.GetHexX(), hexY);
                if (distance < 15 && (_enemies[I]).GetTargetStrength() > 0)
                {
                    result = result - (_enemies[I]).GetTargetStrength() * (15 - distance);
                }
            }

            for (int I = 0, last = _units.Count - 1; I <= last; I++)
            {
                if ((TacticalBattleUnit)_units[I] != U)
                {
                    var hexY = ((TacticalBattleUnit)_units[I]).UnitHexSprite.GetHexY();
                    distance = HexGeometry.DeltaHexToRadius(X, Y, ((TacticalBattleUnit)_units[I]).UnitHexSprite.GetHexX(), hexY);
                    if (distance < 15 && (_units[I]).GetTargetStrength() > 0)
                    {
                        result = result + (_units[I]).GetTargetStrength() * (15 - distance);
                    }
                }
            }

            for (int I = 0, last = _allies.Count - 1; I <= last; I++)
            {
                var hexY = ((TacticalBattleUnit)_allies[I]).UnitHexSprite.GetHexY();
                distance = HexGeometry.DeltaHexToRadius(X, Y, ((TacticalBattleUnit)_allies[I]).UnitHexSprite.GetHexX(), hexY);
                if (distance < 15 && (_allies[I]).GetTargetStrength() > 0)
                {
                    result = result + (_allies[I]).GetTargetStrength() * (15 - distance);
                }
            }
        }

        return result;
    }

    // AoWTC.pas:880
    // The tactical AI passes the wall as Ignore. The original casts that object to TTacticalCombatUnit
    // without a check and compares the reference with the sprite's unit. Ignore is a combat object so the
    // wall reaches the same comparison.
    public int MakeTerrainPath(CombatMoveControlSettings moveControlSettings, ref PathStepList path, ref int value, TacticalBattleUnit U, CombatObject ignore, bool freeStrikes, bool findWalls, int cost)
    {
        int result;
        int J;
        int X = default;
        int Y = default;
        TacticalBattleUnitHexSprite hexSprite;
        CityWall wall;
        bool newWall;
        if (path.Count < 2)
        {
            result = -1;
        }
        else
        {
            result = 0;
        }

        try
        {
            value = 0;
            if (result != -1)
            {
                for (int I = 0, last = path.Count - 1; I <= last; I++)
                {
                    hexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].Field[path.List[I].Location.X, path.List[I].Location.Y].FindOwnedChild(2228484);
                    if (freeStrikes && (hexSprite == null || hexSprite == U.UnitHexSprite && I - 1 >= 0 && CombatMap.Current.MapContainer.MapLevels[0].Field[path.List[I - 1].Location.X, path.List[I - 1].Location.Y].FindOwnedChild(2228484) == null))
                    {
                        J = 1;
                        for (; J <= 6; J += 1)
                        {
                            HexGeometry.CenterHexNumberToHex(path.List[I].Location.X, path.List[I].Location.Y, J, ref X, ref Y);
                            hexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2228484);
                            if (hexSprite != null && hexSprite.Unit != ignore && hexSprite.Unit.GetSide() != U.GetSide() && CombatObject.MeleeDamageTypes(hexSprite.Unit) != Set<DamageType>.Of() && !hexSprite.Unit.GetLocked() && hexSprite.Unit.GetAbilityEnabled(1) == U.GetAbilityEnabled(1))
                            {
                                var tacticalBattleDamageValueToDamageEvaluation2 = TacticalBattleDamageValueToDamageEvaluation(CombatObject.StrikeDamageValue(hexSprite.Unit, U), 0, hexSprite.Unit, U);
                                value = value - tacticalBattleDamageValueToDamageEvaluation2 * 10;
                            }
                        }
                    }

                    if (I < path.Count - 1)
                    {
                        if (CombatMap.Current.Combat.CombatData.WallType != 0)
                        {
                            var pathY = path[I];
                            var pathX = path[I];
                            wall = (CityWall)CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y].FindOwnedChild(2229508);
                            if (wall == null)
                            {
                                var pathLocal = path[I];
                                var path2 = path[I];
                                wall = CombatMap.Current.MapContainer.MapLevels[0].Field[path2.Location.X, pathLocal.Location.Y].FindOwnedChild(2229510) as CityWall;
                            }
                        }
                        else
                        {
                            wall = null;
                        }

                        var path3 = path[I];
                        var path4 = path[I];
                        var mapField = CombatMap.Current.MapContainer.MapLevels[0].Field[path4.Location.X, path3.Location.Y];
                        var path5 = path[I];
                        var path6 = path[I];
                        var fieldItem = CombatMap.Current.MapContainer.MapLevels[0].Field[path6.Location.X, path5.Location.Y];
                        path.List[I] = path.List[I] with { MoveCost = moveControlSettings.MovePointTable[fieldItem.TerrainType][mapField.OverlayType - MovementTables.FirstOverlay] };
                        if (wall != null)
                        {
                            // { The wall lookup uses the current path point and the no-overlay column. }
                            if (_side == CombatSideIndex.Attacker && (U.GetAbilityEnabled(65) || U.GetAbilityEnabled(117)) && I == 0)
                            {
                                var pathY = path[I];
                                var pathX = path[I];
                                var item = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y];
                                path.List[I] = path.List[I] with { MoveCost = moveControlSettings.MovePointTable[item.TerrainType][-1 - MovementTables.FirstOverlay] };
                            }
                            else
                            {
                                if ((_side == CombatSideIndex.Attacker || CombatMap.Current.Combat.CombatData.WallBreached || _ignoreWalls) && (wall.HitPoints == 0 || (_side == CombatSideIndex.Defender && wall is CityWallDoor) || CombatMap.Current.MoveControl.UnitHexSprite.Unit.GetAbilityEnabled(1) || CombatMap.Current.MoveControl.UnitHexSprite.Unit.GetAbilityEnabled(66) || CombatMap.Current.MoveControl.UnitHexSprite.Unit.GetAbilityEnabled(36)))
                                {
                                    var pathY = path[I];
                                    var pathX = path[I];
                                    var item2 = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y];
                                    path.List[I] = path.List[I] with { MoveCost = moveControlSettings.MovePointTable[item2.TerrainType][-1 - MovementTables.FirstOverlay] };
                                }
                                else
                                {
                                    path.List[I] = path.List[I] with { MoveCost = 100 };
                                }
                            }

                            if (findWalls && wall.HitPoints > 0)
                            {
                                newWall = true;
                                J = 0;
                                while (newWall && J < _walls.Count)
                                {
                                    if (_walls[J] == wall)
                                    {
                                        newWall = false;
                                    }
                                    else
                                    {
                                        J += 1;
                                    }
                                }

                                if (newWall)
                                {
                                    _walls.Add(wall);
                                }
                            }
                        }

                        var path7 = path[I];
                        var path8 = path[I];
                        if (CombatMap.Current.MapContainer.MapLevels[0].Field[path8.Location.X, path7.Location.Y].FindOwnedChild(2228484) != null)
                        {
                            var pathY = path[I];
                            var pathX = path[I];
                            var item2 = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y];
                            path.List[I] = path.List[I] with { MoveCost = moveControlSettings.MovePointTable[item2.TerrainType][-1 - MovementTables.FirstOverlay] };
                        }
                    }

                    result = result + path.List[I].MoveCost;
                }
            }
        }
        catch
        {
            MessageBox(0, "Error during Eval Movepath", NullTerminatedText("Error - TCAI " + U.Troop.GetName()), 0);
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
                    result = result * multiplier;
                }

                if (divider != 1 && divider != 0)
                {
                    result = result / divider;
                }
            }
            catch
            {
                try
                {
                    if (divider != 1 && divider != 0)
                    {
                        result = result / divider;
                    }

                    if (multiplier != 1)
                    {
                        result = result * multiplier;
                    }
                }
                catch
                {
                    result = 1000000;
                }
            }
        }
        else
        {
            try
            {
                if (divider != 1)
                {
                    result = result * divider;
                }

                if (multiplier != 1 && multiplier != 0)
                {
                    result = result / multiplier;
                }
            }
            catch
            {
                try
                {
                    if (multiplier != 1 && multiplier != 0)
                    {
                        result = result / multiplier;
                    }

                    if (divider != 1)
                    {
                        result = result * divider;
                    }
                }
                catch
                {
                    result = -1000000;
                }
            }
        }

        if (result > 1000000)
        {
            result = 1000000;
        }

        if (result < -1000000)
        {
            result = -1000000;
        }

        return result;
    }

    // AoWTC.pas:980
    public void EvalAll(ref int value, List<CombatObject> list, CombatObject ignore, int spell, TacticalBattleUnit U)
    {
        CombatObject target;
        AbilityDamageValues damageValue = default;
        for (int I = 0, last = list.Count - 1; I <= last; I++)
        {
            if (list[I] != ignore)
            {
                target = list[I];
                GamePack.Current.SpellControl.GetSpell(spell).GetCombatDamageValues(U, target, ref damageValue);
                if (damageValue.SourceDamage > 0)
                {
                    switch (spell)
                    {
                        case 102:
                            if (target.GetSide() != _side)
                            {
                                var tacticalBattleDamageValueToDamageEvaluation2 = TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, U, target);
                                value = value - tacticalBattleDamageValueToDamageEvaluation2;
                            }
                            else
                            {
                                var tacticalBattleDamageValueToDamageEvaluation2 = TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, U, target);
                                value = value + tacticalBattleDamageValueToDamageEvaluation2;
                            }

                            break;
                        default:
                            if (target.GetSide() != _side)
                            {
                                var hitPoints = target.GetHitPoints();
                                var hits = target.GetHits();
                                var multDamageEvaluation = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, U, target), hits, hitPoints);
                                value = value + multDamageEvaluation;
                            }
                            else
                            {
                                var hitPoints = target.GetHitPoints();
                                var hits = target.GetHits();
                                var multDamageEvaluation = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, U, target), hits, hitPoints);
                                value = value - multDamageEvaluation;
                            }

                            break;
                    }
                }
            }
        }
    }

    // AoWTC.pas:1007
    public void EvalHex(CombatObject ignore, int ability, int spell, int X, int Y, ref int value, bool hitWalls, bool friendly, TacticalBattleUnit U, object ignoreWall)
    {
        TacticalBattleUnitHexSprite hexSprite;
        CityWall wall;
        AbilityDamageValues damageValue = default;
        hexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2228484);
        if (hexSprite != null && hexSprite.Unit != ignore && (friendly || hexSprite.Unit.GetSide() != U.GetSide()))
        {
            if (ability == 52)
            {
                GamePack.Current.SpellControl.GetSpell(spell).GetCombatDamageValues(U, hexSprite.Unit, ref damageValue);
            }
            else
            {
                if (TacticalTables.AbilityTypes[ability] == BattleEffectType.Touch)
                {
                    GamePack.Current.AbilityControl.GetAbility(ability).TacticalBattleGetDamageValues(U, hexSprite.Unit, ref damageValue);
                }
                else
                {
                    GamePack.Current.AbilityControl.GetAbility(ability).TacticalBattleGetDamageValues(U, hexSprite.Unit, ref damageValue);
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
                                if (hexSprite.Unit.GetSide() != U.GetSide())
                                {
                                    var tacticalBattleDamageValueToDamageEvaluation2 = TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, U, hexSprite.Unit);
                                    value = value + tacticalBattleDamageValueToDamageEvaluation2;
                                }
                                else
                                {
                                    var tacticalBattleDamageValueToDamageEvaluation2 = TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, U, hexSprite.Unit);
                                    value = value - tacticalBattleDamageValueToDamageEvaluation2;
                                }

                                break;
                            default:
                                if (hexSprite.Unit.GetSide() != U.GetSide())
                                {
                                    var hitPoints = hexSprite.Unit.GetHitPoints();
                                    var hits = hexSprite.Unit.GetHits();
                                    var multDamageEvaluation = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, U, hexSprite.Unit), hits, hitPoints);
                                    value = value + multDamageEvaluation;
                                }
                                else
                                {
                                    var hitPoints = hexSprite.Unit.GetHitPoints();
                                    var hits = hexSprite.Unit.GetHits();
                                    var multDamageEvaluation = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, U, hexSprite.Unit), hits, hitPoints);
                                    value = value - multDamageEvaluation;
                                }

                                break;
                        }

                        break;
                    default:
                        if (hexSprite.Unit.GetSide() != U.GetSide())
                        {
                            var hitPoints = hexSprite.Unit.GetHitPoints();
                            var hits = hexSprite.Unit.GetHits();
                            var multDamageEvaluation = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, U, hexSprite.Unit), hits, hitPoints);
                            value = value + multDamageEvaluation;
                        }
                        else
                        {
                            var hitPoints = hexSprite.Unit.GetHitPoints();
                            var hits = hexSprite.Unit.GetHits();
                            var multDamageEvaluation = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, U, hexSprite.Unit), hits, hitPoints);
                            value = value - multDamageEvaluation;
                        }

                        break;
                }
            }
        }

        if (hitWalls && CombatMap.Current.Combat.CombatData.WallType != 0)
        {
            wall = (CityWall)CombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2229508);
            if (wall == null)
            {
                wall = CombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2229510) as CityWall;
            }

            if (!CombatMap.Current.Combat.CombatData.WallBreached && ignoreWall != null && wall != null && (ignore == null || !(ignore is CombatWall) || (ignore is CombatWall && ignoreWall != wall)))
            {
                if (ability == 52)
                {
                    GamePack.Current.SpellControl.GetSpell(spell).GetCombatDamageValues(U, CombatMap.Current.Combat.CombatData.Wall, ref damageValue);
                }
                else
                {
                    if (TacticalTables.AbilityTypes[ability] == BattleEffectType.Touch)
                    {
                        GamePack.Current.AbilityControl.GetAbility(ability).TacticalBattleGetDamageValues(U, CombatMap.Current.Combat.CombatData.Wall, ref damageValue);
                    }
                    else
                    {
                        GamePack.Current.AbilityControl.GetAbility(ability).TacticalBattleGetDamageValues(U, CombatMap.Current.Combat.CombatData.Wall, ref damageValue);
                    }
                }

                if (ability == 65)
                {
                    damageValue.TargetDamage = 0;
                }

                if (damageValue.SourceDamage > 0)
                {
                    if (U.GetSide() == CombatSideIndex.Attacker)
                    {
                        var multDamageEvaluation = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, U, CombatMap.Current.Combat.CombatData.Wall), TacticalTables.WallMaxHitPoints[wall.WallType - 1], wall.HitPoints);
                        value = value + multDamageEvaluation;
                    }
                    else
                    {
                        var multDamageEvaluation = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue.SourceDamage, 0, U, CombatMap.Current.Combat.CombatData.Wall), TacticalTables.WallMaxHitPoints[wall.WallType - 1], wall.HitPoints);
                        value = value - multDamageEvaluation;
                    }
                }
            }
        }
    }

    // AoWTC.pas:1072
    public void EvalArea(ref int value, CombatObject ignore, int ability, int spell, int X, int Y, bool hitWalls, bool includeCenter, bool friendly, TacticalBattleUnit U, object ignoreWall)
    {
        int x1 = default;
        int y1 = default;
        for (int I = 0; I <= 6; I++)
        {
            HexGeometry.CenterHexNumberToHex(X, Y, I, ref x1, ref y1);
            if (includeCenter || U.UnitHexSprite.GetHexX() != x1 || U.UnitHexSprite.GetHexY() != y1)
            {
                EvalHex(ignore, ability, spell, x1, y1, ref value, hitWalls, friendly, U, ignoreWall);
            }
        }
    }

    // AoWTC.pas:1084
    public int EvalPath(ref int value, CombatObject ignore, int ability, int spell, int x1, int y1, int x2, int y2, bool hitWalls, TacticalBattleUnit U, object ignoreWall)
    {
        int result;
        Point3DList Path;
        DelphiList<RangedObstacle> obstacles;
        int X = default;
        int Y = default;
        int V = default;
        int total;
        int original;
        int chance;
        BattleEffectType effect;
        if (ability == 52)
        {
            effect = TacticalTables.SpellTypes[spell];
        }
        else
        {
            effect = TacticalTables.AbilityTypes[ability];
        }

        switch (effect)
        {
            case BattleEffectType.Arrow:
            case BattleEffectType.Rock:
            case BattleEffectType.Comet:
            case BattleEffectType.Breath:
            case BattleEffectType.CometBall:
            case BattleEffectType.Ray:
            case BattleEffectType.AltArrow:
                Path = new Point3DList();
                obstacles = new DelphiList<RangedObstacle>();
                if (effect == BattleEffectType.Breath)
                {
                    TacticalTables.MakeRangedPath(U.GetSide(), 50, 20, 50, (short)x1, (short)y1, (short)x2, (short)y2, 0, 0, false, ref Path, ref obstacles);
                }
                else
                {
                    TacticalTables.MakeAbilityPath(U.GetSide(), (short)ability, (short)spell, (short)x1, (short)y1, (short)x2, (short)y2, ref Path, ref obstacles);
                }

                total = 0;
                original = value;
                chance = 100;
                {
                    for (int I = 0, last = obstacles.Count - 1; I <= last; I++)
                    {
                        var obstacle = obstacles[I];
                        var path = Path[obstacle.PathIndex];
                        var obstacle2 = obstacles[I];
                        HexGeometry.PixelToHex(Path[obstacle2.PathIndex].X, path.Y, out X, out Y);
                        V = 0;
                        EvalHex(ignore, ability, spell, X, Y, ref V, hitWalls, true, U, ignoreWall);
                        try
                        {
                            var obstacle3 = obstacles[I];
                            total = total + V * chance / 100 * obstacle3.Chance / 100;
                        }
                        catch
                        {
                            total = total + obstacles[I].Chance * (V / 100 * chance / 100);
                        }

                        chance = (100 - obstacles[I].Chance) * chance / 100;
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

                value = value + total;
                Path?.Dispose();
                {
                    for (int I = 0, last = obstacles.Count - 1; I <= last; I++)
                    {
                        (obstacles[I] as IDisposable)?.Dispose();
                    }
                }

                obstacles?.Dispose();
                result = original - value;
                break;
            default:
                result = 0;
                break;
        }

        return result;
    }

    // AoWTC.pas:1130
    public int EvalBreath(ref int value, CombatObject ignore, int ability, int spell, int x1, int y1, int x2, int y2, TacticalBattleUnit U)
    {
        int V = default;
        int X = default;
        int Y = default;
        var hexX = U.UnitHexSprite.GetHexX();
        var hexY = U.UnitHexSprite.GetHexY();
        int result = EvalPath(ref value, ignore, ability, spell, hexX, hexY, x2, y2, false, U, null);
        for (int I = 0; I <= 11; I++)
        {
            V = 0;
            var deltaHexToHexNumber2 = HexGeometry.DeltaHexToHexNumber(x1, y1, x2, y2);
            HexGeometry.CenterHexNumberToHex(x1, y1, TacticalTables.BreathHexNumber[TacticalTables.BreathDirection[deltaHexToHexNumber2 - 1] * 12 + I - 12], ref X, ref Y);
            EvalHex(ignore, ability, spell, X, Y, ref V, true, true, U, null);
            if (V != 0)
            {
                var fUnitHexSpriteHexX = U.UnitHexSprite.GetHexX();
                var fUnitHexSpriteHexY = U.UnitHexSprite.GetHexY();
                EvalPath(ref V, ignore, ability, spell, fUnitHexSpriteHexX, fUnitHexSpriteHexY, X, Y, false, U, null);
            }

            value = value + V;
            result = result - V;
        }

        return result;
    }

    // AoWTC.pas:1149
    public void CheckUnit(int X, int Y, int hits, int hitPoints, CombatMoveControlSettings settings, TacticalBattleUnit U, CombatObject target, DelphiList<CombatAbilityRef> abilities, object wall)
    {
        int safeValue;
        int I;
        int V;
        int distance;
        int range;
        int cost = default;
        int bestIndex;
        int bestValue;
        int D = default;
        int hexX = default;
        int hexY = default;
        int originalX;
        int originalY;
        int x = default;
        int y = default;
        int pathCost;
        int terrainValue;
        int pathValue;
        int ability;
        int spell;
        int value = default;
        PathStepList Path;
        PathStepList DetourPath;
        PathStepList basePath;
        MeleeRound round;
        AbilityCombatInfo abilityInfo = default;
        SpellCombatInfo spellInfo = default;
        CityWall cityWall;
        bool adjacent;
        AbilityDamageValues values = default;
        Path = new MovePath();
        basePath = new MovePath();
        DetourPath = new MovePath();
        terrainValue = 0;
        originalX = X;
        originalY = Y;
        var yhx = U.UnitHexSprite.GetHexY();
        distance = HexGeometry.DeltaHexToRadius(U.UnitHexSprite.GetHexX(), yhx, X, Y);
        if (U.MovePoints > 0)
        {
            CombatMap.Current.MoveControl.CalculatePath(settings, (MovePath)basePath, (sbyte)X, (sbyte)Y, 0);
            pathCost = MakeTerrainPath(settings, ref basePath, ref terrainValue, U, target, true, target is TacticalBattleUnit && _side == CombatSideIndex.Attacker, 1);
            I = 0;
            int last = basePath.Count - 1;
            for (; I <= last; I += 1)
            {
                Path.Add(basePath[I]);
            }
        }
        else
        {
            pathCost = distance * 4;
        }

        if (pathCost != -1)
        {
            try
            {
                if (target is CombatUnit && target.GetSide() != U.GetSide() && CombatObject.MeleeDamageTypes(U) != Set<DamageType>.Of() && (U.GetAbilityEnabled(1) || !target.GetAbilityEnabled(1)))
                {
                    ability = 0;
                    spell = 0;
                    round = new MeleeRound(Creator);
                    round.Calculate(U, target);
                    if (round.GetAttackerDamageValue() > 0)
                    {
                        adjacent = ((MovePath)Path).MovePointsToPosition(U.MovePoints) == 0;
                        U.CanFlee = false;
                        if (target is CombatWall)
                        {
                            _flee = false;
                        }

                        var defenderDamageValue = round.GetDefenderDamageValue();
                        value = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(round.GetAttackerDamageValue(), defenderDamageValue, U, target), hitPoints, hits) + terrainValue;
                        if (target.GetTargetStrength() <= 0 && _enemies.Count > 1)
                        {
                            value -= 10000;
                        }

                        cost = (pathCost - U.MovePoints) * 10;
                        if (cost < 1)
                        {
                            cost = 1;
                        }

                        if (!(U.Troop is Hero))
                        {
                            safeValue = 0;
                        }
                        else
                        {
                            I = -1;
                            bestIndex = ((MovePath)Path).MovePointsToPosition(U.MovePoints);
                            var pathY = Path[bestIndex];
                            safeValue = UnitSafe(U, Path[bestIndex].Location.X, pathY.Location.Y);
                            while (bestIndex < Path.Count - 1 && safeValue < 0)
                            {
                                var path = Path[bestIndex];
                                safeValue = UnitSafe(U, Path[bestIndex].Location.X, path.Location.Y);
                                if (safeValue >= 0)
                                {
                                    I = bestIndex;
                                }

                                bestIndex += 1;
                            }

                            if (I > -1)
                            {
                                V = 0;
                                for (; V <= I - 1; V += 1)
                                {
                                    Path.Delete(0);
                                }
                            }
                        }

                        if (Path.Count > 1)
                        {
                            if ((safeValue >= 0 || _normalUnits == 0) && adjacent)
                            {
                                if (value > 0 || _normalUnits == 0 || !(U.Troop is Hero) || _units.Count <= 1)
                                {
                                    _heroMoves += 1;
                                    if (U.Troop is Hero)
                                    {
                                        I = 50;
                                    }
                                    else
                                    {
                                        I = 100;
                                    }

                                    CreateMove(BattleAIMoveType.Melee, X, Y, ScaleDamageEvaluation(value, I, 1), ability, spell, U, target);
                                }
                            }
                            else
                            {
                                if (_ignoreWalls || ChargeFromPosition((int)(((uint)U.Position) >> 4)))
                                {
                                    if (!((safeValue < 0 && _normalUnits != 0) || U.MovePoints <= 0 || (Path.List[Path.Count - 2].MoveCost > U.MovePoints && Path.List[Path.Count - 2].MoveCost <= 15)))
                                    {
                                        _heroMoves += 1;
                                        var moves = U.GetMoves();
                                        var multDamageEvaluation = ScaleDamageEvaluation(value, moves, cost);
                                        var path = Path[0];
                                        CreateMove(BattleAIMoveType.Move, Path[0].Location.X, path.Location.Y, multDamageEvaluation, ability, spell, U, target);
                                    }
                                    else
                                    {
                                        var moves = U.GetMoves();
                                        CreateMove(BattleAIMoveType.Fake, X, Y, ScaleDamageEvaluation(value, moves, cost), ability, spell, U, target);
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
                MessageBox(0, "Error during Melee Check", "Error - TCAI ", 0);
            }

            for (int abilityIndex = 0, last = abilities.Count - 1; abilityIndex <= last; abilityIndex++)
            {
                Path.Clear();
                I = 0;
                int limit = basePath.Count - 1;
                for (; I <= limit; I += 1)
                {
                    Path.Add(basePath[I]);
                }

                ability = abilities[abilityIndex].Ability;
                spell = 0;
                X = originalX;
                Y = originalY;
                GamePack.Current.AbilityControl.GetAbility(ability).GetCombatInfo(U.Troop, ref abilityInfo);
                switch (ability)
                {
                    case 52:
                        try
                        {
                            if (!(target is CombatWall) || !CombatMap.Current.Combat.CombatData.WallBreached)
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
                                        GamePack.Current.SpellControl.GetSpell(spell).GetCombatInfo(U.Troop, ref spellInfo);
                                        GamePack.Current.SpellControl.GetSpell(spell).GetCombatDamageValues(U, target, ref values);
                                        if (values.SourceDamage > 0)
                                        {
                                            U.CanFlee = false;
                                            if (target is CombatWall)
                                            {
                                                _flee = false;
                                            }

                                            pathValue = 0;
                                            range = spellInfo.Range;
                                            switch (spell)
                                            {
                                                case 108:
                                                case 114:
                                                    range = 4;
                                                    break;
                                            }

                                            if (range == -1)
                                            {
                                                range = 100;
                                            }

                                            switch (spell)
                                            {
                                                case 8:
                                                case >= 15 and <= 17:
                                                case 32:
                                                case 33:
                                                case 36:
                                                case 37:
                                                case 40:
                                                case 42:
                                                case 43:
                                                case 49:
                                                case 56:
                                                case 77:
                                                case 102:
                                                case 106:
                                                case 110:
                                                case 111:
                                                    value = TacticalBattleDamageValueToDamageEvaluation(values.SourceDamage, 0, U, target);
                                                    break;
                                                default:
                                                    value = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(values.SourceDamage, 0, U, target), hitPoints, hits);
                                                    break;
                                            }

                                            if (target.GetTargetStrength() <= 0 && _enemies.Count > 1)
                                            {
                                                value -= 10000;
                                            }

                                            if (U.MovePoints > 0)
                                            {
                                                I = ((MovePath)Path).MovePointsToPosition(U.MovePoints);
                                                var pathY = Path[I];
                                                D = HexGeometry.DeltaHexToRadius(Path[I].Location.X, pathY.Location.Y, X, Y);
                                                if (D <= range || distance <= range)
                                                {
                                                    cost = value;
                                                    if (distance <= range)
                                                    {
                                                        switch (spell)
                                                        {
                                                            case 117:
                                                                var xhx = U.UnitHexSprite.GetHexX();
                                                                var fUnitHexSpriteHexY = U.UnitHexSprite.GetHexY();
                                                                pathValue = EvalPath(ref value, target, ability, spell, xhx, fUnitHexSpriteHexY, X, Y, true, U, wall);
                                                                EvalArea(ref value, target, ability, spell, X, Y, true, true, true, U, wall);
                                                                bestIndex = X;
                                                                bestValue = Y;
                                                                {
                                                                    I = 1;
                                                                    for (; I <= 6; I += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(X, Y, I, ref hexX, ref hexY);
                                                                        var hexY2 = U.UnitHexSprite.GetHexY();
                                                                        if (HexGeometry.DeltaHexToRadius(U.UnitHexSprite.GetHexX(), hexY2, hexX, hexY) <= range)
                                                                        {
                                                                            D = cost;
                                                                            var fUnitHexSpriteHexX = U.UnitHexSprite.GetHexX();
                                                                            var hexY3 = U.UnitHexSprite.GetHexY();
                                                                            V = EvalPath(ref D, target, ability, spell, fUnitHexSpriteHexX, hexY3, hexX, hexY, true, U, wall);
                                                                            EvalArea(ref D, target, ability, spell, hexX, hexY, true, true, true, U, wall);
                                                                            if (D > value)
                                                                            {
                                                                                value = D;
                                                                                pathValue = V;
                                                                                bestIndex = hexX;
                                                                                bestValue = hexY;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                X = bestIndex;
                                                                Y = bestValue;
                                                                break;
                                                            case 127:
                                                                value = 0;
                                                                cost = 0;
                                                                {
                                                                    I = 1;
                                                                    for (; I <= 36; I += 1)
                                                                    {
                                                                        var hexY2 = U.UnitHexSprite.GetHexY();
                                                                        HexGeometry.CenterHexNumberToHex(U.UnitHexSprite.GetHexX(), hexY2, I, ref hexX, ref hexY);
                                                                        EvalHex(null, ability, spell, hexX, hexY, ref value, true, true, U, wall);
                                                                    }
                                                                }

                                                                break;
                                                            case 128:
                                                                EvalHex(target, ability, spell, U.UnitHexSprite.GetHexX(), U.UnitHexSprite.GetHexY(), ref value, true, true, U, wall);
                                                                {
                                                                    I = 0;
                                                                    for (; I <= 18; I += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(X, Y, I, ref hexX, ref hexY);
                                                                        EvalHex(target, ability, spell, hexX, hexY, ref value, true, true, U, wall);
                                                                    }
                                                                }

                                                                bestIndex = X;
                                                                bestValue = Y;
                                                                {
                                                                    I = 1;
                                                                    for (; I <= 18; I += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(X, Y, I, ref hexX, ref hexY);
                                                                        var hexY2 = U.UnitHexSprite.GetHexY();
                                                                        if (HexGeometry.DeltaHexToRadius(U.UnitHexSprite.GetHexX(), hexY2, hexX, hexY) <= range)
                                                                        {
                                                                            D = cost;
                                                                            EvalHex(target, ability, spell, U.UnitHexSprite.GetHexX(), U.UnitHexSprite.GetHexY(), ref D, true, true, U, wall);
                                                                            V = 0;
                                                                            for (; V <= 18; V += 1)
                                                                            {
                                                                                HexGeometry.CenterHexNumberToHex(hexX, hexY, V, ref x, ref y);
                                                                                EvalHex(target, ability, spell, x, y, ref D, true, true, U, wall);
                                                                            }

                                                                            if (D > value)
                                                                            {
                                                                                value = D;
                                                                                bestIndex = hexX;
                                                                                bestValue = hexY;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                X = bestIndex;
                                                                Y = bestValue;
                                                                break;
                                                            case 100:
                                                            case 103:
                                                            case 119:
                                                            case 122:
                                                            case 123:
                                                                var hexX2 = U.UnitHexSprite.GetHexX();
                                                                var hexY4 = U.UnitHexSprite.GetHexY();
                                                                pathValue = EvalPath(ref value, target, ability, spell, hexX2, hexY4, X, Y, false, U, wall);
                                                                break;
                                                            case 116:
                                                                EvalArea(ref value, target, ability, spell, X, Y, true, true, true, U, wall);
                                                                bestIndex = X;
                                                                bestValue = Y;
                                                                {
                                                                    I = 1;
                                                                    for (; I <= 6; I += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(X, Y, I, ref hexX, ref hexY);
                                                                        var hexY2 = U.UnitHexSprite.GetHexY();
                                                                        if (HexGeometry.DeltaHexToRadius(U.UnitHexSprite.GetHexX(), hexY2, hexX, hexY) <= range)
                                                                        {
                                                                            D = cost;
                                                                            EvalArea(ref D, target, ability, spell, hexX, hexY, true, true, true, U, wall);
                                                                            if (D > value)
                                                                            {
                                                                                value = D;
                                                                                bestIndex = hexX;
                                                                                bestValue = hexY;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                X = bestIndex;
                                                                Y = bestValue;
                                                                break;
                                                            case 104:
                                                            case 120:
                                                                EvalArea(ref value, target, ability, spell, X, Y, false, true, true, U, wall);
                                                                bestIndex = X;
                                                                bestValue = Y;
                                                                {
                                                                    I = 1;
                                                                    for (; I <= 6; I += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(X, Y, I, ref hexX, ref hexY);
                                                                        var hexY2 = U.UnitHexSprite.GetHexY();
                                                                        if (HexGeometry.DeltaHexToRadius(U.UnitHexSprite.GetHexX(), hexY2, hexX, hexY) <= range)
                                                                        {
                                                                            D = cost;
                                                                            EvalArea(ref D, target, ability, spell, hexX, hexY, false, true, true, U, wall);
                                                                            if (D > value)
                                                                            {
                                                                                value = D;
                                                                                bestIndex = hexX;
                                                                                bestValue = hexY;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                X = bestIndex;
                                                                Y = bestValue;
                                                                break;
                                                            case 106:
                                                                EvalArea(ref value, target, ability, spell, X, Y, false, false, false, U, wall);
                                                                bestIndex = X;
                                                                bestValue = Y;
                                                                {
                                                                    I = 1;
                                                                    for (; I <= 6; I += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(X, Y, I, ref hexX, ref hexY);
                                                                        var hexY2 = U.UnitHexSprite.GetHexY();
                                                                        if (HexGeometry.DeltaHexToRadius(U.UnitHexSprite.GetHexX(), hexY2, hexX, hexY) <= range)
                                                                        {
                                                                            D = cost;
                                                                            EvalArea(ref D, target, ability, spell, hexX, hexY, false, false, false, U, wall);
                                                                            if (D > value)
                                                                            {
                                                                                value = D;
                                                                                bestIndex = hexX;
                                                                                bestValue = hexY;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                X = bestIndex;
                                                                Y = bestValue;
                                                                break;
                                                            case 112:
                                                                if (_side == CombatSideIndex.Defender && CombatMap.Current.Combat.CombatData.WallType != 0)
                                                                {
                                                                    value = -9999;
                                                                }
                                                                else
                                                                {
                                                                    EvalAll(ref value, _enemies, target, spell, U);
                                                                    EvalAll(ref value, _allies, target, spell, U);
                                                                    EvalAll(ref value, _units, target, spell, U);
                                                                }

                                                                break;
                                                            case 125:
                                                                EvalAll(ref value, _enemies, target, spell, U);
                                                                EvalAll(ref value, _allies, target, spell, U);
                                                                EvalAll(ref value, _units, target, spell, U);
                                                                break;
                                                            case 102:
                                                                EvalAll(ref value, _units, target, spell, U);
                                                                break;
                                                            case 105:
                                                                EvalAll(ref value, _enemies, target, spell, U);
                                                                break;
                                                            case 108:
                                                            case 114:
                                                                var hexX3 = U.UnitHexSprite.GetHexX();
                                                                var hexY5 = U.UnitHexSprite.GetHexY();
                                                                pathValue = EvalBreath(ref value, target, ability, spell, hexX3, hexY5, X, Y, U);
                                                                break;
                                                        }
                                                    }
                                                }

                                                if (D <= range && (pathValue > 0 || value > 0))
                                                {
                                                    if (pathValue != 0 && value < cost)
                                                    {
                                                        bestIndex = 0;
                                                        bestValue = value;
                                                        D = cost;
                                                        I = Path.Count - 2;
                                                        while (I >= 1)
                                                        {
                                                            var path = Path[I];
                                                            var pathX = Path[I];
                                                            cityWall = (CityWall)CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, path.Location.Y].FindOwnedChild(2229508);
                                                            if (cityWall == null && _side == CombatSideIndex.Attacker)
                                                            {
                                                                var path2 = Path[I];
                                                                var path3 = Path[I];
                                                                cityWall = CombatMap.Current.MapContainer.MapLevels[0].Field[path3.Location.X, path2.Location.Y].FindOwnedChild(2229510) as CityWall;
                                                            }

                                                            if (!(cityWall == null || cityWall.HitPoints <= 0 || U.GetAbilityEnabled(1) || U.GetAbilityEnabled(36) || U.GetAbilityEnabled(66)))
                                                            {
                                                                I = 1;
                                                            }
                                                            else
                                                            {
                                                                var path2 = Path[I];
                                                                var path3 = Path[I];
                                                                var path4 = Path[0];
                                                                if (HexGeometry.DeltaHexToRadius(Path[0].Location.X, path4.Location.Y, path3.Location.X, path2.Location.Y) <= range)
                                                                {
                                                                    var path5 = Path[I];
                                                                    var path6 = Path[I];
                                                                    V = EvalPath(ref cost, target, ability, spell, path5.Location.X, path6.Location.Y, X, Y, spell == 117, U, wall);
                                                                    cost = D;
                                                                    if (V < pathValue)
                                                                    {
                                                                        bestValue = bestValue + pathValue - V;
                                                                        pathValue = V;
                                                                        bestIndex = I;
                                                                    }
                                                                }
                                                            }

                                                            I -= 1;
                                                        }

                                                        if (bestIndex > 0)
                                                        {
                                                            DetourPath.Clear();
                                                            I = bestIndex;
                                                            int last2 = Path.Count - 1;
                                                            for (; I <= last2; I += 1)
                                                            {
                                                                var path = Path[I];
                                                                DetourPath.Add(path);
                                                            }

                                                            terrainValue = 0;
                                                            pathCost = MakeTerrainPath(settings, ref DetourPath, ref terrainValue, U, target, true, false, 1);
                                                            pathValue += bestValue;
                                                            pathCost = ((MovePath)Path).PositionToMovePoints(0);
                                                        }
                                                    }

                                                    bestIndex = 0;
                                                    I = 1;
                                                    int last3 = Path.Count - 2;
                                                    for (; I <= last3; I += 1)
                                                    {
                                                        var path = Path[I];
                                                        if (HexGeometry.DeltaHexToRadius(X, Y, Path[I].Location.X, path.Location.Y) <= range && ((MovePath)Path).PositionToMovePoints(I) <= U.MovePoints)
                                                        {
                                                            cost = 1000;
                                                            var pathX = Path[I];
                                                            var path2 = Path[I];
                                                            V = EvalPath(ref cost, target, ability, spell, pathX.Location.X, path2.Location.Y, X, Y, spell == 117, U, wall);
                                                            if (V == 0 && cost == 1000)
                                                            {
                                                                bestIndex = I;
                                                            }
                                                        }
                                                    }

                                                    if (bestIndex > 0)
                                                    {
                                                        I = 0;
                                                        for (; I <= bestIndex - 1; I += 1)
                                                        {
                                                            Path.Delete(0);
                                                        }
                                                    }

                                                    pathCost = ((MovePath)Path).PositionToMovePoints(0);
                                                }

                                                // { Keep move selection inside the positive movement guard. }
                                                cost = (pathCost - U.MovePoints - range * 4) * 10;
                                                if (cost < 1)
                                                {
                                                    cost = 1;
                                                }

                                                if (!(U.Troop is Hero))
                                                {
                                                    safeValue = 0;
                                                }
                                                else
                                                {
                                                    I = -1;
                                                    bestIndex = ((MovePath)Path).MovePointsToPosition(U.MovePoints);
                                                    var path = Path[bestIndex];
                                                    safeValue = UnitSafe(U, Path[bestIndex].Location.X, path.Location.Y);
                                                    while (bestIndex < Path.Count - 1 && safeValue < 0)
                                                    {
                                                        var path2 = Path[bestIndex];
                                                        safeValue = UnitSafe(U, Path[bestIndex].Location.X, path2.Location.Y);
                                                        if (safeValue >= 0)
                                                        {
                                                            I = bestIndex;
                                                        }

                                                        bestIndex += 1;
                                                    }

                                                    if (I > -1)
                                                    {
                                                        V = 0;
                                                        for (; V <= I - 1; V += 1)
                                                        {
                                                            Path.Delete(0);
                                                        }
                                                    }
                                                }

                                                if ((pathValue == 0 || value != 0) && distance <= range && U.GetMoves() == U.MovePoints)
                                                {
                                                    _heroMoves += 1;
                                                    I = Math.Abs(value + pathValue);
                                                    if (I < 1)
                                                    {
                                                        I = 1;
                                                    }

                                                    CreateMove(BattleAIMoveType.Ranged, X, Y, ScaleDamageEvaluation(value, value * 100 / I, 1), ability, spell, U, target);
                                                    if (pathValue > 0 && U.MovePoints > 0 && DetourPath.Count > 1)
                                                    {
                                                        if (!(DetourPath.List[DetourPath.Count - 2].MoveCost > U.MovePoints && DetourPath.List[DetourPath.Count - 2].MoveCost <= 15))
                                                        {
                                                            _heroMoves += 1;
                                                            var multDamageEvaluation = ScaleDamageEvaluation(pathValue + terrainValue, 100 * pathValue / I, 1);
                                                            var detourPath = DetourPath[0];
                                                            CreateMove(BattleAIMoveType.MoveRanged, DetourPath[0].Location.X, detourPath.Location.Y, multDamageEvaluation, ability, spell, U, target);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (Path.Count > 1 && (_ignoreWalls || ChargeFromPosition((int)(((uint)U.Position) >> 4))))
                                                    {
                                                        if (!((safeValue < 0 && _normalUnits != 0) || U.MovePoints <= 0 || Path.Count <= 1 || (Path.List[Path.Count - 2].MoveCost > U.MovePoints && Path.List[Path.Count - 2].MoveCost <= 15)))
                                                        {
                                                            var moves = U.GetMoves();
                                                            var multDamageEvaluation = ScaleDamageEvaluation(pathValue + terrainValue + value, moves, cost);
                                                            var path = Path[0];
                                                            CreateMove(BattleAIMoveType.MoveRanged, Path[0].Location.X, path.Location.Y, multDamageEvaluation, ability, spell, U, target);
                                                        }
                                                        else
                                                        {
                                                            var moves = U.GetMoves();
                                                            CreateMove(BattleAIMoveType.Fake, X, Y, ScaleDamageEvaluation(ScaleDamageEvaluation(pathValue, 100, 1) + terrainValue + value, moves, cost), ability, spell, U, target);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        break;
                                }
                            }
                        }
                        catch
                        {
                            MessageBox(0, "Error during Spell Check", NullTerminatedText("Error - TCAI " + IntToStr(ability) + " " + IntToStr(spell)), 0);
                        }

                        break;
                    default:
                        switch (TacticalTables.AbilityTypes[ability])
                        {
                            case BattleEffectType.Touch:
                                if (target is CombatWall || target is CombatUnit && ((TouchAbility)GamePack.Current.AbilityControl.GetAbility(ability)).CanTouch(U, target))
                                {
                                    try
                                    {
                                        GamePack.Current.AbilityControl.GetAbility(ability).TacticalBattleGetDamageValues(U, target, ref values);
                                        if (values.SourceDamage > 0)
                                        {
                                            adjacent = ((MovePath)Path).MovePointsToPosition(U.MovePoints) == 0;
                                            U.CanFlee = false;
                                            if (target is CombatWall)
                                            {
                                                _flee = false;
                                            }

                                            if (ability == 65)
                                            {
                                                values.TargetDamage = 0;
                                            }

                                            value = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(values.SourceDamage, values.TargetDamage, U, target), hitPoints, hits) + terrainValue;
                                            switch (ability)
                                            {
                                                case >= 27 and <= 29:
                                                case 40:
                                                case 60:
                                                case 106:
                                                case 107:
                                                case 148:
                                                    value = TacticalBattleDamageValueToDamageEvaluation(values.SourceDamage, 0, U, target);
                                                    break;
                                                default:
                                                    value = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(values.SourceDamage, 0, U, target), hitPoints, hits);
                                                    break;
                                            }

                                            if (ability == 117 && CombatObject.MeleeDamageTypes(U) == Set<DamageType>.Of())
                                            {
                                                value = ScaleDamageEvaluation(value, 100, 1);
                                            }

                                            if (target.GetTargetStrength() <= 0 && _enemies.Count > 1)
                                            {
                                                value -= 10000;
                                            }

                                            if (U.MovePoints > 0 && ((MovePath)Path).MovePointsToPosition(U.MovePoints) == 0)
                                            {
                                                switch (ability)
                                                {
                                                    case 65:
                                                        EvalArea(ref value, target, ability, spell, Path.List[1].Location.X, Path.List[1].Location.Y, true, false, true, U, wall);
                                                        break;
                                                    case 114:
                                                        EvalArea(ref value, target, ability, spell, Path.List[1].Location.X, Path.List[1].Location.Y, false, false, true, U, wall);
                                                        break;
                                                }
                                            }

                                            cost = (pathCost - U.MovePoints) * 10;
                                            if (cost < 1)
                                            {
                                                cost = 1;
                                            }

                                            if (!(U.Troop is Hero))
                                            {
                                                safeValue = 0;
                                            }
                                            else
                                            {
                                                I = -1;
                                                bestIndex = ((MovePath)Path).MovePointsToPosition(U.MovePoints);
                                                var pathY = Path[bestIndex];
                                                safeValue = UnitSafe(U, Path[bestIndex].Location.X, pathY.Location.Y);
                                                while (bestIndex < Path.Count - 1 && safeValue < 0)
                                                {
                                                    var path = Path[bestIndex];
                                                    safeValue = UnitSafe(U, Path[bestIndex].Location.X, path.Location.Y);
                                                    if (safeValue >= 0)
                                                    {
                                                        I = bestIndex;
                                                    }

                                                    bestIndex += 1;
                                                }

                                                if (I > -1)
                                                {
                                                    V = 0;
                                                    for (; V <= I - 1; V += 1)
                                                    {
                                                        Path.Delete(0);
                                                    }
                                                }
                                            }

                                            if (Path.Count > 1)
                                            {
                                                if ((safeValue >= 0 || _normalUnits == 0) && adjacent)
                                                {
                                                    _heroMoves += 1;
                                                    if (U.Troop is Hero)
                                                    {
                                                        I = 1;
                                                    }
                                                    else
                                                    {
                                                        if (target is CombatWall && CombatMap.Current.Combat.CombatData.WallBreached && CombatMap.Current.Round > 1)
                                                        {
                                                            I = 10;
                                                        }
                                                        else
                                                        {
                                                            I = 100;
                                                        }
                                                    }

                                                    CreateMove(BattleAIMoveType.Touch, X, Y, ScaleDamageEvaluation(value, I, 1), ability, spell, U, target);
                                                }
                                                else
                                                {
                                                    if (_ignoreWalls || ChargeFromPosition((int)(((uint)U.Position) >> 4)))
                                                    {
                                                        if (!((safeValue < 0 && _normalUnits != 0) || U.MovePoints <= 0 || (Path.List[Path.Count - 2].MoveCost > U.MovePoints && Path.List[Path.Count - 2].MoveCost <= 15)))
                                                        {
                                                            _heroMoves += 1;
                                                            var moves = U.GetMoves();
                                                            var multDamageEvaluation = ScaleDamageEvaluation(value, moves, cost);
                                                            var path = Path[0];
                                                            CreateMove(BattleAIMoveType.Move, Path[0].Location.X, path.Location.Y, multDamageEvaluation, ability, spell, U, target);
                                                        }
                                                        else
                                                        {
                                                            var moves = U.GetMoves();
                                                            CreateMove(BattleAIMoveType.Fake, X, Y, ScaleDamageEvaluation(value, moves, cost), ability, spell, U, target);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        MessageBox(0, "Error during Touch Check", NullTerminatedText("Error - TCAI " + IntToStr(ability) + " " + IntToStr(spell)), 0);
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
                                    cost = GamePack.Current.AbilityControl.GetAbility(ability).TacticalBattleGetDamageValue(U, target);
                                    if (cost > 0)
                                    {
                                        U.CanFlee = false;
                                        if (target is CombatWall)
                                        {
                                            _flee = false;
                                        }

                                        pathValue = 0;
                                        if (GamePack.Current.AbilityControl.GetAbility(ability).GetCombatMode(U.Troop) == CombatMode.LongRange)
                                        {
                                            range = 12;
                                        }
                                        else
                                        {
                                            if (GamePack.Current.AbilityControl.GetAbility(ability).GetCombatMode(U.Troop) == CombatMode.MediumRange)
                                            {
                                                range = 8;
                                            }
                                            else
                                            {
                                                range = 4;
                                            }
                                        }

                                        value = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(cost, 0, U, target), hitPoints, hits);
                                        if (target.GetTargetStrength() <= 0 && _enemies.Count > 1)
                                        {
                                            value -= 10000;
                                        }

                                        if (U.MovePoints > 0)
                                        {
                                            I = ((MovePath)Path).MovePointsToPosition(U.MovePoints);
                                            var pathY = Path[I];
                                            D = HexGeometry.DeltaHexToRadius(Path[I].Location.X, pathY.Location.Y, X, Y);
                                            if (D <= range || distance <= range)
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
                                                            var xhx = U.UnitHexSprite.GetHexX();
                                                            var fUnitHexSpriteHexY = U.UnitHexSprite.GetHexY();
                                                            pathValue = EvalPath(ref value, target, ability, spell, xhx, fUnitHexSpriteHexY, X, Y, false, U, wall);
                                                            break;
                                                        case 24:
                                                        case 44:
                                                            var fUnitHexSpriteHexX = U.UnitHexSprite.GetHexX();
                                                            var hexY2 = U.UnitHexSprite.GetHexY();
                                                            pathValue = EvalPath(ref value, target, ability, spell, fUnitHexSpriteHexX, hexY2, X, Y, true, U, wall);
                                                            break;
                                                        case 30:
                                                        case >= 86 and <= 90:
                                                            var hexX2 = U.UnitHexSprite.GetHexX();
                                                            var hexY3 = U.UnitHexSprite.GetHexY();
                                                            pathValue = EvalBreath(ref value, target, ability, spell, hexX2, hexY3, X, Y, U);
                                                            break;
                                                    }
                                                }
                                            }

                                            if (D <= range && (pathValue > 0 || value > 0))
                                            {
                                                if (pathValue != 0 && value < cost)
                                                {
                                                    bestIndex = 0;
                                                    bestValue = value;
                                                    D = cost;
                                                    I = Path.Count - 2;
                                                    while (I >= 1)
                                                    {
                                                        var path = Path[I];
                                                        var pathX = Path[I];
                                                        cityWall = (CityWall)CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, path.Location.Y].FindOwnedChild(2229508);
                                                        if (cityWall == null && _side == CombatSideIndex.Attacker)
                                                        {
                                                            var path2 = Path[I];
                                                            var path3 = Path[I];
                                                            cityWall = CombatMap.Current.MapContainer.MapLevels[0].Field[path3.Location.X, path2.Location.Y].FindOwnedChild(2229510) as CityWall;
                                                        }

                                                        if (!(cityWall == null || cityWall.HitPoints <= 0 || U.GetAbilityEnabled(1) || U.GetAbilityEnabled(36) || U.GetAbilityEnabled(66)))
                                                        {
                                                            I = 1;
                                                        }
                                                        else
                                                        {
                                                            var path2 = Path[I];
                                                            var path3 = Path[I];
                                                            var path4 = Path[0];
                                                            if (HexGeometry.DeltaHexToRadius(Path[0].Location.X, path4.Location.Y, path3.Location.X, path2.Location.Y) <= range)
                                                            {
                                                                var path5 = Path[I];
                                                                var path6 = Path[I];
                                                                V = EvalPath(ref cost, target, ability, spell, path5.Location.X, path6.Location.Y, X, Y, ability == 24 || ability == 44, U, wall);
                                                                cost = D;
                                                                if (V < pathValue)
                                                                {
                                                                    bestValue = bestValue + pathValue - V;
                                                                    pathValue = V;
                                                                    bestIndex = I;
                                                                }
                                                            }
                                                        }

                                                        I -= 1;
                                                    }

                                                    if (bestIndex > 0)
                                                    {
                                                        DetourPath.Clear();
                                                        I = bestIndex;
                                                        int last2 = Path.Count - 1;
                                                        for (; I <= last2; I += 1)
                                                        {
                                                            var path = Path[I];
                                                            DetourPath.Add(path);
                                                        }

                                                        terrainValue = 0;
                                                        pathCost = MakeTerrainPath(settings, ref DetourPath, ref terrainValue, U, target, true, false, 1);
                                                        pathValue += bestValue;
                                                        pathCost = ((MovePath)Path).PositionToMovePoints(0);
                                                    }
                                                }

                                                bestIndex = 0;
                                                I = 1;
                                                int last3 = Path.Count - 2;
                                                for (; I <= last3; I += 1)
                                                {
                                                    var path = Path[I];
                                                    if (HexGeometry.DeltaHexToRadius(X, Y, Path[I].Location.X, path.Location.Y) <= range && ((MovePath)Path).PositionToMovePoints(I) <= U.MovePoints)
                                                    {
                                                        cost = 1000;
                                                        var pathX = Path[I];
                                                        var path2 = Path[I];
                                                        V = EvalPath(ref cost, target, ability, spell, pathX.Location.X, path2.Location.Y, X, Y, spell == 117, U, wall);
                                                        if (V == 0 && cost == 1000)
                                                        {
                                                            bestIndex = I;
                                                        }
                                                    }
                                                }

                                                if (bestIndex > 0)
                                                {
                                                    I = 0;
                                                    for (; I <= bestIndex - 1; I += 1)
                                                    {
                                                        Path.Delete(0);
                                                    }
                                                }

                                                pathCost = ((MovePath)Path).PositionToMovePoints(0);
                                            }

                                            // { Keep move selection inside the positive movement guard. }
                                            cost = (pathCost - U.MovePoints - range * 4) * 10;
                                            if (cost < 1)
                                            {
                                                cost = 1;
                                            }

                                            if (!(U.Troop is Hero))
                                            {
                                                safeValue = 0;
                                            }
                                            else
                                            {
                                                I = -1;
                                                bestIndex = ((MovePath)Path).MovePointsToPosition(U.MovePoints);
                                                var path = Path[bestIndex];
                                                safeValue = UnitSafe(U, Path[bestIndex].Location.X, path.Location.Y);
                                                while (bestIndex < Path.Count - 1 && safeValue < 0)
                                                {
                                                    var path2 = Path[bestIndex];
                                                    safeValue = UnitSafe(U, Path[bestIndex].Location.X, path2.Location.Y);
                                                    if (safeValue >= 0)
                                                    {
                                                        I = bestIndex;
                                                    }

                                                    bestIndex += 1;
                                                }

                                                if (I > -1)
                                                {
                                                    V = 0;
                                                    for (; V <= I - 1; V += 1)
                                                    {
                                                        Path.Delete(0);
                                                    }
                                                }
                                            }

                                            var hexY4 = U.UnitHexSprite.GetHexY();
                                            if (!((UnitSafe(U, U.UnitHexSprite.GetHexX(), hexY4) < 0 && _normalUnits != 0) || (pathValue != 0 && value == 0) || distance > range || U.GetMoves() != U.MovePoints))
                                            {
                                                _heroMoves += 1;
                                                if (target is CombatWall && CombatMap.Current.Combat.CombatData.WallBreached && CombatMap.Current.Round > 1)
                                                {
                                                    I = 10;
                                                }
                                                else
                                                {
                                                    I = 100;
                                                }

                                                V = Math.Abs(value + pathValue);
                                                if (V < 1)
                                                {
                                                    V = 1;
                                                }

                                                CreateMove(BattleAIMoveType.Ranged, X, Y, ScaleDamageEvaluation(value, I * value / V, 1), ability, spell, U, target);
                                                if (pathValue > 0 && U.MovePoints > 0 && DetourPath.Count > 1)
                                                {
                                                    if (!(DetourPath.List[DetourPath.Count - 2].MoveCost > U.MovePoints && DetourPath.List[DetourPath.Count - 2].MoveCost <= 15))
                                                    {
                                                        _heroMoves += 1;
                                                        var multDamageEvaluation = ScaleDamageEvaluation(pathValue + terrainValue, I * pathValue / V, 1);
                                                        var detourPath = DetourPath[0];
                                                        CreateMove(BattleAIMoveType.MoveRanged, DetourPath[0].Location.X, detourPath.Location.Y, multDamageEvaluation, ability, spell, U, target);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (Path.Count > 1 && (_ignoreWalls || ChargeFromPosition((int)(((uint)U.Position) >> 4))))
                                                {
                                                    if (!((safeValue < 0 && _normalUnits != 0) || U.MovePoints <= 0 || Path.Count <= 1 || (Path.List[Path.Count - 2].MoveCost > U.MovePoints && Path.List[Path.Count - 2].MoveCost <= 15)))
                                                    {
                                                        var moves = U.GetMoves();
                                                        var multDamageEvaluation = ScaleDamageEvaluation(pathValue + terrainValue + value, moves, cost);
                                                        var path = Path[0];
                                                        CreateMove(BattleAIMoveType.MoveRanged, Path[0].Location.X, path.Location.Y, multDamageEvaluation, ability, spell, U, target);
                                                    }
                                                    else
                                                    {
                                                        var moves = U.GetMoves();
                                                        CreateMove(BattleAIMoveType.Fake, X, Y, ScaleDamageEvaluation(ScaleDamageEvaluation(pathValue, 100, 1) + terrainValue + value, moves, cost), ability, spell, U, target);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    MessageBox(0, "Error during Ranged Check", NullTerminatedText("Error - TCAI " + IntToStr(ability) + " " + IntToStr(spell)), 0);
                                }

                                break;
                        }

                        break;
                }
            }
        }

        Path?.Dispose();
        basePath?.Dispose();
        DetourPath?.Dispose();
    }

    // AoWTC.pas:1628
    public void EvalBattle()
    {
        int I;
        int J;
        int K = default;
        int L = default;
        int N;
        int value = default;
        BattleAIMove move;
        BattleAIMove otherMove;
        TacticalBattleUnit U;
        TacticalBattleUnit target;
        CombatMoveControlSettings settings;
        bool moved;
        bool found;
        bool visible;
        DelphiList<CombatAbilityRef> abilities;
        int terrainCost = default;
        int pathCost;
        List<int> bonuses;
        string reason = "";
        PathStepList Path = default;
        AbilityCombatInfo aiInfo = default;
        SpellCombatInfo spellInfo = default;
        CityWall Wall;
        Status = AIStatus.Busy;
        if (StrategicMap.Current.TokenManager.PlayerReady(CombatMap.Current.CurrentPlayer))
        {
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
                        I = 0;
                        int last = _moves.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            (_moves[I] as IDisposable)?.Dispose();
                        }

                        _moves.Clear();
                        _walls.Clear();
                        var tacticalBattle = CombatMap.Current.Combat;
                        _side = (CombatSideIndex)tacticalBattle.CombatData.Players.GetPlayers((sbyte)tacticalBattle.CombatData.CurrentPlayer).SideIndex;
                        _normalUnits = 0;
                        I = 0;
                        int limit = tacticalBattle.CombatData.GetCount() - 1;
                        for (; I <= limit; I += 1)
                        {
                            if (tacticalBattle.CombatData.GetObjects(I) != null && tacticalBattle.CombatData.GetObjects(I) is CombatUnit && tacticalBattle.CombatData.GetObjects(I).GetEnabled())
                            {
                                visible = false;
                                J = 0;
                                while (!visible && J < 19)
                                {
                                    var hexY = ((TacticalBattleUnit)tacticalBattle.CombatData.GetObjects(I)).UnitHexSprite.GetHexY();
                                    HexGeometry.CenterHexNumberToHex(((TacticalBattleUnit)tacticalBattle.CombatData.GetObjects(I)).UnitHexSprite.GetHexX(), hexY, J, ref K, ref L);
                                    if (!CombatMap.Current.ExplorationEnabled || CombatMap.Current.MapContainer.GetField(K, L, 0).GetExplored(CombatMap.Current.SeatedPlayer))
                                    {
                                        visible = true;
                                    }
                                    else
                                    {
                                        J += 1;
                                    }
                                }

                                if (visible)
                                {
                                    if (tacticalBattle.CombatData.GetObjects(I).GetSide() == _side)
                                    {
                                        if ((byte)tacticalBattle.CombatData.GetObjects(I).CombatPlayer == tacticalBattle.CombatData.CurrentPlayer)
                                        {
                                            if (!tacticalBattle.CombatData.GetObjects(I).GetLocked())
                                            {
                                                if (((CombatUnit)tacticalBattle.CombatData.GetObjects(I)).Troop is Hero)
                                                {
                                                    var combatObject = tacticalBattle.CombatData.GetObjects(I);
                                                    _units.Add(combatObject);
                                                }
                                                else
                                                {
                                                    _normalUnits += 1;
                                                    var objectValue = tacticalBattle.CombatData.GetObjects(I);
                                                    _units.Insert(0, objectValue);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var objectValue = tacticalBattle.CombatData.GetObjects(I);
                                            _allies.Add(objectValue);
                                        }
                                    }
                                    else
                                    {
                                        var objectValue = tacticalBattle.CombatData.GetObjects(I);
                                        _enemies.Add(objectValue);
                                    }
                                }
                            }
                        }

                        Creator.GetEngine().OutputDebugString("AI found " + IntToStr(_units.Count) + " Own units.");
                        Creator.GetEngine().OutputDebugString("AI found " + IntToStr(_allies.Count) + " Ally units.");
                        Creator.GetEngine().OutputDebugString("AI found " + IntToStr(_enemies.Count) + " Enemy units.");
                        _ignoreWalls = _side == CombatSideIndex.Attacker;
                        _flee = _side == CombatSideIndex.Attacker && CombatMap.Current.Combat.CombatData.WallType != 0 && !CombatMap.Current.Combat.CombatData.WallBreached;
                        _minMoves = 100;
                        I = 0;
                        int last2 = _units.Count - 1;
                        for (; I <= last2; I += 1)
                        {
                            U = (TacticalBattleUnit)_units[I];
                            U.CanFlee = _side == CombatSideIndex.Attacker;
                            if (U.GetMoves() < _minMoves)
                            {
                                _minMoves = U.GetMoves();
                            }

                            J = 0;
                            int last3 = _enemies.Count - 1;
                            for (; J <= last3; J += 1)
                            {
                                target = (TacticalBattleUnit)_enemies[J];
                                if (!_ignoreWalls && target.GetSide() != U.GetSide())
                                {
                                    var hexY = target.UnitHexSprite.GetHexY();
                                    var hexX = target.UnitHexSprite.GetHexX();
                                    var fUnitHexSpriteHexY = U.UnitHexSprite.GetHexY();
                                    K = HexGeometry.DeltaHexToRadius(U.UnitHexSprite.GetHexX(), fUnitHexSpriteHexY, hexX, hexY);
                                    L = 22;
                                    for (; L <= 169; L += 1)
                                    {
                                        if (target.GetAbilityEnabled(L))
                                        {
                                            if (L == 52 && target.Troop is Hero)
                                            {
                                                N = 0;
                                                for (; N <= 130; N += 1)
                                                {
                                                    bool condition = target.Troop.Player > 0 && TacticalTables.SpellTypes[N] != BattleEffectType.None && StrategicMap.Current.Players.GetPlayers(target.Troop.Player).MagicControl.Spells.IndexOf(N) != -1;
                                                    if (condition)
                                                    {
                                                        var spell = GamePack.Current.SpellControl.GetSpell(N);
                                                        condition = ((Hero)target.Troop).CanCastSpellInstantly(N, spell.BaseManaCost, ref reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        GamePack.Current.SpellControl.GetSpell(N).GetCombatInfo(target.Troop, ref spellInfo);
                                                        if (spellInfo.TargetTypes.Contains(CombatTargetTypeElement.Enemy) && (K <= spellInfo.Range || spellInfo.Range == -1))
                                                        {
                                                            value = GamePack.Current.SpellControl.GetSpell(N).GetCombatDamageValue(target, U);
                                                            if (value > 0)
                                                            {
                                                                if (Set<BattleEffectType>.Of(BattleEffectType.Arrow, BattleEffectType.Rock, BattleEffectType.Comet, BattleEffectType.Breath, BattleEffectType.CometBall, BattleEffectType.Ray).Contains(TacticalTables.SpellTypes[N]))
                                                                {
                                                                    var fUnitHexSpriteHexX = target.UnitHexSprite.GetHexX();
                                                                    var hexY2 = target.UnitHexSprite.GetHexY();
                                                                    var hexX2 = U.UnitHexSprite.GetHexX();
                                                                    var hexY3 = U.UnitHexSprite.GetHexY();
                                                                    EvalPath(ref value, U, L, N, fUnitHexSpriteHexX, hexY2, hexX2, hexY3, false, target, null);
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
                                                if (TacticalTables.AbilityTypes[L] != BattleEffectType.None && TacticalTables.AbilityTypes[L] != BattleEffectType.Touch)
                                                {
                                                    GamePack.Current.AbilityControl.GetAbility(L).GetCombatInfo(target.Troop, ref aiInfo);
                                                    if ((((byte)aiInfo.TargetTypes.Image) & 2) != 0)
                                                    {
                                                        if (GamePack.Current.AbilityControl.GetAbility(L).GetCombatMode(target.Troop) == CombatMode.LongRange)
                                                        {
                                                            N = 12;
                                                        }
                                                        else
                                                        {
                                                            if (GamePack.Current.AbilityControl.GetAbility(L).GetCombatMode(target.Troop) == CombatMode.MediumRange)
                                                            {
                                                                N = 8;
                                                            }
                                                            else
                                                            {
                                                                N = 4;
                                                            }
                                                        }

                                                        if (K <= N)
                                                        {
                                                            value = GamePack.Current.AbilityControl.GetAbility(L).TacticalBattleGetDamageValue(target, U);
                                                            if (value > 0)
                                                            {
                                                                if (Set<BattleEffectType>.Of(BattleEffectType.Arrow, BattleEffectType.Rock, BattleEffectType.Comet, BattleEffectType.Breath, BattleEffectType.CometBall, BattleEffectType.Ray).Contains(TacticalTables.AbilityTypes[L]))
                                                                {
                                                                    var fUnitHexSpriteHexX = target.UnitHexSprite.GetHexX();
                                                                    var hexY2 = target.UnitHexSprite.GetHexY();
                                                                    var hexX2 = U.UnitHexSprite.GetHexX();
                                                                    var hexY3 = U.UnitHexSprite.GetHexY();
                                                                    EvalPath(ref value, U, L, 0, fUnitHexSpriteHexX, hexY2, hexX2, hexY3, false, target, null);
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
                        MessageBox(0, "Error - TCAI", "Error during Create Unit List", 0);
                    }

                    break;
                case 1:
                    try
                    {
                        while (_unitIndex < _units.Count && _unitLimit < 1)
                        {
                            U = (TacticalBattleUnit)_units[_unitIndex];
                            if (!U.GetLocked())
                            {
                                abilities = new DelphiList<CombatAbilityRef>();
                                try
                                {
                                    J = 0;
                                    for (; J <= 169; J += 1)
                                    {
                                        if (U.GetAbilityEnabled(J))
                                        {
                                            switch (J)
                                            {
                                                case 1:
                                                case 36:
                                                case 66:
                                                case 117:
                                                    _flee = false;
                                                    break;
                                            }

                                            if (J == 52 && U.Troop is Hero)
                                            {
                                                K = 0;
                                                for (; K <= 130; K += 1)
                                                {
                                                    bool condition = U.Troop.Player > 0 && TacticalTables.SpellTypes[K] != BattleEffectType.None && StrategicMap.Current.Players.GetPlayers(U.Troop.Player).MagicControl.Spells.IndexOf(K) != -1;
                                                    if (condition)
                                                    {
                                                        var spell = GamePack.Current.SpellControl.GetSpell(K);
                                                        condition = ((Hero)U.Troop).CanCastSpellInstantly(K, spell.BaseManaCost, ref reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        GamePack.Current.SpellControl.GetSpell(K).GetCombatInfo(U.Troop, ref spellInfo);
                                                        if (spellInfo.TargetTypes.Contains(CombatTargetTypeElement.Enemy))
                                                        {
                                                            abilities.Add(new CombatAbilityRef());
                                                            abilities[abilities.Count - 1].Ability = J;
                                                            abilities[abilities.Count - 1].Spell = K;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (TacticalTables.AbilityTypes[J] != BattleEffectType.None)
                                                {
                                                    GamePack.Current.AbilityControl.GetAbility(J).GetCombatInfo(U.Troop, ref aiInfo);
                                                    if ((((byte)aiInfo.TargetTypes.Image) & 2) != 0)
                                                    {
                                                        abilities.Add(new CombatAbilityRef());
                                                        abilities[abilities.Count - 1].Ability = J;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    MessageBox(0, "Error during Eval Enemies", NullTerminatedText("Error - TCAI " + U.Troop.GetName()), 0);
                                }

                                settings = new CombatMoveControlSettings(null) { Holder = Creator.CreationHolder() };
                                if (U.MovePoints > 0)
                                {
                                    var hexY = U.UnitHexSprite.GetHexY();
                                    settings.SetSourcePosition(U.UnitHexSprite.GetHexX(), hexY, 0);
                                    settings.ArmyHexSprite = U.UnitHexSprite;
                                    settings.MoveFlags = Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Melee, CombatMoveModeElement.Move, CombatMoveModeElement.Wall);
                                    U.Troop.CreateMovePointTable(out settings.MovePointTable);
                                    settings.Player = U.GetPlayer();
                                    settings.MovePointsMax = 200;
                                    CombatMap.Current.MoveControl.ProcessMove(settings);
                                }

                                if (abilities.Count > 0 || CombatObject.MeleeDamageTypes(U) != Set<DamageType>.Of())
                                {
                                    J = 0;
                                    int last = _enemies.Count - 1;
                                    for (; J <= last; J += 1)
                                    {
                                        target = (TacticalBattleUnit)_enemies[J];
                                        if (!(U.Troop is Hero) || _heroMoves == 0)
                                        {
                                            var hits = target.GetHits();
                                            var hitPoints = target.GetHitPoints();
                                            var hexY = target.UnitHexSprite.GetHexY();
                                            CheckUnit(target.UnitHexSprite.GetHexX(), hexY, hitPoints, hits, settings, U, target, abilities, null);
                                        }
                                    }
                                }

                                I = 0;
                                int limit = abilities.Count - 1;
                                for (; I <= limit; I += 1)
                                {
                                    (abilities[I] as IDisposable)?.Dispose();
                                }

                                abilities.Clear();
                                try
                                {
                                    J = 22;
                                    for (; J <= 169; J += 1)
                                    {
                                        if (U.GetAbilityEnabled(J))
                                        {
                                            if (J == 52 && U.Troop is Hero)
                                            {
                                                K = 0;
                                                for (; K <= 130; K += 1)
                                                {
                                                    bool condition = TacticalTables.SpellTypes[K] != BattleEffectType.None && StrategicMap.Current.Players.GetPlayers(U.Troop.Player).MagicControl.Spells.IndexOf(K) != -1;
                                                    if (condition)
                                                    {
                                                        var spell = GamePack.Current.SpellControl.GetSpell(K);
                                                        condition = ((Hero)U.Troop).CanCastSpellInstantly(K, spell.BaseManaCost, ref reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        GamePack.Current.SpellControl.GetSpell(K).GetCombatInfo(U.Troop, ref spellInfo);
                                                        if (spellInfo.TargetTypes.Contains(CombatTargetTypeElement.Friend))
                                                        {
                                                            abilities.Add(new CombatAbilityRef());
                                                            abilities[abilities.Count - 1].Ability = J;
                                                            abilities[abilities.Count - 1].Spell = K;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (TacticalTables.AbilityTypes[J] != BattleEffectType.None)
                                                {
                                                    GamePack.Current.AbilityControl.GetAbility(J).GetCombatInfo(U.Troop, ref aiInfo);
                                                    if ((((byte)aiInfo.TargetTypes.Image) & 1) != 0)
                                                    {
                                                        abilities.Add(new CombatAbilityRef());
                                                        abilities[abilities.Count - 1].Ability = J;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    MessageBox(0, "Error during Eval Friends", NullTerminatedText("Error - TCAI " + U.Troop.GetName()), 0);
                                }

                                if (abilities.Count > 0)
                                {
                                    J = 0;
                                    int last = _units.Count - 1;
                                    for (; J <= last; J += 1)
                                    {
                                        target = (TacticalBattleUnit)_units[J];
                                        if (!(U.Troop is Hero) || _heroMoves == 0)
                                        {
                                            var hits = target.GetHits();
                                            var hitPoints = target.GetHitPoints();
                                            var hexY = target.UnitHexSprite.GetHexY();
                                            CheckUnit(target.UnitHexSprite.GetHexX(), hexY, hitPoints, hits, settings, U, target, abilities, null);
                                        }
                                    }
                                }

                                settings?.Dispose();
                                I = 0;
                                int last2 = abilities.Count - 1;
                                for (; I <= last2; I += 1)
                                {
                                    (abilities[I] as IDisposable)?.Dispose();
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
                                if (CombatMap.Current.Combat.CombatData.WallType != 0 && CombatMap.Current.Combat.CombatData.Wall != null && _side == CombatSideIndex.Attacker)
                                {
                                    J = 0;
                                    int last = CombatMap.Current.Walls.Count - 1;
                                    for (; J <= last; J += 1)
                                    {
                                        bool condition = (CombatMap.Current.Walls[J]).HitPoints > 0;
                                        if (condition)
                                        {
                                            var wall = CombatMap.Current.Walls[J];
                                            var wall2 = CombatMap.Current.Walls[J];
                                            condition = ((CityWall)wall).HitPoints < TacticalTables.WallMaxHitPoints[((CityWall)wall2).WallType - 1];
                                        }

                                        if (condition)
                                        {
                                            found = true;
                                            K = 0;
                                            while (found && K < _walls.Count)
                                            {
                                                if (_walls[K] == CombatMap.Current.Walls[J])
                                                {
                                                    found = false;
                                                }
                                                else
                                                {
                                                    K += 1;
                                                }
                                            }

                                            if (found)
                                            {
                                                var wall = CombatMap.Current.Walls[J];
                                                _walls.Add(wall);
                                            }
                                        }
                                    }
                                }

                                if (CombatMap.Current.Combat.CombatData.WallType == 0 || CombatMap.Current.Combat.CombatData.Wall == null || _side == CombatSideIndex.Defender)
                                {
                                    _currentMove += 1;
                                }

                                _unitIndex = 0;
                                Creator.GetEngine().OutputDebugString("AI calculated " + IntToStr(_units.Count * _enemies.Count) + " Attack paths.");
                                Creator.GetEngine().OutputDebugString("AI calculated " + IntToStr(_moves.Count) + " Moves.");
                            }
                            catch
                            {
                                MessageBox(0, "Error - TCAI ", "Error during Eval Ending", 0);
                            }
                        }

                        _unitLimit = 0;
                    }
                    catch
                    {
                        MessageBox(0, "Error - TCAI", "Error during Create Move List", 0);
                    }

                    break;
                case 2:
                    try
                    {
                        while (_unitIndex < _units.Count && _unitLimit < 1)
                        {
                            U = (TacticalBattleUnit)_units[_unitIndex];
                            if (!U.GetLocked())
                            {
                                abilities = new DelphiList<CombatAbilityRef>();
                                J = 0;
                                for (; J <= 169; J += 1)
                                {
                                    if (U.GetAbilityEnabled(J))
                                    {
                                        switch (J)
                                        {
                                            case 1:
                                            case 36:
                                            case 66:
                                            case 117:
                                                _flee = false;
                                                break;
                                        }

                                        if (J == 52 && U.Troop is Hero)
                                        {
                                            if (U.Troop.Player > 0)
                                            {
                                                K = 0;
                                                for (; K <= 130; K += 1)
                                                {
                                                    bool condition = TacticalTables.SpellTypes[K] != BattleEffectType.None && StrategicMap.Current.Players.GetPlayers(U.Troop.Player).MagicControl.Spells.IndexOf(K) != -1;
                                                    if (condition)
                                                    {
                                                        var spell = GamePack.Current.SpellControl.GetSpell(K);
                                                        condition = ((Hero)U.Troop).CanCastSpellInstantly(K, spell.BaseManaCost, ref reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        GamePack.Current.SpellControl.GetSpell(K).GetCombatInfo(U.Troop, ref spellInfo);
                                                        if (spellInfo.DamageTypes.Contains(DamageType.Wall))
                                                        {
                                                            _flee = false;
                                                            abilities.Add(new CombatAbilityRef());
                                                            abilities[abilities.Count - 1].Ability = J;
                                                            abilities[abilities.Count - 1].Spell = K;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (TacticalTables.AbilityTypes[J] != BattleEffectType.None)
                                            {
                                                GamePack.Current.AbilityControl.GetAbility(J).GetCombatInfo(U.Troop, ref aiInfo);
                                                if (aiInfo.DamageTypes.Contains(DamageType.Wall))
                                                {
                                                    _flee = false;
                                                    abilities.Add(new CombatAbilityRef());
                                                    abilities[abilities.Count - 1].Ability = J;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (abilities.Count > 0)
                                {
                                    settings = new CombatMoveControlSettings(null) { Holder = Creator.CreationHolder() };
                                    if (U.MovePoints > 0)
                                    {
                                        var hexY = U.UnitHexSprite.GetHexY();
                                        settings.SetSourcePosition(U.UnitHexSprite.GetHexX(), hexY, 0);
                                        settings.ArmyHexSprite = U.UnitHexSprite;
                                        settings.MoveFlags = Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Melee, CombatMoveModeElement.Move, CombatMoveModeElement.Wall);
                                        if (U.GetAbilityEnabled(65) || U.GetAbilityEnabled(117))
                                        {
                                            settings.MoveFlags = settings.MoveFlags + Set<CombatMoveModeElement>.Of(CombatMoveModeElement.WallCrushing);
                                        }

                                        U.Troop.CreateMovePointTable(out settings.MovePointTable);
                                        settings.Player = U.GetPlayer();
                                        settings.MovePointsMax = 200;
                                        CombatMap.Current.MoveControl.ProcessMove(settings);
                                    }

                                    J = 0;
                                    int last = _walls.Count - 1;
                                    for (; J <= last; J += 1)
                                    {
                                        Wall = _walls[J];
                                        if (!(U.Troop is Hero) || _heroMoves == 0)
                                        {
                                            var hexY = Wall.GetHexY();
                                            CheckUnit(Wall.GetHexX(), hexY, Wall.HitPoints, TacticalTables.WallMaxHitPoints[Wall.WallType - 1] * 2, settings, U, CombatMap.Current.Combat.CombatData.Wall, abilities, Wall);
                                        }
                                    }

                                    if (_walls.Count == 0)
                                    {
                                        L = 100;
                                        Wall = null;
                                        J = 0;
                                        int limit = CombatMap.Current.Walls.Count - 1;
                                        for (; J <= limit; J += 1)
                                        {
                                            var hexY = U.UnitHexSprite.GetHexY();
                                            var hexY2 = (CombatMap.Current.Walls[J]).GetHexY();
                                            N = HexGeometry.DeltaHexToRadius((CombatMap.Current.Walls[J]).GetHexX(), hexY2, U.UnitHexSprite.GetHexX(), hexY);
                                            if ((CombatMap.Current.Walls[J]).HitPoints > 0 && N < L)
                                            {
                                                Wall = CombatMap.Current.Walls[J];
                                                L = N;
                                            }
                                        }

                                        if (Wall != null)
                                        {
                                            var hexY = Wall.GetHexY();
                                            CheckUnit(Wall.GetHexX(), hexY, Wall.HitPoints * 2, TacticalTables.WallMaxHitPoints[Wall.WallType - 1], settings, U, CombatMap.Current.Combat.CombatData.Wall, abilities, Wall);
                                        }
                                    }

                                    settings?.Dispose();
                                    I = 0;
                                    int last2 = abilities.Count - 1;
                                    for (; I <= last2; I += 1)
                                    {
                                        (abilities[I] as IDisposable)?.Dispose();
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
                            Creator.GetEngine().OutputDebugString("AI calculated " + IntToStr(_units.Count * _enemies.Count) + " Attack paths.");
                            Creator.GetEngine().OutputDebugString("AI calculated " + IntToStr(_moves.Count) + " Moves.");
                        }

                        _unitLimit = 0;
                    }
                    catch
                    {
                        MessageBox(0, "Error - TCAI", "Error during Create WallCrush List", 0);
                    }

                    break;
                case 3:
                    try
                    {
                        bonuses = new List<int>();
                        I = 0;
                        int last = _units.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            bonuses.Add(0);
                        }

                        while (_unitIndex < _moves.Count && _unitLimit < 50)
                        {
                            move = _moves[_unitIndex];
                            if (move.MoveType != BattleAIMoveType.Fake)
                            {
                                J = 0;
                                int limit = _units.Count - 1;
                                for (; J <= limit; J += 1)
                                {
                                    bonuses[J] = 0;
                                }

                                J = 0;
                                int last2 = _moves.Count - 1;
                                for (; J <= last2; J += 1)
                                {
                                    otherMove = _moves[J];
                                    if (move.UnitHexSprite.Unit != otherMove.UnitHexSprite.Unit)
                                    {
                                        K = HexGeometry.DeltaHexToRadius(move.X, move.Y, otherMove.X, otherMove.Y) * 4 - otherMove.UnitHexSprite.Unit.MovePoints;
                                        if (K < 4)
                                        {
                                            K = 4;
                                        }

                                        L = otherMove.UnitHexSprite.Unit.GetMoves() * (otherMove.Value / (K * 10));
                                        K = _units.IndexOf(otherMove.UnitHexSprite.Unit);
                                        if (bonuses[K] < L)
                                        {
                                            bonuses[K] = L;
                                        }
                                    }
                                }

                                J = 0;
                                int last3 = _units.Count - 1;
                                for (; J <= last3; J += 1)
                                {
                                    if (move.GroupValue > -1000000 && move.GroupValue < 1000000)
                                    {
                                        var bonus = bonuses[J];
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
                        MessageBox(0, "Error - TCAI", "Error during Build Group Bonuses", 0);
                    }

                    break;
                case 4:
                    try
                    {
                        I = 0;
                        int last = _units.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            U = (TacticalBattleUnit)_units[I];
                            if (U.Troop is Hero && _heroMoves == 0 && U.MovePoints > 0)
                            {
                                var hexY = U.UnitHexSprite.GetHexY();
                                K = UnitSafe(U, U.UnitHexSprite.GetHexX(), hexY);
                                value = K;
                                N = -1;
                                J = 0;
                                int limit = _units.Count - 1;
                                for (; J <= limit; J += 1)
                                {
                                    target = (TacticalBattleUnit)_units[J];
                                    if (U != target)
                                    {
                                        var fUnitHexSpriteHexY = target.UnitHexSprite.GetHexY();
                                        L = UnitSafe(U, target.UnitHexSprite.GetHexX(), fUnitHexSpriteHexY);
                                        if (L > K)
                                        {
                                            N = J;
                                            K = L;
                                        }
                                    }
                                }

                                if (K > value)
                                {
                                    settings = new CombatMoveControlSettings(null) { Holder = Creator.CreationHolder() };
                                    // { Original uses X for both coordinates. }
                                    var hexX = U.UnitHexSprite.GetHexX();
                                    settings.SetSourcePosition(U.UnitHexSprite.GetHexX(), hexX, 0);
                                    var fUnitHexSpriteHexY = ((TacticalBattleUnit)_units[N]).UnitHexSprite.GetHexY();
                                    settings.SetDestinationPosition(((TacticalBattleUnit)_units[N]).UnitHexSprite.GetHexX(), fUnitHexSpriteHexY, 0);
                                    settings.ArmyHexSprite = U.UnitHexSprite;
                                    settings.MoveFlags = Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Melee, CombatMoveModeElement.Move, CombatMoveModeElement.Wall);
                                    U.Troop.CreateMovePointTable(out settings.MovePointTable);
                                    settings.Player = U.GetPlayer();
                                    CombatMap.Current.MoveControl.ProcessMove(settings);
                                    if (settings.Path.Count > 2)
                                    {
                                        settings.Path.Delete(0);
                                        var path = settings.Path[0];
                                        CreateMove(BattleAIMoveType.Move, settings.Path[0].Location.X, path.Location.Y, K, 0, 0, U, null);
                                    }

                                    settings?.Dispose();
                                }
                                else
                                {
                                    if (_side == CombatSideIndex.Attacker)
                                    {
                                        U.CanFlee = true;
                                    }
                                }
                            }
                        }

                        if (_side == CombatSideIndex.Attacker)
                        {
                            I = 0;
                            int limit = _units.Count - 1;
                            for (; I <= limit; I += 1)
                            {
                                U = (TacticalBattleUnit)_units[I];
                                if ((!(U.Troop is Hero) || _heroMoves == 0 || (U.Troop is Hero && _units.Count == 1)) && U.GetEnabled() && !U.GetLocked() && U.MovePoints > 0 && (_flee || U.CanFlee))
                                {
                                    J = 0;
                                    int last2 = CombatMap.Current.FleeLocations.Count - 1;
                                    for (; J <= last2; J += 1)
                                    {
                                        var hexY = U.UnitHexSprite.GetHexY();
                                        var hexY2 = (CombatMap.Current.FleeLocations[J]).GetHexY();
                                        var deltaHexToRadius2 = HexGeometry.DeltaHexToRadius((CombatMap.Current.FleeLocations[J]).GetHexX(), hexY2, U.UnitHexSprite.GetHexX(), hexY);
                                        var hexY3 = (CombatMap.Current.FleeLocations[J]).GetHexY();
                                        CreateMove(BattleAIMoveType.Move, (CombatMap.Current.FleeLocations[J]).GetHexX(), hexY3, 110000000 - deltaHexToRadius2 * 10, 0, 0, U, null);
                                    }
                                }
                            }
                        }

                        I = 0;
                        while (I < _moves.Count)
                        {
                            move = _moves[I];
                            switch (move.MoveType)
                            {
                                case BattleAIMoveType.Fake:
                                    _moves.RemoveAt(I);
                                    break;
                                default:
                                    {
                                        if (move.Value > -100000000 && move.Value < 100000000)
                                        {
                                            move.Value += move.GroupValue / 4;
                                        }

                                        I += 1;
                                    }

                                    break;
                            }
                        }

                        I = 1;
                        int last3 = _moves.Count - 1;
                        for (; I <= last3; I += 1)
                        {
                            move = _moves[I];
                            J = 0;
                            K = -1;
                            while (J < I)
                            {
                                otherMove = _moves[J];
                                if (move.Value > otherMove.Value)
                                {
                                    K = J;
                                    J = I;
                                }
                                else
                                {
                                    J += 1;
                                }
                            }

                            if (K > -1)
                            {
                                if (I != K)
                                {
                                    var relocated = _moves[I];
                                    _moves.RemoveAt(I);
                                    _moves.Insert(K, relocated);
                                }
                            }
                        }

                        _currentMove += 1;
                    }
                    catch
                    {
                        MessageBox(0, "Error - TCAI", "Error during Add Group Bonuses", 0);
                    }

                    break;
                case 5:
                    try
                    {
                        moved = false;
                        while (_moves.Count > 0 && !moved)
                        {
                            move = _moves[0];
                            switch (move.MoveType)
                            {
                                case BattleAIMoveType.Move:
                                case BattleAIMoveType.MoveRanged:
                                    {
                                        var tacticalBattleUnitHexSprite = move.UnitHexSprite;
                                        if ((move.UnitHexSprite.Unit.Troop is Hero || (_side == CombatSideIndex.Attacker && CombatMap.Current.Round == 1)) && move.UnitHexSprite.Unit.MovePoints > _minMoves)
                                        {
                                            move.UnitHexSprite.Unit.MovePoints = _minMoves;
                                        }

                                        settings = new CombatMoveControlSettings(null) { Holder = Creator.CreationHolder() };
                                        var hexLevel = tacticalBattleUnitHexSprite.GetHexLevel();
                                        var hexY = tacticalBattleUnitHexSprite.GetHexY();
                                        settings.SetSourcePosition(tacticalBattleUnitHexSprite.GetHexX(), hexY, hexLevel);
                                        settings.SetDestinationPosition(move.X, move.Y, 0);
                                        settings.ArmyHexSprite = move.UnitHexSprite;
                                        if (move.MoveType == BattleAIMoveType.MoveRanged || CombatObject.MeleeDamageTypes(move.UnitHexSprite.Unit) == Set<DamageType>.Of())
                                        {
                                            settings.MoveFlags = Set<CombatMoveModeElement>.Of(CombatMoveModeElement.MoveRanged);
                                        }
                                        else
                                        {
                                            settings.MoveFlags = Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Melee);
                                        }

                                        if (CombatMap.Current.Combat.CombatData.WallType != 0)
                                        {
                                            if (!(CombatMap.Current.Combat.CombatData.WallBreached && CombatMap.Current.MapContainer.MapLevels[0].Field[move.X, move.Y].FindOwnedChild(2229508) == null && CombatMap.Current.MapContainer.MapLevels[0].Field[move.X, move.Y].FindOwnedChild(2229510) == null))
                                            {
                                                if (!_ignoreWalls || !ChargeFromPosition((int)(((uint)move.UnitHexSprite.Unit.Position) >> 4)) || _side == CombatSideIndex.Attacker)
                                                {
                                                    settings.MoveFlags = settings.MoveFlags + Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Wall);
                                                }
                                            }
                                        }

                                        move.UnitHexSprite.Unit.Troop.CreateMovePointTable(out settings.MovePointTable);
                                        settings.Player = move.UnitHexSprite.Unit.GetPlayer();
                                        CombatMap.Current.MoveControl.ProcessMove(settings);
                                        if (settings.Path.Count < 2 && !settings.MoveFlags.Contains(CombatMoveModeElement.Wall))
                                        {
                                            settings.MoveFlags = settings.MoveFlags + Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Wall);
                                            CombatMap.Current.MoveControl.ProcessMove(settings);
                                        }

                                        Wall = (CityWall)CombatMap.Current.MapContainer.MapLevels[0].Field[move.X, move.Y].FindOwnedChild(2229508);
                                        if (Wall == null)
                                        {
                                            Wall = CombatMap.Current.MapContainer.MapLevels[0].Field[move.X, move.Y].FindOwnedChild(2229510) as CityWall;
                                        }

                                        if (settings.Path.Count == 2 && move.MoveType == BattleAIMoveType.MoveRanged && Wall != null)
                                        {
                                            move.UnitHexSprite.Unit.MovePoints = 0;
                                        }
                                        else
                                        {
                                            if (settings.Path.Count > 1)
                                            {
                                                Path = settings.Path;
                                                pathCost = MakeTerrainPath(settings, ref Path, ref terrainCost, move.UnitHexSprite.Unit, null, false, false, 0);
                                                if (Path.Count > 1 && pathCost > -1)
                                                {
                                                    if (Path[Path.Count - 2].MoveCost > 0 && Path[Path.Count - 2].MoveCost <= move.UnitHexSprite.Unit.MovePoints)
                                                    {
                                                        if (move.MoveType == BattleAIMoveType.MoveRanged)
                                                        {
                                                            value = ((MovePath)Path).PositionToMovePoints(0);
                                                            if (value < move.UnitHexSprite.Unit.MovePoints)
                                                            {
                                                                move.UnitHexSprite.Unit.MovePoints = value;
                                                            }
                                                        }

                                                        value = ((MovePath)Path).MovePointsToPosition(move.UnitHexSprite.Unit.MovePoints);
                                                        value = ((MovePath)Path).PositionToMovePoints(value);
                                                        if (value > 0 && value < move.UnitHexSprite.Unit.MovePoints)
                                                        {
                                                            move.UnitHexSprite.Unit.MovePoints = value;
                                                        }

                                                        // { Original repeats X. }
                                                        Creator.GetEngine().OutputDebugString("AI moves to X: " + IntToStr(move.X) + " Y: " + IntToStr(move.X));
                                                        moved = TacticalBattleUnit.MoveCombatUnit(move.UnitHexSprite.Unit, (MovePath)Path);
                                                        if (move.UnitHexSprite.Unit.CombatPlayer == CombatMap.Current.SeatedPlayer)
                                                        {
                                                            var unitHexSpriteHexY = move.UnitHexSprite.GetHexY();
                                                            var hexX = move.UnitHexSprite.GetHexX();
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
                                    var hexY2 = move.UnitHexSprite.GetHexY();
                                    settings.SetSourcePosition(move.UnitHexSprite.GetHexX(), hexY2, 0);
                                    settings.SetDestinationPosition(move.X, move.Y, 0);
                                    settings.ArmyHexSprite = move.UnitHexSprite;
                                    settings.MoveFlags = Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Melee);
                                    move.UnitHexSprite.Unit.Troop.CreateMovePointTable(out settings.MovePointTable);
                                    settings.Player = move.UnitHexSprite.Unit.GetPlayer();
                                    CombatMap.Current.MoveControl.ProcessMove(settings);
                                    if (settings.Path.Count > 1)
                                    {
                                        Path = settings.Path;
                                        pathCost = MakeTerrainPath(settings, ref Path, ref terrainCost, move.UnitHexSprite.Unit, null, false, false, 1);
                                        if (Path.Count > 1 && pathCost > -1)
                                        {
                                            if (Path[Path.Count - 2].MoveCost <= move.UnitHexSprite.Unit.MovePoints)
                                            {
                                                // { Original repeats X in the debug text. }
                                                Creator.GetEngine().OutputDebugString("AI touches at X: " + IntToStr(move.X) + " Y: " + IntToStr(move.X));
                                                moved = move.UnitHexSprite.MeleeMoveTacticalBattle((MovePath)Path);
                                                if (move.UnitHexSprite.Unit.CombatPlayer == CombatMap.Current.SeatedPlayer)
                                                {
                                                    var hexY = move.UnitHexSprite.GetHexY();
                                                    var hexX = move.UnitHexSprite.GetHexX();
                                                    CombatMap.Current.CenterViewTo(hexX, hexY, 0);
                                                }
                                            }
                                        }
                                    }

                                    settings?.Dispose();
                                    break;
                                case BattleAIMoveType.Touch:
                                    GamePack.Current.AbilityControl.GetAbility(move.Ability).GetCombatInfo(move.UnitHexSprite.Unit.Troop, ref aiInfo);
                                    settings = new CombatMoveControlSettings(null) { Holder = Creator.CreationHolder() };
                                    var hexY3 = move.UnitHexSprite.GetHexY();
                                    settings.SetSourcePosition(move.UnitHexSprite.GetHexX(), hexY3, 0);
                                    settings.SetDestinationPosition(move.X, move.Y, 0);
                                    settings.ArmyHexSprite = move.UnitHexSprite;
                                    settings.MoveFlags = Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Melee);
                                    if (aiInfo.DamageTypes.Contains(DamageType.Wall))
                                    {
                                        settings.MoveFlags = settings.MoveFlags + Set<CombatMoveModeElement>.Of(CombatMoveModeElement.WallCrushing);
                                    }

                                    move.UnitHexSprite.Unit.Troop.CreateMovePointTable(out settings.MovePointTable);
                                    settings.Player = move.UnitHexSprite.Unit.GetPlayer();
                                    move.UnitHexSprite.SelectedAbility = move.Ability;
                                    CombatMap.Current.MoveControl.ProcessMove(settings);
                                    if (settings.Path.Count > 1)
                                    {
                                        Path = settings.Path;
                                        pathCost = MakeTerrainPath(settings, ref Path, ref terrainCost, move.UnitHexSprite.Unit, null, false, false, 1);
                                        if (Path.Count > 1 && pathCost > -1)
                                        {
                                            if (Path[Path.Count - 2].MoveCost <= move.UnitHexSprite.Unit.MovePoints)
                                            {
                                                move.UnitHexSprite.SelectedAbility = move.Ability;
                                                // { Original repeats X in the debug text. }
                                                Creator.GetEngine().OutputDebugString("AI melees at X: " + IntToStr(move.X) + " Y: " + IntToStr(move.X));
                                                moved = move.UnitHexSprite.AbilityTouchMoveTacticalBattle((MovePath)Path);
                                                if (move.UnitHexSprite.Unit.CombatPlayer == CombatMap.Current.SeatedPlayer)
                                                {
                                                    var hexY = move.UnitHexSprite.GetHexY();
                                                    var hexX = move.UnitHexSprite.GetHexX();
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
                                    Creator.GetEngine().OutputDebugString("AI ranged at X: " + IntToStr(move.X) + " Y: " + IntToStr(move.X));
                                    moved = move.UnitHexSprite.AbilityRangedTacticalBattle(move.X, move.Y);
                                    if (move.UnitHexSprite.Unit.CombatPlayer == CombatMap.Current.SeatedPlayer)
                                    {
                                        var hexY = move.UnitHexSprite.GetHexY();
                                        var hexX = move.UnitHexSprite.GetHexX();
                                        CombatMap.Current.CenterViewTo(hexX, hexY, 0);
                                    }

                                    break;
                            }

                            (
                            (_moves[0]) as IDisposable)?.Dispose();
                            _moves.RemoveAt(0);
                        }

                        if (!moved)
                        {
                            Status = AIStatus.Done;
                        }

                        I = 0;
                        int last = _moves.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            (_moves[I] as IDisposable)?.Dispose();
                        }

                        _moves.Clear();
                        _currentMove = 0;
                    }
                    catch
                    {
                        MessageBox(0, "Error - TCAI", "Error during Execute Moves", 0);
                    }

                    break;
            }
        }
    }

    public enum AIStatus : byte
    {
        Idle = 0, Busy = 1, Done = 2
    }

}
