// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World;

public partial class MapObjectResourceList : EResourceList, IChildList<MapObjectResource>
{
    // The map object resource in slot index, read as the original reads the list, without a range check.
    // AcceptsResource admits only a MapObjectResource, so the cast never fails.
    public new MapObjectResource this[int index] => (MapObjectResource)GetChild(index);

    protected sealed override bool AcceptsResource(EngineResource resource) => resource is MapObjectResource;

    int IChildSink<MapObjectResource>.AddChild(MapObjectResource child) => AddChild(child);

    public MapObjectResourceList(ECustomNode owner) : base(owner)
    { }

    // MAPOBJ.PAS:641
    public MapObjectResource GetMapObjectResource(int index) => this[index];

    // MAPOBJ.PAS:646
    public bool FindMapObjectResource(int classId, TerrainList terrainList, Set<TerrainCompareModeElement> mode, ref DelphiList list)
    {
        MapObjectResource R;
        int[] iCell = new int[1];
        ref int I = ref iCell[0];
        // { Preserve the original stack locals. }
        bool result = false;
        I = 0;
        int last = Count - 1;
        for (; I <= last; I += 1)
        {
            R = this[I];
            if (R.EngineClass.ClassId == classId)
            {
                if (terrainList == null || R.TerrainList.CompareTerrain(terrainList, mode))
                {
                    result = true;
                    list.Add(R);
                }
            }
        }

        return result;
    }

    public sealed class ResourcesAccessor(MapObjectResourceList target)
    {
        public MapObjectResource this[int index] { get => target.GetMapObjectResource(index); }
    }

    public ResourcesAccessor Resources => new ResourcesAccessor(this);

}
