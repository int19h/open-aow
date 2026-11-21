#nullable disable warnings
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.Commands;

public partial class ArmyCombatMoveTokenEvent
{
    public void ExecuteCombat()
    {
        if ((HeadlessHooks.Tactical || FrameFixtureInputs.Tactical) && (HeadlessHooks.TacticalBattles == 0 || HeadlessHooks.TacticalSelections < HeadlessHooks.TacticalBattles))
        {
            _combatType = CombatType.Tactical;
            HeadlessHooks.TacticalSelections++;
        }
        MapFieldMessage message = default;
        MapField field = GetMapField(Path[0].Location);
        sbyte defender = field.GetPlayer();
        MapField? source = ArmyHexSprite.Owner;
        ArmyHexSprite.SetOwner(null);
        ArmyHexSprite otherArmyHexSprite = source.GetArmyHexSprite();
        ArmyHexSprite.SetOwner(source);
        foreach (Troop? troop2 in ArmyHexSprite.Army.Units)
        {
            if ((Selection & 1) != 0)
            {
                Troop? unit = troop2;
                troop2.UnitStatus = unit.UnitStatus | UnitStatuses.Moving;
            }
            else
            {
                Troop? unit = troop2;
                troop2.UnitStatus = unit.UnitStatus & ~UnitStatuses.Moving;
            }

            Selection = (byte)(Selection >>> 1);
        }

        if (otherArmyHexSprite is not null)
        {
            using (ArmyHexSprite.Army.BeginUpdate())
            {
                ArmyHexSprite armyHexSprite2 = ArmyHexSprite;
                armyHexSprite2.Army.Mode = ArmyHexSprite.Army.Mode | ArmyModes.DisableTerrainValidation;
                ArmyHexSprite = armyHexSprite2;
                for (int i = otherArmyHexSprite.Army.Units.Count - 1; i >= 0; i--)
                {
                    Troop? troop = otherArmyHexSprite.Army.Units.GetUnit(i);
                    troop.UnitStatus &= ~UnitStatuses.Moving;
                    _ = ArmyHexSprite.Army.AddUnit(troop);
                }

                ArmyHexSprite armyHexSprite3 = ArmyHexSprite;
                armyHexSprite3.Army.Mode = ArmyHexSprite.Army.Mode & ~ArmyModes.DisableTerrainValidation;
                ArmyHexSprite = armyHexSprite3;
            }
        }

        ArmyHexSprite.CancelMove();
        StrategicMap.Current.LockGameOverEvent();
        Battle combat = _combatType == CombatType.Fast ? StrategicMap.Current.CreateCombat(FastBattle.ClassId) : StrategicMap.Current.CreateCombat(TacticalBattle.ClassId);

        sbyte hexY = ArmyHexSprite.HexY;
        sbyte hexLevel = ArmyHexSprite.HexLevel;
        sbyte player = field.GetPlayer();
        byte independentRelation = field.GetIndependentRelation();
        PathStep item = Path[0];
        PathStep fPathItem = Path[0];
        PathStep item2 = Path[0];
        sbyte hexX = ArmyHexSprite.HexX;
        _ = combat.Setup((byte)ArmyHexSprite.Army.Player, attackerRelation: (byte)ArmyHexSprite.Army.IndependentRelation, hexX, hexY, attackerL: hexLevel, defenderPlayer: (byte)player, defenderRelation: independentRelation, item.Location.X, fPathItem.Location.Y, L: item2.Location.MapLevel, logActions: _combatType == CombatType.Fast);
        combat.SetExecutedHandler(CombatExecuted);
        message.MessageId = 4359;
        MapFieldMessageParameters param = message.Param;
        param[0] = combat;
        message.Param = param;
        MapFieldMessageParameters param2 = message.Param;
        param2[1] = ArmyHexSprite;
        message.Param = param2;
        message.Result = 0;
        field.SendMapFieldMessage(ref message);
        if (message.Result.Int == 0)
        {
            sbyte fArmyHexSpriteHexLevel = ArmyHexSprite.HexLevel;
            sbyte fArmyHexSpriteHexY = ArmyHexSprite.HexY;
            sbyte fArmyHexSpriteHexX = ArmyHexSprite.HexX;
            combat.InitializeDefaultArmies(ArmyHexSprite.Army, fArmyHexSpriteHexX, fArmyHexSpriteHexY, L: fArmyHexSpriteHexLevel);
        }

        combat.Initialize();
        combat.Activate();
        CombatResult combatResult = combat.Execute();
        _combatMode = ArmyCombatMoveTokenEventMode.CombatExecuted;
        if (combatResult != CombatResult.Busy)
        {
            combat.Deactivate();
            combat.Finalize();
            if (combat is FastBattle)
            {
                DistributeCombatEvent(combat.Logbook, execute: true);
                if (StrategicMap.Current.RequestCombatType && StrategicMap.Current.SeatedPlayer == defender)
                {
                    UserEventLog log = new(null)
                    {
                        Holder = CreationHolder(),
                        Data = this
                    };
                    StrategicMap.Current.TriggerExecuteEventLog(log, immediate: false, done: RestoreHotSeatModeCallback);
                    _ = log.Release();
                    StrategicMap.Current.AIExecutor.Lock();
                    StrategicMap.Current.LockGameOverEvent();
                    _combatMode = ArmyCombatMoveTokenEventMode.RestoringHotSeat;
                }
            }

            bool condition = combatResult != CombatResult.NoDefenders;
            if (condition)
            {
                PathStep item3 = Path[0];
                PathStep item4 = Path[0];
                PathStep item5 = Path[0];
                condition = StrategicMap.Current.VisibleForPlayer(item5.Location.X, item4.Location.Y, item3.Location.MapLevel, player: StrategicMap.Current.SeatedPlayer);
            }

            if (condition)
            {
                ImageSequenceAnimation animation = new(null) { Holder = CreationHolder() };
                animation.SetImageSequence(GamePack.Current.ImageSequences.ImageSequences[22]);
                PathStep item3 = Path[0];
                PathStep item4 = Path[0];
                PathStep item5 = Path[0];
                _ = StrategicMap.Current.PlaceHex(animation, item5.Location.X, item4.Location.Y, L: item3.Location.MapLevel);
            }

            StrategicMap.Current.DestroyCombat();
            StrategicMap.Current.UnlockGameOverEvent();
            ArmyHexSprite = GetMapField(Path[MoveIndex].Location).GetArmyHexSprite();
            if (ArmyHexSprite is not null && combatResult is CombatResult.CannotInflictDamage or (>= CombatResult.Undecided and <= CombatResult.Fled))
            {
                FinishMove();
                ArmyHexSprite = null;
            }

            if (ArmyHexSprite is not null)
            {
                ArmyHexSprite.CancelMove();
                bool moved;
                ArmyHexSprite armyHexSprite;
                using (ArmyHexSprite.Army.BeginUpdate())
                {
                    armyHexSprite = new ArmyHexSprite(null) { Holder = CreationHolder() };
                    using (armyHexSprite.Army.BeginUpdate())
                    {
                        armyHexSprite.Army.AssignSettings(ArmyHexSprite.Army);
                        armyHexSprite.Direction = ArmyHexSprite.Direction;
                        armyHexSprite.Army.SetPlayer(ArmyHexSprite.Army.Player);
                        for (int i = ArmyHexSprite.Army.Units.Count - 1; i >= 0; i--)
                        {
                            if (ArmyHexSprite.Army.Units.GetUnit(i).UnitStatus.Contains(UnitStatuses.Moving))
                            {
                                Troop? unit = ArmyHexSprite.Army.Units.GetUnit(i);
                                ArmyHexSprite.Army.Units.GetUnit(i).UnitStatus = unit.UnitStatus & ~UnitStatuses.Moving;
                                Troop? fArmyUnit = ArmyHexSprite.Army.Units.GetUnit(i);
                                _ = armyHexSprite.Army.AddUnit(fArmyUnit);
                            }
                        }
                    }

                    moved = armyHexSprite.Army.Units.Count != 0;
                    if (moved)
                    {
                        PathStep item3 = Path[MoveIndex];
                        PathStep item4 = Path[MoveIndex];
                        PathStep item5 = Path[MoveIndex];
                        _ = armyHexSprite.PlacePrivate(StrategicMap.Current.MapContainer, item5.Location.X, item4.Location.Y, L: item3.Location.MapLevel);
                        if (ArmyHexSprite.GetSelected())
                        {
                            StrategicMap.Current.Selection.UnselectLocation();
                            StrategicMap.Current.Selection.SelectLocationHexSprite(armyHexSprite);
                        }
                    }

                    ArmyHexSprite armyHexSprite4 = ArmyHexSprite;
                    armyHexSprite4.Army.Mode = ArmyHexSprite.Army.Mode & ~ArmyModes.Moving;
                    ArmyHexSprite = armyHexSprite4;
                }

                if (moved)
                {
                    ArmyHexSprite movedArmyHexSprite = armyHexSprite;
                    ArmyHexSprite = movedArmyHexSprite;
                    movedArmyHexSprite.Army.Mode |= ArmyModes.Moving;
                    Selection = 255;
                    if (combatResult is CombatResult.Victory or CombatResult.NoDefenders || base.CanMoveNext())
                    {
                        int direction = GetDirection(MoveIndex);
                        _ = ArmyHexSprite.MoveFirst(direction, instant: false);
                        ArmyHexSprite.SetMoveStatus(50);
                    }
                    else
                    {
                        FinishMove();
                        ArmyHexSprite = null;
                        return;
                    }
                }
                else
                {
                    // { The original drops the empty new army without freeing it. }
                    ArmyHexSprite = null;
                }
            }
        }
        else
        {
            StrategicMap.Current.UnlockGameOverEvent();
            ArmyHexSprite = null;
            FinishMove();
        }
    }
}
