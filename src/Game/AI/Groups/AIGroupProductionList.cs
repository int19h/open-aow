// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// { Interface declaration run owned by P5. }
// children
public partial class AIGroupProductionList : OwningNode, IChildList<AIGroupProduction>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AIGroupProduction, so the cast never fails.
    public AIGroupProduction this[int index] => (AIGroupProduction)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIGroupProduction;

    int IChildSink<AIGroupProduction>.AddChild(AIGroupProduction child) => AddChild(child);

    public AIGroupProductionList(CustomNode owner) : base(owner)
    { }

    // AIGroup.pas:1
    public AIGroupProduction GetProductions(int index) => this[index];

    // AIGroup.pas:6
    public int GetStrengthMax(Set<DamageType> damageTypes)
    {
        int result = 0;
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            result += this[i].Troop.GetStrengthMax(damageTypes);
        }

        return result;
    }

    // AIGroup.pas:15
    public void Add(ProductionControl control, Troop troop)
    {
        var aiGroupProduction = new AIGroupProduction(this);
        aiGroupProduction.Control = control;
        aiGroupProduction.Troop = troop;
    }

    // AIGroup.pas:24
    public void Delete(int index) => GetProductions(index)?.Dispose();

    // AIGroup.pas:29
    public void DeleteUnit(Troop troop)
    {
        int i;
        i = IndexOfU(troop);
        if (i != -1)
        {
            GetProductions(i)?.Dispose();
        }
    }

    // AIGroup.pas:37
    public void ChangeGroup(int index, AIGroup group)
    {
        var aiGroupProduction = GetProductions(index);
        aiGroupProduction.Control.Settings.GroupId = group.GroupId;
        aiGroupProduction.SetOwner(group.Productions);
    }

    // AIGroup.pas:46
    public int IndexOfU(Troop troop)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (troop == this[result].Troop)
            {
                return result;
            }
        }

        return -1;
    }

    // Engine.pas:631

}
