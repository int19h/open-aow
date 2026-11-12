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
        Combat.Battle combat = default;
        MapFieldMessage message = default;
        UserEventLog log = default;
        ImageSequenceAnimation animation = default;
        Troop troop = default;
        field = GetMapField(Path[0]);
        defender = field.GetPlayer();
        source = ArmyHexSprite.Owner;
        ArmyHexSprite.SetOwner(null);
        otherArmyHexSprite = source.GetArmyHexSprite();
        ArmyHexSprite.SetOwner(source);
        for (int I = 0, last = ArmyHexSprite.Army.Count - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                var unit = ArmyHexSprite.Army.GetUnit(I);
                ArmyHexSprite.Army.GetUnit(I).UnitStatus = unit.UnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
            }
            else
            {
                var unit = ArmyHexSprite.Army.GetUnit(I);
                ArmyHexSprite.Army.GetUnit(I).UnitStatus = unit.UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        if (otherArmyHexSprite != null)
        {
            ArmyHexSprite.Army.BeginUpdate();
            ArmyHexSprite.Army.Mode = ArmyHexSprite.Army.Mode + Set<ArmyModeElement>.Of(ArmyModeElement.DisableTerrainValidation);
            for (int I = otherArmyHexSprite.Army.Count - 1; I >= 0; I--)
            {
                troop = otherArmyHexSprite.Army.GetUnit(I);
                troop.UnitStatus = troop.UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
                ArmyHexSprite.Army.AddUnit(troop);
            }

            ArmyHexSprite.Army.Mode = ArmyHexSprite.Army.Mode - Set<ArmyModeElement>.Of(ArmyModeElement.DisableTerrainValidation);
            ArmyHexSprite.Army.EndUpdate();
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
        combat.Setup((byte)ArmyHexSprite.Army.Player, (byte)ArmyHexSprite.Army.IndependentRelation, hexX, hexY, hexLevel, (byte)player, independentRelation, item.X, fPathItem.Y, item2.MapLevel, _combatType == CombatType.Fast);
        combat.SetExecutedHandler(CombatExecuted);
        message.MessageId = 4359;
        message.Param[0] = combat;
        message.Param[1] = ArmyHexSprite;
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
        if (combatResult != Game.Combat.CombatResult.Busy)
        {
            combat.Deactivate();
            combat.Finalize();
            if (combat is FastBattle)
            {
                DistributeCombatEvent(combat.Logbook, true);
                if (StrategicMap.Current.RequestCombatType && StrategicMap.Current.SeatedPlayer == defender)
                {
                    log = new UserEventLog(null);
                    log.Data = this;
                    StrategicMap.Current.TriggerExecuteEventLog(log, false, RestoreHotSeatModeCallback);
                    log.Release();
                    StrategicMap.Current.AIExecutor.Lock();
                    StrategicMap.Current.LockGameOverEvent();
                    _combatMode = ArmyCombatMoveTokenEventMode.RestoringHotSeat;
                }
            }

            bool condition = combatResult != Game.Combat.CombatResult.NoDefenders;
            if (condition)
            {
                var item3 = Path[0];
                var item4 = Path[0];
                var item5 = Path[0];
                condition = StrategicMap.Current.VisibleForPlayer(item5.X, item4.Y, item3.MapLevel, StrategicMap.Current.SeatedPlayer);
            }

            if (condition)
            {
                animation = new ImageSequenceAnimation(null);
                animation.SetImageSequence(GamePack.Current.ImageSequences.ImageSequences[22]);
                var item3 = Path[0];
                var item4 = Path[0];
                var item5 = Path[0];
                StrategicMap.Current.PlaceHex(animation, item5.X, item4.Y, item3.MapLevel);
            }

            StrategicMap.Current.DestroyCombat();
            StrategicMap.Current.UnlockGameOverEvent();
            ArmyHexSprite = (ArmyHexSprite)GetMapField(Path[MoveIndex]).FindOwnedChild(131607);
            if (ArmyHexSprite != null && Set<CombatResult>.Of(Game.Combat.CombatResult.CannotInflictDamage, Game.Combat.CombatResult.Undecided, Game.Combat.CombatResult.CannotBreachWall, Game.Combat.CombatResult.Fled).Contains(combatResult))
            {
                FinishMove();
                ArmyHexSprite = null;
            }

            if (ArmyHexSprite != null)
            {
                ArmyHexSprite.CancelMove();
                ArmyHexSprite.Army.BeginUpdate();
                armyHexSprite = new ArmyHexSprite(null);
                armyHexSprite.Army.BeginUpdate();
                armyHexSprite.Army.AssignSettings(ArmyHexSprite.Army);
                armyHexSprite.Direction = ArmyHexSprite.Direction;
                armyHexSprite.Army.SetPlayer(ArmyHexSprite.Army.Player);
                for (int I = ArmyHexSprite.Army.Count - 1; I >= 0; I--)
                {
                    if (ArmyHexSprite.Army.GetUnit(I).UnitStatus.Contains(UnitStatusElement.Moving))
                    {
                        var unit = ArmyHexSprite.Army.GetUnit(I);
                        ArmyHexSprite.Army.GetUnit(I).UnitStatus = unit.UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
                        var fArmyUnit = ArmyHexSprite.Army.GetUnit(I);
                        armyHexSprite.Army.AddUnit(fArmyUnit);
                    }
                }

                armyHexSprite.Army.EndUpdate();
                if (armyHexSprite.Army.Count != 0)
                {
                    var item3 = Path[MoveIndex];
                    var item4 = Path[MoveIndex];
                    var item5 = Path[MoveIndex];
                    armyHexSprite.PlacePrivate(StrategicMap.Current.MapContainer, item5.X, item4.Y, item3.MapLevel);
                    if (ArmyHexSprite.GetSelected())
                    {
                        StrategicMap.Current.Selection.UnselectLocation();
                        StrategicMap.Current.Selection.SelectLocationHexSprite(armyHexSprite);
                    }

                    ArmyHexSprite.Army.Mode = ArmyHexSprite.Army.Mode - Set<ArmyModeElement>.Of(ArmyModeElement.Moving);
                    ArmyHexSprite.Army.EndUpdate();
                    movedArmyHexSprite = armyHexSprite;
                    ArmyHexSprite = movedArmyHexSprite;
                    movedArmyHexSprite.Army.Mode = movedArmyHexSprite.Army.Mode + Set<ArmyModeElement>.Of(ArmyModeElement.Moving);
                    Selection = 255;
                    if (Set<CombatResult>.Of(Game.Combat.CombatResult.Victory, Game.Combat.CombatResult.NoDefenders).Contains(combatResult) || base.CanMoveNext())
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
                    ArmyHexSprite.Army.Mode = ArmyHexSprite.Army.Mode - Set<ArmyModeElement>.Of(ArmyModeElement.Moving);
                    ArmyHexSprite.Army.EndUpdate();
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
