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

    public new const int ClassId = 2236417;

    private MeleeRound _meleeRound;
    private int _strikeIndex;

    // CombatTE.pas:454
    public override MoveStatus LastMove()
    {
        TacticalBattleUnitHexSprite hexSprite;
        int X = default;
        int Y = default;
        MeleeRound.Strike strike = default;
        MoveStatus result = MoveStatus.Moving;
        try
        {
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
                            var hexY = Unit.UnitHexSprite.HexY;
                            var hexX = Unit.UnitHexSprite.HexX;
                            notExplorationEnabled = CombatMap.Current.MapContainer.GetField(hexX, hexY, 0).GetExplored(CombatMap.Current.SeatedPlayer);
                        }

                        if (notExplorationEnabled)
                        {
                            var hexY = Unit.UnitHexSprite.HexY;
                            var hexX = Unit.UnitHexSprite.HexX;
                            CombatMap.Current.MakeVisible(hexX, hexY, 0, 5);
                        }

                        var pathY = Path[0];
                        var pathX = Path[0];
                        hexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(pathX.X, pathY.Y).FindOwnedChild(2228484);
                        if (hexSprite == null)
                        {
                            PreviousUnit = null;
                        }
                        else
                        {
                            PreviousUnit = hexSprite.Unit;
                        }

                        if (PreviousUnit == null)
                        {
                            result = MoveStatus.Done;
                        }
                        else
                        {
                            _meleeRound = new MeleeRound(this);
                            _meleeRound.Calculate(Unit, PreviousUnit);
                            if (_meleeRound.Count == 0)
                            {
                                result = MoveStatus.Done;
                            }
                            else
                            {
                                Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepUp);
                                var hexY = Unit.UnitHexSprite.HexY;
                                var fUnitHexSpriteHexY = PreviousUnit.UnitHexSprite.HexY;
                                var hexX = Unit.UnitHexSprite.HexX;
                                var fUnitHexSpriteHexX = PreviousUnit.UnitHexSprite.HexX;
                                var meleeDirIndex = TacticalTables.GetMeleeDirectionIndex(Unit.UnitHexSprite.HexX, hexX - fUnitHexSpriteHexX, hexY - fUnitHexSpriteHexY);
                                Unit.UnitHexSprite.SetDirection((byte)meleeDirIndex);
                                PreviousUnit.UnitHexSprite.SetDirection((byte)(Unit.UnitHexSprite.Direction + 3));
                                if (PreviousUnit.UnitHexSprite.Direction > 6)
                                {
                                    PreviousUnit.UnitHexSprite.SetDirection((byte)(PreviousUnit.UnitHexSprite.Direction - 6));
                                }
                            }
                        }

                        Step = TokenStep.FirstStrike;
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token Start", "Error - TCMelMoveToken", 0);
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
                        Delay = Delay - 1;
                    }
                    else
                    {
                        ExecuteStrike();
                    }

                    break;
                case TokenStep.InitializeStrike:
                    try
                    {
                        _meleeRound.GetMeleeStrike(_strikeIndex - 1, out strike);
                        if (strike.Side == CombatSideIndex.Defender && PreviousUnit.UnitHexSprite.ActionDone)
                        {
                            if (Strike.Damage > 0)
                            {
                                Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                TacticalBattleUnit.MakeHitBlood(Unit, Strike.Damage);
                                var hexY = Unit.UnitHexSprite.HexY;
                                HexGeometry.HexToPixel(Unit.UnitHexSprite.HexX, hexY, out X, out Y);
                                if (Strike.DamageTypes.Value - Strike.Effects.Value != Set<DamageType>.Of())
                                {
                                    CombatEvents.MakeDamageEffectHexSpriteList(X + Unit.UnitHexSprite.OffsetX, Y + Unit.UnitHexSprite.OffsetY, Strike.DamageTypes.Value - Strike.Effects.Value, false);
                                }

                                if (Strike.Effects.Value != Set<DamageType>.Of())
                                {
                                    CombatEvents.MakeDamageEffectHexSpriteList(X + Unit.UnitHexSprite.OffsetX, Y + Unit.UnitHexSprite.OffsetY, Strike.Effects.Value, true);
                                }
                            }

                            Step = TokenStep.ExecuteStrike;
                        }
                        else
                        {
                            _meleeRound.GetMeleeStrike(_strikeIndex - 1, out strike);
                            if (strike.Side == CombatSideIndex.Attacker && Unit.UnitHexSprite.ActionDone)
                            {
                                if (Strike.Damage > 0)
                                {
                                    PreviousUnit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                    TacticalBattleUnit.MakeHitBlood(PreviousUnit, Strike.Damage);
                                    var hexY = PreviousUnit.UnitHexSprite.HexY;
                                    HexGeometry.HexToPixel(PreviousUnit.UnitHexSprite.HexX, hexY, out X, out Y);
                                    if (Strike.DamageTypes.Value - Strike.Effects.Value != Set<DamageType>.Of())
                                    {
                                        CombatEvents.MakeDamageEffectHexSpriteList(X, Y, Strike.DamageTypes.Value - Strike.Effects.Value, false);
                                    }

                                    if (Strike.Effects.Value != Set<DamageType>.Of())
                                    {
                                        CombatEvents.MakeDamageEffectHexSpriteList(X, Y, Strike.Effects.Value, true);
                                    }
                                }

                                Step = TokenStep.ExecuteStrike;
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token InitStrike", "Error - TCMelMoveToken", 0);
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
                            _meleeRound.GetMeleeStrike(_strikeIndex - 1, out strike);
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
                        MessageBox(0, "Error during Token ExeStrike", "Error - TCMelMoveToken", 0);
                    }

                    break;
                case TokenStep.End:
                    if (Unit.UnitHexSprite.ActionDone)
                    {
                        result = MoveStatus.Done;
                    }
                    else
                    {
                        result = MoveStatus.Moving;
                    }

                    break;
            }

            if (result == MoveStatus.Done)
            {
                if (_meleeRound != null)
                {
                    _meleeRound?.Dispose();
                }

                if (Strike != null)
                {
                    Strike.Release();
                }

                if (Unit.GetEnabled())
                {
                    Unit.MovePoints = 0;
                    Unit.UnitHexSprite.Unselect();
                    if (Unit.CombatPlayer == CombatMap.Current.CurrentCombatPlayer)
                    {
                        var hexX = Unit.UnitHexSprite.HexX;
                        CombatMap.Current.Players.GetPlayers(CombatMap.Current.CurrentCombatPlayer).LastX = hexX;
                        var hexY = Unit.UnitHexSprite.HexY;
                        CombatMap.Current.Players.GetPlayers(CombatMap.Current.CurrentCombatPlayer).LastY = hexY;
                    }
                }
            }
        }
        catch
        {
            MessageBox(0, "Error during Token", "Error - TCMelMoveToken", 0);
        }

        return result;
    }

    // CombatTE.pas:577
    public void NextStrike()
    {
        bool locked;
        MeleeRound.Strike strike = default;
        locked = true;
        while (locked && _strikeIndex < _meleeRound.Count)
        {
            _meleeRound.GetMeleeStrike(_strikeIndex, out strike);
            if (strike.Side == CombatSideIndex.Attacker)
            {
                locked = Unit.GetLocked();
            }
            else
            {
                locked = PreviousUnit.GetLocked();
            }

            if (locked)
            {
                _strikeIndex = _strikeIndex + 1;
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
        MeleeRound.Strike strike = default;
        if (Strike != null)
        {
            Strike.Release();
        }

        RandSeed = StrategicMap.Current.Random(16777215);
        Strike = _meleeRound.CreateStrikeCombatAction(_strikeIndex);
        _meleeRound.GetMeleeStrike(_strikeIndex, out strike);
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

        _strikeIndex = _strikeIndex + 1;
        Step = TokenStep.InitializeStrike;
    }

    // intf_P5_07.inc:765

}
