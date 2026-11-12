// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.Combat;

// inherited list, count
public partial class CombatObjectList : DelphiList<CombatObject>
{
    // AIGroup.pas:7352
    public CombatObject GetObject(int index) => List[index];

    // System.pas:111

}
