// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Structures;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// children
public partial class AIGroupProductionLocationList : EReferenceNode, IChildList<AIGroupProductionLocation>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AIGroupProductionLocation, so the cast never fails.
    public AIGroupProductionLocation this[int index] => (AIGroupProductionLocation)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIGroupProductionLocation;

    int IChildSink<AIGroupProductionLocation>.AddChild(AIGroupProductionLocation child) => AddChild(child);

    public AIGroupProductionLocationList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:907
    public AIGroupProductionLocation GetProductionLocation(int index)
    {
        Assert(index >= 0 && index < Count, "", "AIGroup.pas", 909);
        return this[index];
    }

    // AIGroup.pas:919
    public void SortOnMovePoints()
    {
        SortChildren<AIGroupProductionLocation>(CompareAIProductionLocationMovePoints);
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
            if (this[result].ProductionControl == control)
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
            result = this[I];
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
            location = this[I];
            if (!location.Validate(control))
            {
                RemoveChild(location);
                result = false;
            }
        }

        return result;
    }

    private static int CompareAIProductionLocationMovePoints(AIGroupProductionLocation item1, AIGroupProductionLocation item2)
    {
        return item2.MovePoints - item1.MovePoints;
    }

    // Engine.pas:631

    // Engine.pas:631

}
