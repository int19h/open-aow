// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Rtl;

namespace Aow.Game.Session;

// day, relation
public partial class DiplomaticRelationHistoryItem : EObject
{
    public new const int ClassID = 131354;

    public DiplomaticRelationHistoryItem(ECustomNode owner) : base(owner)
    { }

    public int FDay;
    public DiplomaticRelation FRelation;

    // Player.PAS:33
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FDay, 20);
        Stream.rwData(Bytes.Of(ref FRelation, 1), 1, 21);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int("Day", ref FDay);
        persister.Enum("Relation", ref FRelation);
    }

    // Engine.pas:524

}
