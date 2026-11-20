#nullable disable
using Aow.Game.Combat;

using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

internal static class PassiveAbilities
{

    internal static MovementAbility CreateMovementAbility(int id, string name, Set<MoveType> moveTypes, GameEngine engine)
    {
        MovementAbility ability = new(null)
        {
            Holder = engine,
            AbilityId = id,
            AbilityName = name,
            MoveTypes = moveTypes
        };
        if (moveTypes.Contains(MoveType.Mountaineering) || moveTypes.Contains(MoveType.Forestry) || moveTypes.Contains(MoveType.CaveCrawling))
        {
            ability.SelectionTypes += Set<AbilitySelectionType>.Of(AbilitySelectionType.Editor);
        }

        return ability;
    }

    internal static ImmunityAbility CreateImmunityAbility(int id, string name, Set<DamageType> damageTypes, GameEngine engine)
    {
        ImmunityAbility ability = new(null)
        {
            Holder = engine,
            AbilityId = id,
            AbilityName = name,
            ImmunityTypes = damageTypes,
            SelectionTypes = (Set<DamageType>.Of(DamageType.Physical, DamageType.Death, DamageType.Holy, DamageType.Magic) & damageTypes) == Set<DamageType>.Of()
                ? Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.RingItem)
                : (Set<DamageType>.Of(DamageType.Death, DamageType.Holy, DamageType.Magic) & damageTypes) == Set<DamageType>.Of()
                    ? Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.RingItem)
                    : Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit)
        };

        if ((Set<DamageType>.Of(DamageType.Physical, DamageType.Magic) & damageTypes) == Set<DamageType>.Of())
        {
            ability.SelectionTypes += Set<AbilitySelectionType>.Of(AbilitySelectionType.TorsoItem);
        }

        return ability;
    }

    internal static ProtectionAbility CreateProtectionAbility(int id, string name, Set<DamageType> damageTypes, Set<AbilitySelectionType> selectionTypes, GameEngine engine)
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
