// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

// children
public partial class AIGroupList : ENode
{
    public AIGroupList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:526
    public AIGroup GetGroups(int Index)
    {
        AIGroup Result;
        if (Index >= 0 && Index < Count)
        {
            Result = (AIGroup)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // AIGroup.pas:533
    public int IndexOfID(int ID)
    {
        int Result = FChildren.Count;
        while (Result != 0)
        {
            Result -= 1;
            if (ID == ((AIGroup)FChildren.List[Result]).FGroupID)
            {
                return Result;
            }
        }

        return -1;
    }

    // AIGroup.pas:544
    public AIGroup FindID(int ID)
    {
        AIGroup Result;
        int I;
        I = IndexOfID(ID);
        if (I != -1)
        {
            Result = (AIGroup)FChildren.List[I];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // AIGroup.pas:559
    public void SortOnProcessPriority()
    {
        FChildren.Sort(CompareAIGroupProcessPriority);
    }

    private static int CompareAIGroupProcessPriority(object Item1, object Item2)
    {
        int num = ((AIGroup)Item2).GetProcessPriority() - ((AIGroup)Item1).GetProcessPriority();
        if (num == 0)
        {
            num = ((AIGroup)Item2).FGroupID - ((AIGroup)Item1).FGroupID;
        }
        return num;
    }

    // Engine.pas:631

    // Engine.pas:631

}
