// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// (ability ID, success)
public partial class CommandCombatAction : SingleTargetCombatAction
{
    public new const int ClassId = 131763;

    public CommandCombatAction(ECustomNode owner) : base(owner)
    { }

    public int AbilityId;
    public bool Successful;

    // AoWAb.PAS:5044
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref AbilityId, 20);
        stream.Boolean(ref Successful, 21);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        AbilityId = persister.Int32("AbilityId", value: AbilityId);
        Successful = persister.Boolean("Successful", value: Successful);
    }

    // AoWAb.PAS:5063
    public override void Execute(Battle combat)
    {
        CombatUnit source;
        CombatUnit target;
        if (Successful)
        {
            source = (CombatUnit)combat.CombatData.FindId(SourceId);
            target = (CombatUnit)combat.CombatData.FindId(TargetId);
            source.SetExperience(source.GetExperience() + target.GetLevel());
            for (int I = 0; I <= 2; I++)
            {
                ((CommandAbility)GamePack.Current.AbilityControl.GetAbility(AbilityRules.CommandAbilityIds[I])).ResetCommandedUnits(target);
            }

            for (int I = 0; I <= 2; I++)
            {
                ((CommandAbility)GamePack.Current.AbilityControl.GetAbility(AbilityRules.CommandAbilityIds[I])).Uncommand(target);
            }

            if (source.GetSide() != target.GetSide())
            {
                ((CommandAbility)GamePack.Current.AbilityControl.GetAbility(AbilityId)).Command(source, target);
            }
        }
    }

    // AoWAb.PAS:5081
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 0)
        {
            GamePack.Current.AbilityControl.GetAbility(AbilityId).SfxLibrary.Play(0);
        }

        if (viewer.Delay == viewer.Time)
        {
            Execute(viewer.Combat);
        }

        return viewer.Delay * 2 > viewer.Time;
    }

    // AoWAb.PAS:5056
    public override void ShowObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        base.ShowObject(viewer, objectValue, frame, direction, X, Y, surface);
        if (objectValue.Id == SourceId)
        {
            GamePack.Current.AbilityControl.GetAbility(AbilityId).ImageSequences.Show(0, X + 32, Y + 32, viewer.Time, surface);
        }
    }

    // AoWAb.PAS:5050
    public void Initialize(byte sourceId, byte targetId, bool successful)
    {
        SourceId = sourceId;
        TargetId = targetId;
        Successful = successful;
    }

    // intf_P3_02.inc:38

}
