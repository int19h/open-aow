#nullable disable
// Translated from Animation.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw.Animations;
using Aow.Game.Net;

namespace Aow.Game.Commands;

// state, animation
public class AnimationTokenEvent(CustomNode owner) : TokenEvent(owner)
{

    // Animation.pas:388
    public new const int ClassId = 135433;

    private AnimationTokenEventState _state;
    private Animation _animation;

    // Animation.pas:399
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _animation = persister.Created("Animation", objectValue: _animation);
    }

    // Animation.pas:405
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result = base.Execute(tokenEvent);
        if (_animation is not null)
        {
            if (_state == AnimationTokenEventState.Idle)
            {
                if (_animation.PlaceTokenEvent(this, executor: tokenEvent))
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
                if (!_animation.ExecuteTokenEvent(this, executor: tokenEvent))
                {
                    _ = _animation.RemoveTokenEvent(this, executor: tokenEvent);
                    _state = AnimationTokenEventState.Done;
                }
            }

            if (_state == AnimationTokenEventState.Done)
            {
                return ExecuteResult.Done;
            }
        }
        else
        {
            return ExecuteResult.Done;
        }

        return result;
    }

    // Animation.pas:393
    public void Initialize(Animation animation)
    {
        _animation?.Dispose();

        _animation = animation;
    }
}

public enum AnimationTokenEventState : byte
{
    Idle = 0, Playing = 1, Done = 2
}
