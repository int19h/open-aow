// Translated from Animation.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw.Animations;

using Aow.Game.Net;

namespace Aow.Game.Commands;

// state, animation
public partial class AnimationTokenEvent : TokenEvent
{
    public AnimationTokenEvent(ECustomNode owner) : base(owner)
    { }

    // Animation.pas:388
    public new const int ClassId = 135433;

    public AnimationTokenEventState State;
    public Animation Animation;

    // Animation.pas:399
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwCreateEObject(ref Animation, null, 15);
    }

    // Animation.pas:405
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result = base.Execute(tokenEvent);
        if (Animation != null)
        {
            if (State == AnimationTokenEventState.Idle)
            {
                if (Animation.PlaceTokenEvent(this, tokenEvent))
                {
                    State = AnimationTokenEventState.Playing;
                }
                else
                {
                    result = ExecuteResult.DoneHalt;
                }
            }

            if (State == AnimationTokenEventState.Playing)
            {
                if (!Animation.ExecuteTokenEvent(this, tokenEvent))
                {
                    Animation.RemoveTokenEvent(this, tokenEvent);
                    State = AnimationTokenEventState.Done;
                }
            }

            if (State == AnimationTokenEventState.Done)
            {
                result = ExecuteResult.Done;
            }
        }
        else
        {
            result = ExecuteResult.Done;
        }

        return result;
    }

    // Animation.pas:393
    public void Initialize(Animation animation)
    {
        if (Animation != null)
        {
            Animation?.Dispose();
        }

        Animation = animation;
    }

}

public enum AnimationTokenEventState : byte
{
    Idle = 0, Playing = 1, Done = 2
}
