#nullable disable
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

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
        CombatResult combatResult = default;
        MapField field = default;
        MapField source = default;
        sbyte defender = default;
        ArmyHexSprite otherArmyHexSprite = default;
        ArmyHexSprite armyHexSprite = default;
        ArmyHexSprite movedArmyHexSprite = default;
        Battle combat = default;
        MapFieldMessage message = default;
        UserEventLog log = default;
        ImageSequenceAnimation animation = default;
        Troop troop = default;
        field = GetMapField(Path[0].Location);
        defender = field.GetPlayer();
        source = ArmyHexSprite.Owner;
        ArmyHexSprite.SetOwner(null);
        otherArmyHexSprite = source.GetArmyHexSprite();
        ArmyHexSprite.SetOwner(source);
        for (int i = 0, last = ArmyHexSprite.Army.Units.Count - 1; i <= last; i++)
        {
            if ((Selection & 1) != 0)
            {
                Troop unit = ArmyHexSprite.Army.Units.GetUnit(i);
                ArmyHexSprite.Army.Units.GetUnit(i).UnitStatus = unit.UnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
            }
            else
            {
                Troop unit = ArmyHexSprite.Army.Units.GetUnit(i);
                ArmyHexSprite.Army.Units.GetUnit(i).UnitStatus = unit.UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        if (otherArmyHexSprite is not null)
        {
            using (ArmyHexSprite.Army.BeginUpdate())
            {
                ArmyHexSprite armyHexSprite2 = ArmyHexSprite;
                armyHexSprite2.Army.Mode = ArmyHexSprite.Army.Mode + Set<ArmyModeElement>.Of(ArmyModeElement.DisableTerrainValidation);
                ArmyHexSprite = armyHexSprite2;
                for (int i = otherArmyHexSprite.Army.Units.Count - 1; i >= 0; i--)
                {
                    troop = otherArmyHexSprite.Army.Units.GetUnit(i);
                    troop.UnitStatus -= Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
                    _ = ArmyHexSprite.Army.AddUnit(troop);
                }

                ArmyHexSprite armyHexSprite3 = ArmyHexSprite;
                armyHexSprite3.Army.Mode = ArmyHexSprite.Army.Mode - Set<ArmyModeElement>.Of(ArmyModeElement.DisableTerrainValidation);
                ArmyHexSprite = armyHexSprite3;
            }
        }

        ArmyHexSprite.CancelMove();
        StrategicMap.Current.LockGameOverEvent();
        combat = _combatType == CombatType.Fast ? StrategicMap.Current.CreateCombat(131760) : StrategicMap.Current.CreateCombat(2228496);

        sbyte hexY = ArmyHexSprite.GetHexY();
        sbyte hexLevel = ArmyHexSprite.GetHexLevel();
        sbyte player = field.GetPlayer();
        byte independentRelation = field.GetIndependentRelation();
        PathStep item = Path[0];
        PathStep fPathItem = Path[0];
        PathStep item2 = Path[0];
        sbyte hexX = ArmyHexSprite.GetHexX();
        _ = combat.Setup((byte)ArmyHexSprite.Army.Player, (byte)ArmyHexSprite.Army.IndependentRelation, hexX, hexY, hexLevel, (byte)player, independentRelation, item.Location.X, fPathItem.Location.Y, item2.Location.MapLevel, _combatType == CombatType.Fast);
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
            sbyte fArmyHexSpriteHexLevel = ArmyHexSprite.GetHexLevel();
            sbyte fArmyHexSpriteHexY = ArmyHexSprite.GetHexY();
            sbyte fArmyHexSpriteHexX = ArmyHexSprite.GetHexX();
            combat.InitializeDefaultArmies(ArmyHexSprite.Army, fArmyHexSpriteHexX, fArmyHexSpriteHexY, fArmyHexSpriteHexLevel);
        }

        combat.Initialize();
        combat.Activate();
        combatResult = combat.Execute();
        _combatMode = ArmyCombatMoveTokenEventMode.CombatExecuted;
        if (combatResult != CombatResult.Busy)
        {
            combat.Deactivate();
            combat.Finalize();
            if (combat is FastBattle)
            {
                DistributeCombatEvent(combat.Logbook, true);
                if (StrategicMap.Current.RequestCombatType && StrategicMap.Current.SeatedPlayer == defender)
                {
                    log = new UserEventLog(null)
                    {
                        Holder = CreationHolder(),
                        Data = this
                    };
                    StrategicMap.Current.TriggerExecuteEventLog(log, false, RestoreHotSeatModeCallback);
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
                condition = StrategicMap.Current.VisibleForPlayer(item5.Location.X, item4.Location.Y, item3.Location.MapLevel, StrategicMap.Current.SeatedPlayer);
            }

            if (condition)
            {
                animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                animation.SetImageSequence(GamePack.Current.ImageSequences.ImageSequences[22]);
                PathStep item3 = Path[0];
                PathStep item4 = Path[0];
                PathStep item5 = Path[0];
                _ = StrategicMap.Current.PlaceHex(animation, item5.Location.X, item4.Location.Y, item3.Location.MapLevel);
            }

            StrategicMap.Current.DestroyCombat();
            StrategicMap.Current.UnlockGameOverEvent();
            ArmyHexSprite = GetMapField(Path[MoveIndex].Location).GetArmyHexSprite();
            if (ArmyHexSprite is not null && Set<CombatResult>.Of(CombatResult.CannotInflictDamage, CombatResult.Undecided, CombatResult.CannotBreachWall, CombatResult.Fled).Contains(combatResult))
            {
                FinishMove();
                ArmyHexSprite = null;
            }

            if (ArmyHexSprite is not null)
            {
                ArmyHexSprite.CancelMove();
                bool moved;
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
                            if (ArmyHexSprite.Army.Units.GetUnit(i).UnitStatus.Contains(UnitStatusElement.Moving))
                            {
                                Troop unit = ArmyHexSprite.Army.Units.GetUnit(i);
                                ArmyHexSprite.Army.Units.GetUnit(i).UnitStatus = unit.UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
                                Troop fArmyUnit = ArmyHexSprite.Army.Units.GetUnit(i);
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
                        _ = armyHexSprite.PlacePrivate(StrategicMap.Current.MapContainer, item5.Location.X, item4.Location.Y, item3.Location.MapLevel);
                        if (ArmyHexSprite.GetSelected())
                        {
                            StrategicMap.Current.Selection.UnselectLocation();
                            StrategicMap.Current.Selection.SelectLocationHexSprite(armyHexSprite);
                        }
                    }

                    ArmyHexSprite armyHexSprite4 = ArmyHexSprite;
                    armyHexSprite4.Army.Mode = ArmyHexSprite.Army.Mode - Set<ArmyModeElement>.Of(ArmyModeElement.Moving);
                    ArmyHexSprite = armyHexSprite4;
                }

                if (moved)
                {
                    movedArmyHexSprite = armyHexSprite;
                    ArmyHexSprite = movedArmyHexSprite;
                    movedArmyHexSprite.Army.Mode = movedArmyHexSprite.Army.Mode + Set<ArmyModeElement>.Of(ArmyModeElement.Moving);
                    Selection = 255;
                    if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.NoDefenders).Contains(combatResult) || base.CanMoveNext())
                    {
                        int direction = GetDirection(MoveIndex);
                        _ = ArmyHexSprite.MoveFirst(direction, false);
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
