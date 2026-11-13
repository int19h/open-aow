// Translated from BehaviorAG.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

// TargetSelector
public partial class BehaviorAIGroupControl : AIGroupControl
{
    public AIGroupTargetSelector TargetSelector { get; }

    // BehaviorAG.pas:150
    public override void Dispose()
    {
        TargetSelector.Release();
        base.Dispose();
    }

    // BehaviorAG.pas:144
    public BehaviorAIGroupControl(ECustomNode owner) : base(owner)
    {
        TargetSelector = new AIGroupTargetSelector(null) { Holder = CreationHolder() };
    }

}
