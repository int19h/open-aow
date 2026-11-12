
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Combat;

public partial class CombatPredictorUnit
{
    public void Setup(Troop troop, CombatPredictor predictor, byte side)
    {
        Troop = troop;
        troop.AddRef();
        Predictor = predictor;
        CpuId = predictor.CreateUniqueCpuId();
        Side = side;
        InitialHitPoints = troop.GetHitPoints() * 100;
        Enabled = 1;
        TargetsInvalid = 1;
        WallTargetsInvalid = 1;
        Opponents = predictor.Sides[side == 0 ? 1 : 0];
    }
}
