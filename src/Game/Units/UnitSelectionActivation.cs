#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

public sealed class UnitSelectionActivation(CustomNode? owner) : AbilityActivation(owner)
{

    public UnitSelector? UnitSelector { get; private set; }

    // Ability.PAS:696
    public override void Dispose()
    {
        UnitSelector? selector = UnitSelector;
        if (selector is not null)
        {
            _ = selector.Release();
            UnitSelector = null;
        }

        base.Dispose();
    }

    // Ability.PAS:709
    public void SetUnitSelector(UnitSelector value)
    {
        UnitSelector? old = UnitSelector;
        if (old != value)
        {
            _ = (old?.Release());

            old = value;
            UnitSelector = old;
            _ = (old?.AddRef());
        }
    }
}
