// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Structures;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// children
public partial class AIGroupProductionLocationList : EReferenceNode
{
    public AIGroupProductionLocationList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:907
    public AIGroupProductionLocation GetProductionLocation(int index)
    {
        Assert(index >= 0 && index < _children.Count, "", "AIGroup.pas", 909);
        return (AIGroupProductionLocation)_children.List[index];
    }

    // AIGroup.pas:919
    public void SortOnMovePoints()
    {
        _children.Sort(CompareAIProductionLocationMovePoints);
    }

    // AIGroup.pas:924
    public void Add(AIGroupProductionLocation location)
    {
        AddChild(location);
    }

    // AIGroup.pas:929
    public void Delete(int index)
    {
        RemoveChild(GetProductionLocation(index));
    }

    // AIGroup.pas:934
    public int IndexOfPC(ProductionControl control)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (((AIGroupProductionLocation)_children.List[result]).ProductionControl == control)
            {
                return result;
            }
        }

        return -1;
    }

    // AIGroup.pas:945
    public AIGroupProductionLocation FindPC(ProductionControl control)
    {
        AIGroupProductionLocation result;
        int I;
        I = IndexOfPC(control);
        if (I != -1)
        {
            result = (AIGroupProductionLocation)_children.List[I];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // AIGroup.pas:956
    public bool Validate(AIGroupControl control)
    {
        AIGroupProductionLocation location;
        bool result = true;
        for (int I = Count - 1; I >= 0; I--)
        {
            location = (AIGroupProductionLocation)_children.List[I];
            if (!location.Validate(control))
            {
                RemoveChild(location);
                result = false;
            }
        }

        return result;
    }

    private static int CompareAIProductionLocationMovePoints(object item1, object item2)
    {
        return ((AIGroupProductionLocation)item2).MovePoints - ((AIGroupProductionLocation)item1).MovePoints;
    }

    // Engine.pas:631

    // Engine.pas:631

}
