// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public class SummonBoar : SummonSpell
{
    // SummonSpells.pas:354
    public SummonBoar(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_summonBoarText);
        SpellId = 31;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Earth;
        _ = UnitIndices.Add(235);
        CastDuration = 5;
    }

    private static readonly string s_summonBoarText = "Summon Boar";
}
