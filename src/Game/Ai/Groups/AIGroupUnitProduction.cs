// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.Ai.Groups;

// references, unit, production location, gold, mana, upkeep, production time, priority,
// research
public partial class AIGroupUnitProduction : EObject
{
    // AIGroup.pas:2389
    public AIGroupUnitProduction(ECustomNode owner) : base(owner)
    {
        FRefCount = 1;
    }

    public int FRefCount;
    public Unit FU;
    public AIGroupProductionLocation FProductionLocation;
    public int FGold;
    public int FMana;
    public int FGoldUpkeep;
    public int FManaUpkeep;
    public int FProductionTime;
    public int FPriority;
    public bool FResearch;

    // AIGroup.pas:2395
    public override void Dispose()
    {
        SetU(null);
        SetProductionLocation(null);
        base.Dispose();
    }

    // AIGroup.pas:2402
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // AIGroup.pas:2408
    public override int Release()
    {
        int Count;
        FRefCount -= 1;
        Count = FRefCount;
        int Result = Count;
        if (Count == 0)
        {
            Dispose();
        }

        return Result;
    }

    // AIGroup.pas:2415
    public void SetU(Unit U)
    {
        if (U != FU)
        {
            if (FU != null)
            {
                FU.Release();
            }

            FU = U;
            if (FU != null)
            {
                FU.AddRef();
            }
        }
    }

    // AIGroup.pas:2425
    public void SetProductionLocation(AIGroupProductionLocation Location)
    {
        if (Location != FProductionLocation)
        {
            if (FProductionLocation != null)
            {
                FProductionLocation.Release();
            }

            FProductionLocation = Location;
            if (FProductionLocation != null)
            {
                FProductionLocation.AddRef();
            }
        }
    }

    // intf_P5_07.inc:485

}
