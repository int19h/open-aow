// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonAirElemental : SummonSpell
{
    // SummonSpells.pas:366
    public SummonAirElemental(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_summonAirElementalText);
        SpellId = 27;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Air;
        UnitIndices.Add(224);
        CastDuration = 40;
        AIBasePriority = 750;
    }

    private static readonly string s_summonAirElementalText = "Summon Air Elemental";
}
