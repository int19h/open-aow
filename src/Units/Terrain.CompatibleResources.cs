using Aow.Engine.Nodes;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Hexes;

public partial class TerrainMO
{
    public virtual TerrainMOR FindTerrainResource(byte terrainType)
    {
        var candidates = new DelphiList();
        var resources = GetResourceList(FResource.Engine).FChildren;
        for (int i = 0; i < resources.Count; i++)
        {
            var child = (EObject)resources.List[i];
            if (child.EClass.ClassID != FResource.EClass.ClassID) continue;
            var resource = (TerrainMOR)child;
            if (resource.FDecorationType == ((TerrainMOR)FResource).FDecorationType &&
                resource.FTerrainType == terrainType && resource != FResource &&
                resource.TerrainList.CompareTerrain(FResource.TerrainList,
                    Set<TerrainCompareModeElement>.Of(TerrainCompareModeElement.SameOverlayType)))
                candidates.Add(resource);
        }
        TerrainMOR result = candidates.Count == 0 ? null : (TerrainMOR)candidates[RtlUnit.Random(candidates.Count)];
        candidates?.Dispose();
        return result;
    }

    public TerrainMOR FindCompatibleResource(byte terrainType, sbyte overlayType, int decorationType)
    {
        var candidates = new DelphiList();
        var resources = GetResourceList(FResource.Engine).FChildren;
        for (int i = 0; i < resources.Count; i++)
        {
            var child = (EObject)resources.List[i];
            if (child.EClass.ClassID != FResource.EClass.ClassID) continue;
            var resource = (TerrainMOR)child;
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
        TerrainMOR result = candidates.Count == 0 ? null : (TerrainMOR)candidates[RtlUnit.Random(candidates.Count)];
        candidates?.Dispose();
        return result;
    }
}
