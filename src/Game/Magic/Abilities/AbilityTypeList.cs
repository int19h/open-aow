// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class AbilityTypeList : DelphiList<Ability>
{
    // Ability.PAS:2033
    public Ability GetAbility(int Index) => List[Index];

    // System.pas:111

}
