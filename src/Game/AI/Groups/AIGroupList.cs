// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

// children
public class AIGroupList(CustomNode owner) : OwningNode(owner), IChildList<AIGroup>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AIGroup, so the cast never fails.
    public AIGroup this[int index] => Groups[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIGroup;

    int IChildSink<AIGroup>.AddChild(AIGroup child) => AddChild(child);

    /// <summary>The groups of this list, in their stored order.</summary>
    public AIGroupCollection Groups => new(this);

    // AIGroup.pas:526
    public AIGroup GetGroups(int index) => Groups.GetGroups(index);

    // AIGroup.pas:533
    public int IndexOfId(int id) => Groups.IndexOfId(id);

    // AIGroup.pas:544
    public AIGroup FindId(int id) => Groups.FindId(id);

    // AIGroup.pas:559
    public void SortOnProcessPriority() => Groups.SortOnProcessPriority();
}
