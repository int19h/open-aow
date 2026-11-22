// Translated from CombatILTer.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Rtl;

namespace Aow.Game.World.Terrain;

public class CombatImageLibraryTerrainResource(CustomNode owner) : ImageLibraryTerrainMapObjectResource(owner)
{

    // CombatILTer.pas:175
    public new const int ClassId = 2_228_535;

    public int BurnValue { get; private set; }
    public int Defense { get; private set; }

    // CombatILTer.pas:181
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Defense = persister.UnsignedByte(name: "Defense", value: Defense.TruncateToSByte());
        BurnValue = persister.UnsignedByte(name: "BurnValue", value: BurnValue);
    }

    // CombatILTer.pas:188
    public override int ResourceUserClassId() => CombatImageLibraryTerrain.ClassId;

    // CombatILTer.pas:178
    public override string ResourceEditName() => "Combat ILTerrain";

}
