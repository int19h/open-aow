// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class SummonedAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:1100
    public SummonedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(SummonedRStr);
        FAbilityID = 155;
    }

    // PassiveAb.pas:1122
    public override bool Remove(AbilityOwner owner)
    {
        bool Result = base.Remove(owner);
        if (Result && owner is Troop unit)
        {
            unit.Killed();
            unit.SetOwner(null);
        }

        return Result;
    }

    // PassiveAb.pas:1134
    public override int GetAIUpkeepPriority(AbilityOwner Owner)
    {
        if (Owner is not Troop unit)
        {
            return 0;
        }

        int Result;
        switch (unit.GetUnitLevel())
        {
            case 1:
                Result = 250;
                break;
            case 2:
                Result = 250;
                break;
            case 3:
                Result = 500;
                break;
            case 4:
                Result = 1000;
                break;
            default:
                Result = 1000;
                break;
        }

        return Result;
    }

    // PassiveAb.pas:1108
    // Static: the original calls this and GetDefaultDispelResistance on the nil result of GetSpell(155)
    // (PrdPlace.pas:336, SummonSpells.pas:258), which works there because they read only the unit argument.
    public static int GetDefaultManaUpkeep(Troop AUnit)
    {
        int Result;
        if (AUnit is Unit)
        {
            Result = (AUnit.GetUnitLevel() + 1) * 2;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // PassiveAb.pas:1115
    public static int GetDefaultDispelResistance(Troop AUnit)
    {
        int Result;
        if (AUnit is Unit)
        {
            Result = AUnit.GetUnitLevel() + 3;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // PassiveAb.pas:1147
    public int UnitIndexToAIUpkeepPriority(int UnitIndex)
    {
        int Result;
        switch (AoWHSSet.Current.FUnitResources.GetUnitResource(UnitIndex).FLevel)
        {
            case 1:
                Result = 250;
                break;
            case 2:
                Result = 250;
                break;
            case 3:
                Result = 500;
                break;
            case 4:
                Result = 1000;
                break;
            default:
                Result = 1000;
                break;
        }

        return Result;
    }

    private static readonly string SummonedRStr = "Summoned";
}
