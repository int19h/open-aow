// Translated from Terrain.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Rtl;

namespace Aow.Game.World.Hexes;

// _decorationType (-1 none), _terrainType. persisted as ids $12 and $11
public class TerrainMapObjectResource(CustomNode owner) : MultiHexMapObjectResource(owner)
{

    // Terrain.pas:327
    public new const int ClassId = 131_431;

    // Terrain.pas:310
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        TerrainType = persister.Byte("TerrainType", value: TerrainType.TruncateToByte());
        DecorationType = persister.UnsignedByte(name: "DecorationType", value: DecorationType.TruncateToSByte());
    }

    // Terrain.pas:322
    public override int ResourceUserClassId() => TerrainMapObject.ClassId;

    public int DecorationType { get; set; } = -1;
    public int TerrainType { get; set; }

    // Terrain.pas:317
    public override string ResourceEditName() => s_terrainText;

    private static readonly string s_terrainText = "Terrain";

}
