#nullable disable
// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class MindDecay : CombatSpell
{
    // CombatSpells.pas:1190
    public MindDecay(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_mindDecayText);
        SpellId = 106;
        var spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 5;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.None);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
        spellCombatInfo.Radius = 3;

        CombatInfo = spellCombatInfo;
    }

    // CombatSpells.pas:1207
    public override CombatAction CreateCombatAction(CombatObject source, CombatObject target, bool chargeMana)
    {
        MindDecayCombatAction action;
        action = new MindDecayCombatAction(null) { Holder = CreationHolder() };
        action.SourceId = source.Id;
        action.TargetId = target.Id;
        action.SpellId = SpellId;
        if (chargeMana)
        {
            action.ManaCost = BaseManaCost;
        }

        if (target is CombatUnit targetUnit && targetUnit.Troop.GetUnitType() != UnitType.Machine && targetUnit.Troop.GetRace() != 11 && !targetUnit.Troop.GetAbilityEnabled(129) && !(targetUnit.Troop is Leader))
        {
            var resistance = target.GetResistance();
            action.Succeeded = HitChance.HitRole(CombatInfo.Attack - resistance);
        }

        return action;
    }

    private static readonly string s_mindDecayText = "Mind Decay";
}
