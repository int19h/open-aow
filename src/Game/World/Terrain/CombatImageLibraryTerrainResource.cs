// Translated from CombatILTer.pas.
using System.Runtime.CompilerServices;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.World.Terrain;

public partial class CombatImageLibraryTerrainResource : ImageLibraryTerrainMapObjectResource
{
    public CombatImageLibraryTerrainResource(ECustomNode owner) : base(owner)
    { }

    // CombatILTer.pas:175
    public new const int ClassID = 2228535;

    public sbyte FBurnValue;
    public sbyte FDefense;

    // CombatILTer.pas:181
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDefense), 20);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FBurnValue), 21);
    }

    // CombatILTer.pas:188
    public override int ResourceUserClassID() => 2228534;

    // CombatILTer.pas:178
    public override string ResourceEditName() => "Combat ILTerrain";

}
