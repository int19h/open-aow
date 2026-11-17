#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

public partial class UnitSelectionActivation : AbilityActivation
{
    public UnitSelectionActivation(CustomNode owner) : base(owner) => UnitSelector = null;

    public UnitSelector UnitSelector { get; private set; }

    // Ability.PAS:696
    public override void Dispose()
    {
        UnitSelector selector;
        selector = UnitSelector;
        if (selector != null)
        {
            _ = selector.Release();
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
            _ = (old?.Release());

            old = value;
            UnitSelector = old;
            _ = (old?.AddRef());
        }
    }

    // intf_P1_08.inc:76

}
