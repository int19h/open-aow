// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// (maximum level, level costs.)
public partial class MarksmanshipAbility : MultiLevelAbility
{
    // PassiveAb.pas:1384
    public MarksmanshipAbility(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(s_marksmanshipText);
        AbilityType = AbilityType.Enhancement;
        AbilityId = 32;
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.HeadItem, AbilitySelectionType.TorsoItem, AbilitySelectionType.DefenseItem, AbilitySelectionType.RingItem, AbilitySelectionType.Editor);
        MaxLevel = 4;
        LevelCosts[1] = 5;
        LevelCosts[2] = 5;
        LevelCosts[3] = 5;
        LevelCosts[4] = 5;
    }

    // PassiveAb.pas:1398
    public override string GetLevelName(int level)
    {
        string result = "";
        switch (level)
        {
            case 0:
                result = AbilityRules.TranslateResourceString(s_marksmanshipText);
                break;
            case 1:
                result = AbilityRules.TranslateResourceString(s_marksmanshipText) + " I";
                break;
            case 2:
                result = AbilityRules.TranslateResourceString(s_marksmanshipText) + " II";
                break;
            case 3:
                result = AbilityRules.TranslateResourceString(s_marksmanshipText) + " III";
                break;
            case 4:
                result = AbilityRules.TranslateResourceString(s_marksmanshipText) + " IV";
                break;
        }

        return result;
    }

    private static readonly string s_marksmanshipText = "Marksmanship";
}
