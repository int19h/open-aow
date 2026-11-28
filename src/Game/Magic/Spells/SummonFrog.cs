// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public class SummonFrog : SummonSpell
{
    // SummonSpells.pas:341
    public SummonFrog(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_summonFrogText);
        SpellId = 22;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Water;
        _ = UnitIndices.Add(236);
        CastDuration = 5;
        AIBasePriority = 200;
    }

    private static readonly string s_summonFrogText = "Summon Frog";
}
