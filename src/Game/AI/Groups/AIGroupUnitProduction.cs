// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.AI.Groups;

// references, unit, production location, gold, mana, upkeep, production time, priority,
// research
public partial class AIGroupUnitProduction : EngineObject
{
    // AIGroup.pas:2389
    public AIGroupUnitProduction(ECustomNode owner) : base(owner)
    {
        RefCount = 1;
    }

    public int RefCount;
    public Unit Unit;
    public AIGroupProductionLocation ProductionLocation;
    public int Gold;
    public int Mana;
    public int GoldUpkeep;
    public int ManaUpkeep;
    public int ProductionTime;
    public int Priority;
    public bool Research;

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
        RefCount += 1;
        return RefCount;
    }

    // AIGroup.pas:2408
    public override int Release()
    {
        int count;
        RefCount -= 1;
        count = RefCount;
        int result = count;
        if (count == 0)
        {
            Dispose();
        }

        return result;
    }

    // AIGroup.pas:2415
    public void SetU(Unit U)
    {
        if (U != Unit)
        {
            if (Unit != null)
            {
                Unit.Release();
            }

            Unit = U;
            if (Unit != null)
            {
                Unit.AddRef();
            }
        }
    }

    // AIGroup.pas:2425
    public void SetProductionLocation(AIGroupProductionLocation location)
    {
        if (location != ProductionLocation)
        {
            if (ProductionLocation != null)
            {
                ProductionLocation.Release();
            }

            ProductionLocation = location;
            if (ProductionLocation != null)
            {
                ProductionLocation.AddRef();
            }
        }
    }

    // intf_P5_07.inc:485

}
