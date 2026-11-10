// Translated from AoWHex.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

public partial class AoWBorderHexagon : BorderHexagon
{
    public AoWBorderHexagon(ECustomNode owner) : base(owner)
    { }

    // AoWHex.pas:261
    public override void UpdateMapField(in HSMapFieldInfo Info)
    {
        if (Info.HN == 0 && AoWHSMap.Current != null && AoWHSMap.Current.FDay == 0)
        {
            ((AoWMapField)(object)Info.MapField).FFieldFlags = ((AoWMapField)(object)Info.MapField).FFieldFlags & -4096;
        }
    }
}
