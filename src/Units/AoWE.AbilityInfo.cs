using Aow.Rtl;

namespace Aow.Units.AoWE;

public partial class TAbstractUnit
{
    // Unit.PAS:713. TP2AbilityCalls is a layout view, not an instance type.
    public void ListAbilityInfo(TStrings names, TStrings types, TStrings sources)
    {
        names.Clear();
        sources?.Clear();
        for (int i = 0; i < GetAbilityCount(); i++)
        {
            if (!GetAbilitySet(i)) continue;
            var owner = GetAbilityOwner(i);
            var ability = AoWE.AoWHSSet.FAbilityControl.GetAbility(i);
            var name = ability.GetName(owner);
            if (name != "") names.AddObject(name, Rtl.Rtl.IntToObject(i));
            types?.AddObject(AoWE.TranslateRStr(AoWE.TAbilityTypeRStr[(int)ability.GetAbilityType()]), Rtl.Rtl.IntToObject(i));
            var source = ability.GetSourceName(owner);
            sources?.AddObject(source, Rtl.Rtl.IntToObject(i));
        }
    }
}
