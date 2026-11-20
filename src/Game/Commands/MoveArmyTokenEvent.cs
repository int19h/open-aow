#nullable disable
// Translated from AoWE.pas.
using System.Diagnostics.CodeAnalysis;
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI.Groups;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Commands;

// mode, unit IDs, selection, army sprite, moving/target player, target, destination, animate
public class MoveArmyTokenEvent(CustomNode owner) : MoveTokenEvent(owner)
{

    public new const int ClassId = 135_424;

    public Set<ArmyMoveModeElement> Mode { get; protected set; }

    private readonly Int32List _unitIds = new();

    public byte Selection { get; protected set; }
    public ArmyHexSprite ArmyHexSprite { get; protected set; }

    private sbyte _player;

    public sbyte TargetPlayer { get; private set; }
    public MapCoordinate Target { get; private set; }

    private MapCoordinate Destination { get; set; }

    private bool _animate;

    // MoveT.PAS:2066
    public override void Dispose()
    {
        _unitIds?.Dispose();
        base.Dispose();
    }

    // MoveT.PAS:2093
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int32List(name: "UnitIds", list: _unitIds);
        _player = persister.UnsignedByte(name: "Player", value: _player);
        Target = persister.Coordinate(name: "Target", value: Target, width: CoordinateWidth.Byte);
        TargetPlayer = persister.UnsignedByte(name: "TargetPlayer", value: TargetPlayer);
        Mode = persister.Set("Mode", value: Mode);
        Destination = persister.Coordinate(name: "Destination", value: Destination, width: CoordinateWidth.Byte);
    }

    // MoveT.PAS:2137
    public override bool Validate()
    {
        int i = Path.Count - 1;
        bool result = true;
        if (i < 1)
        {
            result = false;
        }
        else
        {
            ArmyHexSprite = GetMapField(Path[i].Location).GetArmyHexSprite();
            if (ArmyHexSprite is null)
            {
                result = false;
            }
            else
            {
                i = _unitIds.Count;
                Selection = 0;
                while (i != 0 && result)
                {
                    i -= 1;
                    int item = _unitIds[i];
                    int unitIndex = ArmyHexSprite.Army.Units.IndexOfId(item);
                    if (unitIndex != -1)
                    {
                        Selection = (byte)(Selection | (1 << unitIndex));
                    }

                    int fUnitIdsItem = _unitIds[i];
                    result = ArmyHexSprite.Army.Units.IndexOfId(fUnitIdsItem) != -1;
                }

                MoveControlSettings settings = new(null) { Holder = CreationHolder() };
                settings.SetupArmy(ArmyHexSprite, Selection);
                settings.Mode = settings.Mode.With(MoveModeElement.AI);
                if (StrategicMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human && StrategicMap.Current.ExplorationEnabled)
                {
                    settings.Mode = settings.Mode.With(MoveModeElement.ExploredTerrainOnly);
                }

                result = result && StrategicMap.Current.MoveControl.ValidPath(settings, Path);
                result = result && ArmyHexSprite.Army.Player == _player;
                sbyte targetPlayer = StrategicMap.Current.MapContainer.GetField(Target.X, Target.Y, Target.MapLevel).GetPlayer();
                result = result && (targetPlayer == TargetPlayer || targetPlayer == ArmyHexSprite.Army.Player);
                if (Mode.Contains(ArmyMoveModeElement.BuildRoads))
                {
                    bool condition = result;
                    if (condition)
                    {
                        sbyte movePoints = ArmyHexSprite.Army.MovePoints(Selection);
                        condition = AbilityRules.RoadConstructionCost(_player, Path, movePoints) <= StrategicMap.Current.Players.GetPlayers(_player).Gems;
                    }

                    result = condition;
                }

                _ = settings.Release();
            }
        }

        if (!result && StrategicMap.Current.Players.GetPlayers(_player).PlayerType != PlayerType.Human && StrategicMap.Current.Players.GetPlayers(_player).AIControl.MoveExecutor.Status == AIMoveExecutorStatus.Busy)
        {
            StrategicMap.Current.Players.GetPlayers(_player).AIControl.MoveExecutor.InvalidateMove(ArmyHexSprite, Selection);
        }

        return result;
    }

    // MoveT.PAS:2183
    public override void InitializeMove()
    {
        base.InitializeMove();
        _animate = false;
        int last = Path.Count - 1;
        Selection = ArmyHexSprite.Army.ValidateSelection(Selection);
        byte validateSelection = ArmyHexSprite.Army.ValidateSelection(255);
        if (Selection != validateSelection)
        {
            ArmyHexSprite armyHexSprite = new(null) { Holder = CreationHolder() };
            armyHexSprite.Army.SetPlayer(ArmyHexSprite.Army.Player);
            _ = armyHexSprite.Army.AddUnitSelection(ArmyHexSprite.Army.Units, Selection);
            PathStep item = Path[last];
            PathStep fPathItem = Path[last];
            PathStep item2 = Path[last];
            _ = armyHexSprite.PlacePrivate(StrategicMap.Current.MapContainer, item2.Location.X, fPathItem.Location.Y, L: item.Location.MapLevel);
            ArmyHexSprite.Army.ResetDestinationPosition();
            if (ArmyHexSprite.GetSelected())
            {
                StrategicMap.Current.Selection.UnselectLocation();
                StrategicMap.Current.Selection.SelectLocationHexSprite(armyHexSprite);
                StrategicMap.Current.SelectedArmy.DeactivateMoveSelection();
            }

            ArmyHexSprite = armyHexSprite;
            Selection = 255;
        }
        else
        {
            if (ArmyHexSprite.GetSelected())
            {
                StrategicMap.Current.SelectedArmy.DeactivateMoveSelection();
            }
        }

        Army army = ArmyHexSprite.Army;
        army.Mode = army.Mode.With(ArmyModeElement.Moving);
        army.Destination = new MapCoordinate(Destination.X, Destination.Y, Destination.MapLevel);
    }

    // MoveT.PAS:2221
    public override void FinishMove()
    {
        base.FinishMove();
        if (ArmyHexSprite is not null)
        {
            bool selected = ArmyHexSprite.GetSelected();
            MapField field = ArmyHexSprite.Owner;
            ArmyHexSprite.SetOwner(null);
            ArmyHexSprite armyHexSprite = GetNoneMovingArmyHexSprite(field);
            ArmyHexSprite.SetOwner(field);
            if (armyHexSprite is not null)
            {
                ArmyHexSprite armyHexSprite2 = ArmyHexSprite;
                armyHexSprite2.Army.UserMode = ArmyHexSprite.Army.UserMode & armyHexSprite.Army.UserMode;
                ArmyHexSprite = armyHexSprite2;
                armyHexSprite.Army.AssignSettings(ArmyHexSprite.Army);
                _ = armyHexSprite.Army.AddUnitSelection(ArmyHexSprite.Army.Units, selection: 255);
                ArmyHexSprite = armyHexSprite;
                if (selected)
                {
                    StrategicMap.Current.SelectedArmy.Select(armyHexSprite);
                    StrategicMap.Current.SelectedArmy.DeactivateMoveSelection();
                }
            }

            ArmyHexSprite armyHexSprite3 = ArmyHexSprite;
            armyHexSprite3.Army.Mode = ArmyHexSprite.Army.Mode - Set<ArmyModeElement>.Of(ArmyModeElement.Moving);
            ArmyHexSprite = armyHexSprite3;
            ArmyHexSprite.CancelMove();
            if (selected)
            {
                StrategicMap.Current.SelectedArmy.ActivateMoveSelection();
                StrategicMap.Current.SelectedArmy.Update();
                if (Mode.Contains(ArmyMoveModeElement.SelectEndLocation))
                {
                    sbyte hexLevel = ArmyHexSprite.HexLevel;
                    sbyte hexY = ArmyHexSprite.HexY;
                    sbyte hexX = ArmyHexSprite.HexX;
                    StrategicMap.Current.Selection.SelectLocationHex(hexX, hexY, hexLevel);
                }
            }
        }
    }

    // MoveT.PAS:2368
    public override MoveStatus DefaultMove()
    {
        if (!ArmyHexSprite.Moving())
        {
            if (CanMoveNext())
            {
                SetupNextMove();
                return !Animating() ? ExecuteMove() : MoveStatus.Moving;
            }
            else
            {
                return MoveStatus.Failed;
            }
        }
        else
        {
            if (Animating())
            {
                UpdateAnimation();
                bool condition = ArmyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer;
                if (!condition)
                {
                    sbyte hexLevel = ArmyHexSprite.HexLevel;
                    sbyte hexY = ArmyHexSprite.HexY;
                    sbyte hexX = ArmyHexSprite.HexX;
                    condition = StrategicMap.Current.Watching(hexX, hexY, L: hexLevel, radius: 0);
                }

                if (condition)
                {
                    ArmyHexSprite.PlayMoveSfx();
                }

                return !Animating() ? ExecuteMove() : MoveStatus.Moving;
            }
            else
            {
                return MoveStatus.Failed;
            }
        }
    }

    // MoveT.PAS:2304
    public virtual bool Animating() => ArmyHexSprite.GetMoveStatus() is < 100 and not -1;

    // MoveT.PAS:2309
    public virtual void UpdateAnimation() => ArmyHexSprite.MoveNext();

    // MoveT.PAS:2342
    public virtual void SetupNextMove()
    {
        if ((MoveVisible(MoveIndex) || _animate) && (Path[MoveIndex].Location.MapLevel == StrategicMap.Current.ViewMapLevel || StrategicMap.Current.ShowIndependentMoves))
        {
            _animate = true;
            int direction = GetDirection(MoveIndex);
            bool instant;
            if (direction == 0)
            {
                direction = GetHexDirection(MoveIndex);
                instant = true;
            }
            else
            {
                instant = false;
            }

            _ = ArmyHexSprite.MoveFirst(direction, instant);
            if (ObserveMove(MoveIndex - 1))
            {
                MapField field = GetMapField(Path[MoveIndex - 1].Location);
                _ = StrategicMap.Current.ValidateObservation(ArmyHexSprite.Army.Player, field.HexX, field.HexY, field.HexLevel, radius: 0);
            }
        }
    }

    // MoveT.PAS:2312
    public virtual bool CanMoveNext()
    {
        bool result = false;
        bool stop = false;
        int i = MoveIndex;
        while (i != 0 && !(stop || result))
        {
            i -= 1;
            MapField field = GetMapField(Path[i].Location);
            result = field.GetPlayer() == -1;
            if (!result)
            {
                ArmyHexSprite armyHexSprite = GetNoneMovingArmyHexSprite(field);
                if (armyHexSprite is not null)
                {
                    if (armyHexSprite.Army.Player == ArmyHexSprite.Army.Player)
                    {
                        result = ArmyHexSprite.Army.CanCombine(armyHexSprite.Army, selection: 255);
                    }
                    else
                    {
                        // { Original queries the moving player toward the other army. }
                        if (StrategicMap.Current.Players.GetDiplomaticRelation(ArmyHexSprite.Army.Player, otherPlayer: armyHexSprite.Army.Player) != DiplomaticRelation.Alliance)
                        {
                            stop = true;
                        }
                    }
                }
                else
                {
                    result = field.GetPlayer() == ArmyHexSprite.Army.Player;
                }
            }
        }

        return result;
    }

    // MoveT.PAS:2279
    public virtual MoveStatus ExecuteMove()
    {
        if (Mode.Contains(ArmyMoveModeElement.BuildRoads))
        {
            _ = ConstructRoad(field: GetMapField(Path[MoveIndex].Location), player: ArmyHexSprite.Army.Player);
        }

        MoveIndex -= 1;
        MapField field = GetMapField(Path[MoveIndex].Location);
        PathStep item = Path[MoveIndex];
        _ = ArmyHexSprite.MoveTo(field, item.MoveCost);
        if (field.FindChildIndex(ArmyHexSprite) != -1)
        {
            int direction = GetDirection(MoveIndex + 1);
            if (direction == 0)
            {
                direction = GetHexDirection(MoveIndex + 1);
            }

            if (direction is > 0 and <= 6)
            {
                ArmyHexSprite.SetDirection((byte)direction);
            }
            else
            {
                ArmyHexSprite.SetDirection(0);
            }

            if (Mode.Contains(ArmyMoveModeElement.BuildRoads))
            {
                _ = ConstructRoad(ArmyHexSprite.Army.Player, field);
            }

            return ProcessMove();
        }
        else
        {
            ArmyHexSprite = null;
            return MoveStatus.Done;
        }
    }

    // MoveT.PAS:2108
    public virtual bool Setup(ArmyHexSprite armyHexSprite, byte selection, MovePath path, Set<ArmyMoveModeElement> mode)
    {
        Path.Clear();
        int first = path.MovePointsToPosition(armyHexSprite.Army.MovePoints(selection));
        int last2 = path.Count - 1;
        if (last2 > first)
        {
            for (int i = first; i <= last2; i++)
            {
                PathStep item = path[i];
                _ = Path.Add(item);
            }

            ArmyHexSprite = armyHexSprite;
            _player = armyHexSprite.Army.Player;
            Mode = mode;
            TargetPlayer = GetMapField(path[0].Location).GetPlayer();
            Target = new MapCoordinate(path[0].Location.X, path[0].Location.Y, path[0].Location.MapLevel);
            Selection = armyHexSprite.Army.ValidateSelection(selection);
            for (int i = 0, last = armyHexSprite.Army.Units.Count - 1; i <= last; i++)
            {
                if (((1 << i) & selection) != 0)
                {
                    Troop troop = armyHexSprite.Army.Units.GetUnit(i);
                    _ = _unitIds.Add(troop.UnitId);
                }
            }

            Destination = new MapCoordinate(ArmyHexSprite.Army.Destination.X, ArmyHexSprite.Army.Destination.Y, ArmyHexSprite.Army.Destination.MapLevel);
            return true;
        }
        else
        {
            return false;
        }
    }

    // MoveT.PAS:2259
    public bool ObserveMove(int index)
    {
        bool visible = MoveVisible(index);
        bool result = visible && StrategicMap.Current.ShowIndependentMoves && (StrategicMap.Current.SeatedPlayer != ArmyHexSprite.Army.Player || StrategicMap.Current.Players.GetPlayers((sbyte)(byte)StrategicMap.Current.SeatedPlayer).PlayerType != PlayerType.Human);
        if (result && StrategicMap.Current.TokenManager.TokenExecutor.ExecutingTokenEvent is MultiMoveTokenEvent eventValue && eventValue.Simultaneous)
        {
            visible = this == eventValue.Events[0];
            return visible;
        }

        return result;
    }

    // MoveT.PAS:2269
    public bool MoveVisible(int index)
    {
        bool result;
        if (StrategicMap.Current.SeatedPlayer != -1)
        {
            PathStep item = Path[index];
            PathStep fPathItem = Path[index];
            PathStep item2 = Path[index];
            result = StrategicMap.Current.Visible(item2.Location.X, fPathItem.Location.Y, L: item.Location.MapLevel, radius: 0);
            if (result && ArmyHexSprite.Army.Status.Contains(ArmyStatusElement.FullyConcealed))
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // intf_P5_07.inc:816
    public ArmyHexSprite GetNoneMovingArmyHexSprite(MapField field)
    {
        for (int i = field.Count - 1; i >= 0; i--)
        {
            HexSprite sprite = field.HexSpriteList[i];
            // The list holds every hex sprite on the field. Only ArmyHexSprite registers 131607.
            if (sprite is ArmyHexSprite army && sprite.Owner == field)
            {
                if (!army.Army.Mode.Contains(ArmyModeElement.Moving))
                {
                    return army;
                }
            }
        }
        return null;
    }

    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "The unimplemented Delphi ConstructRoad stub keeps player and field.")]
    private static bool ConstructRoad(sbyte player, MapField field) => false;
}
