#nullable disable warnings
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Hexes;

public partial class TerrainMapObject
{
    public virtual TerrainMapObjectResource? FindTerrainResource(int terrainType)
    {
        DelphiList<TerrainMapObjectResource> candidates = [];
        MapObjectResourceList resources = GetResourceList(Resource.Engine);
        for (int i = 0; i < resources.Count; i++)
        {
            MapObjectResource child = resources[i];
            if (child is not TerrainMapObjectResource resource || resource.EngineClass.ClassId != Resource.EngineClass.ClassId)
            {
                continue;
            }

            if (resource.DecorationType == Resource.DecorationType &&
                resource.TerrainType == terrainType && resource != Resource &&
                resource.TerrainList.CompareTerrain(Resource.TerrainList,
                    mode: TerrainCompareModes.SameOverlayType))
            {
                _ = candidates.Add(resource);
            }
        }
        TerrainMapObjectResource? result = candidates.Count == 0 ? null : candidates[DelphiRandom.Random(candidates.Count)];
        return result;
    }

    public TerrainMapObjectResource? FindCompatibleResource(int terrainType, int overlayType, int decorationType)
    {
        DelphiList<TerrainMapObjectResource> candidates = [];
        MapObjectResourceList resources = GetResourceList(Resource.Engine);
        for (int i = 0; i < resources.Count; i++)
        {
            MapObjectResource child = resources[i];
            if (child is not TerrainMapObjectResource resource || resource.EngineClass.ClassId != Resource.EngineClass.ClassId)
            {
                continue;
            }

            if (decorationType != -1 && resource.DecorationType != decorationType)
            {
                continue;
            }

            if (resource.TerrainType != terrainType || resource == Resource ||
                !resource.TerrainList.CompareTerrain(Resource.TerrainList, mode: default))
            {
                continue;
            }

            bool compatible = true;
            int j = resource.TerrainList.Count;
            while (compatible && j != 0)
            {
                j--;
                int overlay = resource.TerrainList.GetOverlay(j);
                compatible = overlay < 0 || overlay == overlayType;
            }
            if (compatible)
            {
                _ = candidates.Add(resource);
            }
        }
        TerrainMapObjectResource? result = candidates.Count == 0 ? null : candidates[DelphiRandom.Random(candidates.Count)];
        return result;
    }
}
