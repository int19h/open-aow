// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonWaterElemental : SummonSpell
{
    // SummonSpells.pas:405
    public SummonWaterElemental(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_summonWaterElementalText);
        SpellId = 30;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Water;
        UnitIndices.Add(227);
        CastDuration = 40;
        AIBasePriority = 500;
    }

    private static readonly string s_summonWaterElementalText = "Summon Water Elemental";
}
