// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class LightningStormSpell : StormSpell
{
    // Storms.pas:1014
    public LightningStormSpell(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameTables.LightningStormText);
        SpellId = 7;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Air;
    }

    // Storms.pas:1024
    public override Storm CreateStorm() => new LightningStorm(null) { Holder = CreationHolder() };
}
