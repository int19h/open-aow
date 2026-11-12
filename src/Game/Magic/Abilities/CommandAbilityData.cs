// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Combat;

namespace Aow.Game.Magic.Abilities;

// (ability ID, commanded combat-unit IDs)
public partial class CommandAbilityData : AbilityData
{
    public new const int ClassId = 131755;

    // AoWAb.PAS:4949
    public CommandAbilityData(ECustomNode owner) : base(owner)
    {
        CommandedUnits = new ByteList();
    }

    public int AbilityId;

    public ByteList CommandedUnits { get; }

    // AoWAb.PAS:4954
    public override void Dispose()
    {
        CommandedUnits?.Dispose();
        base.Dispose();
    }

    // AoWAb.PAS:4967
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref AbilityId, 20);
        stream.Payload(21, CommandedUnits.LoadFromStream, CommandedUnits.SaveToStream);
    }

    // AoWAb.PAS:4963
    public override int GetAbilityId() => AbilityId;

    // AoWAb.PAS:4973
    public int CommandingTargetPriority(Combat.Battle combat)
    {
        CombatObject target;
        int result = 0;
        for (int I = CommandedUnits.Count - 1; I >= 0; I--)
        {
            target = combat.CombatData.FindId(CommandedUnits.List[I]);
            if (target != null)
            {
                result += target.GetTargetPriority();
            }
        }

        return result;
    }

    // AoWAb.PAS:4985
    public int CommandingTargetStrength(Combat.Battle combat)
    {
        CombatObject target;
        int result = 0;
        for (int I = CommandedUnits.Count - 1; I >= 0; I--)
        {
            target = combat.CombatData.FindId(CommandedUnits.List[I]);
            if (target != null)
            {
                result += target.GetTargetStrength();
            }
        }

        return result;
    }

    // intf_P4_02.inc:475

}
