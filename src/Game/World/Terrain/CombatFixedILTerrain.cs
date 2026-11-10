// Translated from CombatILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Terrain;

public partial class CombatFixedILTerrain : CombatILTerrain
{
    public CombatFixedILTerrain(ECustomNode owner) : base(owner)
    { }

    // CombatILTer.pas:191
    public new const int ClassID = 2228536;

    // CombatILTer.pas:197
    public override bool CanChangeTerrain(in HSSenderInfo Info)
    {
        bool Result;
        if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
        {
            bool validTerrainType = ValidTerrainType(Info.MapField.TerrainType);
            if (validTerrainType)
            {
                var terrain = FResource.TerrainList.GetTerrain(Info.HN);
                validTerrainType = GetMostFrequentMapFieldTerrainType((MapLevel)Owner.Owner, FXhx, FYhx, terrain) == Info.MapField.TerrainType;
            }

            Result = validTerrainType;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // CombatILTer.pas:206
    public override void ChangeTerrain(in HSTerrainInfo Info)
    {
        if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
        {
            if (ValidTerrainType(Info.TerrainType))
            {
                FTerrainType = Info.TerrainType;
                FImageIndex = GetTerrainTypeImage(Info.TerrainType);
            }
        }
    }

    // CombatILTer.pas:216
    public override void TerrainChanged(in HSTerrainInfo Info)
    {
        if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
        {
            if (ValidTerrainType(Info.TerrainType))
            {
                FTerrainType = Info.TerrainType;
                FImageIndex = GetTerrainTypeImage(Info.TerrainType);
            }
            else
            {
                throw new InvalidOperationException("TILFixedTerrain Failure");
            }
        }
    }

    // CombatILTer.pas:194
    public override sbyte ForceTerrainType(MapLevel MapLevel, sbyte Xhx, sbyte Yhx) => -1;

    public new partial class Resource : CombatILTerrainResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // CombatILTer.pas:227
        public new const int ClassID = 2228537;

        // CombatILTer.pas:233
        public override int ResourceUserClassID() => 2228536;

        // CombatILTer.pas:230
        public override string ResourceEditName() => "Combat FixedILTerrain";

    }

}
