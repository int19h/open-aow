// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World;

public partial class MapObjectResourceList : EResourceList
{
    public MapObjectResourceList(ECustomNode owner) : base(owner)
    { }

    // MAPOBJ.PAS:641
    public MapObjectResource GetMapObjectResource(int index) => (MapObjectResource)_children.List[index];

    // MAPOBJ.PAS:646
    public bool FindMapObjectResource(int classId, TerrainList terrainList, Set<TerrainCompareModeElement> mode, ref DelphiList list)
    {
        MapObjectResource R;
        int[] iCell = new int[1];
        ref int I = ref iCell[0];
        // { Preserve the original stack locals. }
        bool result = false;
        I = 0;
        int last = _children.Count - 1;
        for (; I <= last; I += 1)
        {
            R = (MapObjectResource)_children.List[I];
            if (R.EngineClass.ClassId == classId)
            {
                if (terrainList == null || R._terrainList.CompareTerrain(terrainList, mode))
                {
                    result = true;
                    list.Add(R);
                }
            }
        }

        return result;
    }

    public new sealed class ResourcesAccessor(MapObjectResourceList target)
    {
        public MapObjectResource this[int index] { get => target.GetMapObjectResource(index); }
    }

    public new ResourcesAccessor Resources => new ResourcesAccessor(this);

}
