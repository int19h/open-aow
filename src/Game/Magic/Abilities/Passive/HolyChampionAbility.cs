// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class HolyChampionAbility : EnhancementAbility
{
    // PassiveAb.pas:484
    public HolyChampionAbility(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(GameText.HolyChampionText);
        AbilityId = 146;
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.HeadItem, AbilitySelectionType.TorsoItem, AbilitySelectionType.DefenseItem, AbilitySelectionType.RingItem, AbilitySelectionType.Editor);
    }

    // PassiveAb.pas:492
    public override bool CanExpand(AbilityOwner owner)
    {
        bool result = base.CanExpand(owner);
        if (result && owner is Troop)
        {
            result = AlignmentTables.PureGoodAlignments.Contains(((Troop)owner).GetAlignment());
        }

        return result;
    }
}
