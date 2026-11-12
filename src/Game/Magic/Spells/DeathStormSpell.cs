// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class DeathStormSpell : StormSpell
{
    // Storms.pas:969
    public DeathStormSpell(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameTables.s_deathStormText);
        SpellId = 5;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Death;
    }

    // Storms.pas:979
    public override Storm CreateStorm() => new DeathStorm(null);
}
