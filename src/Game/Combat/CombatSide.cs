// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// SideIndex. Opponent. Strength. ConquerCount
public partial class CombatSide : ENode
{
    // Combat.pas:5999
    public CombatSide(ECustomNode owner) : base(owner)
    {
    }

    public byte SideIndex;
    public CombatSide Opponent;
    public int Strength;
    public int ConquerCount;

    // Combat.pas:6002
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
    }

    // Combat.pas:6008
    public CombatObject GetObjects(int index) => (CombatObject)_children.List[index];

    // Combat.pas:6011
    public int IndexOfId(int id)
    {
        for (int result = Count - 1; result >= 0; result--)
        {
            if (((CombatObject)_children.List[result]).Id == id)
            {
                return result;
            }
        }

        return -1;
    }

    // Combat.pas:6018
    public int GetDestroyedObjects()
    {
        int result = 0;
        for (int I = Count - 1; I >= 0; I--)
        {
            if (((CombatObject)_children.List[I]).Status.Contains(CombatObjectStatusElement.Destroyed))
            {
                result += 1;
            }
        }

        return result;
    }

    // Combat.pas:6026
    public int GetUndestroyedConquerObjects()
    {
        CombatObject objectValue;
        int result = 0;
        for (int I = Count - 1; I >= 0; I--)
        {
            objectValue = (CombatObject)_children.List[I];
            if ((Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed, CombatObjectStatusElement.Fled) & objectValue.Status) == Set<CombatObjectStatusElement>.Of() && objectValue.ControlMode.Contains(CombatObjectControlModeElement.ConquerObject))
            {
                result += 1;
            }
        }

        return result;
    }

    // Combat.pas:6038
    public void OutputDebugInfo()
    {
        string S;
        if (SideIndex == 0)
        {
            GameTables.GameEngine.OutputDebugMessage(CombatAbilityTables.s_combatDebugChannel, "--- Combat Side Attacker ---");
        }
        else
        {
            GameTables.GameEngine.OutputDebugMessage(CombatAbilityTables.s_combatDebugChannel, "--- Combat Side Defender ---");
        }

        for (int I = 0, last = _children.Count - 1; I <= last; I++)
        {
            var idString = GetObjects(I).IdString();
            var targetPriority = GetObjects(I).GetTargetPriority();
            var targetStrength = GetObjects(I).GetTargetStrength();
            S = "(" + IntToStr(I) + "):" + idString + " (Priority:" + IntToStr(targetPriority) + "/Strength:" + IntToStr(targetStrength) + ")";
            GameTables.GameEngine.OutputDebugMessage(CombatAbilityTables.s_combatDebugChannel, S);
        }
    }

    // Combat.pas:6052
    public void UpdateSettings()
    {
        int N;
        CombatObject objectValue;
        Strength = 0;
        ConquerCount = 0;
        for (int I = _children.Count - 1; I >= 0; I--)
        {
            objectValue = (CombatObject)_children.List[I];
            N = objectValue.Targets.Count;
            if (N > 0)
            {
                Strength += objectValue.CombatValue / N;
            }

            if (objectValue.TargetedBy.Count > 0 && objectValue.AttackerCount == 0)
            {
                ConquerCount += 1;
            }
        }
    }

    // intf_P3_01.inc:352

}
