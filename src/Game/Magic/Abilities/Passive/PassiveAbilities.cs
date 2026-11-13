using Aow.Game.Combat;

using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

internal static class PassiveAbilities
{

    internal static MovementAbility CreateMovementAbility(int id, string name, Set<MoveType> moveTypes, GameEngine engine)
    {
        MovementAbility ability;
        ability = new MovementAbility(null) { Holder = engine };
        ability.AbilityId = id;
        ability.AbilityName = name;
        ability.MoveTypes = moveTypes;
        if (moveTypes.Contains(MoveType.Mountaineering) || moveTypes.Contains(MoveType.Forestry) || moveTypes.Contains(MoveType.CaveCrawling))
        {
            ability.SelectionTypes = ability.SelectionTypes + Set<AbilitySelectionType>.Of(AbilitySelectionType.Editor);
        }

        return ability;
    }

    internal static ImmunityAbility CreateImmunityAbility(int id, string name, Set<DamageType> damageTypes, GameEngine engine)
    {
        ImmunityAbility ability;
        ability = new ImmunityAbility(null) { Holder = engine };
        ability.AbilityId = id;
        ability.AbilityName = name;
        ability.ImmunityTypes = damageTypes;
        if ((Set<DamageType>.Of(DamageType.Physical, DamageType.Death, DamageType.Holy, DamageType.Magic) & damageTypes) == Set<DamageType>.Of())
        {
            ability.SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.RingItem);
        }
        else
        {
            if ((Set<DamageType>.Of(DamageType.Death, DamageType.Holy, DamageType.Magic) & damageTypes) == Set<DamageType>.Of())
            {
                ability.SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.RingItem);
            }
            else
            {
                ability.SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
            }
        }

        if ((Set<DamageType>.Of(DamageType.Physical, DamageType.Magic) & damageTypes) == Set<DamageType>.Of())
        {
            ability.SelectionTypes = ability.SelectionTypes + Set<AbilitySelectionType>.Of(AbilitySelectionType.TorsoItem);
        }

        return ability;
    }

    internal static ProtectionAbility CreateProtectionAbility(int id, string name, Set<DamageType> damageTypes, Set<AbilitySelectionType> selectionTypes, GameEngine engine)
    {
        ProtectionAbility ability;
        ability = new ProtectionAbility(null) { Holder = engine };
        ability.AbilityId = id;
        ability.AbilityName = name;
        ability.ProtectionTypes = damageTypes;
        ability.SelectionTypes = selectionTypes;
        return ability;
    }

}
