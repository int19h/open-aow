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
            int I = default;
        TCombat Combat = default;
        TMapFieldMsg Msg = default;
        TUserEventLog Log = default;
        TImageSequenceAnimation Anim = default;
        TAbstractUnit AUnit = default;
            var call__6625 = FPath[0];
            var call__6626 = GetMapF(call__6625);
            Field = call__6626;
            Defender = Field.GetPlayer();
            Source = (TAoWMapField)FArmyHS.Owner;
            FArmyHS.SetOwner(null);
            OtherArmyHS = Source.GetArmyHS();
            FArmyHS.SetOwner(Source);
            {
                I = 0;
                int end__6627 = FArmyHS.FArmy.Count - 1;
                for (; I <= end__6627; I += 1)
                {
                    if ((FSelection & 1) != 0)
                    {
                        var call__6628 = FArmyHS.FArmy.GetUnit(I);
                        var call__6629 = FArmyHS.FArmy.GetUnit(I);
                        call__6629.FUnitStatus = call__6628.FUnitStatus + TSet<TUnitStatusElement>.Of(TUnitStatusElement.usMoving);
                    }
                    else
                    {
                        var call__6630 = FArmyHS.FArmy.GetUnit(I);
                        var call__6631 = FArmyHS.FArmy.GetUnit(I);
                        call__6631.FUnitStatus = call__6630.FUnitStatus - TSet<TUnitStatusElement>.Of(TUnitStatusElement.usMoving);
                    }

                    FSelection = (byte)(int)(((uint)FSelection) >> 1);
                }
            }

            if (OtherArmyHS != null)
            {
                FArmyHS.FArmy.BeginUpdate();
                FArmyHS.FArmy.FMode = FArmyHS.FArmy.FMode + TSet<TArmyModeElement>.Of(TArmyModeElement.amDisableTerrainValidation);
                {
                    I = OtherArmyHS.FArmy.Count - 1;
                    byte end__6632 = 0;
                    for (; I >= end__6632; I -= 1)
                    {
                        var call__6633 = OtherArmyHS.FArmy.GetUnit(I);
                        AUnit = call__6633;
                        AUnit.FUnitStatus = AUnit.FUnitStatus - TSet<TUnitStatusElement>.Of(TUnitStatusElement.usMoving);
                        FArmyHS.FArmy.AddUnit(AUnit);
                    }
                }

                FArmyHS.FArmy.FMode = FArmyHS.FArmy.FMode - TSet<TArmyModeElement>.Of(TArmyModeElement.amDisableTerrainValidation);
                FArmyHS.FArmy.EndUpdate();
            }

            FArmyHS.CancelMove();
        AoWE.AoWHSMap.LockGameOverEvent();
            if (FCombatType == TCombatType.ctFast)
            {
                var call__6634 = AoWE.AoWHSMap.CreateCombat(131760);
                Combat = call__6634;
            }
            else
            {
                var call__6635 = AoWE.AoWHSMap.CreateCombat(2228496);
                Combat = call__6635;
            }

            var call__6636 = FArmyHS.GetYhx();
            var call__6637 = FArmyHS.GetLhx();
            var call__6638 = Field.GetPlayer();
            var call__6639 = Field.GetIndependentRelation();
            var call__6640 = FPath[0];
            var call__6641 = FPath[0];
            var call__6642 = FPath[0];
            var call__6643 = FArmyHS.GetXhx();
            Combat.Setup((byte)FArmyHS.FArmy.FPlayer, (byte)FArmyHS.FArmy.FIndependentRelation, call__6643, call__6636, call__6637, (byte)call__6638, call__6639, call__6640.X, call__6641.Y, call__6642.L, FCombatType == TCombatType.ctFast);
            Combat.FOnExecuted = CombatExecuted;
            Msg.MsgID = 4359;
            Msg.Param[0] = Combat;
            Msg.Param[1] = FArmyHS;
            Msg.Result = 0;
            Field.SendMapFieldMsg(ref Msg);
            if (Msg.Result.Value == 0)
            {
                var call__6644 = FArmyHS.GetLhx();
                var call__6645 = FArmyHS.GetYhx();
                var call__6646 = FArmyHS.GetXhx();
                Combat.InitDefaultArmies(FArmyHS.FArmy, call__6646, call__6645, call__6644);
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
                AoWE.DistributeCombatEvent(Combat.FLogbook, true);
                    if (AoWE.AoWHSMap.FRequestCombatType && AoWE.AoWHSMap.FSeatedPlayer == Defender)
                    {
                        Log = new TUserEventLog(null);
                        Log.FData = this;
                    AoWE.AoWHSMap.TriggerExecuteEventLog(Log, false, RestoreHotSeatModeCallBack);
                        Log.Release();
                    AoWE.AoWHSMap.FAIExecuter.Lock();
                    AoWE.AoWHSMap.LockGameOverEvent();
                        FCombatMode = TCombatMoveMode.cmRestoringHotseat;
                    }
                }

                bool bool__6647 = CombatResult != TCombatResult.crNoDefenders;
                if (bool__6647)
                {
                    var call__6648 = FPath[0];
                    var call__6649 = FPath[0];
                    var call__6650 = FPath[0];
                    var call__6651 = AoWE.AoWHSMap.VisibleForPlayer(call__6650.X, call__6649.Y, call__6648.L, AoWE.AoWHSMap.FSeatedPlayer);
                    bool__6647 = call__6651;
                }

                if (bool__6647)
                {
                    Anim = new TImageSequenceAnimation(null);
                    var call__6652 = AoWE.AoWHSSet.FImageSequences.ImageSequences[22];
                    Anim.SetImageSequence(call__6652);
                    var call__6653 = FPath[0];
                    var call__6654 = FPath[0];
                    var call__6655 = FPath[0];
                AoWE.AoWHSMap.PlaceHX(Anim, call__6655.X, call__6654.Y, call__6653.L);
                }

            AoWE.AoWHSMap.DestroyCombat();
            AoWE.AoWHSMap.UnlockGameOverEvent();
                var call__6656 = FPath[FMoveIndex];
                var call__6657 = GetMapF(call__6656);
                var call__6658 = call__6657.FindOwnedChild(131607);
                FArmyHS = (TArmyHS)call__6658;
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
                    {
                        I = FArmyHS.FArmy.Count - 1;
                        byte end__6659 = 0;
                        for (; I >= end__6659; I -= 1)
                        {
                            var call__6660 = FArmyHS.FArmy.GetUnit(I);
                            if (call__6660.FUnitStatus.Contains(TUnitStatusElement.usMoving))
                            {
                                var call__6661 = FArmyHS.FArmy.GetUnit(I);
                                var call__6662 = FArmyHS.FArmy.GetUnit(I);
                                call__6662.FUnitStatus = call__6661.FUnitStatus - TSet<TUnitStatusElement>.Of(TUnitStatusElement.usMoving);
                                var call__6663 = FArmyHS.FArmy.GetUnit(I);
                                ArmyHS.FArmy.AddUnit(call__6663);
                            }
                        }
                    }

                    ArmyHS.FArmy.EndUpdate();
                    var call__6664 = ArmyHS.FArmy.Count;
                    if (call__6664 != 0)
                    {
                        var call__6665 = FPath[FMoveIndex];
                        var call__6666 = FPath[FMoveIndex];
                        var call__6667 = FPath[FMoveIndex];
                        ArmyHS.PlacePrivate(AoWE.AoWHSMap.MapContainer, call__6667.X, call__6666.Y, call__6665.L);
                        var call__6668 = FArmyHS.GetSelected();
                        if (call__6668)
                        {
                        AoWE.AoWHSMap.Selection.UnselectLocation();
                        AoWE.AoWHSMap.Selection.SelectLocationHS(ArmyHS);
                        }

                        FArmyHS.FArmy.FMode = FArmyHS.FArmy.FMode - TSet<TArmyModeElement>.Of(TArmyModeElement.amMoving);
                        FArmyHS.FArmy.EndUpdate();
                        MovedArmyHS = ArmyHS;
                        FArmyHS = MovedArmyHS;
                        MovedArmyHS.FArmy.FMode = MovedArmyHS.FArmy.FMode + TSet<TArmyModeElement>.Of(TArmyModeElement.amMoving);
                        FSelection = 255;
                        if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crNoDefenders).Contains(CombatResult) || base.CanMoveNext())
                        {
                            var call__6669 = GetDirection(FMoveIndex);
                            FArmyHS.MoveFirst(call__6669, false);
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
            AoWE.AoWHSMap.UnlockGameOverEvent();
                FArmyHS = null;
                FinishMove();
            }
        }
}
