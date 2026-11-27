// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public class SummonBlackDragon : SummonSpell
{
    // SummonSpells.pas:431
    public SummonBlackDragon(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_summonBlackDragonText);
        SpellId = 26;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Death;
        _ = UnitIndices.Add(217);
        CastDuration = 40;
        AIBasePriority = 750;
    }

    private static readonly string s_summonBlackDragonText = "Summon Black Dragon";
}
