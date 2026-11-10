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

    internal static TMovementAbility CreateMovementAbility(int ID, string Name, TSet<TMoveType> MoveTypes)
    {
        TMovementAbility Ability;
        Ability = new TMovementAbility(null);
        Ability.FAbilityID = ID;
        Ability.FAbilityName = Name;
        Ability.FMoveTypes = MoveTypes;
        if (MoveTypes.Contains(TMoveType.mtMountaineering) || MoveTypes.Contains(TMoveType.mtForestry) || MoveTypes.Contains(TMoveType.mtCaveCrawling))
        {
            Ability.FSelectionTypes = Ability.FSelectionTypes + TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of(Aow.Game.Magic.Abilities.TAbilitySelectionType.astEditor);
        }

        return Ability;
    }

    internal static TImmunityAbility CreateImmunityAbility(int ID, string Name, TSet<TDamageType> DamageTypes)
    {
        TImmunityAbility Ability;
        Ability = new TImmunityAbility(null);
        Ability.FAbilityID = ID;
        Ability.FAbilityName = Name;
        Ability.FImmunityTypes = DamageTypes;
        if ((TSet<TDamageType>.Of(TDamageType.dtPhysical, TDamageType.dtDeath, TDamageType.dtHoly, TDamageType.dtMagic) & DamageTypes) == TSet<TDamageType>.Of())
        {
            Ability.FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of(Aow.Game.Magic.Abilities.TAbilitySelectionType.astUnit, Aow.Game.Magic.Abilities.TAbilitySelectionType.astRingItem);
        }
        else
        {
            if ((TSet<TDamageType>.Of(TDamageType.dtDeath, TDamageType.dtHoly, TDamageType.dtMagic) & DamageTypes) == TSet<TDamageType>.Of())
            {
                Ability.FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of(Aow.Game.Magic.Abilities.TAbilitySelectionType.astUnit, Aow.Game.Magic.Abilities.TAbilitySelectionType.astRingItem);
            }
            else
            {
                Ability.FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of(Aow.Game.Magic.Abilities.TAbilitySelectionType.astUnit);
            }
        }

        if ((TSet<TDamageType>.Of(TDamageType.dtPhysical, TDamageType.dtMagic) & DamageTypes) == TSet<TDamageType>.Of())
        {
            Ability.FSelectionTypes = Ability.FSelectionTypes + TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of(Aow.Game.Magic.Abilities.TAbilitySelectionType.astTorsoItem);
        }

        return Ability;
    }

    internal static TProtectionAbility CreateProtectionAbility(int ID, string Name, TSet<TDamageType> DamageTypes, TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType> SelectionTypes)
    {
        TProtectionAbility Ability;
        Ability = new TProtectionAbility(null);
        Ability.FAbilityID = ID;
        Ability.FAbilityName = Name;
        Ability.FProtectionTypes = DamageTypes;
        Ability.FSelectionTypes = SelectionTypes;
        return Ability;
    }

}
