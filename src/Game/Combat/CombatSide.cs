#nullable disable warnings
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.Combat;

// SideIndex. Opponent. Strength. ConquerCount
public sealed class CombatSide(CustomNode? owner) : OwningNode(owner), IChildList<CombatObject>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a CombatObject, so the cast never fails.
    public CombatObject this[int index] => GetChild<CombatObject>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is CombatObject;

    int IChildSink<CombatObject>.AddChild(CombatObject child) => AddChild(child);

    public int SideIndex { get; set; }
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
            if (this[i].Status.Contains(CombatObjectStatuses.Destroyed))
            {
                result += 1;
            }
        }

        return result;
    }

    // Combat.pas:6026
    public int GetUndestroyedConquerObjects()
    {
        int result = 0;
        for (int i = Count - 1; i >= 0; i--)
        {
            CombatObject objectValue = this[i];
            if (((CombatObjectStatuses.Destroyed | CombatObjectStatuses.Fled) & objectValue.Status) == CombatObjectStatuses.None && objectValue.ControlMode.Contains(CombatObjectControlModes.ConquerObject))
            {
                result += 1;
            }
        }

        return result;
    }

    // Combat.pas:6038
    public void OutputDebugInfo()
    {
        if (SideIndex == 0)
        {
            GetEngine().OutputDebugMessage(DebugChannel.Combat, S: "--- Combat Side Attacker ---");
        }
        else
        {
            GetEngine().OutputDebugMessage(DebugChannel.Combat, S: "--- Combat Side Defender ---");
        }

        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            string idString = GetObjects(i).IdString();
            int targetPriority = GetObjects(i).GetTargetPriority();
            int targetStrength = GetObjects(i).GetTargetStrength();
            string s = $"({i.ToString(CultureInfo.InvariantCulture)}):{idString} (Priority:{targetPriority.ToString(CultureInfo.InvariantCulture)}/Strength:{targetStrength.ToString(CultureInfo.InvariantCulture)})";
            GetEngine().OutputDebugMessage(DebugChannel.Combat, s);
        }
    }

    // Combat.pas:6052
    public void UpdateSettings()
    {
        Strength = 0;
        ConquerCount = 0;
        foreach (CombatObject objectValue in this.Reverse())
        {
            int n = objectValue.Targets.Count;
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
}
