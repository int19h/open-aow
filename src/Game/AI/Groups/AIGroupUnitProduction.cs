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
        _refCount = 1;
    }

    private int _refCount;

    public Unit Unit { get; private set; }
    public AIGroupProductionLocation ProductionLocation { get; private set; }
    public int Gold { get; set; }
    public int Mana { get; set; }
    public int GoldUpkeep { get; set; }
    public int ManaUpkeep { get; set; }
    public int ProductionTime { get; set; }
    public int Priority { get; set; }
    public bool Research { get; set; }

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
        _refCount += 1;
        return _refCount;
    }

    // AIGroup.pas:2408
    public override int Release()
    {
        int count;
        _refCount -= 1;
        count = _refCount;
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
