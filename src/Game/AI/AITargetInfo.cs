// Translated from AoWE.pas.
namespace Aow.Game.AI;

public struct AITargetInfo
{
    public sbyte Player { get; internal set; }
    public sbyte IndependentRelation { get; internal set; }
    public AITargetMode Mode { get; internal set; }
}
