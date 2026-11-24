// Translated from AoWE.pas.
using Aow.Game.Session;

namespace Aow.Game.AI;

/// <summary>The default value holds no player. Construction goes through <c>new AITargetInfo()</c>.</summary>
public struct AITargetInfo
{
    public PlayerId? Player { get; internal set; } = PlayerId.Independent;
    public sbyte IndependentRelation { get; internal set; }
    public AITargetMode Mode { get; internal set; }

    public AITargetInfo() { }
}
