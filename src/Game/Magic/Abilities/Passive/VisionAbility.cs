// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type. maximum level, level costs
public partial class VisionAbility : MultiLevelAbility
{
    // PassiveAb.pas:461
    public VisionAbility(ECustomNode owner) : base(owner)
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
        string result = "";
        switch (level)
        {
            case 0:
                result = AbilityRules.TranslateResourceString(s_visionText);
                break;
            case 1:
                result = AbilityRules.TranslateResourceString(s_visionText) + " I";
                break;
            case 2:
                result = AbilityRules.TranslateResourceString(s_visionText) + " II";
                break;
            case 3:
                result = AbilityRules.TranslateResourceString(s_visionText) + " III";
                break;
            case 4:
                result = AbilityRules.TranslateResourceString(s_visionText) + " IV";
                break;
        }

        return result;
    }

    private static readonly string s_visionText = "Vision";
}
