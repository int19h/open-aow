// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.Units;

// unit index, researched
public partial class UnitProduction(CustomNode owner) : EngineObject(owner)
{
    public new const int ClassId = 131737;

    public int UnitIndex { get; set; }
    public bool Researched { get; set; }

    // Structur.PAS:7
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        UnitIndex = persister.Int32("UnitIndex", value: UnitIndex);
        Researched = persister.Boolean("Researched", value: Researched);
    }

    // Engine.pas:524

}
