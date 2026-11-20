// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public class SummonGreatEagle : SummonSpell
{
    // SummonSpells.pas:444
    public SummonGreatEagle(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_summonGreatEagleText);
        SpellId = 23;
        BaseManaCost = 5;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Air;
        _ = UnitIndices.Add(234);
        CastDuration = 10;
        AIBasePriority = 250;
    }

    private static readonly string s_summonGreatEagleText = "Summon Great Eagle";
}
