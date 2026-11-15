#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// SideIndex. Opponent. Strength. ConquerCount
public partial class CombatSide : OwningNode, IChildList<CombatObject>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a CombatObject, so the cast never fails.
    public CombatObject this[int index] => (CombatObject)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is CombatObject;

    int IChildSink<CombatObject>.AddChild(CombatObject child) => AddChild(child);

    // Combat.pas:5999
    public CombatSide(CustomNode owner) : base(owner)
    {
    }

    public byte SideIndex { get; set; }
    public CombatSide Opponent { get; set; }
    public int Strength { get; private set; }
    public int ConquerCount { get; private set; }

    // Combat.pas:6002
    public override void PersistFields(IPersister persister) => base.PersistFields(persister);

    // Combat.pas:6008
    public CombatObject GetObjects(int index) => this[index];

    // The caller sets up only units on the side it reads here, so the cast never fails there.
    public T GetObjects<T>(int index) where T : CombatObject => (T)GetObjects(index);

    // Combat.pas:6011
    public int IndexOfId(int id)
    {
        for (int result = Count - 1; result >= 0; result--)
        {
            if (this[result].Id == id)
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
        for (int i = Count - 1; i >= 0; i--)
        {
            if (this[i].Status.Contains(CombatObjectStatusElement.Destroyed))
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
        for (int i = Count - 1; i >= 0; i--)
        {
            objectValue = this[i];
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
        string s;
        if (SideIndex == 0)
        {
            GetEngine().OutputDebugMessage(DebugChannel.Combat, "--- Combat Side Attacker ---");
        }
        else
        {
            GetEngine().OutputDebugMessage(DebugChannel.Combat, "--- Combat Side Defender ---");
        }

        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            var idString = GetObjects(i).IdString();
            var targetPriority = GetObjects(i).GetTargetPriority();
            var targetStrength = GetObjects(i).GetTargetStrength();
            s = "(" + IntToStr(i) + "):" + idString + " (Priority:" + IntToStr(targetPriority) + "/Strength:" + IntToStr(targetStrength) + ")";
            GetEngine().OutputDebugMessage(DebugChannel.Combat, s);
        }
    }

    // Combat.pas:6052
    public void UpdateSettings()
    {
        int n;
        CombatObject objectValue;
        Strength = 0;
        ConquerCount = 0;
        for (int i = Count - 1; i >= 0; i--)
        {
            objectValue = this[i];
            n = objectValue.Targets.Count;
            if (n > 0)
            {
                Strength += objectValue.CombatValue / n;
            }

            if (objectValue.TargetedBy.Count > 0 && objectValue.AttackerCount == 0)
            {
                ConquerCount += 1;
            }
        }
    }

    // intf_P3_01.inc:352

}
