// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Session;

public partial class DiplomaticRelationHistoryList : ENode, IChildList<DiplomaticRelationHistoryItem>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a DiplomaticRelationHistoryItem, so the cast never fails.
    public DiplomaticRelationHistoryItem this[int index] => (DiplomaticRelationHistoryItem)_children.List[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is DiplomaticRelationHistoryItem;

    int IChildSink<DiplomaticRelationHistoryItem>.AddChild(DiplomaticRelationHistoryItem child) => AddChild(child);

    public new const int ClassId = 131355;

    public DiplomaticRelationHistoryList(ECustomNode owner) : base(owner)
    { }

    // Player.PAS:45
    public DiplomaticRelationHistoryItem GetItems(int index) => (DiplomaticRelationHistoryItem)_children.EObjects[index];

    // Player.PAS:50
    public int GetLastModifiedOnDay()
    {
        int result;
        if (Count != 0)
        {
            result = GetItems(Count - 1).Day;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Player.PAS:56
    public DiplomaticRelation GetRelationOnDay(int day)
    {
        int I;
        DiplomaticRelation result = DiplomaticRelation.Unknown;
        I = 0;
        while (I < Count && GetItems(I).Day <= day)
        {
            result = GetItems(I).Relation;
            I += 1;
        }

        return result;
    }

    // Player.PAS:69
    public void Add(DiplomaticRelation relation, int day)
    {
        var diplomaticRelationHistoryItem = new DiplomaticRelationHistoryItem(this);
        diplomaticRelationHistoryItem.Day = day;
        diplomaticRelationHistoryItem.Relation = relation;
    }

    // Engine.pas:631

}
