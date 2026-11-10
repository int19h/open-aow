using Aow.Game.Combat;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

internal static class PassiveAbilities
{

    internal static MovementAbility CreateMovementAbility(int ID, string Name, Set<MoveType> MoveTypes)
    {
        MovementAbility Ability;
        Ability = new MovementAbility(null);
        Ability.FAbilityID = ID;
        Ability.FAbilityName = Name;
        Ability.FMoveTypes = MoveTypes;
        if (MoveTypes.Contains(MoveType.Mountaineering) || MoveTypes.Contains(MoveType.Forestry) || MoveTypes.Contains(MoveType.CaveCrawling))
        {
            Ability.FSelectionTypes = Ability.FSelectionTypes + Set<AbilitySelectionType>.Of(AbilitySelectionType.Editor);
        }

        return Ability;
    }

    internal static ImmunityAbility CreateImmunityAbility(int ID, string Name, Set<DamageType> DamageTypes)
    {
        ImmunityAbility Ability;
        Ability = new ImmunityAbility(null);
        Ability.FAbilityID = ID;
        Ability.FAbilityName = Name;
        Ability.FImmunityTypes = DamageTypes;
        if ((Set<DamageType>.Of(DamageType.Physical, DamageType.Death, DamageType.Holy, DamageType.Magic) & DamageTypes) == Set<DamageType>.Of())
        {
            Ability.FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.RingItem);
        }
        else
        {
            if ((Set<DamageType>.Of(DamageType.Death, DamageType.Holy, DamageType.Magic) & DamageTypes) == Set<DamageType>.Of())
            {
                Ability.FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.RingItem);
            }
            else
            {
                Ability.FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
            }
        }

        if ((Set<DamageType>.Of(DamageType.Physical, DamageType.Magic) & DamageTypes) == Set<DamageType>.Of())
        {
            Ability.FSelectionTypes = Ability.FSelectionTypes + Set<AbilitySelectionType>.Of(AbilitySelectionType.TorsoItem);
        }

        return Ability;
    }

    internal static ProtectionAbility CreateProtectionAbility(int ID, string Name, Set<DamageType> DamageTypes, Set<AbilitySelectionType> SelectionTypes)
    {
        ProtectionAbility Ability;
        Ability = new ProtectionAbility(null);
        Ability.FAbilityID = ID;
        Ability.FAbilityName = Name;
        Ability.FProtectionTypes = DamageTypes;
        Ability.FSelectionTypes = SelectionTypes;
        return Ability;
    }

}
