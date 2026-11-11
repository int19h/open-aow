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
    public AIGroupProduction GetProductions(int Index) => (AIGroupProduction)FChildren.List[Index];

    // AIGroup.pas:6
    public int GetStrengthMax(Set<DamageType> DamageTypes)
    {
        int Result = 0;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            Result += ((AIGroupProduction)FChildren.List[I]).FUnit.GetStrengthMax(DamageTypes);
        }

        return Result;
    }

    // AIGroup.pas:15
    public void Add(ProductionControl Control, Troop AUnit)
    {
        var aiGroupProduction = new AIGroupProduction(this);
        aiGroupProduction.FControl = Control;
        aiGroupProduction.FUnit = AUnit;
    }

    // AIGroup.pas:24
    public void Delete(int Index)
    {
        GetProductions(Index)?.Dispose();
    }

    // AIGroup.pas:29
    public void DeleteUnit(Troop AUnit)
    {
        int I;
        I = IndexOfU(AUnit);
        if (I != -1)
        {
            GetProductions(I)?.Dispose();
        }
    }

    // AIGroup.pas:37
    public void ChangeGroup(int Index, AIGroup Group)
    {
        var aiGroupProduction = GetProductions(Index);
        aiGroupProduction.FControl.FSettings.FGroupID = Group.FGroupID;
        aiGroupProduction.SetOwner(Group.FProductions);
    }

    // AIGroup.pas:46
    public int IndexOfU(Troop AUnit)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (AUnit == ((AIGroupProduction)FChildren.List[Result]).FUnit)
            {
                return Result;
            }
        }

        return -1;
    }

    // Engine.pas:631

}
