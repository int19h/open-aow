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

    private AnimationTokenEventState _state;
    private Animation _animation;

    // Animation.pas:399
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Created(ref _animation, null, 15);
    }

    // Animation.pas:405
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result = base.Execute(tokenEvent);
        if (_animation != null)
        {
            if (_state == AnimationTokenEventState.Idle)
            {
                if (_animation.PlaceTokenEvent(this, tokenEvent))
                {
                    _state = AnimationTokenEventState.Playing;
                }
                else
                {
                    result = ExecuteResult.DoneHalt;
                }
            }

            if (_state == AnimationTokenEventState.Playing)
            {
                if (!_animation.ExecuteTokenEvent(this, tokenEvent))
                {
                    _animation.RemoveTokenEvent(this, tokenEvent);
                    _state = AnimationTokenEventState.Done;
                }
            }

            if (_state == AnimationTokenEventState.Done)
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
        if (_animation != null)
        {
            _animation?.Dispose();
        }

        _animation = animation;
    }

}

public enum AnimationTokenEventState : byte
{
    Idle = 0, Playing = 1, Done = 2
}
