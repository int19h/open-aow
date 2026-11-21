// Translated from Spring.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Draw.Animations;

public sealed class SpringOfLifeAnimation(CustomNode owner, ShowAnimationEvent? show = null) : Animation(owner, show: show)
{
    public SpringOfLifeAnimation(CustomNode owner) : this(owner, show: null)
    {
    }

    // Spring.pas:64
    public override void NewFrame()
    {
        base.NewFrame();
        if (FrameCounter > 20)
        {
            Dispose();
        }
    }

    // Spring.pas:59
    public override AnimationControlModes AnimationControlMode() => s_springMode;

    private static readonly AnimationControlModes s_springMode = AnimationControlModes.LinkToNewFrame | AnimationControlModes.OwnedByMap;

}
