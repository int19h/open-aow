#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class UnholyChampionAbility : EnhancementAbility
{
    // PassiveAb.pas:499
    public UnholyChampionAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.UnholyChampionText);
        AbilityId = 147;
        SelectionTypes = AbilitySelectionTypes.Unit | AbilitySelectionTypes.HeadItem | AbilitySelectionTypes.TorsoItem | AbilitySelectionTypes.DefenseItem | AbilitySelectionTypes.RingItem | AbilitySelectionTypes.Editor;
    }

    // PassiveAb.pas:507
    public override bool CanExpand(AbilityOwner owner)
    {
        bool result = base.CanExpand(owner);
        return result && owner is Troop troop ? AlignmentTables.PureEvilAlignments.Contains(troop.GetAlignment().ToFlag()) : result;
    }
}
