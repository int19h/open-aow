// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Rtl;

namespace Aow.Game.Session;

// day, relation
public partial class DiplomaticRelationHistoryItem : EngineObject
{
    public new const int ClassId = 131354;

    public DiplomaticRelationHistoryItem(ECustomNode owner) : base(owner)
    { }

    public int Day;
    public DiplomaticRelation Relation;

    // Player.PAS:33
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref Day, 20);
        stream.Bytes(Bytes.Of(ref Relation, 1), 1, 21);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Day = persister.Int32("Day", value: Day);
        Relation = persister.Enum("Relation", value: Relation);
    }

    // Engine.pas:524

}
