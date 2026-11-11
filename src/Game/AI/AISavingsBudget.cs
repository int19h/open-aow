// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI;

// Uses TAIBudget fields
public partial class AISavingsBudget : AIBudget
{
    // AI.pas:2189
    public new const int ClassID = 131741;

    // AI.pas:2180
    public AISavingsBudget(ECustomNode owner) : base(owner)
    {
        FRequiredGold = 150;
        FRequiredMana = 150;
        FDayGoldMax = 25;
        FDayManaMax = 15;
    }

    // intf_P5_06.inc:555

}
