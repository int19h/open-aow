#nullable disable warnings
// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public class IceStormSpell : StormSpell
{
    // Storms.pas:984
    public IceStormSpell(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.IceStormText);
        SpellId = 4;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Air;
    }

    // Storms.pas:994
    public override Storm CreateStorm() => new IceStorm(null) { Holder = CreationHolder() };
}
