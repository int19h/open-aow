// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class TurnUndeadCA : DamageCA
{
    public new const int ClassID = 131769;

    public TurnUndeadCA(ECustomNode owner) : base(owner)
    { }

    // AoWAb.PAS:3088
    public override void Execute(Battle Combat)
    {
        CombatUnit Source;
        CombatUnit Target;
        UnitEnchantmentAbilityData Data;
        base.Execute(Combat);
        if (FDamage != 0)
        {
            Source = (CombatUnit)Combat.FCombatData.FindID(FSourceID);
            Target = (CombatUnit)Combat.FCombatData.FindID(FTargetID);
            if (!Target.FStatus.Contains(CombatObjectStatusElement.Destroyed))
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(34).Expand(Target.FUnit);
                Data = (UnitEnchantmentAbilityData)Target.FUnit.GetAbilityData(34);

                if (Data != null)
                {
                    Data.FEnchantmentPlayerIndex = Source.FCombatPlayer;
                }
            }
        }
    }

    // AoWAb.PAS:3115
    public override bool Play(CombatViewer Viewer)
    {
        bool Result = base.Play(Viewer);
        if (Viewer.FTime == 0)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(38).FSFXLibrary.Play(0);
        }

        return Result;
    }

    // AoWAb.PAS:3108
    public override void ShowObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
        if (Obj.FID == FTargetID)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(38).FImageSequences.Show(0, X + 32, Y + 32, Viewer.FTime, Surface);
        }
    }

    // AoWAb.PAS:3084
    public bool GetSuccessfull() => FDamage != 0;

    // intf_P3_02.inc:38

}
