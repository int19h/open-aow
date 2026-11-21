#nullable disable warnings
// Translated from BehaviorAG.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

// TargetSelector
public class BehaviorAIGroupControl : AIGroupControl
{
    public AIGroupTargetSelector TargetSelector { get; }

    // BehaviorAG.pas:150
    public override void Dispose()
    {
        _ = TargetSelector.Release();
        base.Dispose();
    }

    // BehaviorAG.pas:144
    public BehaviorAIGroupControl(CustomNode owner) : base(owner) => TargetSelector = new AIGroupTargetSelector(null) { Holder = CreationHolder() };

}
