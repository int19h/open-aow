using Aow.Units.IsoHex;

namespace Aow.Units.AoWHex;

public partial class TAoWWaterHexagon
{
    public override void UpdateTransitions() => UpdateTransitionCore(typeof(TLowerIsometricHexagon), true,
        (hn, terrain) => FShoreTerrain[hn - 1] = (byte)terrain);
}
