// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonMermaid : SummonSpell
{
    // SummonSpells.pas:488
    public SummonMermaid(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_summonMermaidText);
        SpellId = 78;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Death;
        UnitIndices.Add(218);
        CastDuration = 5;
        AIBasePriority = -1;
    }

    private static readonly string s_summonMermaidText = "Summon Mermaid";
}
