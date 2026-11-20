// Translated from Spring.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Draw.Animations;

public class SpringOfLifeAnimation(CustomNode owner) : Animation(owner)
{

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
    public override Set<AnimationControlModeElement> AnimationControlMode() => s_springMode;

    private static readonly Set<AnimationControlModeElement> s_springMode = Set<AnimationControlModeElement>.FromMask(17uL);

}
