// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class IceStormSpell : StormSpell
{
    // Storms.pas:984
    public IceStormSpell(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameTables.IceStormText);
        SpellId = 4;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Air;
    }

    // Storms.pas:994
    public override Storm CreateStorm() => new IceStorm(null) { Holder = CreationHolder() };
}
