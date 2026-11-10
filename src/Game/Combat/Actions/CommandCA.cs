// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// (ability ID, success)
public partial class CommandCA : SingleTargetCA
{
    public new const int ClassID = 131763;

    public CommandCA(ECustomNode owner) : base(owner)
    { }

    public int FAbilityID;
    public bool FSuccessful;

    // AoWAb.PAS:5044
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FAbilityID, 20);
        Stream.rwBool(ref FSuccessful, 21);
    }

    // AoWAb.PAS:5063
    public override void Execute(Battle Combat)
    {
        CombatUnit Source;
        CombatUnit Target;
        if (FSuccessful)
        {
            Source = (CombatUnit)Combat.FCombatData.FindID(FSourceID);
            Target = (CombatUnit)Combat.FCombatData.FindID(FTargetID);
            Source.SetExperience(Source.GetExperience() + Target.GetLevel());
            for (int I = 0; I <= 2; I++)
            {
                ((CommandAbility)AoWHSSet.Current.FAbilityControl.GetAbility(AbilityRules.CommandAbilityIDs[I])).ResetCommandedUnits(Target);
            }

            for (int I = 0; I <= 2; I++)
            {
                ((CommandAbility)AoWHSSet.Current.FAbilityControl.GetAbility(AbilityRules.CommandAbilityIDs[I])).Uncommand(Target);
            }

            if (Source.GetSide() != Target.GetSide())
            {
                ((CommandAbility)AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID)).Command(Source, Target);
            }
        }
    }

    // AoWAb.PAS:5081
    public override bool Play(CombatViewer Viewer)
    {
        if (Viewer.FTime == 0)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID).FSFXLibrary.Play(0);
        }

        if (Viewer.FDelay == Viewer.FTime)
        {
            Execute(Viewer.FCombat);
        }

        return Viewer.FDelay * 2 > Viewer.FTime;
    }

    // AoWAb.PAS:5056
    public override void ShowObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
        if (Obj.FID == FSourceID)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID).FImageSequences.Show(0, X + 32, Y + 32, Viewer.FTime, Surface);
        }
    }

    // AoWAb.PAS:5050
    public void Initialize(byte SourceID, byte TargetID, bool Successful)
    {
        FSourceID = SourceID;
        FTargetID = TargetID;
        FSuccessful = Successful;
    }

    // intf_P3_02.inc:38

}
