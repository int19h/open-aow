// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// children. group control
public partial class AIGroupControlTargetList : EReferenceNode
{
    public AIGroupControlTargetList(ECustomNode owner) : base(owner)
    { }

    public AIGroupControl Control { get; set; }

    // AIGroup.pas:684
    public AIGroupControlTarget GetTargets(int index)
    {
        Assert(index >= 0 && index < _children.Count, "", "AIGroup.pas", 687);
        return (AIGroupControlTarget)_children.List[index];
    }

    // AIGroup.pas:696
    public void SortOnPriority()
    {
        _children.Sort(CompareAIGroupTargetPriority);
    }

    // AIGroup.pas:706
    public void SortOnDataB()
    {
        _children.Sort(CompareAIGroupTargetDataB);
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
            target = (AIGroupControlTarget)_children.List[I];
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
            if (location == ((AIGroupControlTarget)_children.List[result]).Location.Packed)
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
