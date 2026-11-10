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
            ((EObject)FChildren.List[I]).Release();
        }

        FChildren.Clear();
    }

    // Unit.PAS:2386
    public override bool AddUnit(AbstractUnit AUnit)
    {
        if (CanAddUnit(AUnit))
        {
            AddChild(AUnit);
            return true;
        }

        return false;
    }

    // Unit.PAS:2396
    public override bool DeleteUnit(AbstractUnit AUnit)
    {
        if (CanDeleteUnit(AUnit))
        {
            RemoveChild(AUnit);
            return true;
        }

        return false;
    }

    // Unit.PAS:2406
    public void AddUnitsEx(UnitList Units, byte Selection)
    {
        for (int I = 0, last = Units.Count - 1; I <= last; I++)
        {
            if (((1 << I) & Selection) != 0)
            {
                AddUnit(Units.GetUnit(I));
            }
        }
    }

    // Engine.pas:680

}
