// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

namespace Aow.Game.Session;

// day, relation
public partial class DiplomaticRelationHistoryItem : EngineObject
{
    public new const int ClassId = 131354;

    public DiplomaticRelationHistoryItem(ECustomNode owner) : base(owner)
    { }

    public int Day { get; set; }
    public DiplomaticRelation Relation { get; set; }

    // Player.PAS:33
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Day = persister.Int32("Day", value: Day);
        Relation = persister.Enum("Relation", value: Relation);
    }

    // Engine.pas:524

}
