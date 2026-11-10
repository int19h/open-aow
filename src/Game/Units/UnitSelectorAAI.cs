// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

public partial class UnitSelectorAAI : ActivateAbilityInfo
{
    // Ability.PAS:690
    public UnitSelectorAAI(ECustomNode owner) : base(owner)
    {
        FUnitSelector = null;
    }

    public UnitSelector FUnitSelector;

    // Ability.PAS:696
    public override void Dispose()
    {
        UnitSelector Selector;
        Selector = FUnitSelector;
        if (Selector != null)
        {
            Selector.Release();
            FUnitSelector = null;
        }

        base.Dispose();
    }

    // Ability.PAS:709
    public void SetUnitSelector(UnitSelector Value)
    {
        UnitSelector Old;
        Old = FUnitSelector;
        if (Old != Value)
        {
            if (Old != null)
            {
                Old.Release();
            }

            Old = Value;
            FUnitSelector = Old;
            if (Old != null)
            {
                Old.AddRef();
            }
        }
    }

    // intf_P1_08.inc:76

}
