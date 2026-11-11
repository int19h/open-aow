// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// (successful)
public partial class EntangleCombatAction : SingleTargetCombatAction
{
    public new const int ClassID = 131774;

    public EntangleCombatAction(ECustomNode owner) : base(owner)
    { }

    public bool FSuccessful;

    // AoWAb.PAS:2927
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwBool(ref FSuccessful, 20);
    }

    // AoWAb.PAS:2938
    public override void Execute(Battle Combat)
    {
        CombatUnit Source;
        CombatUnit Target;
        UnitEnchantmentAbilityData Data;
        if (FSuccessful)
        {
            Source = (CombatUnit)Combat.FCombatData.FindID(FSourceID);
            Target = (CombatUnit)Combat.FCombatData.FindID(FTargetID);
            AoWHSSet.Current.FAbilityControl.GetAbility(94).Expand(Target.FUnit);
            Data = (UnitEnchantmentAbilityData)Target.FUnit.GetAbilityData(94);
            if (Data != null)
            {
                Data.FEnchantmentPlayerIndex = Source.FCombatPlayer;
            }
        }
    }

    // AoWAb.PAS:2960
    public override bool Play(CombatViewer Viewer)
    {
        if (Viewer.FTime == 0)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(40).FSFXLibrary.Play(0);
        }

        if (Viewer.FDelay == Viewer.FTime)
        {
            Execute(Viewer.FCombat);
        }

        return Viewer.FDelay * 2 > Viewer.FTime;
    }

    // AoWAb.PAS:2953
    public override void ShowObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in LockedSurface Surface)
    {
        base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
        if (Obj.FID == FTargetID)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(40).FImageSequences.Show(0, X + 32, Y + 32, Viewer.FTime, Surface);
        }
    }

    // AoWAb.PAS:2932
    public void Initialize(byte SourceID, byte TargetID, bool Successful)
    {
        FSourceID = SourceID;
        FTargetID = TargetID;
        FSuccessful = Successful;
    }

    // intf_P3_02.inc:38

}
