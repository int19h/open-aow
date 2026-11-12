// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// children. group control
public partial class AIGroupControlTargetList : EReferenceNode, IChildList<AIGroupControlTarget>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AIGroupControlTarget, so the cast never fails.
    public AIGroupControlTarget this[int index] => (AIGroupControlTarget)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIGroupControlTarget;

    int IChildSink<AIGroupControlTarget>.AddChild(AIGroupControlTarget child) => AddChild(child);

    public AIGroupControlTargetList(ECustomNode owner) : base(owner)
    { }

    public AIGroupControl Control { get; set; }

    // AIGroup.pas:684
    public AIGroupControlTarget GetTargets(int index)
    {
        Assert(index >= 0 && index < Count, "", "AIGroup.pas", 687);
        return this[index];
    }

    // AIGroup.pas:696
    public void SortOnPriority()
    {
        SortChildren(CompareAIGroupTargetPriority);
    }

    // AIGroup.pas:706
    public void SortOnDataB()
    {
        SortChildren(CompareAIGroupTargetDataB);
    }

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
        AoWHSMap.Current.AIExecutor.UseProcessTime(count * 20);
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
    public int IndexOfMapCoordinate(int location)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (location == this[result].Location.Packed)
            {
                return result;
            }
        }

        return -1;
    }

    private static int CompareAIGroupTargetPriority(object item1, object item2)
    {
        return ((AIGroupControlTarget)item2).Data.Priority - ((AIGroupControlTarget)item1).Data.Priority;
    }

    private static int CompareAIGroupTargetDataB(object item1, object item2)
    {
        return ((AIGroupControlTarget)item2).DataB - ((AIGroupControlTarget)item1).DataB;
    }

    // Engine.pas:631

    // Engine.pas:631

}
