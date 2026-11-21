// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class WallClimbingAbility : AdjustableAbility
{
    // PassiveAb.pas:922
    public WallClimbingAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_wallClimbingText);
        AbilityId = 36;
        SelectionTypes = AbilitySelectionTypes.Unit | AbilitySelectionTypes.HeadItem | AbilitySelectionTypes.TorsoItem | AbilitySelectionTypes.DefenseItem | AbilitySelectionTypes.RingItem | AbilitySelectionTypes.Editor;
    }

    // PassiveAb.pas:931
    public override WallCombatFeatures GetWallCombatFeatures(AbilityOwner owner) => WallCombatFeatures.WallPassing | WallCombatFeatures.Participate;

    private static readonly string s_wallClimbingText = "Wall Climbing";
}
