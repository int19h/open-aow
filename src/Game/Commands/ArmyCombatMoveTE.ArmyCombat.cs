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

public partial class ArmyCombatMoveTE
{
    public void ExecuteCombat()
    {
        if ((HeadlessHooks.Tactical || FrameFixtureInputs.Tactical) && (HeadlessHooks.TacticalBattles == 0 || HeadlessHooks.TacticalSelections < HeadlessHooks.TacticalBattles))
        {
            FCombatType = CombatType.Tactical;
            HeadlessHooks.TacticalSelections++;
        }
        CombatResult CombatResult = default;
        AoWMapField Field = default;
        AoWMapField Source = default;
        sbyte Defender = default;
        ArmyHS OtherArmyHS = default;
        ArmyHS ArmyHS = default;
        ArmyHS MovedArmyHS = default;
        Combat.Battle Combat = default;
        MapFieldMsg Msg = default;
        UserEventLog Log = default;
        ImageSequenceAnimation Anim = default;
        AbstractUnit AUnit = default;
        Field = GetMapF(FPath[0]);
        Defender = Field.GetPlayer();
        Source = (AoWMapField)FArmyHS.Owner;
        FArmyHS.SetOwner(null);
        OtherArmyHS = Source.GetArmyHS();
        FArmyHS.SetOwner(Source);
        for (int I = 0, last = FArmyHS.FArmy.Count - 1; I <= last; I++)
        {
            if ((FSelection & 1) != 0)
            {
                var unit = FArmyHS.FArmy.GetUnit(I);
                FArmyHS.FArmy.GetUnit(I).FUnitStatus = unit.FUnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
            }
            else
            {
                var unit = FArmyHS.FArmy.GetUnit(I);
                FArmyHS.FArmy.GetUnit(I).FUnitStatus = unit.FUnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
            }

            FSelection = (byte)(int)(((uint)FSelection) >> 1);
        }

        if (OtherArmyHS != null)
        {
            FArmyHS.FArmy.BeginUpdate();
            FArmyHS.FArmy.FMode = FArmyHS.FArmy.FMode + Set<ArmyModeElement>.Of(ArmyModeElement.DisableTerrainValidation);
            for (int I = OtherArmyHS.FArmy.Count - 1; I >= 0; I--)
            {
                AUnit = OtherArmyHS.FArmy.GetUnit(I);
                AUnit.FUnitStatus = AUnit.FUnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
                FArmyHS.FArmy.AddUnit(AUnit);
            }

            FArmyHS.FArmy.FMode = FArmyHS.FArmy.FMode - Set<ArmyModeElement>.Of(ArmyModeElement.DisableTerrainValidation);
            FArmyHS.FArmy.EndUpdate();
        }

        FArmyHS.CancelMove();
        AoWHSMap.Current.LockGameOverEvent();
        if (FCombatType == CombatType.Fast)
        {
            Combat = AoWHSMap.Current.CreateCombat(131760);
        }
        else
        {
            Combat = AoWHSMap.Current.CreateCombat(2228496);
        }

        var yhx = FArmyHS.GetYhx();
        var lhx = FArmyHS.GetLhx();
        var player = Field.GetPlayer();
        var independentRelation = Field.GetIndependentRelation();
        var item = FPath[0];
        var fPathItem = FPath[0];
        var item2 = FPath[0];
        var xhx = FArmyHS.GetXhx();
        Combat.Setup((byte)FArmyHS.FArmy.FPlayer, (byte)FArmyHS.FArmy.FIndependentRelation, xhx, yhx, lhx, (byte)player, independentRelation, item.X, fPathItem.Y, item2.L, FCombatType == CombatType.Fast);
        Combat.FOnExecuted = CombatExecuted;
        Msg.MsgID = 4359;
        Msg.Param[0] = Combat;
        Msg.Param[1] = FArmyHS;
        Msg.Result = 0;
        Field.SendMapFieldMsg(ref Msg);
        if (Msg.Result.Int == 0)
        {
            var fArmyHSLhx = FArmyHS.GetLhx();
            var fArmyHSYhx = FArmyHS.GetYhx();
            var fArmyHSXhx = FArmyHS.GetXhx();
            Combat.InitDefaultArmies(FArmyHS.FArmy, fArmyHSXhx, fArmyHSYhx, fArmyHSLhx);
        }

        Combat.Initialize();
        Combat.Activate();
        CombatResult = Combat.Execute();
        FCombatMode = CombatMoveMode.CombatExecuted;
        if (CombatResult != Game.Combat.CombatResult.Busy)
        {
            Combat.Deactivate();
            Combat.Finalize();
            if (Combat is FastBattle)
            {
                DistributeCombatEvent(Combat.FLogbook, true);
                if (AoWHSMap.Current.FRequestCombatType && AoWHSMap.Current.FSeatedPlayer == Defender)
                {
                    Log = new UserEventLog(null);
                    Log.FData = this;
                    AoWHSMap.Current.TriggerExecuteEventLog(Log, false, RestoreHotSeatModeCallBack);
                    Log.Release();
                    AoWHSMap.Current.FAIExecuter.Lock();
                    AoWHSMap.Current.LockGameOverEvent();
                    FCombatMode = CombatMoveMode.RestoringHotseat;
                }
            }

            bool condition = CombatResult != Game.Combat.CombatResult.NoDefenders;
            if (condition)
            {
                var item3 = FPath[0];
                var item4 = FPath[0];
                var item5 = FPath[0];
                condition = AoWHSMap.Current.VisibleForPlayer(item5.X, item4.Y, item3.L, AoWHSMap.Current.FSeatedPlayer);
            }

            if (condition)
            {
                Anim = new ImageSequenceAnimation(null);
                Anim.SetImageSequence(AoWHSSet.Current.FImageSequences.ImageSequences[22]);
                var item3 = FPath[0];
                var item4 = FPath[0];
                var item5 = FPath[0];
                AoWHSMap.Current.PlaceHX(Anim, item5.X, item4.Y, item3.L);
            }

            AoWHSMap.Current.DestroyCombat();
            AoWHSMap.Current.UnlockGameOverEvent();
            FArmyHS = (ArmyHS)GetMapF(FPath[FMoveIndex]).FindOwnedChild(131607);
            if (FArmyHS != null && Set<CombatResult>.Of(Game.Combat.CombatResult.CannotInflictDamage, Game.Combat.CombatResult.Undecided, Game.Combat.CombatResult.CannotBreachWall, Game.Combat.CombatResult.Fled).Contains(CombatResult))
            {
                FinishMove();
                FArmyHS = null;
            }

            if (FArmyHS != null)
            {
                FArmyHS.CancelMove();
                FArmyHS.FArmy.BeginUpdate();
                ArmyHS = new ArmyHS(null);
                ArmyHS.FArmy.BeginUpdate();
                ArmyHS.FArmy.AssignSettings(FArmyHS.FArmy);
                ArmyHS.FDirection = FArmyHS.FDirection;
                ArmyHS.FArmy.SetPlayer(FArmyHS.FArmy.FPlayer);
                for (int I = FArmyHS.FArmy.Count - 1; I >= 0; I--)
                {
                    if (FArmyHS.FArmy.GetUnit(I).FUnitStatus.Contains(UnitStatusElement.Moving))
                    {
                        var unit = FArmyHS.FArmy.GetUnit(I);
                        FArmyHS.FArmy.GetUnit(I).FUnitStatus = unit.FUnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
                        var fArmyUnit = FArmyHS.FArmy.GetUnit(I);
                        ArmyHS.FArmy.AddUnit(fArmyUnit);
                    }
                }

                ArmyHS.FArmy.EndUpdate();
                if (ArmyHS.FArmy.Count != 0)
                {
                    var item3 = FPath[FMoveIndex];
                    var item4 = FPath[FMoveIndex];
                    var item5 = FPath[FMoveIndex];
                    ArmyHS.PlacePrivate(AoWHSMap.Current.MapContainer, item5.X, item4.Y, item3.L);
                    if (FArmyHS.GetSelected())
                    {
                        AoWHSMap.Current.Selection.UnselectLocation();
                        AoWHSMap.Current.Selection.SelectLocationHS(ArmyHS);
                    }

                    FArmyHS.FArmy.FMode = FArmyHS.FArmy.FMode - Set<ArmyModeElement>.Of(ArmyModeElement.Moving);
                    FArmyHS.FArmy.EndUpdate();
                    MovedArmyHS = ArmyHS;
                    FArmyHS = MovedArmyHS;
                    MovedArmyHS.FArmy.FMode = MovedArmyHS.FArmy.FMode + Set<ArmyModeElement>.Of(ArmyModeElement.Moving);
                    FSelection = 255;
                    if (Set<CombatResult>.Of(Game.Combat.CombatResult.Victory, Game.Combat.CombatResult.NoDefenders).Contains(CombatResult) || base.CanMoveNext())
                    {
                        var direction = GetDirection(FMoveIndex);
                        FArmyHS.MoveFirst(direction, false);
                        FArmyHS.SetMoveStatus(50);
                    }
                    else
                    {
                        FinishMove();
                        FArmyHS = null;
                        return;
                    }
                }
                else
                {
                    // { The original drops the empty new army without freeing it. }
                    FArmyHS.FArmy.FMode = FArmyHS.FArmy.FMode - Set<ArmyModeElement>.Of(ArmyModeElement.Moving);
                    FArmyHS.FArmy.EndUpdate();
                    FArmyHS = null;
                }
            }
        }
        else
        {
            AoWHSMap.Current.UnlockGameOverEvent();
            FArmyHS = null;
            FinishMove();
        }
    }
}
