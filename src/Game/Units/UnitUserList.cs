// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

// inherited FChildren
public partial class UnitUserList : UnitList
{

    public UnitUserList(ECustomNode owner) : base(owner)
    { }

    // Unit.PAS:2380
    public override void Clear()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            this[I].Release();
        }

        ClearChildren();
    }

    // Unit.PAS:2386
    public override bool AddUnit(Troop troop)
    {
        if (CanAddUnit(troop))
        {
            AddChild(troop);
            return true;
        }

        return false;
    }

    // Unit.PAS:2396
    public override bool DeleteUnit(Troop troop)
    {
        if (CanDeleteUnit(troop))
        {
            RemoveChild(troop);
            return true;
        }

        return false;
    }

    // Unit.PAS:2406
    public void AddUnits(UnitList units, byte selection)
    {
        for (int I = 0, last = units.Count - 1; I <= last; I++)
        {
            if (((1 << I) & selection) != 0)
            {
                AddUnit(units.GetUnit(I));
            }
        }
    }

    // Engine.pas:680

}
