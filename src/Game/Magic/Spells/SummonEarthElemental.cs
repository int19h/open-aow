// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonEarthElemental : SummonSpell
{
    // SummonSpells.pas:379
    public SummonEarthElemental(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_summonEarthElementalText);
        SpellId = 28;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Earth;
        UnitIndices.Add(225);
        CastDuration = 40;
        AIBasePriority = 750;
    }

    private static readonly string s_summonEarthElementalText = "Summon Earth Elemental";
}
