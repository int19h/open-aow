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
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
        FMaxLevel = 7;
        FAbilityID = 50;
    }

    // AoWAb.PAS:947
    public override string GetLevelName(int Level) => AbilityRules.Format(AbilityRules.TranslateRStr(P4TransportUnitsRStr), [IntToStr(Level)]);

    private static readonly string P4TransportUnitsRStr = "Transport (%s units)";
}
