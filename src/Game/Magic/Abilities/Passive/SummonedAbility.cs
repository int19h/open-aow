#nullable disable warnings
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class SummonedAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:1100
    public SummonedAbility(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_summonedText);
        AbilityId = 155;
    }

    // PassiveAb.pas:1122
    public override bool Remove(AbilityOwner? owner)
    {
        bool result = base.Remove(owner);
        if (result && owner is Troop troop)
        {
            troop.Killed();
            troop.SetOwner(null);
        }

        return result;
    }

    // PassiveAb.pas:1134
    public override int GetAIUpkeepPriority(AbilityOwner owner)
    {
        return owner is not Troop troop
            ? 0
            : troop.UnitLevel switch
            {
                1 => 250,
                2 => 250,
                3 => 500,
                4 => 1_000,
                _ => 1_000,
            };
    }

    // PassiveAb.pas:1108
    // Static: the original calls this and GetDefaultDispelResistance on the nil result of GetSpell(155)
    // (PrdPlace.pas:336, SummonSpells.pas:258), which works there because they read only the unit argument.
    public static int GetDefaultManaUpkeep(Troop troop) => troop is Unit ? (troop.UnitLevel + 1) * 2 : 0;

    // PassiveAb.pas:1115
    public static int GetDefaultDispelResistance(Troop? troop) => troop is Unit ? troop.UnitLevel + 3 : 0;

    // PassiveAb.pas:1147
    public int UnitIndexToAIUpkeepPriority(int unitIndex)
    {
        return GetEngine().GamePack.UnitResources.GetUnitResource(unitIndex).Level switch
        {
            1 => 250,
            2 => 250,
            3 => 500,
            4 => 1_000,
            _ => 1_000,
        };
    }

    private static readonly string s_summonedText = "Summoned";
}
