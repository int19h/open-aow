// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class DiseaseCloud : CombatSpell
{
    // CombatSpells.pas:1146
    public DiseaseCloud(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(DiseaseCloudRStr);
        FSpellID = 104;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 3;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Death);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
    }

    private static readonly string DiseaseCloudRStr = "Disease Cloud";
}
