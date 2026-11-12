// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonSyron : SummonSpell
{
    // SummonSpells.pas:328
    public SummonSyron(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_summonSyronText);
        SpellId = 24;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Cosmos;
        UnitIndices.Add(221);
        CastDuration = 40;
        AIBasePriority = 500;
    }

    private static readonly string s_summonSyronText = "Summon Syron";
}
