// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonFrog : SummonSpell
{
    // SummonSpells.pas:341
    public SummonFrog(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(SummonFrogRStr);
        FSpellID = 22;
        FBaseManaCost = 5;
        FSpellLevel = 1;
        FMagicSphere = MagicSphere.Water;
        FUnitIndices.Add(236);
        FCastDuration = 5;
        FAIBasePriority = 200;
    }

    private static readonly string SummonFrogRStr = "Summon Frog";
}
