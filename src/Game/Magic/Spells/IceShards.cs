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
    public IceShards(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(IceShardsRStr);
        FSpellID = 119;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 4;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
        spellCombatInfo.RepeatCount = 3;
        spellCombatInfo.Range = 12;
    }

    private static readonly string IceShardsRStr = "Ice Shards";
}
