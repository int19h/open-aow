// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class SummonedAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:1100
    public SummonedAbility(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_summonedText);
        AbilityId = 155;
    }

    // PassiveAb.pas:1122
    public override bool Remove(AbilityOwner owner)
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
        if (owner is not Troop troop)
        {
            return 0;
        }

        int result;
        switch (troop.GetUnitLevel())
        {
            case 1:
                result = 250;
                break;
            case 2:
                result = 250;
                break;
            case 3:
                result = 500;
                break;
            case 4:
                result = 1000;
                break;
            default:
                result = 1000;
                break;
        }

        return result;
    }

    // PassiveAb.pas:1108
    // Static: the original calls this and GetDefaultDispelResistance on the nil result of GetSpell(155)
    // (PrdPlace.pas:336, SummonSpells.pas:258), which works there because they read only the unit argument.
    public static int GetDefaultManaUpkeep(Troop troop)
    {
        int result;
        if (troop is Unit)
        {
            result = (troop.GetUnitLevel() + 1) * 2;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // PassiveAb.pas:1115
    public static int GetDefaultDispelResistance(Troop troop)
    {
        int result;
        if (troop is Unit)
        {
            result = troop.GetUnitLevel() + 3;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // PassiveAb.pas:1147
    public int UnitIndexToAIUpkeepPriority(int unitIndex)
    {
        int result;
        switch (GamePack.Current.UnitResources.GetUnitResource(unitIndex).Level)
        {
            case 1:
                result = 250;
                break;
            case 2:
                result = 250;
                break;
            case 3:
                result = 500;
                break;
            case 4:
                result = 1000;
                break;
            default:
                result = 1000;
                break;
        }

        return result;
    }

    private static readonly string s_summonedText = "Summoned";
}
