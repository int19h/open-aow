// Translated from AoWE.pas.
namespace Aow.Game.AI;

public struct AITargetData
{
    public int Priority { get; internal set; }
    public AITargetType TargetType { get; internal set; }
    public int Cost { get; internal set; }
}
