// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

// (possession succeeded)
public partial class PossessCA : SingleTargetCA
{
    public new const int ClassID = 131765;

    public PossessCA(ECustomNode owner) : base(owner)
    { }

    public bool FSuccessful;

    // AoWAb.PAS:2471
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwBool(ref FSuccessful, 20);
    }

    // AoWAb.PAS:2484
    public override void Execute(Battle Combat)
    {
        CombatUnit Source;
        CombatUnit Target;
        AbstractUnit NewUnit;
        AbstractUnit OldUnit;
        PossessedAbilityData Data;
        int Experience;
        if (FSuccessful)
        {
            Source = (CombatUnit)Combat.FCombatData.FindID(FSourceID);
            Target = (CombatUnit)Combat.FCombatData.FindID(FTargetID);
            for (int I = 0; I <= 2; I++)
            {
                ((CommandAbility)AoWHSSet.Current.FAbilityControl.GetAbility(AbilityRules.CommandAbilityIDs[I])).ResetCommandedUnits(Target);
            }

            for (int I = 0; I <= 2; I++)
            {
                ((CommandAbility)AoWHSSet.Current.FAbilityControl.GetAbility(AbilityRules.CommandAbilityIDs[I])).Uncommand(Target);
            }

            NewUnit = Target.FUnit;
            NewUnit.AddRef();
            Target.SetUnit(null);
            Target.DestroyObject();
            OldUnit = Source.FUnit;
            OldUnit.AddRef();
            OldUnit.Killed();
            OldUnit.SetOwner(null);
            Source.SetUnit(NewUnit);
            if (OldUnit.FUnitStatus.Contains(UnitStatusElement.Moving))
            {
                Source.FUnit.FUnitStatus = Source.FUnit.FUnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
            }

            AoWHSSet.Current.FAbilityControl.GetAbility(107).Expand(Source.FUnit);
            if (OldUnit.GetAbilityEnabled(107))
            {
                Data = (PossessedAbilityData)OldUnit.GetAbilityData(107);
                if (Data != null)
                {
                    Experience = Data.FSourceExperience;
                }
                else
                {
                    Experience = 0;
                }
            }
            else
            {
                Experience = OldUnit.GetExperience();
            }

            Data = (PossessedAbilityData)Source.FUnit.GetAbilityData(107);
            if (Data != null)
            {
                Data.FSourceExperience = Source.FUnit.GetUnitLevel() + Experience;
            }

            NewUnit.Release();
            OldUnit.Release();
        }
    }

    // AoWAb.PAS:2537
    public override bool Play(CombatViewer Viewer)
    {
        if (Viewer.FTime == 0)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(106).FSFXLibrary.Play(0);
        }

        if (Viewer.FDelay == Viewer.FTime)
        {
            Execute(Viewer.FCombat);
        }

        return Viewer.FDelay * 2 > Viewer.FTime;
    }

    // AoWAb.PAS:2529
    public override void ShowObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
        if (Obj.FID == FSourceID || Obj.FID == FTargetID)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(106).FImageSequences.Show(0, X + 32, Y + 32, Viewer.FTime, Surface);
        }
    }

    // AoWAb.PAS:2477
    public void Initialize(byte SourceID, byte TargetID, bool Successful)
    {
        FSourceID = SourceID;
        FTargetID = TargetID;
        FSuccessful = Successful;
    }

    // intf_P3_02.inc:38

}
