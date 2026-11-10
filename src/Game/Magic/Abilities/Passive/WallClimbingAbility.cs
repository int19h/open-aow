// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class WallClimbingAbility : AdjustableAbility
{
    // PassiveAb.pas:922
    public WallClimbingAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(WallClimbingRStr);
        FAbilityID = 36;
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.HeadItem, AbilitySelectionType.TorsoItem, AbilitySelectionType.DefenseItem, AbilitySelectionType.RingItem, AbilitySelectionType.Editor);
    }

    // PassiveAb.pas:931
    public override Set<WallCombatFeaturesElement> GetWallCombatFeatures(AbilityOwner Owner) => Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallPassing, WallCombatFeaturesElement.Participate);

    private static readonly string WallClimbingRStr = "Wall Climbing";
}
