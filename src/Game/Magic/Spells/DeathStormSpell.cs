#nullable disable
// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public class DeathStormSpell : StormSpell
{
    // Storms.pas:969
    public DeathStormSpell(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.DeathStormText);
        SpellId = 5;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Death;
    }

    // Storms.pas:979
    public override Storm CreateStorm() => new DeathStorm(null) { Holder = CreationHolder() };
}
