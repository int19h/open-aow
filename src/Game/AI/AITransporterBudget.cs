// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI;

// Uses TAIBudget fields
public class AITransporterBudget : AIBudget
{
    // AI.pas:2198
    public new const int ClassId = 131_742;

    // AI.pas:2192
    public AITransporterBudget(CustomNode owner) : base(owner) => DayGoldMax = 15;
}
