// Translated from Terrain.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

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
        TerrainType = persister.Byte("TerrainType", value: TerrainType);
        DecorationType = PersistSupport.UnsignedByte(persister, name: "DecorationType", value: DecorationType);
    }

    // Terrain.pas:322
    public override int ResourceUserClassId() => TerrainMapObject.ClassId;

    public sbyte DecorationType { get; set; } = -1;
    public byte TerrainType { get; set; }

    // Terrain.pas:317
    public override string ResourceEditName() => s_terrainText;

    private static readonly string s_terrainText = "Terrain";

}
