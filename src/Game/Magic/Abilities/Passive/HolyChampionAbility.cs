// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class HolyChampionAbility : EnhancementAbility
{
    // PassiveAb.pas:484
    public HolyChampionAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), GameText.HolyChampionText);
        AbilityId = 146;
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.HeadItem, AbilitySelectionType.TorsoItem, AbilitySelectionType.DefenseItem, AbilitySelectionType.RingItem, AbilitySelectionType.Editor);
    }

    // PassiveAb.pas:492
    public override bool CanExpand(AbilityOwner owner)
    {
        bool result = base.CanExpand(owner);
        return result && owner is Troop troop ? AlignmentTables.PureGoodAlignments.Contains(troop.GetAlignment()) : result;
    }
}
