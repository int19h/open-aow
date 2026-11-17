// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public partial class GroupControl(CustomNode owner) : EngineObject(owner)
{

    // AIGroup.pas:1565
    public virtual GroupBehavior Behavior() => GroupBehavior.None;
}
