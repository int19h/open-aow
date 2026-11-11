// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// (successful)
public partial class WebCombatAction : SingleTargetCombatAction
{
    public new const int ClassID = 131768;

    public WebCombatAction(ECustomNode owner) : base(owner)
    { }

    public bool FSuccessful;

    // AoWAb.PAS:2765
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwBool(ref FSuccessful, 20);
    }

    // AoWAb.PAS:2778
    public override void Execute(Battle Combat)
    {
        CombatUnit Target;
        if (FSuccessful)
        {
            Target = (CombatUnit)Combat.FCombatData.FindID(FTargetID);
            AoWHSSet.Current.FAbilityControl.GetAbility(97).Expand(Target.FUnit);
        }
    }

    // AoWAb.PAS:2797
    public override bool Play(CombatViewer Viewer)
    {
        if (Viewer.FTime == 0)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(27).FSFXLibrary.Play(0);
        }

        if (Viewer.FDelay == Viewer.FTime)
        {
            Execute(Viewer.FCombat);
        }

        return Viewer.FDelay * 2 > Viewer.FTime;
    }

    // AoWAb.PAS:2789
    public override void ShowObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in LockedSurface Surface)
    {
        base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
        if (Obj.FID == FTargetID)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(27).FImageSequences.Show(0, X + 32, Y + 32, Viewer.FTime, Surface);
        }
    }

    // AoWAb.PAS:2771
    public void Initialize(byte SourceID, byte TargetID, bool Successful)
    {
        FSourceID = SourceID;
        FTargetID = TargetID;
        FSuccessful = Successful;
    }

    // intf_P3_02.inc:38

}
