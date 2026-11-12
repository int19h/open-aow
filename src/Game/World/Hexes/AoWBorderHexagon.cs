// Translated from AoWHex.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

public partial class AoWBorderHexagon : BorderHexagon
{
    public AoWBorderHexagon(ECustomNode owner) : base(owner)
    { }

    // AoWHex.pas:261
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (info.HexNumber == 0 && AoWHSMap.Current != null && AoWHSMap.Current.Day == 0)
        {
            ((AoWMapField)(object)info.MapField).FieldFlags = ((AoWMapField)(object)info.MapField).FieldFlags & -4096;
        }
    }
}
