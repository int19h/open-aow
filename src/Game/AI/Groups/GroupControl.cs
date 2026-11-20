// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public sealed class GroupControl(CustomNode owner) : EngineObject(owner)
{

    // AIGroup.pas:1565
    public GroupBehavior Behavior() => GroupBehavior.None;
}
