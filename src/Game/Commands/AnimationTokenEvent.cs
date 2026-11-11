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
    public new const int ClassID = 135433;

    public State FState;
    public Animation FAnimation;

    // Animation.pas:399
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwCreateEObject(ref FAnimation, null, 15);
    }

    // Animation.pas:405
    public override ExecuteResult Execute(TokenExecutor TE)
    {
        ExecuteResult Result = base.Execute(TE);
        if (FAnimation != null)
        {
            if (FState == State.Idle)
            {
                if (FAnimation.PlaceTE(this, TE))
                {
                    FState = State.Playing;
                }
                else
                {
                    Result = ExecuteResult.DoneHalt;
                }
            }

            if (FState == State.Playing)
            {
                if (!FAnimation.ExecuteTE(this, TE))
                {
                    FAnimation.RemoveTE(this, TE);
                    FState = State.Done;
                }
            }

            if (FState == State.Done)
            {
                Result = ExecuteResult.Done;
            }
        }
        else
        {
            Result = ExecuteResult.Done;
        }

        return Result;
    }

    // Animation.pas:393
    public void Initialize(Animation Animation)
    {
        if (FAnimation != null)
        {
            FAnimation?.Dispose();
        }

        FAnimation = Animation;
    }

    public enum State : byte
    {
        Idle = 0, Playing = 1, Done = 2
    }

}
