namespace Aow.Units.AoWHex;

public partial class TAoWWaterHexagon
{
    public override void UpdateTransitions() => UpdateTransitionCore(typeof(IsoHex.TLowerIsometricHexagon), true,
        (hn, terrain) => FShoreTerrain[hn - 1] = unchecked((byte)terrain));
}
