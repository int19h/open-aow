#nullable disable
// Translated from CombatTE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Draw;

using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.Commands;

public class CombatMoveTokenEvent(CustomNode owner) : MoveTokenEvent(owner)
{

    // CombatTE.pas:206
    public new const int ClassId = 2236416;

    public TacticalBattleUnit Unit { get; private set; }
    public int Delay { get; protected set; }

    private List<TacticalBattleUnit> _objects;

    public TacticalBattleUnit PreviousUnit { get; protected set; }

    private TacticalBattleUnit _freeStrikeUnit;
    private sbyte _neighbour;

    public TokenStep Step { get; protected set; }
    public StrikeCombatAction Strike { get; protected set; }

    // CombatTE.pas:214
    public override void Dispose()
    {
        _objects?.Clear();

        base.Dispose();
    }

    // CombatTE.pas:209
    public override MapField GetMapField(MapCoordinate location) => CombatMap.Current.MapContainer.GetField(location.X, location.Y, 0);

    // CombatTE.pas:234
    public override bool Validate()
    {
        _ = base.Validate();
        int index = Path.Count - 1;
        MapField mapField = GetMapField(Path[index].Location);
        TacticalBattleUnitHexSprite hexSprite = mapField.FindOwnedChild<TacticalBattleUnitHexSprite>(2228484);
        if (hexSprite is not null)
        {
            _objects = [];
            Unit = hexSprite.Unit;
            return true;
        }
        else
        {
            return false;
        }
    }

    // CombatTE.pas:299
    public override MoveStatus DefaultMove()
    {
        MapField mapField;
        void CheckFreeStrike()
        {
            if (Unit.GetEnabled() && _freeStrikeUnit is null)
            {
                mapField = Unit.UnitHexSprite.Owner;
                int hexNumber = _neighbour + 1;
                int hexX = default;
                int hexY = default;
                while (hexNumber <= 6)
                {
                    sbyte yhx = Unit.UnitHexSprite.HexY;
                    HexGeometry.CenterHexNumberToHex(Unit.UnitHexSprite.HexX, yhx, hexNumber, ref hexX, ref hexY);
                    TacticalBattleUnitHexSprite hexSprite = CombatMap.Current.MapContainer.MapLevels[0].GetField(hexX, hexY).FindOwnedChild<TacticalBattleUnitHexSprite>(2228484);
                    if (hexSprite is not null && PreviousUnit != hexSprite.Unit && hexSprite.Unit.GetEnabled() && !hexSprite.Unit.GetLocked() && CombatObject.MeleeDamageTypes(hexSprite.Unit) != Set<DamageType>.Of() && hexSprite.Unit.GetSide() != Unit.GetSide() && hexSprite.Action == TacticalBattleUnitHexSprite.SpriteAction.Move && hexSprite.Unit.GetAbilityEnabled(1) == Unit.GetAbilityEnabled(1))
                    {
                        _neighbour = (sbyte)hexNumber;
                        _freeStrikeUnit = hexSprite.Unit;
                        hexX = 0;
                        int last = _objects.Count - 1;
                        for (; hexX <= last; hexX += 1)
                        {
                            if (_objects[hexX] == _freeStrikeUnit)
                            {
                                _freeStrikeUnit = null;
                            }
                        }

                        if (_freeStrikeUnit is not null)
                        {
                            hexNumber = 6;
                            Step = TokenStep.Start;
                        }
                    }

                    hexNumber++;
                }

                if (_freeStrikeUnit is null)
                {
                    _neighbour = 0;
                }
                else
                {
                    _objects.Add(_freeStrikeUnit);
                }
            }
        }

        MoveStatus result = MoveStatus.Moving;
        try
        {
            if (_freeStrikeUnit is not null)
            {
                result = MoveStatus.Moving;
                switch (Step)
                {
                    case TokenStep.Start:
                        try
                        {
                            Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Busy);
                            _freeStrikeUnit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepUp);
                            sbyte hexY = _freeStrikeUnit.UnitHexSprite.HexY;
                            sbyte fUnitHexSpriteHexY = Unit.UnitHexSprite.HexY;
                            sbyte hexX = _freeStrikeUnit.UnitHexSprite.HexX;
                            sbyte fUnitHexSpriteHexX = Unit.UnitHexSprite.HexX;
                            ushort meleeDirIndex = TacticalTables.GetMeleeDirectionIndex(_freeStrikeUnit.UnitHexSprite.HexX, hexX - fUnitHexSpriteHexX, hexY - fUnitHexSpriteHexY);
                            _freeStrikeUnit.UnitHexSprite.SetDirection((byte)meleeDirIndex);
                            Step = TokenStep.InitializeStrike;
                        }
                        catch
                        {
                            _ = MessageBox(0, "Error during Token Start", "Error - TCMoveToken", 0);
                        }

                        break;
                    case TokenStep.InitializeStrike:
                        try
                        {
                            if (_freeStrikeUnit.UnitHexSprite.ActionDone)
                            {
                                RandSeed = StrategicMap.Current.Random(16777215);
                                Strike = CombatObject.CreateStrikeCombatAction(_freeStrikeUnit, Unit, true);
                                if (Strike.Damage == 0)
                                {
                                    _freeStrikeUnit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.MeleeMiss);
                                }
                                else
                                {
                                    _freeStrikeUnit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.MeleeHit);
                                }

                                Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Busy);
                                Step = TokenStep.ExecuteStrike;
                            }
                        }
                        catch
                        {
                            _ = MessageBox(0, "Error during Token InitStrike", "Error - TCMoveToken", 0);
                        }

                        break;
                    case TokenStep.ExecuteStrike:
                        try
                        {
                            if (_freeStrikeUnit.UnitHexSprite.ActionDone)
                            {
                                if (Strike.Damage > 0)
                                {
                                    Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                    TacticalBattleUnit.MakeHitBlood(Unit, Strike.Damage);
                                }

                                sbyte hexY = Unit.UnitHexSprite.HexY;
                                HexGeometry.HexToPixel(Unit.UnitHexSprite.HexX, hexY, out int x, out int y);
                                if (Strike.DamageTypes - Strike.Effects != Set<DamageType>.Of())
                                {
                                    CombatEvents.MakeDamageEffectHexSpriteList(x, y, Strike.DamageTypes - Strike.Effects, false);
                                }

                                if (Strike.Effects != Set<DamageType>.Of())
                                {
                                    CombatEvents.MakeDamageEffectHexSpriteList(x, y, Strike.Effects, true);
                                }

                                Step = TokenStep.EndStrike;
                            }
                        }
                        catch
                        {
                            _ = MessageBox(0, "Error during Token ExeStrike", "Error - TCMoveToken", 0);
                        }

                        break;
                    case TokenStep.EndStrike:
                        try
                        {
                            if (Unit.UnitHexSprite.ActionDone || Strike.Damage == 0)
                            {
                                if (Strike.Damage > 0)
                                {
                                    CombatMap.Current.Combat.ExecuteCombatAction(Strike);
                                }

                                Step = TokenStep.End;
                                _freeStrikeUnit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepBack);
                            }
                        }
                        catch
                        {
                            _ = MessageBox(0, "Error during Token EndStrike", "Error - TCMoveToken", 0);
                        }

                        break;
                    case TokenStep.End:
                        try
                        {
                            if (_freeStrikeUnit.UnitHexSprite.ActionDone)
                            {
                                if (!Unit.GetEnabled())
                                {
                                    result = MoveStatus.Done;
                                }
                                else
                                {
                                    if (Unit.GetLocked())
                                    {
                                        Unit.UnitHexSprite.CancelMove();
                                        result = MoveStatus.Done;
                                    }
                                    else
                                    {
                                        Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Move);
                                    }
                                }

                                _ = (Strike?.Release());
                                Strike = null;

                                _freeStrikeUnit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Move);
                                Step = TokenStep.Start;
                                _freeStrikeUnit = null;
                                CheckFreeStrike();
                            }
                        }
                        catch
                        {
                            _ = MessageBox(0, "Error during Token End", "Error - TCMoveToken", 0);
                        }

                        break;
                    case TokenStep.FirstStrike:
                    case TokenStep.ExecuteEndStrike:
                    case TokenStep.StepUp:
                    case TokenStep.DelayStrike:
                    default:
                        break;
                }
            }
            else
            {
                if (Unit.GetEnabled())
                {
                    if (Unit.UnitHexSprite.Moving())
                    {
                        if (Unit.UnitHexSprite.MoveNext() == 100 || !Unit.UnitHexSprite.GetVisible())
                        {
                            result = ExecuteDefaultMove();
                            if (result == MoveStatus.Moving)
                            {
                                CheckFreeStrike();
                            }
                        }
                        else
                        {
                            result = MoveStatus.Moving;
                        }
                    }
                    else
                    {
                        if (MoveIndex != 0)
                        {
                            bool notExplorationEnabled = !CombatMap.Current.ExplorationEnabled;
                            if (!notExplorationEnabled)
                            {
                                sbyte hexY = Unit.UnitHexSprite.HexY;
                                sbyte hexX = Unit.UnitHexSprite.HexX;
                                notExplorationEnabled = CombatMap.Current.MapContainer.GetField(hexX, hexY, 0).GetExplored(CombatMap.Current.SeatedPlayer);
                            }

                            if (notExplorationEnabled)
                            {
                                sbyte hexY = Unit.UnitHexSprite.HexY;
                                sbyte hexX = Unit.UnitHexSprite.HexX;
                                CombatMap.Current.MakeVisible(hexX, hexY, 0, 3);
                            }

                            int direction = GetDirection(MoveIndex);
                            mapField = GetMapField(Path[MoveIndex - 1].Location);
                            if (mapField.GetPlayer() == -1)
                            {
                                if (direction != 0)
                                {
                                    result = Unit.UnitHexSprite.MoveFirst(direction, false) == -1 ? MoveStatus.Failed : MoveStatus.Moving;

                                    if (result == MoveStatus.Moving)
                                    {
                                        CheckFreeStrike();
                                    }
                                }
                                else
                                {
                                    result = ExecuteDefaultMove();
                                }
                            }
                            else
                            {
                                result = MoveStatus.Done;
                            }
                        }
                        else
                        {
                            result = MoveStatus.Done;
                        }
                    }
                }
            }

            if (result == MoveStatus.Done)
            {
                if (Unit.GetEnabled() && Unit.CombatPlayer == CombatMap.Current.CurrentCombatPlayer)
                {
                    sbyte hexX = Unit.UnitHexSprite.HexX;
                    CombatMap.Current.Players.GetPlayers(CombatMap.Current.CurrentCombatPlayer).LastX = hexX;
                    sbyte hexY = Unit.UnitHexSprite.HexY;
                    CombatMap.Current.Players.GetPlayers(CombatMap.Current.CurrentCombatPlayer).LastY = hexY;
                }
            }
        }
        catch
        {
            _ = MessageBox(0, "Error during Token", "Error - TCMoveToken", 0);
        }

        return result;
    }

    // CombatTE.pas:220
    public virtual bool Setup(TacticalBattleUnit unitObject, MovePath path)
    {
        Path.Clear();
        int startIndex = path.MovePointsToPosition(unitObject.MovePoints);
        if (startIndex != path.Count - 1)
        {
            for (int i = startIndex, last = path.Count - 1; i <= last; i++)
            {
                PathStep pathLocal = path[i];
                _ = Path.Add(pathLocal);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    // CombatTE.pas:253
    public MoveStatus ExecuteDefaultMove()
    {
        _ = Move();
        MapField oldField = Unit.UnitHexSprite.Owner;
        MapField newField = GetMapField(Path[MoveIndex].Location);
        _ = Unit.UnitHexSprite.MoveTo(newField, 0);
        MoveStatus result;
        if (newField.FindChildIndex(Unit.UnitHexSprite) != -1)
        {
            result = ProcessMove();
            PathStep path = Path[MoveIndex];
            Unit.MovedTo(oldField, newField, path.MoveCost);
            Rect rectangle = new(oldField.HexX, oldField.HexY, oldField.HexX + 1, oldField.HexY + 1);
            CombatMap.Current.TriggerMapChangedEvent(rectangle, 0);
            if (MoveIndex == 0 && newField.FindChild(2229544) is not null && Unit.CombatPlayer != CombatMap.Current.Combat.CombatData.DefenderPlayer)
            {
                Unit.SetStatus(Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Fled));
                sbyte hexY = Unit.UnitHexSprite.HexY;
                sbyte hexX = Unit.UnitHexSprite.HexX;
                sbyte fUnitHexSpriteHexY = Unit.UnitHexSprite.HexY;
                Rect r = new(Unit.UnitHexSprite.HexX, fUnitHexSpriteHexY, hexX + 1, hexY + 1);
                Unit.UnitHexSprite?.Dispose();
                CombatMap.Current.TriggerMapChangedEvent(r, 0);
                Unit.UnitHexSprite = null;
                return MoveStatus.Done;
            }
            else
            {
                if (CombatMap.Current.ExplorationEnabled)
                {
                    sbyte player = Unit.GetPlayer();
                    int visibilityRange = Unit.GetVisibilityRange();
                    sbyte hexY = Unit.UnitHexSprite.HexY;
                    sbyte hexX = Unit.UnitHexSprite.HexX;
                    _ = CombatMap.Current.ExploreArea(hexX, hexY, 0, player, visibilityRange);
                    int fUnitVisibilityRange = Unit.GetVisibilityRange();
                    sbyte fUnitHexSpriteHexY = Unit.UnitHexSprite.HexY;
                    sbyte fUnitHexSpriteHexX = Unit.UnitHexSprite.HexX;
                    CombatMap.Current.MapChangedRadius(fUnitHexSpriteHexX, fUnitHexSpriteHexY, 0, fUnitVisibilityRange + 1);
                }
                else
                {
                    sbyte hexY = Unit.UnitHexSprite.HexY;
                    sbyte hexX = Unit.UnitHexSprite.HexX;
                    sbyte fUnitHexSpriteHexY = Unit.UnitHexSprite.HexY;
                    Rect rectangle2 = new(Unit.UnitHexSprite.HexX, fUnitHexSpriteHexY, hexX + 1, hexY + 1);
                    CombatMap.Current.TriggerMapChangedEvent(rectangle2, 0);
                }

                sbyte fUnitPlayer = Unit.GetPlayer();
                if (StrategicMap.Current.Players.GetPlayers(fUnitPlayer).PlayerType != PlayerType.Human || CombatMap.Current.Players.GetPlayers(Unit.CombatPlayer).Auto)
                {
                    Unit.MovePoints = 0;
                }
            }
        }
        else
        {
            Unit.UnitHexSprite = null;
            return MoveStatus.Done;
        }

        return result;
    }

    public enum TokenStep : byte
    {
        Start = 0, End = 1, FirstStrike = 2, InitializeStrike = 3, ExecuteStrike = 4, EndStrike = 5, ExecuteEndStrike = 6, StepUp = 7, DelayStrike = 8
    }
}
