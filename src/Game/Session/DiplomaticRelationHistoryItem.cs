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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref Day, 20);
        stream.rwData(Bytes.Of(ref Relation, 1), 1, 21);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int32("Day", ref Day);
        persister.Enum("Relation", ref Relation);
    }

    // Engine.pas:524

}
