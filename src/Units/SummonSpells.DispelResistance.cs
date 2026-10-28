using Aow.Units.AoWE;

namespace Aow.Units.SummonSpells;

public partial class TSummonSpell
{
    // SummonSpells.pas:255 calls PassiveAb.pas:1115 on a nil receiver.
    // The native routine reads only the unit argument.
    public int GetDispelResistance(TAbstractUnit unit) => unit is TUnit ? unit.GetUnitLevel() + 3 : 0;
}
