// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class FireStormSpell : StormSpell
{
    // Storms.pas:939
    public FireStormSpell(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameTables.FireStormText);
        SpellId = 3;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Fire;
    }

    // Storms.pas:949
    public override Storm CreateStorm() => new FireStorm(null);
}
