#nullable disable
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

    // The collect step admits only tactical battle units, so the element casts never fail.
    private readonly List<TacticalBattleUnit> _units;
    private readonly List<TacticalBattleUnit> _allies;
    private readonly List<TacticalBattleUnit> _enemies;
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
        for (int i = 0, last = _moves.Count - 1; i <= last; i++)
        {
            (_moves[i] as IDisposable)?.Dispose();
        }

        _moves?.Clear();
    }

    // AoWTC.pas:752
    public BattleAI(EngineObject creator)
    {
        Creator = creator;
        _units = new List<TacticalBattleUnit>();
        _allies = new List<TacticalBattleUnit>();
        _enemies = new List<TacticalBattleUnit>();
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
    public void CreateMove(BattleAIMoveType moveType, int x, int y, int value, int ability, int spell, TacticalBattleUnit unit, CombatObject target)
    {
        BattleAIMove m;
        m = new BattleAIMove();
        m.UnitHexSprite = unit.UnitHexSprite;
        m.Target = target;
        m.MoveType = moveType;
        m.Ability = (short)ability;
        m.Spell = (short)spell;
        m.X = (ushort)x;
        m.Y = (ushort)y;
        m.Value = value;
        _moves.Add(m);
    }

    // AoWTC.pas:805
    public int TacticalBattleDamageValueToDamageEvaluation(int damageValue1, int damageValue2, CombatObject source, CombatObject target)
    {
        int strength;
        strength = target.GetTargetStrength();
        if (target is TacticalBattleUnit targetUnit)
        {
            if (targetUnit.Troop is Hero)
            {
                strength = strength * 2;
            }

            if (targetUnit.Troop is Leader)
            {
                strength = strength * 2;
            }
        }

        int result = strength * damageValue1;
        strength = source.GetTargetStrength();
        if (source is TacticalBattleUnit sourceUnit)
        {
            if (sourceUnit.Troop is Hero)
            {
                strength = strength * 2;
            }

            if (sourceUnit.Troop is Leader)
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
        int j;
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
                for (int i = 1; i <= 6; i++)
                {
                    if (CombatMap.Current.PositionStatus[i] == 1)
                    {
                        j = i - 1;
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
    public int UnitSafe(TacticalBattleUnit unit, int x, int y)
    {
        int result;
        int distance;
        if (!(unit.Troop is Hero) || (_units.Count == 1 && _side == CombatSideIndex.Defender))
        {
            result = 0;
        }
        else
        {
            result = unit.GetTargetStrength() * 20 * unit.GetHitPoints() / unit.GetHits();
            if (unit.Troop is Leader)
            {
                result = result / 2;
            }

            for (int i = 0, last = _enemies.Count - 1; i <= last; i++)
            {
                var hexY = _enemies[i].UnitHexSprite.GetHexY();
                distance = HexGeometry.DeltaHexToRadius(x, y, _enemies[i].UnitHexSprite.GetHexX(), hexY);
                if (distance < 15 && (_enemies[i]).GetTargetStrength() > 0)
                {
                    result = result - (_enemies[i]).GetTargetStrength() * (15 - distance);
                }
            }

            for (int i = 0, last = _units.Count - 1; i <= last; i++)
            {
                if (_units[i] != unit)
                {
                    var hexY = _units[i].UnitHexSprite.GetHexY();
                    distance = HexGeometry.DeltaHexToRadius(x, y, _units[i].UnitHexSprite.GetHexX(), hexY);
                    if (distance < 15 && (_units[i]).GetTargetStrength() > 0)
                    {
                        result = result + (_units[i]).GetTargetStrength() * (15 - distance);
                    }
                }
            }

            for (int i = 0, last = _allies.Count - 1; i <= last; i++)
            {
                var hexY = _allies[i].UnitHexSprite.GetHexY();
                distance = HexGeometry.DeltaHexToRadius(x, y, _allies[i].UnitHexSprite.GetHexX(), hexY);
                if (distance < 15 && (_allies[i]).GetTargetStrength() > 0)
                {
                    result = result + (_allies[i]).GetTargetStrength() * (15 - distance);
                }
            }
        }

        return result;
    }

    // AoWTC.pas:880
    // The tactical AI passes the wall as Ignore. The original casts that object to TTacticalCombatUnit
    // without a check and compares the reference with the sprite's unit. Ignore is a combat object so the
    // wall reaches the same comparison.
    public int MakeTerrainPath(CombatMoveControlSettings moveControlSettings, ref MovePath path, ref int value, TacticalBattleUnit unit, CombatObject ignore, bool freeStrikes, bool findWalls, int cost)
    {
        int result;
        int j;
        int x = default;
        int y = default;
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
                for (int i = 0, last = path.Count - 1; i <= last; i++)
                {
                    hexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].Field[path.List[i].Location.X, path.List[i].Location.Y].FindOwnedChild(2228484);
                    if (freeStrikes && (hexSprite == null || hexSprite == unit.UnitHexSprite && i - 1 >= 0 && CombatMap.Current.MapContainer.MapLevels[0].Field[path.List[i - 1].Location.X, path.List[i - 1].Location.Y].FindOwnedChild(2228484) == null))
                    {
                        j = 1;
                        for (; j <= 6; j += 1)
                        {
                            HexGeometry.CenterHexNumberToHex(path.List[i].Location.X, path.List[i].Location.Y, j, ref x, ref y);
                            hexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].Field[x, y].FindOwnedChild(2228484);
                            if (hexSprite != null && hexSprite.Unit != ignore && hexSprite.Unit.GetSide() != unit.GetSide() && CombatObject.MeleeDamageTypes(hexSprite.Unit) != Set<DamageType>.Of() && !hexSprite.Unit.GetLocked() && hexSprite.Unit.GetAbilityEnabled(1) == unit.GetAbilityEnabled(1))
                            {
                                var tacticalBattleDamageValueToDamageEvaluation2 = TacticalBattleDamageValueToDamageEvaluation(CombatObject.StrikeDamageValue(hexSprite.Unit, unit), 0, hexSprite.Unit, unit);
                                value = value - tacticalBattleDamageValueToDamageEvaluation2 * 10;
                            }
                        }
                    }

                    if (i < path.Count - 1)
                    {
                        if (CombatMap.Current.Combat.CombatData.WallType != 0)
                        {
                            var pathY = path[i];
                            var pathX = path[i];
                            wall = (CityWall)CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y].FindOwnedChild(2229508);
                            if (wall == null)
                            {
                                var pathLocal = path[i];
                                var path2 = path[i];
                                wall = CombatMap.Current.MapContainer.MapLevels[0].Field[path2.Location.X, pathLocal.Location.Y].FindOwnedChild(2229510) as CityWall;
                            }
                        }
                        else
                        {
                            wall = null;
                        }

                        var path3 = path[i];
                        var path4 = path[i];
                        var mapField = CombatMap.Current.MapContainer.MapLevels[0].Field[path4.Location.X, path3.Location.Y];
                        var path5 = path[i];
                        var path6 = path[i];
                        var fieldItem = CombatMap.Current.MapContainer.MapLevels[0].Field[path6.Location.X, path5.Location.Y];
                        path.List[i] = path.List[i] with { MoveCost = moveControlSettings.MovePointTable[fieldItem.TerrainType][mapField.OverlayType - MovementTables.FirstOverlay] };
                        if (wall != null)
                        {
                            // { The wall lookup uses the current path point and the no-overlay column. }
                            if (_side == CombatSideIndex.Attacker && (unit.GetAbilityEnabled(65) || unit.GetAbilityEnabled(117)) && i == 0)
                            {
                                var pathY = path[i];
                                var pathX = path[i];
                                var item = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y];
                                path.List[i] = path.List[i] with { MoveCost = moveControlSettings.MovePointTable[item.TerrainType][-1 - MovementTables.FirstOverlay] };
                            }
                            else
                            {
                                if ((_side == CombatSideIndex.Attacker || CombatMap.Current.Combat.CombatData.WallBreached || _ignoreWalls) && (wall.HitPoints == 0 || (_side == CombatSideIndex.Defender && wall is CityWallDoor) || CombatMap.Current.MoveControl.UnitHexSprite.Unit.GetAbilityEnabled(1) || CombatMap.Current.MoveControl.UnitHexSprite.Unit.GetAbilityEnabled(66) || CombatMap.Current.MoveControl.UnitHexSprite.Unit.GetAbilityEnabled(36)))
                                {
                                    var pathY = path[i];
                                    var pathX = path[i];
                                    var item2 = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y];
                                    path.List[i] = path.List[i] with { MoveCost = moveControlSettings.MovePointTable[item2.TerrainType][-1 - MovementTables.FirstOverlay] };
                                }
                                else
                                {
                                    path.List[i] = path.List[i] with { MoveCost = 100 };
                                }
                            }

                            if (findWalls && wall.HitPoints > 0)
                            {
                                newWall = true;
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

                        var path7 = path[i];
                        var path8 = path[i];
                        if (CombatMap.Current.MapContainer.MapLevels[0].Field[path8.Location.X, path7.Location.Y].FindOwnedChild(2228484) != null)
                        {
                            var pathY = path[i];
                            var pathX = path[i];
                            var item2 = CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, pathY.Location.Y];
                            path.List[i] = path.List[i] with { MoveCost = moveControlSettings.MovePointTable[item2.TerrainType][-1 - MovementTables.FirstOverlay] };
                        }
                    }

                    result = result + path.List[i].MoveCost;
                }
            }
        }
        catch
        {
            MessageBox(0, "Error during Eval Movepath", NullTerminatedText("Error - TCAI " + unit.Troop.GetName()), 0);
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
    public void EvalAll(ref int value, List<TacticalBattleUnit> list, CombatObject ignore, int spell, TacticalBattleUnit U)
    {
        CombatObject target;
        AbilityDamageValues damageValue = default;
        for (int i = 0, last = list.Count - 1; i <= last; i++)
        {
            if (list[i] != ignore)
            {
                target = list[i];
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
    public void EvalArea(ref int value, CombatObject ignore, int ability, int spell, int x, int y, bool hitWalls, bool includeCenter, bool friendly, TacticalBattleUnit unit, object ignoreWall)
    {
        int x1 = default;
        int y1 = default;
        for (int i = 0; i <= 6; i++)
        {
            HexGeometry.CenterHexNumberToHex(x, y, i, ref x1, ref y1);
            if (includeCenter || unit.UnitHexSprite.GetHexX() != x1 || unit.UnitHexSprite.GetHexY() != y1)
            {
                EvalHex(ignore, ability, spell, x1, y1, ref value, hitWalls, friendly, unit, ignoreWall);
            }
        }
    }

    // AoWTC.pas:1084
    public int EvalPath(ref int value, CombatObject ignore, int ability, int spell, int x1, int y1, int x2, int y2, bool hitWalls, TacticalBattleUnit U, object ignoreWall)
    {
        int result;
        Point3DList point3DList;
        DelphiList<RangedObstacle> obstacles;
        int x = default;
        int y = default;
        int v = default;
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
                point3DList = new Point3DList();
                obstacles = new DelphiList<RangedObstacle>();
                if (effect == BattleEffectType.Breath)
                {
                    TacticalTables.MakeRangedPath(U.GetSide(), 50, 20, 50, (short)x1, (short)y1, (short)x2, (short)y2, 0, 0, false, ref point3DList, ref obstacles);
                }
                else
                {
                    TacticalTables.MakeAbilityPath(U.GetSide(), (short)ability, (short)spell, (short)x1, (short)y1, (short)x2, (short)y2, ref point3DList, ref obstacles);
                }

                total = 0;
                original = value;
                chance = 100;
                {
                    for (int i = 0, last = obstacles.Count - 1; i <= last; i++)
                    {
                        var obstacle = obstacles[i];
                        var path = point3DList[obstacle.PathIndex];
                        var obstacle2 = obstacles[i];
                        HexGeometry.PixelToHex(point3DList[obstacle2.PathIndex].X, path.Y, out x, out y);
                        v = 0;
                        EvalHex(ignore, ability, spell, x, y, ref v, hitWalls, true, U, ignoreWall);
                        try
                        {
                            var obstacle3 = obstacles[i];
                            total = total + v * chance / 100 * obstacle3.Chance / 100;
                        }
                        catch
                        {
                            total = total + obstacles[i].Chance * (v / 100 * chance / 100);
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

                value = value + total;
                point3DList?.Dispose();
                {
                    for (int i = 0, last = obstacles.Count - 1; i <= last; i++)
                    {
                        (obstacles[i] as IDisposable)?.Dispose();
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
        int v = default;
        int x = default;
        int y = default;
        var hexX = U.UnitHexSprite.GetHexX();
        var hexY = U.UnitHexSprite.GetHexY();
        int result = EvalPath(ref value, ignore, ability, spell, hexX, hexY, x2, y2, false, U, null);
        for (int i = 0; i <= 11; i++)
        {
            v = 0;
            var deltaHexToHexNumber2 = HexGeometry.DeltaHexToHexNumber(x1, y1, x2, y2);
            HexGeometry.CenterHexNumberToHex(x1, y1, TacticalTables.BreathHexNumber[TacticalTables.BreathDirection[deltaHexToHexNumber2 - 1] * 12 + i - 12], ref x, ref y);
            EvalHex(ignore, ability, spell, x, y, ref v, true, true, U, null);
            if (v != 0)
            {
                var fUnitHexSpriteHexX = U.UnitHexSprite.GetHexX();
                var fUnitHexSpriteHexY = U.UnitHexSprite.GetHexY();
                EvalPath(ref v, ignore, ability, spell, fUnitHexSpriteHexX, fUnitHexSpriteHexY, x, y, false, U, null);
            }

            value = value + v;
            result = result - v;
        }

        return result;
    }

    // AoWTC.pas:1149
    public void CheckUnit(int x2, int y2, int hits, int hitPoints, CombatMoveControlSettings settings, TacticalBattleUnit unit, CombatObject target, DelphiList<CombatAbilityRef> abilities, object wall)
    {
        int safeValue;
        int i;
        int v;
        int distance;
        int range;
        int cost = default;
        int bestIndex;
        int bestValue;
        int d = default;
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
        MovePath pathStepList;
        MovePath detourPathPathStepList;
        MovePath basePath;
        MeleeRound round;
        AbilityCombatInfo abilityInfo = default;
        SpellCombatInfo spellInfo = default;
        CityWall cityWall;
        bool adjacent;
        AbilityDamageValues values = default;
        pathStepList = new MovePath();
        basePath = new MovePath();
        detourPathPathStepList = new MovePath();
        terrainValue = 0;
        originalX = x2;
        originalY = y2;
        var yhx = unit.UnitHexSprite.GetHexY();
        distance = HexGeometry.DeltaHexToRadius(unit.UnitHexSprite.GetHexX(), yhx, x2, y2);
        if (unit.MovePoints > 0)
        {
            CombatMap.Current.MoveControl.CalculatePath(settings, basePath, (sbyte)x2, (sbyte)y2, 0);
            pathCost = MakeTerrainPath(settings, ref basePath, ref terrainValue, unit, target, true, target is TacticalBattleUnit && _side == CombatSideIndex.Attacker, 1);
            i = 0;
            int last = basePath.Count - 1;
            for (; i <= last; i += 1)
            {
                pathStepList.Add(basePath[i]);
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
                if (target is CombatUnit && target.GetSide() != unit.GetSide() && CombatObject.MeleeDamageTypes(unit) != Set<DamageType>.Of() && (unit.GetAbilityEnabled(1) || !target.GetAbilityEnabled(1)))
                {
                    ability = 0;
                    spell = 0;
                    round = new MeleeRound(Creator);
                    round.Calculate(unit, target);
                    if (round.GetAttackerDamageValue() > 0)
                    {
                        adjacent = pathStepList.MovePointsToPosition(unit.MovePoints) == 0;
                        unit.CanFlee = false;
                        if (target is CombatWall)
                        {
                            _flee = false;
                        }
                        value = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(damageValue2: round.GetDefenderDamageValue(), damageValue1: round.GetAttackerDamageValue(), source: unit, target: target), hitPoints, hits) + terrainValue;
                        if (target.GetTargetStrength() <= 0 && _enemies.Count > 1)
                        {
                            value -= 10000;
                        }

                        cost = (pathCost - unit.MovePoints) * 10;
                        if (cost < 1)
                        {
                            cost = 1;
                        }

                        if (!(unit.Troop is Hero))
                        {
                            safeValue = 0;
                        }
                        else
                        {
                            i = -1;
                            bestIndex = pathStepList.MovePointsToPosition(unit.MovePoints);
                            var pathY = pathStepList[bestIndex];
                            safeValue = UnitSafe(unit, pathStepList[bestIndex].Location.X, pathY.Location.Y);
                            while (bestIndex < pathStepList.Count - 1 && safeValue < 0)
                            {
                                var path = pathStepList[bestIndex];
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
                                if (value > 0 || _normalUnits == 0 || !(unit.Troop is Hero) || _units.Count <= 1)
                                {
                                    _heroMoves += 1;
                                    if (unit.Troop is Hero)
                                    {
                                        i = 50;
                                    }
                                    else
                                    {
                                        i = 100;
                                    }

                                    CreateMove(BattleAIMoveType.Melee, x2, y2, ScaleDamageEvaluation(value, i, 1), ability, spell, unit, target);
                                }
                            }
                            else
                            {
                                if (_ignoreWalls || ChargeFromPosition((int)(((uint)unit.Position) >> 4)))
                                {
                                    if (!((safeValue < 0 && _normalUnits != 0) || unit.MovePoints <= 0 || (pathStepList.List[pathStepList.Count - 2].MoveCost > unit.MovePoints && pathStepList.List[pathStepList.Count - 2].MoveCost <= 15)))
                                    {
                                        _heroMoves += 1;
                                        var multDamageEvaluation = ScaleDamageEvaluation(multiplier: unit.GetMoves(), value: value, divider: cost);
                                        var path = pathStepList[0];
                                        CreateMove(BattleAIMoveType.Move, pathStepList[0].Location.X, path.Location.Y, multDamageEvaluation, ability, spell, unit, target);
                                    }
                                    else
                                    {
                                        CreateMove(BattleAIMoveType.Fake, x2, y2, ScaleDamageEvaluation(multiplier: unit.GetMoves(), value: value, divider: cost), ability, spell, unit, target);
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
                pathStepList.Clear();
                i = 0;
                int limit = basePath.Count - 1;
                for (; i <= limit; i += 1)
                {
                    pathStepList.Add(basePath[i]);
                }

                ability = abilities[abilityIndex].Ability;
                spell = 0;
                x2 = originalX;
                y2 = originalY;
                GamePack.Current.AbilityControl.GetAbility(ability).GetCombatInfo(unit.Troop, ref abilityInfo);
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
                                        GamePack.Current.SpellControl.GetSpell(spell).GetCombatInfo(unit.Troop, ref spellInfo);
                                        GamePack.Current.SpellControl.GetSpell(spell).GetCombatDamageValues(unit, target, ref values);
                                        if (values.SourceDamage > 0)
                                        {
                                            unit.CanFlee = false;
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
                                                    value = TacticalBattleDamageValueToDamageEvaluation(values.SourceDamage, 0, unit, target);
                                                    break;
                                                default:
                                                    value = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(values.SourceDamage, 0, unit, target), hitPoints, hits);
                                                    break;
                                            }

                                            if (target.GetTargetStrength() <= 0 && _enemies.Count > 1)
                                            {
                                                value -= 10000;
                                            }

                                            if (unit.MovePoints > 0)
                                            {
                                                i = pathStepList.MovePointsToPosition(unit.MovePoints);
                                                var pathY = pathStepList[i];
                                                d = HexGeometry.DeltaHexToRadius(pathStepList[i].Location.X, pathY.Location.Y, x2, y2);
                                                if (d <= range || distance <= range)
                                                {
                                                    cost = value;
                                                    if (distance <= range)
                                                    {
                                                        switch (spell)
                                                        {
                                                            case 117:
                                                                var xhx = unit.UnitHexSprite.GetHexX();
                                                                var fUnitHexSpriteHexY = unit.UnitHexSprite.GetHexY();
                                                                pathValue = EvalPath(ref value, target, ability, spell, xhx, fUnitHexSpriteHexY, x2, y2, true, unit, wall);
                                                                EvalArea(ref value, target, ability, spell, x2, y2, true, true, true, unit, wall);
                                                                bestIndex = x2;
                                                                bestValue = y2;
                                                                {
                                                                    i = 1;
                                                                    for (; i <= 6; i += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(x2, y2, i, ref hexX, ref hexY);
                                                                        var hexY2 = unit.UnitHexSprite.GetHexY();
                                                                        if (HexGeometry.DeltaHexToRadius(unit.UnitHexSprite.GetHexX(), hexY2, hexX, hexY) <= range)
                                                                        {
                                                                            d = cost;
                                                                            var fUnitHexSpriteHexX = unit.UnitHexSprite.GetHexX();
                                                                            var hexY3 = unit.UnitHexSprite.GetHexY();
                                                                            v = EvalPath(ref d, target, ability, spell, fUnitHexSpriteHexX, hexY3, hexX, hexY, true, unit, wall);
                                                                            EvalArea(ref d, target, ability, spell, hexX, hexY, true, true, true, unit, wall);
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
                                                                        var hexY2 = unit.UnitHexSprite.GetHexY();
                                                                        HexGeometry.CenterHexNumberToHex(unit.UnitHexSprite.GetHexX(), hexY2, i, ref hexX, ref hexY);
                                                                        EvalHex(null, ability, spell, hexX, hexY, ref value, true, true, unit, wall);
                                                                    }
                                                                }

                                                                break;
                                                            case 128:
                                                                EvalHex(target, ability, spell, unit.UnitHexSprite.GetHexX(), unit.UnitHexSprite.GetHexY(), ref value, true, true, unit, wall);
                                                                {
                                                                    i = 0;
                                                                    for (; i <= 18; i += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(x2, y2, i, ref hexX, ref hexY);
                                                                        EvalHex(target, ability, spell, hexX, hexY, ref value, true, true, unit, wall);
                                                                    }
                                                                }

                                                                bestIndex = x2;
                                                                bestValue = y2;
                                                                {
                                                                    i = 1;
                                                                    for (; i <= 18; i += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(x2, y2, i, ref hexX, ref hexY);
                                                                        var hexY2 = unit.UnitHexSprite.GetHexY();
                                                                        if (HexGeometry.DeltaHexToRadius(unit.UnitHexSprite.GetHexX(), hexY2, hexX, hexY) <= range)
                                                                        {
                                                                            d = cost;
                                                                            EvalHex(target, ability, spell, unit.UnitHexSprite.GetHexX(), unit.UnitHexSprite.GetHexY(), ref d, true, true, unit, wall);
                                                                            v = 0;
                                                                            for (; v <= 18; v += 1)
                                                                            {
                                                                                HexGeometry.CenterHexNumberToHex(hexX, hexY, v, ref x, ref y);
                                                                                EvalHex(target, ability, spell, x, y, ref d, true, true, unit, wall);
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
                                                                var hexX2 = unit.UnitHexSprite.GetHexX();
                                                                var hexY4 = unit.UnitHexSprite.GetHexY();
                                                                pathValue = EvalPath(ref value, target, ability, spell, hexX2, hexY4, x2, y2, false, unit, wall);
                                                                break;
                                                            case 116:
                                                                EvalArea(ref value, target, ability, spell, x2, y2, true, true, true, unit, wall);
                                                                bestIndex = x2;
                                                                bestValue = y2;
                                                                {
                                                                    i = 1;
                                                                    for (; i <= 6; i += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(x2, y2, i, ref hexX, ref hexY);
                                                                        var hexY2 = unit.UnitHexSprite.GetHexY();
                                                                        if (HexGeometry.DeltaHexToRadius(unit.UnitHexSprite.GetHexX(), hexY2, hexX, hexY) <= range)
                                                                        {
                                                                            d = cost;
                                                                            EvalArea(ref d, target, ability, spell, hexX, hexY, true, true, true, unit, wall);
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
                                                                EvalArea(ref value, target, ability, spell, x2, y2, false, true, true, unit, wall);
                                                                bestIndex = x2;
                                                                bestValue = y2;
                                                                {
                                                                    i = 1;
                                                                    for (; i <= 6; i += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(x2, y2, i, ref hexX, ref hexY);
                                                                        var hexY2 = unit.UnitHexSprite.GetHexY();
                                                                        if (HexGeometry.DeltaHexToRadius(unit.UnitHexSprite.GetHexX(), hexY2, hexX, hexY) <= range)
                                                                        {
                                                                            d = cost;
                                                                            EvalArea(ref d, target, ability, spell, hexX, hexY, false, true, true, unit, wall);
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
                                                                EvalArea(ref value, target, ability, spell, x2, y2, false, false, false, unit, wall);
                                                                bestIndex = x2;
                                                                bestValue = y2;
                                                                {
                                                                    i = 1;
                                                                    for (; i <= 6; i += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(x2, y2, i, ref hexX, ref hexY);
                                                                        var hexY2 = unit.UnitHexSprite.GetHexY();
                                                                        if (HexGeometry.DeltaHexToRadius(unit.UnitHexSprite.GetHexX(), hexY2, hexX, hexY) <= range)
                                                                        {
                                                                            d = cost;
                                                                            EvalArea(ref d, target, ability, spell, hexX, hexY, false, false, false, unit, wall);
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
                                                                    value = -9999;
                                                                }
                                                                else
                                                                {
                                                                    EvalAll(ref value, _enemies, target, spell, unit);
                                                                    EvalAll(ref value, _allies, target, spell, unit);
                                                                    EvalAll(ref value, _units, target, spell, unit);
                                                                }

                                                                break;
                                                            case 125:
                                                                EvalAll(ref value, _enemies, target, spell, unit);
                                                                EvalAll(ref value, _allies, target, spell, unit);
                                                                EvalAll(ref value, _units, target, spell, unit);
                                                                break;
                                                            case 102:
                                                                EvalAll(ref value, _units, target, spell, unit);
                                                                break;
                                                            case 105:
                                                                EvalAll(ref value, _enemies, target, spell, unit);
                                                                break;
                                                            case 108:
                                                            case 114:
                                                                var hexX3 = unit.UnitHexSprite.GetHexX();
                                                                var hexY5 = unit.UnitHexSprite.GetHexY();
                                                                pathValue = EvalBreath(ref value, target, ability, spell, hexX3, hexY5, x2, y2, unit);
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
                                                            var path = pathStepList[i];
                                                            var pathX = pathStepList[i];
                                                            cityWall = (CityWall)CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, path.Location.Y].FindOwnedChild(2229508);
                                                            if (cityWall == null && _side == CombatSideIndex.Attacker)
                                                            {
                                                                var path2 = pathStepList[i];
                                                                var path3 = pathStepList[i];
                                                                cityWall = CombatMap.Current.MapContainer.MapLevels[0].Field[path3.Location.X, path2.Location.Y].FindOwnedChild(2229510) as CityWall;
                                                            }

                                                            if (!(cityWall == null || cityWall.HitPoints <= 0 || unit.GetAbilityEnabled(1) || unit.GetAbilityEnabled(36) || unit.GetAbilityEnabled(66)))
                                                            {
                                                                i = 1;
                                                            }
                                                            else
                                                            {
                                                                var path2 = pathStepList[i];
                                                                var path3 = pathStepList[i];
                                                                var path4 = pathStepList[0];
                                                                if (HexGeometry.DeltaHexToRadius(pathStepList[0].Location.X, path4.Location.Y, path3.Location.X, path2.Location.Y) <= range)
                                                                {
                                                                    var path5 = pathStepList[i];
                                                                    var path6 = pathStepList[i];
                                                                    v = EvalPath(ref cost, target, ability, spell, path5.Location.X, path6.Location.Y, x2, y2, spell == 117, unit, wall);
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
                                                                detourPathPathStepList.Add(pathStepList[i]);
                                                            }

                                                            terrainValue = 0;
                                                            pathCost = MakeTerrainPath(settings, ref detourPathPathStepList, ref terrainValue, unit, target, true, false, 1);
                                                            pathValue += bestValue;
                                                            pathCost = pathStepList.PositionToMovePoints(0);
                                                        }
                                                    }

                                                    bestIndex = 0;
                                                    i = 1;
                                                    int last3 = pathStepList.Count - 2;
                                                    for (; i <= last3; i += 1)
                                                    {
                                                        var path = pathStepList[i];
                                                        if (HexGeometry.DeltaHexToRadius(x2, y2, pathStepList[i].Location.X, path.Location.Y) <= range && pathStepList.PositionToMovePoints(i) <= unit.MovePoints)
                                                        {
                                                            cost = 1000;
                                                            var pathX = pathStepList[i];
                                                            var path2 = pathStepList[i];
                                                            v = EvalPath(ref cost, target, ability, spell, pathX.Location.X, path2.Location.Y, x2, y2, spell == 117, unit, wall);
                                                            if (v == 0 && cost == 1000)
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
                                                cost = (pathCost - unit.MovePoints - range * 4) * 10;
                                                if (cost < 1)
                                                {
                                                    cost = 1;
                                                }

                                                if (!(unit.Troop is Hero))
                                                {
                                                    safeValue = 0;
                                                }
                                                else
                                                {
                                                    i = -1;
                                                    bestIndex = pathStepList.MovePointsToPosition(unit.MovePoints);
                                                    var path = pathStepList[bestIndex];
                                                    safeValue = UnitSafe(unit, pathStepList[bestIndex].Location.X, path.Location.Y);
                                                    while (bestIndex < pathStepList.Count - 1 && safeValue < 0)
                                                    {
                                                        var path2 = pathStepList[bestIndex];
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

                                                if ((pathValue == 0 || value != 0) && distance <= range && unit.GetMoves() == unit.MovePoints)
                                                {
                                                    _heroMoves += 1;
                                                    i = Math.Abs(value + pathValue);
                                                    if (i < 1)
                                                    {
                                                        i = 1;
                                                    }

                                                    CreateMove(BattleAIMoveType.Ranged, x2, y2, ScaleDamageEvaluation(value, value * 100 / i, 1), ability, spell, unit, target);
                                                    if (pathValue > 0 && unit.MovePoints > 0 && detourPathPathStepList.Count > 1)
                                                    {
                                                        if (!(detourPathPathStepList.List[detourPathPathStepList.Count - 2].MoveCost > unit.MovePoints && detourPathPathStepList.List[detourPathPathStepList.Count - 2].MoveCost <= 15))
                                                        {
                                                            _heroMoves += 1;
                                                            var multDamageEvaluation = ScaleDamageEvaluation(pathValue + terrainValue, 100 * pathValue / i, 1);
                                                            var detourPath = detourPathPathStepList[0];
                                                            CreateMove(BattleAIMoveType.MoveRanged, detourPathPathStepList[0].Location.X, detourPath.Location.Y, multDamageEvaluation, ability, spell, unit, target);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (pathStepList.Count > 1 && (_ignoreWalls || ChargeFromPosition((int)(((uint)unit.Position) >> 4))))
                                                    {
                                                        if (!((safeValue < 0 && _normalUnits != 0) || unit.MovePoints <= 0 || pathStepList.Count <= 1 || (pathStepList.List[pathStepList.Count - 2].MoveCost > unit.MovePoints && pathStepList.List[pathStepList.Count - 2].MoveCost <= 15)))
                                                        {
                                                            var multDamageEvaluation = ScaleDamageEvaluation(multiplier: unit.GetMoves(), value: pathValue + terrainValue + value, divider: cost);
                                                            var path = pathStepList[0];
                                                            CreateMove(BattleAIMoveType.MoveRanged, pathStepList[0].Location.X, path.Location.Y, multDamageEvaluation, ability, spell, unit, target);
                                                        }
                                                        else
                                                        {
                                                            CreateMove(BattleAIMoveType.Fake, x2, y2, ScaleDamageEvaluation(multiplier: unit.GetMoves(), value: ScaleDamageEvaluation(pathValue, 100, 1) + terrainValue + value, divider: cost), ability, spell, unit, target);
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
                                if (target is CombatWall || target is CombatUnit && ((TouchAbility)GamePack.Current.AbilityControl.GetAbility(ability)).CanTouch(unit, target))
                                {
                                    try
                                    {
                                        GamePack.Current.AbilityControl.GetAbility(ability).TacticalBattleGetDamageValues(unit, target, ref values);
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

                                            value = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(values.SourceDamage, values.TargetDamage, unit, target), hitPoints, hits) + terrainValue;
                                            switch (ability)
                                            {
                                                case >= 27 and <= 29:
                                                case 40:
                                                case 60:
                                                case 106:
                                                case 107:
                                                case 148:
                                                    value = TacticalBattleDamageValueToDamageEvaluation(values.SourceDamage, 0, unit, target);
                                                    break;
                                                default:
                                                    value = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(values.SourceDamage, 0, unit, target), hitPoints, hits);
                                                    break;
                                            }

                                            if (ability == 117 && CombatObject.MeleeDamageTypes(unit) == Set<DamageType>.Of())
                                            {
                                                value = ScaleDamageEvaluation(value, 100, 1);
                                            }

                                            if (target.GetTargetStrength() <= 0 && _enemies.Count > 1)
                                            {
                                                value -= 10000;
                                            }

                                            if (unit.MovePoints > 0 && pathStepList.MovePointsToPosition(unit.MovePoints) == 0)
                                            {
                                                switch (ability)
                                                {
                                                    case 65:
                                                        EvalArea(ref value, target, ability, spell, pathStepList.List[1].Location.X, pathStepList.List[1].Location.Y, true, false, true, unit, wall);
                                                        break;
                                                    case 114:
                                                        EvalArea(ref value, target, ability, spell, pathStepList.List[1].Location.X, pathStepList.List[1].Location.Y, false, false, true, unit, wall);
                                                        break;
                                                }
                                            }

                                            cost = (pathCost - unit.MovePoints) * 10;
                                            if (cost < 1)
                                            {
                                                cost = 1;
                                            }

                                            if (!(unit.Troop is Hero))
                                            {
                                                safeValue = 0;
                                            }
                                            else
                                            {
                                                i = -1;
                                                bestIndex = pathStepList.MovePointsToPosition(unit.MovePoints);
                                                var pathY = pathStepList[bestIndex];
                                                safeValue = UnitSafe(unit, pathStepList[bestIndex].Location.X, pathY.Location.Y);
                                                while (bestIndex < pathStepList.Count - 1 && safeValue < 0)
                                                {
                                                    var path = pathStepList[bestIndex];
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
                                                    if (unit.Troop is Hero)
                                                    {
                                                        i = 1;
                                                    }
                                                    else
                                                    {
                                                        if (target is CombatWall && CombatMap.Current.Combat.CombatData.WallBreached && CombatMap.Current.Round > 1)
                                                        {
                                                            i = 10;
                                                        }
                                                        else
                                                        {
                                                            i = 100;
                                                        }
                                                    }

                                                    CreateMove(BattleAIMoveType.Touch, x2, y2, ScaleDamageEvaluation(value, i, 1), ability, spell, unit, target);
                                                }
                                                else
                                                {
                                                    if (_ignoreWalls || ChargeFromPosition((int)(((uint)unit.Position) >> 4)))
                                                    {
                                                        if (!((safeValue < 0 && _normalUnits != 0) || unit.MovePoints <= 0 || (pathStepList.List[pathStepList.Count - 2].MoveCost > unit.MovePoints && pathStepList.List[pathStepList.Count - 2].MoveCost <= 15)))
                                                        {
                                                            _heroMoves += 1;
                                                            var multDamageEvaluation = ScaleDamageEvaluation(multiplier: unit.GetMoves(), value: value, divider: cost);
                                                            var path = pathStepList[0];
                                                            CreateMove(BattleAIMoveType.Move, pathStepList[0].Location.X, path.Location.Y, multDamageEvaluation, ability, spell, unit, target);
                                                        }
                                                        else
                                                        {
                                                            CreateMove(BattleAIMoveType.Fake, x2, y2, ScaleDamageEvaluation(multiplier: unit.GetMoves(), value: value, divider: cost), ability, spell, unit, target);
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
                                    cost = GamePack.Current.AbilityControl.GetAbility(ability).TacticalBattleGetDamageValue(unit, target);
                                    if (cost > 0)
                                    {
                                        unit.CanFlee = false;
                                        if (target is CombatWall)
                                        {
                                            _flee = false;
                                        }

                                        pathValue = 0;
                                        if (GamePack.Current.AbilityControl.GetAbility(ability).GetCombatMode(unit.Troop) == CombatMode.LongRange)
                                        {
                                            range = 12;
                                        }
                                        else
                                        {
                                            if (GamePack.Current.AbilityControl.GetAbility(ability).GetCombatMode(unit.Troop) == CombatMode.MediumRange)
                                            {
                                                range = 8;
                                            }
                                            else
                                            {
                                                range = 4;
                                            }
                                        }

                                        value = ScaleDamageEvaluation(TacticalBattleDamageValueToDamageEvaluation(cost, 0, unit, target), hitPoints, hits);
                                        if (target.GetTargetStrength() <= 0 && _enemies.Count > 1)
                                        {
                                            value -= 10000;
                                        }

                                        if (unit.MovePoints > 0)
                                        {
                                            i = pathStepList.MovePointsToPosition(unit.MovePoints);
                                            var pathY = pathStepList[i];
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
                                                            var xhx = unit.UnitHexSprite.GetHexX();
                                                            var fUnitHexSpriteHexY = unit.UnitHexSprite.GetHexY();
                                                            pathValue = EvalPath(ref value, target, ability, spell, xhx, fUnitHexSpriteHexY, x2, y2, false, unit, wall);
                                                            break;
                                                        case 24:
                                                        case 44:
                                                            var fUnitHexSpriteHexX = unit.UnitHexSprite.GetHexX();
                                                            var hexY2 = unit.UnitHexSprite.GetHexY();
                                                            pathValue = EvalPath(ref value, target, ability, spell, fUnitHexSpriteHexX, hexY2, x2, y2, true, unit, wall);
                                                            break;
                                                        case 30:
                                                        case >= 86 and <= 90:
                                                            var hexX2 = unit.UnitHexSprite.GetHexX();
                                                            var hexY3 = unit.UnitHexSprite.GetHexY();
                                                            pathValue = EvalBreath(ref value, target, ability, spell, hexX2, hexY3, x2, y2, unit);
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
                                                        var path = pathStepList[i];
                                                        var pathX = pathStepList[i];
                                                        cityWall = (CityWall)CombatMap.Current.MapContainer.MapLevels[0].Field[pathX.Location.X, path.Location.Y].FindOwnedChild(2229508);
                                                        if (cityWall == null && _side == CombatSideIndex.Attacker)
                                                        {
                                                            var path2 = pathStepList[i];
                                                            var path3 = pathStepList[i];
                                                            cityWall = CombatMap.Current.MapContainer.MapLevels[0].Field[path3.Location.X, path2.Location.Y].FindOwnedChild(2229510) as CityWall;
                                                        }

                                                        if (!(cityWall == null || cityWall.HitPoints <= 0 || unit.GetAbilityEnabled(1) || unit.GetAbilityEnabled(36) || unit.GetAbilityEnabled(66)))
                                                        {
                                                            i = 1;
                                                        }
                                                        else
                                                        {
                                                            var path2 = pathStepList[i];
                                                            var path3 = pathStepList[i];
                                                            var path4 = pathStepList[0];
                                                            if (HexGeometry.DeltaHexToRadius(pathStepList[0].Location.X, path4.Location.Y, path3.Location.X, path2.Location.Y) <= range)
                                                            {
                                                                var path5 = pathStepList[i];
                                                                var path6 = pathStepList[i];
                                                                v = EvalPath(ref cost, target, ability, spell, path5.Location.X, path6.Location.Y, x2, y2, ability == 24 || ability == 44, unit, wall);
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
                                                            detourPathPathStepList.Add(pathStepList[i]);
                                                        }

                                                        terrainValue = 0;
                                                        pathCost = MakeTerrainPath(settings, ref detourPathPathStepList, ref terrainValue, unit, target, true, false, 1);
                                                        pathValue += bestValue;
                                                        pathCost = pathStepList.PositionToMovePoints(0);
                                                    }
                                                }

                                                bestIndex = 0;
                                                i = 1;
                                                int last3 = pathStepList.Count - 2;
                                                for (; i <= last3; i += 1)
                                                {
                                                    var path = pathStepList[i];
                                                    if (HexGeometry.DeltaHexToRadius(x2, y2, pathStepList[i].Location.X, path.Location.Y) <= range && pathStepList.PositionToMovePoints(i) <= unit.MovePoints)
                                                    {
                                                        cost = 1000;
                                                        var pathX = pathStepList[i];
                                                        var path2 = pathStepList[i];
                                                        v = EvalPath(ref cost, target, ability, spell, pathX.Location.X, path2.Location.Y, x2, y2, spell == 117, unit, wall);
                                                        if (v == 0 && cost == 1000)
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
                                            cost = (pathCost - unit.MovePoints - range * 4) * 10;
                                            if (cost < 1)
                                            {
                                                cost = 1;
                                            }

                                            if (!(unit.Troop is Hero))
                                            {
                                                safeValue = 0;
                                            }
                                            else
                                            {
                                                i = -1;
                                                bestIndex = pathStepList.MovePointsToPosition(unit.MovePoints);
                                                var path = pathStepList[bestIndex];
                                                safeValue = UnitSafe(unit, pathStepList[bestIndex].Location.X, path.Location.Y);
                                                while (bestIndex < pathStepList.Count - 1 && safeValue < 0)
                                                {
                                                    var path2 = pathStepList[bestIndex];
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

                                            var hexY4 = unit.UnitHexSprite.GetHexY();
                                            if (!((UnitSafe(unit, unit.UnitHexSprite.GetHexX(), hexY4) < 0 && _normalUnits != 0) || (pathValue != 0 && value == 0) || distance > range || unit.GetMoves() != unit.MovePoints))
                                            {
                                                _heroMoves += 1;
                                                if (target is CombatWall && CombatMap.Current.Combat.CombatData.WallBreached && CombatMap.Current.Round > 1)
                                                {
                                                    i = 10;
                                                }
                                                else
                                                {
                                                    i = 100;
                                                }

                                                v = Math.Abs(value + pathValue);
                                                if (v < 1)
                                                {
                                                    v = 1;
                                                }

                                                CreateMove(BattleAIMoveType.Ranged, x2, y2, ScaleDamageEvaluation(value, i * value / v, 1), ability, spell, unit, target);
                                                if (pathValue > 0 && unit.MovePoints > 0 && detourPathPathStepList.Count > 1)
                                                {
                                                    if (!(detourPathPathStepList.List[detourPathPathStepList.Count - 2].MoveCost > unit.MovePoints && detourPathPathStepList.List[detourPathPathStepList.Count - 2].MoveCost <= 15))
                                                    {
                                                        _heroMoves += 1;
                                                        var multDamageEvaluation = ScaleDamageEvaluation(pathValue + terrainValue, i * pathValue / v, 1);
                                                        var detourPath = detourPathPathStepList[0];
                                                        CreateMove(BattleAIMoveType.MoveRanged, detourPathPathStepList[0].Location.X, detourPath.Location.Y, multDamageEvaluation, ability, spell, unit, target);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (pathStepList.Count > 1 && (_ignoreWalls || ChargeFromPosition((int)(((uint)unit.Position) >> 4))))
                                                {
                                                    if (!((safeValue < 0 && _normalUnits != 0) || unit.MovePoints <= 0 || pathStepList.Count <= 1 || (pathStepList.List[pathStepList.Count - 2].MoveCost > unit.MovePoints && pathStepList.List[pathStepList.Count - 2].MoveCost <= 15)))
                                                    {
                                                        var multDamageEvaluation = ScaleDamageEvaluation(multiplier: unit.GetMoves(), value: pathValue + terrainValue + value, divider: cost);
                                                        var path = pathStepList[0];
                                                        CreateMove(BattleAIMoveType.MoveRanged, pathStepList[0].Location.X, path.Location.Y, multDamageEvaluation, ability, spell, unit, target);
                                                    }
                                                    else
                                                    {
                                                        CreateMove(BattleAIMoveType.Fake, x2, y2, ScaleDamageEvaluation(multiplier: unit.GetMoves(), value: ScaleDamageEvaluation(pathValue, 100, 1) + terrainValue + value, divider: cost), ability, spell, unit, target);
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

        pathStepList?.Dispose();
        basePath?.Dispose();
        detourPathPathStepList?.Dispose();
    }

    // AoWTC.pas:1628
    public void EvalBattle()
    {
        int i;
        int j;
        int k = default;
        int l = default;
        int n;
        int value = default;
        BattleAIMove move;
        BattleAIMove otherMove;
        TacticalBattleUnit u;
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
        MovePath pathStepList = default;
        AbilityCombatInfo aiInfo = default;
        SpellCombatInfo spellInfo = default;
        CityWall cityWall;
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
                        i = 0;
                        int last = _moves.Count - 1;
                        for (; i <= last; i += 1)
                        {
                            (_moves[i] as IDisposable)?.Dispose();
                        }

                        _moves.Clear();
                        _walls.Clear();
                        var tacticalBattle = CombatMap.Current.Combat;
                        _side = (CombatSideIndex)tacticalBattle.CombatData.Players.GetPlayers((sbyte)tacticalBattle.CombatData.CurrentPlayer).SideIndex;
                        _normalUnits = 0;
                        i = 0;
                        int limit = tacticalBattle.CombatData.GetCount() - 1;
                        for (; i <= limit; i += 1)
                        {
                            if (tacticalBattle.CombatData.GetObjects(i) != null && tacticalBattle.CombatData.GetObjects(i) is CombatUnit && tacticalBattle.CombatData.GetObjects(i).GetEnabled())
                            {
                                visible = false;
                                j = 0;
                                while (!visible && j < 19)
                                {
                                    var hexY = tacticalBattle.CombatData.GetObjects<TacticalBattleUnit>(i).UnitHexSprite.GetHexY();
                                    HexGeometry.CenterHexNumberToHex(tacticalBattle.CombatData.GetObjects<TacticalBattleUnit>(i).UnitHexSprite.GetHexX(), hexY, j, ref k, ref l);
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
                                    if (tacticalBattle.CombatData.GetObjects(i).GetSide() == _side)
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
                                                    _units.Insert(0, tacticalBattle.CombatData.GetObjects<TacticalBattleUnit>(i));
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

                        Creator.GetEngine().OutputDebugString("AI found " + IntToStr(_units.Count) + " Own units.");
                        Creator.GetEngine().OutputDebugString("AI found " + IntToStr(_allies.Count) + " Ally units.");
                        Creator.GetEngine().OutputDebugString("AI found " + IntToStr(_enemies.Count) + " Enemy units.");
                        _ignoreWalls = _side == CombatSideIndex.Attacker;
                        _flee = _side == CombatSideIndex.Attacker && CombatMap.Current.Combat.CombatData.WallType != 0 && !CombatMap.Current.Combat.CombatData.WallBreached;
                        _minMoves = 100;
                        i = 0;
                        int last2 = _units.Count - 1;
                        for (; i <= last2; i += 1)
                        {
                            u = _units[i];
                            u.CanFlee = _side == CombatSideIndex.Attacker;
                            if (u.GetMoves() < _minMoves)
                            {
                                _minMoves = u.GetMoves();
                            }

                            j = 0;
                            int last3 = _enemies.Count - 1;
                            for (; j <= last3; j += 1)
                            {
                                target = _enemies[j];
                                if (!_ignoreWalls && target.GetSide() != u.GetSide())
                                {
                                    var hexY = target.UnitHexSprite.GetHexY();
                                    var hexX = target.UnitHexSprite.GetHexX();
                                    var fUnitHexSpriteHexY = u.UnitHexSprite.GetHexY();
                                    k = HexGeometry.DeltaHexToRadius(u.UnitHexSprite.GetHexX(), fUnitHexSpriteHexY, hexX, hexY);
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
                                                        var spell = GamePack.Current.SpellControl.GetSpell(n);
                                                        condition = ((Hero)target.Troop).CanCastSpellInstantly(n, spell.BaseManaCost, ref reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        GamePack.Current.SpellControl.GetSpell(n).GetCombatInfo(target.Troop, ref spellInfo);
                                                        if (spellInfo.TargetTypes.Contains(CombatTargetTypeElement.Enemy) && (k <= spellInfo.Range || spellInfo.Range == -1))
                                                        {
                                                            value = GamePack.Current.SpellControl.GetSpell(n).GetCombatDamageValue(target, u);
                                                            if (value > 0)
                                                            {
                                                                if (Set<BattleEffectType>.Of(BattleEffectType.Arrow, BattleEffectType.Rock, BattleEffectType.Comet, BattleEffectType.Breath, BattleEffectType.CometBall, BattleEffectType.Ray).Contains(TacticalTables.SpellTypes[n]))
                                                                {
                                                                    var fUnitHexSpriteHexX = target.UnitHexSprite.GetHexX();
                                                                    var hexY2 = target.UnitHexSprite.GetHexY();
                                                                    var hexX2 = u.UnitHexSprite.GetHexX();
                                                                    var hexY3 = u.UnitHexSprite.GetHexY();
                                                                    EvalPath(ref value, u, l, n, fUnitHexSpriteHexX, hexY2, hexX2, hexY3, false, target, null);
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
                                                if (TacticalTables.AbilityTypes[l] != BattleEffectType.None && TacticalTables.AbilityTypes[l] != BattleEffectType.Touch)
                                                {
                                                    GamePack.Current.AbilityControl.GetAbility(l).GetCombatInfo(target.Troop, ref aiInfo);
                                                    if ((((byte)aiInfo.TargetTypes.Image) & 2) != 0)
                                                    {
                                                        if (GamePack.Current.AbilityControl.GetAbility(l).GetCombatMode(target.Troop) == CombatMode.LongRange)
                                                        {
                                                            n = 12;
                                                        }
                                                        else
                                                        {
                                                            if (GamePack.Current.AbilityControl.GetAbility(l).GetCombatMode(target.Troop) == CombatMode.MediumRange)
                                                            {
                                                                n = 8;
                                                            }
                                                            else
                                                            {
                                                                n = 4;
                                                            }
                                                        }

                                                        if (k <= n)
                                                        {
                                                            value = GamePack.Current.AbilityControl.GetAbility(l).TacticalBattleGetDamageValue(target, u);
                                                            if (value > 0)
                                                            {
                                                                if (Set<BattleEffectType>.Of(BattleEffectType.Arrow, BattleEffectType.Rock, BattleEffectType.Comet, BattleEffectType.Breath, BattleEffectType.CometBall, BattleEffectType.Ray).Contains(TacticalTables.AbilityTypes[l]))
                                                                {
                                                                    var fUnitHexSpriteHexX = target.UnitHexSprite.GetHexX();
                                                                    var hexY2 = target.UnitHexSprite.GetHexY();
                                                                    var hexX2 = u.UnitHexSprite.GetHexX();
                                                                    var hexY3 = u.UnitHexSprite.GetHexY();
                                                                    EvalPath(ref value, u, l, 0, fUnitHexSpriteHexX, hexY2, hexX2, hexY3, false, target, null);
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
                            u = _units[_unitIndex];
                            if (!u.GetLocked())
                            {
                                abilities = new DelphiList<CombatAbilityRef>();
                                try
                                {
                                    j = 0;
                                    for (; j <= 169; j += 1)
                                    {
                                        if (u.GetAbilityEnabled(j))
                                        {
                                            switch (j)
                                            {
                                                case 1:
                                                case 36:
                                                case 66:
                                                case 117:
                                                    _flee = false;
                                                    break;
                                            }

                                            if (j == 52 && u.Troop is Hero)
                                            {
                                                k = 0;
                                                for (; k <= 130; k += 1)
                                                {
                                                    bool condition = u.Troop.Player > 0 && TacticalTables.SpellTypes[k] != BattleEffectType.None && StrategicMap.Current.Players.GetPlayers(u.Troop.Player).MagicControl.Spells.IndexOf(k) != -1;
                                                    if (condition)
                                                    {
                                                        var spell = GamePack.Current.SpellControl.GetSpell(k);
                                                        condition = ((Hero)u.Troop).CanCastSpellInstantly(k, spell.BaseManaCost, ref reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        GamePack.Current.SpellControl.GetSpell(k).GetCombatInfo(u.Troop, ref spellInfo);
                                                        if (spellInfo.TargetTypes.Contains(CombatTargetTypeElement.Enemy))
                                                        {
                                                            abilities.Add(new CombatAbilityRef());
                                                            abilities[abilities.Count - 1].Ability = j;
                                                            abilities[abilities.Count - 1].Spell = k;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (TacticalTables.AbilityTypes[j] != BattleEffectType.None)
                                                {
                                                    GamePack.Current.AbilityControl.GetAbility(j).GetCombatInfo(u.Troop, ref aiInfo);
                                                    if ((((byte)aiInfo.TargetTypes.Image) & 2) != 0)
                                                    {
                                                        abilities.Add(new CombatAbilityRef());
                                                        abilities[abilities.Count - 1].Ability = j;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    MessageBox(0, "Error during Eval Enemies", NullTerminatedText("Error - TCAI " + u.Troop.GetName()), 0);
                                }

                                settings = new CombatMoveControlSettings(null) { Holder = Creator.CreationHolder() };
                                if (u.MovePoints > 0)
                                {
                                    var hexY = u.UnitHexSprite.GetHexY();
                                    settings.SetSourcePosition(u.UnitHexSprite.GetHexX(), hexY, 0);
                                    settings.ArmyHexSprite = u.UnitHexSprite;
                                    settings.MoveFlags = Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Melee, CombatMoveModeElement.Move, CombatMoveModeElement.Wall);
                                    var movePointTable = settings.MovePointTable;
                                    u.Troop.CreateMovePointTable(out movePointTable);
                                    settings.MovePointTable = movePointTable;
                                    settings.Player = u.GetPlayer();
                                    settings.MovePointsMax = 200;
                                    CombatMap.Current.MoveControl.ProcessMove(settings);
                                }

                                if (abilities.Count > 0 || CombatObject.MeleeDamageTypes(u) != Set<DamageType>.Of())
                                {
                                    j = 0;
                                    int last = _enemies.Count - 1;
                                    for (; j <= last; j += 1)
                                    {
                                        target = _enemies[j];
                                        if (!(u.Troop is Hero) || _heroMoves == 0)
                                        {
                                            var hits = target.GetHits();
                                            var hitPoints = target.GetHitPoints();
                                            var hexY = target.UnitHexSprite.GetHexY();
                                            CheckUnit(target.UnitHexSprite.GetHexX(), hexY, hitPoints, hits, settings, u, target, abilities, null);
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
                                                        var spell = GamePack.Current.SpellControl.GetSpell(k);
                                                        condition = ((Hero)u.Troop).CanCastSpellInstantly(k, spell.BaseManaCost, ref reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        GamePack.Current.SpellControl.GetSpell(k).GetCombatInfo(u.Troop, ref spellInfo);
                                                        if (spellInfo.TargetTypes.Contains(CombatTargetTypeElement.Friend))
                                                        {
                                                            abilities.Add(new CombatAbilityRef());
                                                            abilities[abilities.Count - 1].Ability = j;
                                                            abilities[abilities.Count - 1].Spell = k;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (TacticalTables.AbilityTypes[j] != BattleEffectType.None)
                                                {
                                                    GamePack.Current.AbilityControl.GetAbility(j).GetCombatInfo(u.Troop, ref aiInfo);
                                                    if ((((byte)aiInfo.TargetTypes.Image) & 1) != 0)
                                                    {
                                                        abilities.Add(new CombatAbilityRef());
                                                        abilities[abilities.Count - 1].Ability = j;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    MessageBox(0, "Error during Eval Friends", NullTerminatedText("Error - TCAI " + u.Troop.GetName()), 0);
                                }

                                if (abilities.Count > 0)
                                {
                                    j = 0;
                                    int last = _units.Count - 1;
                                    for (; j <= last; j += 1)
                                    {
                                        target = _units[j];
                                        if (!(u.Troop is Hero) || _heroMoves == 0)
                                        {
                                            var hits = target.GetHits();
                                            var hitPoints = target.GetHitPoints();
                                            var hexY = target.UnitHexSprite.GetHexY();
                                            CheckUnit(target.UnitHexSprite.GetHexX(), hexY, hitPoints, hits, settings, u, target, abilities, null);
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
                                if (CombatMap.Current.Combat.CombatData.WallType != 0 && CombatMap.Current.Combat.CombatData.Wall != null && _side == CombatSideIndex.Attacker)
                                {
                                    j = 0;
                                    int last = CombatMap.Current.Walls.Count - 1;
                                    for (; j <= last; j += 1)
                                    {
                                        bool condition = (CombatMap.Current.Walls[j]).HitPoints > 0;
                                        if (condition)
                                        {
                                            var wall = CombatMap.Current.Walls[j];
                                            var wall2 = CombatMap.Current.Walls[j];
                                            condition = wall.HitPoints < TacticalTables.WallMaxHitPoints[wall2.WallType - 1];
                                        }

                                        if (condition)
                                        {
                                            found = true;
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
                                                var wall = CombatMap.Current.Walls[j];
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
                            u = _units[_unitIndex];
                            if (!u.GetLocked())
                            {
                                abilities = new DelphiList<CombatAbilityRef>();
                                j = 0;
                                for (; j <= 169; j += 1)
                                {
                                    if (u.GetAbilityEnabled(j))
                                    {
                                        switch (j)
                                        {
                                            case 1:
                                            case 36:
                                            case 66:
                                            case 117:
                                                _flee = false;
                                                break;
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
                                                        var spell = GamePack.Current.SpellControl.GetSpell(k);
                                                        condition = ((Hero)u.Troop).CanCastSpellInstantly(k, spell.BaseManaCost, ref reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        GamePack.Current.SpellControl.GetSpell(k).GetCombatInfo(u.Troop, ref spellInfo);
                                                        if (spellInfo.DamageTypes.Contains(DamageType.Wall))
                                                        {
                                                            _flee = false;
                                                            abilities.Add(new CombatAbilityRef());
                                                            abilities[abilities.Count - 1].Ability = j;
                                                            abilities[abilities.Count - 1].Spell = k;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (TacticalTables.AbilityTypes[j] != BattleEffectType.None)
                                            {
                                                GamePack.Current.AbilityControl.GetAbility(j).GetCombatInfo(u.Troop, ref aiInfo);
                                                if (aiInfo.DamageTypes.Contains(DamageType.Wall))
                                                {
                                                    _flee = false;
                                                    abilities.Add(new CombatAbilityRef());
                                                    abilities[abilities.Count - 1].Ability = j;
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
                                        var hexY = u.UnitHexSprite.GetHexY();
                                        settings.SetSourcePosition(u.UnitHexSprite.GetHexX(), hexY, 0);
                                        settings.ArmyHexSprite = u.UnitHexSprite;
                                        settings.MoveFlags = Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Melee, CombatMoveModeElement.Move, CombatMoveModeElement.Wall);
                                        if (u.GetAbilityEnabled(65) || u.GetAbilityEnabled(117))
                                        {
                                            settings.MoveFlags = settings.MoveFlags + Set<CombatMoveModeElement>.Of(CombatMoveModeElement.WallCrushing);
                                        }

                                        var movePointTable2 = settings.MovePointTable;
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
                                        if (!(u.Troop is Hero) || _heroMoves == 0)
                                        {
                                            var hexY = cityWall.GetHexY();
                                            CheckUnit(cityWall.GetHexX(), hexY, cityWall.HitPoints, TacticalTables.WallMaxHitPoints[cityWall.WallType - 1] * 2, settings, u, CombatMap.Current.Combat.CombatData.Wall, abilities, cityWall);
                                        }
                                    }

                                    if (_walls.Count == 0)
                                    {
                                        l = 100;
                                        cityWall = null;
                                        j = 0;
                                        int limit = CombatMap.Current.Walls.Count - 1;
                                        for (; j <= limit; j += 1)
                                        {
                                            var hexY = u.UnitHexSprite.GetHexY();
                                            var hexY2 = (CombatMap.Current.Walls[j]).GetHexY();
                                            n = HexGeometry.DeltaHexToRadius((CombatMap.Current.Walls[j]).GetHexX(), hexY2, u.UnitHexSprite.GetHexX(), hexY);
                                            if ((CombatMap.Current.Walls[j]).HitPoints > 0 && n < l)
                                            {
                                                cityWall = CombatMap.Current.Walls[j];
                                                l = n;
                                            }
                                        }

                                        if (cityWall != null)
                                        {
                                            var hexY = cityWall.GetHexY();
                                            CheckUnit(cityWall.GetHexX(), hexY, cityWall.HitPoints * 2, TacticalTables.WallMaxHitPoints[cityWall.WallType - 1], settings, u, CombatMap.Current.Combat.CombatData.Wall, abilities, cityWall);
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

                                j = 0;
                                int last2 = _moves.Count - 1;
                                for (; j <= last2; j += 1)
                                {
                                    otherMove = _moves[j];
                                    if (move.UnitHexSprite.Unit != otherMove.UnitHexSprite.Unit)
                                    {
                                        k = HexGeometry.DeltaHexToRadius(move.X, move.Y, otherMove.X, otherMove.Y) * 4 - otherMove.UnitHexSprite.Unit.MovePoints;
                                        if (k < 4)
                                        {
                                            k = 4;
                                        }

                                        l = otherMove.UnitHexSprite.Unit.GetMoves() * (otherMove.Value / (k * 10));
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
                                    if (move.GroupValue > -1000000 && move.GroupValue < 1000000)
                                    {
                                        var bonus = bonuses[j];
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
                        i = 0;
                        int last = _units.Count - 1;
                        for (; i <= last; i += 1)
                        {
                            u = _units[i];
                            if (u.Troop is Hero && _heroMoves == 0 && u.MovePoints > 0)
                            {
                                var hexY = u.UnitHexSprite.GetHexY();
                                k = UnitSafe(u, u.UnitHexSprite.GetHexX(), hexY);
                                value = k;
                                n = -1;
                                j = 0;
                                int limit = _units.Count - 1;
                                for (; j <= limit; j += 1)
                                {
                                    target = _units[j];
                                    if (u != target)
                                    {
                                        var fUnitHexSpriteHexY = target.UnitHexSprite.GetHexY();
                                        l = UnitSafe(u, target.UnitHexSprite.GetHexX(), fUnitHexSpriteHexY);
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
                                    var hexX = u.UnitHexSprite.GetHexX();
                                    settings.SetSourcePosition(u.UnitHexSprite.GetHexX(), hexX, 0);
                                    var fUnitHexSpriteHexY = (_units[n]).UnitHexSprite.GetHexY();
                                    settings.SetDestinationPosition((_units[n]).UnitHexSprite.GetHexX(), fUnitHexSpriteHexY, 0);
                                    settings.ArmyHexSprite = u.UnitHexSprite;
                                    settings.MoveFlags = Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Melee, CombatMoveModeElement.Move, CombatMoveModeElement.Wall);
                                    var movePointTable3 = settings.MovePointTable;
                                    u.Troop.CreateMovePointTable(out movePointTable3);
                                    settings.MovePointTable = movePointTable3;
                                    settings.Player = u.GetPlayer();
                                    CombatMap.Current.MoveControl.ProcessMove(settings);
                                    if (settings.Path.Count > 2)
                                    {
                                        settings.Path.Delete(0);
                                        var path = settings.Path[0];
                                        CreateMove(BattleAIMoveType.Move, settings.Path[0].Location.X, path.Location.Y, k, 0, 0, u, null);
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
                                if ((!(u.Troop is Hero) || _heroMoves == 0 || (u.Troop is Hero && _units.Count == 1)) && u.GetEnabled() && !u.GetLocked() && u.MovePoints > 0 && (_flee || u.CanFlee))
                                {
                                    j = 0;
                                    int last2 = CombatMap.Current.FleeLocations.Count - 1;
                                    for (; j <= last2; j += 1)
                                    {
                                        var hexY = u.UnitHexSprite.GetHexY();
                                        var hexY2 = (CombatMap.Current.FleeLocations[j]).GetHexY();
                                        var deltaHexToRadius2 = HexGeometry.DeltaHexToRadius((CombatMap.Current.FleeLocations[j]).GetHexX(), hexY2, u.UnitHexSprite.GetHexX(), hexY);
                                        var hexY3 = (CombatMap.Current.FleeLocations[j]).GetHexY();
                                        CreateMove(BattleAIMoveType.Move, (CombatMap.Current.FleeLocations[j]).GetHexX(), hexY3, 110000000 - deltaHexToRadius2 * 10, 0, 0, u, null);
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
                                default:
                                    {
                                        if (move.Value > -100000000 && move.Value < 100000000)
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
                                    var relocated = _moves[i];
                                    _moves.RemoveAt(i);
                                    _moves.Insert(k, relocated);
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

                                        var movePointTable4 = settings.MovePointTable;
                                        move.UnitHexSprite.Unit.Troop.CreateMovePointTable(out movePointTable4);
                                        settings.MovePointTable = movePointTable4;
                                        settings.Player = move.UnitHexSprite.Unit.GetPlayer();
                                        CombatMap.Current.MoveControl.ProcessMove(settings);
                                        if (settings.Path.Count < 2 && !settings.MoveFlags.Contains(CombatMoveModeElement.Wall))
                                        {
                                            settings.MoveFlags = settings.MoveFlags + Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Wall);
                                            CombatMap.Current.MoveControl.ProcessMove(settings);
                                        }

                                        cityWall = (CityWall)CombatMap.Current.MapContainer.MapLevels[0].Field[move.X, move.Y].FindOwnedChild(2229508);
                                        if (cityWall == null)
                                        {
                                            cityWall = CombatMap.Current.MapContainer.MapLevels[0].Field[move.X, move.Y].FindOwnedChild(2229510) as CityWall;
                                        }

                                        if (settings.Path.Count == 2 && move.MoveType == BattleAIMoveType.MoveRanged && cityWall != null)
                                        {
                                            move.UnitHexSprite.Unit.MovePoints = 0;
                                        }
                                        else
                                        {
                                            if (settings.Path.Count > 1)
                                            {
                                                pathStepList = settings.Path;
                                                pathCost = MakeTerrainPath(settings, ref pathStepList, ref terrainCost, move.UnitHexSprite.Unit, null, false, false, 0);
                                                if (pathStepList.Count > 1 && pathCost > -1)
                                                {
                                                    if (pathStepList[pathStepList.Count - 2].MoveCost > 0 && pathStepList[pathStepList.Count - 2].MoveCost <= move.UnitHexSprite.Unit.MovePoints)
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
                                                        Creator.GetEngine().OutputDebugString("AI moves to X: " + IntToStr(move.X) + " Y: " + IntToStr(move.X));
                                                        moved = TacticalBattleUnit.MoveCombatUnit(move.UnitHexSprite.Unit, pathStepList);
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
                                    var movePointTable5 = settings.MovePointTable;
                                    move.UnitHexSprite.Unit.Troop.CreateMovePointTable(out movePointTable5);
                                    settings.MovePointTable = movePointTable5;
                                    settings.Player = move.UnitHexSprite.Unit.GetPlayer();
                                    CombatMap.Current.MoveControl.ProcessMove(settings);
                                    if (settings.Path.Count > 1)
                                    {
                                        pathStepList = settings.Path;
                                        pathCost = MakeTerrainPath(settings, ref pathStepList, ref terrainCost, move.UnitHexSprite.Unit, null, false, false, 1);
                                        if (pathStepList.Count > 1 && pathCost > -1)
                                        {
                                            if (pathStepList[pathStepList.Count - 2].MoveCost <= move.UnitHexSprite.Unit.MovePoints)
                                            {
                                                // { Original repeats X in the debug text. }
                                                Creator.GetEngine().OutputDebugString("AI touches at X: " + IntToStr(move.X) + " Y: " + IntToStr(move.X));
                                                moved = move.UnitHexSprite.MeleeMoveTacticalBattle(pathStepList);
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

                                    var movePointTable6 = settings.MovePointTable;
                                    move.UnitHexSprite.Unit.Troop.CreateMovePointTable(out movePointTable6);
                                    settings.MovePointTable = movePointTable6;
                                    settings.Player = move.UnitHexSprite.Unit.GetPlayer();
                                    move.UnitHexSprite.SelectedAbility = move.Ability;
                                    CombatMap.Current.MoveControl.ProcessMove(settings);
                                    if (settings.Path.Count > 1)
                                    {
                                        pathStepList = settings.Path;
                                        pathCost = MakeTerrainPath(settings, ref pathStepList, ref terrainCost, move.UnitHexSprite.Unit, null, false, false, 1);
                                        if (pathStepList.Count > 1 && pathCost > -1)
                                        {
                                            if (pathStepList[pathStepList.Count - 2].MoveCost <= move.UnitHexSprite.Unit.MovePoints)
                                            {
                                                move.UnitHexSprite.SelectedAbility = move.Ability;
                                                // { Original repeats X in the debug text. }
                                                Creator.GetEngine().OutputDebugString("AI melees at X: " + IntToStr(move.X) + " Y: " + IntToStr(move.X));
                                                moved = move.UnitHexSprite.AbilityTouchMoveTacticalBattle(pathStepList);
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
