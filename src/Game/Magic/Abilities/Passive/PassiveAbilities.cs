#nullable disable
using Aow.Game.Combat;
using Aow.Game.World.Movement;

namespace Aow.Game.Magic.Abilities.Passive;

internal static class PassiveAbilities
{

    internal static MovementAbility CreateMovementAbility(int id, string name, MoveTypes moveTypes, GameEngine engine)
    {
        MovementAbility ability = new(null)
        {
            Holder = engine,
            AbilityId = id,
            AbilityName = name,
            MoveTypes = moveTypes
        };
        if (moveTypes.Contains(MoveTypes.Mountaineering) || moveTypes.Contains(MoveTypes.Forestry) || moveTypes.Contains(MoveTypes.CaveCrawling))
        {
            ability.SelectionTypes |= AbilitySelectionTypes.Editor;
        }

        return ability;
    }

    internal static ImmunityAbility CreateImmunityAbility(int id, string name, DamageTypes damageTypes, GameEngine engine)
    {
        ImmunityAbility ability = new(null)
        {
            Holder = engine,
            AbilityId = id,
            AbilityName = name,
            ImmunityTypes = damageTypes,
            SelectionTypes = ((DamageTypes.Physical | DamageTypes.Death | DamageTypes.Holy | DamageTypes.Magic) & damageTypes) == DamageTypes.None
                ? AbilitySelectionTypes.Unit | AbilitySelectionTypes.RingItem
                : ((DamageTypes.Death | DamageTypes.Holy | DamageTypes.Magic) & damageTypes) == DamageTypes.None
                    ? AbilitySelectionTypes.Unit | AbilitySelectionTypes.RingItem
                    : AbilitySelectionTypes.Unit
        };

        if (((DamageTypes.Physical | DamageTypes.Magic) & damageTypes) == DamageTypes.None)
        {
            ability.SelectionTypes |= AbilitySelectionTypes.TorsoItem;
        }

        return ability;
    }

    internal static ProtectionAbility CreateProtectionAbility(int id, string name, DamageTypes damageTypes, AbilitySelectionTypes selectionTypes, GameEngine engine)
    {
        ProtectionAbility ability = new(null)
        {
            Holder = engine,
            AbilityId = id,
            AbilityName = name,
            ProtectionTypes = damageTypes,
            SelectionTypes = selectionTypes
        };
        return ability;
    }
}
