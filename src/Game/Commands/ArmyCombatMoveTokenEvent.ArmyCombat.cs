#nullable disable
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Map;
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
                var unit = ArmyHexSprite.Army.Units.GetUnit(i);
                ArmyHexSprite.Army.Units.GetUnit(i).UnitStatus = unit.UnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
            }
            else
            {
                var unit = ArmyHexSprite.Army.Units.GetUnit(i);
                ArmyHexSprite.Army.Units.GetUnit(i).UnitStatus = unit.UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        if (otherArmyHexSprite != null)
        {
            using (ArmyHexSprite.Army.BeginUpdate())
            {
                var armyHexSprite2 = ArmyHexSprite;
                armyHexSprite2.Army.Mode = ArmyHexSprite.Army.Mode + Set<ArmyModeElement>.Of(ArmyModeElement.DisableTerrainValidation);
                ArmyHexSprite = armyHexSprite2;
                for (int i = otherArmyHexSprite.Army.Units.Count - 1; i >= 0; i--)
                {
                    troop = otherArmyHexSprite.Army.Units.GetUnit(i);
                    troop.UnitStatus = troop.UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
                    ArmyHexSprite.Army.AddUnit(troop);
                }

                var armyHexSprite3 = ArmyHexSprite;
                armyHexSprite3.Army.Mode = ArmyHexSprite.Army.Mode - Set<ArmyModeElement>.Of(ArmyModeElement.DisableTerrainValidation);
                ArmyHexSprite = armyHexSprite3;
            }
        }

        ArmyHexSprite.CancelMove();
        StrategicMap.Current.LockGameOverEvent();
        if (_combatType == CombatType.Fast)
        {
            combat = StrategicMap.Current.CreateCombat(131760);
        }
        else
        {
            combat = StrategicMap.Current.CreateCombat(2228496);
        }

        var hexY = ArmyHexSprite.GetHexY();
        var hexLevel = ArmyHexSprite.GetHexLevel();
        var player = field.GetPlayer();
        var independentRelation = field.GetIndependentRelation();
        var item = Path[0];
        var fPathItem = Path[0];
        var item2 = Path[0];
        var hexX = ArmyHexSprite.GetHexX();
        combat.Setup((byte)ArmyHexSprite.Army.Player, (byte)ArmyHexSprite.Army.IndependentRelation, hexX, hexY, hexLevel, (byte)player, independentRelation, item.Location.X, fPathItem.Location.Y, item2.Location.MapLevel, _combatType == CombatType.Fast);
        combat.SetExecutedHandler(CombatExecuted);
        message.MessageId = 4359;
        var param = message.Param;
        param[0] = combat;
        message.Param = param;
        var param2 = message.Param;
        param2[1] = ArmyHexSprite;
        message.Param = param2;
        message.Result = 0;
        field.SendMapFieldMessage(ref message);
        if (message.Result.Int == 0)
        {
            var fArmyHexSpriteHexLevel = ArmyHexSprite.GetHexLevel();
            var fArmyHexSpriteHexY = ArmyHexSprite.GetHexY();
            var fArmyHexSpriteHexX = ArmyHexSprite.GetHexX();
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
                    log = new UserEventLog(null) { Holder = CreationHolder() };
                    log.Data = this;
                    StrategicMap.Current.TriggerExecuteEventLog(log, false, RestoreHotSeatModeCallback);
                    log.Release();
                    StrategicMap.Current.AIExecutor.Lock();
                    StrategicMap.Current.LockGameOverEvent();
                    _combatMode = ArmyCombatMoveTokenEventMode.RestoringHotSeat;
                }
            }

            bool condition = combatResult != CombatResult.NoDefenders;
            if (condition)
            {
                var item3 = Path[0];
                var item4 = Path[0];
                var item5 = Path[0];
                condition = StrategicMap.Current.VisibleForPlayer(item5.Location.X, item4.Location.Y, item3.Location.MapLevel, StrategicMap.Current.SeatedPlayer);
            }

            if (condition)
            {
                animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                animation.SetImageSequence(GamePack.Current.ImageSequences.ImageSequences[22]);
                var item3 = Path[0];
                var item4 = Path[0];
                var item5 = Path[0];
                StrategicMap.Current.PlaceHex(animation, item5.Location.X, item4.Location.Y, item3.Location.MapLevel);
            }

            StrategicMap.Current.DestroyCombat();
            StrategicMap.Current.UnlockGameOverEvent();
            ArmyHexSprite = (ArmyHexSprite)GetMapField(Path[MoveIndex].Location).FindOwnedChild(131607);
            if (ArmyHexSprite != null && Set<CombatResult>.Of(CombatResult.CannotInflictDamage, CombatResult.Undecided, CombatResult.CannotBreachWall, CombatResult.Fled).Contains(combatResult))
            {
                FinishMove();
                ArmyHexSprite = null;
            }

            if (ArmyHexSprite != null)
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
                                var unit = ArmyHexSprite.Army.Units.GetUnit(i);
                                ArmyHexSprite.Army.Units.GetUnit(i).UnitStatus = unit.UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
                                var fArmyUnit = ArmyHexSprite.Army.Units.GetUnit(i);
                                armyHexSprite.Army.AddUnit(fArmyUnit);
                            }
                        }
                    }

                    moved = armyHexSprite.Army.Units.Count != 0;
                    if (moved)
                    {
                        var item3 = Path[MoveIndex];
                        var item4 = Path[MoveIndex];
                        var item5 = Path[MoveIndex];
                        armyHexSprite.PlacePrivate(StrategicMap.Current.MapContainer, item5.Location.X, item4.Location.Y, item3.Location.MapLevel);
                        if (ArmyHexSprite.GetSelected())
                        {
                            StrategicMap.Current.Selection.UnselectLocation();
                            StrategicMap.Current.Selection.SelectLocationHexSprite(armyHexSprite);
                        }
                    }

                    var armyHexSprite4 = ArmyHexSprite;
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
                        var direction = GetDirection(MoveIndex);
                        ArmyHexSprite.MoveFirst(direction, false);
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
