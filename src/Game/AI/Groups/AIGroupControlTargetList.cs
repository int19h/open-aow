// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

// children. group control
public partial class AIGroupControlTargetList : EReferenceNode, IChildList<AIGroupControlTarget>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AIGroupControlTarget, so the cast never fails.
    public AIGroupControlTarget this[int index] => Targets[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIGroupControlTarget;

    int IChildSink<AIGroupControlTarget>.AddChild(AIGroupControlTarget child) => AddChild(child);

    public AIGroupControlTargetList(ECustomNode owner) : base(owner)
    { }

    public AIGroupControl Control { get; set; }

    /// <summary>The targets of this list, in their stored order.</summary>
    public AIGroupControlTargetCollection Targets => new(this);

    // AIGroup.pas:684
    public AIGroupControlTarget GetTargets(int index) => Targets.GetTargets(index);

    // AIGroup.pas:696
    public void SortOnPriority() => Targets.SortOnPriority();

    // AIGroup.pas:706
    public void SortOnDataB() => Targets.SortOnDataB();

    // AIGroup.pas:711
    public void Add(AIGroupControlTarget target)
    {
        AddChild(target);
    }

    // AIGroup.pas:716
    public void Delete(int index)
    {
        RemoveChild(GetTargets(index));
    }

    // AIGroup.pas:721
    public bool Validate()
    {
        AIGroupControlTarget target;
        bool result = true;
        var count = Count;
        StrategicMap.Current.AIExecutor.UseProcessTime(count * 20);
        for (int I = Count - 1; I >= 0; I--)
        {
            target = this[I];
            if (!target.Validate(Control))
            {
                RemoveChild(target);
                result = false;
            }
        }

        return result;
    }

    // AIGroup.pas:739
    public int IndexOfMapCoordinate(MapCoordinate location) => Targets.IndexOfMapCoordinate(location);

    // Engine.pas:631

    // Engine.pas:631

}
