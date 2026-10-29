using Aow.Rtl;
using Aow.Units.HSEngine;

namespace Aow.Units.Terrain;

public partial class TTerrainMO
{
    public virtual TTerrainMOR FindTerrainResource(byte terrainType)
    {
        var candidates = new TList();
        var resources = GetResourceList(FResource.Engine).FChildren;
        for (int i = 0; i < resources.Count; i++)
        {
            var item = (Engine.TEObject)resources.List[i];
            if (item.EClass.ClassID != FResource.EClass.ClassID) continue;
            var resource = (TTerrainMOR)item;
            if (resource.FDecorationType == ((TTerrainMOR)FResource).FDecorationType &&
                resource.FTerrainType == terrainType && resource != FResource &&
                resource.TerrainList.CompareTerrain(FResource.TerrainList,
                    TSet<TTerrainCompareModeElement>.Of(TTerrainCompareModeElement.cmSameOverlayType)))
                candidates.Add(resource);
        }
        TTerrainMOR result = candidates.Count == 0 ? null : (TTerrainMOR)candidates[Rtl.Rtl.Random(candidates.Count)];
        candidates.Free();
        return result;
    }

    public TTerrainMOR FindCompatibleResource(byte terrainType, sbyte overlayType, int decorationType)
    {
        var candidates = new TList();
        var resources = GetResourceList(FResource.Engine).FChildren;
        for (int i = 0; i < resources.Count; i++)
        {
            var item = (Engine.TEObject)resources.List[i];
            if (item.EClass.ClassID != FResource.EClass.ClassID) continue;
            var resource = (TTerrainMOR)item;
            if (decorationType != -1 && resource.FDecorationType != decorationType) continue;
            if (resource.FTerrainType != terrainType || resource == FResource ||
                !resource.TerrainList.CompareTerrain(FResource.TerrainList, default)) continue;
            bool compatible = true;
            int j = resource.FTerrainList.Count;
            while (compatible && j != 0)
            {
                j--;
                int overlay = unchecked((sbyte)resource.TerrainList.GetOverlay(j));
                compatible = overlay < 0 || overlay == overlayType;
            }
            if (compatible) candidates.Add(resource);
        }
        TTerrainMOR result = candidates.Count == 0 ? null : (TTerrainMOR)candidates[Rtl.Rtl.Random(candidates.Count)];
        candidates.Free();
        return result;
    }
}
