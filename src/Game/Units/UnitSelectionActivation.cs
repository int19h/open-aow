#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

public partial class UnitSelectionActivation : AbilityActivation
{
    // Ability.PAS:690
    public UnitSelectionActivation(CustomNode owner) : base(owner) => UnitSelector = null;

    public UnitSelector UnitSelector { get; private set; }

    // Ability.PAS:696
    public override void Dispose()
    {
        UnitSelector selector;
        selector = UnitSelector;
        if (selector != null)
        {
            selector.Release();
            UnitSelector = null;
        }

        base.Dispose();
    }

    // Ability.PAS:709
    public void SetUnitSelector(UnitSelector value)
    {
        UnitSelector old;
        old = UnitSelector;
        if (old != value)
        {
            old?.Release();

            old = value;
            UnitSelector = old;
            old?.AddRef();
        }
    }

    // intf_P1_08.inc:76

}
