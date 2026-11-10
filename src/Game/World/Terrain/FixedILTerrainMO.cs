// Translated from ILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Terrain;

public partial class FixedILTerrainMO : ILTerrainMO
{

    public FixedILTerrainMO(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 131464;

    // ILTer.pas:295
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

    // ILTer.pas:305
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

    // ILTer.pas:315
    public override void TerrainChanged(in HSTerrainInfo Info)
    {
        if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
        {
            if (ValidTerrainType(Info.TerrainType))
            {
                FTerrainType = Info.TerrainType;
                UpdateTerrain((MapLevel)Owner.Owner, FXhx, FYhx);
                FImageIndex = GetTerrainTypeImage(Info.TerrainType);
            }
            else
            {
                try
                {
                    throw new InvalidOperationException("TILFixedTerrain Failure");
                }
                catch
                {
                    Dispose();
                }
            }
        }
    }

    // ILTer.pas:290
    public override sbyte ForceTerrainType(MapLevel MapLevel, sbyte Xhx, sbyte Yhx) => -1;

}
