// Translated from Spring.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Draw.Animations;

public partial class SpringOfLifeAnimation : Animation
{
    public SpringOfLifeAnimation(ECustomNode owner) : base(owner)
    { }

    // Spring.pas:64
    public override void NewFrame()
    {
        base.NewFrame();
        if (FFrameCounter > 20)
        {
            Dispose();
        }
    }

    // Animation.pas:43

    // Spring.pas:59
    public override Set<AnimationControlModeElement> AnimationControlMode() => SpringMode;

    private static Set<AnimationControlModeElement> SpringMode = Set<AnimationControlModeElement>.FromMask(17uL);

}
