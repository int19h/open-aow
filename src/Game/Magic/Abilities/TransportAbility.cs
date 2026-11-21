// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public class TransportAbility : MultiLevelAbility
{
    // AoWAb.PAS:939
    public TransportAbility(CustomNode owner) : base(owner)
    {
        SelectionTypes = AbilitySelectionTypes.Unit;
        MaxLevel = 7;
        AbilityId = 50;
    }

    // AoWAb.PAS:947
    public override string GetLevelName(int level) => AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_transportUnitsFormat), arguments: [level.ToString(CultureInfo.InvariantCulture)]);

    private static readonly string s_transportUnitsFormat = "Transport (%s units)";
}
