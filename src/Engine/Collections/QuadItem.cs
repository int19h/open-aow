// Translated from Engine.pas.
using Aow.Game.Combat;

namespace Aow.Engine.Collections;

public struct QuadItem<TA> where TA : class
{
    public TA First { get; init; }
    public int Second { get; init; }
    public int Third { get; init; }
    public int Fourth { get; set; }

    internal static int CompareRoundTargets(in QuadItem<CombatPredictorUnit> A, in QuadItem<CombatPredictorUnit> B) => A.Fourth - B.Fourth;

}
