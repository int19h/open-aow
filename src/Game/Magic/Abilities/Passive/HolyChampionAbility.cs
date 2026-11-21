#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class HolyChampionAbility : EnhancementAbility
{
    // PassiveAb.pas:484
    public HolyChampionAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.HolyChampionText);
        AbilityId = 146;
        SelectionTypes = AbilitySelectionTypes.Unit | AbilitySelectionTypes.HeadItem | AbilitySelectionTypes.TorsoItem | AbilitySelectionTypes.DefenseItem | AbilitySelectionTypes.RingItem | AbilitySelectionTypes.Editor;
    }

    // PassiveAb.pas:492
    public override bool CanExpand(AbilityOwner owner)
    {
        bool result = base.CanExpand(owner);
        return result && owner is Troop troop ? AlignmentTables.PureGoodAlignments.Contains(troop.GetAlignment().ToFlag()) : result;
    }
}
