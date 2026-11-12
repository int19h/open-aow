
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Combat;

public partial class CombatPredictorUnit
{
    public void Setup(Troop troop, CombatPredictor predictor, byte side)
    {
        Troop = troop;
        troop.AddRef();
        _predictor = predictor;
        CpuId = predictor.CreateUniqueCpuId();
        Side = side;
        InitialHitPoints = troop.GetHitPoints() * 100;
        Enabled = 1;
        _targetsInvalid = 1;
        _wallTargetsInvalid = 1;
        _opponents = predictor.Sides[side == 0 ? 1 : 0];
    }
}
