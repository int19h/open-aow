// Translated from Engine.pas.
using Aow.Game.Combat;

namespace Aow.Engine.Collections;

public struct QuadItem<TA> where TA : class
{
    public TA First;
    public int Second;
    public int Third;
    public int Fourth;

    internal static int CompareRoundTargets(in QuadItem<CombatPredictorUnit> A, in QuadItem<CombatPredictorUnit> B) => A.Fourth - B.Fourth;

}
