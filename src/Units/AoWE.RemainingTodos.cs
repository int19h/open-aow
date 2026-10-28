using Aow.Rtl;

namespace Aow.Units.AoWE;

public partial class TAbilityControl
{
    public void UnregisterAbilityID(int id)
    {
        if (FAbilities[id] is TAbility ability) UnregisterAbility(ability);
    }
}

public partial class TStrikeAbility
{
    public override bool Remove(TAbilityOwner owner)
    {
        if (!owner.GetAbEnabled(21)) return false;
        base.Remove(owner);
        for (int i = owner.GetAbCount() - 1; i >= 0; --i)
            if (owner.GetAbEnabled(i) && AoWE.AoWHSSet.FAbilityControl.GetAbility(i) is TStrikeEnhancementAbility)
                while (AoWE.AoWHSSet.FAbilityControl.GetAbility(i).Remove(owner)) { }
        return true;
    }
}

public partial class TAbstractUnit
{
    // The producer ExecuteLifeMasteryFearRole writes the source marker on duration data.
    public void ResetLifeMasteryFear()
    {
        if (GetAbilityEnabled(53) && ((TDurationAbilityData)GetAbilityData(53)).FSource == 1)
            RemoveAbility(53);
    }
}
