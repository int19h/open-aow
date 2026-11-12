// Translated from Terrain.pas.
using System.Runtime.CompilerServices;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

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

    internal sbyte _decorationType;
    internal byte _terrainType;

    // Terrain.pas:310
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref _terrainType, 17);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref _decorationType), 18);
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
