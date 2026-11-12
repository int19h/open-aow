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
    public new const int ClassId = 2228535;

    public sbyte BurnValue;
    public sbyte Defense;

    // CombatILTer.pas:181
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Defense), 20);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref BurnValue), 21);
    }

    // CombatILTer.pas:188
    public override int ResourceUserClassId() => 2228534;

    // CombatILTer.pas:178
    public override string ResourceEditName() => "Combat ILTerrain";

}
