// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Session;

public partial class DiplomaticRelationHistoryList(CustomNode owner) : OwningNode(owner), IChildList<DiplomaticRelationHistoryItem>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a DiplomaticRelationHistoryItem, so the cast never fails.
    public DiplomaticRelationHistoryItem this[int index] => GetChild<DiplomaticRelationHistoryItem>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is DiplomaticRelationHistoryItem;

    int IChildSink<DiplomaticRelationHistoryItem>.AddChild(DiplomaticRelationHistoryItem child) => AddChild(child);

    public new const int ClassId = 131355;

    // Player.PAS:45
    public DiplomaticRelationHistoryItem GetItems(int index) => GetChildInRange<DiplomaticRelationHistoryItem>(index);

    // Player.PAS:50
    public int GetLastModifiedOnDay()
    {
        int result = Count != 0 ? GetItems(Count - 1).Day : 0;
        return result;
    }

    // Player.PAS:56
    public DiplomaticRelation GetRelationOnDay(int day)
    {
        int i;
        DiplomaticRelation result = DiplomaticRelation.Unknown;
        i = 0;
        while (i < Count && GetItems(i).Day <= day)
        {
            result = GetItems(i).Relation;
            i += 1;
        }

        return result;
    }

    // Player.PAS:69
    public void Add(DiplomaticRelation relation, int day)
    {
        _ = new DiplomaticRelationHistoryItem(this)
        {
            Day = day,
            Relation = relation
        };
    }

    // Engine.pas:631

}
