// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode. duration data
// countdown, player index
public partial class CrusaderAbility : DurationAbility
{
    // PassiveAb.pas:703
    public CrusaderAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(CrusaderRStr);
        FAbilityID = 101;
        FAbilityType = AbilityType.Handicap;
        FDurationMode = DurationMode.Day;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:724
    public override void NewTurn(AbilityOwner Owner, sbyte Player)
    {
        DurationAbilityData Data;
        if (FDurationMode == DurationMode.Day)
        {
            Data = (DurationAbilityData)Owner.GetAbilityData(FAbilityID);
            if (Data != null && Data.FDuration != 0 && Data.FPlayerIndex == Player)
            {
                Data.FDuration = (byte)(Data.FDuration - 1);
            }
        }
    }

    // PassiveAb.pas:713
    public override bool Remove(AbilityOwner owner)
    {
        bool Result = base.Remove(owner);
        if (owner is AbstractUnit unit)
        {
            unit.Killed();
        }

        return Result;
    }

    // PassiveAb.pas:719
    public override int GetTotalDuration() => 3;

    private static readonly string CrusaderRStr = "Crusader";
}
