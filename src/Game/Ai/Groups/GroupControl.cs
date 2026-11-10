// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class GroupControl : EObject
{
    public GroupControl(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:1565
    public virtual GroupBehavior Behavior() => GroupBehavior.None;
}
