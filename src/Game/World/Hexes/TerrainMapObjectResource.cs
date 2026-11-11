// Translated from Terrain.pas.
using System.Runtime.CompilerServices;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.World.Hexes;

// FDecorationType (-1 none), FTerrainType. persisted as ids $12 and $11
public partial class TerrainMapObjectResource : MultiHexMapObjectResource
{
    public TerrainMapObjectResource(ECustomNode owner) : base(owner)
    {
        FDecorationType = -1;
    }

    // Terrain.pas:327
    public new const int ClassID = 131431;

    internal sbyte FDecorationType;
    internal byte FTerrainType;

    // Terrain.pas:310
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref FTerrainType, 17);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDecorationType), 18);
    }

    // Terrain.pas:299

    // Terrain.pas:322
    public override int ResourceUserClassID() => 131430;

    public sbyte DecorationType { get => FDecorationType; set => FDecorationType = value; }
    public byte TerrainType { get => FTerrainType; set => FTerrainType = value; }

    // Terrain.pas:42

    // Terrain.pas:317
    public override string ResourceEditName() => TerrainStr;

    private static readonly string TerrainStr = "Terrain";

}
