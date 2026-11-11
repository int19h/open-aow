// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonBlackSpider : SummonSpell
{
    // SummonSpells.pas:475
    public SummonBlackSpider(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(SummonBlackSpiderRStr);
        FSpellID = 20;
        FBaseManaCost = 5;
        FSpellLevel = 1;
        FMagicSphere = MagicSphere.Death;
        FUnitIndices.Add(222);
        FCastDuration = 5;
        FAIBasePriority = 200;
    }

    // SummonSpells.pas:470
    public override void SetupSummonSpellTE(SummonSpellTokenEvent Event)
    {
        base.SetupSummonSpellTE(Event);
    }

    private static readonly string SummonBlackSpiderRStr = "Summon Black Spider";
}
