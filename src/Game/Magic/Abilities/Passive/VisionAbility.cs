// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type. maximum level, level costs
public class VisionAbility : MultiLevelAbility
{
    // PassiveAb.pas:461
    public VisionAbility(CustomNode owner) : base(owner)
    {
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.HeadItem, AbilitySelectionType.TorsoItem, AbilitySelectionType.DefenseItem, AbilitySelectionType.RingItem, AbilitySelectionType.Editor);
        MaxLevel = 4;
        AbilityId = 64;
        LevelCosts[1] = 5;
        LevelCosts[2] = 5;
        LevelCosts[3] = 5;
        LevelCosts[4] = 5;
    }

    // PassiveAb.pas:473
    public override string GetLevelName(int level)
    {
        return level switch
        {
            0 => AbilityRules.TranslateResourceString(GetEngine(), s_visionText),
            1 => $"{AbilityRules.TranslateResourceString(GetEngine(), s_visionText)} I",
            2 => $"{AbilityRules.TranslateResourceString(GetEngine(), s_visionText)} II",
            3 => $"{AbilityRules.TranslateResourceString(GetEngine(), s_visionText)} III",
            4 => $"{AbilityRules.TranslateResourceString(GetEngine(), s_visionText)} IV",
            _ => "",
        };
    }

    private static readonly string s_visionText = "Vision";
}
