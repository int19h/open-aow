// Translated from MainStructure.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Terrain;

public class MainStructure(CustomNode owner) : CombatStructure(owner)
{

    // MainStructure.pas:37
    public new const int ClassId = 2_229_532;

    // MainStructure.pas:40
    public override int GetTerrainTypeImage(int terrainType) => terrainType;

    public new class Resource(CustomNode owner) : CombatStructureResource(owner)
    {

        // MainStructure.pas:43
        public new const int ClassId = 2_229_533;

        // MainStructure.pas:49
        public override int ResourceUserClassId() => MainStructure.ClassId;

        // MainStructure.pas:46
        public override string ResourceEditName() => "Combat Main Structure";

    }
}
