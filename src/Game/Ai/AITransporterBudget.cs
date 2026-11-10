// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Ai;

// Uses TAIBudget fields
public partial class AITransporterBudget : AIBudget
{
    // AI.pas:2198
    public new const int ClassID = 131742;

    // AI.pas:2192
    public AITransporterBudget(ECustomNode owner) : base(owner)
    {
        FDayGoldMax = 15;
    }

    // intf_P5_06.inc:562

}
