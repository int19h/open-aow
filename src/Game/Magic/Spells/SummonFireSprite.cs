// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonFireSprite : SummonSpell
{
    // SummonSpells.pas:457
    public SummonFireSprite(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_summonFireSpriteText);
        SpellId = 21;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Fire;
        UnitIndices.Add(228);
        CastDuration = 10;
        AIBasePriority = 200;
    }

    private static readonly string s_summonFireSpriteText = "Summon Fire Sprite";
}
