// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

using Aow.Rtl;

namespace Aow.Game.Commands;

// events, simultaneous
public partial class MultiMoveTE : TokenEvent
{
    public new const int ClassID = 135442;

    public TokenEventList FEvents;
    public bool FSimultaneous;

    // MoveT.PAS:3293
    public override void Dispose()
    {
        FEvents?.Dispose();
        base.Dispose();
    }

    // MoveT.PAS:3302
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FEvents, 20);
        Stream.rwBool(ref FSimultaneous, 21);
    }

    // MoveT.PAS:3286
    public MultiMoveTE(ECustomNode owner) : base(owner)
    {
        FEvents = new TokenEventList(null);
        FSimultaneous = true;
    }

    // MoveT.PAS:3309
    public override ExecuteResult Execute(TokenExecuter TE)
    {
        TokenEventList Events;
        bool Valid;
        ExecuteResult MoveResult;
        int I;
        Events = FEvents;
        if (TE.ExecuteCount == 0)
        {
            I = Events.Count;
            Valid = true;
            while (I != 0)
            {
                I -= 1;
                // { The original retains only the final validation result. }
                Valid = ((MoveTE)Events[I]).Validate();
            }
        }
        else
        {
            Valid = true;
        }

        if (Valid)
        {
            if (FSimultaneous)
            {
                I = Events.Count - 1;
                for (; I >= 0; I -= 1)
                {
                    MoveResult = Events[I].Execute(TE);
                    if (MoveResult == ExecuteResult.Done || MoveResult == ExecuteResult.DoneHalt)
                    {
                        Events.Delete(I);
                    }
                }
            }
            else
            {
                MoveResult = ExecuteResult.Done;
                while (Set<ExecuteResult>.Of(ExecuteResult.Done, ExecuteResult.DoneHalt).Contains(MoveResult) && FEvents.Count > 0)
                {
                    MoveResult = Events[0].Execute(TE);
                    if (MoveResult == ExecuteResult.Done || MoveResult == ExecuteResult.DoneHalt)
                    {
                        Events.Delete(0);
                    }
                }
            }
        }

        if (!Valid || FEvents.Count == 0)
        {
            MoveResult = ExecuteResult.Done;
        }
        else
        {
            MoveResult = ExecuteResult.Retry;
        }

        return MoveResult;
    }

    // intf_P5_07.inc:788

}
