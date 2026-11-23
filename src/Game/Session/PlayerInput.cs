using Aow.Game.Combat;
using Aow.Game.Magic.Spells;

namespace Aow.Game.Session;

// The inputs a host installs on the engine.
public sealed class PlayerInput : IPlayerInput
{
    private PlayerInput()
    {
    }

    // The input of a host that takes no player choice. A call fails at once, so a path that
    // asks by mistake stops instead of waiting for an answer that never arrives.
    public static PlayerInput Unattended { get; } = new();

    // The call hands the caster to this input, so a refused call releases it before it throws.
    public void ChooseSpellTargets(SpellCaster caster)
    {
        _ = caster.Release();
        throw new InvalidOperationException("No player input is installed for a strategic spell choice.");
    }

    public void ChooseCombatSpellTarget(CombatSpellCaster caster)
    {
        _ = caster.Release();
        throw new InvalidOperationException("No player input is installed for a combat spell choice.");
    }
}
