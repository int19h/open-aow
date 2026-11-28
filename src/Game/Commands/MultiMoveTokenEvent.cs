#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Net;

namespace Aow.Game.Commands;

// events, simultaneous
public class MultiMoveTokenEvent : TokenEvent
{
    public new const int ClassId = 135_442;

    public TokenEventList Events { get; }

    public bool Simultaneous { get; set; }

    // MoveT.PAS:3293
    public override void Dispose()
    {
        Events?.Dispose();
        base.Dispose();
    }

    // MoveT.PAS:3302
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("Events", objectValue: Events);
        Simultaneous = persister.Boolean("Simultaneous", value: Simultaneous);
    }

    // MoveT.PAS:3286
    public MultiMoveTokenEvent(CustomNode? owner) : base(owner)
    {
        Events = new TokenEventList(null) { Holder = CreationHolder() };
        Simultaneous = true;
    }

    // MoveT.PAS:3309
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        TokenEventList events = Events;
        bool valid;
        int i;
        if (tokenEvent.ExecuteCount == 0)
        {
            i = events.Count;
            valid = true;
            while (i != 0)
            {
                i -= 1;
                // { The original retains only the final validation result. }
                valid = events[i].Validate();
            }

            tokenEvent.ReportValidated(this, isValid: valid);
        }
        else
        {
            valid = true;
        }

        ExecuteResult moveResult;
        if (valid)
        {
            if (Simultaneous)
            {
                i = events.Count - 1;
                for (; i >= 0; i -= 1)
                {
                    moveResult = events[i].Execute(tokenEvent);
                    if (moveResult is ExecuteResult.Done or ExecuteResult.DoneHalt)
                    {
                        events.Delete(i);
                    }
                }
            }
            else
            {
                moveResult = ExecuteResult.Done;
                while (moveResult is ExecuteResult.Done or ExecuteResult.DoneHalt && Events.Count > 0)
                {
                    moveResult = events[0].Execute(tokenEvent);
                    if (moveResult is ExecuteResult.Done or ExecuteResult.DoneHalt)
                    {
                        events.Delete(0);
                    }
                }
            }
        }

        moveResult = !valid || Events.Count == 0 ? ExecuteResult.Done : ExecuteResult.Retry;

        return moveResult;
    }
}
