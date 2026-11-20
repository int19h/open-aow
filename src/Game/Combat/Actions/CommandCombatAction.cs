// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// (ability ID, success)
public class CommandCombatAction(CustomNode owner) : SingleTargetCombatAction(owner)
{
    public new const int ClassId = 131763;

    public int AbilityId { get; set; }
    public bool Successful { get; private set; }

    public override bool TouchSucceeded => Successful;

    // AoWAb.PAS:5044
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        AbilityId = persister.Int32("AbilityId", value: AbilityId);
        Successful = persister.Boolean("Successful", value: Successful);
    }

    // AoWAb.PAS:5063
    public override void Execute(Battle combat)
    {
        if (Successful)
        {
            CombatUnit source = combat.CombatData.FindId<CombatUnit>(SourceId);
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(TargetId);
            source.SetExperience(source.Experience + target.Level);
            for (int i = 0; i <= 2; i++)
            {
                GamePack.Current.AbilityControl.GetAbility<CommandAbility>(AbilityRules.CommandAbilityIds[i]).ResetCommandedUnits(target);
            }

            for (int i = 0; i <= 2; i++)
            {
                GamePack.Current.AbilityControl.GetAbility<CommandAbility>(AbilityRules.CommandAbilityIds[i]).Uncommand(target);
            }

            if (source.Side != target.Side)
            {
                GamePack.Current.AbilityControl.GetAbility<CommandAbility>(AbilityId).Command(source, target);
            }
        }
    }

    // AoWAb.PAS:5081
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 0)
        {
            _ = GamePack.Current.AbilityControl[AbilityId].SfxLibrary.Play(0);
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
            _ = GamePack.Current.AbilityControl[AbilityId].ImageSequences.Show(0, X + 32, Y + 32, viewer.Time, surface);
        }
    }

    // AoWAb.PAS:5050
    public void Initialize(byte sourceId, byte targetId, bool successful)
    {
        SourceId = sourceId;
        TargetId = targetId;
        Successful = successful;
    }
}
