// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonFireSprite : SummonSpell
{
    // SummonSpells.pas:457
    public SummonFireSprite(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(SummonFireSpriteRStr);
        FSpellID = 21;
        FBaseManaCost = 5;
        FSpellLevel = 1;
        FMagicSphere = MagicSphere.Fire;
        FUnitIndices.Add(228);
        FCastDuration = 10;
        FAIBasePriority = 200;
    }

    private static readonly string SummonFireSpriteRStr = "Summon Fire Sprite";
}
