// Translated from BehaviorAG.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

// FTargetSelector
public partial class BehaviorAIGroupControl : AIGroupControl
{
    public AIGroupTargetSelector FTargetSelector;

    // BehaviorAG.pas:150
    public override void Dispose()
    {
        FTargetSelector.Release();
        base.Dispose();
    }

    // BehaviorAG.pas:144
    public BehaviorAIGroupControl(ECustomNode owner) : base(owner)
    {
        FTargetSelector = new AIGroupTargetSelector(null);
    }

}
