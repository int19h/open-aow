// Translated from BloodShrine.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

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
    public override Set<AnimationControlModeElement> AnimationControlMode() => Set<AnimationControlModeElement>.Of(AnimationControlModeElement.LinkToNewFrame, AnimationControlModeElement.OwnedByMap);

}
