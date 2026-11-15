// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class DispelMagicAbilityCombatAction : DispelMagicCombatAction
{
    public new const int ClassId = 131775;

    public DispelMagicAbilityCombatAction(CustomNode owner) : base(owner)
    { }

    // AoWAb.PAS:4018
    public override void Execute(Battle combat)
    {
        CombatUnit source;
        base.Execute(combat);
        source = combat.CombatData.FindId<CombatUnit>(SourceId);
        ((DispelMagicAbility)GamePack.Current.AbilityControl.GetAbility(60)).SetEnabled(source.Troop, false);
    }

    // intf_P4_02.inc:660

}
