// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class IceShards : CombatSpell
{
    // CombatSpells.pas:1537
    public IceShards(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_iceShardsText);
        SpellId = 119;
        ref var spellCombatInfo = ref CombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 4;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
        spellCombatInfo.RepeatCount = 3;
        spellCombatInfo.Range = 12;
    }

    private static readonly string s_iceShardsText = "Ice Shards";
}
