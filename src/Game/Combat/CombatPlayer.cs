// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.Combat;

public partial class CombatPlayer(CustomNode owner) : EngineObject(owner)
{
    // Combat2.pas:115
    public new const int ClassId = 131744;

    public sbyte Player { get; set; }
    public byte IndependentRelation { get; set; }
    public byte SideIndex { get; set; }

    // Combat2.pas:118
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        IndependentRelation = persister.Byte("IndependentRelation", value: IndependentRelation);
        SideIndex = persister.RawByte("SideIndex", value: SideIndex);
    }

    // Engine.pas:524

}
