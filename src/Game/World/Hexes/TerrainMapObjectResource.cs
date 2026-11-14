// Translated from Terrain.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.World.Hexes;

// _decorationType (-1 none), _terrainType. persisted as ids $12 and $11
public partial class TerrainMapObjectResource : MultiHexMapObjectResource
{
    public TerrainMapObjectResource(ECustomNode owner) : base(owner)
    {
        _decorationType = -1;
    }

    // Terrain.pas:327
    public new const int ClassId = 131431;

    private sbyte _decorationType;
    private byte _terrainType;

    // Terrain.pas:310
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _terrainType = persister.Byte("TerrainType", value: _terrainType);
        _decorationType = PersistSupport.UnsignedByte(persister, name: "DecorationType", value: _decorationType);
    }

    // Terrain.pas:299

    // Terrain.pas:322
    public override int ResourceUserClassId() => 131430;

    public sbyte DecorationType { get => _decorationType; set => _decorationType = value; }
    public byte TerrainType { get => _terrainType; set => _terrainType = value; }

    // Terrain.pas:42

    // Terrain.pas:317
    public override string ResourceEditName() => s_terrainText;

    private static readonly string s_terrainText = "Terrain";

}
