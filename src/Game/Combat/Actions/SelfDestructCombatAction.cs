// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

public partial class SelfDestructCombatAction : DamageCombatAction
{
    public new const int ClassID = 131764;

    public SelfDestructCombatAction(ECustomNode owner) : base(owner)
    { }

    // AoWAb.PAS:2116
    public override void Execute(Battle Combat)
    {
        CombatObject Source;
        CombatObject Target;
        Source = Combat.FCombatData.FindID(FSourceID);
        Target = Combat.FCombatData.FindID(FTargetID);
        FDamageDone = (byte)Source.DoDamage(Target, FDamage);
        if (FEffects.Value != Set<DamageType>.Of())
        {
            Target.ExecuteDamageEffects(FEffects.Value);
        }

        Source.ExecuteDamage(Source.GetHitPoints());
    }

    // AoWAb.PAS:2127
    public override void ShowObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in LockedSurface Surface)
    {
        base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
        if ((Obj.FID == FTargetID || Obj.FID == FSourceID) && Viewer.FTime >= Viewer.FDelay - 2)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(65).FImageSequences.Show(40, X + 32, Y + 32, Viewer.FTime, Surface);
        }
    }

    // intf_P3_02.inc:38

}
