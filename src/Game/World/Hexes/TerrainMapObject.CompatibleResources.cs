using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Hexes;

public partial class TerrainMapObject
{
    public virtual TerrainMapObjectResource FindTerrainResource(byte terrainType)
    {
        var candidates = new DelphiList<TerrainMapObjectResource>();
        var resources = GetResourceList(_resource.Engine);
        for (int i = 0; i < resources.Count; i++)
        {
            var child = resources[i];
            if (child.EngineClass.ClassId != _resource.EngineClass.ClassId) continue;
            var resource = (TerrainMapObjectResource)child;
            if (resource.DecorationType == ((TerrainMapObjectResource)_resource).DecorationType &&
                resource.TerrainType == terrainType && resource != _resource &&
                resource.TerrainList.CompareTerrain(_resource.TerrainList,
                    Set<TerrainCompareModeElement>.Of(TerrainCompareModeElement.SameOverlayType)))
                candidates.Add(resource);
        }
        TerrainMapObjectResource result = candidates.Count == 0 ? null : candidates[RtlUnit.Random(candidates.Count)];
        candidates?.Dispose();
        return result;
    }

    public TerrainMapObjectResource FindCompatibleResource(byte terrainType, sbyte overlayType, int decorationType)
    {
        var candidates = new DelphiList<TerrainMapObjectResource>();
        var resources = GetResourceList(_resource.Engine);
        for (int i = 0; i < resources.Count; i++)
        {
            var child = resources[i];
            if (child.EngineClass.ClassId != _resource.EngineClass.ClassId) continue;
            var resource = (TerrainMapObjectResource)child;
            if (decorationType != -1 && resource.DecorationType != decorationType) continue;
            if (resource.TerrainType != terrainType || resource == _resource ||
                !resource.TerrainList.CompareTerrain(_resource.TerrainList, default)) continue;
            bool compatible = true;
            int j = resource.TerrainList.Count;
            while (compatible && j != 0)
            {
                j--;
                int overlay = resource.TerrainList.GetOverlay(j);
                compatible = overlay < 0 || overlay == overlayType;
            }
            if (compatible) candidates.Add(resource);
        }
        TerrainMapObjectResource result = candidates.Count == 0 ? null : candidates[RtlUnit.Random(candidates.Count)];
        candidates?.Dispose();
        return result;
    }
}
