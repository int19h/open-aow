// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Session;

public partial class DiplomaticRelationHistoryList : ENode
{
    public new const int ClassID = 131355;

    public DiplomaticRelationHistoryList(ECustomNode owner) : base(owner)
    { }

    // Player.PAS:45
    public DiplomaticRelationHistoryItem GetItems(int Index) => (DiplomaticRelationHistoryItem)FChildren.EObjects[Index];

    // Player.PAS:50
    public int GetLastModifiedOnDay()
    {
        int Result;
        if (Count != 0)
        {
            Result = GetItems(Count - 1).FDay;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Player.PAS:56
    public DiplomaticRelation GetRelationOnDay(int Day)
    {
        int I;
        DiplomaticRelation Result = DiplomaticRelation.Unknown;
        I = 0;
        while (I < Count && GetItems(I).FDay <= Day)
        {
            Result = GetItems(I).FRelation;
            I += 1;
        }

        return Result;
    }

    // Player.PAS:69
    public void Add(DiplomaticRelation Relation, int Day)
    {
        var diplomaticRelationHistoryItem = new DiplomaticRelationHistoryItem(this);
        diplomaticRelationHistoryItem.FDay = Day;
        diplomaticRelationHistoryItem.FRelation = Relation;
    }

    // Engine.pas:631

}
