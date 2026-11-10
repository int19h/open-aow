// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.Combat;

// inherited list, count
public partial class CombatObjectList : DelphiList
{
    // AIGroup.pas:7352
    public CombatObject GetObject(int Index) => (CombatObject)List[Index];

    // System.pas:111

}
