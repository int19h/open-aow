// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Combat;

namespace Aow.Game.Magic.Abilities;

// (ability ID, commanded combat-unit IDs)
public partial class CommandAbilityData : AbilityData
{
    public new const int ClassID = 131755;

    // AoWAb.PAS:4949
    public CommandAbilityData(ECustomNode owner) : base(owner)
    {
        FCommandedUnits = new ByteList();
    }

    public int FAbilityID;
    public ByteList FCommandedUnits;

    // AoWAb.PAS:4954
    public override void Dispose()
    {
        FCommandedUnits?.Dispose();
        base.Dispose();
    }

    // AoWAb.PAS:4967
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FAbilityID, 20);
        Stream.rwStreamProc(21, FCommandedUnits.LoadFromStream, FCommandedUnits.SaveToStream);
    }

    // AoWAb.PAS:4963
    public override int GetAbilityID() => FAbilityID;

    // AoWAb.PAS:4973
    public int CommandingTargetPriority(Combat.Battle Combat)
    {
        CombatObject Target;
        int Result = 0;
        for (int I = FCommandedUnits.Count - 1; I >= 0; I--)
        {
            Target = Combat.FCombatData.FindID(FCommandedUnits.List[I]);
            if (Target != null)
            {
                Result += Target.GetTargetPriority();
            }
        }

        return Result;
    }

    // AoWAb.PAS:4985
    public int CommandingTargetStrength(Combat.Battle Combat)
    {
        CombatObject Target;
        int Result = 0;
        for (int I = FCommandedUnits.Count - 1; I >= 0; I--)
        {
            Target = Combat.FCombatData.FindID(FCommandedUnits.List[I]);
            if (Target != null)
            {
                Result += Target.GetTargetStrength();
            }
        }

        return Result;
    }

    // intf_P4_02.inc:475

}
