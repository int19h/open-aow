// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class UnholyChampionAbility : EnhancementAbility
{
    // PassiveAb.pas:499
    public UnholyChampionAbility(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(GameText.s_unholyChampionText);
        AbilityId = 147;
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.HeadItem, AbilitySelectionType.TorsoItem, AbilitySelectionType.DefenseItem, AbilitySelectionType.RingItem, AbilitySelectionType.Editor);
    }

    // PassiveAb.pas:507
    public override bool CanExpand(AbilityOwner owner)
    {
        bool result = base.CanExpand(owner);
        if (result && owner is Troop)
        {
            result = AlignmentTables.s_pureEvilAlignments.Contains(((Troop)owner).GetAlignment());
        }

        return result;
    }
}
