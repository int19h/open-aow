// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.Commands;

// status, path, move index
public partial class MoveTE : TokenEvent
{
    public MoveTE(ECustomNode owner) : base(owner)
    {
        FPath = new Movepath();
        FMoveIndex = 0;
        FStatus = MoveStatus.Idle;
    }

    public MoveStatus FStatus;
    public Movepath FPath;
    public int FMoveIndex;

    // MoveT.PAS:1958
    public override void Dispose()
    {
        FPath?.Dispose();
        base.Dispose();
    }

    // MoveT.PAS:1964
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStreamProc(20, FPath.LoadFromStream, FPath.SaveToStream);
    }

    // MoveT.PAS:2027
    public override ExecuteResult Execute(TokenExecuter TE)
    {
        ExecuteResult Result;
        base.Execute(TE);
        if (FStatus == MoveStatus.Idle)
        {
            if (Validate())
            {
                FStatus = MoveStatus.Moving;
                InitializeMove();
                AoWHSMap.Current.Random(1048575);
                switch (FPath.Count)
                {
                    case >= 0 and <= 1:
                        FStatus = MoveStatus.Done;
                        break;
                    case 2:
                        FStatus = LastMove();
                        break;
                    default:
                        FStatus = DefaultMove();
                        break;
                }
            }
            else
            {
                FStatus = MoveStatus.Failed;
            }
        }
        else
        {
            if (FStatus == MoveStatus.Moving)
            {
                FStatus = ProcessMove();
            }
        }

        if (FStatus == MoveStatus.Done)
        {
            FinishMove();
            Result = ExecuteResult.Done;
        }
        else
        {
            if (FStatus == MoveStatus.Failed)
            {
                FinishMove();
                Result = ExecuteResult.DoneHalt;
            }
            else
            {
                if (FStatus == MoveStatus.Finished)
                {
                    Result = ExecuteResult.Done;
                }
                else
                {
                    Result = ExecuteResult.Retry;
                }
            }
        }

        return Result;
    }

    // MoveT.PAS:1970
    public virtual AoWMapField GetMapF(XYL Location) => (AoWMapField)AoWHSMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.L);

    // MoveT.PAS:2019
    public virtual MoveStatus ProcessMove()
    {
        MoveStatus Result;
        if (FMoveIndex == 1)
        {
            Result = LastMove();
        }
        else
        {
            if (FMoveIndex > 1 && FMoveIndex < FPath.Count)
            {
                Result = DefaultMove();
            }
            else
            {
                Result = MoveStatus.Done;
            }
        }

        return Result;
    }

    // MoveT.PAS:2004
    public virtual bool Validate() => true;

    // MoveT.PAS:2010
    public virtual void FinishMove()
    {
        FStatus = MoveStatus.Finished;
    }

    // MoveT.PAS:1997
    public virtual MoveStatus Move()
    {
        MoveStatus Result;
        FMoveIndex -= 1;
        if (FMoveIndex == 0)
        {
            Result = MoveStatus.Done;
        }
        else
        {
            Result = MoveStatus.Moving;
        }

        return Result;
    }

    // MoveT.PAS:2016
    public virtual MoveStatus LastMove() => DefaultMove();

    // MoveT.PAS:2013
    public virtual MoveStatus DefaultMove() => Move();

    // MoveT.PAS:1975
    public int GetDirection(int Index)
    {
        int Result;
        if (Index > 0 && Index < FPath.Count)
        {
            var item = FPath[Index];
            var fPathItem = FPath[Index - 1];
            bool condition = item.L == fPathItem.L;
            if (condition)
            {
                var item2 = FPath[Index - 1];
                var item3 = FPath[Index - 1];
                var item4 = FPath[Index];
                condition = HexGeometry.dHXtoRad(FPath[Index].X, item4.Y, item3.X, item2.Y) == 1;
            }

            if (condition)
            {
                var item2 = FPath[Index - 1];
                var item3 = FPath[Index - 1];
                var item4 = FPath[Index];
                Result = HexGeometry.dHXtoHNfast(FPath[Index].X, item4.Y, item3.X, item2.Y);
            }
            else
            {
                Result = 0;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // MoveT.PAS:1989
    public int GetHT(int Index)
    {
        int Result;
        if (Index > 0 && Index < FPath.Count)
        {
            var item = FPath[Index - 1];
            var fPathItem = FPath[Index - 1];
            var item2 = FPath[Index];
            Result = HexGeometry.dHXtoHT(FPath[Index].X, item2.Y, fPathItem.X, item.Y);
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // intf_P5_07.inc:765

}
