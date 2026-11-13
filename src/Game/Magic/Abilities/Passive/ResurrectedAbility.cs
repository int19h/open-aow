// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class ResurrectedAbility : AdjustableAbility
{
    // PassiveAb.pas:892
    public ResurrectedAbility(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), GameText.ResurrectedText);
        AbilityId = 128;
        AbilityType = AbilityType.Handicap;
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }
}
