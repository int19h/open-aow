// Translated from CombatTE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Draw;

using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

public partial class CombatMoveTokenEvent : MoveTokenEvent
{
    public CombatMoveTokenEvent(ECustomNode owner) : base(owner)
    { }

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
        if (_objects != null)
        {
            _objects?.Clear();
        }

        base.Dispose();
    }

    // CombatTE.pas:209
    public override AoWMapField GetMapField(MapCoordinate location) => (AoWMapField)CombatMap.Current.MapContainer.GetField(location.X, location.Y, 0);

    // CombatTE.pas:234
    public override bool Validate()
    {
        bool result;
        AoWMapField mapField;
        int index;
        TacticalBattleUnitHexSprite hexSprite;
        base.Validate();
        index = Path.Count - 1;
        mapField = GetMapField(Path[index]);
        hexSprite = (TacticalBattleUnitHexSprite)mapField.FindOwnedChild(2228484);
        if (hexSprite != null)
        {
            _objects = new List<TacticalBattleUnit>();
            Unit = hexSprite.Unit;
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // CombatTE.pas:299
    public override MoveStatus DefaultMove()
    {
        AoWMapField mapField;
        int direction;
        int X = default;
        int Y = default;
        void CheckFreeStrike()
        {
            int hexNumber = default;
            int hexX = default;
            int hexY = default;
            TacticalBattleUnitHexSprite hexSprite = default;
            if (Unit.GetEnabled() && _freeStrikeUnit == null)
            {
                mapField = (AoWMapField)Unit.UnitHexSprite.Owner;
                hexNumber = _neighbour + 1;
                while (hexNumber <= 6)
                {
                    var yhx = Unit.UnitHexSprite.HexY;
                    HexGeometry.CenterHexNumberToHex(Unit.UnitHexSprite.HexX, yhx, hexNumber, ref hexX, ref hexY);
                    hexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(hexX, hexY).FindOwnedChild(2228484);
                    if (hexSprite != null && PreviousUnit != hexSprite.Unit && hexSprite.Unit.GetEnabled() && !hexSprite.Unit.GetLocked() && CombatObject.MeleeDamageTypes(hexSprite.Unit) != Set<DamageType>.Of() && hexSprite.Unit.GetSide() != Unit.GetSide() && hexSprite.Action == TacticalBattleUnitHexSprite.SpriteAction.Move && hexSprite.Unit.GetAbilityEnabled(1) == Unit.GetAbilityEnabled(1))
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

                        if (_freeStrikeUnit != null)
                        {
                            hexNumber = 6;
                            Step = TokenStep.Start;
                        }
                    }

                    hexNumber = hexNumber + 1;
                }

                if (_freeStrikeUnit == null)
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
            if (_freeStrikeUnit != null)
            {
                result = MoveStatus.Moving;
                switch (Step)
                {
                    case TokenStep.Start:
                        try
                        {
                            Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Busy);
                            _freeStrikeUnit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepUp);
                            var hexY = _freeStrikeUnit.UnitHexSprite.HexY;
                            var fUnitHexSpriteHexY = Unit.UnitHexSprite.HexY;
                            var hexX = _freeStrikeUnit.UnitHexSprite.HexX;
                            var fUnitHexSpriteHexX = Unit.UnitHexSprite.HexX;
                            var meleeDirIndex = TacticalTables.GetMeleeDirectionIndex(_freeStrikeUnit.UnitHexSprite.HexX, hexX - fUnitHexSpriteHexX, hexY - fUnitHexSpriteHexY);
                            _freeStrikeUnit.UnitHexSprite.SetDirection((byte)meleeDirIndex);
                            Step = TokenStep.InitializeStrike;
                        }
                        catch
                        {
                            MessageBox(0, "Error during Token Start", "Error - TCMoveToken", 0);
                        }

                        break;
                    case TokenStep.InitializeStrike:
                        try
                        {
                            if (_freeStrikeUnit.UnitHexSprite.ActionDone)
                            {
                                RandSeed = AoWHSMap.Current.Random(16777215);
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
                            MessageBox(0, "Error during Token InitStrike", "Error - TCMoveToken", 0);
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

                                var hexY = Unit.UnitHexSprite.HexY;
                                HexGeometry.HexToPixel(Unit.UnitHexSprite.HexX, hexY, ref X, ref Y);
                                if (Strike.DamageTypes.Value - Strike.Effects.Value != Set<DamageType>.Of())
                                {
                                    CombatEvents.MakeDamageEffectHexSpriteList(X, Y, Strike.DamageTypes.Value - Strike.Effects.Value, false);
                                }

                                if (Strike.Effects.Value != Set<DamageType>.Of())
                                {
                                    CombatEvents.MakeDamageEffectHexSpriteList(X, Y, Strike.Effects.Value, true);
                                }

                                Step = TokenStep.EndStrike;
                            }
                        }
                        catch
                        {
                            MessageBox(0, "Error during Token ExeStrike", "Error - TCMoveToken", 0);
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
                            MessageBox(0, "Error during Token EndStrike", "Error - TCMoveToken", 0);
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

                                if (Strike != null)
                                {
                                    Strike.Release();
                                    Strike = null;
                                }

                                _freeStrikeUnit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Move);
                                Step = TokenStep.Start;
                                _freeStrikeUnit = null;
                                CheckFreeStrike();
                            }
                        }
                        catch
                        {
                            MessageBox(0, "Error during Token End", "Error - TCMoveToken", 0);
                        }

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
                                var hexY = Unit.UnitHexSprite.HexY;
                                var hexX = Unit.UnitHexSprite.HexX;
                                notExplorationEnabled = ((AoWMapField)CombatMap.Current.MapContainer.GetField(hexX, hexY, 0)).GetExplored(CombatMap.Current.SeatedPlayer);
                            }

                            if (notExplorationEnabled)
                            {
                                var hexY = Unit.UnitHexSprite.HexY;
                                var hexX = Unit.UnitHexSprite.HexX;
                                CombatMap.Current.MakeVisible(hexX, hexY, 0, 3);
                            }

                            direction = GetDirection(MoveIndex);
                            mapField = GetMapField(Path[MoveIndex - 1]);
                            if (mapField.GetPlayer() == -1)
                            {
                                if (direction != 0)
                                {
                                    if (Unit.UnitHexSprite.MoveFirst(direction, false) == -1)
                                    {
                                        result = MoveStatus.Failed;
                                    }
                                    else
                                    {
                                        result = MoveStatus.Moving;
                                    }

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
                    var hexX = Unit.UnitHexSprite.HexX;
                    CombatMap.Current.Players.GetPlayers(CombatMap.Current.CurrentCombatPlayer).LastX = hexX;
                    var hexY = Unit.UnitHexSprite.HexY;
                    CombatMap.Current.Players.GetPlayers(CombatMap.Current.CurrentCombatPlayer).LastY = hexY;
                }
            }
        }
        catch
        {
            MessageBox(0, "Error during Token", "Error - TCMoveToken", 0);
        }

        return result;
    }

    // CombatTE.pas:220
    public virtual bool Setup(TacticalBattleUnit unitObject, MovePath path)
    {
        bool result;
        int startIndex;
        Path.Clear();
        startIndex = path.MovePointsToPosition(unitObject.MovePoints);
        if (startIndex != path.Count - 1)
        {
            for (int I = startIndex, last = path.Count - 1; I <= last; I++)
            {
                var pathLocal = path[I];
                Path.Add((MapCoordinate)pathLocal);
            }

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // CombatTE.pas:253
    public MoveStatus ExecuteDefaultMove()
    {
        MoveStatus result;
        AoWMapField newField;
        AoWMapField oldField;
        Rect R;
        Move();
        oldField = (AoWMapField)Unit.UnitHexSprite.Owner;
        newField = GetMapField(Path[MoveIndex]);
        Unit.UnitHexSprite.MoveTo(newField, 0);
        if (newField.FindChildIndex(Unit.UnitHexSprite) != -1)
        {
            result = ProcessMove();
            var path = Path[MoveIndex];
            Unit.MovedTo(oldField, newField, path.Tag);
            var rectangle = new Rect(oldField.HexX, oldField.HexY, oldField.HexX + 1, oldField.HexY + 1);
            CombatMap.Current.TriggerMapChangedEvent(rectangle, 0);
            if (MoveIndex == 0 && newField.FindChild(2229544) != null && Unit.CombatPlayer != CombatMap.Current.Combat.CombatData.DefenderPlayer)
            {
                Unit.SetStatus(Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Fled));
                var hexY = Unit.UnitHexSprite.HexY;
                var hexX = Unit.UnitHexSprite.HexX;
                var fUnitHexSpriteHexY = Unit.UnitHexSprite.HexY;
                R = new Rect(Unit.UnitHexSprite.HexX, fUnitHexSpriteHexY, hexX + 1, hexY + 1);
                Unit.UnitHexSprite?.Dispose();
                CombatMap.Current.TriggerMapChangedEvent(R, 0);
                Unit.UnitHexSprite = null;
                result = MoveStatus.Done;
            }
            else
            {
                if (CombatMap.Current.ExplorationEnabled)
                {
                    var player = Unit.GetPlayer();
                    var visibilityRange = Unit.GetVisibilityRange();
                    var hexY = Unit.UnitHexSprite.HexY;
                    var hexX = Unit.UnitHexSprite.HexX;
                    CombatMap.Current.ExploreArea(hexX, hexY, 0, player, visibilityRange);
                    var fUnitVisibilityRange = Unit.GetVisibilityRange();
                    var fUnitHexSpriteHexY = Unit.UnitHexSprite.HexY;
                    var fUnitHexSpriteHexX = Unit.UnitHexSprite.HexX;
                    CombatMap.Current.MapChangedRadius(fUnitHexSpriteHexX, fUnitHexSpriteHexY, 0, fUnitVisibilityRange + 1);
                }
                else
                {
                    var hexY = Unit.UnitHexSprite.HexY;
                    var hexX = Unit.UnitHexSprite.HexX;
                    var fUnitHexSpriteHexY = Unit.UnitHexSprite.HexY;
                    var rectangle2 = new Rect(Unit.UnitHexSprite.HexX, fUnitHexSpriteHexY, hexX + 1, hexY + 1);
                    CombatMap.Current.TriggerMapChangedEvent(rectangle2, 0);
                }

                var fUnitPlayer = Unit.GetPlayer();
                if (AoWHSMap.Current.Players.GetPlayers(fUnitPlayer).PlayerType != PlayerType.Human || CombatMap.Current.Players.GetPlayers(Unit.CombatPlayer).Auto)
                {
                    Unit.MovePoints = 0;
                }
            }
        }
        else
        {
            Unit.UnitHexSprite = null;
            result = MoveStatus.Done;
        }

        return result;
    }

    public enum TokenStep : byte
    {
        Start = 0, End = 1, FirstStrike = 2, InitializeStrike = 3, ExecuteStrike = 4, EndStrike = 5, ExecuteEndStrike = 6, StepUp = 7, DelayStrike = 8
    }

    // intf_P5_07.inc:765

}
