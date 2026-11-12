// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.AI.Groups;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

// mode, unit IDs, selection, army sprite, moving/target player, target, destination, animate
public partial class MoveArmyTokenEvent : MoveTokenEvent
{
    public new const int ClassId = 135424;

    public Set<ArmyMoveModeElement> Mode;
    private Int32List _unitIds;
    public byte Selection;
    public ArmyHexSprite ArmyHexSprite;
    private sbyte _player;
    public sbyte TargetPlayer;
    public sbyte TargetX;
    public sbyte TargetY;
    public sbyte TargetL;
    private sbyte _destinationX;
    private sbyte _destinationY;
    private sbyte _destinationL;
    private bool _animate;

    // MoveT.PAS:2066
    public override void Dispose()
    {
        _unitIds?.Dispose();
        base.Dispose();
    }

    // MoveT.PAS:2093
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwStreamProc(30, _unitIds.LoadFromStream, _unitIds.SaveToStream);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _player), 31);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref TargetX), 32);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref TargetY), 33);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref TargetL), 34);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref TargetPlayer), 35);
        RwSet(ref Mode, 1, 36, stream.rwData, 0);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _destinationX), 37);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _destinationY), 38);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _destinationL), 39);
    }

    // MoveT.PAS:2060
    public MoveArmyTokenEvent(ECustomNode owner) : base(owner)
    {
        _unitIds = new Int32List();
    }

    // MoveT.PAS:2137
    public override bool Validate()
    {
        int I;
        int unitIndex;
        MoveControlSettings settings;
        sbyte targetPlayer;
        I = Path.Count - 1;
        bool result = true;
        if (I < 1)
        {
            result = false;
        }
        else
        {
            ArmyHexSprite = (ArmyHexSprite)GetMapField(Path[I]).FindOwnedChild(131607);
            if (ArmyHexSprite == null)
            {
                result = false;
            }
            else
            {
                I = _unitIds.Count;
                Selection = 0;
                while (I != 0 && result)
                {
                    I -= 1;
                    var item = _unitIds[I];
                    unitIndex = ArmyHexSprite.Army.IndexOfId(item);
                    if (unitIndex != -1)
                    {
                        Selection = (byte)(Selection | (1 << unitIndex));
                    }

                    var fUnitIdsItem = _unitIds[I];
                    result = ArmyHexSprite.Army.IndexOfId(fUnitIdsItem) != -1;
                }

                settings = new MoveControlSettings(null);
                settings.SetupArmy(ArmyHexSprite, Selection);
                settings.Mode = settings.Mode.With(MoveModeElement.AI);
                if (AoWHSMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human && AoWHSMap.Current.ExplorationEnabled)
                {
                    settings.Mode = settings.Mode.With(MoveModeElement.ExploredTerrainOnly);
                }

                result = result && AoWHSMap.Current.MoveControl.ValidPath(settings, Path);
                result = result && ArmyHexSprite.Army.Player == _player;
                targetPlayer = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(TargetX, TargetY, TargetL)).GetPlayer();
                result = result && (targetPlayer == TargetPlayer || targetPlayer == ArmyHexSprite.Army.Player);
                if (Mode.Contains(ArmyMoveModeElement.BuildRoads))
                {
                    bool condition = result;
                    if (condition)
                    {
                        var movePoints = ArmyHexSprite.Army.MovePoints(Selection);
                        condition = AbilityRules.RoadConstructionCost(_player, Path, movePoints) <= AoWHSMap.Current.Players.GetPlayers(_player).Gems;
                    }

                    result = condition;
                }

                settings.Release();
            }
        }

        if (!result && AoWHSMap.Current.Players.GetPlayers(_player).PlayerType != PlayerType.Human && AoWHSMap.Current.Players.GetPlayers(_player).AIControl.MoveExecutor.Status == AIMoveExecutorStatus.Busy)
        {
            AoWHSMap.Current.Players.GetPlayers(_player).AIControl.MoveExecutor.InvalidateMove(ArmyHexSprite, Selection);
        }

        return result;
    }

    // MoveT.PAS:2183
    public override void InitializeMove()
    {
        int last;
        ArmyHexSprite armyHexSprite;
        base.InitializeMove();
        _animate = false;
        last = Path.Count - 1;
        Selection = ArmyHexSprite.Army.ValidateSelection(Selection);
        var validateSelection = ArmyHexSprite.Army.ValidateSelection(255);
        if (Selection != validateSelection)
        {
            armyHexSprite = new ArmyHexSprite(null);
            armyHexSprite.Army.SetPlayer(ArmyHexSprite.Army.Player);
            armyHexSprite.Army.AddUnitSelection(ArmyHexSprite.Army, Selection);
            var item = Path[last];
            var fPathItem = Path[last];
            var item2 = Path[last];
            armyHexSprite.PlacePrivate(AoWHSMap.Current.MapContainer, item2.X, fPathItem.Y, item.MapLevel);
            ArmyHexSprite.Army.ResetDestinationPosition();
            if (ArmyHexSprite.GetSelected())
            {
                AoWHSMap.Current.Selection.UnselectLocation();
                AoWHSMap.Current.Selection.SelectLocationHexSprite(armyHexSprite);
                AoWHSMap.Current.SelectedArmy.DeactivateMoveSelection();
            }

            ArmyHexSprite = armyHexSprite;
            Selection = 255;
        }
        else
        {
            if (ArmyHexSprite.GetSelected())
            {
                AoWHSMap.Current.SelectedArmy.DeactivateMoveSelection();
            }
        }

        var army = ArmyHexSprite.Army;
        army.Mode = army.Mode.With(ArmyModeElement.Moving);
        army.DestinationX = this._destinationX;
        army.DestinationY = this._destinationY;
        army.DestinationL = this._destinationL;
    }

    // MoveT.PAS:2221
    public override void FinishMove()
    {
        bool selected;
        MapField field;
        ArmyHexSprite armyHexSprite;
        base.FinishMove();
        if (ArmyHexSprite != null)
        {
            selected = ArmyHexSprite.GetSelected();
            field = (MapField)ArmyHexSprite.Owner;
            ArmyHexSprite.SetOwner(null);
            armyHexSprite = GetNoneMovingArmyHexSprite(field);
            ArmyHexSprite.SetOwner(field);
            if (armyHexSprite != null)
            {
                ArmyHexSprite.Army.UserMode = ArmyHexSprite.Army.UserMode & armyHexSprite.Army.UserMode;
                armyHexSprite.Army.AssignSettings(ArmyHexSprite.Army);
                armyHexSprite.Army.AddUnitSelection(ArmyHexSprite.Army, 255);
                ArmyHexSprite = armyHexSprite;
                if (selected)
                {
                    AoWHSMap.Current.SelectedArmy.Select(armyHexSprite);
                    AoWHSMap.Current.SelectedArmy.DeactivateMoveSelection();
                }
            }

            ArmyHexSprite.Army.Mode = ArmyHexSprite.Army.Mode - Set<ArmyModeElement>.Of(ArmyModeElement.Moving);
            ArmyHexSprite.CancelMove();
            if (selected)
            {
                AoWHSMap.Current.SelectedArmy.ActivateMoveSelection();
                AoWHSMap.Current.SelectedArmy.Update();
                if (Mode.Contains(ArmyMoveModeElement.SelectEndLocation))
                {
                    var hexLevel = ArmyHexSprite.GetHexLevel();
                    var hexY = ArmyHexSprite.GetHexY();
                    var hexX = ArmyHexSprite.GetHexX();
                    AoWHSMap.Current.Selection.SelectLocationHex(hexX, hexY, hexLevel);
                }
            }
        }
    }

    // MoveT.PAS:2368
    public override MoveStatus DefaultMove()
    {
        MoveStatus result;
        if (!ArmyHexSprite.Moving())
        {
            if (CanMoveNext())
            {
                SetupNextMove();
                if (!Animating())
                {
                    result = ExecuteMove();
                }
                else
                {
                    result = MoveStatus.Moving;
                }
            }
            else
            {
                result = MoveStatus.Failed;
            }
        }
        else
        {
            if (Animating())
            {
                UpdateAnimation();
                bool condition = ArmyHexSprite.Army.Player == AoWHSMap.Current.SeatedPlayer;
                if (!condition)
                {
                    var hexLevel = ArmyHexSprite.GetHexLevel();
                    var hexY = ArmyHexSprite.GetHexY();
                    var hexX = ArmyHexSprite.GetHexX();
                    condition = AoWHSMap.Current.Watching(hexX, hexY, hexLevel, 0);
                }

                if (condition)
                {
                    ArmyHexSprite.PlayMoveSfx();
                }

                if (!Animating())
                {
                    result = ExecuteMove();
                }
                else
                {
                    result = MoveStatus.Moving;
                }
            }
            else
            {
                result = MoveStatus.Failed;
            }
        }

        return result;
    }

    // MoveT.PAS:2304
    public virtual bool Animating() => ArmyHexSprite.GetMoveStatus() < 100 && ArmyHexSprite.GetMoveStatus() != -1;

    // MoveT.PAS:2309
    public virtual void UpdateAnimation()
    {
        ArmyHexSprite.MoveNext();
    }

    // MoveT.PAS:2342
    public virtual void SetupNextMove()
    {
        int direction;
        bool instant;
        AoWMapField field;
        if ((MoveVisible(MoveIndex) || _animate) && (Path[MoveIndex].MapLevel == AoWHSMap.Current.ViewL || AoWHSMap.Current.ShowIndependentMoves))
        {
            _animate = true;
            direction = GetDirection(MoveIndex);
            if (direction == 0)
            {
                direction = GetHexDirection(MoveIndex);
                instant = true;
            }
            else
            {
                instant = false;
            }

            ArmyHexSprite.MoveFirst(direction, instant);
            if (ObserveMove(MoveIndex - 1))
            {
                field = GetMapField(Path[MoveIndex - 1]);
                AoWHSMap.Current.ValidateObservation(ArmyHexSprite.Army.Player, field.HexX, field.HexY, field.HexLevel, 0);
            }
        }
    }

    // MoveT.PAS:2312
    public virtual bool CanMoveNext()
    {
        bool stop;
        int I;
        AoWMapField field;
        ArmyHexSprite armyHexSprite;
        bool result = false;
        stop = false;
        I = MoveIndex;
        while (I != 0 && !(stop || result))
        {
            I -= 1;
            field = GetMapField(Path[I]);
            result = field.GetPlayer() == -1;
            if (!result)
            {
                armyHexSprite = GetNoneMovingArmyHexSprite(field);
                if (armyHexSprite != null)
                {
                    if (armyHexSprite.Army.Player == ArmyHexSprite.Army.Player)
                    {
                        result = ArmyHexSprite.Army.CanCombine(armyHexSprite.Army, 255);
                    }
                    else
                    {
                        // { Original queries the moving player toward the other army. }
                        if (AoWHSMap.Current.Players.GetDiplomaticRelation(ArmyHexSprite.Army.Player, armyHexSprite.Army.Player) != DiplomaticRelation.Alliance)
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
        MoveStatus result;
        AoWMapField field;
        int direction;
        if (Mode.Contains(ArmyMoveModeElement.BuildRoads))
        {
            var mapField = GetMapField(Path[MoveIndex]);
            ConstructRoad(ArmyHexSprite.Army.Player, mapField);
        }

        MoveIndex -= 1;
        field = GetMapField(Path[MoveIndex]);
        var item = Path[MoveIndex];
        ArmyHexSprite.MoveTo(field, item.Tag);
        if (field.FindChildIndex(ArmyHexSprite) != -1)
        {
            direction = GetDirection(MoveIndex + 1);
            if (direction == 0)
            {
                direction = GetHexDirection(MoveIndex + 1);
            }

            if (direction > 0 && direction <= 6)
            {
                ArmyHexSprite.SetDirection((byte)direction);
            }
            else
            {
                ArmyHexSprite.SetDirection(0);
            }

            if (Mode.Contains(ArmyMoveModeElement.BuildRoads))
            {
                ConstructRoad(ArmyHexSprite.Army.Player, field);
            }

            result = ProcessMove();
        }
        else
        {
            ArmyHexSprite = null;
            result = MoveStatus.Done;
        }

        return result;
    }

    // MoveT.PAS:2108
    public virtual bool Setup(ArmyHexSprite armyHexSprite, byte selection, MovePath path, Set<ArmyMoveModeElement> mode)
    {
        bool result;
        int first;
        int Last;
        Path.Clear();
        first = path.MovePointsToPosition(armyHexSprite.Army.MovePoints(selection));
        Last = path.Count - 1;
        if (Last > first)
        {
            for (int I = first; I <= Last; I++)
            {
                var item = path[I];
                Path.Add(item);
            }

            ArmyHexSprite = armyHexSprite;
            _player = armyHexSprite.Army.Player;
            Mode = mode;
            TargetPlayer = GetMapField(path[0]).GetPlayer();
            TargetX = path[0].X;
            TargetY = path[0].Y;
            TargetL = path[0].MapLevel;
            Selection = armyHexSprite.Army.ValidateSelection(selection);
            for (int I = 0, last = armyHexSprite.Army.Count - 1; I <= last; I++)
            {
                if (((1 << I) & selection) != 0)
                {
                    var troop = armyHexSprite.Army.GetUnit(I);
                    _unitIds.Add(troop.UnitId);
                }
            }

            _destinationX = ArmyHexSprite.Army.DestinationX;
            _destinationY = ArmyHexSprite.Army.DestinationY;
            _destinationL = ArmyHexSprite.Army.DestinationL;
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // MoveT.PAS:2259
    public bool ObserveMove(int index)
    {
        bool visible;
        MultiMoveTokenEvent eventValue;
        visible = MoveVisible(index);
        bool result = visible && AoWHSMap.Current.ShowIndependentMoves && (AoWHSMap.Current.SeatedPlayer != ArmyHexSprite.Army.Player || AoWHSMap.Current.Players.GetPlayers((sbyte)(byte)AoWHSMap.Current.SeatedPlayer).PlayerType != PlayerType.Human);
        if (result && AoWHSMap.Current.TokenManager.TokenExecutor.GetExecutingTokenEvent() is MultiMoveTokenEvent && ((MultiMoveTokenEvent)AoWHSMap.Current.TokenManager.TokenExecutor.GetExecutingTokenEvent()).Simultaneous)
        {
            eventValue = (MultiMoveTokenEvent)AoWHSMap.Current.TokenManager.TokenExecutor.GetExecutingTokenEvent();
            visible = this == eventValue.Events[0];
            result = visible;
        }

        return result;
    }

    // MoveT.PAS:2269
    public bool MoveVisible(int index)
    {
        bool result;
        if (AoWHSMap.Current.SeatedPlayer != -1)
        {
            var item = Path[index];
            var fPathItem = Path[index];
            var item2 = Path[index];
            result = AoWHSMap.Current.Visible(item2.X, fPathItem.Y, item.MapLevel, 0);
            if (result && ArmyHexSprite.Army.Status.Contains(ArmyStatusElement.FullyConcealed))
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // intf_P5_07.inc:816
    public ArmyHexSprite GetNoneMovingArmyHexSprite(MapField field)
    {
        for (int i = field.Count - 1; i >= 0; i--)
        {
            var sprite = field.HexSpriteList[i];
            if (sprite.EngineClass.ClassId == 0x20217 && sprite.Owner == field)
            {
                var army = (ArmyHexSprite)sprite;
                if (!army.Army.Mode.Contains(ArmyModeElement.Moving)) return army;
            }
        }
        return null;
    }

    private static bool ConstructRoad(sbyte player, AoWMapField field)
    {
        return false;
    }
}
