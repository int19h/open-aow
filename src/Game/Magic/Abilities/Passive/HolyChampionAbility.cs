// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class HolyChampionAbility : EnhancementAbility
{
    // PassiveAb.pas:484
    public HolyChampionAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(GameText.HolyChampionRStr);
        FAbilityID = 146;
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.HeadItem, AbilitySelectionType.TorsoItem, AbilitySelectionType.DefenseItem, AbilitySelectionType.RingItem, AbilitySelectionType.Editor);
    }

    // PassiveAb.pas:492
    public override bool CanExpand(AbilityOwner owner)
    {
        bool Result = base.CanExpand(owner);
        if (Result && owner is AbstractUnit)
        {
            Result = AlignmentTables.alsPureGood.Contains(((AbstractUnit)owner).GetAlignment());
        }

        return Result;
    }
}
