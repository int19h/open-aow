// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

public partial class SelfDestructCombatAction(CustomNode owner) : DamageCombatAction(owner)
{
    public new const int ClassId = 131764;

    // AoWAb.PAS:2116
    public override void Execute(Battle combat)
    {
        CombatObject source = combat.CombatData.FindId(SourceId);
        CombatObject target = combat.CombatData.FindId(TargetId);
        DamageDone = (byte)source.DoDamage(target, Damage);
        if (Effects != Set<DamageType>.Of())
        {
            _ = target.ExecuteDamageEffects(Effects);
        }

        _ = source.ExecuteDamage(source.GetHitPoints());
    }

    // AoWAb.PAS:2127
    public override void ShowObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        base.ShowObject(viewer, objectValue, frame, direction, X, Y, surface);
        if ((objectValue.Id == TargetId || objectValue.Id == SourceId) && viewer.Time >= viewer.Delay - 2)
        {
            _ = GamePack.Current.AbilityControl.GetAbility(65).ImageSequences.Show(40, X + 32, Y + 32, viewer.Time, surface);
        }
    }

    // intf_P3_02.inc:38

}
