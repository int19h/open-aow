// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class PassWallAbility : AdjustableAbility
{
    // PassiveAb.pas:937
    public PassWallAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(PassWallRStr);
        FAbilityID = 66;
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.HeadItem, AbilitySelectionType.TorsoItem, AbilitySelectionType.DefenseItem, AbilitySelectionType.RingItem);
    }

    // PassiveAb.pas:946
    public override Set<WallCombatFeaturesElement> GetWallCombatFeatures(AbilityOwner Owner) => Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallPassing, WallCombatFeaturesElement.Participate);

    private static readonly string PassWallRStr = "Pass Wall";
}
