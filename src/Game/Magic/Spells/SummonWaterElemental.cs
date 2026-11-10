// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonWaterElemental : SummonSpell
{
    // SummonSpells.pas:405
    public SummonWaterElemental(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(SummonWaterElementalRStr);
        FSpellID = 30;
        FBaseManaCost = 5;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Water;
        FUnitIndices.Add(227);
        FCastDuration = 40;
        FAIBasePriority = 500;
    }

    private static readonly string SummonWaterElementalRStr = "Summon Water Elemental";
}
