// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class PassWallAbility : AdjustableAbility
{
    // PassiveAb.pas:937
    public PassWallAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_passWallText);
        AbilityId = 66;
        SelectionTypes = AbilitySelectionTypes.Unit | AbilitySelectionTypes.HeadItem | AbilitySelectionTypes.TorsoItem | AbilitySelectionTypes.DefenseItem | AbilitySelectionTypes.RingItem;
    }

    // PassiveAb.pas:946
    public override WallCombatFeatures GetWallCombatFeatures(AbilityOwner owner) => WallCombatFeatures.WallPassing | WallCombatFeatures.Participate;

    private static readonly string s_passWallText = "Pass Wall";
}
