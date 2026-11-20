// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.Magic.Abilities;

public class TransportAbility : MultiLevelAbility
{
    // AoWAb.PAS:939
    public TransportAbility(CustomNode owner) : base(owner)
    {
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
        MaxLevel = 7;
        AbilityId = 50;
    }

    // AoWAb.PAS:947
    public override string GetLevelName(int level) => AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_transportUnitsFormat), arguments: [IntToStr(level)]);

    private static readonly string s_transportUnitsFormat = "Transport (%s units)";
}
