// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonBlackSpider : SummonSpell
{
    // SummonSpells.pas:475
    public SummonBlackSpider(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_summonBlackSpiderText);
        SpellId = 20;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Death;
        UnitIndices.Add(222);
        CastDuration = 5;
        AIBasePriority = 200;
    }

    // SummonSpells.pas:470
    public override void SetupSummonSpellTokenEvent(SummonSpellTokenEvent eventValue)
    {
        base.SetupSummonSpellTokenEvent(eventValue);
    }

    private static readonly string s_summonBlackSpiderText = "Summon Black Spider";
}
