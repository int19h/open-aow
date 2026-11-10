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
    public MapObjectResource GetMOR(int Index) => (MapObjectResource)FChildren.List[Index];

    // MAPOBJ.PAS:646
    public bool FindMOR(int AClassID, TerrainList TerrainList, Set<TerrainCompareModeElement> Mode, ref DelphiList List)
    {
        MapObjectResource R;
        int[] I__cell = new int[1];
        ref int I = ref I__cell[0];
        // { Preserve the original stack locals. }
        bool Result = false;
        I = 0;
        int last = FChildren.Count - 1;
        for (; I <= last; I += 1)
        {
            R = (MapObjectResource)FChildren.List[I];
            if (R.EClass.ClassID == AClassID)
            {
                if (TerrainList == null || R.FTerrainList.CompareTerrain(TerrainList, Mode))
                {
                    Result = true;
                    List.Add(R);
                }
            }
        }

        return Result;
    }

    public sealed class MORsAccessor(MapObjectResourceList target)
    {
        public MapObjectResource this[int Index] { get => target.GetMOR(Index); }
    }

    public MORsAccessor MORs => new MORsAccessor(this);

}
