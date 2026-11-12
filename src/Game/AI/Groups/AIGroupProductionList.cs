// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// { Interface declaration run owned by P5. }
// children
public partial class AIGroupProductionList : ENode
{
    public AIGroupProductionList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:1
    public AIGroupProduction GetProductions(int index) => (AIGroupProduction)_children.List[index];

    // AIGroup.pas:6
    public int GetStrengthMax(Set<DamageType> damageTypes)
    {
        int result = 0;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            result += ((AIGroupProduction)_children.List[I]).Troop.GetStrengthMax(damageTypes);
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
    public void Delete(int index)
    {
        GetProductions(index)?.Dispose();
    }

    // AIGroup.pas:29
    public void DeleteUnit(Troop troop)
    {
        int I;
        I = IndexOfU(troop);
        if (I != -1)
        {
            GetProductions(I)?.Dispose();
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
            if (troop == ((AIGroupProduction)_children.List[result]).Troop)
            {
                return result;
            }
        }

        return -1;
    }

    // Engine.pas:631

}
