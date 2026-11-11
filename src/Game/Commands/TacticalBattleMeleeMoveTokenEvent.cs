// Translated from CombatTE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

public partial class TacticalBattleMeleeMoveTokenEvent : CombatMoveTokenEvent
{
    public TacticalBattleMeleeMoveTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 2236417;

    public MeleeRound FMeleeRound;
    public int FStrikeIndex;

    // CombatTE.pas:454
    public override MoveStatus LastMove()
    {
        TacticalBattleUnitHexSprite HS;
        int X = default;
        int Y = default;
        MeleeRound.Strike Strike = default;
        MoveStatus Result = MoveStatus.Moving;
        try
        {
            switch (FStep)
            {
                case TokenStep.Start:
                    try
                    {
                        Result = MoveStatus.Moving;
                        FUnit.FUnitHS.CancelMove();
                        bool notExplorationEnabled = !CombatMap.Current.FExplorationEnabled;
                        if (!notExplorationEnabled)
                        {
                            var yhx = FUnit.FUnitHS.Yhx;
                            var xhx = FUnit.FUnitHS.Xhx;
                            notExplorationEnabled = ((AoWMapField)CombatMap.Current.MapContainer.GetField(xhx, yhx, 0)).GetExplored(CombatMap.Current.FSeatedPlayer);
                        }

                        if (notExplorationEnabled)
                        {
                            var yhx = FUnit.FUnitHS.Yhx;
                            var xhx = FUnit.FUnitHS.Xhx;
                            CombatMap.Current.MakeVisible(xhx, yhx, 0, 5);
                        }

                        var pathY = FPath[0];
                        var pathX = FPath[0];
                        HS = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(pathX.X, pathY.Y).FindOwnedChild(2228484);
                        if (HS == null)
                        {
                            FPreviousUnit = null;
                        }
                        else
                        {
                            FPreviousUnit = HS.FUnit;
                        }

                        if (FPreviousUnit == null)
                        {
                            Result = MoveStatus.Done;
                        }
                        else
                        {
                            FMeleeRound = new MeleeRound();
                            FMeleeRound.Calculate(FUnit, FPreviousUnit);
                            if (FMeleeRound.FCount == 0)
                            {
                                Result = MoveStatus.Done;
                            }
                            else
                            {
                                FUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepUp);
                                var yhx = FUnit.FUnitHS.Yhx;
                                var fUnitHSYhx = FPreviousUnit.FUnitHS.Yhx;
                                var xhx = FUnit.FUnitHS.Xhx;
                                var fUnitHSXhx = FPreviousUnit.FUnitHS.Xhx;
                                var meleeDirIndex = TacticalTables.GetMeleeDirIndex(FUnit.FUnitHS.Xhx, xhx - fUnitHSXhx, yhx - fUnitHSYhx);
                                FUnit.FUnitHS.SetDirection((byte)meleeDirIndex);
                                FPreviousUnit.FUnitHS.SetDirection((byte)(FUnit.FUnitHS.FDirection + 3));
                                if (FPreviousUnit.FUnitHS.FDirection > 6)
                                {
                                    FPreviousUnit.FUnitHS.SetDirection((byte)(FPreviousUnit.FUnitHS.FDirection - 6));
                                }
                            }
                        }

                        FStep = TokenStep.FirstStrike;
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token Start", "Error - TCMelMoveToken", 0);
                    }

                    break;
                case TokenStep.FirstStrike:
                    if (FUnit.FUnitHS.FActionDone)
                    {
                        ExecuteStrike();
                    }

                    break;
                case TokenStep.DelayStrike:
                    if (FDelay > 0)
                    {
                        FDelay = FDelay - 1;
                    }
                    else
                    {
                        ExecuteStrike();
                    }

                    break;
                case TokenStep.InitStrike:
                    try
                    {
                        FMeleeRound.GetMeleeStrike(FStrikeIndex - 1, ref Strike);
                        if (Strike.Side == CombatSideIndex.Defender && FPreviousUnit.FUnitHS.FActionDone)
                        {
                            if (FStrike.FDamage > 0)
                            {
                                FUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                TacticalBattleUnit.MakeHitBlood(FUnit, FStrike.FDamage);
                                var yhx = FUnit.FUnitHS.Yhx;
                                HexGeometry.HXtoHP(FUnit.FUnitHS.Xhx, yhx, ref X, ref Y);
                                if (FStrike.FDamageTypes.Value - FStrike.FEffects.Value != Set<DamageType>.Of())
                                {
                                    CombatEvents.MakeDEHSList(X + FUnit.FUnitHS.FOffsetX, Y + FUnit.FUnitHS.FOffsetY, FStrike.FDamageTypes.Value - FStrike.FEffects.Value, false);
                                }

                                if (FStrike.FEffects.Value != Set<DamageType>.Of())
                                {
                                    CombatEvents.MakeDEHSList(X + FUnit.FUnitHS.FOffsetX, Y + FUnit.FUnitHS.FOffsetY, FStrike.FEffects.Value, true);
                                }
                            }

                            FStep = TokenStep.ExeStrike;
                        }
                        else
                        {
                            FMeleeRound.GetMeleeStrike(FStrikeIndex - 1, ref Strike);
                            if (Strike.Side == CombatSideIndex.Attacker && FUnit.FUnitHS.FActionDone)
                            {
                                if (FStrike.FDamage > 0)
                                {
                                    FPreviousUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                    TacticalBattleUnit.MakeHitBlood(FPreviousUnit, FStrike.FDamage);
                                    var yhx = FPreviousUnit.FUnitHS.Yhx;
                                    HexGeometry.HXtoHP(FPreviousUnit.FUnitHS.Xhx, yhx, ref X, ref Y);
                                    if (FStrike.FDamageTypes.Value - FStrike.FEffects.Value != Set<DamageType>.Of())
                                    {
                                        CombatEvents.MakeDEHSList(X, Y, FStrike.FDamageTypes.Value - FStrike.FEffects.Value, false);
                                    }

                                    if (FStrike.FEffects.Value != Set<DamageType>.Of())
                                    {
                                        CombatEvents.MakeDEHSList(X, Y, FStrike.FEffects.Value, true);
                                    }
                                }

                                FStep = TokenStep.ExeStrike;
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token InitStrike", "Error - TCMelMoveToken", 0);
                    }

                    break;
                case TokenStep.ExeStrike:
                    try
                    {
                        if (FStrike.FDamage == 0)
                        {
                            NextStrike();
                        }
                        else
                        {
                            FMeleeRound.GetMeleeStrike(FStrikeIndex - 1, ref Strike);
                            if (Strike.Side == CombatSideIndex.Attacker && FPreviousUnit.FUnitHS.FActionDone)
                            {
                                CombatMap.Current.FCombat.ExecuteCombatAction(FStrike);
                                if (!FPreviousUnit.GetEnabled())
                                {
                                    FStep = TokenStep.End;
                                    FUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepBack);
                                }
                                else
                                {
                                    NextStrike();
                                }
                            }
                            else
                            {
                                if (FUnit.FUnitHS.FActionDone)
                                {
                                    CombatMap.Current.FCombat.ExecuteCombatAction(FStrike);
                                    if (FUnit.GetEnabled())
                                    {
                                        NextStrike();
                                    }
                                    else
                                    {
                                        Result = MoveStatus.Done;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token ExeStrike", "Error - TCMelMoveToken", 0);
                    }

                    break;
                case TokenStep.End:
                    if (FUnit.FUnitHS.FActionDone)
                    {
                        Result = MoveStatus.Done;
                    }
                    else
                    {
                        Result = MoveStatus.Moving;
                    }

                    break;
            }

            if (Result == MoveStatus.Done)
            {
                if (FMeleeRound != null)
                {
                    FMeleeRound?.Dispose();
                }

                if (FStrike != null)
                {
                    FStrike.Release();
                }

                if (FUnit.GetEnabled())
                {
                    FUnit.FMovePoints = 0;
                    FUnit.FUnitHS.Unselect();
                    if (FUnit.FCombatPlayer == CombatMap.Current.FCurrentCombatPlayer)
                    {
                        var xhx = FUnit.FUnitHS.Xhx;
                        CombatMap.Current.FPlayers.GetPlayers(CombatMap.Current.FCurrentCombatPlayer).FLastX = xhx;
                        var yhx = FUnit.FUnitHS.Yhx;
                        CombatMap.Current.FPlayers.GetPlayers(CombatMap.Current.FCurrentCombatPlayer).FLastY = yhx;
                    }
                }
            }
        }
        catch
        {
            MessageBox(0, "Error during Token", "Error - TCMelMoveToken", 0);
        }

        return Result;
    }

    // CombatTE.pas:577
    public void NextStrike()
    {
        bool Locked;
        MeleeRound.Strike Strike = default;
        Locked = true;
        while (Locked && FStrikeIndex < FMeleeRound.FCount)
        {
            FMeleeRound.GetMeleeStrike(FStrikeIndex, ref Strike);
            if (Strike.Side == CombatSideIndex.Attacker)
            {
                Locked = FUnit.GetLocked();
            }
            else
            {
                Locked = FPreviousUnit.GetLocked();
            }

            if (Locked)
            {
                FStrikeIndex = FStrikeIndex + 1;
            }
        }

        if (FStrikeIndex < FMeleeRound.FCount)
        {
            FStep = TokenStep.DelayStrike;
            FDelay = 3;
        }
        else
        {
            FStep = TokenStep.End;
            FUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepBack);
        }
    }

    // CombatTE.pas:602
    public void ExecuteStrike()
    {
        MeleeRound.Strike Strike = default;
        if (FStrike != null)
        {
            FStrike.Release();
        }

        RandSeed = AoWHSMap.Current.Random(16777215);
        FStrike = FMeleeRound.CreateStrikeCA(FStrikeIndex);
        FMeleeRound.GetMeleeStrike(FStrikeIndex, ref Strike);
        if (Strike.Side == CombatSideIndex.Attacker)
        {
            if (FStrike.FDamage == 0)
            {
                FUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.MeleeMiss);
            }
            else
            {
                FUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.MeleeHit);
            }
        }
        else
        {
            if (FStrike.FDamage == 0)
            {
                FPreviousUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.MeleeMiss);
            }
            else
            {
                FPreviousUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.MeleeHit);
            }
        }

        FStrikeIndex = FStrikeIndex + 1;
        FStep = TokenStep.InitStrike;
    }

    // intf_P5_07.inc:765

}
