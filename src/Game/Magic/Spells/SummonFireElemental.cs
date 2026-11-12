// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonFireElemental : SummonSpell
{
    // SummonSpells.pas:392
    public SummonFireElemental(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_summonFireElementalText);
        SpellId = 29;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Fire;
        UnitIndices.Add(226);
        CastDuration = 40;
        AIBasePriority = 500;
    }

    private static readonly string s_summonFireElementalText = "Summon Fire Elemental";
}
