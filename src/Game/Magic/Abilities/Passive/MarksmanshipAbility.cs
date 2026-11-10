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
        FAbilityName = AbilityRules.TranslateRStr(MarksmanshipRStr);
        FAbilityType = AbilityType.Enhancement;
        FAbilityID = 32;
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.HeadItem, AbilitySelectionType.TorsoItem, AbilitySelectionType.DefenseItem, AbilitySelectionType.RingItem, AbilitySelectionType.Editor);
        FMaxLevel = 4;
        FLevelCosts[1] = 5;
        FLevelCosts[2] = 5;
        FLevelCosts[3] = 5;
        FLevelCosts[4] = 5;
    }

    // PassiveAb.pas:1398
    public override string GetLevelName(int Level)
    {
        string Result = "";
        switch (Level)
        {
            case 0:
                Result = AbilityRules.TranslateRStr(MarksmanshipRStr);
                break;
            case 1:
                Result = AbilityRules.TranslateRStr(MarksmanshipRStr) + " I";
                break;
            case 2:
                Result = AbilityRules.TranslateRStr(MarksmanshipRStr) + " II";
                break;
            case 3:
                Result = AbilityRules.TranslateRStr(MarksmanshipRStr) + " III";
                break;
            case 4:
                Result = AbilityRules.TranslateRStr(MarksmanshipRStr) + " IV";
                break;
        }

        return Result;
    }

    private static readonly string MarksmanshipRStr = "Marksmanship";
}
