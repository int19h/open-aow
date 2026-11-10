
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Combat;

public partial class CombatPredictorUnit
{
    public void Setup(AbstractUnit unit, CombatPredictor predictor, byte side)
    {
        FUnit = unit;
        unit.AddRef();
        FPredictor = predictor;
        FCPUID = predictor.CreateUniqueCPUID();
        FSide = side;
        FInitialHitPoints = unit.GetHitPoints() * 100;
        FEnabled = 1;
        FTargetsInvalid = 1;
        FWallTargetsInvalid = 1;
        FOpponents = predictor.FSides[side == 0 ? 1 : 0];
    }
}
