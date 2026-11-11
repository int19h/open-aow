// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class UnholyChampionAbility : EnhancementAbility
{
    // PassiveAb.pas:499
    public UnholyChampionAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(GameText.UnholyChampionRStr);
        FAbilityID = 147;
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.HeadItem, AbilitySelectionType.TorsoItem, AbilitySelectionType.DefenseItem, AbilitySelectionType.RingItem, AbilitySelectionType.Editor);
    }

    // PassiveAb.pas:507
    public override bool CanExpand(AbilityOwner owner)
    {
        bool Result = base.CanExpand(owner);
        if (Result && owner is Troop)
        {
            Result = AlignmentTables.alsPureEvil.Contains(((Troop)owner).GetAlignment());
        }

        return Result;
    }
}
