// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

// (ability ID)
public partial class RangedAttackCA : DamageCA
{
    public new const int ClassID = 131753;

    public RangedAttackCA(ECustomNode owner) : base(owner)
    { }

    public int FAbilityID;

    // AoWAb.PAS:4631
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwData(Bytes.Of(ref FAbilityID, 4), 4, 20);
    }

    // AoWAb.PAS:4643
    public override string GetName() => AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID).GetName(null);

    // AoWAb.PAS:4647
    public override bool Play(CombatViewer Viewer)
    {
        if (Viewer.FTime == 1)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID).FSFXLibrary.Play(0);
        }

        return base.Play(Viewer);
    }

    // AoWAb.PAS:4636
    public override void ShowObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
        if (Obj.FID == FTargetID && Viewer.FTime != 0)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID).FImageSequences.ShowLooped(40, X + 32, Y + 32, Viewer.FTime, Surface);
        }
    }

    // intf_P3_02.inc:38

}
