// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Map;

// The layout of TMapField up to its terrain and overlay types, which THSMap.CanChangeTerrain
// saves and restores as one Word (the original reads word ptr [MapF])
public partial class MapFieldTandO : ECustomNode
{
    public MapFieldTandO(ECustomNode owner) : base(owner)
    { }

    internal object FHSList;
    internal int FCounts;
    internal int FXYL;
    internal ushort FTandO;
}
