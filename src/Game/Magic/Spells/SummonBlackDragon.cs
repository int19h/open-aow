// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonBlackDragon : SummonSpell
{
    // SummonSpells.pas:431
    public SummonBlackDragon(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(SummonBlackDragonRStr);
        FSpellID = 26;
        FBaseManaCost = 5;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Death;
        FUnitIndices.Add(217);
        FCastDuration = 40;
        FAIBasePriority = 750;
    }

    private static readonly string SummonBlackDragonRStr = "Summon Black Dragon";
}
