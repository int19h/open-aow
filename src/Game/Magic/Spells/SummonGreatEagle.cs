// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonGreatEagle : SummonSpell
{
    // SummonSpells.pas:444
    public SummonGreatEagle(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_summonGreatEagleText);
        SpellId = 23;
        BaseManaCost = 5;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Air;
        UnitIndices.Add(234);
        CastDuration = 10;
        AIBasePriority = 250;
    }

    private static readonly string s_summonGreatEagleText = "Summon Great Eagle";
}
