
namespace Aow.Game.World.Hexes;

public partial class UpperIsometricHex
{
    public override void UpdateTransitions() => UpdateTransitionCore(typeof(UpperIsometricHex), false);
}
