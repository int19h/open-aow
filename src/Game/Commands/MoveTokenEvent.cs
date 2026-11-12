// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.Commands;

// status, path, move index
public partial class MoveTokenEvent : TokenEvent
{
    public MoveTokenEvent(ECustomNode owner) : base(owner)
    {
        Path = new MovePath();
        MoveIndex = 0;
        Status = MoveStatus.Idle;
    }

    public MoveStatus Status;
    public MovePath Path;
    public int MoveIndex;

    // MoveT.PAS:1958
    public override void Dispose()
    {
        Path?.Dispose();
        base.Dispose();
    }

    // MoveT.PAS:1964
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwStreamProc(20, Path.LoadFromStream, Path.SaveToStream);
    }

    // MoveT.PAS:2027
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result;
        base.Execute(tokenEvent);
        if (Status == MoveStatus.Idle)
        {
            if (Validate())
            {
                Status = MoveStatus.Moving;
                InitializeMove();
                AoWHSMap.Current.Random(1048575);
                switch (Path.Count)
                {
                    case >= 0 and <= 1:
                        Status = MoveStatus.Done;
                        break;
                    case 2:
                        Status = LastMove();
                        break;
                    default:
                        Status = DefaultMove();
                        break;
                }
            }
            else
            {
                Status = MoveStatus.Failed;
            }
        }
        else
        {
            if (Status == MoveStatus.Moving)
            {
                Status = ProcessMove();
            }
        }

        if (Status == MoveStatus.Done)
        {
            FinishMove();
            result = ExecuteResult.Done;
        }
        else
        {
            if (Status == MoveStatus.Failed)
            {
                FinishMove();
                result = ExecuteResult.DoneHalt;
            }
            else
            {
                if (Status == MoveStatus.Finished)
                {
                    result = ExecuteResult.Done;
                }
                else
                {
                    result = ExecuteResult.Retry;
                }
            }
        }

        return result;
    }

    // MoveT.PAS:1970
    public virtual AoWMapField GetMapField(MapCoordinate location) => (AoWMapField)AoWHSMap.Current.MapContainer.GetField(location.X, location.Y, location.MapLevel);

    // MoveT.PAS:2019
    public virtual MoveStatus ProcessMove()
    {
        MoveStatus result;
        if (MoveIndex == 1)
        {
            result = LastMove();
        }
        else
        {
            if (MoveIndex > 1 && MoveIndex < Path.Count)
            {
                result = DefaultMove();
            }
            else
            {
                result = MoveStatus.Done;
            }
        }

        return result;
    }

    // MoveT.PAS:2004
    public virtual bool Validate() => true;

    // MoveT.PAS:2010
    public virtual void FinishMove()
    {
        Status = MoveStatus.Finished;
    }

    // MoveT.PAS:1997
    public virtual MoveStatus Move()
    {
        MoveStatus result;
        MoveIndex -= 1;
        if (MoveIndex == 0)
        {
            result = MoveStatus.Done;
        }
        else
        {
            result = MoveStatus.Moving;
        }

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
            var item = Path[index];
            var fPathItem = Path[index - 1];
            bool condition = item.MapLevel == fPathItem.MapLevel;
            if (condition)
            {
                var item2 = Path[index - 1];
                var item3 = Path[index - 1];
                var item4 = Path[index];
                condition = HexGeometry.DeltaHexToRadius(Path[index].X, item4.Y, item3.X, item2.Y) == 1;
            }

            if (condition)
            {
                var item2 = Path[index - 1];
                var item3 = Path[index - 1];
                var item4 = Path[index];
                result = HexGeometry.DeltaHexToHexNumberFast(Path[index].X, item4.Y, item3.X, item2.Y);
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
            var item = Path[index - 1];
            var fPathItem = Path[index - 1];
            var item2 = Path[index];
            result = HexGeometry.DeltaHexToHexDirection(Path[index].X, item2.Y, fPathItem.X, item.Y);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // intf_P5_07.inc:765

}
