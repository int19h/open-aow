using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Hexes;

public partial class TerrainMapObject
{
    public virtual TerrainMapObjectResource FindTerrainResource(byte terrainType)
    {
        var candidates = new DelphiList();
        var resources = GetResourceList(_resource.Engine)._children;
        for (int i = 0; i < resources.Count; i++)
        {
            var child = (EngineObject)resources.List[i];
            if (child.EngineClass.ClassId != _resource.EngineClass.ClassId) continue;
            var resource = (TerrainMapObjectResource)child;
            if (resource._decorationType == ((TerrainMapObjectResource)_resource)._decorationType &&
                resource._terrainType == terrainType && resource != _resource &&
                resource.TerrainList.CompareTerrain(_resource.TerrainList,
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
        var resources = GetResourceList(_resource.Engine)._children;
        for (int i = 0; i < resources.Count; i++)
        {
            var child = (EngineObject)resources.List[i];
            if (child.EngineClass.ClassId != _resource.EngineClass.ClassId) continue;
            var resource = (TerrainMapObjectResource)child;
            if (decorationType != -1 && resource._decorationType != decorationType) continue;
            if (resource._terrainType != terrainType || resource == _resource ||
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
        TerrainMapObjectResource result = candidates.Count == 0 ? null : (TerrainMapObjectResource)candidates[RtlUnit.Random(candidates.Count)];
        candidates?.Dispose();
        return result;
    }
}
