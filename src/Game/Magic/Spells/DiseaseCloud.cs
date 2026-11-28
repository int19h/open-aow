// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public class DiseaseCloud : CombatSpell
{
    // CombatSpells.pas:1146
    public DiseaseCloud(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_diseaseCloudText);
        SpellId = 104;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 3;
        spellCombatInfo.DamageTypes = DamageTypes.Death;
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;

        CombatInfo = spellCombatInfo;
    }

    private static readonly string s_diseaseCloudText = "Disease Cloud";
}
