// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public class SummonFireSprite : SummonSpell
{
    // SummonSpells.pas:457
    public SummonFireSprite(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_summonFireSpriteText);
        SpellId = 21;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Fire;
        _ = UnitIndices.Add(228);
        CastDuration = 10;
        AIBasePriority = 200;
    }

    private static readonly string s_summonFireSpriteText = "Summon Fire Sprite";
}
