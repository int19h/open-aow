// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Net;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.Commands;

// status, path, move index
public partial class MoveTokenEvent : TokenEvent
{
    public MoveTokenEvent(CustomNode owner) : base(owner)
    {
        Path = new MovePath();
        MoveIndex = 0;
        _status = MoveStatus.Idle;
    }

    private MoveStatus _status;

    public MovePath Path { get; }
    public int MoveIndex { get; protected set; }

    // MoveT.PAS:1958
    public override void Dispose()
    {
        Path?.Dispose();
        base.Dispose();
    }

    // MoveT.PAS:1964
    // The path is the integer list of its steps. Each step is the four bytes of its map coordinate.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        bool present = persister.Has("Path");
        List<int> items = [with(Path.Count)];
        for (int index = 0; index < Path.Count; index++)
        {
            items.Add(PathStepImage.From(Path[index]).ToInt32());
        }

        int stored = persister.Int32List("Path", items: items, defaultValue: PathStepImage.From(Path.Default).ToInt32());
        if (!persister.Loading || !present)
        {
            return;
        }

        Path.Clear();
        Path.Default = Step(stored);
        for (int index = 0; index < items.Count; index++)
        {
            Path.Add(Step(items[index]));
        }
    }

    private static PathStep Step(int value) =>
        new(
            Location: new MapCoordinate(
                X: unchecked((sbyte)(byte)value),
                Y: unchecked((sbyte)(byte)(value >> 8)),
                MapLevel: unchecked((sbyte)(byte)(value >> 16))),
            MoveCost: unchecked((sbyte)(byte)(value >> 24)));

    // MoveT.PAS:2027
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result;
        base.Execute(tokenEvent);
        if (_status == MoveStatus.Idle)
        {
            if (Validate())
            {
                _status = MoveStatus.Moving;
                InitializeMove();
                StrategicMap.Current.Random(1048575);
                switch (Path.Count)
                {
                    case >= 0 and <= 1:
                        _status = MoveStatus.Done;
                        break;
                    case 2:
                        _status = LastMove();
                        break;
                    default:
                        _status = DefaultMove();
                        break;
                }
            }
            else
            {
                _status = MoveStatus.Failed;
            }
        }
        else
        {
            if (_status == MoveStatus.Moving)
            {
                _status = ProcessMove();
            }
        }

        if (_status == MoveStatus.Done)
        {
            FinishMove();
            result = ExecuteResult.Done;
        }
        else
        {
            if (_status == MoveStatus.Failed)
            {
                FinishMove();
                result = ExecuteResult.DoneHalt;
            }
            else
            {
                result = _status == MoveStatus.Finished ? ExecuteResult.Done : ExecuteResult.Retry;
            }
        }

        return result;
    }

    // MoveT.PAS:1970
    public virtual MapField GetMapField(MapCoordinate location) => StrategicMap.Current.MapContainer.GetField(location.X, location.Y, location.MapLevel);

    // MoveT.PAS:2019
    public virtual MoveStatus ProcessMove()
    {
        MoveStatus result = MoveIndex == 1 ? LastMove() : MoveIndex > 1 && MoveIndex < Path.Count ? DefaultMove() : MoveStatus.Done;
        return result;
    }

    // MoveT.PAS:2004
    public virtual bool Validate() => true;

    // MoveT.PAS:2010
    public virtual void FinishMove() => _status = MoveStatus.Finished;

    // MoveT.PAS:1997
    public virtual MoveStatus Move()
    {
        MoveStatus result;
        MoveIndex -= 1;
        result = MoveIndex == 0 ? MoveStatus.Done : MoveStatus.Moving;

        return result;
    }

    // MoveT.PAS:2016
    public virtual MoveStatus LastMove() => DefaultMove();

    // MoveT.PAS:2013
    public virtual MoveStatus DefaultMove() => Move();

    // MoveT.PAS:1975
    public int GetDirection(int index)
    {
        int result;
        if (index > 0 && index < Path.Count)
        {
            PathStep item = Path[index];
            PathStep fPathItem = Path[index - 1];
            bool condition = item.Location.MapLevel == fPathItem.Location.MapLevel;
            if (condition)
            {
                PathStep item2 = Path[index - 1];
                PathStep item3 = Path[index - 1];
                PathStep item4 = Path[index];
                condition = HexGeometry.DeltaHexToRadius(Path[index].Location.X, item4.Location.Y, item3.Location.X, item2.Location.Y) == 1;
            }

            if (condition)
            {
                PathStep item2 = Path[index - 1];
                PathStep item3 = Path[index - 1];
                PathStep item4 = Path[index];
                result = HexGeometry.DeltaHexToHexNumberFast(Path[index].Location.X, item4.Location.Y, item3.Location.X, item2.Location.Y);
            }
            else
            {
                result = 0;
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // MoveT.PAS:1989
    public int GetHexDirection(int index)
    {
        int result;
        if (index > 0 && index < Path.Count)
        {
            PathStep item = Path[index - 1];
            PathStep fPathItem = Path[index - 1];
            PathStep item2 = Path[index];
            result = HexGeometry.DeltaHexToHexDirection(Path[index].Location.X, item2.Location.Y, fPathItem.Location.X, item.Location.Y);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // intf_P5_07.inc:765

}
