// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonGoldDragon : SummonSpell
{
    // SummonSpells.pas:418
    public SummonGoldDragon(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_summonGoldDragonText);
        SpellId = 25;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Life;
        UnitIndices.Add(216);
        CastDuration = 40;
        AIBasePriority = 750;
    }

    private static readonly string s_summonGoldDragonText = "Summon Gold Dragon";
}
