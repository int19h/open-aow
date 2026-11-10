// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonEarthElemental : SummonSpell
{
    // SummonSpells.pas:379
    public SummonEarthElemental(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(SummonEarthElementalRStr);
        FSpellID = 28;
        FBaseManaCost = 5;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Earth;
        FUnitIndices.Add(225);
        FCastDuration = 40;
        FAIBasePriority = 750;
    }

    private static readonly string SummonEarthElementalRStr = "Summon Earth Elemental";
}
