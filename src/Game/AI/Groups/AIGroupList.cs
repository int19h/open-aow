// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

// children
public partial class AIGroupList : ENode
{
    public AIGroupList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:526
    public AIGroup GetGroups(int index)
    {
        AIGroup result;
        if (index >= 0 && index < Count)
        {
            result = (AIGroup)_children.List[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // AIGroup.pas:533
    public int IndexOfId(int id)
    {
        int result = _children.Count;
        while (result != 0)
        {
            result -= 1;
            if (id == ((AIGroup)_children.List[result]).GroupId)
            {
                return result;
            }
        }

        return -1;
    }

    // AIGroup.pas:544
    public AIGroup FindId(int id)
    {
        AIGroup result;
        int I;
        I = IndexOfId(id);
        if (I != -1)
        {
            result = (AIGroup)_children.List[I];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // AIGroup.pas:559
    public void SortOnProcessPriority()
    {
        _children.Sort(CompareAIGroupProcessPriority);
    }

    private static int CompareAIGroupProcessPriority(object item1, object item2)
    {
        int number = ((AIGroup)item2).GetProcessPriority() - ((AIGroup)item1).GetProcessPriority();
        if (number == 0)
        {
            number = ((AIGroup)item2).GroupId - ((AIGroup)item1).GroupId;
        }
        return number;
    }

    // Engine.pas:631

    // Engine.pas:631

}
