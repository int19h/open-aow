using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Hexes;

public partial class TerrainMapObject
{
    public virtual TerrainMapObjectResource FindTerrainResource(byte terrainType)
    {
        var candidates = new DelphiList();
        var resources = GetResourceList(FResource.Engine).FChildren;
        for (int i = 0; i < resources.Count; i++)
        {
            var child = (EngineObject)resources.List[i];
            if (child.EClass.ClassID != FResource.EClass.ClassID) continue;
            var resource = (TerrainMapObjectResource)child;
            if (resource.FDecorationType == ((TerrainMapObjectResource)FResource).FDecorationType &&
                resource.FTerrainType == terrainType && resource != FResource &&
                resource.TerrainList.CompareTerrain(FResource.TerrainList,
                    Set<TerrainCompareModeElement>.Of(TerrainCompareModeElement.SameOverlayType)))
                candidates.Add(resource);
        }
        TerrainMapObjectResource result = candidates.Count == 0 ? null : (TerrainMapObjectResource)candidates[RtlUnit.Random(candidates.Count)];
        candidates?.Dispose();
        return result;
    }

    public TerrainMapObjectResource FindCompatibleResource(byte terrainType, sbyte overlayType, int decorationType)
    {
        var candidates = new DelphiList();
        var resources = GetResourceList(FResource.Engine).FChildren;
        for (int i = 0; i < resources.Count; i++)
        {
            var child = (EngineObject)resources.List[i];
            if (child.EClass.ClassID != FResource.EClass.ClassID) continue;
            var resource = (TerrainMapObjectResource)child;
            if (decorationType != -1 && resource.FDecorationType != decorationType) continue;
            if (resource.FTerrainType != terrainType || resource == FResource ||
                !resource.TerrainList.CompareTerrain(FResource.TerrainList, default)) continue;
            bool compatible = true;
            int j = resource.FTerrainList.Count;
            while (compatible && j != 0)
            {
                j--;
                int overlay = resource.TerrainList.GetOverlay(j);
                compatible = overlay < 0 || overlay == overlayType;
            }
            if (compatible) candidates.Add(resource);
        }
        TerrainMapObjectResource result = candidates.Count == 0 ? null : (TerrainMapObjectResource)candidates[RtlUnit.Random(candidates.Count)];
        candidates?.Dispose();
        return result;
    }
}
