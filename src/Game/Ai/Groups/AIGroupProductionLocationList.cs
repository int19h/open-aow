// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Structures;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ai.Groups;

// children
public partial class AIGroupProductionLocationList : EReferenceNode
{
    public AIGroupProductionLocationList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:907
    public AIGroupProductionLocation GetProductionLocation(int Index)
    {
        Assert(Index >= 0 && Index < FChildren.Count, "", "AIGroup.pas", 909);
        return (AIGroupProductionLocation)FChildren.List[Index];
    }

    // AIGroup.pas:919
    public void SortOnMovePoints()
    {
        FChildren.Sort(CompareAIProductionLocationMovePoints);
    }

    // AIGroup.pas:924
    public void Add(AIGroupProductionLocation Location)
    {
        AddChild(Location);
    }

    // AIGroup.pas:929
    public void Delete(int Index)
    {
        RemoveChild(GetProductionLocation(Index));
    }

    // AIGroup.pas:934
    public int IndexOfPC(ProductionControl Control)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (((AIGroupProductionLocation)FChildren.List[Result]).FProductionControl == Control)
            {
                return Result;
            }
        }

        return -1;
    }

    // AIGroup.pas:945
    public AIGroupProductionLocation FindPC(ProductionControl Control)
    {
        AIGroupProductionLocation Result;
        int I;
        I = IndexOfPC(Control);
        if (I != -1)
        {
            Result = (AIGroupProductionLocation)FChildren.List[I];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // AIGroup.pas:956
    public bool Validate(AIGroupControl Control)
    {
        AIGroupProductionLocation Location;
        bool Result = true;
        for (int I = Count - 1; I >= 0; I--)
        {
            Location = (AIGroupProductionLocation)FChildren.List[I];
            if (!Location.Validate(Control))
            {
                RemoveChild(Location);
                Result = false;
            }
        }

        return Result;
    }

    private static int CompareAIProductionLocationMovePoints(object Item1, object Item2)
    {
        return ((AIGroupProductionLocation)Item2).FMovePoints - ((AIGroupProductionLocation)Item1).FMovePoints;
    }

    // Engine.pas:631

    // Engine.pas:631

}
