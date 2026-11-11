// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// (successful)
public partial class InvokeDeathCombatAction : SingleTargetCombatAction
{
    public new const int ClassID = 131780;

    public InvokeDeathCombatAction(ECustomNode owner) : base(owner)
    { }

    public bool FSuccessful;

    // AoWAb.PAS:3400
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwBool(ref FSuccessful, 20);
    }

    // AoWAb.PAS:3420
    public override void Execute(Battle Combat)
    {
        CombatObject Source;
        CombatObject Target;
        if (FSuccessful)
        {
            Source = Combat.FCombatData.FindID(FSourceID);
            Target = Combat.FCombatData.FindID(FTargetID);
            Source.DoDamage(Target, Target.GetHitPoints());
        }
    }

    // AoWAb.PAS:3412
    public override bool Play(CombatViewer Viewer)
    {
        if (Viewer.FTime == 0)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(35).FSFXLibrary.Play(0);
        }

        if (Viewer.FDelay == Viewer.FTime)
        {
            Execute(Viewer.FCombat);
        }

        return Viewer.FDelay * 2 > Viewer.FTime;
    }

    // AoWAb.PAS:3405
    public override void ShowObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in LockedSurface Surface)
    {
        base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
        if (Obj.FID == FTargetID)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(35).FImageSequences.Show(0, X + 32, Y + 32, Viewer.FTime, Surface);
        }
    }

    // AoWAb.PAS:3431
    public void Initialize(byte SourceID, byte TargetID, bool Successful)
    {
        FSourceID = SourceID;
        FTargetID = TargetID;
        FSuccessful = Successful;
    }

    // intf_P3_02.inc:38

}
