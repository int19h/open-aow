// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World;

public sealed class MapObjectResourceList(CustomNode owner) : ResourceCollection(owner), IChildList<MapObjectResource>
{
    // The map object resource in slot index, read as the original reads the list, without a range check.
    // AcceptsResource admits only a MapObjectResource, so the cast never fails.
    public new MapObjectResource this[int index] => GetChild<MapObjectResource>(index);

    protected sealed override bool AcceptsResource(EngineResource resource) => resource is MapObjectResource;

    int IChildSink<MapObjectResource>.AddChild(MapObjectResource child) => AddChild(child);

    // MAPOBJ.PAS:641
    public MapObjectResource GetMapObjectResource(int index) => this[index];

    // MAPOBJ.PAS:646
    public bool FindMapObjectResource(int classId, TerrainList terrainList, TerrainCompareModes mode, DelphiList<MapObjectResource> list)
    {
        int[] iCell = new int[1];
        ref int i = ref iCell[0];
        // { Preserve the original stack locals. }
        bool result = false;
        i = 0;
        int last = Count - 1;
        for (; i <= last; i += 1)
        {
            MapObjectResource r = this[i];
            if (r.EngineClass.ClassId == classId)
            {
                if (terrainList is null || r.TerrainList.CompareTerrain(terrainList, mode))
                {
                    result = true;
                    _ = list.Add(r);
                }
            }
        }

        return result;
    }

    public sealed class ResourcesAccessor(MapObjectResourceList target)
    {
        public MapObjectResource this[int index] => target.GetMapObjectResource(index);
    }

    public ResourcesAccessor Resources => new(this);

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => ((IEnumerable<MapObjectResource>)this).GetEnumerator();
}
