using Aow.Rtl;
using Aow.Units.Animation;
using Aow.Units.EventLog;
using Aow.Units.HSEngine;

namespace Aow.Units.AoWE;

public partial class TArmyCombatMoveTE
{
    public void ExecuteCombat()
    {
        if ((HeadlessHooks.Tactical || FrameFixtureInputs.Tactical) && (HeadlessHooks.TacticalBattles == 0 || HeadlessHooks.TacticalSelections < HeadlessHooks.TacticalBattles))
        {
            FCombatType = TCombatType.ctTactical;
            HeadlessHooks.TacticalSelections++;
        }
        TCombatResult CombatResult = default;
        TAoWMapField Field = default;
        TAoWMapField Source = default;
        sbyte Defender = default;
        TArmyHS OtherArmyHS = default;
        TArmyHS ArmyHS = default;
        TArmyHS MovedArmyHS = default;
        TCombat Combat = default;
        TMapFieldMsg Msg = default;
        TUserEventLog Log = default;
        TImageSequenceAnimation Anim = default;
        TAbstractUnit AUnit = default;
        Field = GetMapF(FPath[0]);
        Defender = Field.GetPlayer();
        Source = (TAoWMapField)FArmyHS.Owner;
        FArmyHS.SetOwner(null);
        OtherArmyHS = Source.GetArmyHS();
        FArmyHS.SetOwner(Source);
        for (int I = 0, last = FArmyHS.FArmy.Count - 1; I <= last; I++)
        {
            if ((FSelection & 1) != 0)
            {
                var unit = FArmyHS.FArmy.GetUnit(I);
                FArmyHS.FArmy.GetUnit(I).FUnitStatus = unit.FUnitStatus + TSet<TUnitStatusElement>.Of(TUnitStatusElement.usMoving);
            }
            else
            {
                var unit = FArmyHS.FArmy.GetUnit(I);
                FArmyHS.FArmy.GetUnit(I).FUnitStatus = unit.FUnitStatus - TSet<TUnitStatusElement>.Of(TUnitStatusElement.usMoving);
            }

            FSelection = (byte)(int)(((uint)FSelection) >> 1);
        }

        if (OtherArmyHS != null)
        {
            FArmyHS.FArmy.BeginUpdate();
            FArmyHS.FArmy.FMode = FArmyHS.FArmy.FMode + TSet<TArmyModeElement>.Of(TArmyModeElement.amDisableTerrainValidation);
            for (int I = OtherArmyHS.FArmy.Count - 1; I >= 0; I--)
            {
                AUnit = OtherArmyHS.FArmy.GetUnit(I);
                AUnit.FUnitStatus = AUnit.FUnitStatus - TSet<TUnitStatusElement>.Of(TUnitStatusElement.usMoving);
                FArmyHS.FArmy.AddUnit(AUnit);
            }

            FArmyHS.FArmy.FMode = FArmyHS.FArmy.FMode - TSet<TArmyModeElement>.Of(TArmyModeElement.amDisableTerrainValidation);
            FArmyHS.FArmy.EndUpdate();
        }

        FArmyHS.CancelMove();
        AoWEUnit.AoWHSMap.LockGameOverEvent();
        if (FCombatType == TCombatType.ctFast)
        {
            Combat = AoWEUnit.AoWHSMap.CreateCombat(131760);
        }
        else
        {
            Combat = AoWEUnit.AoWHSMap.CreateCombat(2228496);
        }

        var yhx = FArmyHS.GetYhx();
        var lhx = FArmyHS.GetLhx();
        var player = Field.GetPlayer();
        var independentRelation = Field.GetIndependentRelation();
        var item = FPath[0];
        var fPathItem = FPath[0];
        var item2 = FPath[0];
        var xhx = FArmyHS.GetXhx();
        Combat.Setup((byte)FArmyHS.FArmy.FPlayer, (byte)FArmyHS.FArmy.FIndependentRelation, xhx, yhx, lhx, (byte)player, independentRelation, item.X, fPathItem.Y, item2.L, FCombatType == TCombatType.ctFast);
        Combat.FOnExecuted = CombatExecuted;
        Msg.MsgID = 4359;
        Msg.Param[0] = Combat;
        Msg.Param[1] = FArmyHS;
        Msg.Result = 0;
        Field.SendMapFieldMsg(ref Msg);
        if (Msg.Result.Value == 0)
        {
            var fArmyHSLhx = FArmyHS.GetLhx();
            var fArmyHSYhx = FArmyHS.GetYhx();
            var fArmyHSXhx = FArmyHS.GetXhx();
            Combat.InitDefaultArmies(FArmyHS.FArmy, fArmyHSXhx, fArmyHSYhx, fArmyHSLhx);
        }

        Combat.Initialize();
        Combat.Activate();
        CombatResult = Combat.Execute();
        FCombatMode = TCombatMoveMode.cmCombatExecuted;
        if (CombatResult != TCombatResult.crBusy)
        {
            Combat.Deactivate();
            Combat.Finalize();
            if (Combat is TFastCombat)
            {
                AoWEUnit.DistributeCombatEvent(Combat.FLogbook, true);
                if (AoWEUnit.AoWHSMap.FRequestCombatType && AoWEUnit.AoWHSMap.FSeatedPlayer == Defender)
                {
                    Log = new TUserEventLog(null);
                    Log.FData = this;
                    AoWEUnit.AoWHSMap.TriggerExecuteEventLog(Log, false, RestoreHotSeatModeCallBack);
                    Log.Release();
                    AoWEUnit.AoWHSMap.FAIExecuter.Lock();
                    AoWEUnit.AoWHSMap.LockGameOverEvent();
                    FCombatMode = TCombatMoveMode.cmRestoringHotseat;
                }
            }

            bool condition = CombatResult != TCombatResult.crNoDefenders;
            if (condition)
            {
                var item3 = FPath[0];
                var item4 = FPath[0];
                var item5 = FPath[0];
                condition = AoWEUnit.AoWHSMap.VisibleForPlayer(item5.X, item4.Y, item3.L, AoWEUnit.AoWHSMap.FSeatedPlayer);
            }

            if (condition)
            {
                Anim = new TImageSequenceAnimation(null);
                Anim.SetImageSequence(AoWEUnit.AoWHSSet.FImageSequences.ImageSequences[22]);
                var item3 = FPath[0];
                var item4 = FPath[0];
                var item5 = FPath[0];
                AoWEUnit.AoWHSMap.PlaceHX(Anim, item5.X, item4.Y, item3.L);
            }

            AoWEUnit.AoWHSMap.DestroyCombat();
            AoWEUnit.AoWHSMap.UnlockGameOverEvent();
            FArmyHS = (TArmyHS)GetMapF(FPath[FMoveIndex]).FindOwnedChild(131607);
            if (FArmyHS != null && TSet<TCombatResult>.Of(TCombatResult.crCannotInflictDamage, TCombatResult.crUndecided, TCombatResult.crCannotBreachWall, TCombatResult.crFled).Contains(CombatResult))
            {
                FinishMove();
                FArmyHS = null;
            }

            if (FArmyHS != null)
            {
                FArmyHS.CancelMove();
                FArmyHS.FArmy.BeginUpdate();
                ArmyHS = new TArmyHS(null);
                ArmyHS.FArmy.BeginUpdate();
                ArmyHS.FArmy.AssignSettings(FArmyHS.FArmy);
                ArmyHS.FDirection = FArmyHS.FDirection;
                ArmyHS.FArmy.SetPlayer(FArmyHS.FArmy.FPlayer);
                for (int I = FArmyHS.FArmy.Count - 1; I >= 0; I--)
                {
                    if (FArmyHS.FArmy.GetUnit(I).FUnitStatus.Contains(TUnitStatusElement.usMoving))
                    {
                        var unit = FArmyHS.FArmy.GetUnit(I);
                        FArmyHS.FArmy.GetUnit(I).FUnitStatus = unit.FUnitStatus - TSet<TUnitStatusElement>.Of(TUnitStatusElement.usMoving);
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
                    ArmyHS.PlacePrivate(AoWEUnit.AoWHSMap.MapContainer, item5.X, item4.Y, item3.L);
                    if (FArmyHS.GetSelected())
                    {
                        AoWEUnit.AoWHSMap.Selection.UnselectLocation();
                        AoWEUnit.AoWHSMap.Selection.SelectLocationHS(ArmyHS);
                    }

                    FArmyHS.FArmy.FMode = FArmyHS.FArmy.FMode - TSet<TArmyModeElement>.Of(TArmyModeElement.amMoving);
                    FArmyHS.FArmy.EndUpdate();
                    MovedArmyHS = ArmyHS;
                    FArmyHS = MovedArmyHS;
                    MovedArmyHS.FArmy.FMode = MovedArmyHS.FArmy.FMode + TSet<TArmyModeElement>.Of(TArmyModeElement.amMoving);
                    FSelection = 255;
                    if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crNoDefenders).Contains(CombatResult) || base.CanMoveNext())
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
                    FArmyHS.FArmy.FMode = FArmyHS.FArmy.FMode - TSet<TArmyModeElement>.Of(TArmyModeElement.amMoving);
                    FArmyHS.FArmy.EndUpdate();
                    FArmyHS = null;
                }
            }
        }
        else
        {
            AoWEUnit.AoWHSMap.UnlockGameOverEvent();
            FArmyHS = null;
            FinishMove();
        }
    }
}
