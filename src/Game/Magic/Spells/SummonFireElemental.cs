// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public class SummonFireElemental : SummonSpell
{
    // SummonSpells.pas:392
    public SummonFireElemental(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_summonFireElementalText);
        SpellId = 29;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Fire;
        _ = UnitIndices.Add(226);
        CastDuration = 40;
        AIBasePriority = 500;
    }

    private static readonly string s_summonFireElementalText = "Summon Fire Elemental";
}
