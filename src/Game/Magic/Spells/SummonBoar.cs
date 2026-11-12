// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonBoar : SummonSpell
{
    // SummonSpells.pas:354
    public SummonBoar(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_summonBoarText);
        SpellId = 31;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Earth;
        UnitIndices.Add(235);
        CastDuration = 5;
    }

    private static readonly string s_summonBoarText = "Summon Boar";
}
