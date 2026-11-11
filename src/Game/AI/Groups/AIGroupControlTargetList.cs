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

    public AIGroupControl FControl;

    // AIGroup.pas:684
    public AIGroupControlTarget GetTargets(int Index)
    {
        Assert(Index >= 0 && Index < FChildren.Count, "", "AIGroup.pas", 687);
        return (AIGroupControlTarget)FChildren.List[Index];
    }

    // AIGroup.pas:696
    public void SortOnPriority()
    {
        FChildren.Sort(CompareAIGroupTargetPriority);
    }

    // AIGroup.pas:706
    public void SortOnDataB()
    {
        FChildren.Sort(CompareAIGroupTargetDataB);
    }

    // AIGroup.pas:711
    public void Add(AIGroupControlTarget Target)
    {
        AddChild(Target);
    }

    // AIGroup.pas:716
    public void Delete(int Index)
    {
        RemoveChild(GetTargets(Index));
    }

    // AIGroup.pas:721
    public bool Validate()
    {
        AIGroupControlTarget Target;
        bool Result = true;
        var count = Count;
        AoWHSMap.Current.FAIExecuter.UseProcessTime(count * 20);
        for (int I = Count - 1; I >= 0; I--)
        {
            Target = (AIGroupControlTarget)FChildren.List[I];
            if (!Target.Validate(FControl))
            {
                RemoveChild(Target);
                Result = false;
            }
        }

        return Result;
    }

    // AIGroup.pas:739
    public int IndexOfXYL(int Location)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (Location == ((AIGroupControlTarget)FChildren.List[Result]).FLocation.Packed)
            {
                return Result;
            }
        }

        return -1;
    }

    private static int CompareAIGroupTargetPriority(object Item1, object Item2)
    {
        return ((AIGroupControlTarget)Item2).FData.Priority - ((AIGroupControlTarget)Item1).FData.Priority;
    }

    private static int CompareAIGroupTargetDataB(object Item1, object Item2)
    {
        return ((AIGroupControlTarget)Item2).FDataB - ((AIGroupControlTarget)Item1).FDataB;
    }

    // Engine.pas:631

    // Engine.pas:631

}
