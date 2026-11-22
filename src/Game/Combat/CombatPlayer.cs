// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Rtl;

namespace Aow.Game.Combat;

public class CombatPlayer(CustomNode owner) : EngineObject(owner)
{
    // Combat2.pas:115
    public new const int ClassId = 131_744;

    public sbyte Player { get; set; }
    public int IndependentRelation { get; set; }
    public int SideIndex { get; set; }

    // Combat2.pas:118
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = persister.UnsignedByte(name: "Player", value: Player);
        IndependentRelation = persister.Byte("IndependentRelation", value: IndependentRelation.TruncateToByte());
        SideIndex = persister.RawByte("SideIndex", value: SideIndex.TruncateToByte());
    }
}
