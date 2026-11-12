// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

public partial class TransportAbility : MultiLevelAbility
{
    // AoWAb.PAS:939
    public TransportAbility(ECustomNode owner) : base(owner)
    {
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
        MaxLevel = 7;
        AbilityId = 50;
    }

    // AoWAb.PAS:947
    public override string GetLevelName(int level) => AbilityRules.Format(AbilityRules.TranslateResourceString(s_transportUnitsFormat), [IntToStr(level)]);

    private static readonly string s_transportUnitsFormat = "Transport (%s units)";
}
