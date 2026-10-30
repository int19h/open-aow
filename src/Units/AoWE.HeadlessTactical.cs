using Aow.Rtl;
using Aow.Units;

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
            Aow.Units.AoWE.TCombatResult CombatResult = default;
            Aow.Units.AoWE.TAoWMapField Field = default;
            Aow.Units.AoWE.TAoWMapField Source = default;
            sbyte Defender = default;
            Aow.Units.AoWE.TArmyHS OtherArmyHS = default;
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            Aow.Units.AoWE.TArmyHS MovedArmyHS = default;
            int I = default;
            Aow.Units.AoWE.TCombat Combat = default;
            Aow.Units.HSEngine.TMapFieldMsg Msg = default;
            Aow.Units.EventLog.TUserEventLog Log = default;
            Aow.Units.Animation.TImageSequenceAnimation Anim = default;
            Aow.Units.AoWE.TAbstractUnit AUnit = default;
            var call__6625 = FPath[0];
            var call__6626 = GetMapF(call__6625);
            Field = call__6626;
            Defender = unchecked((sbyte)Field.GetPlayer());
            Source = (Aow.Units.AoWE.TAoWMapField)FArmyHS.Owner;
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
                        call__6629.FUnitStatus = call__6628.FUnitStatus + TSet<Aow.Units.AoWE.TUnitStatusElement>.Of(Aow.Units.AoWE.TUnitStatusElement.usMoving);
                    }
                    else
                    {
                        var call__6630 = FArmyHS.FArmy.GetUnit(I);
                        var call__6631 = FArmyHS.FArmy.GetUnit(I);
                        call__6631.FUnitStatus = call__6630.FUnitStatus - TSet<Aow.Units.AoWE.TUnitStatusElement>.Of(Aow.Units.AoWE.TUnitStatusElement.usMoving);
                    }

                    FSelection = unchecked((byte)((int)(((uint)FSelection) >> 1)));
                }
            }

            if (OtherArmyHS != null)
            {
                FArmyHS.FArmy.BeginUpdate();
                FArmyHS.FArmy.FMode = FArmyHS.FArmy.FMode + TSet<Aow.Units.AoWE.TArmyModeElement>.Of(Aow.Units.AoWE.TArmyModeElement.amDisableTerrainValidation);
                {
                    I = OtherArmyHS.FArmy.Count - 1;
                    byte end__6632 = 0;
                    for (; I >= end__6632; I -= 1)
                    {
                        var call__6633 = OtherArmyHS.FArmy.GetUnit(I);
                        AUnit = call__6633;
                        AUnit.FUnitStatus = AUnit.FUnitStatus - TSet<Aow.Units.AoWE.TUnitStatusElement>.Of(Aow.Units.AoWE.TUnitStatusElement.usMoving);
                        FArmyHS.FArmy.AddUnit(AUnit);
                    }
                }

                FArmyHS.FArmy.FMode = FArmyHS.FArmy.FMode - TSet<Aow.Units.AoWE.TArmyModeElement>.Of(Aow.Units.AoWE.TArmyModeElement.amDisableTerrainValidation);
                FArmyHS.FArmy.EndUpdate();
            }

            FArmyHS.CancelMove();
            Aow.Units.AoWE.AoWE.AoWHSMap.LockGameOverEvent();
            if (FCombatType == Aow.Units.AoWE.TCombatType.ctFast)
            {
                var call__6634 = Aow.Units.AoWE.AoWE.AoWHSMap.CreateCombat(131760);
                Combat = call__6634;
            }
            else
            {
                var call__6635 = Aow.Units.AoWE.AoWE.AoWHSMap.CreateCombat(2228496);
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
            Combat.Setup(unchecked((byte)FArmyHS.FArmy.FPlayer), unchecked((byte)FArmyHS.FArmy.FIndependentRelation), call__6643, call__6636, call__6637, unchecked((byte)call__6638), unchecked((byte)call__6639), call__6640.X, call__6641.Y, call__6642.L, FCombatType == Aow.Units.AoWE.TCombatType.ctFast);
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
            FCombatMode = Aow.Units.AoWE.TCombatMoveMode.cmCombatExecuted;
            if (CombatResult != Aow.Units.AoWE.TCombatResult.crBusy)
            {
                Combat.Deactivate();
                Combat.Finalize();
                if (Combat is Aow.Units.AoWE.TFastCombat)
                {
                    Aow.Units.AoWE.AoWE.DistributeCombatEvent(Combat.FLogbook, true);
                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FRequestCombatType && (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Defender))
                    {
                        Log = Aow.Units.EventLog.TUserEventLog.Create(null);
                        Log.FData = this;
                        Aow.Units.AoWE.AoWE.AoWHSMap.TriggerExecuteEventLog(Log, false, RestoreHotSeatModeCallBack);
                        Log.Release();
                        Aow.Units.AoWE.AoWE.AoWHSMap.FAIExecuter.Lock();
                        Aow.Units.AoWE.AoWE.AoWHSMap.LockGameOverEvent();
                        FCombatMode = Aow.Units.AoWE.TCombatMoveMode.cmRestoringHotseat;
                    }
                }

                bool bool__6647 = (CombatResult != Aow.Units.AoWE.TCombatResult.crNoDefenders);
                if (bool__6647)
                {
                    var call__6648 = FPath[0];
                    var call__6649 = FPath[0];
                    var call__6650 = FPath[0];
                    var call__6651 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(call__6650.X, call__6649.Y, call__6648.L, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    bool__6647 = call__6651;
                }

                if (bool__6647)
                {
                    Anim = new Aow.Units.Animation.TImageSequenceAnimation(null);
                    var call__6652 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[22];
                    Anim.SetImageSequence(call__6652);
                    var call__6653 = FPath[0];
                    var call__6654 = FPath[0];
                    var call__6655 = FPath[0];
                    Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Anim, call__6655.X, call__6654.Y, call__6653.L);
                }

                Aow.Units.AoWE.AoWE.AoWHSMap.DestroyCombat();
                Aow.Units.AoWE.AoWE.AoWHSMap.UnlockGameOverEvent();
                var call__6656 = FPath[FMoveIndex];
                var call__6657 = GetMapF(call__6656);
                var call__6658 = call__6657.FindOwnedChild(131607);
                FArmyHS = (Aow.Units.AoWE.TArmyHS)call__6658;
                if ((FArmyHS != null) && TSet<Aow.Units.AoWE.TCombatResult>.Of(Aow.Units.AoWE.TCombatResult.crCannotInflictDamage, Aow.Units.AoWE.TCombatResult.crUndecided, Aow.Units.AoWE.TCombatResult.crCannotBreachWall, Aow.Units.AoWE.TCombatResult.crFled).Contains(CombatResult))
                {
                    FinishMove();
                    FArmyHS = null;
                }

                if (FArmyHS != null)
                {
                    FArmyHS.CancelMove();
                    FArmyHS.FArmy.BeginUpdate();
                    ArmyHS = Aow.Units.AoWE.TArmyHS.Create(null);
                    ArmyHS.FArmy.BeginUpdate();
                    ArmyHS.FArmy.AssignSettings(FArmyHS.FArmy);
                    ArmyHS.FDirection = unchecked((byte)FArmyHS.FDirection);
                    ArmyHS.FArmy.SetPlayer(unchecked((sbyte)FArmyHS.FArmy.FPlayer));
                    {
                        I = FArmyHS.FArmy.Count - 1;
                        byte end__6659 = 0;
                        for (; I >= end__6659; I -= 1)
                        {
                            var call__6660 = FArmyHS.FArmy.GetUnit(I);
                            if (call__6660.FUnitStatus.Contains(Aow.Units.AoWE.TUnitStatusElement.usMoving))
                            {
                                var call__6661 = FArmyHS.FArmy.GetUnit(I);
                                var call__6662 = FArmyHS.FArmy.GetUnit(I);
                                call__6662.FUnitStatus = call__6661.FUnitStatus - TSet<Aow.Units.AoWE.TUnitStatusElement>.Of(Aow.Units.AoWE.TUnitStatusElement.usMoving);
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
                        ArmyHS.PlacePrivate(Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer, unchecked((sbyte)call__6667.X), unchecked((sbyte)call__6666.Y), unchecked((sbyte)call__6665.L));
                        var call__6668 = FArmyHS.GetSelected();
                        if (call__6668)
                        {
                            Aow.Units.AoWE.AoWE.AoWHSMap.Selection.UnselectLocation();
                            Aow.Units.AoWE.AoWE.AoWHSMap.Selection.SelectLocationHS(ArmyHS);
                        }

                        FArmyHS.FArmy.FMode = FArmyHS.FArmy.FMode - TSet<Aow.Units.AoWE.TArmyModeElement>.Of(Aow.Units.AoWE.TArmyModeElement.amMoving);
                        FArmyHS.FArmy.EndUpdate();
                        MovedArmyHS = ArmyHS;
                        FArmyHS = MovedArmyHS;
                        MovedArmyHS.FArmy.FMode = MovedArmyHS.FArmy.FMode + TSet<Aow.Units.AoWE.TArmyModeElement>.Of(Aow.Units.AoWE.TArmyModeElement.amMoving);
                        FSelection = 255;
                        if (TSet<Aow.Units.AoWE.TCombatResult>.Of(Aow.Units.AoWE.TCombatResult.crVictory, Aow.Units.AoWE.TCombatResult.crNoDefenders).Contains(CombatResult) || base.CanMoveNext())
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
                        FArmyHS.FArmy.FMode = FArmyHS.FArmy.FMode - TSet<Aow.Units.AoWE.TArmyModeElement>.Of(Aow.Units.AoWE.TArmyModeElement.amMoving);
                        FArmyHS.FArmy.EndUpdate();
                        FArmyHS = null;
                    }
                }
            }
            else
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.UnlockGameOverEvent();
                FArmyHS = null;
                FinishMove();
            }
        }
}
