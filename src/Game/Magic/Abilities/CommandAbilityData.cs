#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Combat;

namespace Aow.Game.Magic.Abilities;

// (ability ID, commanded combat-unit IDs)
public class CommandAbilityData(CustomNode owner) : AbilityData(owner)
{

    public new const int ClassId = 131_755;

    public int AbilityId { get; set; }

    public ByteList CommandedUnits { get; } = new ByteList();

    // AoWAb.PAS:4954
    public override void Dispose()
    {
        CommandedUnits?.Dispose();
        base.Dispose();
    }

    // AoWAb.PAS:4967
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        AbilityId = persister.Int32("AbilityId", value: AbilityId);
        persister.ByteList(name: "CommandedUnits", list: CommandedUnits);
    }

    // AoWAb.PAS:4963
    public override int GetAbilityId() => AbilityId;

    // AoWAb.PAS:4973
    public int CommandingTargetPriority(Battle combat)
    {
        int result = 0;
        for (int i = CommandedUnits.Count - 1; i >= 0; i--)
        {
            CombatObject target = combat.CombatData.FindId(CommandedUnits.List[i]);
            if (target is not null)
            {
                result += target.GetTargetPriority();
            }
        }

        return result;
    }

    // AoWAb.PAS:4985
    public int CommandingTargetStrength(Battle combat)
    {
        int result = 0;
        for (int i = CommandedUnits.Count - 1; i >= 0; i--)
        {
            CombatObject target = combat.CombatData.FindId(CommandedUnits.List[i]);
            if (target is not null)
            {
                result += target.GetTargetStrength();
            }
        }

        return result;
    }
}
