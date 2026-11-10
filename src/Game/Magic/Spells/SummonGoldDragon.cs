// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonGoldDragon : SummonSpell
{
    // SummonSpells.pas:418
    public SummonGoldDragon(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(SummonGoldDragonRStr);
        FSpellID = 25;
        FBaseManaCost = 5;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Life;
        FUnitIndices.Add(216);
        FCastDuration = 40;
        FAIBasePriority = 750;
    }

    private static readonly string SummonGoldDragonRStr = "Summon Gold Dragon";
}
