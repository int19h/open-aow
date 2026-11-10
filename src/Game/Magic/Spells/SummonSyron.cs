// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonSyron : SummonSpell
{
    // SummonSpells.pas:328
    public SummonSyron(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(SummonSyronRStr);
        FSpellID = 24;
        FBaseManaCost = 5;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Cosmos;
        FUnitIndices.Add(221);
        FCastDuration = 40;
        FAIBasePriority = 500;
    }

    private static readonly string SummonSyronRStr = "Summon Syron";
}
