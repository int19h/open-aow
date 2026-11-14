// Translated from CombatILTer.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.World.Terrain;

public partial class CombatImageLibraryTerrainResource : ImageLibraryTerrainMapObjectResource
{
    public CombatImageLibraryTerrainResource(ECustomNode owner) : base(owner)
    { }

    // CombatILTer.pas:175
    public new const int ClassId = 2228535;

    public sbyte BurnValue { get; private set; }
    public sbyte Defense { get; private set; }

    // CombatILTer.pas:181
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Defense = PersistSupport.UnsignedByte(persister, name: "Defense", value: Defense);
        BurnValue = PersistSupport.UnsignedByte(persister, name: "BurnValue", value: BurnValue);
    }

    // CombatILTer.pas:188
    public override int ResourceUserClassId() => 2228534;

    // CombatILTer.pas:178
    public override string ResourceEditName() => "Combat ILTerrain";

}
