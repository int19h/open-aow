// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// FSideIndex. FOpponent. FStrength. FConquerCount
public partial class CombatSide : ENode
{
    // Combat.pas:5999
    public CombatSide(ECustomNode owner) : base(owner)
    {
    }

    public byte FSideIndex;
    public CombatSide FOpponent;
    public int FStrength;
    public int FConquerCount;

    // Combat.pas:6002
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
    }

    // Combat.pas:6008
    public CombatObject GetObjects(int Index) => (CombatObject)FChildren.List[Index];

    // Combat.pas:6011
    public int IndexOfID(int ID)
    {
        for (int Result = Count - 1; Result >= 0; Result--)
        {
            if (((CombatObject)FChildren.List[Result]).FID == ID)
            {
                return Result;
            }
        }

        return -1;
    }

    // Combat.pas:6018
    public int GetDestroyedObjects()
    {
        int Result = 0;
        for (int I = Count - 1; I >= 0; I--)
        {
            if (((CombatObject)FChildren.List[I]).FStatus.Contains(CombatObjectStatusElement.Destroyed))
            {
                Result += 1;
            }
        }

        return Result;
    }

    // Combat.pas:6026
    public int GetUndestroyedConquerObjects()
    {
        CombatObject Obj;
        int Result = 0;
        for (int I = Count - 1; I >= 0; I--)
        {
            Obj = (CombatObject)FChildren.List[I];
            if ((Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed, CombatObjectStatusElement.Fled) & Obj.FStatus) == Set<CombatObjectStatusElement>.Of() && Obj.FControlMode.Contains(CombatObjectControlModeElement.ConquerObject))
            {
                Result += 1;
            }
        }

        return Result;
    }

    // Combat.pas:6038
    public void OutputDebugInfo()
    {
        string S;
        if (FSideIndex == 0)
        {
            GameTables.AoWEngine.OutputDebugMessage(CombatAbilityTables.CombatDID, "--- Combat Side Attacker ---");
        }
        else
        {
            GameTables.AoWEngine.OutputDebugMessage(CombatAbilityTables.CombatDID, "--- Combat Side Defender ---");
        }

        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            var idStr = GetObjects(I).IDStr();
            var targetPriority = GetObjects(I).GetTargetPriority();
            var targetStrength = GetObjects(I).GetTargetStrength();
            S = "(" + IntToStr(I) + "):" + idStr + " (Priority:" + IntToStr(targetPriority) + "/Strength:" + IntToStr(targetStrength) + ")";
            GameTables.AoWEngine.OutputDebugMessage(CombatAbilityTables.CombatDID, S);
        }
    }

    // Combat.pas:6052
    public void UpdateSettings()
    {
        int N;
        CombatObject Obj;
        FStrength = 0;
        FConquerCount = 0;
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            Obj = (CombatObject)FChildren.List[I];
            N = Obj.FTargets.Count;
            if (N > 0)
            {
                FStrength += Obj.FCombatValue / N;
            }

            if (Obj.FTargetedBy.Count > 0 && Obj.FAttackerCount == 0)
            {
                FConquerCount += 1;
            }
        }
    }

    // intf_P3_01.inc:352

}
