#nullable disable warnings
// Translated from CombatTE.pas.
using System.Globalization;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.Commands;

public class CombatAbilityTouchMoveTokenEvent(CustomNode? owner) : CombatMoveTokenEvent(owner)
{

    // CombatTE.pas:624
    public new const int ClassId = 2_236_418;

    private BattleEffectSprite _effect;

    public int AbilityId { get; set; }

    private BattleEffectType _effectType;
    private bool _successful;

    private CityWall? _wall;

    // CombatTE.pas:627
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        AbilityId = persister.Int32("AbilityId", value: AbilityId);
    }

    // CombatTE.pas:633
    public override MoveStatus LastMove()
    {
        MapFieldMessage message = default;
        MoveStatus result = MoveStatus.Moving;
        try
        {
            int i;
            int x = default;
            TacticalBattleUnitHexSprite hexSprite;
            switch (Step)
            {
                case TokenStep.Start:
                    try
                    {
                        _effectType = TacticalTables.AbilityTypes[AbilityId];
                        Unit.UnitHexSprite.CancelMove();
                        Unit.UnitHexSprite.Unselect();
                        Unit.UnitHexSprite.SelectedAbility = AbilityId;
                        CombatMap combatMap = GetEngine().CombatMap;
                        bool notExplorationEnabled = !combatMap.ExplorationEnabled;
                        if (!notExplorationEnabled)
                        {
                            int hexY = Unit.UnitHexSprite.HexY;
                            int hexX = Unit.UnitHexSprite.HexX;
                            notExplorationEnabled = combatMap.MapContainer.GetField(hexX, hexY, 0).GetExplored(combatMap.SeatedPlayer);
                        }

                        if (notExplorationEnabled)
                        {
                            int hexY = Unit.UnitHexSprite.HexY;
                            int hexX = Unit.UnitHexSprite.HexX;
                            combatMap.MakeVisible(hexX, hexY, 0, radius: 5);
                        }

                        int fUnitHexSpriteHexY = Unit.UnitHexSprite.HexY;
                        PathStep pathY = Path[0];
                        int fUnitHexSpriteHexX = Unit.UnitHexSprite.HexX;
                        PathStep pathX = Path[0];
                        int meleeDirIndex = TacticalTables.GetMeleeDirectionIndex(Unit.UnitHexSprite.HexX, fUnitHexSpriteHexX - pathX.Location.X, fUnitHexSpriteHexY - pathY.Location.Y);
                        Unit.UnitHexSprite.SetDirection(meleeDirIndex.TruncateToByte());
                        if (AbilityId == 65)
                        {
                            Step = TokenStep.InitializeStrike;
                        }
                        else
                        {
                            if (AbilityId == 114)
                            {
                                Step = TokenStep.ExecuteStrike;
                                Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.RoundAttack);
                            }
                            else
                            {
                                Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepUp);
                                PathStep path = Path[0];
                                PathStep path2 = Path[0];
                                CombatMap engineCombatMap = GetEngine().CombatMap;
                                hexSprite = engineCombatMap.MapContainer.MapLevels[0].GetField(path2.Location.X, path.Location.Y).FindOwnedChild<TacticalBattleUnitHexSprite>(TacticalBattleUnitHexSprite.ClassId);
                                PreviousUnit = hexSprite?.Unit;

                                if (PreviousUnit is not null && AbilityId != 117)
                                {
                                    PreviousUnit.UnitHexSprite.SetDirection((Unit.UnitHexSprite.Direction + 3).TruncateToByte());
                                    if (PreviousUnit.UnitHexSprite.Direction > 6)
                                    {
                                        PreviousUnit.UnitHexSprite.SetDirection((PreviousUnit.UnitHexSprite.Direction - 6).TruncateToByte());
                                    }
                                }
                                else
                                {
                                    if (AbilityId == 117)
                                    {
                                        PathStep path3 = Path[0];
                                        PathStep path4 = Path[0];
                                        _wall = engineCombatMap.MapContainer.MapLevels[0].GetField(path4.Location.X, path3.Location.Y).FindOwnedChild<CityWall>(CityWall.ClassId);
                                        if (_wall is null)
                                        {
                                            PathStep path5 = Path[0];
                                            PathStep path6 = Path[0];
                                            _wall = engineCombatMap.MapContainer.MapLevels[0].GetField(path6.Location.X, path5.Location.Y).FindOwnedChild(2_229_510) as CityWall;
                                        }

                                        if (_wall is null && AbilityId == 117)
                                        {
                                            result = MoveStatus.Done;
                                        }
                                    }
                                    else
                                    {
                                        result = MoveStatus.Done;
                                    }
                                }

                                Step = TokenStep.StepUp;
                            }
                        }
                    }
                    catch
                    {
                        _ = MessageBox(0, text: "Error during Token Start", caption: $"Error - TCTouchToken {AbilityId.ToString(CultureInfo.InvariantCulture)}", flags: 0);
                    }

                    break;
                case TokenStep.StepUp:
                    try
                    {
                        if (Unit.UnitHexSprite.ActionDone)
                        {
                            Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Ability);
                            Step = TokenStep.InitializeStrike;
                        }
                    }
                    catch
                    {
                        _ = MessageBox(0, text: "Error during Token StepUp", caption: $"Error - TCTouchToken {AbilityId.ToString(CultureInfo.InvariantCulture)}", flags: 0);
                    }

                    break;
                case TokenStep.InitializeStrike:
                    try
                    {
                        Point3D point;
                        if (PreviousUnit is not null)
                        {
                            if (Unit.UnitHexSprite.ActionDone)
                            {
                                _effect = GetEngine().CombatMap.AddEffect(_effectType);
                                _effect.AbilityIndex = AbilityId;
                                point = new Point3D();
                                int hexY = PreviousUnit.UnitHexSprite.HexY;
                                int x2 = point.X;
                                int y2 = point.Y;
                                HexGeometry.HexToPixel(PreviousUnit.UnitHexSprite.HexX, hexY, out x2, out y2);
                                point.X = x2;
                                point.Y = y2;
                                point.X += 16;
                                _effect.Path.Add(point);
                                Step = TokenStep.ExecuteStrike;
                            }
                        }
                        else
                        {
                            if (_wall is not null || AbilityId == 65)
                            {
                                switch (AbilityId)
                                {
                                    case 117:
                                        Step = TokenStep.End;
                                        RandSeed = GetEngine().StrategicMap.Random(16777215);
                                        int defense = _wall.GetDefense();
                                        int executeDamageRole = AbilityRules.ExecuteDamageRole(TacticalTables.TacticalBattleDamage[0], attackDefense: TacticalTables.TacticalBattleAttack[0] - defense);
                                        _wall.SetHitPoints(_wall.HitPoints - executeDamageRole);
                                        Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepBack);
                                        break;
                                    case 65:
                                        _effect = GetEngine().CombatMap.AddEffect(_effectType);
                                        _effect.AbilityIndex = AbilityId;
                                        point = new Point3D();
                                        PathStep path = Path[1];
                                        int x3 = point.X;
                                        int y3 = point.Y;
                                        HexGeometry.HexToPixel(Path[1].Location.X, path.Location.Y, out x3, out y3);
                                        point.X = x3;
                                        point.Y = y3;
                                        point.X += 16;
                                        _effect.Path.Add(point);
                                        Step = TokenStep.ExecuteStrike;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    catch
                    {
                        _ = MessageBox(0, text: "Error during Token InitStrike", caption: $"Error - TCTouchToken {AbilityId.ToString(CultureInfo.InvariantCulture)}", flags: 0);
                    }

                    break;
                case TokenStep.ExecuteStrike:
                    try
                    {
                        int y = default;
                        if (_effect is null)
                        {
                            if (!Unit.GetEnabled())
                            {
                                result = MoveStatus.Done;
                            }
                            else
                            {
                                switch (AbilityId)
                                {
                                    case 114:
                                        if (Unit.UnitHexSprite.ActionDone || Unit.UnitHexSprite.ActionFrame >= 17)
                                        {
                                            result = MoveStatus.Done;
                                        }
                                        else
                                        {
                                            if (TacticalTables.RangedAttackDirection[Unit.UnitHexSprite.ActionFrame] > 0)
                                            {
                                                i = ((TacticalTables.RangedAttackDirection[Unit.UnitHexSprite.ActionFrame] + Unit.UnitHexSprite.Direction) % 6) + 1;
                                                PathStep path = Path[1];
                                                HexGeometry.CenterHexNumberToHex(Path[1].Location.X, path.Location.Y, hexNumber: i, ref x, ref y);
                                                hexSprite = GetEngine().CombatMap.MapContainer.MapLevels[0].GetField(x, y).FindOwnedChild<TacticalBattleUnitHexSprite>(TacticalBattleUnitHexSprite.ClassId);
                                                if (hexSprite is null || hexSprite == Unit.UnitHexSprite)
                                                {
                                                    PreviousUnit = null;
                                                }
                                                else
                                                {
                                                    if (hexSprite.Unit.GetAbilityEnabled(1))
                                                    {
                                                        PreviousUnit = null;
                                                    }
                                                    else
                                                    {
                                                        RandSeed = GetEngine().StrategicMap.Random(16777215);
                                                        Strike = CombatObject.CreateStrikeCombatAction(Unit, target: hexSprite.Unit, defensive: true);
                                                        if (Strike.Damage > 0)
                                                        {
                                                            hexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                                            Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.RoundAttackHit);
                                                            TacticalBattleUnit.MakeHitBlood(hexSprite.Unit, Strike.Damage);
                                                            GetEngine().CombatMap.Combat.ExecuteCombatAction(Strike);
                                                        }
                                                        else
                                                        {
                                                            Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.RoundAttack);
                                                        }

                                                        _ = Strike.Release();
                                                        Strike = null;
                                                    }
                                                }
                                            }
                                        }

                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (_effect.Done)
                            {
                                if (!Unit.GetEnabled())
                                {
                                    result = MoveStatus.Done;
                                }
                                else
                                {
                                    // GetAbility asserts on a negative id. The touch switch did nothing for one, so a negative id stays off that lookup.
                                    CombatAction? touchAction = AbilityId >= 0
                                        ? GetEngine().GamePack.AbilityControl[AbilityId]?.CreateTouchMoveCombatAction(Unit, target: PreviousUnit)
                                        : null;
                                    if (touchAction is not null)
                                    {
                                        _successful = touchAction.TouchSucceeded;
                                        GetEngine().CombatMap.Combat.ExecuteCombatAction(touchAction);
                                        _ = touchAction.Release();
                                        if (touchAction is PossessCombatAction)
                                        {
                                            int unitGraphicsResourceIndex = Unit.Troop.UnitGraphicsResourceIndex;
                                            Unit.UnitHexSprite.SetUnitGraphicsResourceIndex(unitGraphicsResourceIndex);
                                            if (!PreviousUnit.GetEnabled())
                                            {
                                                _ = Unit.UnitHexSprite.MoveTo(PreviousUnit.UnitHexSprite.Owner, moveCost: 0);
                                                Unit.UnitHexSprite.SetDirection(PreviousUnit.UnitHexSprite.Direction);
                                                Unit.UnitHexSprite.OffsetX = 0;
                                                Unit.UnitHexSprite.OffsetY = 0;
                                                PreviousUnit.UnitHexSprite?.Dispose();
                                                PreviousUnit.UnitHexSprite = null;
                                                result = MoveStatus.Done;
                                            }
                                        }
                                    }
                                }

                                if (!Unit.GetEnabled() || ((AbilityId is 106 or 107) && _successful))
                                {
                                    result = MoveStatus.Done;
                                }
                                else
                                {
                                    if (!_successful && PreviousUnit is not null && PreviousUnit.GetEnabled() && !PreviousUnit.GetLocked() && CombatObject.MeleeDamageTypes(PreviousUnit) != DamageTypes.None && PreviousUnit.Side != Unit.Side)
                                    {
                                        RandSeed = GetEngine().StrategicMap.Random(16777215);
                                        Strike = CombatObject.CreateStrikeCombatAction(PreviousUnit, target: Unit, defensive: true);
                                        if (Strike.Damage == 0)
                                        {
                                            PreviousUnit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.MeleeMiss);
                                        }
                                        else
                                        {
                                            PreviousUnit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.MeleeHit);
                                        }

                                        Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Busy);
                                        Step = TokenStep.EndStrike;
                                    }
                                    else
                                    {
                                        Step = TokenStep.End;
                                        Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepBack);
                                    }
                                }
                            }
                            else
                            {
                                if (Unit.GetEnabled())
                                {
                                    switch (AbilityId)
                                    {
                                        case 65:
                                            if (_effect.Frame == 5)
                                            {
                                                message.MessageId = 200704;
                                                MapFieldMessageParameters param = message.Param;
                                                param[0] = Unit;
                                                message.Param = param;
                                                message.Result = -1;
                                                i = 0;
                                                for (; i <= 6; i += 1)
                                                {
                                                    PathStep path = Path[1];
                                                    HexGeometry.CenterHexNumberToHex(Path[1].Location.X, path.Location.Y, hexNumber: i, ref x, ref y);
                                                    GetEngine().CombatMap.MapContainer.MapLevels[0].GetField(x, y).SendMapFieldMessage(ref message);
                                                    EffectPaths.CreateFire(50, x, y, engine: GetEngine());
                                                }

                                                Unit.DeathMethod = TacticalBattleUnitDeathMethod.Explode;
                                            }

                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        _ = MessageBox(0, text: "Error during Token ExeStrike", caption: $"Error - TCTouchToken {AbilityId.ToString(CultureInfo.InvariantCulture)}", flags: 0);
                    }

                    break;
                case TokenStep.EndStrike:
                    try
                    {
                        if (PreviousUnit.UnitHexSprite.ActionDone)
                        {
                            if (Strike.Damage > 0)
                            {
                                Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                TacticalBattleUnit.MakeHitBlood(Unit, Strike.Damage);
                            }

                            int hexY = Unit.UnitHexSprite.HexY;
                            HexGeometry.HexToPixel(Unit.UnitHexSprite.HexX, hexY, out i, out x);
                            if ((Strike.DamageTypes & ~Strike.Effects) != DamageTypes.None)
                            {
                                CombatEvents.MakeDamageEffectHexSpriteList(i + Unit.UnitHexSprite.OffsetX, x + Unit.UnitHexSprite.OffsetY, damageTypes: Strike.DamageTypes & ~Strike.Effects, resisted: false, engine: GetEngine()!);
                            }

                            if (Strike.Effects != DamageTypes.None)
                            {
                                CombatEvents.MakeDamageEffectHexSpriteList(i + Unit.UnitHexSprite.OffsetX, x + Unit.UnitHexSprite.OffsetY, damageTypes: Strike.Effects, resisted: true, engine: GetEngine()!);
                            }

                            Step = TokenStep.ExecuteEndStrike;
                        }
                    }
                    catch
                    {
                        _ = MessageBox(0, text: "Error during Token StrikeBack", caption: $"Error - TCTouchToken {AbilityId.ToString(CultureInfo.InvariantCulture)}", flags: 0);
                    }

                    break;
                case TokenStep.ExecuteEndStrike:
                    try
                    {
                        if (Unit.UnitHexSprite.ActionDone || Strike.Damage == 0)
                        {
                            if (Strike.Damage > 0)
                            {
                                GetEngine().CombatMap.Combat.ExecuteCombatAction(Strike);
                            }

                            if (Unit.GetEnabled())
                            {
                                Step = TokenStep.End;
                                Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepBack);
                            }
                            else
                            {
                                result = MoveStatus.Done;
                            }
                        }
                    }
                    catch
                    {
                        _ = MessageBox(0, text: "Error during Token ExeStrikeBack", caption: $"Error - TCTouchToken {AbilityId.ToString(CultureInfo.InvariantCulture)}", flags: 0);
                    }

                    break;
                case TokenStep.End:
                    if (Unit.UnitHexSprite.ActionDone)
                    {
                        result = MoveStatus.Done;
                    }

                    break;
                case TokenStep.FirstStrike:
                case TokenStep.DelayStrike:
                default:
                    break;
            }

            if (result == MoveStatus.Done)
            {
                if (Unit.GetEnabled())
                {
                    Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Move);
                    Unit.UnitHexSprite.SelectedAbility = -1;
                    Unit.MovePoints = 0;
                    Unit.UnitHexSprite.Unselect();
                    CombatMap getPlayersCombatMap = GetEngine().CombatMap;
                    if (Unit.CombatPlayer == getPlayersCombatMap.CurrentCombatPlayer)
                    {
                        int hexX = Unit.UnitHexSprite.HexX;
                        getPlayersCombatMap.Players.GetPlayers(getPlayersCombatMap.CurrentCombatPlayer).LastX = hexX;
                        int hexY = Unit.UnitHexSprite.HexY;
                        getPlayersCombatMap.Players.GetPlayers(getPlayersCombatMap.CurrentCombatPlayer).LastY = hexY;
                    }
                }
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Error during Token", caption: $"Error - TCTouchToken {AbilityId.ToString(CultureInfo.InvariantCulture)}", flags: 0);
        }

        return result;
    }
}
