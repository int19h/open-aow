// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

using Aow.Rtl;

namespace Aow.Game.Commands;

// events, simultaneous
public partial class MultiMoveTokenEvent : TokenEvent
{
    public new const int ClassId = 135442;

    public TokenEventList Events;
    public bool Simultaneous;

    // MoveT.PAS:3293
    public override void Dispose()
    {
        Events?.Dispose();
        base.Dispose();
    }

    // MoveT.PAS:3302
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwEObject(Events, 20);
        stream.rwBool(ref Simultaneous, 21);
    }

    // MoveT.PAS:3286
    public MultiMoveTokenEvent(ECustomNode owner) : base(owner)
    {
        Events = new TokenEventList(null);
        Simultaneous = true;
    }

    // MoveT.PAS:3309
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        TokenEventList events;
        bool valid;
        ExecuteResult moveResult;
        int I;
        events = Events;
        if (tokenEvent.ExecuteCount == 0)
        {
            I = events.Count;
            valid = true;
            while (I != 0)
            {
                I -= 1;
                // { The original retains only the final validation result. }
                valid = ((MoveTokenEvent)events[I]).Validate();
            }
        }
        else
        {
            valid = true;
        }

        if (valid)
        {
            if (Simultaneous)
            {
                I = events.Count - 1;
                for (; I >= 0; I -= 1)
                {
                    moveResult = events[I].Execute(tokenEvent);
                    if (moveResult == ExecuteResult.Done || moveResult == ExecuteResult.DoneHalt)
                    {
                        events.Delete(I);
                    }
                }
            }
            else
            {
                moveResult = ExecuteResult.Done;
                while (Set<ExecuteResult>.Of(ExecuteResult.Done, ExecuteResult.DoneHalt).Contains(moveResult) && Events.Count > 0)
                {
                    moveResult = events[0].Execute(tokenEvent);
                    if (moveResult == ExecuteResult.Done || moveResult == ExecuteResult.DoneHalt)
                    {
                        events.Delete(0);
                    }
                }
            }
        }

        if (!valid || Events.Count == 0)
        {
            moveResult = ExecuteResult.Done;
        }
        else
        {
            moveResult = ExecuteResult.Retry;
        }

        return moveResult;
    }

    // intf_P5_07.inc:788

}
