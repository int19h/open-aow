// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class DispelMagicAbilityCA : DispelMagicCA
{
    public new const int ClassID = 131775;

    public DispelMagicAbilityCA(ECustomNode owner) : base(owner)
    { }

    // AoWAb.PAS:4018
    public override void Execute(Battle Combat)
    {
        CombatUnit Source;
        base.Execute(Combat);
        Source = (CombatUnit)Combat.FCombatData.FindID(FSourceID);
        ((DispelMagicAbility)AoWHSSet.Current.FAbilityControl.GetAbility(60)).SetEnabled(Source.FUnit, false);
    }

    // intf_P4_02.inc:660

}
