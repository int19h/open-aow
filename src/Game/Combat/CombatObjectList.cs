#nullable disable
// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.Combat;

// inherited list, count
public sealed class CombatObjectList : DelphiList<CombatObject>
{
    // AIGroup.pas:7352
    public CombatObject GetObject(int index) => List[index];
}
