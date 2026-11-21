// Translated from BloodShrine.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Draw.Animations;

public sealed class BloodShrineAnimation(CustomNode owner) : Animation(owner)
{

    // BloodShrine.pas:56
    public override void NewFrame()
    {
        base.NewFrame();
        if (FrameCounter > 20)
        {
            Dispose();
        }
    }

    // BloodShrine.pas:53
    public override AnimationControlModes AnimationControlMode() => AnimationControlModes.LinkToNewFrame | AnimationControlModes.OwnedByMap;

}
