// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class PestilenceStormSpell : StormSpell
{
    // Storms.pas:999
    public PestilenceStormSpell(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_pestilenceText);
        SpellId = 9;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Death;
    }

    // Storms.pas:1009
    public override Storm CreateStorm() => new PestilenceStorm(null) { Holder = CreationHolder() };

    private static readonly string s_pestilenceText = "Pestilence";
}
