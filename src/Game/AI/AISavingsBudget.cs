// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI;

// Uses TAIBudget fields
public class AISavingsBudget : AIBudget
{
    // AI.pas:2189
    public new const int ClassId = 131741;

    // AI.pas:2180
    public AISavingsBudget(CustomNode owner) : base(owner)
    {
        RequiredGold = 150;
        RequiredMana = 150;
        DayGoldMax = 25;
        DayManaMax = 15;
    }
}
