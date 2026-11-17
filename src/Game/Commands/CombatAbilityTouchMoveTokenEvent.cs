#nullable disable
// Translated from CombatTE.pas.
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
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

public partial class CombatAbilityTouchMoveTokenEvent(CustomNode owner) : CombatMoveTokenEvent(owner)
{

    // CombatTE.pas:624
    public new const int ClassId = 2236418;

    private BattleEffectSprite _effect;

    public int AbilityId { get; set; }

    private BattleEffectType _effectType;
    private bool _successful;

    private CityWall _wall;

    // CombatTE.pas:627
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        AbilityId = persister.Int32("AbilityId", value: AbilityId);
    }

    // CombatTE.pas:633
    public override MoveStatus LastMove()
    {
        Point3D point;
        int x = default;
        int y = default;
        TacticalBattleUnitHexSprite hexSprite;
        MapFieldMessage message = default;
        MoveStatus result = MoveStatus.Moving;
        try
        {
            int i;
            switch (Step)
            {
                case TokenStep.Start:
                    try
                    {
                        _effectType = TacticalTables.AbilityTypes[AbilityId];
                        Unit.UnitHexSprite.CancelMove();
                        Unit.UnitHexSprite.Unselect();
                        Unit.UnitHexSprite.SelectedAbility = AbilityId;
                        bool notExplorationEnabled = !CombatMap.Current.ExplorationEnabled;
                        if (!notExplorationEnabled)
                        {
                            sbyte hexY = Unit.UnitHexSprite.HexY;
                            sbyte hexX = Unit.UnitHexSprite.HexX;
                            notExplorationEnabled = CombatMap.Current.MapContainer.GetField(hexX, hexY, 0).GetExplored(CombatMap.Current.SeatedPlayer);
                        }

                        if (notExplorationEnabled)
                        {
                            sbyte hexY = Unit.UnitHexSprite.HexY;
                            sbyte hexX = Unit.UnitHexSprite.HexX;
                            CombatMap.Current.MakeVisible(hexX, hexY, 0, 5);
                        }

                        sbyte fUnitHexSpriteHexY = Unit.UnitHexSprite.HexY;
                        PathStep pathY = Path[0];
                        sbyte fUnitHexSpriteHexX = Unit.UnitHexSprite.HexX;
                        PathStep pathX = Path[0];
                        ushort meleeDirIndex = TacticalTables.GetMeleeDirectionIndex(Unit.UnitHexSprite.HexX, fUnitHexSpriteHexX - pathX.Location.X, fUnitHexSpriteHexY - pathY.Location.Y);
                        Unit.UnitHexSprite.SetDirection((byte)meleeDirIndex);
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
                                hexSprite = CombatMap.Current.MapContainer.MapLevels[0].GetField(path2.Location.X, path.Location.Y).FindOwnedChild<TacticalBattleUnitHexSprite>(2228484);
                                PreviousUnit = hexSprite?.Unit;

                                if (PreviousUnit != null && AbilityId != 117)
                                {
                                    PreviousUnit.UnitHexSprite.SetDirection((byte)(Unit.UnitHexSprite.Direction + 3));
                                    if (PreviousUnit.UnitHexSprite.Direction > 6)
                                    {
                                        PreviousUnit.UnitHexSprite.SetDirection((byte)(PreviousUnit.UnitHexSprite.Direction - 6));
                                    }
                                }
                                else
                                {
                                    if (AbilityId == 117)
                                    {
                                        PathStep path3 = Path[0];
                                        PathStep path4 = Path[0];
                                        _wall = CombatMap.Current.MapContainer.MapLevels[0].GetField(path4.Location.X, path3.Location.Y).FindOwnedChild<CityWall>(2229508);
                                        if (_wall == null)
                                        {
                                            PathStep path5 = Path[0];
                                            PathStep path6 = Path[0];
                                            _wall = CombatMap.Current.MapContainer.MapLevels[0].GetField(path6.Location.X, path5.Location.Y).FindOwnedChild(2229510) as CityWall;
                                        }

                                        if (_wall == null && AbilityId == 117)
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
                        _ = MessageBox(0, "Error during Token Start", "Error - TCTouchToken " + IntToStr(AbilityId), 0);
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
                        _ = MessageBox(0, "Error during Token StepUp", "Error - TCTouchToken " + IntToStr(AbilityId), 0);
                    }

                    break;
                case TokenStep.InitializeStrike:
                    try
                    {
                        if (PreviousUnit != null)
                        {
                            if (Unit.UnitHexSprite.ActionDone)
                            {
                                _effect = CombatMap.Current.AddEffect(_effectType);
                                _effect.AbilityIndex = AbilityId;
                                point = new Point3D();
                                sbyte hexY = PreviousUnit.UnitHexSprite.HexY;
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
                            if (_wall != null || AbilityId == 65)
                            {
                                switch (AbilityId)
                                {
                                    case 117:
                                        Step = TokenStep.End;
                                        RandSeed = StrategicMap.Current.Random(16777215);
                                        int defense = _wall.GetDefense();
                                        int executeDamageRole = AbilityRules.ExecuteDamageRole(TacticalTables.TacticalBattleDamage[0], TacticalTables.TacticalBattleAttack[0] - defense);
                                        _wall.SetHitPoints(_wall.HitPoints - executeDamageRole);
                                        Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepBack);
                                        break;
                                    case 65:
                                        _effect = CombatMap.Current.AddEffect(_effectType);
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
                                }
                            }
                        }
                    }
                    catch
                    {
                        _ = MessageBox(0, "Error during Token InitStrike", "Error - TCTouchToken " + IntToStr(AbilityId), 0);
                    }

                    break;
                case TokenStep.ExecuteStrike:
                    try
                    {
                        if (_effect == null)
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
                                                HexGeometry.CenterHexNumberToHex(Path[1].Location.X, path.Location.Y, i, ref x, ref y);
                                                hexSprite = CombatMap.Current.MapContainer.MapLevels[0].GetField(x, y).FindOwnedChild<TacticalBattleUnitHexSprite>(2228484);
                                                if (hexSprite == null || hexSprite == Unit.UnitHexSprite)
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
                                                        RandSeed = StrategicMap.Current.Random(16777215);
                                                        Strike = CombatObject.CreateStrikeCombatAction(Unit, hexSprite.Unit, true);
                                                        if (Strike.Damage > 0)
                                                        {
                                                            hexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                                            Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.RoundAttackHit);
                                                            TacticalBattleUnit.MakeHitBlood(hexSprite.Unit, Strike.Damage);
                                                            CombatMap.Current.Combat.ExecuteCombatAction(Strike);
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
                                    CombatAction touchAction = AbilityId >= 0
                                        ? GamePack.Current.AbilityControl.GetAbility(AbilityId)?.CreateTouchMoveCombatAction(Unit, target: PreviousUnit)
                                        : null;
                                    if (touchAction != null)
                                    {
                                        _successful = touchAction.TouchSucceeded;
                                        CombatMap.Current.Combat.ExecuteCombatAction(touchAction);
                                        _ = touchAction.Release();
                                        if (touchAction is PossessCombatAction)
                                        {
                                            int unitGraphicsResourceIndex = Unit.Troop.GetUnitGraphicsResourceIndex();
                                            Unit.UnitHexSprite.SetUnitGraphicsResourceIndex(unitGraphicsResourceIndex);
                                            if (!PreviousUnit.GetEnabled())
                                            {
                                                _ = Unit.UnitHexSprite.MoveTo(PreviousUnit.UnitHexSprite.Owner, 0);
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

                                if (!Unit.GetEnabled() || ((AbilityId == 106 || AbilityId == 107) && _successful))
                                {
                                    result = MoveStatus.Done;
                                }
                                else
                                {
                                    if (!_successful && PreviousUnit != null && PreviousUnit.GetEnabled() && !PreviousUnit.GetLocked() && CombatObject.MeleeDamageTypes(PreviousUnit) != Set<DamageType>.Of() && PreviousUnit.GetSide() != Unit.GetSide())
                                    {
                                        RandSeed = StrategicMap.Current.Random(16777215);
                                        Strike = CombatObject.CreateStrikeCombatAction(PreviousUnit, Unit, true);
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
                                                    HexGeometry.CenterHexNumberToHex(Path[1].Location.X, path.Location.Y, i, ref x, ref y);
                                                    CombatMap.Current.MapContainer.MapLevels[0].GetField(x, y).SendMapFieldMessage(ref message);
                                                    EffectPaths.CreateFire(false, 50, x, y, engine: GetEngine());
                                                }

                                                Unit.DeathMethod = TacticalBattleUnitDeathMethod.Explode;
                                            }

                                            break;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        _ = MessageBox(0, "Error during Token ExeStrike", "Error - TCTouchToken " + IntToStr(AbilityId), 0);
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

                            sbyte hexY = Unit.UnitHexSprite.HexY;
                            HexGeometry.HexToPixel(Unit.UnitHexSprite.HexX, hexY, out i, out x);
                            if (Strike.DamageTypes - Strike.Effects != Set<DamageType>.Of())
                            {
                                CombatEvents.MakeDamageEffectHexSpriteList(i + Unit.UnitHexSprite.OffsetX, x + Unit.UnitHexSprite.OffsetY, Strike.DamageTypes - Strike.Effects, false);
                            }

                            if (Strike.Effects != Set<DamageType>.Of())
                            {
                                CombatEvents.MakeDamageEffectHexSpriteList(i + Unit.UnitHexSprite.OffsetX, x + Unit.UnitHexSprite.OffsetY, Strike.Effects, true);
                            }

                            Step = TokenStep.ExecuteEndStrike;
                        }
                    }
                    catch
                    {
                        _ = MessageBox(0, "Error during Token StrikeBack", "Error - TCTouchToken " + IntToStr(AbilityId), 0);
                    }

                    break;
                case TokenStep.ExecuteEndStrike:
                    try
                    {
                        if (Unit.UnitHexSprite.ActionDone || Strike.Damage == 0)
                        {
                            if (Strike.Damage > 0)
                            {
                                CombatMap.Current.Combat.ExecuteCombatAction(Strike);
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
                        _ = MessageBox(0, "Error during Token ExeStrikeBack", "Error - TCTouchToken " + IntToStr(AbilityId), 0);
                    }

                    break;
                case TokenStep.End:
                    if (Unit.UnitHexSprite.ActionDone)
                    {
                        result = MoveStatus.Done;
                    }

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
                    if (Unit.CombatPlayer == CombatMap.Current.CurrentCombatPlayer)
                    {
                        sbyte hexX = Unit.UnitHexSprite.HexX;
                        CombatMap.Current.Players.GetPlayers(CombatMap.Current.CurrentCombatPlayer).LastX = hexX;
                        sbyte hexY = Unit.UnitHexSprite.HexY;
                        CombatMap.Current.Players.GetPlayers(CombatMap.Current.CurrentCombatPlayer).LastY = hexY;
                    }
                }
            }
        }
        catch
        {
            _ = MessageBox(0, "Error during Token", "Error - TCTouchToken " + IntToStr(AbilityId), 0);
        }

        return result;
    }

    // intf_P5_07.inc:765

}
