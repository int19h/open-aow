// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public class SummonEarthElemental : SummonSpell
{
    // SummonSpells.pas:379
    public SummonEarthElemental(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_summonEarthElementalText);
        SpellId = 28;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Earth;
        _ = UnitIndices.Add(225);
        CastDuration = 40;
        AIBasePriority = 750;
    }

    private static readonly string s_summonEarthElementalText = "Summon Earth Elemental";
}
