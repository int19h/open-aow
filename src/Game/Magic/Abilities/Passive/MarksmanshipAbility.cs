// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// (maximum level, level costs.)
public partial class MarksmanshipAbility : MultiLevelAbility
{
    // PassiveAb.pas:1384
    public MarksmanshipAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_marksmanshipText);
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
        return level switch
        {
            0 => AbilityRules.TranslateResourceString(GetEngine(), s_marksmanshipText),
            1 => $"{AbilityRules.TranslateResourceString(GetEngine(), s_marksmanshipText)} I",
            2 => $"{AbilityRules.TranslateResourceString(GetEngine(), s_marksmanshipText)} II",
            3 => $"{AbilityRules.TranslateResourceString(GetEngine(), s_marksmanshipText)} III",
            4 => $"{AbilityRules.TranslateResourceString(GetEngine(), s_marksmanshipText)} IV",
            _ => "",
        };
    }

    private static readonly string s_marksmanshipText = "Marksmanship";
}
