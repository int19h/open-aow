// Translated from BehaviorAG.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

// FTargetSelector
public partial class AbstractBehaviorAGC : AIGroupControl
{
    public AIGroupTargetSelector FTargetSelector;

    // BehaviorAG.pas:150
    public override void Dispose()
    {
        FTargetSelector.Release();
        base.Dispose();
    }

    // BehaviorAG.pas:144
    public AbstractBehaviorAGC(ECustomNode owner) : base(owner)
    {
        FTargetSelector = new AIGroupTargetSelector(null);
    }

}
