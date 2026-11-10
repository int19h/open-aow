// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonMermaid : SummonSpell
{
    // SummonSpells.pas:488
    public SummonMermaid(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(SummonMermaidRStr);
        FSpellID = 78;
        FBaseManaCost = 5;
        FSpellLevel = 1;
        FMagicSphere = MagicSphere.Death;
        FUnitIndices.Add(218);
        FCastDuration = 5;
        FAIBasePriority = -1;
    }

    private static readonly string SummonMermaidRStr = "Summon Mermaid";
}
