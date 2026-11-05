using Aow.Rtl;

namespace Aow.Units.AoWE;

public partial class TAbilityControl
{
    public TAbility GetAbility(int index)
    {
        Rtl.Rtl.Assert(index >= 0, "Invalid AbilityID", "Ability.PAS", 2069);
        return index < FAbilities.Count ? (TAbility)FAbilities[index] : null;
    }
}
