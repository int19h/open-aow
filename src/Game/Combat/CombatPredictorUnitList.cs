#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Combat;

public class CombatPredictorUnitList : DelphiList<CombatPredictorUnit>
{
    // CombatPredictor.pas:253
    public CombatPredictorUnit? GetUnits(int index) => List[index];

    // CombatPredictor.pas:256
    public int IndexOfUnit(Troop? troop) => this.FindLastIndex(troop, match: static (sought, unit) => unit.Troop == sought);

    // CombatPredictor.pas:267
    public int IndexOfCpuId(int cpuId) => this.FindLastIndex(cpuId, match: static (sought, unit) => unit.CpuId == sought);
}
