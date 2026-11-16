using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// The ability id selects one kind, and that kind overrides the member. An ability of another
// class throws InvalidCastException, as the call site cast did. A null ability fails at the
// call, as a cast of null did. A subclass of the kind inherits the override, as the cast
// accepted that subclass.
public partial class Ability
{
    public virtual ConstructionControlList GetConstructionControls() => throw new InvalidCastException();

    public virtual void RegisterConstructionControl(ConstructionControl control) => throw new InvalidCastException();

    public virtual bool CanTouch(CombatObject source, CombatObject target) => throw new InvalidCastException();

    public virtual Set<DamageType> GetDamageTypes(AbilityOwner owner) => throw new InvalidCastException();

    public virtual void CalculateFreeUnitSwing(Troop source, Troop target) => throw new InvalidCastException();

    public virtual int GetAttackerDamageValue() => throw new InvalidCastException();

    public virtual bool CanCastBreachWallSpell(Troop troop) => throw new InvalidCastException();

    public virtual sbyte GetAttackRepeatRangedAttack(AbilityOwner owner) => throw new InvalidCastException();

    public virtual RangedAttackCombatAction CreateRangedAttackCombatAction(CombatObject source, CombatObject target, int attackPenalty) => throw new InvalidCastException();

    public virtual HealingCombatAction CreateHealingCombatAction(CombatObject source, CombatObject target) => throw new InvalidCastException();

    public virtual bool HealUnit(Troop troop) => throw new InvalidCastException();

    public virtual void SetEnabled(AbilityOwner owner, bool enabled) => throw new InvalidCastException();

    public virtual int GetDispelMana(AbilityOwner owner) => throw new InvalidCastException();

    public virtual DispelMagicAbilityCombatAction CreateDispelMagicCombatAction(CombatUnit source, CombatObject target) => throw new InvalidCastException();

    public virtual EntangleCombatAction CreateEntangleCombatAction(CombatObject source, CombatObject target) => throw new InvalidCastException();

    public virtual WebCombatAction CreateWebCombatAction(CombatObject source, CombatObject target) => throw new InvalidCastException();

    public virtual void Command(CombatUnit source, CombatUnit target) => throw new InvalidCastException();

    public virtual CommandCombatAction CreateCommandCombatAction(CombatUnit source, CombatUnit target) => throw new InvalidCastException();

    public virtual void ResetCommandedUnits(CombatUnit source) => throw new InvalidCastException();

    public virtual void Uncommand(CombatUnit target) => throw new InvalidCastException();

    public virtual TurnUndeadCombatAction CreateTurnUndeadCombatAction(CombatObject source, CombatObject target) => throw new InvalidCastException();

    public virtual InvokeDeathCombatAction CreateInvokeDeathCombatAction(CombatObject source, CombatObject target) => throw new InvalidCastException();

    public virtual PossessCombatAction CreatePossessCombatAction(CombatObject source, CombatObject target) => throw new InvalidCastException();

    public virtual SelfDestructCombatAction CreateSelfDestructCombatAction(CombatObject source, CombatObject target) => throw new InvalidCastException();

    public virtual void SetExternalSource(AbilityOwner owner, byte level, string sourceName) => throw new InvalidCastException();

    public virtual void ResetExternalSource(AbilityOwner owner) => throw new InvalidCastException();

    public virtual int GetAIUpkeepPriority(AbilityOwner owner) => throw new InvalidCastException();

    public virtual int UnitIndexToAIUpkeepPriority(int unitIndex) => throw new InvalidCastException();
}
