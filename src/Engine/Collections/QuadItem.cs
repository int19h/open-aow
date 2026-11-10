// Translated from Engine.pas.
using Aow.Game.Combat;

namespace Aow.Engine.Collections;

public struct QuadItem<TA> where TA : class
{
    public TA A;
    public int B;
    public int C;
    public int D;

    internal static int P3CompareRoundTargets(in QuadItem<CombatPredictorUnit> A, in QuadItem<CombatPredictorUnit> B) => A.D - B.D;

}
