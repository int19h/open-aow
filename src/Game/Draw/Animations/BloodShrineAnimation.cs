// Translated from BloodShrine.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Draw.Animations;

public partial class BloodShrineAnimation : Animation
{
    public BloodShrineAnimation(ECustomNode owner) : base(owner)
    { }

    // BloodShrine.pas:56
    public override void NewFrame()
    {
        base.NewFrame();
        if (FrameCounter > 20)
        {
            Dispose();
        }
    }

    // Animation.pas:43

    // BloodShrine.pas:53
    public override Set<AnimationControlModeElement> AnimationControlMode() => Set<AnimationControlModeElement>.Of(AnimationControlModeElement.LinkToNewFrame, AnimationControlModeElement.OwnedByMap);

}
