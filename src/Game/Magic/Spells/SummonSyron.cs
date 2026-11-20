// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public class SummonSyron : SummonSpell
{
    // SummonSpells.pas:328
    public SummonSyron(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_summonSyronText);
        SpellId = 24;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Cosmos;
        _ = UnitIndices.Add(221);
        CastDuration = 40;
        AIBasePriority = 500;
    }

    private static readonly string s_summonSyronText = "Summon Syron";
}
