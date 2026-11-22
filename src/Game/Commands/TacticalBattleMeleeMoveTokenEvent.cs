#nullable disable warnings
// Translated from CombatTE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.Commands;

public class TacticalBattleMeleeMoveTokenEvent(CustomNode? owner) : CombatMoveTokenEvent(owner)
{
    public new const int ClassId = 2_236_417;

    private MeleeRound _meleeRound;
    private int _strikeIndex;

    // CombatTE.pas:454
    public override MoveStatus LastMove()
    {
        MoveStatus result = MoveStatus.Moving;
        try
        {
            MeleeRound.Strike strike;
            switch (Step)
            {
                case TokenStep.Start:
                    try
                    {
                        result = MoveStatus.Moving;
                        Unit.UnitHexSprite.CancelMove();
                        bool notExplorationEnabled = !CombatMap.Current.ExplorationEnabled;
                        if (!notExplorationEnabled)
                        {
                            int hexY = Unit.UnitHexSprite.HexY;
                            int hexX = Unit.UnitHexSprite.HexX;
                            notExplorationEnabled = CombatMap.Current.MapContainer.GetField(hexX, hexY, 0).GetExplored(CombatMap.Current.SeatedPlayer);
                        }

                        if (notExplorationEnabled)
                        {
                            int hexY = Unit.UnitHexSprite.HexY;
                            int hexX = Unit.UnitHexSprite.HexX;
                            CombatMap.Current.MakeVisible(hexX, hexY, 0, radius: 5);
                        }

                        PathStep pathY = Path[0];
                        PathStep pathX = Path[0];
                        TacticalBattleUnitHexSprite hexSprite = CombatMap.Current.MapContainer.MapLevels[0].GetField(pathX.Location.X, pathY.Location.Y).FindOwnedChild<TacticalBattleUnitHexSprite>(TacticalBattleUnitHexSprite.ClassId);
                        PreviousUnit = hexSprite?.Unit;

                        if (PreviousUnit is null)
                        {
                            result = MoveStatus.Done;
                        }
                        else
                        {
                            _meleeRound = new MeleeRound(this);
                            _meleeRound.Calculate(Unit, defender: PreviousUnit);
                            if (_meleeRound.Count == 0)
                            {
                                result = MoveStatus.Done;
                            }
                            else
                            {
                                Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepUp);
                                int hexY = Unit.UnitHexSprite.HexY;
                                int fUnitHexSpriteHexY = PreviousUnit.UnitHexSprite.HexY;
                                int hexX = Unit.UnitHexSprite.HexX;
                                int fUnitHexSpriteHexX = PreviousUnit.UnitHexSprite.HexX;
                                int meleeDirIndex = TacticalTables.GetMeleeDirectionIndex(Unit.UnitHexSprite.HexX, hexX - fUnitHexSpriteHexX, hexY - fUnitHexSpriteHexY);
                                Unit.UnitHexSprite.SetDirection(meleeDirIndex.TruncateToByte());
                                PreviousUnit.UnitHexSprite.SetDirection((Unit.UnitHexSprite.Direction + 3).TruncateToByte());
                                if (PreviousUnit.UnitHexSprite.Direction > 6)
                                {
                                    PreviousUnit.UnitHexSprite.SetDirection((PreviousUnit.UnitHexSprite.Direction - 6).TruncateToByte());
                                }
                            }
                        }

                        Step = TokenStep.FirstStrike;
                    }
                    catch
                    {
                        _ = MessageBox(0, text: "Error during Token Start", caption: "Error - TCMelMoveToken", flags: 0);
                    }

                    break;
                case TokenStep.FirstStrike:
                    if (Unit.UnitHexSprite.ActionDone)
                    {
                        ExecuteStrike();
                    }

                    break;
                case TokenStep.DelayStrike:
                    if (Delay > 0)
                    {
                        Delay--;
                    }
                    else
                    {
                        ExecuteStrike();
                    }

                    break;
                case TokenStep.InitializeStrike:
                    try
                    {
                        strike = _meleeRound.GetMeleeStrike(_strikeIndex - 1);
                        int x;
                        int y;
                        if (strike.Side == CombatSideIndex.Defender && PreviousUnit.UnitHexSprite.ActionDone)
                        {
                            if (Strike.Damage > 0)
                            {
                                Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                TacticalBattleUnit.MakeHitBlood(Unit, Strike.Damage);
                                int hexY = Unit.UnitHexSprite.HexY;
                                HexGeometry.HexToPixel(Unit.UnitHexSprite.HexX, hexY, out x, out y);
                                if ((Strike.DamageTypes & ~Strike.Effects) != DamageTypes.None)
                                {
                                    CombatEvents.MakeDamageEffectHexSpriteList(x + Unit.UnitHexSprite.OffsetX, y + Unit.UnitHexSprite.OffsetY, damageTypes: Strike.DamageTypes & ~Strike.Effects, resisted: false);
                                }

                                if (Strike.Effects != DamageTypes.None)
                                {
                                    CombatEvents.MakeDamageEffectHexSpriteList(x + Unit.UnitHexSprite.OffsetX, y + Unit.UnitHexSprite.OffsetY, damageTypes: Strike.Effects, resisted: true);
                                }
                            }

                            Step = TokenStep.ExecuteStrike;
                        }
                        else
                        {
                            strike = _meleeRound.GetMeleeStrike(_strikeIndex - 1);
                            if (strike.Side == CombatSideIndex.Attacker && Unit.UnitHexSprite.ActionDone)
                            {
                                if (Strike.Damage > 0)
                                {
                                    PreviousUnit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                    TacticalBattleUnit.MakeHitBlood(PreviousUnit, Strike.Damage);
                                    int hexY = PreviousUnit.UnitHexSprite.HexY;
                                    HexGeometry.HexToPixel(PreviousUnit.UnitHexSprite.HexX, hexY, out x, out y);
                                    if ((Strike.DamageTypes & ~Strike.Effects) != DamageTypes.None)
                                    {
                                        CombatEvents.MakeDamageEffectHexSpriteList(x, y, damageTypes: Strike.DamageTypes & ~Strike.Effects, resisted: false);
                                    }

                                    if (Strike.Effects != DamageTypes.None)
                                    {
                                        CombatEvents.MakeDamageEffectHexSpriteList(x, y, damageTypes: Strike.Effects, resisted: true);
                                    }
                                }

                                Step = TokenStep.ExecuteStrike;
                            }
                        }
                    }
                    catch
                    {
                        _ = MessageBox(0, text: "Error during Token InitStrike", caption: "Error - TCMelMoveToken", flags: 0);
                    }

                    break;
                case TokenStep.ExecuteStrike:
                    try
                    {
                        if (Strike.Damage == 0)
                        {
                            NextStrike();
                        }
                        else
                        {
                            strike = _meleeRound.GetMeleeStrike(_strikeIndex - 1);
                            if (strike.Side == CombatSideIndex.Attacker && PreviousUnit.UnitHexSprite.ActionDone)
                            {
                                CombatMap.Current.Combat.ExecuteCombatAction(Strike);
                                if (!PreviousUnit.GetEnabled())
                                {
                                    Step = TokenStep.End;
                                    Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepBack);
                                }
                                else
                                {
                                    NextStrike();
                                }
                            }
                            else
                            {
                                if (Unit.UnitHexSprite.ActionDone)
                                {
                                    CombatMap.Current.Combat.ExecuteCombatAction(Strike);
                                    if (Unit.GetEnabled())
                                    {
                                        NextStrike();
                                    }
                                    else
                                    {
                                        result = MoveStatus.Done;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        _ = MessageBox(0, text: "Error during Token ExeStrike", caption: "Error - TCMelMoveToken", flags: 0);
                    }

                    break;
                case TokenStep.End:
                    result = Unit.UnitHexSprite.ActionDone ? MoveStatus.Done : MoveStatus.Moving;

                    break;
                case TokenStep.EndStrike:
                case TokenStep.ExecuteEndStrike:
                case TokenStep.StepUp:
                default:
                    break;
            }

            if (result == MoveStatus.Done)
            {
                _meleeRound?.Dispose();

                _ = (Strike?.Release());

                if (Unit.GetEnabled())
                {
                    Unit.MovePoints = 0;
                    Unit.UnitHexSprite.Unselect();
                    if (Unit.CombatPlayer == CombatMap.Current.CurrentCombatPlayer)
                    {
                        int hexX = Unit.UnitHexSprite.HexX;
                        CombatMap.Current.Players.GetPlayers(CombatMap.Current.CurrentCombatPlayer).LastX = hexX;
                        int hexY = Unit.UnitHexSprite.HexY;
                        CombatMap.Current.Players.GetPlayers(CombatMap.Current.CurrentCombatPlayer).LastY = hexY;
                    }
                }
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Error during Token", caption: "Error - TCMelMoveToken", flags: 0);
        }

        return result;
    }

    // CombatTE.pas:577
    public void NextStrike()
    {
        bool locked = true;
        while (locked && _strikeIndex < _meleeRound.Count)
        {
            MeleeRound.Strike strike = _meleeRound.GetMeleeStrike(_strikeIndex);
            locked = strike.Side == CombatSideIndex.Attacker ? Unit.GetLocked() : PreviousUnit.GetLocked();

            if (locked)
            {
                _strikeIndex++;
            }
        }

        if (_strikeIndex < _meleeRound.Count)
        {
            Step = TokenStep.DelayStrike;
            Delay = 3;
        }
        else
        {
            Step = TokenStep.End;
            Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepBack);
        }
    }

    // CombatTE.pas:602
    public void ExecuteStrike()
    {
        _ = (Strike?.Release());

        RandSeed = StrategicMap.Current.Random(16777215);
        Strike = _meleeRound.CreateStrikeCombatAction(_strikeIndex);
        MeleeRound.Strike strike = _meleeRound.GetMeleeStrike(_strikeIndex);
        if (strike.Side == CombatSideIndex.Attacker)
        {
            if (Strike.Damage == 0)
            {
                Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.MeleeMiss);
            }
            else
            {
                Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.MeleeHit);
            }
        }
        else
        {
            if (Strike.Damage == 0)
            {
                PreviousUnit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.MeleeMiss);
            }
            else
            {
                PreviousUnit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.MeleeHit);
            }
        }

        _strikeIndex++;
        Step = TokenStep.InitializeStrike;
    }
}
