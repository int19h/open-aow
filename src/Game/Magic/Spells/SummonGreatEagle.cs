// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonGreatEagle : SummonSpell
{
    // SummonSpells.pas:444
    public SummonGreatEagle(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(SummonGreatEagleRStr);
        FSpellID = 23;
        FBaseManaCost = 5;
        FSpellLevel = 2;
        FMagicSphere = MagicSphere.Air;
        FUnitIndices.Add(234);
        FCastDuration = 10;
        FAIBasePriority = 250;
    }

    private static readonly string SummonGreatEagleRStr = "Summon Great Eagle";
}
